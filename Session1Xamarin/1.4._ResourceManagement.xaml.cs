using Newtonsoft.Json;
using Session1Xamarin;
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
    public partial class _1 : ContentPage
    {
        List<CustomizedView> customizedView = new List<CustomizedView>();
        List<Resource_Type> resourceType = new List<Resource_Type>();
        List<Skill> skill = new List<Skill>();

        public _1()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadResources();
            LoadPickers();

        }

        private async void LoadResources()
        {
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("Content-Type", "application/json");
                var response = await webClient.UploadDataTaskAsync("http://10.0.2.2:57325/Resources", "POST", Encoding.UTF8.GetBytes(""));
                customizedView = JsonConvert.DeserializeObject<List<CustomizedView>>(Encoding.Default.GetString(response));
                // populatin listview with data from customized view
                listViewResources.ItemsSource = customizedView;
            }
        }

        private async void LoadPickers()
        {
            pickerSkill.Items.Clear();
            pickerType.Items.Clear();
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("Content-Type", "application/json");
                var response = await webClient.UploadDataTaskAsync("http://10.0.2.2:57325/Resource_Type", "POST", Encoding.UTF8.GetBytes(""));
                // getting resource data to load in picker
                resourceType = JsonConvert.DeserializeObject<List<Resource_Type>>(Encoding.Default.GetString(response));
                foreach (var item in resourceType)
                {
                    pickerType.Items.Add(item.resTypeName);
                }
            }
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("Content-Type", "application/json");
                // getting skills data to load in skills picker
                var response = await webClient.UploadDataTaskAsync("http://10.0.2.2:57325/Skills", "POST", Encoding.UTF8.GetBytes(""));
                skill = JsonConvert.DeserializeObject<List<Skill>>(Encoding.Default.GetString(response));
                foreach (var item in skill)
                {
                    pickerSkill.Items.Add(item.skillName);
                }
            }
        }



        private async void btnAdd_ClickedAsync(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddResource());
        }

        private async void btnEdit_Clicked(object sender, EventArgs e)
        {
            var selectedToEdit = (CustomizedView)listViewResources.SelectedItem;
            Console.WriteLine(selectedToEdit.ResourceName);
            await Navigation.PushAsync(new EditResource(selectedToEdit.ResourceName));
        }

        private async void btnRemove_ClickedAsync(object sender, EventArgs e)
        {
            var selectedToDelete = (CustomizedView)listViewResources.SelectedItem;
            var confirmationStatus = await DisplayAlert("Remove", $"Are you sure you want to remove {selectedToDelete.ResourceName}?", "Yes", "No");
            if (confirmationStatus)
            {
                using (var webClient = new WebClient())
                {
                    try
                    {
                        webClient.Headers.Add("Content-Type", "application/json");
                        var response = await webClient.UploadDataTaskAsync($"http://10.0.2.2:57325/Resources/Delete?ResourceName={selectedToDelete.ResourceName}", "POST",
                            Encoding.UTF8.GetBytes(""));
                        if (Encoding.Default.GetString(response) == "\"Resource has been successfully deleted!\"")
                        {
                            await DisplayAlert("Remove", "Resource has been successfully deleted!", "Ok");
                            LoadResources();
                        }
                    }
                    catch (Exception)
                    {
                        await DisplayAlert("Remove", "An error occured while trying to remove resource. Please try again later!", "Ok");
                    }

                }
            }
        }






        private void pickerType_SelectedIndexChanged(object sender, EventArgs e){
                if (pickerSkill.SelectedItem == null)
                {
                    var filteredList = (from x in customizedView
                                        where x.ResourceType == pickerType.SelectedItem.ToString()
                                        select x);
                    listViewResources.ItemsSource = filteredList.ToList();
                }
                else
                {
                    var filteredList = (from x in customizedView
                                        where x.ResourceType == pickerType.SelectedItem.ToString() && x.AllocatedSkills.Contains(pickerSkill.SelectedItem.ToString())
                                        select x);
                    listViewResources.ItemsSource = filteredList.ToList();
                }
        }

            private void pickerSkill_SelectedIndexChanged(object sender, EventArgs e)
            {
                if (pickerType.SelectedItem == null)
                {
                    var filteredList = (from x in customizedView
                                        where x.AllocatedSkills.Contains(pickerSkill.SelectedItem.ToString())
                                        select x);
                    listViewResources.ItemsSource = filteredList.ToList();
                }
                else
                {
                    var filteredList = (from x in customizedView
                                        where x.ResourceType == pickerType.SelectedItem.ToString() && x.AllocatedSkills.Contains(pickerSkill.SelectedItem.ToString())
                                        select x);
                    listViewResources.ItemsSource = filteredList.ToList();
                }
            }



            
        }
    
}