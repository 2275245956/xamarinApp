using PatientEHR.Models;
using PatientEHR.ViewModels;
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PatientEHR.Views
{
    public partial class SettingPage : ContentPage
    {

        public SettingPage()
        {
            InitializeComponent();
            this.BindingContext = new SettingPageViewModel();
        }

       
        private async void TrListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as SettingItemModel;
            switch (item.SettingKey.ToLower())
            {
                case "password":
                    //this.DisplayAlert("",item.SettingKey,"Ok");
                    await Shell.Current.Navigation.PushAsync(new PasswordPageChange());
                    break;
                case "manual":
                    await this.DisplayAlert("", item.SettingKey, "Ok");
                    break;
            }
        }
    }
}
