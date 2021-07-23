using PatientEHR.APISDK.FileManager;
using PatientEHR.Models;
using PatientEHR.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PatientEHR.ViewModels
{
    public class TreamentListViewModel : BaseViewModel
    {
        private List<MedicalHistoryTableModel> list = new List<MedicalHistoryTableModel>();
        private APISDK.ApiSDK _sdk => APISDK.ApiSDK.Instance;
        public int PatientNo => _sdk.CurrentUserInfo.PatientNo;
        public int OfficeId => _sdk.CurrentUserInfo.OfficeId;
        private static TreamentListViewModel _instance = new TreamentListViewModel();
        private string _tableKey => "TreatmentTable";
        private string _getlistUrl => "/api/patientehrinfo/GetTreatmentList?patientNo={0}&userOfficeId={1}&fromTime={2}";
        public List<MedicalHistoryTableModel> trList { get => list; set => SetProperty(ref list, value); }

        public static TreamentListViewModel Instance => _instance;

        public Page page { get; set; }
        private TreamentListViewModel()
        {
            this.PropertyChanged +=async (_, __) => {
                await _CheckDataUpdate();
            };
        }

        private async Task _CheckDataUpdate()
        {
            var md = await DataStore.GetRefreshTimeTableModelAsync(_tableKey);
            if (md == null) return;

            var fromTime =md.LastDataDateTime;
            var dataLs = _sdk.Execute<List<MedicalHistoryTableModel>>(string.Format(_getlistUrl, PatientNo, OfficeId, fromTime), null);

            
            if (dataLs.Count > 0)
            {
                var tempLst = new List<MedicalHistoryTableModel>();
                dataLs.ForEach(s=> {
                    if (trList.Find(t => t.MedicalHistoryID == s.MedicalHistoryID) == null) {
                        tempLst.Add(s);
                    }
                });
                if (tempLst.Count <= 0)
                {
                    Toast.MakeText("最新データ", ToastLength.Short);
                    return;
                }
                var bl = await page.DisplayAlert("情報の更新があります", "更新しますか？", "はい", "いいえ");
                if (bl)
                {
                    trList.AddRange(tempLst);
                    trList = trList.OrderByDescending(s=>s.ConsultDate).ToList();
                    Toast.MakeText("更新が完了しました。", ToastLength.Long);
                    await _SaveDataToLocal(tempLst);
                }
            }
        }

        public async Task _SetListData()
        {

            //判断本地数据库是否有数据  没有从接口获取， 并保存数据的最大值的时间   有数据  查询
            var localData = await DataStore.GetMedicalHistoryTableModelsAsync();
            if (localData != null && localData.Count > 0)
            {
                trList = localData;
                //Toast.MakeText("診療記録 本地数据获取", ToastLength.Long); 
                return;
            }
            else
            {
                var fromTime = DateTime.Parse("2017/1/1").Ticks;//DateTime.Now;
                trList = _sdk.Execute<List<MedicalHistoryTableModel>>(string.Format(_getlistUrl, PatientNo, OfficeId, fromTime), null);
                //Toast.MakeText("診療記録 Api数据获取", ToastLength.Long);
                await _SaveDataToLocal(trList);
            }
        }



        private async Task _SaveDataToLocal(List<MedicalHistoryTableModel> lst)
        {
            if (lst != null && lst.Count > 0)
            {
                var lastConsultDate = lst.Max(s => s.ConsultDate);
                await Common.UpdateRefreshTimeByTableKey(_tableKey, lastConsultDate.Ticks, DataStore);
                //保存本地数据
                await DataStore.SaveMedicalHisAsync(lst);
                //保存detail数据
                await _SaveDetailData(lst);
            }
        }
        //保存detail数据
        private async Task _SaveDetailData(List<MedicalHistoryTableModel> lst)
        {
            var url = "/api/patientehrinfo/GetTreatmentDetail?patientNo={0}&MedicalHistoryID={1}";
            foreach (var item in lst)
            {
                var detailInfo = APISDK.ApiSDK.Instance.Execute<TreamentDetailModel>(string.Format(url, _sdk.CurrentUserInfo.PatientNo, item.MedicalHistoryID), null);

                var Soap = detailInfo.SOAPList.Count > 0 ? detailInfo.SOAPList[0] : null;
                var SoapFiles = detailInfo.SOAPFiles;
                var OrderList = detailInfo.OrderList;
                if (OrderList != null && OrderList.Count > 0)
                {
                    await DataStore.SaveOrderItemsAsync(OrderList);
                }
                if (Soap != null && Soap.MedicalHistoryID > 0)
                {
                    await DataStore.SaveSoapAsync(Soap);
                }
                if (SoapFiles.Count > 0)
                {
                    //保存图片至本地
                    foreach (var file in SoapFiles)
                    {
                        file.MedicalInfoFileID = item.MedicalHistoryID;
                        var fileName = file.url.Substring(file.url.LastIndexOf("=") + 1);
                        var lcFile = FileManager.Instance.GetLocalFile(fileName);
                        if (lcFile != null)
                        {
                            file.url = lcFile.FilePath;
                        }
                        else
                        {
                            await FileManager.Instance.DownloadFileAsync(file.url, fileName, (fl) =>
                            {
                                file.url = fl.FilePath;
                                FileManager.Instance.AddLocalFile(fl);
                            }, (ex) => { });
                        }
                    }
                    await DataStore.SaveSoapFileAsync(SoapFiles);
                }
            }
        }
    }
}
