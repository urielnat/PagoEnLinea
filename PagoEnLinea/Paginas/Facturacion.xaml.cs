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
    /// <summary>
    /// esta clase muestra una pantalla todos los datos de facturación que el usuario ha dado de alta
    /// permitiendole la posibilidad de modificarlos y eliminarlos
    /// </summary>
    public partial class Facturacion : ContentPage
    {
      

        public static List<MostrarDatosFacturacion> lista;
        public static MostrarDatosFacturacion item;

        /// <summary>
        /// inicializa los componenetes visuales correspondientes a su XAML y añade un evento de tipo item tapped a la lista
        /// que contiene todos los datos de facturación del usuario.
        /// </summary>
        public Facturacion()
        {
         
           
            InitializeComponent();
          
            listView.ItemTapped += OnitemTapped;
           
        }

        /// <summary>
       /// llama al servicio para mostrar todos los datos de facturacíon ingresadas por el usuario y añadirlos a una lista.
       /// </summary>>
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


        /// <summary>
        /// llama el método conectar que permite llenar la lista de datos de facturación al desplegar la pantalla dirección
        /// del módulo perfil, a su vez esta conectado a la subpantalla "Modal3" la cual muestra un cuadro de dialogo el cual
        /// a partir de el resultado obtenido permite modificar o eliminar un elemento de la lista de correos
        /// (unicamente en Android)
        /// </summary>
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

                    var uri = new Uri(string.Format(Constantes.URL_USUARIOS + "/datos-facturacions/{0}", item.id));
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

        /// <summary>
        /// evento que permite seleccionar un dato de facturación de la lista y elegir la opción de modificarlo o eliminarlo
        /// unicamente en android se hará uso del cuadro de diálogo personalizado ya que en iOS
        /// sus configuraciones nativas no lo requieren. Así mismo consume los servicios correspondientes.
        /// </summary>
        /// <param name="sender">objeto que hace referencia al evento</param>
        /// <param name="e">argumentos que son posibles de obtener apartir del objeto que hace llamada al evento</param>
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

                                var uri = new Uri(string.Format(Constantes.URL_USUARIOS + "/datos-facturacions/{0}", info.id));
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

      

       
        /// <summary>
        /// Evento click al presionar el boton flotante, muestra al usuario la pantalla modificar datos de facturación
        /// pero al recibir parámetros nulos se toma como una pantalla para añadir nueva información de facturacíon
        /// </summary>
        /// <param name="sender">objeto que hace refencia al evento</param>
        /// <param name="e">propiedades o argumentos del objeto que son accesibles a travez de la llamada al evetno</param>
       
        void Handle_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new Modificarfacturacion(null, null, null, 1,null,null));
        }
    }
}
