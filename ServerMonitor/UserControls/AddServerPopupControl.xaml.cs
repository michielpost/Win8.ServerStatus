using ServerMonitor.Services.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ServerMonitor.UserControls
{
    public sealed partial class AddServerPopupControl : UserControl
    {
         private ContentPopupShowAsyncOperation _action;


         public AddServerPopupControl()
        {
            this.InitializeComponent();
        }


        public IAsyncOperation<string> ShowAsync()
        {
            if (_action != null)
                throw new InvalidOperationException();


            var window = Window.Current;


            Canvas.SetLeft(Popup, 0);
            Canvas.SetTop(Popup, 0);
            Grid.Width = Popup.Width = window.Bounds.Width;
            Grid.Height = Popup.Height = window.Bounds.Height;
            Popup.IsOpen = true;


            return _action = new ContentPopupShowAsyncOperation();
        }


        public void ClosePopup()
        {
            Popup.IsOpen = false;
        }


        public void Close()
        {
            if (_action == null)
                throw new InvalidOperationException();


            ClosePopup();


            _action.Close();
            _action = null;
        }


        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;


            if (button != null)
            {
                _action.SetResult((string)button.Tag);
                Close();
            }
        }


        private void ConfirmNoCloseClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;

            if (button != null)
            {
                bool isValid = CheckIsValid();

                if (isValid)
                {
                    _action.SetResult((string)button.Tag);


                    _action.Close();
                    _action = null;
                    //Close();
                }
                else
                {
                    InvalidWarning.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
            }
        }

        private bool CheckIsValid()
        {
            RemoteServer sr = (RemoteServer)this.DataContext;

            return sr.IsValid();
        }




       
        private void Popup_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            Close();
        }


        private void Grid_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            //Ignore this tap
            e.Handled = true;
        }

        //private void TextBox_KeyUp_1(object sender, KeyRoutedEventArgs e)
        //{
        //    if (e.Key == Windows.System.VirtualKey.Enter)
        //    {
        //        e.Handled = true;

        //        ConfirmNoCloseClick(AddButton, null);
                
        //    }
        //}


    }
    
}
