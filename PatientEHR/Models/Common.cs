//using Android.Widget;
using Newtonsoft.Json;
using PatientEHR.APISDK;
using PatientEHR.Services;
using PatientEHR.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PatientEHR.Models
{
    public partial class Common
    {
        const string numchars = "0123456789";
        const string chars = "ABCDEFGHIJKLMNOPQRSTUWVXYZ0123456789abcdefghijklmnopqrstuvwxyz";
        /// <summary>
        /// get current default api server by Preferences
        /// </summary>
        public static ServerPathModel defaultServer
        {
            get
            {
                if (Preferences.Get("DefaultApiServer", null) == null)
                {
                    return null;
                }
                return JsonConvert.DeserializeObject<ServerPathModel>(Preferences.Get("DefaultApiServer", "{}"));

            }
        }

        public static string ImageFolder
        {
            get
            {
                string baseUrl = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                if (Device.RuntimePlatform == Device.iOS)
                {
                    baseUrl = System.IO.Path.Combine(baseUrl, "..", "Library");
                }
                return baseUrl;
            }
        }

        /// <summary>
        /// Check whether the api service is available
        /// </summary>
        /// <param name="justShowError">jush show error message   default:false </param>
        /// <returns>true:available  false:unavailable</returns>
        public static bool _CheckServerOK(bool justShowError = false)
        {

            if (!justShowError)
            {
                DependencyService.Get<IToast>().LongAlert("ホストの接続ガ試しています...");
            }
            var connected = false;
            var service = defaultServer;
            if (service == null)
            {
                DependencyService.Get<IToast>().LongAlert("ホスト名を確認下さい");
            }
            else
            {
                ApiSDK.Instance.APiUrl = service.ServerPath;
                try
                {
                    connected = ApiSDK.Instance.UrlCheck();
                }
                catch (Exception)
                {
                    connected = false;
                }
                var serverMsg = connected ? $"ホスト接続しました" : $"ホスト名を確認下さい";
                if (justShowError)
                {
                    if (!connected)
                    {
                        Toast.MakeText(serverMsg, ToastLength.Long);
                    }

                }
                else if (!justShowError)
                {
                    Toast.MakeText(serverMsg, ToastLength.Long);
                }
                else { }
            }
            return connected;
        }

        public static async Task<bool> UpdateRefreshTimeByTableKey(string tbKey, long tineTicks, ServiceInstance DataStore)
        {
            var md = await DataStore.GetRefreshTimeTableModelAsync(tbKey);
            //更新最新获取数据时间
            if (md == null || md.Id <= 0)
            {
                md = new RefreshTimeTable { TableKey = tbKey, LastDataDateTime = tineTicks };
            }
            var res = await DataStore.SaveRefreshTableItemAsync(md);
            return res > 0;
        }

        public static string RandomStr(int len)
        {
            if (len <= 0) return "";
            var rd = new System.Random(DateTime.Now.Millisecond);
            string strs = string.Empty;
            for (int i = 0; i < len; i++)
            {
                strs += chars[rd.Next(chars.Length)];
            }
            return strs;
        }
        public static string RandomNumStr(int len)
        {
            if (len <= 0) return "";
            var rd = new System.Random(DateTime.Now.Millisecond);
            string strs = string.Empty;
            for (int i = 0; i < len; i++)
            {
                strs += numchars[rd.Next(numchars.Length)];
            }
            return strs;
        }
        /// <summary>
        /// 检测网络是否正常
        /// </summary>
        /// <returns></returns>
        public static bool InternetAvaliable()
        {
            var profiles = Connectivity.ConnectionProfiles;
            var access = Connectivity.NetworkAccess;
            if (access != NetworkAccess.Internet)
            {
                //网络不可访问
                return false;
            }
            if (profiles.Contains(ConnectionProfile.WiFi) || profiles.Contains(ConnectionProfile.Cellular))
            {
                //无线网络 和手机流量
                return true;
            }
            return false;

        }


        public static async Task _SetPageProperty(PatientInfo lgm)
        {
            await FlyoutViewModel.Instance.SetPageProperty(lgm);
            await NoticeViewModel.Instance._SetPageData();
            await TreamentListViewModel.Instance._SetListData();
            await PatientInfoPageViewModel.Instance.GetBaseInfo();
        }

        public static async Task SaveImageFileToLocal(string filePath, string localFilePath, string fileName) {
            if (Device.RuntimePlatform == Device.Android)
            {
                var imageData = System.IO.File.ReadAllBytes(filePath);
                var newFilepath = System.IO.Path.Combine(localFilePath, fileName);

                System.IO.File.WriteAllBytes(newFilepath, imageData);
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {

            }
            else {
            }
        }

        public static  string Sha512CodeString(string userCode,string PassWord) {
            //设userCode为密钥（盐值）
            string salt = userCode;
            byte[] saltpasswordvalue = UTF8Encoding.UTF8.GetBytes(salt + PassWord);
            //计算哈希值，进行sha-512加密
            SHA512 sha512 = SHA512.Create();
            saltpasswordvalue = sha512.ComputeHash(saltpasswordvalue);
            //因为上面计算了一次hash，所以只需要迭代1023次
            for (int i = 0; i < 1023; i++)
            {
                saltpasswordvalue = sha512.ComputeHash(saltpasswordvalue);
            }
            //字节数组转换为16进制
            var res = "";
            if (saltpasswordvalue != null)
            {
                for (int i = 0; i < saltpasswordvalue.Length; i++)
                {
                    res += saltpasswordvalue[i].ToString("X2");
                }
            }
            return res.ToLower();
        }

        public static string GetDeviceID() {
            var deviceId = DependencyService.Get<IDeviceID>().GetDeviceId();
            return deviceId;
        }
    }

    public class ServerPathModel
    {
        public string ServerPath { get; set; }
        public string ServerName { get; set; }

    }
}
