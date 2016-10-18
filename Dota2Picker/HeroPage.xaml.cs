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
using System.Threading.Tasks;

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

        private static string heroImgBasePath = "Resources/";

        private string heroAttPath;
        private string heroRangePath;

        private int x1, x2;


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

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Hero ChosenHero = (Hero)e.Parameter;
            heroID = ChosenHero.id;

            imagePath = ChosenHero.imgPath;
            heroTitle.Text = ChosenHero.name;

            SetHeroAttribute(ChosenHero.attribute_id);
            SetHeroRange(ChosenHero.type_name);

            EnableProgressRing();
            heroRoles.Text = await Task.Run( () => GetHeroRoles(ChosenHero.id));
            GetWeakAgainstHeroes();
            GetStrongAgainstHeroes();
            DisableProgressRing();


        }
        #region ProgressRing
        private void EnableProgressRing()
        {
            progresHeroRoles.IsActive = true;
            progresHeroRoles.Visibility = Visibility.Visible;
        }
        private void DisableProgressRing()
        {
            progresHeroRoles.IsActive = false;
            progresHeroRoles.Visibility = Visibility.Collapsed;
        }
        #endregion

        public void GetWeakAgainstHeroes()
        {
            IsWeakList.ItemsSource = DataBaseConnector.dbInstance.GetWeakAgainst(heroID);
        }

        public void GetStrongAgainstHeroes()
        {
            IsStrongList.ItemsSource = DataBaseConnector.dbInstance.GetStrongAgainst(heroID);
        }

        private string GetHeroRoles(int heroIndex)
        {
            List<Role> heroRoles = DataBaseConnector.dbInstance.GetHeroRoles(heroIndex);

            string roles = "";

            for (int i = 0; i < heroRoles.Count; i++)
            {
                roles += heroRoles[i].name + " ";
            }
            return roles;
        }

        private void SetHeroRange(string rangeType)
        {
            heroRangeText.Text = rangeType;
            heroRangePath = heroImgBasePath + rangeType + ".png";
        }

        private void SetHeroAttribute(int attributeId)
        {
            switch (attributeId)
            {
                case 1: heroAttributeText.Text = "Strength";
                        heroAttPath = heroImgBasePath + "strength.png";
                    break;
                case 2: heroAttributeText.Text = "Agility";
                        heroAttPath = heroImgBasePath + "agility.png";
                    break;
                case 3: heroAttributeText.Text = "Intelligence";
                        heroAttPath = heroImgBasePath + "intelligence.png";
                        heroAttributeText.FontSize = 16;
                    break;
            }
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
            Thickness staticTxtMargin = IsWeakStaticTxt.Margin;
            Thickness listMargin = IsWeakList.Margin;

            if (ApplicationView.GetForCurrentView().Orientation == ApplicationViewOrientation.Landscape)
            {
                MySplitView.CompactPaneLength = Constants.hamburgerMenuWith;
                HamburgerButton.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                SolidColorBrush backgroundBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 43, 43, 43));
                HamburgerButton.Background = backgroundBrush;

                /* LEFT hamburger offset margins */
                margin.Left = Constants.heroPortraitLandOffset;
                heroPortrait.Margin = margin;
                heroRoles.Margin = margin;

                staticTxtMargin.Left = Constants.heroPortraitLandOffset;
                IsWeakStaticTxt.Margin = staticTxtMargin;
                IsStrongStaticTxt.Margin = staticTxtMargin;

                listMargin.Left = Constants.ListLandOffset;
                IsWeakList.Margin = listMargin;
                IsStrongList.Margin = listMargin;
            }
            else
            {
                MySplitView.CompactPaneLength = 0;
                HamburgerButton.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
                HamburgerButton.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);

                /* LEFT hamburger offset margins */
                margin.Left = Constants.heroPortraitOffset;
                heroPortrait.Margin = margin;
                heroRoles.Margin = margin;

                staticTxtMargin.Left = Constants.heroPortraitOffset;
                IsWeakStaticTxt.Margin = staticTxtMargin;
                IsStrongStaticTxt.Margin = staticTxtMargin;

                staticTxtMargin.Left = Constants.ListPortraitOff;
                IsWeakList.Margin = staticTxtMargin;
                IsStrongList.Margin = staticTxtMargin;
            }
        }

        private void CheckDeviceOrientation()
        {
            Thickness margin = heroPortrait.Margin;
            Thickness staticTxtMargin = IsWeakStaticTxt.Margin;
            Thickness listsMargin = IsWeakList.Margin;

            if (ApplicationView.GetForCurrentView().Orientation == ApplicationViewOrientation.Landscape)
            {
                MySplitView.CompactPaneLength = Constants.hamburgerMenuWith;
                HamburgerButton.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                HamburgerButton.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 43, 43, 43));

                /* LEFT hamburger offset margins */
                margin.Left = Constants.heroPortraitLandOffset;
                heroPortrait.Margin = margin;
                heroRoles.Margin = margin;

                staticTxtMargin.Left = Constants.heroPortraitLandOffset;
                IsWeakStaticTxt.Margin = staticTxtMargin;
                IsStrongStaticTxt.Margin = staticTxtMargin;

                listsMargin.Left = Constants.ListLandOffset;
                IsWeakList.Margin = listsMargin;
                IsStrongList.Margin = listsMargin;
            }
            else
            {
                MySplitView.CompactPaneLength = 0;
                HamburgerButton.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
                HamburgerButton.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);

                /* LEFT hamburger offset margins */
                margin.Left = Constants.heroPortraitOffset;
                heroPortrait.Margin = margin;
                heroRoles.Margin = margin;

                staticTxtMargin.Left = Constants.heroPortraitOffset;
                IsWeakStaticTxt.Margin = staticTxtMargin;
                IsStrongStaticTxt.Margin = staticTxtMargin;

                listsMargin.Left = Constants.ListPortraitOff;
                IsWeakList.Margin = listsMargin;
                IsStrongList.Margin = listsMargin;
            }
        }

        private void MainGrid_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            x1 = (int)e.Position.X;
        }

        private void MainGrid_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            x2 = (int)e.Position.X;
            Hero nextHero = new Hero();
            if ( x2 < x1 && (x1-x2) > 100 && heroID < DataBaseConnector.AllHeroesList.Last().id)
            {
                nextHero = DataBaseConnector.AllHeroesList.Find(item => item.id == (heroID + 1));
                this.Frame.Navigate(typeof(HeroPage), nextHero);
            }
            else if ( x2 > x1 && ( x2 - x1 ) > 100 && heroID > DataBaseConnector.AllHeroesList.First().id)
            {
                nextHero = DataBaseConnector.AllHeroesList.Find(item => item.id == (heroID - 1));
                this.Frame.Navigate(typeof(HeroPage), nextHero);
            }
        }
    }
}
