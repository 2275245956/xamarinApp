//using Android.Widget;
using PatientEHR.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PatientEHR.ViewModels
{
    public class PasswordChangeViewModel : BaseViewModel
    {
        private string oldPassword;
        private string newPassword;
        private string reCheckPassword;
        public Action PasswordChanged;

        public Command SaveChanagedPassword { get; }

        private Page _page { get; set; }
        public string OldPassword { get => oldPassword; set => SetProperty(ref oldPassword, value); }
        public string NewPassword { get => newPassword; set => SetProperty(ref newPassword, value); }
        public string ReCheckPassword { get => reCheckPassword; set => SetProperty(ref reCheckPassword, value); }

        public PasswordChangeViewModel(Page page)
        {
            _page = page;
            SaveChanagedPassword = new Command(OnSaveChange, () =>
            {
                return !(string.IsNullOrWhiteSpace(OldPassword) || string.IsNullOrWhiteSpace(NewPassword) || string.IsNullOrWhiteSpace(ReCheckPassword));
            });
            this.PropertyChanged += (_, __) => SaveChanagedPassword.ChangeCanExecute();
        }

        private async void OnSaveChange()
        {
            var sdk = APISDK.ApiSDK.Instance;
            var res =   CheckPassValidate();
            if (res)
            {
                var pinfo = sdk.CurrentUserInfo;
                var postData = new
                {
                    PatientNo = pinfo.PatientNo,
                    OldPassword = OldPassword,
                    NewPassword = NewPassword
                };
                var str = "";
                try
                {
                    str = sdk.Execute<string>("/api/patientehrinfo/changepassword", postData);
                    //Toast.MakeText(Android.App.Application.Context, str, ToastLength.Long).Show();
                    DependencyService.Get<IToast>().LongAlert(str);
                    //密码修改成功   注销用户 重新登录
                    //Shell.Current.CurrentItem 
                    sdk.CurrentLoginInfo.Password = newPassword;
                    await  Shell.Current.Navigation.PopAsync(true);
                    //await Shell.Current.GoToAsync("LoginPage");
                    //System.Environment.Exit(0);
                }
                catch (Exception e)
                {
                    //Toast.MakeText(Android.App.Application.Context, str, ToastLength.Long).Show();
                    DependencyService.Get<IToast>().LongAlert(str);
                } 
            }
            return;

        }

        private bool  CheckPassValidate()
        {
            var currentPwd = APISDK.ApiSDK.Instance.CurrentLoginInfo.Password;
            var regex = new Regex(@"(?=.*[A-Z])                     
                                     .{8,30}", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);

            if (!regex.IsMatch(NewPassword))
            {
                //Toast.MakeText(Android.App.Application.Context, "8文字以上英大文字1文字以上含む", ToastLength.Long).Show();
                DependencyService.Get<IToast>().LongAlert("8文字以上英大文字1文字以上含む");
                return false;
            }
            else if (!NewPassword.Equals(ReCheckPassword))
            {
                 //Toast.MakeText(Android.App.Application.Context, "两次密码不一致", ToastLength.Long).Show();
                DependencyService.Get<IToast>().LongAlert("两次密码不一致");
                return false;
            }
            else if (!currentPwd.Equals(OldPassword))
            {
                 //Toast.MakeText(Android.App.Application.Context, "原始密码输入错误", ToastLength.Long).Show();
                DependencyService.Get<IToast>().LongAlert("原始密码输入错误");
                return false;
            }
            return true;
        }
    }
}
