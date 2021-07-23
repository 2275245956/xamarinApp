using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Android.Widget;
using Newtonsoft.Json;
using PatientEHR.APISDK;
using PatientEHR.Models;
using PatientEHR.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PatientEHR.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {

            InitializeComponent();

            var info = Preferences.Get("LoginPatientInfo", "");
            var PatientInfo = JsonConvert.DeserializeObject<PatientInfo>(info);
            var model = new LoginViewModel(this);
            if (info != null && PatientInfo != null)
            {
                model.UserName = PatientInfo.LoginId;
                model.Password = "";
            }
            this.BindingContext = model;
            //服务器是否可用校验
            #if DEBUG
                Common._CheckServerOK();
            #endif
        }
        /// <summary>
        /// 按钮属性改变时 设置按钮的背景色不变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnProChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.LoginBtn.BackgroundColor = Color.FromHex("#689e39");
        }

        private async void SettingClicked(object sender, EventArgs e)
        {
            var pg = new ServerConfigPage();
            pg.SelectCallBack += Pg_SelectCallBack;
            await Shell.Current.Navigation.PushAsync(pg);
        }

        private void Pg_SelectCallBack(object sender, EventArgs e)
        {
            Common._CheckServerOK();
        }

    }
}