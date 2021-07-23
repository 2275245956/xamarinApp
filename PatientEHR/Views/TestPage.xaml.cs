using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PatientEHR.Views
{
    public partial class TestPage : ContentPage
    {        
        public Command LoadItemsCommand { get; }
        public List<DeviceInfoItem> Items { get; }

        public TestPage()
        {
            InitializeComponent();
            Title = "テスト";
            Items = new List<DeviceInfoItem>();
            LoadItemsCommand = new Command( () =>  ExecuteLoadItemsCommand());
            ExecuteLoadItemsCommand();
            BindingContext = this;
        }


        void ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();

                // Device Model (SMG-950U, iPhone10,6)
                var device = DeviceInfo.Model;
                Items.Add(new DeviceInfoItem { Key = "Model", Description = device });
                // Manufacturer (Samsung)
                var manufacturer = DeviceInfo.Manufacturer;
                Items.Add(new DeviceInfoItem { Key = "Manufacturer", Description = manufacturer });

                // Device Name (Motz's iPhone)
                var deviceName = DeviceInfo.Name;
                Items.Add(new DeviceInfoItem { Key = "Name", Description = $"{deviceName}" });

                // Operating System Version Number (7.0)
                var version = DeviceInfo.VersionString;
                Items.Add(new DeviceInfoItem { Key = "VersionString", Description = $"{version}" });

                // Platform (Android)
                var platform = DeviceInfo.Platform;
                Items.Add(new DeviceInfoItem { Key = "Platform", Description = $"{platform}" });

                // Idiom (Phone)
                var idiom = DeviceInfo.Idiom;
                Items.Add(new DeviceInfoItem { Key = "Idiom", Description = $"{idiom}" });

                // Device Type (Physical)
                var deviceType = DeviceInfo.DeviceType;
                Items.Add(new DeviceInfoItem { Key = "DeviceType", Description = $"{deviceType}" });

                var deviceToken = Preferences.Get("deviceToken", "");
                Items.Add(new DeviceInfoItem { Key = "DeviceToken", Description = $"{deviceToken}" });

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

    public class DeviceInfoItem
    {
        public string Key { get; set; }
        public string Description { get; set; }
    }
}
