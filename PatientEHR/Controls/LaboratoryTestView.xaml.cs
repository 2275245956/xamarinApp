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
    public partial class LaboratoryTestView : ContentView
    {
        public static readonly BindableProperty OrderModelProperty = BindableProperty.Create(nameof(OrderModel), typeof(List<OrderModel>), typeof(LaboratoryTestView), new List<OrderModel>(), propertyChanged: async (bindable, oldValue, newValue) =>
         {
             await ((LaboratoryTestView)bindable).InitializeView();
         });
        public List<OrderModel> OrderModel
        {
            get { return (List<OrderModel>)GetValue(OrderModelProperty); }
            set { SetValue(OrderModelProperty, value); }
        }
        private async Task InitializeView()
        {

            Order_layout.Children.Clear();

            if (OrderModel.Count <= 0) return;
            var screenWidth = DeviceDisplay.MainDisplayInfo.Width;
            var leftTitleWidth = 100;
            var centerTextWidth = screenWidth - leftTitleWidth;
            var stOutSide = new StackLayout { Orientation = StackOrientation.Horizontal };
            var leftLabel = new Label()
            {
                Text = "検査結果",
                WidthRequest = leftTitleWidth,
                FontSize = 16,
                VerticalOptions = LayoutOptions.FillAndExpand,
                VerticalTextAlignment = TextAlignment.Start
            };
            stOutSide.Children.Add(leftLabel);

            var stRight = new StackLayout() { Orientation=StackOrientation.Vertical,HorizontalOptions=LayoutOptions.FillAndExpand,Spacing=0,WidthRequest=350};

            await _SetGridHeader(stRight);

            var gridPadding = new Thickness(3, 3, 0, 3);
            var offsetHeight = 18;
            OrderModel.ForEach(o =>
            {
                var ht = o.OrderName.Length % 6 > 0 ? o.OrderName.Length / 6 + 1 : o.OrderName.Length/6;
                var h = (ht - 1) * offsetHeight < 0?0: (ht - 1) * offsetHeight;
                var stRow = new StackLayout { BackgroundColor = Color.Gray, Spacing = 0, Padding = new Thickness(1,0,1,1),VerticalOptions = LayoutOptions.FillAndExpand, HeightRequest = 30+ h };
                var flowLayOut = new FlexLayout() { Direction = FlexDirection.Row,VerticalOptions=LayoutOptions.FillAndExpand };
                var label1 = new Label { Text = $"{o.OrderName}", FontSize = 14, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, MaxLines = 5,BackgroundColor=Color.White, Padding = gridPadding };
                FlexLayout.SetBasis(label1, new FlexBasis(0.40f, true));
                flowLayOut.Children.Add(label1);

                var cellSt = new StackLayout() { Orientation = StackOrientation.Horizontal, BackgroundColor = Color.White, Spacing =0,Padding = gridPadding,Margin = new Thickness(0.5, 0, 0, 0) };
                FlexLayout.SetBasis(cellSt, new FlexBasis(0.50f, true)); 
                _AddBadgeLabel(cellSt, o);

                var label2 = new Label { Text = $"{o.TestResult} {o.Unit}", FontSize = 14,  HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, BackgroundColor = Color.Transparent,MaxLines = 2 };
                cellSt.Children.Add(label2);
                flowLayOut.Children.Add(cellSt);

                var label3 = new Label { Text = $"{o.LowerValue}~{o.UpperValue}", FontSize = 14, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, Margin = new Thickness(0.5, 0, 1, 0), BackgroundColor = Color.White,Padding=gridPadding, MaxLines = 2 };

                FlexLayout.SetBasis(label3, new FlexBasis(0.35f, true));
                flowLayOut.Children.Add(label3);
                stRow.Children.Add(flowLayOut);
                stRight.Children.Add(stRow);
            });
            stOutSide.Children.Add(stRight);
            Order_layout.Children.Add(stOutSide);
        }

        private  async Task  _SetGridHeader(StackLayout stRight)
        {
            var stRow = new StackLayout { BackgroundColor=Color.Gray,Spacing=0,Padding=1, VerticalOptions = LayoutOptions.FillAndExpand};
            var flowLayOut = new FlexLayout() { Direction=FlexDirection.Row, VerticalOptions = LayoutOptions.FillAndExpand };
            var label1 = new Label { Text = "検査項目", FontSize = 16, BackgroundColor = Color.FromHex("#eef9e3"), HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, Margin = new Thickness(0, 0, 0.5, 0) };
            FlexLayout.SetBasis(label1,new FlexBasis(0.40f,true));
            flowLayOut.Children.Add(label1); 
          
            var label2 = new Label { Text = "結果", FontSize = 16, BackgroundColor = Color.FromHex("#eef9e3"), HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, Margin = new Thickness(0.5, 0, 0.5, 0) };
            FlexLayout.SetBasis(label2,new FlexBasis(0.50f,true));
            flowLayOut.Children.Add(label2);

            var label3 = new Label { Text = "基準値", FontSize = 16, BackgroundColor = Color.FromHex("#eef9e3"), HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, Margin = new Thickness(0.5, 0, 1, 0) }; 
            FlexLayout.SetBasis(label3, new FlexBasis(0.35f, true));
            flowLayOut.Children.Add(label3);

            stRow.Children.Add(flowLayOut); 
            stRight.Children.Add(stRow);
           
        }

        private void _AddBadgeLabel(StackLayout cellSt, OrderModel o)
        {

            var lbl = new Label { Text = o.AbnormalFlag, TextColor = Color.White, FontSize = 12, Padding = new Thickness(8, 0),VerticalTextAlignment=TextAlignment.Center };
            switch (o.AbnormalFlag)
            {
                case "N":
                    return;
                case "L":
                    lbl.BackgroundColor = Color.FromHex("#286bcc");
                    break;
                case "H":
                    lbl.BackgroundColor = Color.Red;
                    break;
                case "LL":
                    lbl.BackgroundColor = Color.FromHex("#286bcc");
                    cellSt.BackgroundColor = Color.Yellow;
                    break;
                case "HH":
                    lbl.BackgroundColor = Color.Red;
                    cellSt.BackgroundColor = Color.Yellow;
                    break;
            }
            cellSt.Children.Add(lbl);
        }
   
        public LaboratoryTestView()
        {
            InitializeComponent();
        }
    }
}