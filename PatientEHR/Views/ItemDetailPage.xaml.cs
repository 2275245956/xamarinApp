using System.ComponentModel;
using Xamarin.Forms;
using PatientEHR.ViewModels;

namespace PatientEHR.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}