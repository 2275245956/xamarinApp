//using Android.OS;
using PatientEHR.APISDK;
using PatientEHR.Models;
using PatientEHR.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PatientEHR.ViewModels
{
    public class AuthenticationViewModel : BaseViewModel
    {
        public event Action SendMailCallBack;
        public event Action CheckSucceedCallBack;

        private string checkCode;

        private Page _page;
        public Command CheckCommand { get; }
        public Command SendAgainCommand { get; }
        public string CheckCode { get => checkCode; set => SetProperty(ref checkCode, value); }

        public AuthenticationViewModel(Page page)
        {
            _page = page;
            CheckCommand = new Command(async () => await OnCheckTapped(), () =>
            {
                return !string.IsNullOrWhiteSpace(CheckCode);
            });
            SendAgainCommand = new Command(async () => await OnSendAgain());
            this.PropertyChanged += (_, __) =>
            {
                CheckCommand.ChangeCanExecute();
            };
        }


        async Task OnSendAgain()
        {
            var lgModel = ApiSDK.Instance.CurrentLoginInfo;
            try
            {
                ApiSDK.Instance.Login(lgModel);

                SendMailCallBack();
            }
            catch (ApiException ex)
            {
                await _page.DisplayAlert("", ex.Message, "OK");
                return;
            }
            catch (Exception ex)
            {
                await _page.DisplayAlert("", ex.Message, "OK");
                return;
            }
        }


        async Task OnCheckTapped()
        {

            //调用接口获取code 
            var lgModel = APISDK.ApiSDK.Instance.CurrentUserInfo;
            var postData = new { Mail = lgModel.Mail, ValidCode = CheckCode, DeviceId = Common.GetDeviceID() };
            try
            {
                var res = APISDK.ApiSDK.Instance.Execute<PatientInfo>("/api/patientehr/validcode", postData);
                if (res != null)
                {
                    APISDK.ApiSDK.Instance.CurrentUserInfo = res;
                    await Shell.Current.Navigation.PopModalAsync();
                    CheckSucceedCallBack();
                }
                else
                {
                  await  _page.DisplayAlert("", "認証コードのチェックが失敗しました！", "OK");
                }
            }
            catch (Exception e)
            {
                await _page.DisplayAlert("", e.Message, "OK");
                CheckCode = "";
            }

        }




    }
}
