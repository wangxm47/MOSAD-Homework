using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace App2.Item_list
{
    class Item_list
    {
        private ObservableCollection<Item.Item_data> Allitems = new ObservableCollection<Item.Item_data>();
        public ObservableCollection<Item.Item_data> AllItems { get { return this.Allitems; } }

        private Item.Item_data selectedItem = default(Item.Item_data);
        public Item.Item_data SelectedItem { get { return selectedItem; } set { this.selectedItem = value; } }

        BitmapImage img;

        public Item_list()
        {
            Img_source();
            Allitems.Add(new Item.Item_data("python", "python",DateTime.Today.Date,img));
            Allitems.Add(new Item.Item_data("pre", "pre", DateTime.Today.Date,img));
        }
        public void Add_item(string title, string detail,DateTime date,BitmapImage img)
        {
            Allitems.Add(new Item.Item_data(title, detail,date,img));
        }
        public void delete_item(string id)
        {
            selectedItem = Allitems.SingleOrDefault(item => item.id == id);
            Allitems.Remove(selectedItem);
            selectedItem = null;
        }
        public void update_item(string title, string detail, string id, DateTime date , BitmapImage img)
        {
            Allitems.SingleOrDefault(item => item.id == id).title = title;
            Allitems.SingleOrDefault(item => item.id == id).detail = detail;
            Allitems.SingleOrDefault(item => item.id == id).date = date;
            Allitems.SingleOrDefault(item => item.id == id).img= img;
            selectedItem = null;
        }

        private async void Img_source()
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/item.jpg"));
            if (file != null)
            {
                IRandomAccessStream ir = await file.OpenAsync(FileAccessMode.Read);
                img = new BitmapImage();
                await img.SetSourceAsync(ir);
            }
        }
    }
}
