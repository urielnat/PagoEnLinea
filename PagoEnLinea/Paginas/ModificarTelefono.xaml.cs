using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PagoEnLinea.Modelos;
using PagoEnLinea.servicios;
using Xamarin.Forms;

namespace PagoEnLinea.Paginas
{
    /// <summary>
    /// Esta clase muestra una pantalla ya sea para modificar o añadir un teléfono
    /// según la el tipo de parámetros que recibe ya que muestra diferentes botones con eventos asociados.
    /// </summary>
   public partial class ModificarTelefono : ContentPage
    {
        string ID;

        /// <summary>
        /// inicializa los componentes visuales de su XAML
        /// muestra u oculta componentes según el tipo de parametro recibido
        /// </summary>
        /// <param name="id">id del teléfono a modificar</param>
        /// <param name="tipoTel">Tipo del teléfono a modficar</param>
        /// <param name="tipo">tipo de pantalla que se mostrará, 0 para tipo modificar 1 para tipo añadir</param>
        public ModificarTelefono(string id, string tipoTel,int tipo)
        {
            InitializeComponent();
            ID = id;
            for (var i = 0; i < pkTipo.Items.Count; i++)
            {
                if (pkTipo.Items[i].Equals(tipoTel))
                {
                    pkTipo.SelectedIndex = i;
                }
            }

            enTelefono.TextChanged += OnTelefonoChanged;
            enLada.TextChanged += OnLadaChanged;


            if (tipo == 0)
            {
                btnModificar.IsVisible = true;
                btnAgregar.IsVisible = false;
            }
            if (tipo == 1)
            {
                btnAgregar.IsVisible = true;
                btnModificar.IsVisible = false;
            }
        }

        /// <summary>
        /// método usado para consumir al servicio de modificar un teléfono, primero valida si los campos ingresados sean correctos
        /// </summary>
        async void conectar()
        {
            if (Application.Current.Properties.ContainsKey("token"))
            {


                ClienteRest cliente = new ClienteRest();
                var inf = await cliente.InfoUsuario<InfoUsuario>(Application.Current.Properties["token"] as string);
                if (inf != null)
                {
                    Telefono modtel;

                   
                   // modtel.persona = inf.persona;
                    bool auth = false, auth2 = false;


                    modtel = new Telefono
                    {
                        id = ID,
                        tipo = pkTipo.Items[pkTipo.SelectedIndex],
                        telefono = enTelefono.Text,
                        lada = enLada.Text
                    };
                         
                    ClienteRest client = new ClienteRest();
                    if (!String.IsNullOrEmpty(enTelefono.Text) && !(enTelefono.Text.Length < 7)&& !(enTelefono.Text.Length > 10))
                    {

                        auth = true;

                    }
                    else
                    {
                        auth = false;
                        await DisplayAlert("Error", "Teléfono Invalido", "ok");
                    }
                  

                    if (!String.IsNullOrEmpty(enLada.Text) && !(enLada.Text.Length < 2))
                    {

                        auth2 = true;

                    }
                    else
                    {
                        auth2 = false;
                        await DisplayAlert("Error", "LADA incorrecta", "ok");
                    }

                    if(auth&&auth2){

                        client.PUT(Constantes.URL_USUARIOS+"/telefonos/modificar", modtel);
                        MessagingCenter.Subscribe<ClienteRest>(this, "OK", (Sender) => {
                            DisplayAlert("Guardado", "¡Teléfono Modificado con Exito!", "OK");
                            MessagingCenter.Unsubscribe<ClienteRest>(this,"OK");
                            Navigation.PopAsync();
                        });

                        MessagingCenter.Subscribe<ClienteRest>(this, "error", (Sender) => {
                            DisplayAlert("Error", "¡No fué posible Modificar el Teléfono!", "OK");
                            MessagingCenter.Unsubscribe<ClienteRest>(this, "error");

                        });
                    }
                    
                }

            }

        }

        /// <summary>
        /// evento del boton modificar que llama al evento conectar();
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void Handle_Clicked(object sender, System.EventArgs e)
        {
            conectar();
        }


