using CookApp.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using static CookApp.Views.ReadyOnes;
using CookApp;

namespace CookApp.Views
{
    /// <summary>
    /// Interaction logic for ShowOrder.xaml
    /// </summary>
    public partial class ShowOrder : UserControl
    {
        public class OrderInfo
        {
            public DateTime Date { get; set; }
            public string Name { get; set; }
            public int DishInOrderId { get; set; }
            public float price { get; set; }
        }

        int tableNum;
        List<OrderInfo> result = new();

        public ShowOrder()
        {
            InitializeComponent();
        }

        private async void ShowButton_Click(object sender, RoutedEventArgs e)
        {
            if (tableNumInput.Text !=null)
            {
                using (HttpClient client = new HttpClient())
                {
                    tableNum = int.Parse(tableNumInput.Text);
                    HttpResponseMessage responseMessage1 = await client.GetAsync($"{AppSettings.BaseUrl}api/DishInOrder/byTable/{tableNum}");
                    if (responseMessage1.IsSuccessStatusCode)
                    {
                        var jsonstring2 = await responseMessage1.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<List<OrderInfo>>(jsonstring2);
                    }
                    else
                        MessageBox.Show("Server Error");
                }

                gridOrder.ItemsSource = result;

                float totalSum = result.Sum(x => x.price);
                totalPriceTextBlock.Text = totalSum.ToString("F2");
                
            }
        }

        private void NumericOnlyTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _))
            {
                e.Handled = true; // Block the input if it's not numeric
            }
        }

        private async void PaidBtn_Click(object sender, RoutedEventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                var orderToArch = new Order();
                orderToArch.TableNum = tableNum;
                var json = JsonConvert.SerializeObject(orderToArch);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await client.PutAsync($"{AppSettings.BaseUrl}api/Order", content);

                if (responseMessage.IsSuccessStatusCode)
                {
                    gridOrder.ItemsSource = null;
                    totalPriceTextBlock.Text = null;
                    MessageBox.Show("Zamówienie zostało przeniesione do archiwum");
                }
                else if (responseMessage.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    MessageBox.Show("API Server Error");
                }
            }
        }
    }
}
