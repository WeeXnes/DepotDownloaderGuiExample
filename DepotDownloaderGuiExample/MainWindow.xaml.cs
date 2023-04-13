using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DepotDownloaderGuiExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Intercepting DepotDownloaders Output and Input
            //Display current download progress in ProgressBar (Invoke needed cause its being accessed from a background worker)
            DepotDownloaderLib.onConsoleOutput += (f, s) =>
            {
                Dispatcher.Invoke(() =>
                {
                    downloadProgress.Value = f;
                });
            };
            //Handling the request for the 2FA code
            DepotDownloaderLib.onConsoleInput += () =>
            {
                string inputRead = (string)Dispatcher.Invoke(() =>
                {
                    //Creating a InputBox window to get user input for the 2FA Code
                    return new InputBox("Enter 2FA Code").ShowDialog();
                });
                return inputRead;
            };
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            List<DownloaderArgument> arguments = new List<DownloaderArgument>();
            arguments.Add(new DownloaderArgument(ArgType.AppId, appId.Text));
            arguments.Add(new DownloaderArgument(ArgType.DepotId, depotId.Text));
            arguments.Add(new DownloaderArgument(ArgType.ManifestId, manifestId.Text));
            arguments.Add(new DownloaderArgument(ArgType.Username, loginName.Text));
            arguments.Add(new DownloaderArgument(ArgType.Password, loginPassword.Text));
            arguments.Add(new DownloaderArgument(ArgType.Directory, directory.Text));
            arguments.Add(new DownloaderArgument(ArgType.MaxDownloads, "10"));
            DepotDownloaderLib.StartDownload(arguments, true);
        }
    }
}