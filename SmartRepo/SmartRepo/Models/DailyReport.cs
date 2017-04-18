using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Realms;
using Plugin.Messaging;

namespace Softentertainer.SmartRepo.Models
{
    using Data;

    class DailyReport
    {
        public DateTime Date { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public TimeSpan IntervalTime { get; set; }

        public string Comment { get; set; }

        public static DailyReport GetReport(DateTime date)
        {
            using (var realm = Realm.GetInstance())
            {
                var data = DailyReportData.GetByDate(realm, date.ToIntDate());
                if (data == null) return null;
                return new DailyReport()
                {
                    Date = data.Date.ToDate(),
                    StartTime = TimeSpan.FromMinutes(data.StartTime),
                    EndTime = TimeSpan.FromMinutes(data.EndTime),
                    IntervalTime = TimeSpan.FromMinutes(data.IntervalTime),
                    Comment = data.Comment,
                };
            }
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
                    var data = new DailyReportData()
                    {
                        Date = this.Date.ToIntDate(),
                        StartTime = (int)this.StartTime.TotalMinutes,
                        EndTime = (int)this.EndTime.TotalMinutes,
                        IntervalTime = (int)this.IntervalTime.TotalMinutes,
                        Comment = this.Comment,
                    };
                    realm.Add(data, update: true);
                    tran.Commit();
                }
            }
        }

        public string CreateMailBody() => CreateMailBody(null);

        public string CreateMailTitle()
        {
            return $"{this.Date:yyyy\\年MM\\月dd\\日} の日報";
        }

        private string CreateMailBody(string newLine)
        {
            var comment = this.Comment;
            if (newLine == null)
            {
                newLine = newLine ?? Environment.NewLine;
            }
            else
            {
                comment = this.Comment.Replace(Environment.NewLine, newLine);
            }
            var builder = new StringBuilder()
                .Append($"作業時間:{this.StartTime:hh\\:mm}～{this.EndTime:hh\\:mm}").Append(newLine)
                .Append($"休憩時間:{this.IntervalTime:hh\\:mm}").Append(newLine)
                .Append($"コメント:").Append(newLine)
                .Append(comment);

            return builder.ToString();
        }

        public IEmailMessage CreateEmail(IEmailTask emailTask, string toName, string toEmailAddress)
        {
            var builder = new EmailMessageBuilder()
                .Subject(CreateMailTitle())
                .Body(CreateMailBody("\r\n"));
            if (!string.IsNullOrEmpty(toEmailAddress))
            {
                if (!string.IsNullOrEmpty(toName))
                {
                    builder.To($"{Settings.ToName}<{Settings.ToMailAddress}>");
                }
                else
                {
                    builder.To(toEmailAddress);
                }
            }
            return builder.Build();
        }
    }
}
