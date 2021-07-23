using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using UIKit;
using Xamarin.Essentials;

namespace PatientEHR.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            global::Xamarin.Forms.Forms.Init();

            Preferences.Set("deviceToken", $"{UIDevice.CurrentDevice.IdentifierForVendor}");
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            var token = getDeviceTokenStr(deviceToken);
            Preferences.Set("deviceToken", token);
            base.RegisteredForRemoteNotifications(application, deviceToken);
        }

        private string getDeviceTokenStr(NSData deviceToken)
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                StringBuilder builder = new StringBuilder();
                byte[] dataBytes = deviceToken.ToArray();
                foreach (var b in dataBytes)
                {
                    builder.AppendFormat("{0:X2}", b);
                }
                return builder.ToString();
            }
            else
            {
                var token = deviceToken.Description;
                if (!string.IsNullOrWhiteSpace(token))
                {
                    token = token.Trim('<').Trim('>');
                }
                return token;
            }
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            IUIAlertViewDelegate ag = null;
            new UIAlertView("Error registering push notifications", error.LocalizedDescription, ag, "OK", null).Show();
        }

        //private void showErrorAlert(string title, string msg)
        //{
        //    var alertController = UIAlertController.Create(title, msg, UIAlertControllerStyle.Alert);
        //    UIAlertAction cancelAction = UIAlertAction.Create("OK", UIAlertActionStyle.Cancel, null);
        //    alertController.AddAction(cancelAction);
        //    _controller. PresentViewController(alertController, true, null);
        //}

    }
}
