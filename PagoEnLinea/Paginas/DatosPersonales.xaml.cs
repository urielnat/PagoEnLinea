using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using PagoEnLinea.Modelos;
using PagoEnLinea.servicios;
using Xamarin.Forms;

namespace PagoEnLinea.Paginas
{
    /// <summary>
    /// Es clase muestra una pantalla al usuario con sus datos personales (nombre, apellido, sexto etc.)
    /// permitiendole visulizarlos y modificarlos al consumir diversos servicios.
    /// </summary>
    public partial class DatosPersonales : ContentPage
    {
        
        Persona persona = new Persona();

        /// <summary>
        /// inicializa los componentes visuales del XAML
        /// añade un evento de tipo TextChanged
        /// al componente dtFecha se le establce como fecha máxima ha seleccionar la fecha actual
        /// </summary>
        public DatosPersonales()
        {
            InitializeComponent();
            enCurp.TextChanged += MayusChanged;
            dtFecha.MaximumDate = DateTime.Now;
          
        }



        /// <summary>
        /// Consume al servicio que obtiene la información del usuario mediante una instancia de la clase CLienteRest
        /// y llena los campos correspondientes
        /// </summary>
        async void conectar()
        {
            if (Application.Current.Properties.ContainsKey("token"))
            {


                ClienteRest cliente = new ClienteRest();
                var inf = await cliente.InfoUsuario<InfoUsuario>(Application.Current.Properties["token"] as string);
                if (inf != null)
                {
                    
                    enNombre.Text = inf.persona.nombre;
                    enPaterno.Text = inf.persona.apaterno;
                    enMaterno.Text = inf.persona.amaterno;
                    enCurp.Text = inf.persona.curp;
                    persona.id = inf.persona.id;
                    dtFecha.Date = DateTime.ParseExact(inf.persona.fechanac, "yyyy-MM-dd", null);
                    for(var i = 0;i < pkEstCvl.Items.Count;i++)
                    {
                        if (pkEstCvl.Items[i].Equals(inf.persona.edoCivil)){
                            pkEstCvl.SelectedIndex = i; 
                        }
                     }
                    for (var i = 0; i < pkSexo.Items.Count; i++)
                    {
                        if (pkSexo.Items[i].Equals(inf.persona.sexo))
                        {
                            pkSexo.SelectedIndex = i;
                        }
                    }
                }

            }

        }

        /// <summary>
        /// llama al método conectar para mostrar cada vez que esta pantalla se muestra.
        /// </summary>
        protected override void OnAppearing()
        {
            conectar();
        }


        /// <summary>
        /// añade un evento de tipo click al boton modificar que permite habilitar las entradas de texto
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
       void Handle_Clicked(object sender, System.EventArgs e)
        {
            //var action = await DisplayActionSheet("Modificar datos", "Cancelar", "Eliminar", "Modificar", "Agregar");
            //Debug.WriteLine("Action: " + action);
            enNombre.IsEnabled = true;
            enPaterno.IsEnabled = true;
            enMaterno.IsEnabled = true;
            enCurp.IsEnabled = true;
            dtFecha.IsEnabled = true;
            pkSexo.IsEnabled = true;
            pkEstCvl.IsEnabled = true;

            btnGuardar.IsEnabled = true;
            btnModificar.IsEnabled = false;
            btnContraseña.IsEnabled = false;

            btnGuardar.BackgroundColor = Color.FromHex("#5CB85C");
            btnModificar.BackgroundColor = Color.Silver;
            btnContraseña.BackgroundColor = Color.Silver;
        }

