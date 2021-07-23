using PatientEHR.Models;
using PatientEHR.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PatientEHR.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PasswordPageChange : ContentPage
    {
        public PasswordPageChange()
        {
            InitializeComponent();
            this.BindingContext = new PasswordChangeViewModel(this);

            var state = Common.InternetAvaliable();
            if (!state) {
                Toast.MakeText("ネットワークが利用できません！", ToastLength.Long);
                this.oldPwd.IsReadOnly = true;
                this.newPwd.IsReadOnly = true;
                this.chkPwd.IsReadOnly = true;
                this.btnChangePwd.IsEnabled = false;
            }

        }
    }
}