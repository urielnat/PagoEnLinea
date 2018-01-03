using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using PagoEnLinea.Modales;
using PagoEnLinea.Modelos;
using PagoEnLinea.servicios;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace PagoEnLinea.Paginas
{
    public partial class Facturacion : ContentPage
    {
        public static List<string> list;

        public static List<MostrarDatosFacturacion> lista;
        public static MostrarDatosFacturacion item;


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
                            id = dato.id,
                            rfc = dato.rfc,
                            email = dato.email.correoe,
                            nomrazonSocial = dato.nomrazonSocial,
                            direccion = dato.direccion.catalogoDir.tipoasentamiento + " " + dato.direccion.catalogoDir.asentamiento + ", " + dato.direccion.calle + " " + dato.direccion.numero,
                            idDireccion = dato.direccion.id,
                            idCatDir = dato.direccion.catalogoDir.id,
                            calle = dato.direccion.calle
                                            

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
            MessagingCenter.Subscribe<Modal3>(this,"modificar",(obj) =>{
                MessagingCenter.Unsubscribe<Modal3>(this,"modificar");
                 Navigation.PushAsync(new Modificarfacturacion(item.id, item.idDireccion, item.idCatDir, 0, item.email,item.calle) { BindingContext =item });

            });

            MessagingCenter.Subscribe<Modal3>(this,"eliminar",async(obj) => {
                MessagingCenter.Unsubscribe<Modal3>(this, "eliminar");
                HttpResponseMessage response;


                try
                {
                    HttpClient cliente = new HttpClient();
                    //cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TOKEN);

                    if (Application.Current.Properties.ContainsKey("token"))
                    {

                        cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties["token"] as string);
                    }

                    var uri = new Uri(string.Format(Constantes.URL + "/datos-facturacions/{0}", item.id));
                    response = await cliente.DeleteAsync(uri);
                    var y = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(y);


                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {

                        System.Diagnostics.Debug.WriteLine("Se eliminó con exito");
                        await DisplayAlert("Eliminado", "Información eliminada con exito", "OK");
                        conectar();

                    }

                    else
                    {
                        System.Diagnostics.Debug.WriteLine(response);
                        await DisplayAlert("Error", "No fué posible intente mas tarde", "OK");

                    }
                }
                catch (HttpRequestException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.InnerException.Message);

                }

            });
        }

        async void OnitemTapped(object sender, ItemTappedEventArgs e)
        {





            item = (MostrarDatosFacturacion)e.Item;
            var info = (MostrarDatosFacturacion)e.Item;

            if (Device.RuntimePlatform == Device.Android)
            {
                await PopupNavigation.PushAsync(new Modal3());
            }
            else { 
            var action = await DisplayActionSheet("¿Qué desea hacer?", "Cancelar", "Eliminar", "Modificar");
            if (!string.IsNullOrEmpty(action))
            {
                if (action.Equals("Modificar"))
                {
                    await Navigation.PushAsync(new Modificarfacturacion(((MostrarDatosFacturacion)e.Item).id, ((MostrarDatosFacturacion)e.Item).idDireccion, ((MostrarDatosFacturacion)e.Item).idCatDir, 0, ((MostrarDatosFacturacion)e.Item).email, ((MostrarDatosFacturacion)e.Item).calle) { BindingContext = (MostrarDatosFacturacion)e.Item });
                }
                if (action.Equals("Eliminar"))
                {
                    var respuesta = await DisplayAlert("Eliminar", "¿Esta seguro que desea eliminar esta información de facturación?", "Si", "Cancelar");
                    {
                        if (respuesta)
                        {

                            HttpResponseMessage response;


                            try
                            {
                                HttpClient cliente = new HttpClient();
                                //cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TOKEN);

                                if (Application.Current.Properties.ContainsKey("token"))
                                {

                                    cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties["token"] as string);
                                }

                                var uri = new Uri(string.Format(Constantes.URL + "/datos-facturacions/{0}", info.id));
                                response = await cliente.DeleteAsync(uri);
                                var y = await response.Content.ReadAsStringAsync();
                                System.Diagnostics.Debug.WriteLine(y);


                                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                {

                                    System.Diagnostics.Debug.WriteLine("Se eliminó con exito");
                                    await DisplayAlert("Eliminado", "Información eliminada con exito", "OK");
                                    conectar();

                                }

                                else
                                {
                                    System.Diagnostics.Debug.WriteLine(response);
                                    await DisplayAlert("Error", "No fué posible intente mas tarde", "OK");

                                }
                            }
                            catch (HttpRequestException ex)
                            {
                                System.Diagnostics.Debug.WriteLine(ex.InnerException.Message);

                            }



                        }
                    }
                }
            }
        }
            ((ListView)sender).SelectedItem = null;






        }

      

       

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new Modificarfacturacion(null, null, null, 1,null,null));
        }
    }
}
