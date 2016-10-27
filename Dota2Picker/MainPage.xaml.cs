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
using System.Threading.Tasks;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Dota2Picker
{
    public sealed partial class MainPage : Page
    {
        
        
        int x1, x2;
        bool ShowHideSearchBox = false;
        public MainPage()
        {
            this.InitializeComponent();
            InitializeUi();

            MainGrid.ManipulationMode = ManipulationModes.TranslateRailsX;

            CheckDeviceOrientation();
            Window.Current.SizeChanged += CheckDeviceOrientation;
            UpdateGridViewItems(BaseViewObject.bvoInstance.lastHeroView);

        }

        #region ProgressRing
        private void EnableProgressRing()
        {
            progressRing.IsActive = true;
            progressRing.Visibility = Visibility.Visible;
        }
        private void DisableProgressRing()
        {
            progressRing.IsActive = false;
            progressRing.Visibility = Visibility.Collapsed;
        }
        #endregion
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            IconsListBox.SelectionChanged -= IconsListBox_SelectionChanged;
            IconsListBox.SelectedIndex = BaseViewObject.bvoInstance.lastHeroView;
            IconsListBox.SelectionChanged += IconsListBox_SelectionChanged;
            UpdateGridViewItems(BaseViewObject.bvoInstance.lastHeroView);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            Window.Current.SizeChanged -= CheckDeviceOrientation;
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Hero selectedHero;
            selectedHero = (Hero)e.ClickedItem;

            this.Frame.Navigate(typeof(HeroPage), selectedHero);
        }

        private void SettingsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Temporary
            if (SettingsListBox.SelectedIndex == 1)
            {
                this.Frame.Navigate(typeof(SettingsPage));
            }
        }

        private void IconsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BaseViewObject.bvoInstance.lastHeroView = IconsListBox.SelectedIndex;
            UpdateGridViewItems(BaseViewObject.bvoInstance.lastHeroView);
            
        }

        private async void UpdateGridViewItems ( int idx)
        {
            EnableProgressRing();
            switch (idx)
            {
                case 0:
                    gridViewHeroes.ItemsSource = await DataBaseConnector.dbInstance.getAllHeroes();
                    MySplitView.IsPaneOpen = false;
                    break;
                case 1:
                    gridViewHeroes.ItemsSource = await DataBaseConnector.dbInstance.getHeroesByStrength();
                    MySplitView.IsPaneOpen = false;
                    break;
                case 2:
                    gridViewHeroes.ItemsSource = await DataBaseConnector.dbInstance.getHeroesByAgility();
                    MySplitView.IsPaneOpen = false;
                    break;
                case 3:
                    gridViewHeroes.ItemsSource = await DataBaseConnector.dbInstance.getHeroesByIntelligence();
                    MySplitView.IsPaneOpen = false;
                    break;
                default:
                    break;
            }
            DisableProgressRing();
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
