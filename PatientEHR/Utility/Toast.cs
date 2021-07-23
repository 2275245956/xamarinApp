using System;
using Xamarin.Forms;

namespace PatientEHR
{
    internal enum ToastLength
    {
        Short,
        Long
    }

    internal class Toast
    {
        public static void MakeText(string msg, ToastLength length)
        {
            var it = DependencyService.Get<IToast>();
            if (length == ToastLength.Long)
            {
                it?.LongAlert(msg);
            }
            else
            {
                it?.ShortAlert(msg);
            }
        }
    }
}
