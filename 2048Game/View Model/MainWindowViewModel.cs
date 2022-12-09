﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;
using System.Drawing;
using _2048Game.Model;
using System.Security.Policy;
using System.Windows.Media;

namespace _2048Game.View_Model
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void Notify([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        int _size_of_field = 4;  //размер MainPlayField (изменяется в RBSize_Click)
        string _RBIsClicked;
        ObservableCollection<Grid> _playFieldContainer;
        MainFieldGrid PlayField;

        public MainWindowViewModel()
        {
            ButtonClickCommand = new ButtonClickCommand(OnClicked);
            _playFieldContainer = new ObservableCollection<Grid>();
            PlayField = new MainFieldGrid(_size_of_field);
            PlayFieldContainer.Add(PlayField.FieldGrid);
        }

        public ICommand? ButtonClickCommand { get;}
        
        private void OnClicked(string NameOfElement)
        { RBIsClicked = NameOfElement;}

        public string RBIsClicked
        {
            get { return _RBIsClicked;}
            set 
            { 
                _RBIsClicked = value;
                if (_RBIsClicked == "RBSize4") { SizeOfField = 4; Notify(); return; }
                if (_RBIsClicked == "RBSize8") { SizeOfField = 8; Notify(); return; }
                if (_RBIsClicked == "RBSize16") { SizeOfField = 16; Notify(); return; }
                MessageBox.Show("Ошибка в названии радиобокса!");
                _RBIsClicked = "RBSize4";
                SizeOfField = 4; Notify();
            }
        }
        public int SizeOfField
        {
            get { return _size_of_field; }
            set 
            {
                if (value == 4 || value == 8 || value == 16)
                    _size_of_field = value;
                else _size_of_field = 4;
                PlayField = new MainFieldGrid(_size_of_field);
                PlayFieldContainer.Clear();
                PlayFieldContainer.Add(PlayField.FieldGrid);
                Notify();
            }
        }
        public ObservableCollection<Grid> PlayFieldContainer
        {
            get { return _playFieldContainer; }
            set 
            {
                _playFieldContainer = value; 
                Notify();
            }
        }
    }
}