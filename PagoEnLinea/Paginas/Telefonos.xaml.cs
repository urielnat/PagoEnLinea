using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using PagoEnLinea.Modelos;
using PagoEnLinea.servicios;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace PagoEnLinea.Paginas
{
        /// <summary>
        /// inicializa los componenetes visuales correspondientes a su XAML y añade un evento de tipo item tapped a la lista
        /// que contiene todos los teléfonos del usuario.
        /// </summary>
    public partial class Telefonos : ContentPage
    {
        public static List<Telefono> list;
        public static Telefono item;
        public Telefonos()
        {
         
            InitializeComponent();
            listView.ItemTapped+= ListView_ItemTapped;
          
        }


        /// <summary>
        /// evento que permite seleccionar un teléfono de la lista y elegir la opción de modificarlo o eliminarlo
        /// unicamente en android se hará uso del cuadro de diálogo personalizado ya que en iOS
        /// sus configuraciones nativas no lo requieren. Así mismo consume los servicios correspondientes.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var info = (Telefono)e.Item;
            item = (Telefono)e.Item;

            if (Device.RuntimePlatform == Device.Android)
            {
                await PopupNavigation.PushAsync(new Modal4());
            }
            else { 
            var action = await DisplayActionSheet("¿Qué desea hacer?", "Cancelar", "Eliminar", "Modificar");
            if (!string.IsNullOrEmpty(action))
            {
                if (action.Equals("Modificar"))
                {
                    await Navigation.PushAsync(new ModificarTelefono((e.Item as Telefono).id, (e.Item as Telefono).tipo, 0) { BindingContext = (Telefono)e.Item });
                }
                if (action.Equals("Eliminar"))
                {
                    var respuesta = await DisplayAlert("Eliminar", "¿Esta seguro que desea eliminar este teléfono?", "Si", "Cancelar");
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

                                var uri = new Uri(string.Format(Constantes.URL_USUARIOS + "/telefono/eliminar/{0}", info.id));
                                response = await cliente.DeleteAsync(uri);
                                var y = await response.Content.ReadAsStringAsync();
                                System.Diagnostics.Debug.WriteLine(y);


                                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                {

                                    System.Diagnostics.Debug.WriteLine("Se eliminó con exito");
                                    await DisplayAlert("Eliminado", "Teléfono eliminado con exito", "OK");
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
        /// llama al servicio para mostrar todos los teléfonos ingresados por el usuario y añadirlos a una lista.
        /// </summary>
       async void conectar()
        {
            if (Application.Current.Properties.ContainsKey("token"))
            {


                ClienteRest cliente = new ClienteRest();
                var inf = await cliente.InfoUsuario<InfoUsuario>(Application.Current.Properties["token"] as string);
                list = new List<Telefono>();
                if (inf != null)
                {
                    foreach (var dato in inf.telefonos)
                    {
                        //catdir.cp = dato.catalogoDir.cp;
                        list.Add(new Telefono
                        {

                            id =dato.id,
                            telefono = dato.telefono,
                            lada = dato.lada,
                            tipo =dato.tipo



                        });
                    }


                    BindingContext = list;
                    listView.ItemsSource = list;

                }

            }
        }


        /// <summary>
        /// llama el método conectar que permite mostrar los teléfonos ingresados por el usuario al desplegar la pantalla 
        /// del módulo perfil, a su vez esta conectado a la subpantalla "Modal4" la cual muestra un cuadro de dialogo el cual
        /// a partir de el resultado obtenido permite modificar o eliminar un elemento de la lista de correos
        /// (unicamente en Android)
        /// </summary>
        protected override void OnAppearing()
        {
            conectar();
            MessagingCenter.Subscribe<Modal4>(this,"modificar",(obj) => {
                MessagingCenter.Unsubscribe<Modal4>(this,"modificar");
                 Navigation.PushAsync(new ModificarTelefono(item.id, item.tipo, 0) { BindingContext = item });

            });
            MessagingCenter.Subscribe<Modal4>(this,"eliminar",async(obj) => {
                MessagingCenter.Unsubscribe<Modal4>(this, "eliminar");

                HttpResponseMessage response;


                try
                {
                HttpClient cliente = new HttpClient();
                //cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TOKEN);

                if (Application.Current.Properties.ContainsKey("token"))
                {

                    cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties["token"] as string);
                }

                var uri = new Uri(string.Format(Constantes.URL_USUARIOS + "/telefono/eliminar/{0}", item.id));
                response = await cliente.DeleteAsync(uri);
                var y = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine(y);


                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    System.Diagnostics.Debug.WriteLine("Se eliminó con exito");
                    await DisplayAlert("Eliminado", "Teléfono eliminado con exito", "OK");
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
        /// Evento click al presionar el boton flotante, muestra al usuario la pantalla modificar teléfono
        /// pero al no recibir parámetros se toma como una pantalla para añadir un nuevo teléfono
        /// </summary>
        /// <param name="sender">objeto que hace refencia al evento</param>
        /// <param name="e">propiedades o argumentos del objeto que son accesibles a travez de la llamada al evento</param>
        void Handle_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new ModificarTelefono(null,null,1));
        }
    }
}
