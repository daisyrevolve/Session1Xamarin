using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Session1Xamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditResource : ContentPage
    {
        public EditResource(string ResourceName)
        {
            InitializeComponent();
            resourceName = ResourceName;
           
        }
    }
}