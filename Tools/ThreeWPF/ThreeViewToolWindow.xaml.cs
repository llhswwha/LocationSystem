// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Helix Toolkit">
//   Copyright (c) 2014 Helix Toolkit contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;

namespace ThreeViewTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ThreeViewToolWindow
    {
        public ThreeViewToolWindow()
        {
            this.InitializeComponent();
        }

        private void MenuTest_OnClick(object sender, RoutedEventArgs e)
        {
            MainViewModel1.DrawPoint(0, 0, 0);

            //ModelVisual3D1.Content = MainViewModel1.Model;
        }

        private void MenuClear_OnClick(object sender, RoutedEventArgs e)
        {
            MainViewModel1.Clear();
        }

        public MainViewModel ModelContent
        {
            get { return MainViewModel1; }
        }

        private void ShowPoints_OnClick(object sender, RoutedEventArgs e)
        {
            MainViewModel1.ShowPoints();
        }

        private void ThreeViewToolWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            MainViewModel1.ShowPoints();
        }

        private void MenuInit_OnClick(object sender, RoutedEventArgs e)
        {
            MainViewModel1.Reset();
        }
    }
}