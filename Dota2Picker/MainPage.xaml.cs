using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;
using Dota2Picker.Objects;
using Dota2Picker.Models;
using Windows.Devices.Sensors;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Dota2Picker
{
    public sealed partial class MainPage : Page
    {
        private DataBaseConnector db = new DataBaseConnector();

        bool ShowHideSearchBox = false;
        public MainPage()
        {
            this.InitializeComponent();
            InitializeUi();

            CheckDataBase();
            CheckDeviceOrientation();
            Window.Current.SizeChanged += CheckDeviceOrientation;

            /*--------------------- Only For Debug -------------------*/
            //List<Hero> AllHeroes = db.getAllHeroes();

            //List<Hero> EsIsWeakAgainst = db.GetWeakAgainst(1); // 1 means hero index. It depends on user choice

            //List<Hero> EsIsStrongAgainst = db.GetStrongAgainst(1); // 1 means hero index. It depends on user choice
            /* ------------------------------------------------------ */

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

        private void IconsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
            var orientation = ApplicationView.GetForCurrentView().Orientation;
            if (orientation == ApplicationViewOrientation.Landscape)
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
            var orientation = ApplicationView.GetForCurrentView().Orientation;
            if (orientation == ApplicationViewOrientation.Landscape)
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

        private async void CheckDataBase()
        {
            await db.CopyDatabase();
        }
    }
}