        /// <summary>
        /// evento click del botón agregar para consumir al servicio de agregar un nuevo teléfono
        /// </summary>
        /// <param name="sender">objeto que hace referencia al evento</param>
        /// <param name="e">argumentos que son posibles de obtener apartir del objeto que hace llamada al evento</param>
         async void Agregar_Clicked(object sender, System.EventArgs e)
        {
            ClienteRest cliente = new ClienteRest();
            var inf = await cliente.InfoUsuario<InfoUsuario>(Application.Current.Properties["token"] as string);
            if (inf != null)
            {
                Modelos.ModificarTelefono modtel = new Modelos.ModificarTelefono();


                modtel.persona = inf.persona;
                bool auth = false, auth2 = false, auth3 = false;


                modtel.telefono = new Telefono
                {
                   
                    tipo = pkTipo.Items[pkTipo.SelectedIndex],
                    telefono = enTelefono.Text,
                    lada = enLada.Text
                };

                ClienteRest client = new ClienteRest();
                if (!String.IsNullOrEmpty(enTelefono.Text) && !(enTelefono.Text.Length < 7)&&!(enTelefono.Text.Length >10))
                {

                    auth = true;

                }
                else
                {
                    auth = false;
                    await DisplayAlert("Error", "Teléfono Invalido", "ok");
                }


                if (!String.IsNullOrEmpty(enLada.Text) && !(enTelefono.Text.Length < 2))
                {

                    auth2 = true;

                }
                else
                {
                    auth2 = false;
                    await DisplayAlert("Error", "LADA incorrecta", "ok");
                }


                if (pkTipo.SelectedIndex>-1)
                {

                    auth3 = true;

                }
                else
                {
                    auth3 = false;
                    await DisplayAlert("Error", "Seleccione el tipo de teléfono", "ok");
                }

                if (auth && auth2&&auth3)
                {

                    client.POST(Constantes.URL_USUARIOS + "/telefono/agregar", modtel,1);
                    MessagingCenter.Subscribe<ClienteRest>(this, "OK", (Sender) => {
                        DisplayAlert("Guardado", "¡Teléfono Añadido con Exito!", "OK");
                        MessagingCenter.Unsubscribe<ClienteRest>(this, "OK");
                        Navigation.PopAsync();
                    });

                    MessagingCenter.Subscribe<ClienteRest>(this, "error", (Sender) => {
                        DisplayAlert("Error", "¡No fué posible añadir el Teléfono!", "OK");
                        MessagingCenter.Unsubscribe<ClienteRest>(this, "error");

                    });
                }

            }

        }

        /// <summary>
        /// valida dinamicamente que solo se puedan ingresar ciertos caracteres en la entrada LADA 
        /// 
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Arguments.</param>
        public void OnLadaChanged(object sender, TextChangedEventArgs args)
        {
            if (!Regex.IsMatch(args.NewTextValue, "^[0-9]+$", RegexOptions.CultureInvariant))
                (sender as Entry).Text = Regex.Replace(args.NewTextValue, "[^0-9]", string.Empty);
            Entry entry = sender as Entry;

            String val = entry.Text;

            if (val.Length > 3)
            {
                val = val.Remove(val.Length - 1);
                entry.Text = val;
                enTelefono.Focus();
            }
        }

        /// <summary>
        /// valida dinámicamente que solo se puedan ingresar ciertos caracteres en la entreda teléfono
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Arguments.</param>
        public void OnTelefonoChanged(object sender, TextChangedEventArgs args)
        {
            if (!Regex.IsMatch(args.NewTextValue, "^[0-9]+$", RegexOptions.CultureInvariant))
                (sender as Entry).Text = Regex.Replace(args.NewTextValue, "[^0-9]", string.Empty);
            Entry entry = sender as Entry;
            String val = entry.Text;

            if (val.Length > 10)
            {
                val = val.Remove(val.Length - 1);
                entry.Text = val;
            }
        }
        }
         
    }

    //http://192.168.0.18:8080/api/telefonos/modificar





