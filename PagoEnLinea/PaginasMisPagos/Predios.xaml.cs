using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PagoEnLinea.PaginasMisPagos
{
    
    public partial class Predios : ContentPage
    {
        List<string> list = new List<string>();
        public Predios()
        {
            InitializeComponent();
            list.Add("");
            listView.ItemsSource = list;
        }
    }
}
