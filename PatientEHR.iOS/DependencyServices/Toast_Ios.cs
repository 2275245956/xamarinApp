using System;
using Foundation;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(PatientEHR.iOS.Toast_Ios))]
namespace PatientEHR.iOS
{
    internal class ToastMessage
    {
        public string Message { get; set; }
        public double Seconds { get; set; }
    }

    public class Toast_Ios : IToast
    {
        private static int showing = 0;
        private static System.Collections.Concurrent.ConcurrentQueue<ToastMessage> messages = new System.Collections.Concurrent.ConcurrentQueue<ToastMessage>();

        const double LONG_DELAY = 3.5;
        const double SHORT_DELAY = 2.0;

        private NSTimer alertDelay;
        private UIAlertController alert;

        public void LongAlert(string message)
        {
            Console.WriteLine(showing);
            ShowAlert(message, LONG_DELAY);
        }
        public void ShortAlert(string message)
        {
            Console.WriteLine(showing);
            ShowAlert(message, SHORT_DELAY);
        }

        void ShowAlert(string message, double seconds)
        {
            if (showing == 1)
            {
                messages.Enqueue(new ToastMessage() { Message = message, Seconds = seconds });
                alertDelay = NSTimer.CreateScheduledTimer(0.5, (obj) =>
                {
                    dismissMessage();
                });
                return;
            }

            System.Threading.Interlocked.Increment(ref showing);
            Console.WriteLine(showing);

            alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            alert.AddAction(UIAlertAction.Create("了解", UIAlertActionStyle.Cancel, null));
            alertDelay = NSTimer.CreateScheduledTimer(seconds, (obj) =>
            {
                dismissMessage();
            });

            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }

        void dismissMessage()
        {
            if (alert != null)
            {
                alert.DismissViewController(true, () =>
                {
                    alert.Dispose();
                    alert = null;

                    System.Threading.Interlocked.Decrement(ref showing);
                    Console.WriteLine(showing);
                    if (!messages.IsEmpty)
                    {
                        ToastMessage msg;
                        if (messages.TryDequeue(out msg))
                        {
                            ShowAlert(msg.Message, msg.Seconds);
                        }
                    }

                });
            }
            if (alertDelay != null)
            {
                alertDelay.Dispose();
                alertDelay = null;
            }
        }
    }
}
