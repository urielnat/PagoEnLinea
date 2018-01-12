using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PagoEnLinea.Modales;
using PagoEnLinea.Modelos;
using PagoEnLinea.servicios;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace PagoEnLinea.Paginas
{

    /// <summary>
    /// esta clase muestra una pantalla todas los direcciones que el usuario ha dado de alta
    /// permitiendole la posibilidad de modificarlos y eliminarlos
    /// </summary>
    public partial class Direccion : ContentPage
    {
        public static List<infodir> list;
        public infodir infdir { set; get; }
        public CatalogoDir catdir { set; get; }
        public static infodir item;


        /// <summary>
        /// inicializa los componenetes visuales correspondientes a su XAML y añade un evento de tipo item tapped a la lista
        /// que contendrá todos las direccioes del usuario.
        /// </summary>
        public Direccion()
        {
          
            InitializeComponent();
            listView.ItemTapped+= ListView_ItemTapped;
        }


        /// <summary>
        /// evento que permite seleccionar una direccion de la lista y elegir la opcion de modificarlo o eliminarlo
        /// unicamente en android se hará uso del cuadro de diálogo personalizado ya que en iOS
        /// sus configuraciones nativas no lo requieren. Así mismo consume los servicios correspondientes.
        /// </summary>
        /// <param name="sender">objeto que hace referencia al evento</param>
        /// <param name="e">argumentos que son posibles de obtener apartir del objeto que hace llamada al evento</param>
        async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var info = (infodir)e.Item;
            item = (infodir)e.Item;


            if (Device.RuntimePlatform == Device.Android)
            {
                await PopupNavigation.PushAsync(new Modal2());
            }
            else { 

            var action = await DisplayActionSheet("¿Qué desea hacer?", "Cancelar", "Eliminar", "Modificar");
            if (!string.IsNullOrEmpty(action))
            {
                if (action.Equals("Modificar"))
                {
                    await Navigation.PushAsync(new ModificarDireccion(info.id, info.idCat, 0, info.estado, info.tipoasentamiento) { BindingContext = (infodir)e.Item });
                }
                if (action.Equals("Eliminar"))
                {
                    var respuesta = await DisplayAlert("Eliminar", "¿Esta seguro que desea eliminar esta dirección?", "Si", "Cancelar");
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

                                var uri = new Uri(string.Format(Constantes.URL_USUARIOS + "/direccion/eliminar/{0}", info.id));
                                response = await cliente.DeleteAsync(uri);
                                var y = await response.Content.ReadAsStringAsync();
                                System.Diagnostics.Debug.WriteLine(y);


                                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                {

                                    System.Diagnostics.Debug.WriteLine("Se eliminó con exito");
                                    await DisplayAlert("Eliminado", "Dirección eliminada con exito", "OK");
                                    conectar();
                                }

                                else
                                {
                                    System.Diagnostics.Debug.WriteLine(response);
                                    var resp = JsonConvert.DeserializeObject<Respuesta>(y);

                                    await DisplayAlert("Error", resp.respuesta, "OK");

                                }
                            }
                            catch (HttpRequestException ex)
                            {
                                System.Diagnostics.Debug.WriteLine(ex.InnerException.Message);
                                await DisplayAlert("Error", "No fué posible intente mas tarde", "OK");
                            }


                        }
                    }
                }
            }
        }
            ((ListView)sender).SelectedItem = null;
        }

     
        /// <summary>
       /// llama al servicio para mostrar todas las direcciones ingresadas por el usuario y añadirlos a una lista.
       /// </summary>
        async void conectar()
        {
            if (Application.Current.Properties.ContainsKey("token"))
            {
                var cont = 1;

                ClienteRest cliente = new ClienteRest();
                var inf = await cliente.InfoUsuario<InfoUsuario>(Application.Current.Properties["token"] as string);
                list = new List<Modelos.infodir>();

                if (inf != null)
                {
                    foreach (var dato in inf.direcciones){
                       
                        list.Add(new Modelos.infodir
                        {
                            NumerodeDireccion = "Dirección " + cont+":",
                            id = dato.id,
                            calle = dato.calle,
                            numero = dato.numero,
                            numeroInterior = dato.numeroInterior,
                            tipo = dato.tipo,

                            cp = dato.catalogoDir.cp,
                            asentamiento = dato.catalogoDir.asentamiento,
                            municipio = dato.catalogoDir.municipio,
                            estado = dato.catalogoDir.estado,
                            pais = dato.catalogoDir.pais,
                            tipoasentamiento = dato.catalogoDir.tipoasentamiento,
                            ciudad = dato.catalogoDir.ciudad,
                            idCat = dato.catalogoDir.id
                               



                        });
                        cont++;
                        System.Diagnostics.Debug.WriteLine(dato.catalogoDir.municipio);
                    }


                    BindingContext = list;
                    listView.ItemsSource = list;
                  
                }

            }
        }


        /// <summary>
        /// llama el método conectar que permite llenar la lista de direcciones al desplegar la pantalla dirección
        /// del módulo perfil, a su vez esta conectado a la subpantalla "Modal2" la cual muestra un cuadro de dialogo el cual
        /// a partir de el resultado obtenido permite modificar o eliminar un elemento de la lista de correos
        /// (unicamente en Android)
        /// </summary>
        protected override void OnAppearing()
        {
            conectar();

            MessagingCenter.Subscribe<Modal2>(this,"modificar",(obj) => {
                MessagingCenter.Unsubscribe<Modal2>(this,"modificar");

                Navigation.PushAsync(new ModificarDireccion(item.id, item.idCat, 0, item.estado, item.tipoasentamiento) { BindingContext = item });




            });

            MessagingCenter.Subscribe<Modal2>(this, "eliminar", async(obj) => {
                MessagingCenter.Unsubscribe<Modal2>(this, "eliminar");


                HttpResponseMessage response;


                try
                {
                    HttpClient cliente = new HttpClient();
                    //cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TOKEN);
                    if (Application.Current.Properties.ContainsKey("token"))
                    {

                        cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties["token"] as string);
                    }

                    var uri = new Uri(string.Format(Constantes.URL_USUARIOS + "/direccion/eliminar/{0}", item.id));
                    response = await cliente.DeleteAsync(uri);
                    var y = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(y);


                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {

                        System.Diagnostics.Debug.WriteLine("Se eliminó con exito");
                        await DisplayAlert("Eliminado", "Dirección eliminada con exito", "OK");
                        conectar();
                    }

                    else
                    {
                        System.Diagnostics.Debug.WriteLine(response);
                        var resp = JsonConvert.DeserializeObject<Respuesta>(y);

                        await DisplayAlert("Error", resp.respuesta, "OK");

                    }
                }
                catch (HttpRequestException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.InnerException.Message);
                    await DisplayAlert("Error", "No fué posible intente mas tarde", "OK");
                }



            });
           
        }


        /// <summary>
        /// Evento click al presionar el boton flotante, muestra al usuario la pantalla modificar dirección
        /// pero al recibir parámetros nulos se toma como una pantalla para añadir una nueva dirección
        /// </summary>
        /// <param name="sender">objeto que hace refencia al evento</param>
        /// <param name="e">propiedades o argumentos del objeto que son accesibles a travez de la llamada al evento</param>
       
        void Handle_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new ModificarDireccion(null, null, 1,null,null));
        }
    }
}
