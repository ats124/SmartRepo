using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softentertainer.SmartRepo.Data
{
    public static class DateTimeExtension
    {
        public static int ToIntDate(this DateTime @this) 
            => (int)(@this - DateTime.MinValue).TotalDays;

        public static DateTime ToDate(this int @this) 
            => DateTime.MinValue.AddDays(@this);
    }
}
