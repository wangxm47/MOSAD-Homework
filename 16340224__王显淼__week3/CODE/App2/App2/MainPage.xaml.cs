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
using App2.Item_list;
using Windows.UI.Popups;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using Windows.UI.Core;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace App2
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Item_list.Item_list item_List { get; set; }
        public MainPage()
        {
            this.InitializeComponent();
            item_List = new Item_list.Item_list();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (e.Parameter.GetType() == typeof(Item_list.Item_list))
            {
                item_List = e.Parameter as Item_list.Item_list;
            }
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                  AppViewBackButtonVisibility.Collapsed;
        }
        private async void SelectPictureButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker open = new FileOpenPicker();
            
            open.ViewMode = PickerViewMode.Thumbnail;
            open.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            open.FileTypeFilter.Add(".jpg");
            open.FileTypeFilter.Add(".png");
            open.FileTypeFilter.Add(".jpeg");
            open.FileTypeFilter.Add(".gif");

            StorageFile file = await open.PickSingleFileAsync();

            if (file != null)
            {
                IRandomAccessStream ir = await file.OpenAsync(FileAccessMode.Read);
                BitmapImage bi = new BitmapImage();
                await bi.SetSourceAsync(ir);
                item_pic.Source = bi;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            item_List.SelectedItem = null;
            Frame.Navigate(typeof(NewPage), item_List);
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (item_List.SelectedItem != null)
            {
                item_List.delete_item(item_List.SelectedItem.id);
                var msg = new MessageDialog("Delete succeed").ShowAsync();
            }
            item_List.SelectedItem = null;
            delete.Visibility = Visibility.Collapsed;
            restore();
        }

        private void items_view_ItemClick(object sender, ItemClickEventArgs e)
        {
            item_List.SelectedItem = e.ClickedItem as Item.Item_data;
            delete.Visibility = Visibility.Visible;
            if (All.ActualWidth > 800)
            {
                CreateButton.Content = "Update";
                title.Text = item_List.SelectedItem.title;
                detail.Text = item_List.SelectedItem.detail;
                date.Date = item_List.SelectedItem.date;
                item_pic.Source = item_List.SelectedItem.img;
            }
            else
            {
                Frame.Navigate(typeof(NewPage), item_List);
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (title.Text == "")
            {
                var msg = new MessageDialog("Title cannot be empty!").ShowAsync();
                return;
            }
            else
            {
                if (detail.Text == "")
                {
                    var msg = new MessageDialog("Detail cannot be empty!").ShowAsync();
                    return;
                }
                else
                {
                    if (date.Date <= DateTime.Now)
                    {
                        var msg = new MessageDialog("The due date has passed!").ShowAsync();
                        return;
                    }
                    else
                    {
                        if (CreateButton.Content.ToString() == "Create")
                        {
                            item_List.Add_item(title.Text, detail.Text, date.Date.DateTime, (BitmapImage)item_pic.Source);
                            var msg = new MessageDialog("Create succeed").ShowAsync();
                            restore();
                        }
                        else
                        {
                            item_List.update_item(title.Text, detail.Text, item_List.SelectedItem.id, date.Date.DateTime, (BitmapImage)item_pic.Source);
                            item_List.SelectedItem = null;
                            Frame.Navigate(typeof(MainPage),item_List);
                            var msg = new MessageDialog("Update succeed").ShowAsync();
                            restore();
                        }
                        
                    }
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            item_List.SelectedItem = null;
            CreateButton.Content = "Create";
            restore();
        }

        private async void restore()
        {
            delete.Visibility = Visibility.Collapsed;
            title.Text = detail.Text = "";
            date.Date = DateTime.Now;

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/item.jpg"));
            if (file != null)
            {
                IRandomAccessStream ir = await file.OpenAsync(FileAccessMode.Read);
                BitmapImage bi = new BitmapImage();
                await bi.SetSourceAsync(ir);
                item_pic.Source = bi;
            }
        }
    }
}
