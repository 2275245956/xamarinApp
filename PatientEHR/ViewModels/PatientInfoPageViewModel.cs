//using Android.Widget;
using PatientEHR.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PatientEHR.ViewModels
{
    public class PatientInfoPageViewModel : BaseViewModel
    {
        public Func<Dictionary<string, List<BaseInfoModel>>, bool> SetDataCallBack;
        private APISDK.ApiSDK _sdk = APISDK.ApiSDK.Instance;
        private string tableKey => "PatientAtttribute";

        private static PatientInfoPageViewModel _instance = new PatientInfoPageViewModel();

        private Dictionary<string, List<BaseInfoModel>> pageDic;
        public Dictionary<string, List<BaseInfoModel>> PageDic { get => pageDic; set => SetProperty(ref pageDic, value); }
        public static PatientInfoPageViewModel Instance => _instance;
        private PatientInfoPageViewModel()
        {
            this.PropertyChanged += (_, __) => { };
        }

        public async Task GetBaseInfo()
        {
            PageDic = new Dictionary<string, List<BaseInfoModel>>();
            var localData = await DataStore.GetPatientAttributeInfoItemsAsync();
            if (localData != null && localData.Count > 0)
            {
                _SetData(localData, null);
                return;
            }
            else
            {
                var pNo = _sdk.CurrentUserInfo.PatientNo;
                try
                {
                    var res = _sdk.Execute<PatientResponseModel>($"/api/patientehrinfo/getpatientbaseinfo?patientNo={pNo}", null);

                    if (res != null)
                    {
                        _SetData(res.MedicalPart, res.FreePart);
                        if (res.MedicalPart.Count > 0)
                        {
                            await Common.UpdateRefreshTimeByTableKey(tableKey, res.MedicalPart[0].LastUpdateTime.Value.Ticks, DataStore);
                            await DataStore.SavePatientAttributeInfoAsync(res.MedicalPart);
                            await DataStore.SavePatientAttributeInfoAsync(res.FreePart);
                        }
                    }
                }
                catch (Exception e)
                {
                    //Toast.MakeText(Android.App.Application.Context, e.Message, ToastLength.Long).Show();
                    DependencyService.Get<IToast>().LongAlert(e.Message);
                }
            }
        }

        private void _SetData(List<BaseInfoModel> res, List<BaseInfoModel> free)
        {
            if (res != null && res.Count > 0)
            {
                res.ForEach(s =>
                {
                    if (!PageDic.ContainsKey(s.AttributeItemGroupName))
                    {
                        PageDic[s.AttributeItemGroupName] = new List<BaseInfoModel>();
                    }
                    PageDic[s.AttributeItemGroupName].Add(s);
                });
            }
            if (free != null && free.Count > 0)
            {
                free.ForEach(f =>
                {
                    f.AttributeItemGroupName = f.AttributeItemName;
                    if (!PageDic.ContainsKey(f.AttributeItemGroupName))
                    {
                        PageDic[f.AttributeItemGroupName] = new List<BaseInfoModel>();
                    }
                    PageDic[f.AttributeItemGroupName].Add(f);
                });
            }



            SetDataCallBack?.Invoke(PageDic);
        }

    }

    public class PatientResponseModel
    {
        public List<BaseInfoModel> MedicalPart { get; set; }
        public List<BaseInfoModel> FreePart { get; set; }

    }
}
