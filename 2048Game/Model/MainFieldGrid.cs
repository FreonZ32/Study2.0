using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace _2048Game.Model
{
    class MainFieldGrid
    {
        public Grid FieldGrid { get; set; }
        public ObservableCollection<Border> Border { get; set; }
        public ObservableCollection<Label> Label { get; set; }
        List<ColumnDefinition> FieldGrid_Columns { get; set; }
        List<RowDefinition> FieldGrid_Rows { get; set; }

        public MainFieldGrid(int size = 4)
        {
            FieldGrid = new Grid();
            Border = new ObservableCollection<Border>();
            Label = new ObservableCollection<Label>();
            FieldGrid_Columns = new List<ColumnDefinition>();
            FieldGrid_Rows = new List<RowDefinition>();
            ChangeFieldSize(size);
        }
        public void ChangeFieldSize(int size)
        {
            Border.Clear();
            Label.Clear();
            FieldGrid_Columns.Clear();
            FieldGrid_Rows.Clear();
            FieldGrid = new Grid();
            for (int i = 0; i < size; i++)
            {
                FieldGrid_Columns.Add(new ColumnDefinition());
                FieldGrid_Columns[i].Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star);
                FieldGrid.ColumnDefinitions.Add(FieldGrid_Columns[i]);
                FieldGrid_Rows.Add(new RowDefinition());
                FieldGrid_Rows[i].Height = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star);
                FieldGrid.RowDefinitions.Add(FieldGrid_Rows[i]);
            }
            for (int i = 0; i < size * size; i++)
            {
                Border.Add(new System.Windows.Controls.Border());
                Border[i].Margin = new System.Windows.Thickness(3);
                Border[i].Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xEE, 0xE2, 0xD6));
                Border[i].CornerRadius = new System.Windows.CornerRadius(4);
                Border[i].Height = (455 - (3 * size * 2)) / size;
                Label.Add(new System.Windows.Controls.Label());
                Label[i].Content = "0";
                Label[i].HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                Label[i].VerticalAlignment = System.Windows.VerticalAlignment.Center;
                Label[i].FontSize = 24-size;
                Border[i].Child = Label[i];
                FieldGrid.Children.Add(Border[i]);
                Grid.SetColumn(Border[i], i % size);
                Grid.SetRow(Border[i], i / size);
            }
        }
    }
}
