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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Dota2Picker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        bool ShowHideSearchBox = false;
        public MainPage()
        {
            this.InitializeComponent();
            InitializeUi();
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

        private void SearchBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            //if (string.IsNullOrWhiteSpace(this.SearchBox.Text))
            //{
            //    ClearSearchBtn.Visibility = Visibility.Collapsed;
            //}
            //else
            //{
            //    ClearSearchBtn.Visibility = Visibility.Visible;
            //}
        }

        private void ClearSearchBtn_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = String.Empty;
        }
    }


}