        /// <summary>
        /// añade un evento de tipo click al presionar el boton modficar contraseña el cual muestra al usuario pantalla
        /// para modificarla
        /// </summary>
        /// <param name="sender">objeto que hace referencia al evento</param>         /// <param name="e">argumentos que son posibles de obtener apartir del objeto que hace llamada al evento</param>
        async void contraseña_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ModificarContraseña());       
        }


        /// <summary>
        /// evento click al presionar el boton guardar, verifica que los campos añadidos sean correctos antes de
        /// consumir el servicio para modificarlos
        /// </summary>
        /// <param name="sender">objeto que hace referencia al evento</param>         /// <param name="e">argumentos que son posibles de obtener apartir del objeto que hace llamada al evento</param>
         void modificar_Clicked(object sender, System.EventArgs e)
        {


            Boolean a1 = false, a2 = false, a3 = false;

            if (string.IsNullOrEmpty(enCurp.Text)|| !Regex.Match(enCurp.Text, "[A-Z][A,E,I,O,U,X][A-Z]{2}[0-9]{2}[0-1][0-9][0-3][0-9][M,H][A-Z]{2}[B,C,D,F,G,H,J,K,L,M,N,Ñ,P,Q,R,S,T,V,W,X,Y,Z]{3}[0-9,A-Z][0-9]").Success)
            {
                DisplayAlert("Campo incorrecto", "Introduzca un CURP valido", "ok");
                a1 = false;
            }
            else
            {

                a1 = true;
            }


            if (string.IsNullOrEmpty(enNombre.Text) || !Regex.Match(enNombre.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚ\s]+$").Success)
            {
                DisplayAlert("Campo incorrecto","Introduzca un nombre valido","ok");
                a2 = false;
            }
            else
            {
                
                a2 = true;
            }
            if (string.IsNullOrEmpty(enPaterno.Text) || !Regex.Match(enPaterno.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚ\s]+$").Success)
            {
                DisplayAlert("Campo incorrecto", "Introduzca un apellido valido", "ok");
                a3 = false;

            }
            else
            {
                
                a3 = true;
            }




            if(a1&&a2&&a3){


            ClienteRest client = new ClienteRest();
         
            persona.nombre = enNombre.Text;
            persona.apaterno = enPaterno.Text;
            persona.amaterno = enMaterno.Text;
            persona.curp = enCurp.Text;
            persona.sexo = pkSexo.Items[pkSexo.SelectedIndex];
            persona.edoCivil = pkEstCvl.Items[pkEstCvl.SelectedIndex];
            persona.fechanac = dtFecha.Date.ToString("yyyy-MM-dd");

            enNombre.IsEnabled = false;
            enPaterno.IsEnabled = false;
            enMaterno.IsEnabled = false;
            enCurp.IsEnabled = false;
            dtFecha.IsEnabled = false;
            pkSexo.IsEnabled = false;
            pkEstCvl.IsEnabled = false;

           

                client.PUT(Constantes.URL_USUARIOS+"/personas/actualizar", persona);

            MessagingCenter.Subscribe<ClienteRest>(this, "OK", (send) =>
            {
                DisplayAlert("Guardado", "¡Datos Modificados con Exito!", "OK");
                MessagingCenter.Unsubscribe<ClienteRest>(this,"OK");
                    btnGuardar.IsEnabled = false;
                btnModificar.IsEnabled = true;
                    btnModificar.BackgroundColor = Color.FromHex("#5CB85C");
                    btnContraseña.IsEnabled = true;
                    btnContraseña.BackgroundColor = Color.FromHex("#3f85bd");                                         

                btnGuardar.BackgroundColor = Color.Silver;

            });

            MessagingCenter.Subscribe<ClienteRest>(this, "error", (Sender) => {
                    
                DisplayAlert("Error", "¡No fue posible modifcar los datos!", "OK");
                    MessagingCenter.Unsubscribe<ClienteRest>(this, "error");
                    btnGuardar.IsEnabled = false;
                btnModificar.IsEnabled = true;
                btnModificar.BackgroundColor = Color.FromHex("#5CB85C");
                btnGuardar.BackgroundColor = Color.Silver;
                    btnContraseña.IsEnabled = true;
                    btnContraseña.BackgroundColor = Color.FromHex("#3f85bd");    

            });


            }else{
                btnGuardar.IsEnabled = true;
                btnModificar.IsEnabled = false;
                btnGuardar.BackgroundColor = Color.FromHex("#5CB85C");
                btnModificar.BackgroundColor = Color.Silver;
                btnContraseña.IsEnabled = true;
                    btnContraseña.BackgroundColor = Color.FromHex("#3f85bd");    
            }
           
        }

        /// <summary>
        /// evento que permite cambiar dinámicamente las letras a mayusculas ingresadas en el teclado virtual
        /// es usado por la entrada CURP
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void MayusChanged(object sender, TextChangedEventArgs e)
        {

            (sender as Entry).Text = e.NewTextValue.ToUpper();

        }
    }
}
 