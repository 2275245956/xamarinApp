using Android.OS;
using System;
using Xamarin.Forms;
using static Android.Provider.Settings;

[assembly: Dependency(typeof(PatientEHR.Droid.DeviceId_Android))]
namespace PatientEHR.Droid
{
    public class DeviceId_Android : IDeviceID
    {
        public string GetDeviceId()
        {
            string id = string.Empty;

            if (!string.IsNullOrWhiteSpace(id))
                return id;
            if (string.IsNullOrWhiteSpace(id) || id == Build.Unknown || id == "0")
            {
                try
                {
                    var context = Android.App.Application.Context;
                    id = Secure.GetString(context.ContentResolver, Secure.AndroidId);
                }
                catch (Exception ex)
                {
                    Android.Util.Log.Warn("DeviceInfo", "Unable to get id: " + ex.ToString());
                }
            }

            return id;
        }
    }
}