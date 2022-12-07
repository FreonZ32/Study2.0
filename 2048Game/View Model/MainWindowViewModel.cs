using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;

namespace _2048Game.View_Model
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void Notify([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        int _size_of_field;  //размер MainPlayField (изменяется в RBSize_Click)
        string _RBIsClicked;
        public MainWindowViewModel()
        {
            SizeOfField = 4;
            ButtonClickCommand = new ButtonClickCommand(OnClicked);
        }

        public ICommand? ButtonClickCommand { get;}
        public ICommand RBSize_Click
        {
            get
            {
                return new ButtonCommands((obj) =>
                { 
                });
            }
        }

        private void OnClicked(string NameOfElement)
        { RBIsClicked = NameOfElement;}


        public string RBIsClicked
        {
            get { return _RBIsClicked;}
            set 
            { 
                _RBIsClicked = value;
                if (_RBIsClicked == "RBSize4") { SizeOfField = 4; Notify(); return; }
                if (_RBIsClicked == "RBSize8") { SizeOfField = 4; Notify(); return; }
                if (_RBIsClicked == "RBSize16") { SizeOfField = 4; Notify(); return; }
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
                Notify();
            }
        }

    }
}
