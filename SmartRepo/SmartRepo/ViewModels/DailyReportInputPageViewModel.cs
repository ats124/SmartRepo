using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
namespace Softentertainer.SmartRepo.ViewModels
{
    using Views;
    using Models;

    /// <summary>
    /// 報告書入力画面のビューモデル
    /// </summary>
    public class DailyReportInputPageViewModel : BindableBase, INavigationAware
    {
		/// <summary>
		/// 選択日付
		/// </summary>
        private DateTime targetDate;
        public DateTime TargetDate
        {
			get { return this.targetDate; }
			set { SetProperty(ref this.targetDate, value); }
        }

		/// <summary>
		/// コメント
		/// </summary>
        private string comment;
        public string Comment
        {
            get { return this.comment; }
            set { SetProperty(ref this.comment, value); }
        }


		/// <summary>
		/// 開始時刻
		/// </summary>
		private TimeSpan startTime;
		public TimeSpan StartTime
		{
			get { return this.startTime;}
			set { SetProperty(ref this.startTime, value);}
		}

		/// <summary>
		/// 終了時刻
		/// </summary>
		private TimeSpan endTime;
		public TimeSpan EndTime
		{
			get { return this.endTime;}
			set { SetProperty(ref this.endTime, value);}
		}

		public TimeSpanItem[] IntervalTimes { get; } = new[]
		{
			new TimeSpanItem(TimeSpan.FromHours(0.5)),
			new TimeSpanItem(TimeSpan.FromHours(1)),
			new TimeSpanItem(TimeSpan.FromHours(1.5)),
		};

		private TimeSpanItem intervalTime;
		public TimeSpanItem IntervalTime
		{
			get { return this.intervalTime; }
			set { SetProperty(ref this.intervalTime, value); }
		}

		/// <summary>
		/// タスク項目リスト
		/// </summary>
		/// <value>タスク情報</value>
		public ObservableCollection<TaskItem> TaskItemList { get; } = new ObservableCollection<TaskItem>(new[] { new TaskItem("item1")});

		/// <summary>
		/// 送信確認イベント
		/// </summary>
		/// <value>The confirm button.</value>
		public DelegateCommand ConfirmButton { get; }

		/// <summary>
		/// タスク項目の削除イベント
		/// </summary>
		/// <value>The task delete buton.</value>
		public ICommand TaskDeleteCommand { get; }

        public DailyReportInputPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService)
        {
            this.ConfirmButton = new DelegateCommand(async () =>
            {
                var report = new DailyReport();
                report.Date = this.TargetDate;
                report.StartTime = this.StartTime;
                report.EndTime = this.EndTime;
                report.IntervalTime = this.IntervalTime.Value;
                report.Comment = this.Comment;
                report.Save();
                await navigationService.NavigateAsync(nameof(ReportConfirmPage), new NavigationParameters()
                {
                    { "Subject", $"日報 ${this.TargetDate:yyyy/MM/dd(ddd)}" },
                    { "Message", $"お疲れ様です。hogehogeです。{Environment.NewLine}{this.Comment}" }
                });
            });

			this.TaskDeleteCommand = new DelegateCommand<TaskItem>((param) =>
			{
				this.TaskItemList.Remove(param);
			});
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
        }

		/// <summary>
		/// Ons the navigating to.
		/// </summary>
		/// <param name="parameters">Parameters.</param>
        public void OnNavigatingTo(NavigationParameters parameters)
        {
            this.TargetDate = (DateTime)parameters["Date"];

            var report = DailyReport.GetReport(this.TargetDate);
            if (report != null)
            {
                this.StartTime = report.StartTime;
                this.EndTime = report.EndTime;
                var intervalTime = this.IntervalTimes.FirstOrDefault(x => x.Value == report.IntervalTime);
                if (intervalTime != null)
                {
                    this.IntervalTime = intervalTime;
                }
                else
                {
                    this.IntervalTime = this.IntervalTimes[1];
                }
                this.Comment = report.Comment;
            }
            else
            {
                // 初期化
                this.StartTime = new TimeSpan(9, 0, 0);
                this.EndTime = new TimeSpan(18, 0, 0);
                this.IntervalTime = this.IntervalTimes[1];
            }
        }

		public class TimeSpanItem
		{
			public TimeSpan Value { get; set; }
			public string Text => this.Value.ToString(@"hh\:mm");

			public TimeSpanItem(TimeSpan value)
			{
				this.Value = value;
			}
		}

		/// <summary>
		/// タスク情報
		/// </summary>
		public class TaskItem :BindableBase
		{
			public TaskItem(string task)
			{
				Task = task;
			}
			string task;
			public String Task
			{
				get { return this.task; }
				set { SetProperty(ref this.task, value);}
			}
		}
    }
}
