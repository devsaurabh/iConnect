using System;
using System.Windows.Input;

namespace iConnect.Client.Controls
{
    public partial class TitleBar
    {
        private string _title;

        #region Event Handlers

        public event EventHandler Close;
        public event EventHandler Drag;

        #endregion

        #region Public Members

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                TxtBlock.Text = _title;
            }
        }

        #endregion

        #region Ctor

        public TitleBar()
        {
            InitializeComponent();
        }
        
        #endregion

        #region Events

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var handler = Drag;
            if (Close != null)
                handler(this, EventArgs.Empty);
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var handler = Close;
            if (Close != null)
                handler(this, EventArgs.Empty);
        } 

        #endregion
    }
}





