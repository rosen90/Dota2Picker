using System;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.ViewManagement;
using Dota2Picker.Objects;
using Dota2Picker.Models;
using Windows.UI.Xaml.Navigation;
using System.Linq;
using System.Collections.Generic;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Dota2Picker
{
    public sealed partial class MainPage : Page
    {
        private static Hero selectedHero;
        
        int x1, x2;
        bool ShowHideSearchBox = false;
        public MainPage()
        {
            this.InitializeComponent();
            InitializeUi();
            gridViewHeroes.ItemsSource = DataBaseConnector.AllHeroesList;

            MainGrid.ManipulationMode = ManipulationModes.TranslateRailsX;

            CheckDeviceOrientation();
            Window.Current.SizeChanged += CheckDeviceOrientation;
            UpdateGridViewItems(BaseViewObject.bvoInstance.lastHeroView);
            
            /*--------------------- Only For Debug -------------------*/
            //AllHeroesList = db.getAllHeroes();

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
        
 
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            selectedHero = (Hero)e.ClickedItem;

            this.Frame.Navigate(typeof(HeroPage), selectedHero);
        }

        private void IconsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BaseViewObject.bvoInstance.lastHeroView = IconsListBox.SelectedIndex;
            UpdateGridViewItems(BaseViewObject.bvoInstance.lastHeroView);
            
        }

        private void UpdateGridViewItems ( int idx)
        {
            switch (idx)
            {
                case 0:
                    gridViewHeroes.ItemsSource = DataBaseConnector.AllHeroesList;
                    MySplitView.IsPaneOpen = false;
                    break;
                case 1:
                    gridViewHeroes.ItemsSource = DataBaseConnector.HeroesByStrengthList;
                    MySplitView.IsPaneOpen = false;
                    break;
                case 2:
                    gridViewHeroes.ItemsSource = DataBaseConnector.HeroesByAgilityList;
                    MySplitView.IsPaneOpen = false;
                    break;
                case 3:
                    gridViewHeroes.ItemsSource = DataBaseConnector.HeroesByIntelligenceList;
                    MySplitView.IsPaneOpen = false;
                    break;
                default:
                    break;
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateGridViewItems(BaseViewObject.bvoInstance.lastHeroView);
            List<Hero> tempList = ((IEnumerable<Hero>)this.gridViewHeroes.ItemsSource).ToList();
            if ( SearchBox.Text.Length != 0)
            {
                tempList = tempList.FindAll( item => item.name.ToLower().Contains(SearchBox.Text.ToLower()));
                gridViewHeroes.ItemsSource = tempList;
            }
        }

        private void CheckDeviceOrientation(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            Thickness margin = gridHeroes.Margin;

            if (ApplicationView.GetForCurrentView().Orientation == ApplicationViewOrientation.Landscape)
            {
                MySplitView.CompactPaneLength = Constants.hamburgerMenuWith;
                HamburgerButton.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                SolidColorBrush backgroundBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 43, 43, 43));
                HamburgerButton.Background = backgroundBrush;

                margin.Left = Constants.gridheroesOffset;
                gridHeroes.Margin = margin;
            }
            else
            {
                MySplitView.CompactPaneLength = 0;
                HamburgerButton.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
                HamburgerButton.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);

                margin.Left = 0;
                gridHeroes.Margin = margin;
            }
        }

        private void CheckDeviceOrientation()
        {
            Thickness margin = gridHeroes.Margin;

            if (ApplicationView.GetForCurrentView().Orientation == ApplicationViewOrientation.Landscape)
            {
                MySplitView.CompactPaneLength = Constants.hamburgerMenuWith;
                HamburgerButton.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                HamburgerButton.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 43, 43, 43));

                margin.Left = Constants.gridheroesOffset;
                gridHeroes.Margin = margin;
            }
            else
            {
                MySplitView.CompactPaneLength = 0;
                HamburgerButton.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
                HamburgerButton.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);

                margin.Left = 0;
                gridHeroes.Margin = margin;
            }
        }
    }
}
