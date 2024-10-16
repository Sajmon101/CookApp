using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using CookApp.Views;
using System.Net;
using CookApp.DB;
using CookApp;

namespace CookApp.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    /// 
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }
        private class LoginInfo
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        private class LoggedEmployeeInfo
        {
            public int Id;
            public string TypeName;
            public string Name;
            public string Surname;
        }

        private async void loginBtn(object sender, RoutedEventArgs e)
        {
            if (loginInput.Text !=null && passwordInput.Password !=null)
            {
                using (HttpClient client = new HttpClient())
                {
                    var loginInfo = new LoginInfo()
                    {
                        UserName = loginInput.Text,
                        Password = passwordInput.Password
                    };

                    var json = JsonConvert.SerializeObject(loginInfo);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    //Console.WriteLine(AppContext.BaseDirectory);
                    HttpResponseMessage responseMessage = await client.PostAsync($"{AppSettings.BaseUrl}api/Login", content);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var jsonstring = await responseMessage.Content.ReadAsStringAsync();
                        var employeeInfo = JsonConvert.DeserializeObject<List<LoggedEmployeeInfo>>(jsonstring);
                        SessionManager.Instance.loggedInEmployeeId = employeeInfo[0].Id;
                        SessionManager.Instance.name = employeeInfo[0].Name;
                        SessionManager.Instance.surname = employeeInfo[0].Surname;

                        if(SessionManager.Instance.loggedInEmployeeId != 0)
                        {
                            if (employeeInfo[0].TypeName == "Kelner")
                            {
                                MainWindow newMainWindow = new MainWindow();
                                newMainWindow.Show();
                                Close();
                            }
                            else
                            {
                                Cooker newCooker = new Cooker();
                                newCooker.Show();
                                Close();
                            }
                        }

                    }
                    else if (responseMessage.StatusCode == HttpStatusCode.NotFound)
                    {
                        MessageBox.Show("Niepoprawne dane logowania");
                    }

                    else MessageBox.Show("Server Error");     
                }
            }
        }
    }
}
