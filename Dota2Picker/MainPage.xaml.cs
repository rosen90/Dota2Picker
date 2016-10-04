using System;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.ViewManagement;
using Dota2Picker.Objects;
using Dota2Picker.Models;
using Windows.UI.Text;
using System.Collections.Generic;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Dota2Picker
{
    public sealed partial class MainPage : Page
    {
        private DataBaseConnector db = new DataBaseConnector();

        List<Hero> AllHeroesList = new List<Hero>();
        int x1, x2;
        bool ShowHideSearchBox = false;
        public MainPage()
        {
            this.InitializeComponent();
            InitializeUi();
            
            MainGrid.ManipulationMode = ManipulationModes.TranslateRailsX;
            
            //CheckDataBase();
            CheckDeviceOrientation();
            Window.Current.SizeChanged += CheckDeviceOrientation;

            /*--------------------- Only For Debug -------------------*/
            AllHeroesList = db.getAllHeroes();
            txtSize.Text = AllHeroesList.Count.ToString();
            //List<Hero> EsIsWeakAgainst = db.GetWeakAgainst(1); // 1 means hero index. It depends on user choice

            //List<Hero> EsIsStrongAgainst = db.GetStrongAgainst(1); // 1 means hero index. It depends on user choice
            /* ------------------------------------------------------ */

        }

        private void MainGrid_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            x2 = (int)e.Position.X;

            if (ApplicationView.GetForCurrentView().Orientation == ApplicationViewOrientation.Portrait)
            {
                if (x1 < x2 && x1 < (int)HamburgerButton.ActualWidth)
                {
                    MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
                }
                else if (x1 > x2 && x1 < 190 && MySplitView.IsPaneOpen)
                {
                    MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
                }
            }
            else
            {
                if (x1 < x2 && x1 < (int)HamburgerButton.ActualWidth * 2)
                {
                    MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
                }
            }

        }

        private void MainGrid_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            x1 = (int)e.Position.X;
        }

        public void ColorizeStatusBar()
        {
            //Mobile customization
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {

                var statusBar = StatusBar.GetForCurrentView();
                if (statusBar != null)
                {
                    statusBar.BackgroundOpacity = 1;
                    statusBar.BackgroundColor = Windows.UI.Colors.Purple;
                    statusBar.ForegroundColor = Windows.UI.Colors.White;
                }
            }
        }

        private async void InitializeUi()
        {
            ColorizeStatusBar();

            // If we have a phone contract, hide the status bar
            if (ApiInformation.IsApiContractPresent("Windows.Phone.PhoneContract", 1, 0))
            {
                var statusBar = StatusBar.GetForCurrentView();
                await statusBar.HideAsync();
            }
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ShowHideSearchBox = !ShowHideSearchBox;

            if (ShowHideSearchBox)
            {
                SearchBox.Visibility = Visibility.Visible;
            }
            else
            {
                SearchBox.Visibility = Visibility.Collapsed;
            }
        }
        
        private void CheckDeviceOrientation(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            if (ApplicationView.GetForCurrentView().Orientation == ApplicationViewOrientation.Landscape)
            {
                MySplitView.CompactPaneLength = 56;
                HamburgerButton.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                SolidColorBrush backgroundBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 43, 43, 43));
                HamburgerButton.Background = backgroundBrush;
            }
            else
            {
                MySplitView.CompactPaneLength = 0;
                HamburgerButton.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
                HamburgerButton.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);  
            }

        }

        private void CheckDeviceOrientation()
        {
            Thickness margin = InfoMsg.Margin;

            if (ApplicationView.GetForCurrentView().Orientation == ApplicationViewOrientation.Landscape)
            {
                MySplitView.CompactPaneLength = 56;
                HamburgerButton.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                HamburgerButton.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 43, 43, 43));

                margin.Top = 10;
                margin.Left = 62;
                InfoMsg.Margin = margin;

            }
            else
            {
                MySplitView.CompactPaneLength = 0;
                HamburgerButton.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
                HamburgerButton.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);

                margin.Top = 10;
                margin.Left = 25;
                InfoMsg.Margin = margin;
            }
        }
        
    }
}
