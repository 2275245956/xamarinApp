using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using PatientEHR.Models;
using RestSharp;

namespace PatientEHR.APISDK
{
    //class ApiToken
    //{
    //    private ApiToken() { }
    //    private static ApiToken _instance = new ApiToken();
    //    public static ApiToken Instance
    //    {
    //        get
    //        {
    //            return _instance;
    //        }
    //    }

    //    public string Token { get; private set; }

    //    public void SetToken(string token)
    //    {
    //        this.Token = token;
    //    }
    //}


    class ApiLoginInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string LoginId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DeviceId { get; set; }
    }


    class ApiResponseMessage<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T MessageValue { get; set; }
    }


    class ApiResponse
    {
        public string ResponseHTML { get; set; }
        public bool IsSuccess { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }

    class NullObject
    {

    }

    class DefaultResponseMessage : ApiResponseMessage<object> { }

    class ApiSDK
    {

        private ApiSDK() { }
        private static ApiSDK _instance = new ApiSDK();
        public static ApiSDK Instance
        {
            get
            {
                return _instance;
            }
        }


        public int PatientNo => CurrentUserInfo.PatientNo;
        public int CurrentOfficeID => CurrentUserInfo.OfficeId;
        public PatientInfo CurrentUserInfo { get;  set; }
        public ApiLoginInfo CurrentLoginInfo { get; private set; }
        private string _apiUrl;
        public string RootUrl { get; private set; }
        public string AppKey { get; set; }
        public string APiUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(_apiUrl) && _apiUrl.Length > 1 && _apiUrl.EndsWith("/"))
                {
                    return _apiUrl.Substring(0, _apiUrl.Length - 1);
                }
                return _apiUrl;
            }
            set
            {
                _apiUrl = value;
            }
        }



        public void ClearCurrentUserInfo()
        {
            CurrentUserInfo = null;
        }

        public void SetHostUrl(string hostUrl)
        {
            _apiUrl = hostUrl;
            try
            {
                var url = new System.Uri(hostUrl);
                RootUrl = $"{url.Scheme}://{url.Host}";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public bool UrlCheck()
        {
            //string url = string.Format("{0}/api/About/ver", APiUrl);
            //using (var client = new XHttpRequestV2(string.Empty))
            //{
            //    var ar = client.GetData(url);
            //    if (ar != null)
            //    {
            //        var result = JsonConvert.DeserializeObject<ApiResponseMessage<AboutVer>>(ar);
            //        if (result.Code == 0)
            //        {
            //            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"(\d+[\.]?)+");
            //            return reg.IsMatch(result.MessageValue.Ver);
            //        }
            //    }
            //}
            //return false;
            return true;
        }

        public PatientInfo Login(ApiLoginInfo loginInfo)
        {
            string url = string.Format("{0}/api/patientehr/login", APiUrl);//?userName={1}&password={2}", APiUrl, txtLoginId.Text, txtPassword.Text);
            using (XHttpRequestV2 client = new XHttpRequestV2(string.Empty))
            {
                var ar = client.Post(url, loginInfo);
                if (ar != null)
                {
                    var result = JsonConvert.DeserializeObject<ApiResponseMessage<PatientInfo>>(ar.ResponseHTML);
                    if (result.Code == 0)
                    {
                        this.CurrentUserInfo = result.MessageValue;
                        CurrentLoginInfo = loginInfo;
                        return result.MessageValue;
                    }
                    else
                    {
                        throw new ApiException(result.Message);
                    }
                }
            }
            return null;
        }

        public T Execute<T>(string apiPath, object inPam)
        {
            //UpdateGID();
            string url = $"{APiUrl}{apiPath}"; //?userName={1}&password={2}", APiUrl, txtLoginId.Text, txtPassword.Text);
            using (XHttpRequestV2 client = new XHttpRequestV2(CurrentUserInfo?.Token))
            {
                Console.WriteLine(url);
                var ar = client.Post(url, inPam);
                if (ar != null)
                {
                    try
                    {
                        var result = JsonConvert.DeserializeObject<ApiResponseMessage<T>>(ar.ResponseHTML);
                        if (result.Code == 0)
                        {
                            return result.MessageValue;
                        }
                        else
                        {
                            throw new ApiException(result.Message);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        var result = JsonConvert.DeserializeObject<DefaultResponseMessage>(ar.ResponseHTML);
                        if (result.Code != 0)
                            throw new ApiException(result.Message);
                        throw ex;
                    }
                }
            }
            throw new NullReferenceException("post result is null");
        }


        public bool LogOut()
        {
            if (string.IsNullOrEmpty(CurrentUserInfo?.Token))
            {
                return true;
            }
            string url = string.Format("{0}/api/user/SignOut/", APiUrl);
            using (XHttpRequestV2 client = new XHttpRequestV2(CurrentUserInfo?.Token))
            {
                var ar = client.Post(url, CurrentUserInfo?.Token);
                CurrentUserInfo = null;
                return true;// ar.IsSuccess;
            }
        } 
    }

    class XHttpRequestV2 : IDisposable
    {
        private string token = string.Empty;
        //private int timeout = 30;//30秒
        public XHttpRequestV2(string token)
        {
            this.token = token;
        }

        public string GetData(string url)
        {
            RestClient client = new RestClient();
            RestRequest request = new RestRequest(url, Method.GET, DataFormat.Json);
            var resp = client.Execute(request);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                return resp.Content;
            }
            else
            {
                throw resp.ErrorException;
            }
        }

        public ApiResponse Post(string url, object obj)
        {
            RestClient client = new RestClient();
            RestRequest request = new RestRequest(url, Method.POST, DataFormat.Json);
            if (!string.IsNullOrEmpty(token))
            {
                request.AddHeader("token", token);
            }
            request.AddJsonBody(obj);
            var resp = client.Execute(request);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                var ar = new ApiResponse()
                {
                    ResponseHTML = resp.Content,
                    IsSuccess = true,
                    StatusCode = resp.StatusCode
                };
                return ar;
            }
            else
            {
                throw resp.ErrorException;
            }
        }

        public ApiResponse Post(string url, byte[] byteArray)
        {
            try
            {
                //var byteArray = getByteArray(postObj);
                HttpWebRequest req = (HttpWebRequest)System.Net.WebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/json";
                req.ContentLength = byteArray.Length;
                //req.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                if (!string.IsNullOrEmpty(token))
                {
                    req.Headers.Add("token", token);
                }
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(byteArray, 0, byteArray.Length);
                }

                using (HttpWebResponse wr = (HttpWebResponse)req.GetResponse())
                {
                    var ar = new ApiResponse()
                    {
                        ResponseHTML = null,
                        IsSuccess = true,
                        StatusCode = wr.StatusCode
                    };
                    //在这里对接收到的页面内容进行处理
                    using (var stream = wr.GetResponseStream())
                    {
                        var streamReader = new StreamReader(stream, Encoding.UTF8);
                        ar.ResponseHTML = streamReader.ReadToEnd();
                    }
                    return ar;
                }
            }
            catch (WebException webEx)
            {
                if (webEx.Response != null)
                {
                    using (var stream = webEx.Response.GetResponseStream())
                    {
                        var sr = new StreamReader(stream, System.Text.Encoding.UTF8);
                        var html = sr.ReadToEnd();
                    }
                }
                throw webEx;
                //throw new Exception("WebException");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static byte[] getByteArray2(object postObj)
        {
            if (postObj == null) return new byte[0];
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(postObj);
            Console.WriteLine(json);
            return System.Text.Encoding.UTF8.GetBytes(json);
        }

        public void Dispose()
        {

        }
    }
}
