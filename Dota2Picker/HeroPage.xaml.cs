using Dota2Picker.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using Dota2Picker.Objects;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.ViewManagement;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Dota2Picker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HeroPage : Page
    {

        private static int heroID;
        private string imagePath;

        public HeroPage()
        {
            this.InitializeComponent();
            this.Loaded += ChallengePage_Loaded;

            MainGrid.ManipulationMode = ManipulationModes.TranslateRailsX;

            CheckDeviceOrientation();
            Window.Current.SizeChanged += CheckDeviceOrientation;
        }

        void ChallengePage_Loaded(object sender, RoutedEventArgs e)
        {
            Thickness margin = heroTitle.Margin;
            margin.Left = -(int)HamburgerButton.ActualWidth;
            heroTitle.Margin = margin;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Hero ChosenHero = (Hero)e.Parameter;
            heroID = ChosenHero.id;

            imagePath = ChosenHero.imgPath;
            heroTitle.Text = ChosenHero.name;

        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void IconsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //_lastIndexForHeroesView = IconsListBox.SelectedIndex;
            //UpdateGridViewItems(_lastIndexForHeroesView);

        }

        private void CheckDeviceOrientation(object sender, WindowSizeChangedEventArgs e)
        {
            Thickness margin = heroPortrait.Margin;

            if (ApplicationView.GetForCurrentView().Orientation == ApplicationViewOrientation.Landscape)
            {
                MySplitView.CompactPaneLength = Constants.hamburgerMenuWith;
                HamburgerButton.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                SolidColorBrush backgroundBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 43, 43, 43));
                HamburgerButton.Background = backgroundBrush;


                margin.Left = Constants.heroPortraitLandOffset;
                heroPortrait.Margin = margin;

            }
            else
            {
                MySplitView.CompactPaneLength = 0;
                HamburgerButton.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
                HamburgerButton.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);

                margin.Left = Constants.heroPortraitOffset;
                heroPortrait.Margin = margin;

            }
        }

        private void CheckDeviceOrientation()
        {
            Thickness margin = heroPortrait.Margin;

            if (ApplicationView.GetForCurrentView().Orientation == ApplicationViewOrientation.Landscape)
            {
                MySplitView.CompactPaneLength = Constants.hamburgerMenuWith;
                HamburgerButton.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                HamburgerButton.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 43, 43, 43));

                margin.Left = Constants.heroPortraitLandOffset;
                heroPortrait.Margin = margin;

            }
            else
            {
                MySplitView.CompactPaneLength = 0;
                HamburgerButton.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
                HamburgerButton.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);

                margin.Left = Constants.heroPortraitOffset;
                heroPortrait.Margin = margin;
            }
        }

    }
}
