using PatientEHR.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PatientEHR.ViewModels
{
    public class SettingPageViewModel:BaseViewModel
    {
        public List<SettingItemModel> SettingItems => new List<SettingItemModel> {
         new SettingItemModel
            {
                SettingKey = "password",
                SettingName = "パスワード変更"
            },
          new SettingItemModel
            {
                SettingKey = "manual",
                SettingName = "マニュアル"
            }
        };

        public SettingPageViewModel()
        {
           
        }
       
    }

   
}
