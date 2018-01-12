using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PagoEnLinea.Modelos;
using PagoEnLinea.servicios;
using Xamarin.Forms;

namespace PagoEnLinea.Paginas
{

    /// <summary>
    /// Esta clase muestra una pantalla ya sea para modificar o añadir un nuevo correo electrónico
    /// según la el tipo de parámetros que recibe ya que muestra diferentes botones con eventos asociados.
    /// </summary>
    public partial class ModificarCorreo : ContentPage
    {
        string ID;

        /// <summary>
        /// inicializa los componentes visuales de su XAML
        /// muestra u oculta componentes según el tipo de parametro recibido
        /// </summary>
        /// <param name="id">id de el correo a modificar</param>
        /// <param name="tipo">tipo de pantalla que se mostrará, 0 para tipo modificar 1 para tipo añadir</param>
        public ModificarCorreo(string id,int tipo)
        {
            ID = id;
            
            InitializeComponent();
            if (tipo == 0)
            {
                btnModificar.IsVisible = true;
                btnAgregar.IsVisible = false;
            }
            if (tipo == 1)
            {
                enTipo.Text = "PERSONAL";
                btnAgregar.IsVisible = true;
                btnModificar.IsVisible = false;
            }
           
        }

        /// <summary>
        /// método usado para consumir al servicio de modificar un correo electrónico
        /// </summary>
        void conectar()
        {
            if (string.IsNullOrEmpty(enCorreo.Text) || !Regex.Match(enCorreo.Text, "^(?(\")(\".+?(?<!\\\\)\"@)|(([0-9A-Za-z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9A-Za-z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-z][-\\w]*[0-9a-z]*\\.)+[a-z0-9][\\-a-z0-9]{0,22}[a-z0-9]))$").Success)
            {
                DisplayAlert("Advertencia", "Introduzca un correo electrónico valido", "OK");

            }
            else
            { 
            ClienteRest cliente = new ClienteRest();
            Email email = new Email();
            email.id = ID;
            email.correoe = enCorreo.Text;
            email.tipo = enTipo.Text;

            cliente.PUT(Constantes.URL_USUARIOS + "/email/actualizar-email", email);
            MessagingCenter.Subscribe<ClienteRest>(this, "OK", (sender) =>
            {
                MessagingCenter.Unsubscribe<ClienteRest>(this, "OK");
                DisplayAlert("Guardado", "¡Correo Modificado con Exito!", "OK");
                Navigation.PopAsync();
            });

            MessagingCenter.Subscribe<ClienteRest>(this, "error", (Sender) =>
            {
                MessagingCenter.Unsubscribe<ClienteRest>(this, "error");
                DisplayAlert("Error", "¡No fué posible modifcar el correo!", "OK");

            });
        }
        }

        /// <summary>
        /// evento click de boton modificar que hace una llamada al método conectar();
        /// </summary>
        /// <param name="sender">objeto que hace referencia al evento</param>         /// <param name="e">argumentos que son posibles de obtener apartir del objeto que hace llamada al evento</param>
        void Handle_Clicked(object sender, System.EventArgs e)
        {
            conectar();

        }


        /// <summary>
        /// evento que permite seleccionar una direccion de la lista y elegir la opcion de modificarlo o eliminarlo
        /// unicamente en android se hará uso del cuadro de diálogo personalizado ya que en iOS
        /// sus configuraciones nativas no lo requieren. Así mismo consume los servicios correspondientes.
        /// </summary>
        /// <param name="sender">objeto que hace referencia al evento</param>         /// <param name="e">argumentos que son posibles de obtener apartir del objeto que hace llamada al evento</param>
        async void Agregar_Clicked(object sender, System.EventArgs e)
        {

            if (string.IsNullOrEmpty(enCorreo.Text) || !Regex.Match(enCorreo.Text, "^(?(\")(\".+?(?<!\\\\)\"@)|(([0-9A-Za-z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9A-Za-z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-z][-\\w]*[0-9a-z]*\\.)+[a-z0-9][\\-a-z0-9]{0,22}[a-z0-9]))$").Success)
            {
                await DisplayAlert("Advertencia","Introduzca un correo electrónico valido","OK");

            }
            else
            {       
            if (Application.Current.Properties.ContainsKey("token"))
            {


                ClienteRest cliente = new ClienteRest();
                var inf = await cliente.InfoUsuario<InfoUsuario>(Application.Current.Properties["token"] as string);
                if (inf != null)
                {
                    AgregarCorreo addemail = new AgregarCorreo();
                    addemail.persona = inf.persona;
                    addemail.email = new Email
                    {
                        correoe = enCorreo.Text,
                        tipo = "PERSONAL"
                    };
                    ClienteRest client = new ClienteRest();



                    client.POST(Constantes.URL_USUARIOS + "/email/agregar", addemail, 1);
                    MessagingCenter.Subscribe<ClienteRest>(this, "OK", (Sender) =>
                    {
                        DisplayAlert("Guardado", "¡Correo añadido con exito!", "OK");
                        Navigation.PopAsync();
                        MessagingCenter.Unsubscribe<ClienteRest>(this, "OK");
                    });

                    MessagingCenter.Subscribe<ClienteRest>(this, "error", (Sender) =>
                    {
                        DisplayAlert("Error", "¡No fué posible añadir el correo actual", "OK");
                        MessagingCenter.Unsubscribe<ClienteRest>(this, "error");

                    });

                }

            }
        }
        }
    }
}
