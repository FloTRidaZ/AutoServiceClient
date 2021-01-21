using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoServiceClient.ru.kso.autoservice.database.connector
{
    public sealed class DBAttributes
    {
        public const int TITLE_ATTR = 1;
        public const int ID_ATTR = 0;
        public const int COST_ATTR = 2;
        public const int DURATION_IN_SECONDS_ATTR = 3;
        public const int PHOTO_ATTR = 5;
        public const int DISCOUNT_ATTR = 4;
        public const int COUNT_ATTR = 0;

        private DBAttributes()
        {

        }
    }
}
