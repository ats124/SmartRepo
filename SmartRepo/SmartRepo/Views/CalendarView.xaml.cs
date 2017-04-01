using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Softentertainer.SmartRepo.Views
{
    public partial class CalendarView : ContentView
    {
        public static readonly BindableProperty CalendarItemsSourceProperty =
            BindableProperty.Create(nameof(CalendarItemsSource), typeof(IDictionary<DateTime, object>), typeof(CalendarView),
                propertyChanged: (bindable, oldValue, newValue) => ((CalendarView)bindable).RefreshCalendarCells(isRefreshSource: true));
        /// <summary>
        /// カレンダーのデータを取得または設定します。
        /// </summary>
        public IDictionary<DateTime, object> CalendarItemsSource
        {
            get => (IDictionary<DateTime, object>)this.GetValue(CalendarItemsSourceProperty);
            set => this.SetValue(CalendarItemsSourceProperty, value);
        }

        public static readonly BindableProperty CalendarItemTemplateProperty =
             BindableProperty.Create(nameof(CalendarItemTemplate), typeof(DataTemplate), typeof(CalendarView),
                propertyChanged: (bindable, oldValue, newValue) => ((CalendarView)bindable).RefreshCalendarCells(isRefreshTemplate: true));
        /// <summary>
        /// カレンダーデータを表現するテンプレートを取得または設定します。
        /// </summary>
        public DataTemplate CalendarItemTemplate
        {
            get => (DataTemplate)this.GetValue(CalendarItemTemplateProperty);
            set => this.SetValue(CalendarItemTemplateProperty, value);
        }

        public static readonly BindableProperty ViewMonthProperty =
            BindableProperty.Create(nameof(ViewMonth), typeof(DateTime), typeof(CalendarView), DateTime.MinValue,
                defaultValueCreator: bindable =>
                {
                    var today = DateTime.Today;
                    return new DateTime(today.Year, today.Month, 1);
                },
                propertyChanged: (bindable, oldValue, newValue) => ((CalendarView)bindable).RefreshCalendarCells(isRefreshDay: true));
        /// <summary>
        /// カレンダー表示する年月を取得または設定します。
        /// </summary>
        public DateTime ViewMonth
        {
            get => (DateTime)this.GetValue(ViewMonthProperty);
            set => this.SetValue(ViewMonthProperty, new DateTime(value.Year, value.Month, 1));
        }

        private readonly Color CalendarBackgroundColor;
        private readonly Color SundayColor;
        private readonly Color SaturdayColor;
        private readonly Color OutOfRangeDayColor;
        private readonly string[] DayOfWeeks;

        private readonly List<(Label DayLabel, ContentView DayContent)> dayElements;

        public CalendarView()
        {
            InitializeComponent();

            this.CalendarBackgroundColor = (Color)this.Resources["CalendarBackgroundColor"];
            this.SundayColor = (Color)this.Resources["SundayColor"];
            this.SaturdayColor = (Color)this.Resources["SaturdayColor"];
            this.OutOfRangeDayColor = (Color)this.Resources["OutOfRangeDayColor"];
            this.DayOfWeeks = (string[])this.Resources["DayOfWeeks"];

            // 曜日行
            CalendarDaysGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            // 6週
            for (var i = 0; i < 6; i++)
                CalendarDaysGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
            // 曜日列
            for (var i = 0; i < 7; i++)
                CalendarDaysGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });

            // 曜日のラベル
            this.DayOfWeeks.ForEach((w, index) =>
            {
                var dayOfWeek = (DayOfWeek)index;
                var label = new Label()
                {
                    Text = w,
                    HorizontalTextAlignment = TextAlignment.Center,
                    BackgroundColor = this.CalendarBackgroundColor,
                    HorizontalOptions = LayoutOptions.Fill,
                    TextColor = 
                        dayOfWeek == DayOfWeek.Sunday 
                            ? this.SundayColor
                        : dayOfWeek == DayOfWeek.Saturday 
                            ? this.SaturdayColor
                            : Color.Default,
                };
                Grid.SetRow(label, 0);
                Grid.SetColumn(label, index);
                CalendarDaysGrid.Children.Add(label);
            });

            // 日付グリッド内に日付ラベル
            this.dayElements = new List<(Label DayLabel, ContentView DayContent)>();
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
                        BackgroundColor = this.CalendarBackgroundColor,
                    };
                    Grid.SetRow(grid, row + 1);
                    Grid.SetColumn(grid, col);
                    CalendarDaysGrid.Children.Add(grid);

                    // 日を表すラベル
                    var label = new Label();
                    label.HorizontalOptions = LayoutOptions.End;
                    Grid.SetRow(label, 0);
                    grid.Children.Add(label);

                    // カレンダーのセルに載せるコンテンツ
                    var content = new ContentView();
                    Grid.SetRow(content, 1);
                    grid.Children.Add(content);

                    this.dayElements.Add((label, content));
                }));

            RefreshCalendarCells(true, true, true);
        }

        private void RefreshCalendarCells(bool isRefreshDay = false, bool isRefreshTemplate = false, bool isRefreshSource = false)
        {
            // カレンダーの先頭日付(日曜日から始める)を取得する
            var viewMonth = this.ViewMonth;
            var firstCalendarDay = viewMonth.AddDays(-(int)viewMonth.DayOfWeek);

            this.dayElements.ForEach((dayElement, index) =>
            {
                var date = firstCalendarDay.AddDays(index);

                // 日付
                if (isRefreshDay)
                {
                    dayElement.DayLabel.Text = date.ToString("%d");
                    dayElement.DayLabel.TextColor = date.Month == viewMonth.Month
                        ? Color.Default
                        : this.OutOfRangeDayColor;
                }

                // コンテンツのテンプレート
                if (isRefreshTemplate)
                {
                    dayElement.DayContent.Content = this.CalendarItemTemplate?.CreateContent() as View;
                }

                // データバインド
                if (isRefreshSource)
                {
                    dayElement.DayContent.BindingContext =
                        this.CalendarItemsSource != null &&
                        this.CalendarItemsSource.TryGetValue(date, out var data)
                            ? data
                            : null;
                }
            });
        }
    }
}
