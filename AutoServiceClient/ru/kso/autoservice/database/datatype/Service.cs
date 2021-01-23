using System;
using System.IO;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace AutoServiceClient.ru.kso.autoservice.database.datatype
{
    public sealed class Service
    {
        private int _id;
        private string _title;
        private int _cost;
        private float _duration;
        private float _discount;
        private float _discountCost;
        private BitmapImage _bitmap;

        public int Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                _title = value;
            }
        }

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

        public float Duration
        {
            get
            {
                return _duration;
            }

            set
            {
                _duration = value;
            }
        }

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

        public float DiscountCost
        {
            get
            {
                return _discountCost;
            }

            private set
            {
                _discountCost = value;
            }
        }

        public BitmapImage Bitmap
        {
            get
            {
                return _bitmap;
            }

            set
            {
                _bitmap = value;
            }
        }

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
            _discountCost = _cost - _cost * _discount;
        }
    }
}
