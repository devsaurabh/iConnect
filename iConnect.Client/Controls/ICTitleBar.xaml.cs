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
        public event EventHandler CloseClick;
        public event EventHandler Drag;
        
        public ICTitleBar()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            EventHandler handler = Drag;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EventHandler handler = CloseClick;
            if (handler != null) handler(this, EventArgs.Empty);   
        }
    }
}
