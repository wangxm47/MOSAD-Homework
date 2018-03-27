using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace App2
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NewPage : Page
    {
        public NewPage()
        {
            this.InitializeComponent();
        }
        private Item_list.Item_list items;
        private void Create_Click(object sender, RoutedEventArgs e)
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
                            items.Add_item(title.Text, detail.Text, date.Date.DateTime, (BitmapImage)item_pic.Source);
                            var msg = new MessageDialog("Create succeed").ShowAsync();
                            Frame.Navigate(typeof(MainPage), items);
                        }
                        else
                        {
                            items.update_item(title.Text, detail.Text, items.SelectedItem.id, date.Date.DateTime, (BitmapImage)item_pic.Source);
                            var msg = new MessageDialog("Update succeed").ShowAsync();
                            Frame.Navigate(typeof(MainPage), items);
                        }
                    }
                }
            }
        }

        private async void Cancel_Click(object sender, RoutedEventArgs e)
        {
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
            Frame.Navigate(typeof(MainPage), items);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            items = e.Parameter as Item_list.Item_list;

            if (rootFrame.CanGoBack)
            {
                // Show UI in title bar if opted-in and in-app backstack is not empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
            else
            {
                // Remove the UI from the title bar if in-app back stack is empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            }

            if (items.SelectedItem != null)
            {
                CreateButton.Content = "Update";
                delete.Visibility = Visibility.Visible;
                title.Text = items.SelectedItem.title;
                detail.Text = items.SelectedItem.detail;
                date.Date = items.SelectedItem.date;
                item_pic.Source = items.SelectedItem.img;
            }
            else
            {
                var msg = new MessageDialog("Welcome!").ShowAsync();
            }
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

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            items.delete_item(items.SelectedItem.id);
            items.SelectedItem = null;
            var msg = new MessageDialog("Delete suceed!").ShowAsync();
            Frame.Navigate(typeof(MainPage), items);
        }
    }
}
