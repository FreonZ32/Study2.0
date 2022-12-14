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
using System.Collections.ObjectModel;
using System.Drawing;
using _2048Game.Model;
using System.Security.Policy;
using System.Windows.Media;
using System.IO;
using System.Security.Cryptography;
using System.Reflection.Metadata;

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
        string _WBtnPress;
        int _score;
        int _maxScore;
        ObservableCollection<Grid> _playFieldContainer;
        MainFieldGrid PlayField;
        StreamReader streamsreader;

        public MainWindowViewModel()
        {
            RBCLickCommand = new ButtonClickCommand(RBOnClick);
            WinBtnPress = new ButtonClickCommand(WindowOnButtonPress);
            _playFieldContainer = new ObservableCollection<Grid>();
            PlayField = new MainFieldGrid(_size_of_field);
            PlayFieldContainer.Add(PlayField.FieldGrid);
            SettingsReader();
        }

        public Action CloseAction { get; set; }
        public ICommand? RBCLickCommand { get;}
        public ICommand? WinBtnPress { get;}

        public ICommand Exit
        {
            get
            {
                return new ButtonCommands((obj) =>
                {
                    CloseAction();
                });
            }
        }
        public ICommand NewGame
        {
            get
            {
                return new ButtonCommands((obj) =>
                {
                    SizeOfField = 4;
                    PlayField = new MainFieldGrid(_size_of_field);
                    PlayFieldContainer.Clear();
                    PlayFieldContainer.Add(PlayField.FieldGrid);
                    SettingsReader();
                });
            }
        }


        private void RBOnClick(string NameOfElement)
        { RBIsClicked = NameOfElement; SettingsWriter(); }
        private void WindowOnButtonPress(string NameOfButton)
        { WBtnPress = NameOfButton; }

        public string RBIsClicked
        {
            get { return _RBIsClicked;}
            set 
            { 
                _RBIsClicked = value;
                if (_RBIsClicked == "RBSize4") { SizeOfField = 4;Score = 0; Notify(); }
                else if (_RBIsClicked == "RBSize8") { SizeOfField = 8; Score = 0; Notify(); }
                else if (_RBIsClicked == "RBSize16") { SizeOfField = 16; Score = 0; Notify(); }
                else
                {
                    MessageBox.Show("Ошибка в названии радиобокса!");
                    _RBIsClicked = "RBSize4";
                    SizeOfField = 4;
                    Score = 0; Notify();
                }
            }
        }
        public string WBtnPress
        {
            get { return _WBtnPress; }
            set
            {
                _WBtnPress = value;
                if (_WBtnPress == "BtnUP") PlayField.MoveToDir("Up");
                if (_WBtnPress == "BtnDOWN") PlayField.MoveToDir("Down");
                if (_WBtnPress == "BtnLEFT") PlayField.MoveToDir("Left");
                if (_WBtnPress == "BtnRIGHT") PlayField.MoveToDir("Right");
                if (PlayField.NextStepPlateCreator() == false) 
                {
                    MessageBox.Show("Вы проиграли!");
                    if (NewGame?.CanExecute(SizeOfField) == true)
                        NewGame.Execute(SizeOfField);
                }
                Score = PlayField.MaxNumber();
                if (Score == 2048) MessageBox.Show("Вы выиграли!");
                PlayField.BrushForColor();
                if (Score > MaxScore) { MaxScore = Score; SettingsWriter(); }
                Notify();
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
        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                Notify();
            }
        }
        public int MaxScore
        {
            get { return _maxScore; }
            set 
            { 
                _maxScore = value;
                Notify(); 
            }
        }
        public void SettingsReader()
        {
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            string path = "";
            path = System.IO.Path.Combine(exePath, "Settings\\Sett.txt");
            streamsreader = new StreamReader(path);
            String line;
            try
            {
                line = streamsreader.ReadLine();
                RBIsClicked = line;
                line = streamsreader.ReadLine();
                MaxScore = Convert.ToInt32(line);
                streamsreader.Close();
            }
            catch(Exception ex)
            {
                streamsreader.Close();
                MessageBox.Show("Сбой файла настройки" + ex);
                MaxScore = 0;
            }
        }
        public void SettingsWriter()
        {
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            string path = "";
            path = System.IO.Path.Combine(exePath,"Settings\\Sett.txt");
            File.WriteAllText(path, "");
            StreamWriter streamswriter = new StreamWriter(path,true);
            streamswriter.WriteLine(_RBIsClicked);
            streamswriter.WriteLine(_maxScore);
            streamswriter.Close();
        }
    }
}
