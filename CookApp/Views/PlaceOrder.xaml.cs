using Azure;
using CookApp.DB;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
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
using CookApp;

namespace CookApp.Views
{
    /// <summary>
    /// Interaction logic for PlaceOrder.xaml
    /// </summary>
    public partial class PlaceOrder : UserControl
    {
        public ObservableCollection<string> dishes { get; set; }

        public PlaceOrder()
        {
            InitializeComponent();
            this.DataContext = this; // Set the DataContext
            dishes = new ObservableCollection<string>();
        }

        private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsNumericInput(e.Text))
            {
                e.Handled = true;
            }
        }

        private bool IsNumericInput(string input)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(input, "[0-9]");
        }

        public class IdInfo
        {
            public int dishId {  get; set; }
            public int orderId { get; set; }
            public int employeeId { get; set; }
            public int dishInOrderId { get; set; }
        }        
        public class IdInfo2
        {
            public int employeeId { get; set; }
            public int dishInOrderId { get; set; }
        }

        int dishInOrderId;
        DishInOrder dishInOrder = new DishInOrder();

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tableNumInput.Text) && !string.IsNullOrEmpty(dishComboBox.Text))
            {
                int tableNum = int.Parse(tableNumInput.Text);
                string dishName = dishComboBox.Text;
                Dish dish = null;
                Order order = null;

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage responseMessage5 = await client.GetAsync($"{AppSettings.BaseUrl}api/Dishes/{dishName}");

                    if (responseMessage5.IsSuccessStatusCode)
                    {
                        var jsonstring = await responseMessage5.Content.ReadAsStringAsync();
                        dish = JsonConvert.DeserializeObject<Dish>(jsonstring);
                    }
                }

                //get do orders żeby zyskać id zamówienia dla danego stolika (gdzie IsArch jest false!). Jeżeli zwróci null to robisz put żeby dodać
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage responseMessage = await client.GetAsync($"{AppSettings.BaseUrl}api/Order/{tableNum}");

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var jsonstring = await responseMessage.Content.ReadAsStringAsync();
                        order = JsonConvert.DeserializeObject<Order>(jsonstring);
                    }
                    else if (responseMessage.StatusCode == HttpStatusCode.NotFound)
                    {
                        //post order
                        var newOrder = new Order();
                        newOrder.TableNum = tableNum;
                        string jsonContent = JsonConvert.SerializeObject(newOrder);
                        HttpResponseMessage responseMessage2 = await client.PostAsync($"{AppSettings.BaseUrl}api/Order", new StringContent(jsonContent, Encoding.UTF8, "application/json"));
                        var responseContent = await responseMessage2.Content.ReadAsStringAsync();
                        order = JsonConvert.DeserializeObject<Order>(responseContent);
                    }
                }

                using (HttpClient client = new HttpClient())
                {
                    //put do dishInOrders
                    var idInfo = new IdInfo
                    {
                        dishId = dish.Id,
                        orderId = order.Id,
                        employeeId = SessionManager.Instance.loggedInEmployeeId,
                        dishInOrderId = 0
                    };
                    string jsonContent1 = JsonConvert.SerializeObject(idInfo);
                    HttpResponseMessage responseMessage = await client.PostAsync($"{AppSettings.BaseUrl}api/DishInOrder", new StringContent(jsonContent1, Encoding.UTF8, "application/json"));
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var jsonstring = await responseMessage.Content.ReadAsStringAsync();
                        dishInOrderId = JsonConvert.DeserializeObject<int>(jsonstring);
                    }
                }

                //add to executives
                using (HttpClient client = new HttpClient())
                {
                    //put executives (cooker = null) i waiter = 
                    var newidInfo2 = new IdInfo2
                    {
                        dishInOrderId = dishInOrderId,
                        employeeId = SessionManager.Instance.loggedInEmployeeId
                    };

                    string jsonContent2 = JsonConvert.SerializeObject(newidInfo2);
                    HttpResponseMessage responseMessage2 = await client.PostAsync($"{AppSettings.BaseUrl}api/ExecutivesControler", new StringContent(jsonContent2, Encoding.UTF8, "application/json"));
                    Console.WriteLine(responseMessage2.StatusCode);
                }

                dishComboBox.SelectedItem = null;
            }
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dishes = new ObservableCollection<string>();
            List<Dish> dishesJson = new();

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage responseMessage = await client.GetAsync($"{AppSettings.BaseUrl}api/Dishes");
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonstring = await responseMessage.Content.ReadAsStringAsync();
                    dishesJson = JsonConvert.DeserializeObject<List<Dish>>(jsonstring);

                    foreach (var dish in dishesJson)
                    {
                        dishes.Add(dish.Name);
                    }                    

                    dishComboBox.ItemsSource = dishes;
                }
                else
                    MessageBox.Show("Server Error");
            }
        }
    }
}
