using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Essentials;

namespace PatientEHR.Droid
{
    [Activity(Label = "PatientEHR", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            Android.Telephony.TelephonyManager mTelephonyMgr= (Android.Telephony.TelephonyManager)GetSystemService(TelephonyService);

            ////IMEI number  
            //String m_deviceId = mTelephonyMgr.DeviceId;
            //Preferences.Set("deviceToken", m_deviceId);

            //Android ID  
            String m_androidId = Android.Provider.Settings.Secure.GetString(ContentResolver, Android.Provider.Settings.Secure.AndroidId);
            Preferences.Set("deviceToken", m_androidId);

            //if (string.IsNullOrEmpty(m_deviceId))
            //Preferences.Set("deviceToken", m_androidId);

            ////WLAN MAC Address              
            //Android.Net.Wifi.WifiManager m_wm = (Android.Net.Wifi.WifiManager)GetSystemService(Android.Content.Context.WifiService);
            //String m_wlanMacAdd = m_wm.ConnectionInfo.MacAddress;
            //Preferences.Set("wlanMacAdd", m_wlanMacAdd);

            ////Blue-tooth Address  
            //Android.Bluetooth.BluetoothAdapter m_BluetoothAdapter = Android.Bluetooth.BluetoothAdapter.DefaultAdapter;
            //String m_bluetoothAdd = m_BluetoothAdapter.Address;
            //Preferences.Set("bluetoothAdd", m_bluetoothAdd);


            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}