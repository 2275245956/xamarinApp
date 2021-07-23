using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(PatientEHR.iOS.DeviceID_Ios))]
namespace PatientEHR.iOS
{
    public class DeviceID_Ios : IDeviceID
    {
        public string GetDeviceId()
        {
            return UIDevice.CurrentDevice.IdentifierForVendor.AsString();
        }
    }
}