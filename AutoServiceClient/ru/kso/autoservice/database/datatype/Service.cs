using System;
using System.IO;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace AutoServiceClient.ru.kso.autoservice.database.datatype
{
    public sealed class Service
    {
        private int _cost;
        private float _discount;

        public int Id { get; set; }

        public string Title { get; set; }

        public int Cost
        {
            get
            {
                return _cost;
            }

            set
            {
                _cost = value;
                SetDiscountCost();
            }
        }

        public float Duration { get; set; }

        public float Discount
        {

            get
            {
                return _discount;
            }

            set
            {
                _discount = value;
                SetDiscountCost();
            }
        }

        public float DiscountCost { get; private set; }

        public BitmapImage Bitmap { get; set; }

        public Service(int id, string title, int cost, float duration)
        {
            Id = id;
            Title = title;
            Cost = cost;
            Duration = duration;
            Discount = 0;
            DiscountCost = Cost - Cost * Discount;
            Bitmap = new BitmapImage();
        }

        public async void SetImageFromStream(Stream stream)
        {
            IRandomAccessStream thread = WindowsRuntimeStreamExtensions.AsRandomAccessStream(stream);
            await Bitmap.SetSourceAsync(thread);
        }

        private void SetDiscountCost()
        {
            DiscountCost = _cost - _cost * _discount;
        }
    }
}
