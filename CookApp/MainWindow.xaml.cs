using Azure;
using CookApp.ViewModels;
using CookApp.Views;
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

namespace CookApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            employeeNameTB.Text = SessionManager.Instance.name + " " + SessionManager.Instance.surname;
        }

        private void ReadyOnesBtn(object sender, RoutedEventArgs e)
        {
            DataContext = new ReadyOnesViewModel();
        }

        private void PlaceOrderBtn(object sender, RoutedEventArgs e)
        {
            DataContext = new PlaceOrderViewModel();
        }
        private void ShowOrderBtn(object sender, RoutedEventArgs e)
        {
            DataContext = new ShowOrderViewModel();
        }
        private void LogoutBtn(object sender, RoutedEventArgs e)
        {
            SessionManager.Instance.loggedInEmployeeId = 0;
            Login newWindow = new Login();
            newWindow.Show();
            this.Close();
        }

    }
}
