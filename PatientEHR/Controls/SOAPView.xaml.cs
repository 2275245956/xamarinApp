using PatientEHR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PatientEHR.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SOAPView : ContentView
    {

        public static readonly BindableProperty SoapModelProperty = BindableProperty.Create(nameof(SoapModel), typeof(SoapModel), typeof(SOAPView), new SoapModel(), propertyChanged: async (bindable, oldValue, newValue) =>
        {
            await ((SOAPView)bindable).InitSoapData();
        });
        public SoapModel SoapModel
        {
            get { return (SoapModel)GetValue(SoapModelProperty); }
            set { SetValue(SoapModelProperty, value); }
        }

        public static readonly BindableProperty SoapFileModelProperty = BindableProperty.Create(nameof(SoapFileModel), typeof(List<SoapFileModel>), typeof(SOAPView), new List<SoapFileModel>(), propertyChanged: async (bindable, oldValue, newValue) =>
         {
             await ((SOAPView)bindable).InitSoapFileData();
         });
        public List<SoapFileModel> SoapFileModel
        {
            get { return (List<SoapFileModel>)GetValue(SoapFileModelProperty); }
            set { SetValue(SoapFileModelProperty, value); }
        }
        private async Task InitSoapFileData()
        {
            this.soap_pic.IsVisible = SoapFileModel != null && SoapFileModel.Count > 0;
            if (SoapFileModel != null && SoapFileModel.Count > 0)
            {
                SoapFileModel.ForEach(f =>
                {
                    Soapfile_layout.Children.Add(new Image { Source = f.url, HeightRequest = 50 });
                });
            }
        }

        private async Task InitSoapData()
        {
           
            this.Soap_layout.IsVisible = SoapModel != null;
            if (SoapModel == null) SoapModel=new SoapModel ();

            await _SOAPData();
        }
        private async Task _SOAPData()
        {
            if (SoapModel == null) return;
            this.SOAP_S.Text = SoapModel.S;
            this.SOAP_O.Text = SoapModel.O;
            this.SOAP_A.Text = SoapModel.A;
            this.SOAP_P.Text = SoapModel.P;
            this.SOAP_Note.Text = SoapModel.Memo;
        }

        public SOAPView()
        {
            InitializeComponent();
        }
    }
}