using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Realms;
namespace Softentertainer.SmartRepo.Data
{
    public class DailyReportData : RealmObject
    {
        [PrimaryKey]
        public int Date { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public int IntervalTime { get; set; }
        public string Comment { get; set; }

        public static DailyReportData GetByDate(Realm realm, int date)
        {
            return realm.All<DailyReportData>().FirstOrDefault(x => x.Date == date);
        }

        public static DailyReportData[] GetByBettweenDate(Realm realm, int start, int end)
        {
            return realm.All<DailyReportData>().Where(x => x.Date >= start && x.Date <= end).ToArray();
        }
    }
}
