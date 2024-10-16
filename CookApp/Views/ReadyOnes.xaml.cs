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
using CookApp.DB;
using System.Diagnostics;
using CookApp;

namespace CookApp.Views
{
    /// <summary>
    /// Interaction logic for ReadyOnes.xaml
    /// </summary>
    public partial class ReadyOnes : UserControl
    {
        public class DishOrderInfo
        {
            public int TableNum { get; set; }
            public DateTime Date { get; set; }
            public string Name { get; set; }
            public int DishInOrderId { get; set; }
        }

        public ReadyOnes()
        {
            InitializeComponent();
        }

        private async void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            DishOrderInfo item = (DishOrderInfo)button.DataContext;

            using (HttpClient client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(item);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await client.PutAsync($"{AppSettings.BaseUrl}api/DishInOrder", content);
                
                if(responseMessage.IsSuccessStatusCode)
                {
                    GetDataFromDBtoGrid();
                    MessageBox.Show("Zatwierdzono");
                }
                else
                {
                    MessageBox.Show("API Server Error");
                }
            }
        }

        private void gridReadyOnes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void UseControl_Loaded(object sender, RoutedEventArgs e)
        {
            GetDataFromDBtoGrid();
        }

        public class EmployeeId
        {
            public int employeeId;
        }

        private async void GetDataFromDBtoGrid()
        {
            using(HttpClient client = new HttpClient()) 
            {

                int employeeId = SessionManager.Instance.loggedInEmployeeId;
                Console.WriteLine(employeeId);

                HttpResponseMessage responseMessage = await client.GetAsync($"{AppSettings.BaseUrl}api/DishInOrder/byId/{employeeId}");
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonstring = await responseMessage.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<List<DishOrderInfo>>(jsonstring);
                    gridReadyOnes.ItemsSource = result;
                }
                else if (responseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    MessageBox.Show("Nie ma nic do odbioru");
                }
                else
                {
                    MessageBox.Show("Server Error");
                    Console.WriteLine(await responseMessage.Content.ReadAsStringAsync());
                }
            } 
        }
    }
}
