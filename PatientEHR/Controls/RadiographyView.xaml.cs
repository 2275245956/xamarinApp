using PatientEHR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PatientEHR.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RadiographyView : ContentView
    {
        public static readonly BindableProperty OrderModelProperty = BindableProperty.Create(nameof(OrderModel), typeof(List<OrderModel>), typeof(RadiographyView), new List<OrderModel>(), propertyChanged: (bindable, oldValue, newValue) =>
        {
            ((RadiographyView)bindable).InitializeView();
        });
        public List<OrderModel> OrderModel
        {
            get { return (List<OrderModel>)GetValue(OrderModelProperty); }
            set { SetValue(OrderModelProperty, value); }
        }

        private void InitializeView()
        {
            Order_layout.Children.Clear();

            if (OrderModel.Count <= 0) return;
            var screenWidth = DeviceDisplay.MainDisplayInfo.Width;
            var leftTitleWidth = 100;
            var centerTextWidth = screenWidth - leftTitleWidth;
            var stOutSide = new StackLayout { Orientation = StackOrientation.Horizontal };
            var leftLabel = new Label()
            {
                Text = "画像",
                WidthRequest = leftTitleWidth,
                FontSize = 16,
                VerticalOptions = LayoutOptions.FillAndExpand,
                VerticalTextAlignment = TextAlignment.Start
            };
            stOutSide.Children.Add(leftLabel);

            var st = new StackLayout { Orientation = StackOrientation.Vertical };
            OrderModel.ForEach(o =>
            {
                st.Children.Add(new Label { Text = o.OrderName, FontSize = 16, WidthRequest = centerTextWidth });
                if (!string.IsNullOrEmpty(o.OrderNote)) {
                    var stNote = new StackLayout() { Orientation = StackOrientation.Horizontal };
                    stNote.Children.Add(new Label { Text = $"∟…{o.OrderNote} ", FontSize = 14});
                    st.Children.Add(stNote);
                }
            });
            stOutSide.Children.Add(st);
            Order_layout.Children.Add(stOutSide);
        }
        public OrderModel DateSource { get; set; }
        public RadiographyView()
        {
            InitializeComponent();
        }
    }
}