using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FinalP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StartupDialog : ContentDialog
    {
        public Action<string, string> OnInputData;

        public string PlayerName => NameBox.Text.Trim();
        public string IpAddress => IpBox.Text.Trim();
        public int Port => int.TryParse(PortBox.Text.Trim(), out var p) ? p : 0;

        public StartupDialog()
        {
            this.InitializeComponent();
        }

        // Add this method to resolve CS1061
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ErrorText.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(PlayerName))
            {
                ErrorText.Text = "Name is required.";
                args.Cancel = true;
                return;
            }

            if (string.IsNullOrWhiteSpace(IpAddress))
            {
                ErrorText.Text = "IP address is required.";
                args.Cancel = true;
                return;
            }

            if (Port <= 0)
            {
                ErrorText.Text = "Port must be a positive number.";
                args.Cancel = true;
                return;
            }
            OnInputData?.Invoke(PlayerName, IpAddress);
            // Add any additional IP/port validation here if you like.
        }
    }
}

