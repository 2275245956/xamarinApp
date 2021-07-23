using Newtonsoft.Json;
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
    public class LoginViewModel : BaseViewModel
    {
        private string userName;
        private string password;
        private Page _page;
        public Command LoginCommand { get; }

        public static AuthenticationPage _authPage => new AuthenticationPage();
        public LoginViewModel(Page page)
        {
            _page = page;
            LoginCommand = new Command(OnLoginClicked, (obj) =>
            {
                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                {
                    return false;
                }
                return true;
            });
            this.PropertyChanged += (_, __) => LoginCommand.ChangeCanExecute();
        }

        public string UserName { get => userName; set => SetProperty(ref userName, value); }

        public string Password { get => password; set => SetProperty(ref password, value); }

        private async void OnLoginClicked(object obj)
        {
            var info = Preferences.Get("LoginPatientInfo", "");
            //网络未连接
            if (!Common.InternetAvaliable())
            {
                    if (string.IsNullOrEmpty(info)) return;
                    var PatientInfo = JsonConvert.DeserializeObject<PatientInfo>(info);
                    var enPwd = Common.Sha512CodeString($"{PatientInfo.LoginId}{PatientInfo.PatientNo}", Password);
                    if (enPwd.ToLower() == PatientInfo.Password.ToLower())
                    {
                        ApiSDK.Instance.CurrentUserInfo = PatientInfo;
                        Common._SetPageProperty(PatientInfo);
                        Shell.Current.GoToAsync($"//{nameof(TreatmentListPage)}");
                    }
                    else
                    {
                        DependencyService.Get<IToast>().LongAlert("ユーザー名が確認できませんでした。");
                    }
            }
            else {

                //if (!Common._CheckServerOK(true)) return;
                //调用接口
                var loginModel = new loginModel
                {
                    UserName = userName,
                    Password = password
                };
                var res = CheckUserValidate(loginModel);
                if (res)
                {
                    try
                    {
                        ApiSDK.Instance.Login(new ApiLoginInfo { LoginId = loginModel.UserName, Password = loginModel.Password, DeviceId = Common.GetDeviceID() });
                        //await Shell.Current.Navigation.PushModalAsync(_authPage, true);
                        if (string.IsNullOrEmpty(ApiSDK.Instance.CurrentUserInfo.DeviceId))//info为空  未验证过  发送邮件验证
                        {
                            await Shell.Current.Navigation.PushModalAsync(_authPage, true);
                        }
                        else 
                        {
                            _authPage.CheckSucceed();
                        }
                        this.Password = "";
                    }
                    catch (ApiException ex)
                    {
                        await _page.DisplayAlert("", ex.Message, "OK");
                    }
                    catch (Exception ex)
                    {
                        await _page.DisplayAlert("", ex.Message, "OK");
                    }
                }
            }
        }


        /// <summary>
        /// 用户校验
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        private bool CheckUserValidate(loginModel loginModel)
        {
            return true;
        }
    }
}
