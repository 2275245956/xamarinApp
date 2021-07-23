using PatientEHR.Models;
using PatientEHR.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PatientEHR.Views
{
    public partial class TreatmentDetailPage : ContentPage
    {
        public TreatmentDetailPage(MedicalHistoryTableModel obj)
        {
            InitializeComponent();
            var md = TreamentDetailViewModel.Instance;
            md.CurrentModel = obj;
            md._page = this;
            md.SetDetailData();
        }
    }
}
