﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Softentertainer.SmartRepo.Views
{
    public partial class CalendarView : ContentView
    {
        public CalendarView()
        {
            InitializeComponent();

            // 日付部分のグリッド定義 7日 * 5週
            for (var i = 0; i < 5; i++)
                CalendarDaysGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
            for (var i = 0; i < 7; i++)
                CalendarDaysGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });

            // 日付グリッド内に日付ラベル
            Enumerable.Range(0, 5).ForEach(row =>
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
                        BackgroundColor = (Color)this.Resources["CalendarBackgroundColor"],
                    };
                    Grid.SetRow(grid, row);
                    Grid.SetColumn(grid, col);

                    var label = new Label() { Text = $"{col + 7 * row + 1}" };
                    label.HorizontalOptions = LayoutOptions.End;
                    Grid.SetRow(label, 0);
                    grid.Children.Add(label);

                    CalendarDaysGrid.Children.Add(grid);
                }));
        }
    }
}
