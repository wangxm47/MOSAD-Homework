using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace App2.Item
{
    public class Item_data
    {
        public string id;
        public string title { get; set; }
        public string detail { get; set; }
        public DateTime date { get; set; }
        public BitmapImage img;
        public Item_data(string title, string detail,DateTime date, BitmapImage img)
        {
            this.id = Guid.NewGuid().ToString();
            this.title = title;
            this.detail = detail;
            this.date = date;
            this.img = img;
        }
        
    }
}
