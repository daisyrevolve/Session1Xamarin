using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Session1Xamarin.Class1;

namespace Session1Xamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage

    {

        List<User_Type> _userTypes = new List<User_Type>();

        public Page1()
        {
            InitializeComponent();
            LoadPicker();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void createAccount_Clicked(object sender, EventArgs e)
        {
            if (entryPassword.Text == entryRePassword.Text)
            {
                var getUserTypeID = (from x in _userTypes
                                     where x.userTypeName == pickUserType.SelectedItem.ToString()
                                     select x.userTypeId).FirstOrDefault();
                var newUser = new User()
                {

                    userId = entryUserID.Text,
                    userName = entryUserName.Text,
                    userPw = entryPassword.Text,
                    userTypeIdFK = getUserTypeID

                };
                using (var webClient = new WebClient())
                {
                    var jsonData = JsonConvert.SerializeObject(newUser);
                    webClient.Headers.Add("Content-type", "application/json");
                    var response = await webClient.UploadDataTaskAsync("http://10.0.2.2:57325/Users/Create", "POST", Encoding.UTF8.GetBytes(jsonData));
                    if (Encoding.Default.GetString(response) == "\"User has been created.\"") {
                        await DisplayAlert("Create Account", "Your account creation is successful", "Ok");
                        await Navigation.PopAsync();
                            }
                    else
                    {
                        await DisplayAlert("Create Account", "Account has not been created", "Ok");
                    }
                   
                }
            }
            else
            {
                await DisplayAlert("Create Account", "Your passwords do not match", "Ok");
            }
        }

        private async void LoadPicker() {
            using (var webClient = new WebClient()) {
            webClient.Headers.Add("Content-type", "application/json");
                var response = await webClient.UploadDataTaskAsync("http://10.0.2.2:57325/User_Type", "POST", Encoding.UTF8.GetBytes(""));
            _userTypes = JsonConvert.DeserializeObject<List<User_Type>>(Encoding.Default.GetString(response));
                foreach (var item in _userTypes) {
                    pickUserType.Items.Add(item.userTypeName);
                }
            }
        }

        
    }
}