using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace AutoServiceClient.ru.kso.autoservice.database.datatype
{
    public sealed class Service
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Cost { get; set; }
        public int Duration { get; set; }
        public float Discount { get; set; }
        public BitmapImage Bitmap
        {
            get
            {
                return Bitmap;
            }
        }

        public Service (int id, string title, int cost, int duration)
        {
            Id = id;
            Title = title;
            Cost = cost;
            Duration = duration;
        }

        public async void SetImage(Stream stream)
        {
            IRandomAccessStream thread = WindowsRuntimeStreamExtensions.AsRandomAccessStream(stream);
            await Task.Run(() => Bitmap.SetSourceAsync(thread));
        }
    }
}
