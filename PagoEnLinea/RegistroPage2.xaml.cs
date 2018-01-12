using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FullCameraPage;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Newtonsoft.Json;
using PagoEnLinea.Modelos;
using PagoEnLinea.servicios;
using Plugin.Connectivity;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;


namespace PagoEnLinea
{
    /// <summary>
    /// esta clase corresponde a la segunda de dos pantallas para el registro de un usuario
    /// añade la funcionalidad de OCR aunque la mayoria de campos ya son autocompletados al ingresar el código postal
    /// ya que en su mayoria son campos correspondientes a direcciones
    /// </summary>
    public partial class RegistroPage2 : ContentPage
    {
        public static List<string> tipoas;
        public Usuarios user { set; get; }
        public Direccion dire { set; get; }
        public CatalogoDir catdire { set; get; }
        public Direccion direc = new Direccion();
        public static bool cargaCP = false;


        /// <summary>
        /// en el constructor se la asignan diferentes eventos a las entradas de texto:
        /// permite detectar cuando cuando el usuario teclea nuevamente encaso de que
        /// la interfaz le muestre un error para borrarlo dinámicamente
        /// asi mismo muestra un error en caso de detectarlo al presionar la tecla de retorno
        /// o cambiar el foco de la entrada
        /// obtiene información de la pantalla anterior para poder consumir el servicio de dar de alta un usuario
        /// ademas de que  si se hizo uso del OCR desde la pantalla anterior algunos campos son compleatados
        /// actualmente esto esta desabilitado para estos y otros datos al cargar el catálogo de direcciones
        /// con el código postal
        /// </summary>
        /// <param name="u">objeto de tipo usuario contiene toda la información capturada de la pantalla anterior</param>
        /// <param name="d">contiene toda la informacion de direcciones capturada por el OCR para manipularla mas facilmente</param>
        /// <param name="cd">contiene toda la informacion del catálogo de direcciones captura por el OCR</param>
        public RegistroPage2(Usuarios u, Direccion d, CatalogoDir cd)
        {
            user = u;
           catdire = cd;

            InitializeComponent();
           
            enCod.TextChanged += OnTextChanged;
            enNumero.TextChanged += validarNumero;
            enNumInt.TextChanged += validarNumero;

            enColonia.TextChanged += borrarError;
            enDomicilio.TextChanged += borrarError;
            enCiudad.TextChanged += borrarError;
            enTelefono.TextChanged += borrarError;
            enTipoAsentamiento.TextChanged += borrarError;
            enEstado.TextChanged += borrarError;
           // enDomicilio.Text = d.calle;
            enLADA.TextChanged += OnLadaChanged;
            enLADA2.TextChanged += OnLada2Changed;
            var jsonstring = JsonConvert.SerializeObject(u);
            //System.Diagnostics.Debug.WriteLine(jsonstring);
            jsonstring = jsonstring.Substring(1, jsonstring.Length - 2);

            //pkpais.SelectedIndex = 0;
            enTelefono.TextChanged += OnTelefonoChanged;
            enCelular.TextChanged += OnTelefonoChanged;
            //llenarPicker();
           // enColonia.Text = cd.asentamiento;
          //  enCod.Text = cd.cp;
           // enNumero.Text = d.numero;

           
            if (Application.Current.Properties.ContainsKey("lada"))
            {
                enDomicilio.Text = Application.Current.Properties["calle"] as string;
                enNumero.Text = Application.Current.Properties["numero"] as string;
                enNumInt.Text = Application.Current.Properties["numeroInterior"] as string;


                enCiudad.Text = Application.Current.Properties["ciudad"] as string;
                enEstado.Text = Application.Current.Properties["estado"] as string;
                enMunicipio.Text = Application.Current.Properties["municipio"] as string;
                enTipoAsentamiento.Text = Application.Current.Properties["tipoAsentamiento"] as string;
                enPais.Text = Application.Current.Properties["pais"] as string;
                enTelefono.Text = Application.Current.Properties["telefono"] as string;
                enLADA.Text = Application.Current.Properties["lada"] as string;
                enCelular.Text = Application.Current.Properties["celular"] as string;
                enLADA2.Text = Application.Current.Properties["lada2"] as string;


            }

            enDomicilio.Completed += verificarError;
            enNumero.Completed += verificarError;
            enNumInt.Completed += verificarError;
            enCod.Completed += verificarError;
            enPais.Completed += verificarError;
            enCiudad.Completed += verificarError;
            enEstado.Completed += verificarError;
            enMunicipio.Completed += verificarError;
            enTipoAsentamiento.Completed += verificarError;
            enColonia.Completed += verificarError;
            enLADA.Completed += verificarError;
            enTelefono.Completed += verificarError;
            enCelular.Completed += verificarError;
            enLADA2.Completed += verificarError;


            enDomicilio.Unfocused += UnfocusError;
            enNumero.Unfocused += UnfocusError;
            enNumInt.Unfocused += UnfocusError;
            enCod.Unfocused += UnfocusError;
            enPais.Unfocused += UnfocusError;
            enCiudad.Unfocused += UnfocusError;
            enEstado.Unfocused += UnfocusError;
            enMunicipio.Unfocused += UnfocusError;
            enTipoAsentamiento.Unfocused += UnfocusError;
            enColonia.Unfocused += UnfocusError;
            enLADA.Unfocused += UnfocusError;
            enTelefono.Unfocused += UnfocusError;
            enCelular.Unfocused += UnfocusError;
            enLADA2.Unfocused += UnfocusError;

            enCelular.Focused += OnFocus;
            enTelefono.Focused += OnFocus;

            //scroll2.Scrolled += onScrolled;
           
        }

       
        /// <summary>
        /// sube el scroll automáticamente en caso de que el usuario este posisionado en el campo LADA del teléfono celular
        /// </summary>
        /// <param name="sender">objeto que hace referencia al evento</param>
        /// <param name="e">argumentos que son posibles de obtener apartir del objeto que hace llamada al evento</param>
       
