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
using CefSharp;
using CefSharp.Wpf;

namespace Pulse_Ignite_WPF_Browser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int tabCount = 0;

        TabItem currentTabItem = null;
        ChromiumWebBrowser currentBrowserShowing = null;

        public MainWindow()
        {
            InitializeComponent();

            DataTransfer dataTransfer = new DataTransfer();
            dataTransfer.CreateXmlFile();
        }

        private void newTabMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TabItem tabItem = new TabItem();
            ChromiumWebBrowser browser = new ChromiumWebBrowser();
            browser.Name = "browser_" + tabCount;

            tabControl.Items.Add(tabItem);
            tabItem.Name = "tab_" + tabCount;
            tabCount++;

            tabItem.Content = browser;

            browser.Address = "https://www.google.co.uk";

            tabItem.Header = "Blank Page (" + tabCount + ")";

            tabControl.SelectedItem = tabItem;

            currentTabItem = tabItem;
            currentBrowserShowing = browser;
            browser.Loaded += FinishedLoadingWebPage;
            
        }

        private void FinishedLoadingWebPage(object sender, RoutedEventArgs e)
        {
            var sndr = sender as ChromiumWebBrowser;

            if(currentTabItem != null)
            {
                string removeHttp = sndr.Address.Replace("http://www.", "");
                string host = removeHttp.Replace("https://www.", "");

                currentTabItem.Header = host;
            }
        }

        private void bkBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currentBrowserShowing.CanGoBack)
            {
                currentBrowserShowing.Back(); 
            }
        }

        private void fwdBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currentBrowserShowing.CanGoForward)
            {
                currentBrowserShowing.Forward(); 
            }
        }

        private void rfshBtn_Click(object sender, RoutedEventArgs e)
        {
            currentBrowserShowing.Reload();
        }

        private void AddressBar_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                Search();
            }
        }

        private void Search()
        {
            if (currentBrowserShowing != null && AddressBar.Text != string.Empty)
            {
                currentBrowserShowing.Address = "https://www.google.com/search?q=" + AddressBar.Text;
            }
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabControl.SelectedItem != null)
            {
                currentTabItem = tabControl.SelectedItem as TabItem; 
            }

            if (currentTabItem != null)
            {
                currentBrowserShowing = currentTabItem.Content as ChromiumWebBrowser; 
            }
        }

        private void SettingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new SettingsWindow();
            settings.ShowDialog();
        }

        private void closeTabMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if(tabCount > 0 && currentTabItem != null)
            {
                tabControl.Items.Remove(currentTabItem);
            }
        }

    }
}
