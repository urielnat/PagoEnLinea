using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PagoEnLinea.PaginasMisPagos
{
    public partial class LiquidacionesPagadasPage : ContentPage
    {
        List<string> list = new List<string>();
        public LiquidacionesPagadasPage()
        {
            InitializeComponent();
            list.Add("");
            listView.ItemsSource = list;
        }

        void Handle_SearchButtonPressed(object sender, System.EventArgs e)
        {
           
        }
    }
}
