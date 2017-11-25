using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Rg.Plugins.Popup.Services;

namespace PagoEnLinea
{
    public partial class TutorialPopUp 
    {
        public TutorialPopUp()
        {
            InitializeComponent();
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            PopupNavigation.PopAsync();
        }
    }
}
