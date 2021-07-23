using PatientEHR.Models;
using PatientEHR.ViewModels;
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PatientEHR.Views
{
    public partial class PatientInfoPage : ContentPage
    {
        public PatientInfoPage()
        {
            InitializeComponent();
            PatientInfoPageViewModel.Instance.SetDataCallBack += SetPageData;
            PatientInfoPageViewModel.Instance.GetBaseInfo();
        }

        public bool SetPageData(Dictionary<string, List<BaseInfoModel>> dic)
        {
            baseInfo_Layout.Children.Clear();
            foreach (var item in dic)
            {
                var st = new StackLayout() {BackgroundColor=Color.FromHex("#689e39"),Padding=new Thickness(0,1,0,1) ,Spacing=1};
                var title = new Label { Text = item.Key, FontSize = 20, HorizontalOptions = LayoutOptions.FillAndExpand,BackgroundColor=Color.FromHex("#689e39"),TextColor=Color.White };
                st.Children.Add (title);
                item.Value.ForEach((v)=> {
                    var rowLay = new FlexLayout() {Direction=FlexDirection.Row,BackgroundColor=Color.White,Padding=5};
                    var leftSt = new StackLayout() { Orientation = StackOrientation.Vertical };
                    var rightSt = new StackLayout() { HorizontalOptions=LayoutOptions.End};
                    FlexLayout.SetBasis(leftSt,new FlexBasis(0.65f,true));
                    FlexLayout.SetBasis(rightSt, new FlexBasis(0.35f,true));
                    leftSt.Children.Add( new Label { Text = v.Attribute, FontSize = 16} );
                    leftSt.Children.Add(new Label { Text =string.Format("{0:yyyy/MM/dd HH:mm}", v.LastUpdateTime), FontSize = 16});

                    rightSt.Children.Add(new Label { Text =v.OfficeName, FontSize = 16,HorizontalTextAlignment=TextAlignment.End});
                    rowLay.Children.Add(leftSt);
                    rowLay.Children.Add(rightSt);

                    st.Children.Add(rowLay);
                });
                baseInfo_Layout.Children.Add(st);
            }
            return true;
        }
    }
}
