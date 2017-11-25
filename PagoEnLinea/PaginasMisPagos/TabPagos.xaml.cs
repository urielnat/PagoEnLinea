using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PagoEnLinea.PaginasMisPagos
{
    public partial class TabPagos : TabbedPage
    {
        public TabPagos()
        {
            InitializeComponent();
            Children.Add(new PrediosPagadosPage());
            Children.Add(new LiquidacionesPagadasPage());
        }
    }
}
