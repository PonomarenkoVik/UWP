using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TetrisInterfaces;
using TetrisInterfaces.Enum;
using TetrisLogic.Classes;
using TetrisWPF.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWP1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly ITetrisLogic _model;
        private DispatcherTimer _timer;


        public MainPage()
        {
            _model = new TetrisGameBoard(_width, _height);
            _timer = new DispatcherTimer();
            _timer.Tick += Step;
            Window.Current.CoreWindow.KeyDown += MainPage_OnKeyDown;
            _model.UpdateEvent += Update;
            _model.GameOverEvent += GameOver;
            _model.SoundEvent += Sound;
            _model.VelocityChangeEvent += VelocityChanged;
            this.InitializeComponent();
        }

        private void MainPage_OnKeyDown(CoreWindow window, KeyEventArgs e)
        {
            Direction dir = Direction.Empty;
            switch (e.VirtualKey)
            {
                case Windows.System.VirtualKey.Down:
                    dir = Direction.Down;
                    break;
                case Windows.System.VirtualKey.Left:
                    dir = Direction.Left;
                    break;
                case Windows.System.VirtualKey.Right:
                    dir = Direction.Right;
                    break;
                case Windows.System.VirtualKey.Space:
                    if (_timer.IsEnabled == true)
                    {
                        _model.Turn();
                    }
                    break;
                case Windows.System.VirtualKey.P:
                    PauseGame_Click(null, null);
                    break;
            }
            if (dir != Direction.Empty && _timer.IsEnabled == true)
            {
                _model.Move(dir);
            }
        }

      

        private void StartClick(object sender, RoutedEventArgs e)
        {
            SetMenuItemStatus(true);
            _model.Start();
            gameBoard.TabFocusNavigation = KeyboardNavigationMode.Cycle;
            _timer.Start();
        }

        private void StopClick(object sender, RoutedEventArgs e)
        {
            GameOver();
        }


        private const int _width = 10;
        private const int _height = 20;



        #region Methods by subscription

        private void Sound(object sender, SoundEventArg arg)
        {
            //TetrisWPF.Sound.Play(arg.Sound);
        }

        private void GameOver()
        {
            _timer.Stop();
            SetMenuItemStatus(false);
        }

        private void Update(object sender, ShowEventArg arg)
        {
            if (arg != null)
            {
                LevelLine.Text = (arg.Level + 1).ToString();
                ScoreLine.Text = arg.BurnedLine.ToString();
                ScoreText.Text = arg.Score.ToString();

                DrawGameBoard(arg);
                DrawNextFigure(arg);
            }
        }

        private void VelocityChanged(object obj, VelocChangedEventArg arg)
        {
            _timer.Interval = new TimeSpan(0, 0, 0, 0, (int)(600 / arg.Vel));
        }

        private void Step(object sender, object args)
        {
            _model.Step();
        }

        #endregion



        #region  Control



        private void PauseGame_Click(object sender, RoutedEventArgs e)
        {
            if (_timer.IsEnabled)
            {
                SetMenuItemStatus(false);
                _timer.Stop();
            }
            else
            {
                SetMenuItemStatus(true);
                _timer.Start();
            }
        }


        private void SetMenuItemStatus(bool status)
        {
            
            StopGameItem.IsEnabled = status;
            StartGameItem.IsEnabled = !status;
            PauseItem.IsEnabled = status;
            InformationItem.IsEnabled = !status;
            OpenGameItem.IsEnabled = !status;
            SaveGameItem.IsEnabled = !status;
            SaveOptionsItem.IsEnabled = !status;

        }

        #endregion



        #region Showing


        private void DrawNextFigure(ShowEventArg arg)
        {
            if (arg.NextFigure == null)
                return;

            NextFigureBoard.Children.Clear();
            for (byte i = 0; i < arg.NextFigure.GetLength(1); i++)
            {
                for (byte j = 0; j < arg.NextFigure.GetLength(0); j++)
                {
                    if (arg.NextFigure[j, i] != null)
                    {
                        SolidColorBrush color = View.GetColor(arg.NextFigure[j, i].Col);
                        NextFigureBoard.Children.Add(View.CreateRectangle(color, j, i));
                    }
                }
            }          
        }

        private void DrawGameBoard(ShowEventArg arg)
        {
            if (arg.Board == null)
                return;

            gameBoard.Children.Clear();
            for (byte i = 0; i < _height; i++)
            {
                for (byte j = 0; j < _width; j++)
                {
                    if (arg.Board[j, i] != null)
                    {
                        SolidColorBrush color = View.GetColor(arg.Board[j, i].Col);
                        gameBoard.Children.Add(View.CreateRectangle(color, j, i));
                    }
                }
            }          
        }

        #endregion
    }
}
