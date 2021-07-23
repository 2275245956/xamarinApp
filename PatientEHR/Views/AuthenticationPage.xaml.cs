using PatientEHR.Models;
using PatientEHR.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
namespace PatientEHR.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthenticationPage : ContentPage
    {
        private DateTime TickTime { get; set; }

        Timer timer = new Timer();

        private string BtnTxt => "認証コードを再送";
        private int WorkMinutes => 10;
        public AuthenticationPage()
        {
            InitializeComponent();
            var model = new AuthenticationViewModel(this);
            model.SendMailCallBack += SendMailUseGmail;//注册事件
            model.CheckSucceedCallBack += CheckSucceed;
            this.BindingContext = model;
            _initTimer();
            SendMailUseGmail();
        }

        private void _initTimer()
        {
            timer.Interval = 1000;
            timer.Elapsed += ((s, e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var seconds = (int)(TickTime - DateTime.Now).TotalSeconds;
                    if (seconds > 0)
                    {
                        this.sendCodeAgainBtn.Text = $"{BtnTxt} ({seconds}秒後)";
                    }
                    else
                    {
                        this.sendCodeAgainBtn.Text = $"{BtnTxt}";
                        this.sendCodeAgainBtn.IsEnabled = true;
                        timer.Stop();
                    }
                });
            });
        }

        void _TickTime()
        {
            this.sendCodeAgainBtn.IsEnabled = false;
            timer.Stop();
            TickTime = DateTime.Now.AddMinutes(10);
            timer.Start();
        }


        public void SendMailUseGmail()
        {
            _TickTime();
        }
        public async void CheckSucceed()
        {
            //Preferences.Set("tick_time", null);
            //获取患者信息
            var apisdk = APISDK.ApiSDK.Instance;
            var lgm = apisdk.CurrentUserInfo;
            var res = apisdk.Execute<PatientBaseInfoModel>($"/api/patientehrinfo/getpatientinfo?officeId={lgm.OfficeId}&patientNo={lgm.PatientNo}", null);
            if (res != null)
            {
                lgm.PatientKana = res.PatientKana;
                lgm.PatientName = res.PatientName;
                lgm.FaceImage = res.FaceImgDownLoadUrl;
                lgm.Age = res.Age;
                lgm.Password = Common.Sha512CodeString($"{lgm.LoginId}{lgm.PatientNo}", apisdk.CurrentLoginInfo.Password);
            }
            Preferences.Set("LoginPatientInfo", JsonConvert.SerializeObject(lgm));
            Common._SetPageProperty(lgm);
            Shell.Current.GoToAsync($"//{nameof(TreatmentListPage)}");
        }

        private void BackBtnTapped(object sender, EventArgs e)
        {
            timer.Stop();
            Shell.Current.Navigation.PopModalAsync();
        }
    }
}