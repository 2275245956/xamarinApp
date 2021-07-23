using PatientEHR.Models;
using PatientEHR.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace PatientEHR.Views
{
    public partial class TreatmentListPage : ContentPage
    {
        public TreatmentListPage()
        {
            InitializeComponent();
            var md = TreamentListViewModel.Instance;
            md.page = this;
            this.BindingContext = md;
        }
 

        private  void trListViewTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as MedicalHistoryTableModel;
            var page = new TreatmentDetailPage(item);
            Shell.Current.Navigation.PushAsync(page);
        }
    }

}
