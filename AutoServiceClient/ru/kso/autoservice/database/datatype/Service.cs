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
        public float Duration { get; set; }
        public float Discount { get; set; }
        public BitmapImage Bitmap { get; set; }

        public Service(int id, string title, int cost, float duration)
        {
            Id = id;
            Title = title;
            Cost = cost;
            Duration = duration;
            Bitmap = new BitmapImage();
        }

        public async void SetImageFromStream(Stream stream)
        {
            IRandomAccessStream thread = WindowsRuntimeStreamExtensions.AsRandomAccessStream(stream);
            await Bitmap.SetSourceAsync(thread);
        }
    }
}
