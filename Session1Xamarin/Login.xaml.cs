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
    public partial class Login : ContentPage
    {

        /*List<User_Type> _userTypes = new List<User_Type>();*/
        List<User> _user = new List<User>();
        public Login()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            loginPassword.Text = string.Empty;
            loginUserID.Text = string.Empty;
        }


        private async void Button_Clicked(object sender, EventArgs e)
        {

            if (loginPassword.Text.Trim() == string.Empty || loginUserID.Text.Trim() == string.Empty)
            {
                await DisplayAlert("Login", "Fields cannot be empty!", "Ok");
            }
            else
            {
                using (var webClient = new WebClient())
                {
                    webClient.Headers.Add("Content-Type", "application/json");
                    var response = await webClient.UploadDataTaskAsync($"http://10.0.2.2:57325/Users/Login?username={loginUserID.Text}&password={loginPassword.Text}", "POST",
                        Encoding.UTF8.GetBytes(""));
                    if (Encoding.Default.GetString(response) != "\"user does not exists.\"")
                    {
                        var user = JsonConvert.DeserializeObject<User>(Encoding.Default.GetString(response));
                        if (user.userTypeIdFK == 1)
                        {
                            await DisplayAlert("Login", $"Welcome {user.userName}!", "Ok");
                           
                        }
                        else
                        {
                            await DisplayAlert("Login", $"Welcome {user.userName}! You are not a manager!", "Ok");
                            loginPassword.Text = string.Empty;
                        }
                    }
                    else
                    {
                        await DisplayAlert("Login", "Invalid User", "Ok");
                        loginPassword.Text = string.Empty;
                    }
                }
            }

        }
    }
}
            
 