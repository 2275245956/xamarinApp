using PatientEHR.Models;
using PatientEHR.Services;
using PatientEHR.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PatientEHR.ViewModels
{
    public class TreamentDetailViewModel : BaseViewModel
    {
        //public Command SaveCommand { get; }
        //public Command DeleteCommand { get; }
        private APISDK.ApiSDK _sdk => APISDK.ApiSDK.Instance;
        private int _currentId => CurrentModel.MedicalHistoryID;
        public Page _page { get; set; }
        public MedicalHistoryTableModel CurrentModel { get; set; }

        private SoapModel soap = new SoapModel();
        private List<SoapFileModel> soapFiles = new List<SoapFileModel>();
        private List<OrderModel> orderList = new List<OrderModel>();
        public SoapModel Soap { get => soap; set => SetProperty(ref soap, value); }
        public List<SoapFileModel> SoapFiles { get => soapFiles; set => SetProperty(ref soapFiles, value); }
        public List<OrderModel> OrderList { get => orderList; set => SetProperty(ref orderList, value); }

        public List<OrderModel> ContactList => OrderList?.FindAll(s => s.OrderDiv == "contact");
        public List<OrderModel> EndoscopeList => OrderList?.FindAll(s => s.OrderDiv == "Endoscope");
        public List<OrderModel> InjectionList => OrderList?.FindAll(s => s.OrderDiv == "Injection");
        public List<OrderModel> LaboratoryTestList => OrderList?.FindAll(s => s.OrderDiv == "LaboratoryTest");
        public List<OrderModel> PhysiologicalTestList => OrderList?.FindAll(s => s.OrderDiv == "PhysiologicalTest");
        public List<OrderModel> PreScriptionList => OrderList?.FindAll(s => s.OrderDiv == "Prescription");
        public List<OrderModel> RadiographyList => OrderList?.FindAll(s => s.OrderDiv == "Radiography");
        public List<OrderModel> SummaryList => OrderList?.FindAll(s => s.OrderDiv == "summary");

        public string DetailTitle => $"{string.Format("{0:yyyy/MM/dd}", CurrentModel.ConsultDate)}  {CurrentModel.OfficeName}  {CurrentModel.SectionName}  {CurrentModel.DrName}";

        private static TreamentDetailViewModel _instance = new TreamentDetailViewModel();

        public static TreamentDetailViewModel Instance => _instance;
        private TreamentDetailViewModel()
        {
            this.PropertyChanged += (_, __) => { };
        }

        public async Task SetDetailData()
        {
            try
            {
                //什么时候获取最新数据    Flag  待修改
                var localdata = await DataStore.GetOrderItemsAsync(_currentId);
                this.OrderList = localdata;
                this.Soap = await DataStore.GetSoapModelAsync(_currentId);
                this.SoapFiles = await DataStore.GetSoapFilesAsync(_currentId);
                //Toast.MakeText("診療記録详细信息 本地数据获取", ToastLength.Long);

                //else
                //{
                //    var pNo =  _sdk.CurrentUserInfo.PatientNo;
                //    var medicalHisId = _currentId;
                //    var url = $"/api/patientehrinfo/GetTreatmentDetail?patientNo={pNo}&MedicalHistoryID={medicalHisId}";
                //    var model = _sdk.Execute<TreamentDetailModel>(url, null);
                //    this.OrderList = new List<OrderModel>();
                //    this.Soap = model.SOAPList.Count > 0 ? model.SOAPList[0] : null;
                //    this.SoapFiles = model.SOAPFiles;
                //    this.OrderList = model.OrderList;
                //    Toast.MakeText("診療記録详细信息 Api数据获取", ToastLength.Long);
                //    await _SaveDataToLocal();
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            this._page.BindingContext = Instance;
        }
        private async Task _SaveDataToLocal()
        {
            //不需要保存上次取数据的最后时间    详细数据根据MedicalHisId来获取   只需要在获取list的地方保存时间即可  此处只需要保存 详细数据
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
                await DataStore.SaveSoapFileAsync(SoapFiles);
            }
        }
    }

    public class TreamentDetailModel
    {
        public List<SoapModel> SOAPList { get; set; }
        public List<SoapFileModel> SOAPFiles { get; set; }
        public List<OrderModel> OrderList { get; set; }
    }
}
