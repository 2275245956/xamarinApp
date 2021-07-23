using PatientEHR.APISDK;
using PatientEHR.Models;
using PatientEHR.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace PatientEHR.Controls
{
    public partial class FlyoutHeader : ContentView
    {
     
        public FlyoutHeader()
        {
            InitializeComponent(); 
            this.BindingContext = FlyoutViewModel.Instance; 
        }

    }
}
