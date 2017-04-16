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
        public DateTimeOffset Date { get; set; }
        public int? StartTime { get; set; }
        public int? EndTime { get; set; }
        public string Comment { get; set; }

        public static DailyReportData GetByDate(Realm realm, DateTimeOffset date)
        {
            return realm.All<DailyReportData>().FirstOrDefault(x => x.Date == date);
        }

        public static DailyReportData[] GetByBettweenDate(Realm realm, DateTimeOffset start, DateTimeOffset end)
        {
            return realm.All<DailyReportData>().Where(x => x.Date >= start && x.Date <= end).ToArray();
        }
    }
}
