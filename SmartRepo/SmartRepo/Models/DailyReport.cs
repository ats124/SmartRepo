using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Realms;

namespace Softentertainer.SmartRepo.Models
{
    using Data;

    class DailyReport
    {
        private DailyReportData Data { get; set; }

        public DateTime Date
        {
            get
            {
                var date = this.Data.Date;
                return new DateTime(date.Year, date.Month, date.Day);
            }
            set
            {
                this.Data.Date = new DateTimeOffset(value.Year, value.Month, value.Day, 0, 0, 0, TimeSpan.Zero);
            }            
        }

        public TimeSpan? StartTime
        {
            get
            {
                return this.Data.StartTime.HasValue
                    ? TimeSpan.FromMinutes(this.Data.StartTime.Value)
                    : (TimeSpan?)null;
            }
            set
            {
                this.Data.StartTime = value.HasValue
                    ? (int)value.Value.TotalMinutes
                    : (int?)null;
            }
        }

        public TimeSpan? EndTime
        {
            get
            {
                return this.Data.EndTime.HasValue
                    ? TimeSpan.FromMinutes(this.Data.EndTime.Value)
                    : (TimeSpan?)null;
            }
            set
            {
                this.Data.EndTime = value.HasValue
                    ? (int)value.Value.TotalMinutes
                    : (int?)null;
            }
        }

        public string Comment
        {
            get { return this.Data.Comment; }
            set { this.Data.Comment = value; }
        }

        public static DateTime[] GetReportExistsDateInMonth(int year, int month)
        {
            using (var realm = Realm.GetInstance())
            {
                return DailyReportData.GetByBettweenDate(
                    realm,
                    new DateTimeOffset(year, month, 1, 0, 0, 0, TimeSpan.Zero),
                    new DateTimeOffset(year, month, DateTime.DaysInMonth(year, month), 0, 0, 0, TimeSpan.Zero))
                    .Select(x => new DateTime(x.Date.Year, x.Date.Month, x.Date.Day))
                    .ToArray();
            }
        }
    }
}
