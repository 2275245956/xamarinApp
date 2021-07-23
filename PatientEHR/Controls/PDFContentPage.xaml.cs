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
    public partial class PDFContentPage : ContentPage
    {
        public PDFContentPage(string webViewUrl)
        {
            InitializeComponent();
            this.webViewUrl.Source = webViewUrl;
        }
    }
}