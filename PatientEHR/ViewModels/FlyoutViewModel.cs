using PatientEHR.APISDK;
using PatientEHR.APISDK.FileManager;
using PatientEHR.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PatientEHR.ViewModels
{
    public class FlyoutViewModel : BaseViewModel
    {
        private string patientName;
        private string patientKana;

        //private string faceImg;
        public string PatientName { get => patientName; set => SetProperty(ref patientName, value); }
        public string PatientKana { get => patientKana; set => SetProperty(ref patientKana, value); }
        public string FaceImg => "faceImg.png";//{ get => faceImg; set => SetProperty(ref faceImg, value); }


        private FlyoutViewModel()
        {
            this.PropertyChanged += (_, __) => { };
        }

        private static FlyoutViewModel _Instance = new FlyoutViewModel();
        public static FlyoutViewModel Instance
        {
            get { return _Instance; }
        }

        public async Task SetPageProperty(PatientInfo lgm)
        {
            //var fileName = $"PatientFaceImg_{lgm.PatientNo}.jpg";
            PatientName = lgm.PatientName;
            //if (Common.InternetAvaliable())
            //{
            //    await FileDbStrore.DownloadFileAsync(lgm.FaceImage, fileName, AfterDownload, OnError);
            //}
            //else//未连接网络  从本地获取
            //{
            //    var file = FileDbStrore.GetLocalFile(fileName);
            //    file.IsLocal = true;
            //    AfterDownload(file);
            //}

        }

        private void AfterDownload(LocalFile file)
        {
            if (string.IsNullOrEmpty(file.FilePath)) return;
            if (!file.IsLocal)//本地获取的图片不需要保存
            { 
                FileDbStrore.AddLocalFile(file);
            }
            //FaceImg = file.FilePath;

        }
        private void OnError(Exception e)
        {
            Console.WriteLine(e.Message);
        }


    }
}