        private async void OnFocus(object sender, FocusEventArgs e)
        {

            string text = (sender as Entry).Text;
            (sender as Entry).Text = "";
            (sender as Entry).Text = text;
            if(sender==enTelefono){

                await Task.Yield();
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                    var heightContentScroll = scroll2.ContentSize.Height;
                    await Task.Yield();
                    await scroll2.ScrollToAsync(enLADA2, ScrollToPosition.Start, true);
                    });
                
            
            }

           


        }


        /// <summary>
        /// Unfocuses the error.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private  void UnfocusError(object sender, FocusEventArgs e)
        {
            if (sender == enDomicilio)
            {
                if (ValidarVacio(enDomicilio.Text) || !Regex.Match(enDomicilio.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚüÜ\s0-9]+$").Success)
                {
                    enDomicilio.ErrorText = "Introduzca una calle valida";

                }
                else
                {
                    enDomicilio.ErrorText = "";
                }
            }
            else if (sender == enNumero)
            {
                if (ValidarVacio(enNumero.Text) || !Regex.Match(enNumero.Text, "^[a-zA-Z0-9/-]*$").Success)
                {
                    lblnum.TextColor = Xamarin.Forms.Color.Red;

                }
                else
                {
                    lblnum.TextColor = Xamarin.Forms.Color.Black;
                }
            }
            else if (sender == enCod)
            {
                if (ValidarVacio(enCod.Text) || enCod.Text.Length < 5)
                {
                    lblCod.TextColor = Xamarin.Forms.Color.Red;

                }
                else
                {
                    lblCod.TextColor = Xamarin.Forms.Color.Black;

                }
            }
            else if (sender == enCiudad)
            {
                if (!string.IsNullOrEmpty(enCiudad.Text))
                {
                    if (!Regex.Match(enCiudad.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚúÜü\s0-9]+$").Success)
                    {
                        enCiudad.ErrorText = "Introduza una ciudad valida";

                    }
                    else
                    {
                        enCiudad.ErrorText = "";

                    }
                }
            }
            else if (sender == enEstado)
            {
                if (ValidarVacio(enEstado.Text) || !Regex.Match(enEstado.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚÜü\s0-9]+$").Success)
                {
                    enEstado.ErrorText = "Introduzca un estado valido";


                }
                else
                {
                    enEstado.ErrorText = "";

                }
            }
            else if (sender == enMunicipio)
            {
                if (ValidarVacio(enMunicipio.Text) || !Regex.Match(enMunicipio.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚÜü\s0-9]+$").Success)
                {
                    enMunicipio.ErrorText = "Introduzca un municipio valido";
                }
                else
                {
                    enMunicipio.ErrorText = "";

                }

            }
            else if (sender == enNumInt)
            {
                if (!string.IsNullOrEmpty(enNumInt.Text))
                {
                    if (!Regex.Match(enNumInt.Text, "^[a-zA-Z0-9/-]*$").Success)
                    {
                        lblNumInt.TextColor = Xamarin.Forms.Color.Red;
                    }
                    else
                    {
                        lblNumInt.TextColor = Xamarin.Forms.Color.Black;
                    }
                }
                else
                {
                    lblNumInt.TextColor = Xamarin.Forms.Color.Black;
                }
            }
            else if (sender == enTipoAsentamiento)
            {
                if (ValidarVacio(enTipoAsentamiento.Text) || !Regex.Match(enTipoAsentamiento.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚ\s0-9]+$").Success)
                {
                    enTipoAsentamiento.ErrorText = "Introduzca un tipo valido";


                }
                else
                {
                    enTipoAsentamiento.ErrorText = "";

                }
            }
            else if (sender == enPais)
            {
                if (ValidarVacio(enPais.Text))
                {
                    enPais.ErrorText = "Introduzca su país";


                }
                else
                {
                    enPais.ErrorText = "";

                }
            }
            else if (sender == enTelefono)
            {
                
               
                if (ValidarVacio(enTelefono.Text) || enTelefono.Text.Length < 7 || enTelefono.Text.Length > 10)
                {
                    
                    enTelefono.ErrorText = "Introduzca un teléfono valido";


                }
                else
                {
                    enTelefono.ErrorText = "";

                }
            }
            else if (sender == enLADA)
            {
                if (ValidarVacio(enLADA.Text) || enLADA.Text.Length < 2)
                {
                    enLADA.ErrorText = "LADA";

                }
                else
                {
                    enLADA.ErrorText = "";

                }
            }
            else if (sender == enCelular)
            {
                
               
                if (!string.IsNullOrEmpty(enCelular.Text))
                {
                    if (enCelular.Text.Length < 7 || enCelular.Text.Length > 10)
                    {

                        enCelular.ErrorText = "Celular incorrecto";

                    }
                    else
                    {

                        enCelular.ErrorText = "";
                    }
                }
                else
                {

                    enCelular.ErrorText = "";
                }
            }
            else if (sender == enLADA2)
            {


                if (!string.IsNullOrEmpty(enLADA2.Text))
                {
                    if (enLADA2.Text.Length < 2)
                    {

                        enLADA2.ErrorText = "LADA incorrecta";
                    }
                    else
                    {

                        enLADA2.ErrorText = "";
                    }
                }
                else
                {

                    enLADA2.ErrorText = "";
                }

            }
        }

        /// <summary>
        /// método que permite detectar dinámicamente si existe algun error en un campo al oprimir
        /// la tecla de retorno en el teclado virtual
        /// </summary>
        /// <param name="sender">objeto que hace referencia al evento</param>
        /// <param name="e">argumentos que son posibles de obtener apartir del objeto que hace llamada al evento</param>
      
        private async void verificarError(object sender, EventArgs e)
        {
            if (sender == enDomicilio)
            {
                
                if (ValidarVacio(enDomicilio.Text) || !Regex.Match(enDomicilio.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚüÜ\s0-9]+$").Success)
                {
                    enDomicilio.ErrorText = "Introduzca una calle valida";

                }
                else
                {
                    enDomicilio.ErrorText = "";
                }

            }
            else if (sender == enNumero)
            {
                if (ValidarVacio(enNumero.Text) || !Regex.Match(enNumero.Text, "^[a-zA-Z0-9/-]*$").Success)
                {
                    lblnum.TextColor = Xamarin.Forms.Color.Red;

                }
                else
                {
                    lblnum.TextColor = Xamarin.Forms.Color.Black;
                }
            }
            else if (sender == enCod)
            {
                if (ValidarVacio(enCod.Text) || enCod.Text.Length < 5)
                {
                    lblCod.TextColor = Xamarin.Forms.Color.Red;

                }
                else
                {
                    lblCod.TextColor = Xamarin.Forms.Color.Black;

                }
            }
            else if (sender == enCiudad)
            {
                if (!string.IsNullOrEmpty(enCiudad.Text))
                {
                    if (!Regex.Match(enCiudad.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚúÜü\s0-9]+$").Success)
                    {
                        enCiudad.ErrorText = "Introduza una ciudad valida";

                    }
                    else
                    {
                        enCiudad.ErrorText = "";

                    }
                }
            }
            else if (sender == enEstado)
            {
                if (ValidarVacio(enEstado.Text) || !Regex.Match(enEstado.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚÜü\s0-9]+$").Success)
                {
                    enEstado.ErrorText = "Introduzca un estado valido";


                }
                else
                {
                    enEstado.ErrorText = "";

                }
            }
            else if (sender == enMunicipio)
            {
                if (ValidarVacio(enMunicipio.Text) || !Regex.Match(enMunicipio.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚÜü\s0-9]+$").Success)
                {
                    enMunicipio.ErrorText = "Introduzca un municipio valido";
                }
                else
                {
                    enMunicipio.ErrorText = "";

                }

            }else if(sender==enNumInt){
                if (!string.IsNullOrEmpty(enNumInt.Text))
                {
                    if (!Regex.Match(enNumInt.Text, "^[a-zA-Z0-9/-]*$").Success)
                    {
                        lblNumInt.TextColor = Xamarin.Forms.Color.Red;
                    }
                    else
                    {
                        lblNumInt.TextColor = Xamarin.Forms.Color.Black;
                    }
                }else{
                    lblNumInt.TextColor = Xamarin.Forms.Color.Black;
                }
            }else if(sender==enTipoAsentamiento){
                if (ValidarVacio(enTipoAsentamiento.Text) || !Regex.Match(enTipoAsentamiento.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚ\s0-9]+$").Success)
                {
                    enTipoAsentamiento.ErrorText = "Introduzca un tipo valido";
                   

                }
                else
                {
                    enTipoAsentamiento.ErrorText = "";
                   
                }
            }else if (sender==enPais){
                if (ValidarVacio(enPais.Text))
                {
                    enPais.ErrorText = "Introduzca su país";


                }
                else
                {
                    enPais.ErrorText = "";
                  
                }
            }else if(sender==enTelefono){
                await Task.Yield();
                await scroll2.ScrollToAsync(enCelular, ScrollToPosition.End, true);
                if (ValidarVacio(enTelefono.Text) || enTelefono.Text.Length < 7 || enTelefono.Text.Length > 10)
                {
                    
                    enTelefono.ErrorText = "Introduzca un teléfono valido";
                   

                }
                else
                {
                    enTelefono.ErrorText = "";

                }
            }else if (sender==enLADA){
                if (ValidarVacio(enLADA.Text) || enLADA.Text.Length < 2)
                {
                    enLADA.ErrorText = "LADA";
                   
                }
                else
                {
                    enLADA.ErrorText = "";

                }
            }else if(sender==enCelular){
                await Task.Yield();
                await scroll2.ScrollToAsync(enCelular, ScrollToPosition.End, true);
               
                if (!string.IsNullOrEmpty(enCelular.Text))
                {
                    if (enCelular.Text.Length < 7 || enCelular.Text.Length > 10)
                    {
                       
                        enCelular.ErrorText = "Celular incorrecto";

                    }
                    else
                    {
                        
                        enCelular.ErrorText = "";
                    }
                }
                else
                {
                    
                    enCelular.ErrorText = "";
                }
            }else if(sender==enLADA2){


            if (!string.IsNullOrEmpty(enLADA2.Text))
                {
                    if (enLADA2.Text.Length < 2)
                    {
                        
                        enLADA2.ErrorText = "LADA";
                    }
                    else
                    {
                     
                        enLADA2.ErrorText = "";
                    }
                }
                else
                {
                   
                    enLADA2.ErrorText = "";
                }

            }
        }
       

        /// <summary>
        /// permite cargar los datos ya ingresados en caso de regresar a la pantalla anterior y volver a esta
        /// 
        /// </summary>
        protected override  void OnAppearing()
        {   
            base.OnAppearing();

          

            if (Application.Current.Properties.ContainsKey("lada"))
            { 
                enDomicilio.Text = Application.Current.Properties["calle"] as string;
                enNumero.Text = Application.Current.Properties["numero"] as string;
                enNumInt.Text = Application.Current.Properties["numeroInterior"] as string;
               

                enCiudad.Text= Application.Current.Properties["ciudad"] as string;
                enEstado.Text= Application.Current.Properties["estado"] as string;
                enMunicipio.Text = Application.Current.Properties["municipio"] as string;
                enTipoAsentamiento.Text=Application.Current.Properties["tipoAsentamiento"] as string ;
                enPais.Text =Application.Current.Properties["pais"] as string ;
                enTelefono.Text = Application.Current.Properties["telefono"] as string;
                enLADA.Text= Application.Current.Properties["lada"] as string;
                enCelular.Text = Application.Current.Properties["celular"] as string;
                enLADA2.Text =  Application.Current.Properties["lada2"] as string;
            
            
            }
            //scroll2.ScrollToAsync(enDomicilio, ScrollToPosition.MakeVisible, true);
         
        }
        /// <summary>
        /// valida que solo se puedan ingresar números en la lada del celular y hacer el salto automático 
        /// al la entrada celular
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Arguments.</param>
        private void OnLada2Changed(object sender, TextChangedEventArgs args)
        {
            if (!Regex.IsMatch(args.NewTextValue, "^[0-9]+$", RegexOptions.CultureInvariant))
                (sender as Xfx.XfxEntry).Text = Regex.Replace(args.NewTextValue, "[^0-9]", string.Empty);
            Xfx.XfxEntry entry = sender as Xfx.XfxEntry;
            entry.ErrorText = "";
            String val = entry.Text;

            if (val.Length > 3)
            {
                var temp = val.Substring(3);
                val = val.Remove(val.Length - 1);
                entry.Text = val;


                enCelular.Focus();
                enCelular.Text = temp;
            }

        }


        /// <summary>
        /// evento del boton registrar valida que todos los campos ingresados sean correctos
        /// y en caso afirmativo consume al servicio de dar de alta un nuevo usuario
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async void registrar_Clicked(object sender, System.EventArgs e)
        {
            bool a1 = false, a2 = false, a3 = false, a4 = false, a5 = false, a6 = false, a7 = false, a8 = false, a9 = false, a10 = false, a11 = true, a12 = false;
            bool comodin = true, comodin2 = true, com = false, com2 = false;

            if (!string.IsNullOrEmpty(enCiudad.Text))
            {
                if (!Regex.Match(enCiudad.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚ\s0-9]+$").Success)
                {
                    enCiudad.ErrorText = "Introduza una ciudad valida";
                    await DisplayAlert("Advertencia", "Introduza una ciudad valida", "OK");
                    comodin = false;
                }
                else
                {
                    enCiudad.ErrorText = "";
                    comodin = true;
                }
            }
            else
            {
                comodin = true;
            }

            if (!string.IsNullOrEmpty(enNumInt.Text))
            {
                if (!Regex.Match(enNumInt.Text, "^[a-zA-Z0-9/-]*$").Success)
                {
                    comodin2 = false;
                }
                else
                {
                    comodin2 = true;
                }
            }
            else
            {
                comodin2 = true;
            }





            if (!string.IsNullOrEmpty(enLADA2.Text))
            {
                if (enLADA2.Text.Length < 2)
                {
                    com = false;
                    await DisplayAlert("Advertencia", "LADA incorrecta", "OK");
                    enLADA2.ErrorText = "LADA";
                }
                else
                {
                    com = true;
                    enLADA2.ErrorText = "";
                }
            }
            else
            {
                com = false;
                enLADA2.ErrorText = "";
            }


            if (!string.IsNullOrEmpty(enCelular.Text))
            {
                if (enCelular.Text.Length < 7 || enCelular.Text.Length > 10)
                {
                    com2 = false;
                    enCelular.ErrorText = "Celular incorrecto";
                    await DisplayAlert("Advertencia", "Celular incorrecto", "OK");

                }
                else
                {
                    com2 = true;
                    enCelular.ErrorText = "";
                }
            }
            else
            {
                com2 = false;
                enCelular.ErrorText = "";
            }


            if (string.IsNullOrEmpty(enColonia.Text) || !Regex.Match(enColonia.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚüÜ\s0-9]+$").Success)
            {
                enColonia.ErrorText = "Introduzca un asentamiento valido";
                await DisplayAlert("Advertencia", "Introduzca un asentamiento valido", "OK");
                a1 = false;

            }
            else
            {
                enColonia.ErrorText = "";
                a1 = true;
            }

            if (ValidarVacio(enDomicilio.Text) || !Regex.Match(enDomicilio.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚüÜ\s0-9]+$").Success)
            {
                enDomicilio.ErrorText = "Introduzca una calle valida";
                await DisplayAlert("Advertencia", "Introduzca una calle valida", "OK");

                a2 = false;

            }
            else
            {
                enDomicilio.ErrorText = "";
                a2 = true;
            }
            if (ValidarVacio(enNumero.Text) || !Regex.Match(enNumero.Text, "^[a-zA-Z0-9/-]*$").Success)
            {
                lblnum.TextColor = Xamarin.Forms.Color.Red;
                a3 = false;

            }
            else
            {
                lblnum.TextColor = Xamarin.Forms.Color.Black;
                a3 = true;
            }

            if (ValidarVacio(enCod.Text) || enCod.Text.Length < 5)
            {
                lblCod.TextColor = Xamarin.Forms.Color.Red;
                a4 = false;
            }
            else
            {
                lblCod.TextColor = Xamarin.Forms.Color.Black;
                a4 = true;
            }


            if (ValidarVacio(enTipoAsentamiento.Text) || !Regex.Match(enTipoAsentamiento.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚ\s0-9]+$").Success)
            {
                enTipoAsentamiento.ErrorText = "Introduzca un tipo valido";
                await DisplayAlert("Advertencia", "Introduzca un tipo valido", "OK");

                a5 = false;

            }
            else
            {
                enTipoAsentamiento.ErrorText = "";
                a5 = true;
            }

            if (ValidarVacio(enEstado.Text) || !Regex.Match(enEstado.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚÜü\s0-9]+$").Success)
            {
                
                enEstado.ErrorText = "Introduzca un estado valido";
                await DisplayAlert("Advertencia","Introduzca un estado valido", "OK");

                a6 = false;

            }
            else
            {
                enEstado.ErrorText = "";
                a6 = true;
            }

            if (ValidarVacio(enPais.Text))
            {
                enPais.ErrorText = "Introduzca su país";
                await DisplayAlert("Advertencia","Introduzca su país", "OK");

                a7 = false;

            }
            else
            {
                enPais.ErrorText = "";
                a7 = true;
            }
            if (ValidarVacio(enTelefono.Text) || enTelefono.Text.Length < 7 || enTelefono.Text.Length > 10)
            {
                await DisplayAlert("Sin número telefónico valido", "Deslice la pantalla para ver todas las opciones", "OK");
                enTelefono.ErrorText = "Introduzca un teléfono valido";
                a8 = false;

            }
            else
            {
                enTelefono.ErrorText = "";
                a8 = true;
            }

            if (ValidarVacio(enMunicipio.Text) || !Regex.Match(enMunicipio.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚ\s0-9]+$").Success)
            {
                await DisplayAlert("Advertencia", "Introduzca un municipio valido", "OK");
          
                enMunicipio.ErrorText = "Introduzca un municipio valido";
                a9 = false;

            }
            else
            {
                enMunicipio.ErrorText = "";
                a9 = true;
            }

            if (ValidarVacio(enLADA.Text) || enLADA.Text.Length < 2)
            {
                await DisplayAlert("Advertencia", "LADA incorrecta", "OK");
                enLADA.ErrorText = "LADA";
                a10 = false;

            }
            else
            {
                enLADA.ErrorText = "";
                a10 = true;
            }

            if(cargaCP){
            if (!(pkAsentamiento.SelectedIndex > -1))
            {
                await DisplayAlert("Campo vacio", "seleccione su asentamiento", "ok");
                a11 = false;

            }
            else
            {
                a11 = true;
            }
            }

            if (a1 && a2 && a3 && a4 && a5 && a6 && a7 && a8 && a9 && a10 && comodin && comodin2&&a11)
            {
                var asentamiento = 
                user.direccion = new Direccion
                {
                    calle = enDomicilio.Text,
                    numero = enNumero.Text,
                    numeroInterior = enNumInt.Text,
                    tipo = "DOMICILIO",

                    catalogoDir = new CatalogoDir
                    {
                        
                        asentamiento = (cargaCP)?pkAsentamiento.Items[pkAsentamiento.SelectedIndex]:enColonia.Text,
                        cp = enCod.Text,
                        ciudad = enCiudad.Text,
                        estado = enEstado.Text,
                        municipio = enMunicipio.Text,
                        tipoasentamiento = enTipoAsentamiento.Text,
                        pais = enPais.Text


                    }
                };



                user.telefono = new List<Telefono>();


                user.telefono.Add(new Telefono
                {
                    telefono = enTelefono.Text,
                    lada = enLADA.Text,
                    tipo = "FIJO"
                });

                if (com && com2)
                {
                    user.telefono.Add(new Telefono
                    {
                        telefono = enCelular.Text,
                        lada = enLADA2.Text,
                        tipo = "MOVIL"
                    });
                }




                //System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(user));

                ClienteRest cliente = new ClienteRest();

                cliente.POST(Constantes.URL_USUARIOS + "/registrar?movil=true&tramitta=false", user, 1);

             
                MessagingCenter.Subscribe<ClienteRest>(this, "OK", async (Sender) =>
                {
                    await DisplayAlert("Guardado", "Usuario registrado con exito", "Ok");
                    await DisplayAlert("Información", "Verifique su correo electrónico para iniciar sesión", "Ok");
                    MessagingCenter.Unsubscribe<ClienteRest>(this, "OK");
                    await Navigation.PopToRootAsync();

                });
                MessagingCenter.Subscribe<ClienteRest>(this, "error", async (Sender) =>
                {
                    MessagingCenter.Unsubscribe<ClienteRest>(this, "error");
                    await DisplayAlert("Error", "No fue posible dar de alta al usuario, posiblemente que este correo ya esta registrado o el servidor esta fuera de línea", "Ok");
                    Application.Current.Properties["calle"] = enDomicilio.Text;
                    Application.Current.Properties["numero"] = enNumero.Text;
                    Application.Current.Properties["numeroInterior"] = enNumInt.Text;
                    Application.Current.Properties["asentamiento"] = (cargaCP) ? pkAsentamiento.Items[pkAsentamiento.SelectedIndex] : enColonia.Text;
                    Application.Current.Properties["cp"] = enCod.Text;
                    Application.Current.Properties["ciudad"] = enCiudad.Text;
                    Application.Current.Properties["estado"] = enEstado.Text;
                    Application.Current.Properties["municipio"] = enMunicipio.Text;
                    Application.Current.Properties["tipoAsentamiento"] = enTipoAsentamiento.Text;
                    Application.Current.Properties["pais"] = enPais.Text;
                    Application.Current.Properties["telefono"] = enTelefono.Text;
                    Application.Current.Properties["lada"] = enLADA.Text;
                    Application.Current.Properties["celular"] = enCelular.Text;
                    Application.Current.Properties["lada2"] = enLADA2.Text;
                   
                     
                          

                    await Application.Current.SavePropertiesAsync();


                     
                
                });
                //await Navigation.PopToRootAsync();
            }
        }

        /// <summary>
        /// valida dinamicamente que solo se ingresen numeros en el código postal 
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Arguments.</param>
        public void OnTextChanged(object sender, TextChangedEventArgs args)
        {
            if (!Regex.IsMatch(args.NewTextValue, "^[0-9]+$", RegexOptions.CultureInvariant))
                (sender as Entry).Text = Regex.Replace(args.NewTextValue, "[^0-9]", string.Empty);
            Entry entry = sender as Entry;
            String val = entry.Text;

            if (val.Length > 9)
            {
                val = val.Remove(val.Length - 1);
                entry.Text = val;
            }
        }
        /// <summary>
        /// verifica que solo se ingresen cierto tipo de caracteres al ingresar un número de direccion y número interior
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Arguments.</param>
        public void validarNumero(object sender, TextChangedEventArgs args)
        {
            if (!Regex.IsMatch(args.NewTextValue, "^[0-9A-Fa-f/-]+$", RegexOptions.CultureInvariant))
                (sender as Entry).Text = Regex.Replace(args.NewTextValue, "[^0-9]", string.Empty);
            Entry entry = sender as Entry;
            String val = entry.Text;
            if (val.Length > 5)
            {
                val = val.Remove(val.Length - 1);
                entry.Text = val;
            }

        }

        /// <summary>
        /// valida que solo se puedan ingresar números en la lada del teléfono y hacer el salto automático 
        /// al la entrada teléfono
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Arguments.</param>
        public void OnLadaChanged(object sender, TextChangedEventArgs args)
        {
            if (!Regex.IsMatch(args.NewTextValue, "^[0-9]+$", RegexOptions.CultureInvariant))
                (sender as Xfx.XfxEntry).Text = Regex.Replace(args.NewTextValue, "[^0-9]", string.Empty);
            Xfx.XfxEntry entry = sender as Xfx.XfxEntry;
            entry.ErrorText = "";
            String val = entry.Text;

            if (val.Length > 3)
            {
                var temp = val.Substring(3);
                val = val.Remove(val.Length - 1);
                entry.Text = val;


                enTelefono.Focus();
                enTelefono.Text = temp;
            }
        }

        /// <summary>
        /// valida que solo se ingresen números en la entrada teléfono y evita que sean mas de 7 digitos
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Arguments.</param>
        public void OnTelefonoChanged(object sender, TextChangedEventArgs args)
        {
            if(!string.IsNullOrEmpty((sender as Entry).Text)){
            if (!Regex.IsMatch(args.NewTextValue, "^[0-9]+$", RegexOptions.CultureInvariant))
                (sender as Entry).Text = Regex.Replace(args.NewTextValue, "[^0-9]", string.Empty);
            Entry entry = sender as Entry;
            String val = entry.Text;

               if (val.Length > 7)
                {
                val = val.Remove(val.Length - 1);
                entry.Text = val;
                enLADA2.Focus();
                }
            }

        }

        /// <summary>
        /// borra en la pantalla el error detectado dinámicamente al ingresar texto nuevo
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Arguments.</param>
        public void borrarError(Object sender, TextChangedEventArgs args)
        {
            (sender as Xfx.XfxEntry).ErrorText = "";
        }

        /// <summary>
        /// verifica si la entrada de texto esta vacia.
        /// </summary>
        /// <returns><c>true</c>, si el campo esta vacio, <c>false</c> en caso contrario.</returns>
        /// <param name="x">texto ingresado en la entrada.</param>
        bool ValidarVacio(string x)
        {
            var auth = (String.IsNullOrEmpty(x)) ? true : false;
            return auth;
        }


        /// <summary>
        /// Cliente de para hacer uso de "Cognitive services de microsoft" para poder añadir la funcionalidad
        /// de OCR, es necesaria obtener una clave personal atra vez de su sitio web
        /// </summary>
        VisionServiceClient visionClient = new VisionServiceClient("2b2d900d589946d4aea338483921078b");

        /// <summary>
        /// Método asincrono envia una imagen para ser reconocida en la API de microsoft
        /// </summary>
        /// <returns>The text description.</returns>
        /// <param name="imageStream">imagen a enviar</param>
       private async Task<OcrResults> GetTextDescription(Stream imageStream)
        {




            return await visionClient.RecognizeTextAsync(imageStream, "es", true);
        }


        /// <summary>
        /// evento del boton capturar, hace una llamada a la pantalla camara (se renderiza nativamente según el dispositivo)
        /// cualquier cambio a la camara es necesaria hacerla en CamaraPageRenderer.cs contenido en el modulo de .droid y .iOS respectivamente
        /// </summary>
        /// <param name="sender">objeto que hace referencia al evento</param>
        /// <param name="e">argumentos que son posibles de obtener apartir del objeto que hace llamada al evento</param>
       private async void TakePicture(PhotoResultEventArgs result)
        {
            await Navigation.PopModalAsync();
            if (!result.Success)
                return;


            bool match = false;






            var cont = 0;
            indicador.IsRunning = true;
            string calle = "", domicilio = "", texto = "";
            //var hasTwoNames = false;
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {

                    var ocr = await GetTextDescription(new MemoryStream(result.Image));


                    foreach (var region in ocr.Regions)
                    {

                        foreach (var line in region.Lines)
                        {
                            var lineStack = new StackLayout
                            { Orientation = StackOrientation.Horizontal };


                            foreach (var words in line.Words)
                            {
                                if (words.Text.Equals("DOMICILIO"))
                                {
                                    match = true;
                                    break;
                                }

                                if (cont == 1)
                                {

                                    calle = calle + words.Text + "%";
                                    //match = false;
                                }
                                if (cont == 2)
                                {
                                    domicilio = domicilio + words.Text + "#";
                                }

                                texto = texto + words.Text;
                            }
                            if (match)
                            {
                                cont++;
                            }

                        }
                    }


                    try
                    {
                        String[] callex = calle.Split('%');

                        String[] domiciliox = domicilio.Split('#');

                        System.Diagnostics.Debug.WriteLine(callex[callex.Length - 2] + " " + callex[1] + " " + callex[2] + " #" + callex.Length);
                        System.Diagnostics.Debug.WriteLine(domicilio);
                        System.Diagnostics.Debug.WriteLine(texto);
                        enDomicilio.Text = callex[1];
                        enNumero.Text = callex[callex.Length - 2];
                        for (int i = 1; i < domiciliox.Length - 2; i++)
                        {
                            enColonia.Text = enColonia.Text + domiciliox[i] + "\t";
                        }


                        enCod.Text = domiciliox[domiciliox.Length - 2];

                    }
                    catch (IndexOutOfRangeException)
                    {
                        await DisplayAlert("Error", "No Fué posible capturar los campos", "OK");
                    }

                }
                catch (ClientException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
                //System.Diagnostics.Debug.WriteLine(texto);
                indicador.IsRunning = false;
            }
            else
            {
                await DisplayAlert("Error de conexión", "Es necesario estar conectado a internet para acceder este servicio", "Ok");
            }
        }

        async void OCR_Clicked(object sender, System.EventArgs e)
        {
            enColonia.Text = "";


            if (CrossConnectivity.Current.IsConnected)
            {
                var cameraPage = new CameraPage();
                cameraPage.OnPhotoResult += TakePicture;
                //await CameraPage_OnPhotoResult();
                await Navigation.PushModalAsync(cameraPage);
            }
            else
            {
                await DisplayAlert("Error de conexión", "Es necesario estar conectado a internet para acceder este servicio", "Ok");
            }
        }

        /**
        void llenarPicker()
        {
            Device.BeginInvokeOnMainThread(async () =>
           {
               if (CrossConnectivity.Current.IsConnected)
               {
                   ClienteRest client = new ClienteRest();
                   var httpclient = await client.GET<TipoAsentamiento>("http://192.168.0.18:8080/api/catalogo-dirs/tipo-asentamiento");
                   tipoas = new List<string>();
                   if (httpclient != null)
                   {

                       foreach (var dato in httpclient.respuesta)
                       {
                           //tipoas.Add(dato);
                           if (!String.IsNullOrEmpty(dato))
                           {
                               pkTipoAsentamiento.Items.Add(dato);
                           }

                       }



                       foreach (var nom in tipoas)
                       {




                       }
                   }


               }
               else { await DisplayAlert("Error de conexion", "No hay coneccion a internet", "ok"); }

           });
        }**/



        /// <summary>
        /// evento cuando se presiona la tecla de retorno en el teclado virtual de la entrada enCod "codigo postal"
        /// para consumir al servicio que muestra el catálodo de direcciones correspondiente al código postal ingresado
        /// y llenar automáticamente los campos correspondientes
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async void algo_Completed(object sender, System.EventArgs e)
        {
            
            System.Diagnostics.Debug.WriteLine("asdasd");
            var cp = ((Entry)sender).Text;

            if (CrossConnectivity.Current.IsConnected)
            {

                ClienteRest cliente = new ClienteRest();
                var resp = await cliente.GET<CodigoPostal>(Constantes.URL_USUARIOS + "/catalogo-dirs/mostrarCatalogo/" + cp);

                if(resp!=null){
                    pkAsentamiento.IsVisible = true;
                    enColonia.IsVisible = false;

                    List<string> asentmientos = new List<string>();
                    foreach(var item in resp.respuesta){
                        asentmientos.Add(item.asentamiento);

                    }
                    pkAsentamiento.ItemsSource = asentmientos;
                    cargaCP = true;
                enCiudad.Text = resp.respuesta[0].ciudad;
                enColonia.Text = resp.respuesta[0].asentamiento;
                enMunicipio.Text = resp.respuesta[0].municipio;
                enEstado.Text = resp.respuesta[0].estado;
                enTipoAsentamiento.Text = resp.respuesta[0].tipoasentamiento;
                    enPais.Text = resp.respuesta[0].pais;


                

                }
            }
            else { await DisplayAlert("Error de conexion", "No hay conexión a internet", "ok"); }
        }
    }
}
