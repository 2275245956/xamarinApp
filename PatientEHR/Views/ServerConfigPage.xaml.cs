using Newtonsoft.Json;
using PatientEHR.APISDK;
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
    public partial class ServerConfigPage : ContentPage
    {
        public event EventHandler SelectCallBack;
        public List<ServerPathModel> Servers { get; set; }

        public ServerConfigPage()
        {
            InitializeComponent();
            _InitServers();

            this.BindingContext = this;
        }

        private void _InitServers()
        {
            Servers = new List<ServerPathModel>() {
                new ServerPathModel {
                    ServerPath=@"http://pasysaz.japaneast.cloudapp.azure.com/EHR2",
                    ServerName="Azure"
                },
                new ServerPathModel {
                    ServerPath=@"http://192.168.3.74/EHR2",
                    ServerName="默认地址"
                },
                new ServerPathModel{
                    ServerPath="http://192.168.3.18:8886",
                    ServerName="本机地址"
                }
            };
            var defaultUrl = Servers.Find(s => s.ServerPath == Common.defaultServer?.ServerPath);
            servicesList.SelectedItem = defaultUrl ?? Servers[0];
        }

        private  void OnServerSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var ser = e.SelectedItem as ServerPathModel;
            ApiSDK.Instance.APiUrl = ser.ServerPath;
            var apiModel = JsonConvert.SerializeObject(ser);
            Preferences.Set("DefaultApiServer", apiModel);
            SelectCallBack?.Invoke(this, EventArgs.Empty);
            //await Navigation.PopAsync();
        }
    }



}