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
    public partial class ContactView : ContentView
    {
        public static readonly BindableProperty OrderModelProperty = BindableProperty.Create(nameof(OrderModel), typeof(List<OrderModel>), typeof(ContactView), new List<OrderModel>(), propertyChanged: (bindable, oldValue, newValue) =>
        {
            ((ContactView)bindable).InitializeView();
        });
        public List<OrderModel> OrderModel
        {
            get { return (List<OrderModel>)GetValue(OrderModelProperty); }
            set { SetValue(OrderModelProperty, value); }
        }

        private void InitializeView()
        {
            if (OrderModel.Count <= 0) return;

        }
         
        public ContactView()
        {
            InitializeComponent();
        }
    }
}