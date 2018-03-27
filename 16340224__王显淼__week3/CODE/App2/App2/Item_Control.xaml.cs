using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using App2.Item;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace App2
{
    public sealed partial class Item_Control : UserControl
    {
        public Item.Item_data Item { get { return DataContext as Item.Item_data; } set { Item = value; } }
        public Item_Control()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) => Bindings.Update();
        }

        private void Check_Click(object sender, RoutedEventArgs e)
        {
            if (Check.IsChecked == true)
            {
                Line1.StrokeThickness = 2;
            }
            else
            {
                Line1.StrokeThickness = 0;
            }
        }


    }
}
