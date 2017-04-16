using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Softentertainer.SmartRepo.Views
{
    using Utils;

    public partial class CalendarView : ContentView
    {
        public struct YearMonth
        {
            /// <summary>
            /// 西暦1年1月からの経過月
            /// </summary>
            private int _monthMount;
            public int Year => 1 + _monthMount / 12;
            public int Month => _monthMount % 12 + 1;

            public YearMonth(int year, int month)
            {
                _monthMount = (year - 1) * 12 + month - 1;
            }

            public YearMonth AddMonths(int value)
            {
                var newYearMonth = this;
                newYearMonth._monthMount += value;
                return newYearMonth;
            }

            public DateTime ToDateTime() => new DateTime(this.Year, this.Month, 1);

            public static YearMonth FromDateTime(DateTime date) => new YearMonth(date.Year, date.Month);

            public static YearMonth ThisMonth => FromDateTime(DateTime.Today);

            public override string ToString()
            {
                return $"{this.Year}/{this.Month:D2}";
            }
        }

        public static readonly BindableProperty CalendarItemsSourceProperty =
            BindableProperty.Create(nameof(CalendarItemsSource), typeof(IDictionary<DateTime, object>), typeof(CalendarView),
                propertyChanged: (bindable, oldValue, newValue) => ((CalendarView)bindable).RefreshCalendarCells(isRefreshSource: true));

        public static readonly BindableProperty CalendarItemTemplateProperty =
             BindableProperty.Create(nameof(CalendarItemTemplate), typeof(DataTemplate), typeof(CalendarView),
                propertyChanged: (bindable, oldValue, newValue) => ((CalendarView)bindable).RefreshCalendarCells(isRefreshTemplate: true));

        public static readonly BindableProperty ViewMonthProperty =
            BindableProperty.Create(nameof(ViewMonth), typeof(YearMonth), typeof(CalendarView),
                defaultValue: YearMonth.ThisMonth,
                defaultBindingMode: BindingMode.TwoWay, 
                defaultValueCreator: bindable => YearMonth.ThisMonth,
                propertyChanged: (bindable, oldValue, newValue) => ((CalendarView)bindable).RefreshCalendarCells(isRefreshDay: true));

        public static readonly BindableProperty CalendarItemCommandProperty = BindableProperty.Create(nameof(CalendarItemCommand), typeof(ICommand), typeof(CalendarView), null);

        private static readonly BindableProperty ItemDateProperty =
            BindableProperty.CreateAttached("ItemDate", typeof(DateTime?), typeof(CalendarView), (DateTime?)null);

        /// <summary>
        /// カレンダーのデータを取得または設定します。
        /// </summary>
        public IDictionary<DateTime, object> CalendarItemsSource
        {
			get { return (IDictionary<DateTime, object>)this.GetValue(CalendarItemsSourceProperty); }
			set { this.SetValue(CalendarItemsSourceProperty, value); }
        }

        /// <summary>
        /// カレンダーデータを表現するテンプレートを取得または設定します。
        /// </summary>
        public DataTemplate CalendarItemTemplate
        {
			get { return (DataTemplate)this.GetValue(CalendarItemTemplateProperty); }
			set { this.SetValue(CalendarItemTemplateProperty, value); }
        }

        /// <summary>
        /// カレンダー表示する年月を取得または設定します。
        /// </summary>
        public YearMonth ViewMonth
        {
			get { return (YearMonth)this.GetValue(ViewMonthProperty); }
			set { this.SetValue(ViewMonthProperty, value); }
        }

        /// <summary>
        /// カレンダーの日付をクリックしたときのコマンド
        /// </summary>
        public ICommand CalendarItemCommand
        {
			get { return (ICommand)this.GetValue(CalendarItemCommandProperty); }
			set { this.SetValue(CalendarItemsSourceProperty, value); }
        }

        private readonly Color calendarBackgroundColor;
        private readonly Color sundayColor;
        private readonly Color saturdayColor;
        private readonly Color outOfRangeDayColor;
        private readonly Color todayBackgroundColor;
        private readonly string[] dayOfWeekStrings;

        private readonly List<Tuple<Grid, Label, ContentView>> dayElements;

        public CalendarView()
        {
            InitializeComponent();

            this.calendarBackgroundColor = (Color)this.Resources["CalendarBackgroundColor"];
            this.sundayColor = (Color)this.Resources["SundayColor"];
            this.saturdayColor = (Color)this.Resources["SaturdayColor"];
            this.outOfRangeDayColor = (Color)this.Resources["OutOfRangeDayColor"];
            this.todayBackgroundColor = (Color)this.Resources["TodayBackgroundColor"];
            this.dayOfWeekStrings = (string[])this.Resources["DayOfWeeks"];

            // 曜日行
            CalendarDaysGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            // 6週
            for (var i = 0; i < 6; i++)
                CalendarDaysGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
            // 曜日列
            for (var i = 0; i < 7; i++)
                CalendarDaysGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });

            // 曜日のラベル
            this.dayOfWeekStrings.ForEach((w, index) =>
            {
                var dayOfWeek = (DayOfWeek)index;
                var label = new Label()
                {
                    Text = w,
                    HorizontalTextAlignment = TextAlignment.Center,
                    BackgroundColor = this.calendarBackgroundColor,
                    HorizontalOptions = LayoutOptions.Fill,
                    TextColor = 
                        dayOfWeek == DayOfWeek.Sunday 
                            ? this.sundayColor
                        : dayOfWeek == DayOfWeek.Saturday 
                            ? this.saturdayColor
                            : Color.Default,
                };
                Grid.SetRow(label, 0);
                Grid.SetColumn(label, index);
                CalendarDaysGrid.Children.Add(label);
            });

            // カレンダーセルの日付ラベル色
            var calendarDayToColorConverter = 
                new DelegateValueConverter((value, targetType, parameter, culture) =>
                {
                    if (value is DateTime)
                    {
						var date = (DateTime)value;
                        return date.Month == this.ViewMonth.Month
                            ? date.DayOfWeek == DayOfWeek.Sunday
                                ? this.sundayColor
                            : date.DayOfWeek == DayOfWeek.Saturday
                                ? this.saturdayColor
                            : Color.Default
                        : this.outOfRangeDayColor;
                    }
                    return Color.Default;
                });

            // カレンダーセルの背景色
            var calendarDayToBackgroundColor =
                new DelegateValueConverter((value, targetType, parameter, culture) =>
                {
					if (value is DateTime)
					{
						var date = (DateTime)value;
                        if (date == DateTime.Today)
                        {
                            return this.todayBackgroundColor;
                        }
					}
					return this.calendarBackgroundColor;
                });

            // カレンダーセルのタップ
            var gr = new TapGestureRecognizer();
            gr.Tapped += CalendarDayGridCell_Tapped;

            // 日付グリッド内に日付ラベル
            this.dayElements = new List<Tuple<Grid, Label, ContentView>>();
            Enumerable.Range(0, 6).ForEach(row =>
                Enumerable.Range(0, 7).ForEach(col =>
                {
                    var grid = new Grid()
                    {
                        HorizontalOptions = LayoutOptions.Fill,
                        VerticalOptions = LayoutOptions.Fill,
                        RowDefinitions = new RowDefinitionCollection()
                        {
                            new RowDefinition() { Height = GridLength.Auto },
                            new RowDefinition() { Height = GridLength.Star },
                        },
                    };
                    grid.SetBinding(Grid.BackgroundColorProperty, new Binding(".", mode: BindingMode.OneWay, converter: calendarDayToBackgroundColor));
                    grid.GestureRecognizers.Add(gr);
                    Grid.SetRow(grid, row + 1);
                    Grid.SetColumn(grid, col);                    
                    CalendarDaysGrid.Children.Add(grid);

                    // 日を表すラベル
                    var label = new Label();
                    label.SetBinding(Label.TextProperty, new Binding(".", mode: BindingMode.OneWay, stringFormat: "{0:%d}"));
                    label.SetBinding(Label.TextColorProperty, new Binding(".", mode: BindingMode.OneWay, converter: calendarDayToColorConverter));
                    label.HorizontalOptions = LayoutOptions.End;
                    Grid.SetRow(label, 0);
                    grid.Children.Add(label);

                    // カレンダーのセルに載せるコンテンツ
                    var content = new ContentView()
                    {
                        HorizontalOptions = LayoutOptions.Fill,
                        VerticalOptions = LayoutOptions.Fill,
                    };
                    Grid.SetRow(content, 1);
                    grid.Children.Add(content);

                    this.dayElements.Add(Tuple.Create(grid, label, content));
                }));

            RefreshCalendarCells(true, true, true);
        }

        private void CalendarDayGridCell_Tapped(object sender, EventArgs e)
        {
            // 日付のラベル部分のバインディングコンテキスト
            var date = (DateTime)((Grid)sender).Children.OfType<Label>().First().BindingContext;
            if (this.CalendarItemCommand?.CanExecute(date) ?? false)
            {
                this.CalendarItemCommand?.Execute(date);
            }
        }

        private void ViewMonthPreviousButton_Clicked(object sender, EventArgs e)
        {
            this.ViewMonth = this.ViewMonth.AddMonths(-1);
        }

        private void ViewMonthNextButton_Clicked(object sender, EventArgs e)
        {
            this.ViewMonth = this.ViewMonth.AddMonths(1);
        }

        private void RefreshCalendarCells(bool isRefreshDay = false, bool isRefreshTemplate = false, bool isRefreshSource = false)
        {
            // カレンダーの先頭日付(日曜日から始める)を取得する
            var viewMonth = this.ViewMonth;
            var viewMonthDateTime = viewMonth.ToDateTime();
            var firstCalendarDay = viewMonthDateTime.AddDays(-(int)viewMonthDateTime.DayOfWeek);
            var today = DateTime.Today;

            var calendarItemTemplate = this.CalendarItemTemplate;
            var calendarItemSource = this.CalendarItemsSource;

            this.dayElements.ForEach((dayElement, index) =>
            {
                var date = firstCalendarDay.AddDays(index);

                // 日付
                if (isRefreshDay)
                {
                    dayElement.Item2.BindingContext = date;
                }

                // コンテンツのテンプレート
                if (isRefreshTemplate)
                {
                    dayElement.Item3.Content = this.CalendarItemTemplate?.CreateContent() as View;
                }

                // データバインド
                if (isRefreshTemplate || isRefreshSource || isRefreshDay)
                {
                    object data = null;
                    this.CalendarItemsSource?.TryGetValue(date, out data);
                    dayElement.Item3.BindingContext = data;
                }
            });
        }
    }
}
