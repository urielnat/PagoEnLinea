using System;
using System.Collections.Generic;
using PagoEnLinea.Modelos;
using PagoEnLinea.servicios;
using Xamarin.Forms;

namespace PagoEnLinea.Paginas
{
    public partial class Facturacion : ContentPage
    {
        public static List<string> list;

        public static List<MostrarDatosFacturacion> lista;



        public Facturacion()
        {
         
            list = new List<string>();
            InitializeComponent();
            list.Add("");
            list.Add("");
            listView.ItemTapped += OnitemTapped;
           
        }


        async void conectar()
        {
            if (Application.Current.Properties.ContainsKey("token"))
            {


                ClienteRest cliente = new ClienteRest();
                var inf = await cliente.InfoUsuario<InfoUsuario>(Application.Current.Properties["token"] as string);
                //list = new List<DatosFacturacion>();

                lista = new List<MostrarDatosFacturacion>();



                if (inf != null)
                {
                    foreach (var dato in inf.datosFacturacion)
                    {

                        lista.Add(new MostrarDatosFacturacion
                        {
                            id=dato.id,
                            rfc = dato.rfc,
                            email = dato.email.correoe,
                            nomrazonSocial = dato.nomrazonSocial,
                            direccion = dato.direccion.catalogoDir.tipoasentamiento+" "+ dato.direccion.catalogoDir.asentamiento + ", " + dato.direccion.calle +" "+dato.direccion.numero,
                            idDireccion = dato.direccion.id,
                            idCatDir = dato.direccion.catalogoDir.id
                                            

                        });

                    }
                    BindingContext = lista;
                    listView.ItemsSource = lista;
                }
            }
        }
        protected override void OnAppearing()
        {
            conectar();
        }
       
        async void OnitemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            // do something with e.SelectedItem
            await Navigation.PushAsync(new Modificarfacturacion(((MostrarDatosFacturacion)e.Item).id, ((MostrarDatosFacturacion)e.Item).idDireccion, ((MostrarDatosFacturacion)e.Item).idCatDir, 0) { BindingContext = (MostrarDatosFacturacion)e.Item });

           ((ListView)sender).SelectedItem = null; // de-select
        }

        async void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            //await Navigation.PushAsync(new Modificarfacturacion(((MostrarDatosFacturacion)e.SelectedItem).id,((MostrarDatosFacturacion)e.SelectedItem).idDireccion,((MostrarDatosFacturacion)e.SelectedItem).idCatDir,0){BindingContext= (MostrarDatosFacturacion)e.SelectedItem});
        }

       

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new Modificarfacturacion(null, null, null, 1));
        }
    }
}
