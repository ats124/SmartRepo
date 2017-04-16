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
                var date = this.Data.Date.ToDate();
                return new DateTime(date.Year, date.Month, date.Day);
            }
            set
            {
                this.Data.Date = value.ToIntDate();
            }            
        }

        public TimeSpan StartTime
        {
            get
            {
                return TimeSpan.FromMinutes(this.Data.StartTime);
            }
            set
            {
                this.Data.StartTime = (int)value.TotalMinutes;
            }
        }

        public TimeSpan EndTime
        {
            get
            {
                return TimeSpan.FromMinutes(this.Data.EndTime);
            }
            set
            {
                this.Data.EndTime = (int)value.TotalMinutes;
            }
        }

        public TimeSpan IntervalTime
        {
            get
            {
                return TimeSpan.FromMinutes(this.Data.IntervalTime);
            }
            set
            {
                this.Data.IntervalTime = (int)value.TotalMinutes;
            }
        }

        private DailyReport(DailyReportData data)
        {
            this.Data = data;
        }

        public static DailyReport CreateNew()
        {
            return new DailyReport(new DailyReportData());
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
                    new DateTime(year, month, 1).ToIntDate(),
                    new DateTime(year, month, DateTime.DaysInMonth(year, month)).ToIntDate())
                    .Select(x => x.Date.ToDate())
                    .ToArray();
            }
        }

        public void Save()
        {
            using (var realm = Realm.GetInstance())
            {
                using (var tran = realm.BeginWrite())
                {
                    realm.Add(this.Data, update: true);
                    tran.Commit();
                }
            }
        }
    }
}
