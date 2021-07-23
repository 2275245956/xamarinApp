using PatientEHR.APISDK.FileManager;
using PatientEHR.Controls;
using PatientEHR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PatientEHR.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoticeDetailPage : ContentPage
    {
        public NoticeModel notice { get; set; }
        List<string> imgType = new List<string> { "jpg", "jpeg", "png", "bmp", "gif" };
        public NoticeDetailPage(NoticeModel model)
        {
            this.notice = model;
            InitializeComponent();
            _SetPageData();
        }

        private void _SetPageData()
        {
            this.txtContent.Text = notice.ContentTxt;
            this.txtTitle.Text = notice.Title;
            this.txtNoticeDate.Text = string.Format("{0:yyyy/MM/dd}  {1}", notice.NoticeDateTime, notice.UserName);

            _ShowFileDownload();
        }

        private void _ShowFileDownload()
        {
            if (!string.IsNullOrEmpty(notice.DownloadUrls))
            {
                var urls = notice.DownloadUrls.Split(',');
                for (int i = 0; i < urls.Length; i++)
                {
                    var extension = urls[i].Substring(urls[i].LastIndexOf(".") + 1);
                    var fileName = urls[i].Substring(urls[i].LastIndexOf("/") + 1);

                    var file = FileManager.Instance.GetLocalFile(fileName);
                    if (file != null) urls[i] = file.FilePath;

                    if (imgType.Contains(extension))
                    {
                        this.downloadUrls_Layout.Children.Add(new Image { Source = urls[i], WidthRequest = 300 });
                    }
                    //else  文件显示
                    //{
                    //    var lbl = new Label
                    //    {
                    //        FormattedText = new FormattedString()
                    //    };
                    //    Span span = new Span { Text = fileName, TextColor = Color.Blue, TextDecorations = TextDecorations.Underline };
                    //    var spanTapped = new TapGestureRecognizer();
                    //    spanTapped.Tapped += (sender, ev) =>
                    //    {
                    //        //Shell.Current.Navigation.PushAsync(new PDFContentPage(s));
                    //        Browser.OpenAsync(urls[i]);
                    //    };
                    //    span.GestureRecognizers.Add(spanTapped);
                    //    lbl.FormattedText.Spans.Add(span);
                    //    this.downloadUrls_Layout.Children.Add(lbl);
                    //}

                }

            }
        }

        public void AfterDownload(LocalFile file)
        {
        }
    }
}