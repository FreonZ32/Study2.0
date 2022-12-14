using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace _2048Game.Model
{
    class MainFieldGrid :INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void Notify([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        string ContentOfEmptyPlate = "";    //just for debug
        int SizeofField { get; set; }   //Постоянно размер поля (равный по ширине/высоте)
        public Grid FieldGrid { get; set; }
        public List<List<Border>> PlateBorder { get; set; } //Плитки
        public List<List<Label>> PlateLabel { get; set; }   //Контент плиток (цифры)
        List<ColumnDefinition> FieldGrid_Columns { get; set; }
        List<RowDefinition> FieldGrid_Rows { get; set; }

        public MainFieldGrid(int size = 4)
        {
            SizeofField = size;
            FieldGrid = new Grid();
            PlateBorder = new List<List<Border>>();
            PlateLabel = new List<List<Label>>();
            FieldGrid_Columns = new List<ColumnDefinition>();
            FieldGrid_Rows = new List<RowDefinition>();
            ChangeFieldSize();
        }


        public void ChangeFieldSize()
        {
            PlateBorder.Clear();
            PlateLabel.Clear();
            FieldGrid_Columns.Clear();
            FieldGrid_Rows.Clear();
            FieldGrid = new Grid();
            for (int i = 0; i < SizeofField; i++)
            {
                FieldGrid_Columns.Add(new ColumnDefinition());
                FieldGrid_Columns[i].Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star);
                FieldGrid.ColumnDefinitions.Add(FieldGrid_Columns[i]);
                FieldGrid_Rows.Add(new RowDefinition());
                FieldGrid_Rows[i].Height = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star);
                FieldGrid.RowDefinitions.Add(FieldGrid_Rows[i]);
            }
            for (int i = 0; i < SizeofField; i++)
            {
                List<Border> borderList = new List<Border>();
                List<Label> labelList = new List<Label>();
                for (int j = 0; j < SizeofField; j++)
                {
                    borderList.Add(new Border());
                    borderList[j].Margin = new System.Windows.Thickness(3);
                    borderList[j].Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xCD, 0xC1, 0xB5));
                    borderList[j].CornerRadius = new System.Windows.CornerRadius(4);
                    borderList[j].Height = (455 - (3 * SizeofField * 2)) / SizeofField;

                    labelList.Add(new Label());
                    labelList[j].Content = ContentOfEmptyPlate;
                    labelList[j].HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    labelList[j].VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    labelList[j].FontSize = 24 - SizeofField;

                    borderList[j].Child = labelList[j];
                    FieldGrid.Children.Add(borderList[j]);
                    Grid.SetColumn(borderList[j], j);
                    Grid.SetRow(borderList[j], i);
                }
                PlateBorder.Add(borderList);
                PlateLabel.Add(labelList);
            }
        }
        public bool NextStepPlateCreator()
        {
            Random random = new Random();
            List<(int,int)> availableTiles = new List<(int,int)>(); //Список всех достумных плиток
            for (int i = 0; i < PlateLabel.Count; i++)
            {
                for (int j = 0; j < PlateLabel[i].Count; j++)
                { if (PlateLabel[i][j].Content.ToString() == ContentOfEmptyPlate) availableTiles.Add((i,j));}
            }
            if(availableTiles.Count == 0)return false;  //Если доступных нет, функция вернет false

            int winner = random.Next(0, availableTiles.Count);  // Выбор рандомной доступной плитки
            int twoOfFour = random.Next(0,100);
            if (twoOfFour < 10)
            {//10% появление плитки 4
                PlateLabel[availableTiles[winner].Item1][availableTiles[winner].Item2].Content = "4";return true;
            }
            else
            {//90% появление плитки 2
                PlateLabel[availableTiles[winner].Item1][availableTiles[winner].Item2].Content = "2";return true;
            }
        }
        public void MoveHorisontal()
        {
            for (int i = 0; i < SizeofField; i++)
            {
                for (int j = 0; j < SizeofField; j++)
                {
                    if (PlateLabel[j][i].Content.ToString() == ContentOfEmptyPlate) continue;
                    for (int k = j+1; k < SizeofField; k++)
                    {
                        if (PlateLabel[k][i].Content.ToString() == ContentOfEmptyPlate) continue;
                        if (Convert.ToInt32(PlateLabel[j][i].Content) == Convert.ToInt32(PlateLabel[k][i].Content))
                        {
                            PlateLabel[j][i].Content = (Convert.ToInt32(PlateLabel[j][i].Content) * 2).ToString();
                            PlateLabel[k][i].Content = ContentOfEmptyPlate;
                            break;
                        }
                        break;
                    }
                    for (int k = j-1; k >= 0; k--)
                    {
                        if (PlateLabel[k][i].Content.ToString() == ContentOfEmptyPlate)
                        {
                            PlateLabel[k][i].Content = PlateLabel[k+1][i].Content;
                            PlateLabel[k+1][i].Content = ContentOfEmptyPlate;
                        }
                        else break;
                    }
                }
            }
        }
        public void MoveVertical()
        {
            for (int i = 0; i < SizeofField; i++)
            {
                for (int j = 0; j < SizeofField; j++)
                {
                    if (PlateLabel[i][j].Content.ToString() == ContentOfEmptyPlate) continue;
                    for (int k = j + 1; k < SizeofField; k++)
                    {
                        if (PlateLabel[i][k].Content.ToString() == ContentOfEmptyPlate) continue;
                        if (Convert.ToInt32(PlateLabel[i][j].Content) == Convert.ToInt32(PlateLabel[i][k].Content))
                        {
                            PlateLabel[i][j].Content = (Convert.ToInt32(PlateLabel[i][j].Content) * 2).ToString();
                            PlateLabel[i][k].Content = ContentOfEmptyPlate;
                            break;
                        }
                        break;
                    }
                    for (int k = j - 1; k >= 0; k--)
                    {
                        if (PlateLabel[i][k].Content.ToString() == ContentOfEmptyPlate)
                        {
                            PlateLabel[i][k].Content = PlateLabel[i][k+1].Content;
                            PlateLabel[i][k+1].Content = ContentOfEmptyPlate;
                        }
                        else break;
                    }
                }
            }
        }
        public void ReversAllList()
        {
            for (int i = 0; i < SizeofField; i++)
            {
                PlateLabel[i].Reverse();
            }
        }
        public void MoveToDir(string dir)
        {
            if(dir == "Down")
            {
                PlateLabel.Reverse();
                MoveHorisontal();
                PlateLabel.Reverse();
            }
            if(dir == "Up")
            {
                MoveHorisontal();
            }
            if(dir == "Left")
            {
                MoveVertical();
            }
            if(dir == "Right")
            {
                ReversAllList();
                MoveVertical();
                ReversAllList();
            }
        }
        public int MaxNumber()
        {
            int max = 0;
            for (int i = 0; i < SizeofField; i++)
                for (int j = 0; j < SizeofField; j++)
                {
                    if (PlateLabel[i][j].Content.ToString() != ContentOfEmptyPlate)
                        if (max < Convert.ToInt32(PlateLabel[i][j].Content)) max = Convert.ToInt32(PlateLabel[i][j].Content);
                }
            return max;
        }
        public void BrushForColor()
        {
            for (int i = 0; i < SizeofField; i++)
                for (int j = 0; j < SizeofField; j++)
                {
                    if(PlateLabel[i][j].Content.ToString() == ContentOfEmptyPlate) PlateBorder[i][j].Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xCD, 0xC1, 0xB5));
                    else 
                        switch(Convert.ToInt32(PlateLabel[i][j].Content))
                        {
                            case 2: PlateBorder[i][j].Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xEE, 0xE4, 0xDA));break;
                            case 4: PlateBorder[i][j].Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xEC, 0xE0, 0xCA)); break;
                            case 8: PlateBorder[i][j].Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xF2, 0xB1, 0x79)); break;
                            case 16: PlateBorder[i][j].Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xEC, 0x8D, 0x53)); break;
                            case 32: PlateBorder[i][j].Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xED, 0x69, 0x3D)); break;
                            case 64: PlateBorder[i][j].Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xED, 0x45, 0x45)); break;
                            case 128: PlateBorder[i][j].Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xFF, 0x0B, 0x0B)); break;
                            default: PlateBorder[i][j].Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0xFF, 0x0B, 0x0B)); break;
                        }
                }
        }
    }
    
}
