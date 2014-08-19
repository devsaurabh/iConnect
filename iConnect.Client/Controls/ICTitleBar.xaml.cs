using System;
using System.Windows;
using System.Windows.Controls;

namespace iConnect_Client.Controls
{

    /// <summary>
    /// Interaction logic for ICTitleBar.xaml
    /// </summary>
    /// 
    public partial class ICTitleBar : UserControl
    {
        public ICTitleBar()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Application.Current.MainWindow.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
