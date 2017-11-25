using System;
using System.Collections.Generic;
using PagoEnLinea.PaginasMisPagos;
using Xamarin.Forms;

namespace PagoEnLinea.PaginasPago
{
    public partial class TabPredios : TabbedPage
    {
        public TabPredios()
        {
            InitializeComponent();
            Children.Add(new Datos());
            Children.Add(new Predios());
        }
    }
}
