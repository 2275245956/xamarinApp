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
    public partial class PrescriptionView : ContentView
    {
        public static readonly BindableProperty OrderModelProperty = BindableProperty.Create(nameof(OrderModel), typeof(List<OrderModel>), typeof(PrescriptionView), new List<OrderModel>(), propertyChanged: (bindable, oldValue, newValue) =>
        {
            ((PrescriptionView)bindable).InitializeView();
        });
        public List<OrderModel> OrderModel
        {
            get { return (List<OrderModel>)GetValue(OrderModelProperty); }
            set { SetValue(OrderModelProperty, value); }
        }

        private List<string> userWays = new List<string>();

        private void InitializeView()
        {
            Order_layout.Children.Clear();

            if (OrderModel.Count <= 0) return;
            var screenWidth = DeviceDisplay.MainDisplayInfo.Width;//宽度 
            if (OrderModel == null || OrderModel.Count <= 0) return;
            var dic = new Dictionary<string, List<OrderModel>>();
            OrderModel.ForEach(o =>
            {
                var key = $"RP-{o.GroupNo}";
                if (!dic.ContainsKey(key))
                {
                    dic[key] = new List<OrderModel>();
                }
                dic[key].Add(o);
            });


            var leftTitleWidth = 100;
            var centerTextWidth = screenWidth - leftTitleWidth;
            var unitWidth = 250;
            var titleType = "処方";
            var leftLabel = new Label()
            {
                Text = titleType,
                WidthRequest = leftTitleWidth,
                FontSize = 16,
                VerticalOptions = LayoutOptions.FillAndExpand,
                VerticalTextAlignment = TextAlignment.Start
            };
            var outsideStack = new StackLayout() { Orientation = StackOrientation.Horizontal };
            outsideStack.Children.Add(leftLabel);


            var centerStacklayout = new StackLayout() { Orientation = StackOrientation.Vertical };//垂直显示
            foreach (var item in dic)
            {
                centerStacklayout.Children.Add(new Label { Text = item.Key, FontSize = 14, });//显示 key  RP-0
                //添加项
                var firstGroupModel = item.Value.FirstOrDefault();


                item.Value.ForEach(t =>
                {
                    var st = new StackLayout { Orientation = StackOrientation.Horizontal };//水平显示    内容和单位
                    unitWidth = string.IsNullOrEmpty(t.Unit) ? 0 : unitWidth;
                    st.Children.Add(new Label { Text = $"∟{t.OrderName}", FontSize = 14, WidthRequest = centerTextWidth - unitWidth });
                    if (!string.IsNullOrEmpty(t.Unit))
                    {
                        st.Children.Add(new Label { Text = $"{formatDecimal(t.Amount)} {t.Unit}", FontSize = 14, WidthRequest = unitWidth, HorizontalTextAlignment = TextAlignment.End });
                    }
                    centerStacklayout.Children.Add(st);

                    if (!string.IsNullOrEmpty(t.OrderNote))
                    {
                        var stNote = new StackLayout() { Orientation = StackOrientation.Horizontal, Spacing = 0 };
                        stNote.Children.Add(new Label { Text = $"∟…", FontSize = 14, WidthRequest = 80 });

                        var stlabel = new StackLayout() { Orientation = StackOrientation.Vertical };
                        foreach (var l in GetAllLines(string.Format("{0}", t.OrderNote)))
                        {
                            if (string.IsNullOrEmpty(l.Trim())) continue;
                            stlabel.Children.Add(new Label { Text = $"{l.Trim()} ", FontSize = 14, WidthRequest = centerTextWidth - unitWidth - 80 });
                        }
                        stNote.Children.Add(stlabel);

                        centerStacklayout.Children.Add(stNote);
                    }
                });
                //一组只显示一个  药用量和 groupNote
                if (!string.IsNullOrEmpty(firstGroupModel.DrugOrderInfo))
                {
                    var stDrugOrder = new StackLayout() { Orientation = StackOrientation.Horizontal, Spacing = 0 };

                    stDrugOrder.Children.Add(new Label { Text = $"∟…", FontSize = 14, WidthRequest = 80 });
                    var stlabel = new StackLayout() { Orientation = StackOrientation.Vertical };


                    foreach (var l in GetAllLines(string.Format("{0}", firstGroupModel.DrugOrderInfo)))
                    {
                        if (!string.IsNullOrEmpty(l.Trim()))
                        {
                            stlabel.Children.Add(new Label { Text = $"{l.Trim()}", FontSize = 14, WidthRequest = centerTextWidth - unitWidth - 80 });
                        }

                    }
                    stDrugOrder.Children.Add(stlabel);

                    stDrugOrder.Children.Add(new Label { Text = $"{firstGroupModel.Times} {firstGroupModel.TimesUnit}", FontSize = 14, WidthRequest = unitWidth, HorizontalTextAlignment = TextAlignment.End });
                    centerStacklayout.Children.Add(stDrugOrder);
                }
                if (!string.IsNullOrEmpty(firstGroupModel.GroupNote))
                {
                    var stGroupNote = new StackLayout() { Orientation = StackOrientation.Horizontal, Spacing = 0 };
                    stGroupNote.Children.Add(new Label { Text = $"∟…", FontSize = 14, WidthRequest = 80 });

                    var stlabel = new StackLayout() { Orientation = StackOrientation.Vertical };
                    foreach (var l in GetAllLines(string.Format("{0}", firstGroupModel.GroupNote)))
                    {
                        if (string.IsNullOrEmpty(l.Trim())) continue;
                        stGroupNote.Children.Add(new Label { Text = $"{l.Trim()}", FontSize = 14, WidthRequest = centerTextWidth - unitWidth - 80 });
                    }
                    stGroupNote.Children.Add(stlabel);

                    stGroupNote.Children.Add(new Label { Text = $"{firstGroupModel.GroupNote}", FontSize = 14, WidthRequest = centerTextWidth - unitWidth - 80 });
                    centerStacklayout.Children.Add(stGroupNote);
                }

            }

            outsideStack.Children.Add(centerStacklayout);
            Order_layout.Children.Add(outsideStack);

        }

        string TimeAndTimeunit(int times, string timeUnit)
        {
            if ("全量".Equals(timeUnit) || times == 0)
            {
                return "";
            }
            else
            {
                return string.Format("{0} {1}", times, timeUnit);
            }
        }
        string formatDecimal(decimal d)
        {
            var s = string.Format("{0}", d);
            var ss = s.Split(new[] { '.' });
            if (ss.Length == 1)
            {
                return s;
            }

            while (!string.IsNullOrEmpty(ss[1]) && ss[1].EndsWith("0"))
            {
                ss[1] = ss[1].Substring(0, ss[1].Length - 1);
            }

            if (!string.IsNullOrEmpty(ss[1]))
            {
                return string.Format("{0}.{1}", ss[0], ss[1]);
            }
            else
            {
                return ss[0];
            }
        }
        string[] GetAllLines(string source)
        {
            var strs = source.Split(new[] { '\n' });
            return strs;
        }
        public PrescriptionView()
        {
            InitializeComponent();
        }
    }
}