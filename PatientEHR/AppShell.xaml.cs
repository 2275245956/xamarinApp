using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
//using Android.OS;
using PatientEHR.APISDK.FileManager;
using PatientEHR.ViewModels;
using PatientEHR.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PatientEHR
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public Command OpenWebCommand { get; }
        public Command LogoutCommand { get; }

        public AppShell()
        {
            InitializeComponent();
            //Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));


            OpenWebCommand = new Command(async () => await ExecuteOpenWebCommand());
            LogoutCommand = new Command(async () => await ExecuteLogoutCommand());
            //await Shell.Current.GoToAsync("//LoginPage");

            this.BindingContext = this;
        }

        async Task ExecuteOpenWebCommand()
        {
            Shell.Current.FlyoutIsPresented = false;
            await Browser.OpenAsync("https://aka.ms/xamarin-quickstart");
        }

        async Task ExecuteLogoutCommand()
        {
            Shell.Current.FlyoutIsPresented = false;
            var bl = await DisplayAlert("", "ログアウトします、よろしいですか？", "はい", "いいえ");
            if (bl)
            {
                //APISDK.ApiSDK.Instance.LogOut();
                //await Shell.Current.GoToAsync("//LoginPage");
                //await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                //Preferences.Remove("LoginPatientInfo");//清除登录信息

                //switch (Device.RuntimePlatform)
                //{
                //    case Device.Android:
                //        Process.KillProcess(Android.OS.Process.MyPid());
                //        break;
                //    case Device.iOS:
                //        Thread.CurrentThread.Abort();
                //        break;
                //}
                FileManager.Instance.Close();
                Shell.Current.CurrentItem = loginItem;
            }
        }
    }
}
