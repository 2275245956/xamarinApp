using System;
using System.Collections.Generic;
using PatientEHR.Models;
using PatientEHR.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PatientEHR.Views
{
    public partial class NoticePage : ContentPage
    {

        public NoticePage()
        {
            InitializeComponent();
            this.BindingContext = NoticeViewModel.Instance;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as NoticeModel;
            Shell.Current.Navigation.PushAsync(new NoticeDetailPage(item), true);
        }
    }
}
