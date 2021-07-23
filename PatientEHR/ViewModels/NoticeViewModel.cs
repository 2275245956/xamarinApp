using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using PatientEHR.APISDK;
using PatientEHR.APISDK.FileManager;
using PatientEHR.Models;
using PatientEHR.Views;
using RestSharp;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PatientEHR.ViewModels
{
    public class NoticeViewModel : BaseViewModel
    {
        public List<NoticeModel> ItemList { get=> itemList; set=>SetProperty(ref itemList,value); }
        public List<NoticeModel> itemList=new List<NoticeModel> ();

        private string _tableKey => "NoticeTable";

        private ApiSDK _sdk => ApiSDK.Instance;

        private static NoticeViewModel _Instance = new NoticeViewModel();
        public static NoticeViewModel Instance
        {
            get { return _Instance; }
        }
        private NoticeViewModel()
        {
            this.PropertyChanged +=(_, __) => { };
        }
        public async Task _SetPageData()
        {
            List<NoticeModel> localData=  await DataStore.GetNoticeItemsAsync();
            if (localData.Count > 0)
            {
                ItemList = localData;
                //Toast.MakeText("お知らせ 本地数据获取", ToastLength.Long);
            }
            else
            {
                var url = "/api/patientehrinfo/GetBBSList";
                try
                {
                    var res = _sdk.Execute<NoticeApiModel>(url, null);
                    if (res != null && res.Rows.Count > 0)
                    {
                        ItemList = res.Rows;
                        //Toast.MakeText(" お知らせ Api数据获取", ToastLength.Long);
                        await _SaveDataToLocal();
                    }
                }
                catch (Exception e)
                {
                    ItemList = new List<NoticeModel>();
                    //throw;
                }
            }
            await _SaveFileToLocal();
        }

        private async Task _SaveDataToLocal()
        {
          
            if (ItemList != null && ItemList.Count > 0)
            {
                await Common.UpdateRefreshTimeByTableKey(_tableKey, ItemList[0].NoticeDateTime.Ticks, DataStore);
                //保存本地数据
                await DataStore.AddNewNoticeItemsAsync(ItemList);
            }
        }
        private async Task _SaveFileToLocal() {
            foreach (var item in ItemList)
            {
                if (string.IsNullOrEmpty(item.DownloadUrls)) continue;

                var urls = item.DownloadUrls.Split(',');
                for (int i = 0; i < urls.Length; i++)
                {
                    var extension = urls[i].Substring(urls[i].LastIndexOf(".") + 1);
                    var fileName = urls[i].Substring(urls[i].LastIndexOf("/") + 1);
                    //根据filename 先从本地获取file  
                    var file = FileManager.Instance.GetLocalFile(fileName);

                    if (file == null)
                    {
                        FileManager.Instance.DownloadFileAsync(urls[i],
                                                               fileName,
                                                               (f) =>
                                                               {
                                                                   urls[i] = f.FilePath;
                                                                   FileManager.Instance.AddLocalFile(f);
                                                               },
                                                               (ex) => { }
                         ).GetAwaiter();
                    }
                }
            }
        }
    }
    public class NoticeApiModel
    {
        public int Total { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public List<NoticeModel> Rows { get; set; }
    }

    public class NoticeParam
    {
        public int UserRole { get; set; }
        public int OfficeID { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; } = 10;
    }
}
