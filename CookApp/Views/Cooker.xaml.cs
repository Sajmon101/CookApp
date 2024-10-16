using CookApp.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for Cooker.xaml
    /// </summary>
    public partial class Cooker : Window
    {
        private int cookerId = SessionManager.Instance.loggedInEmployeeId;
        public class CookerDish
        {
            public int Id;
            public string Name;
        }

        public class IdInfo2
        {
            public int employeeId { get; set; }
            public int dishInOrderId { get; set; }
        }

        public CookerDish firstDish = new();
        public CookerDish secondDish = new();

        public Cooker()
        {
            InitializeComponent();
            ShowEmployeeName();
            UpdateDishes();
            UpdateQueueLenghtDisp();
        }

        private async void ConfBtn1(object sender, RoutedEventArgs e)
        {
            ChangeDishToReady(firstDish.Id);
            await UpdateDishes();
            UpdateQueueLenghtDisp();
        }
        private async void ConfBtn2(object sender, RoutedEventArgs e)
        {
            ChangeDishToReady(secondDish.Id);
            await UpdateDishes();
            UpdateQueueLenghtDisp();
        }

        private void LogoutBtn(object sender, RoutedEventArgs e)
        {
            SessionManager.Instance.loggedInEmployeeId = 0;
            Login newWindow = new Login();
            newWindow.Show();
            this.Close();
        }

        private void ShowEmployeeName()
        {
            EmployeeName.Text = SessionManager.Instance.name + " " + SessionManager.Instance.surname;
        }

        private async void UpdateQueueLenghtDisp()
        {
            using (HttpClient client = new HttpClient())
            {
                Console.WriteLine(AppSettings.BaseUrl);
                HttpResponseMessage responseMessage = await client.GetAsync($"{AppSettings.BaseUrl}api/ExecutivesControler");

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonstring = await responseMessage.Content.ReadAsStringAsync();
                    int queueLength = JsonConvert.DeserializeObject<int>(jsonstring);
                    queueLenghtTB.Text = queueLength.ToString();
                }

            }
                return;
        }

        private async Task UpdateDishes()
        {
            List<CookerDish> assignedDishes = await GetAssignedDishes();

            if (assignedDishes.Count == 0)
            {
                CookerDish freshAssign1 = await AssignDishToCooker();
                assignedDishes.Add(freshAssign1);

                CookerDish freshAssign2 = await AssignDishToCooker();
                assignedDishes.Add(freshAssign2);
            }
            else if (assignedDishes.Count == 1)
            {
                CookerDish freshAssign = await AssignDishToCooker();
                assignedDishes.Add(freshAssign);
            }

            UpdateUI(assignedDishes);
            return;
        }

        private void UpdateUI(List<CookerDish> dishes)
        {
            switch (dishes.Count)
            {
                case 1:
                    UpdateUIDish(firstDishTB, firstDish, dishes[0]);
                    ClearUIDish(secondDishTB, secondDish);
                    UpdateQueueLenghtDisp();
                    break;

                case 2:
                    UpdateUIDish(firstDishTB, firstDish, dishes[0]);
                    UpdateUIDish(secondDishTB, secondDish, dishes[1]);
                    UpdateQueueLenghtDisp();
                    break;

                default:
                    ClearUIDish(firstDishTB, firstDish);
                    ClearUIDish(secondDishTB, secondDish);
                    break;
            }
        }

        private void UpdateUIDish(TextBlock textBox, CookerDish dish, CookerDish cookerDish)
        {
            textBox.Text = cookerDish.Name;
            dish.Id = cookerDish.Id;
            dish.Name = cookerDish.Name;
        }

        private void ClearUIDish(TextBlock textBox, CookerDish dish)
        {
            textBox.Text = "Brak w kolejce";
            dish.Id = 0;
            dish.Name = null;
        }

        private async Task<List<CookerDish>> GetAssignedDishes()
        {
            using (HttpClient client = new HttpClient())
            {
                List<CookerDish> assignedDishes = new();
                HttpResponseMessage responseMessage = await client.GetAsync($"{AppSettings.BaseUrl}api/DishInOrder/forCooker/{cookerId}");

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonstring = await responseMessage.Content.ReadAsStringAsync();
                    assignedDishes = JsonConvert.DeserializeObject<List<CookerDish>>(jsonstring);
                }

                return assignedDishes;
            }
        }

        private async Task<CookerDish> AssignDishToCooker()
        {
            CookerDish freshlyAssignedDish = new();

            using (HttpClient client = new HttpClient())
            {
                IdInfo2 loggedCooker = new IdInfo2
                {
                    employeeId = cookerId
                };
                string jsonContent = JsonConvert.SerializeObject(loggedCooker);
                HttpResponseMessage responseMessage = await client.PostAsync($"{AppSettings.BaseUrl}api/ExecutivesControler/forCooker", new StringContent(jsonContent, Encoding.UTF8, "application/json"));

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonstring = await responseMessage.Content.ReadAsStringAsync();
                    freshlyAssignedDish = JsonConvert.DeserializeObject<CookerDish>(jsonstring);
                }
            }
            return freshlyAssignedDish;
        }

        private async void ChangeDishToReady(int dishToConfirmId)
        {
            if(dishToConfirmId != 0)
            {
                using (HttpClient client = new HttpClient())
                {
                    IdInfo2 dishToConfirm = new IdInfo2
                    {
                        employeeId = cookerId,
                        dishInOrderId = dishToConfirmId
                    };
                    var json = JsonConvert.SerializeObject(dishToConfirm);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage responseMessage = await client.PutAsync($"{AppSettings.BaseUrl}api/ExecutivesControler", content);

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Zatwierdzono");
                        UpdateDishes();
                    }
                    else
                    {
                        MessageBox.Show("API Server Error");
                    }
                }
            }
        }
    }
}
