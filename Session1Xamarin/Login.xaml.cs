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
        }


        private async void Button_Clicked(object sender, EventArgs e)
        {

            var getUserTypeID = (from x in _user
                                 where x.userTypeIdFK == 1
                                 select x.userId).FirstOrDefault();

            var getPassword = (from x in _user
                               where x.userId == loginUserID.Text
                               select x.userPw).FirstOrDefault();

               if (loginUserID.Text == getUserTypeID && loginPassword.Text == getPassword) {

                using (var webClient = new WebClient())
                {

                
                    webClient.Headers.Add("Content-type", "application/json");
                    var response = await webClient.UploadDataTaskAsync("http://10.0.2.2:57325/Users/Login?username=" 
                        + getUserTypeID + "&password=" + getPassword, "POST", Encoding.UTF8.GetBytes(""));
                    if (Encoding.Default.GetString(response) != null)
                    {

                        /*var jsonResult = JsonConvert.DeserializeObject(response);*/
                        await DisplayAlert("Login", "Successful", "Ok");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("Login", "Unsuccessful", "Ok");
                    }
                }
            }
               
            
        }
    }
}
            
 