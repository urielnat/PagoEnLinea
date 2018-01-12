using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FullCameraPage;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using PagoEnLinea.Modelos;
using PagoEnLinea.servicios;
using Plugin.Connectivity;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;

namespace PagoEnLinea
{

    /// <summary>
    /// esta clase corresponde a la primera de dos pantallas para el registro de un usuario
    /// añade la funcionalidad de OCR
    /// </summary>
    public partial class RegistroPage : ContentPage
    {
        public Usuarios users = new Usuarios();
        public Persona p = new Persona();
        public Direccion dire = new Direccion();
        public CatalogoDir catDir = new CatalogoDir();

        /// <summary>
        /// en el constructor se la asignan diferentes eventos a las entradas de texto:
        /// permite detectar cuando cuando el usuario teclea nuevamente encaso de que
        /// la interfaz le muestre un error para borrarlo dinámicamente
        /// asi mismo muestra un error en caso de detectarlo al presionar la tecla de retorno
        /// o cambiar el foco de la entrada
        /// muestra al iniciar una subpantalla con una leve explicacion de la funcionalidad de OCR
        /// </summary>
        public RegistroPage()
        {
            InitializeComponent();
            enNombre.TextChanged += borrarError;
            enPaterno.TextChanged += borrarError;
            enMaterno.TextChanged += borrarError;
            enCorreo.TextChanged += borrarError;
            enPassword.TextChanged += borrarError;
            enPassword2.TextChanged += borrarError;
            enCURP.TextChanged += borrarError;
            enCURP.TextChanged += MayusChanged;

            enCURP.Completed += VerificarError;
            enCURP.Unfocused += errorUnfocus;

            enNombre.Completed += VerificarError;
            enPaterno.Completed += VerificarError;
            enMaterno.Completed += VerificarError;
            enCorreo.Completed += VerificarError;
            enPassword.Completed += VerificarError;
            enPassword2.Completed += VerificarError;


            enNombre.Unfocused += errorUnfocus;
            enPaterno.Unfocused += errorUnfocus;
            enMaterno.Unfocused += errorUnfocus;
            enCorreo.Unfocused += errorUnfocus;
            enPassword.Unfocused += errorUnfocus;
            enPassword2.Unfocused += errorUnfocus;

            dtFecha.MaximumDate = DateTime.Now;
            dtFecha.Date = DateTime.Now;
            PopupNavigation.PushAsync(new TutorialPopUp(), false);
            enPassword.Focused += OnFocus;

        }


        /// <summary>
        /// sube el scroll automáticamente en caso de que el usuario este posisionado en el campo contraseña
        /// </summary>
        /// <param name="sender">objeto que hace referencia al evento</param>
        /// <param name="e">argumentos que son posibles de obtener apartir del objeto que hace llamada al evento</param>
        private async void OnFocus(object sender, FocusEventArgs e)
        {


            if (sender == enPassword)
            {

                await Task.Yield();
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var heightContentScroll = scroll2.ContentSize.Height;
                    await Task.Yield();
                    await scroll2.ScrollToAsync(enPassword2, ScrollToPosition.Start, true);
                });


            }




        }

        /// <summary>
        /// método que permite detectar dinámicamente si existe algun error en un campo al cambiar
        /// el foco de la aplicación
        /// </summary>
        /// <param name="sender">objeto que hace referencia al evento</param>
        /// <param name="e">argumentos que son posibles de obtener apartir del objeto que hace llamada al evento</param>
        private void errorUnfocus(object sender, FocusEventArgs e)
        {


            if (sender == enCURP)
            {
                if (string.IsNullOrEmpty(enCURP.Text) || !Regex.Match(enCURP.Text, "[A-Z][A,E,I,O,U,X][A-Z]{2}[0-9]{2}[0-1][0-9][0-3][0-9][M,H][A-Z]{2}[B,C,D,F,G,H,J,K,L,M,N,Ñ,P,Q,R,S,T,V,W,X,Y,Z]{3}[0-9,A-Z][0-9]").Success)
                    enCURP.ErrorText = "Introduzca un CURP valido";
                else enCURP.ErrorText = "";
            }
            else if (sender == enNombre)
            {
                if (string.IsNullOrEmpty(enNombre.Text) || !Regex.Match(enNombre.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚüÜ\s]+$").Success)
                {
                    enNombre.ErrorText = "Introduzca un nombre válido";
                }
                else
                {
                    enNombre.ErrorText = "";
                }
            }
            else if (sender == enPaterno)
            {
                if (string.IsNullOrEmpty(enPaterno.Text) || !Regex.Match(enPaterno.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚüÜ\s]+$").Success)
                {
                    enPaterno.ErrorText = "Introduzca un apellido válido";


                }
                else
                {
                    enPaterno.ErrorText = "";

                }
            }
            else if (sender == enMaterno)
            {
                if (!string.IsNullOrEmpty(enMaterno.Text))
                {
                    if (!Regex.Match(enMaterno.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚüÜ\s]+$").Success)
                    {
                        enMaterno.ErrorText = "Introduzca un apellido válido";


                    }
                    else
                    {
                        enMaterno.ErrorText = "";
                    }
                }
            }
            else if (sender == enPassword)
            {
               
                if (string.IsNullOrEmpty(enPassword.Text) || enPassword.Text.Length < 8 || enPassword.Text.Length > 16)
                {
                    enPassword.ErrorText = "contraseña inválida (mínimo 8 caracteres)";

                }
            }
            else if (sender == enPassword2)
            {
               
                if (!(string.IsNullOrEmpty(enPassword.Text)) && !(string.IsNullOrEmpty(enPassword2.Text)))
                {
                    if (enPassword.Text.Equals(enPassword2.Text))
                    {
                        enPassword2.ErrorText = "";

                    }
                    else
                    {
                        enPassword2.ErrorText = "Las contraseñas no concuerdan";
                    }

                }
            }
            else if (sender == enCorreo)
            {
                if (string.IsNullOrEmpty(enCorreo.Text) || !Regex.Match(enCorreo.Text, "^(?(\")(\".+?(?<!\\\\)\"@)|(([0-9A-Za-z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9A-Za-z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-z][-\\w]*[0-9a-z]*\\.)+[a-z0-9][\\-a-z0-9]{0,22}[a-z0-9]))$").Success)
                {
                    enCorreo.ErrorText = "Introduzca un correo electrónico válido";

                }
                else
                {
                    enCorreo.ErrorText = "";

                }
            }
        }



        /// <summary>
        /// método que permite detectar dinámicamente si existe algun error en un campo al oprimir
        /// la tecla de retorno en el teclado virtual
        /// </summary>
        /// <param name="sender">objeto que hace referencia al evento</param>
        /// <param name="e">argumentos que son posibles de obtener apartir del objeto que hace llamada al evento</param>
        private async void VerificarError(object sender, EventArgs e)
        {


            if (sender == enCURP)
            {
                if (string.IsNullOrEmpty(enCURP.Text) || !Regex.Match(enCURP.Text, "[A-Z][A,E,I,O,U,X][A-Z]{2}[0-9]{2}[0-1][0-9][0-3][0-9][M,H][A-Z]{2}[B,C,D,F,G,H,J,K,L,M,N,Ñ,P,Q,R,S,T,V,W,X,Y,Z]{3}[0-9,A-Z][0-9]").Success)
                    enCURP.ErrorText = "Introduzca un CURP valido";
                else enCURP.ErrorText = "";
            }
            else if (sender == enNombre)
            {
                if (string.IsNullOrEmpty(enNombre.Text) || !Regex.Match(enNombre.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚ\s]+$").Success)
                {
                    enNombre.ErrorText = "Introduzca un nombre válido";
                }
                else
                {
                    enNombre.ErrorText = "";
                }
            }
            else if (sender == enPaterno)
            {
                if (string.IsNullOrEmpty(enPaterno.Text) || !Regex.Match(enPaterno.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚüÜ\s]+$").Success)
                {
                    enPaterno.ErrorText = "Introduzca un apellido válido";


                }
                else
                {
                    enPaterno.ErrorText = "";

                }
            }
            else if (sender == enMaterno)
            {
                if (!string.IsNullOrEmpty(enMaterno.Text))
                {
                    if (!Regex.Match(enMaterno.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚüÜ\s]+$").Success)
                    {
                        enMaterno.ErrorText = "Introduzca un apellido válido";


                    }
                    else
                    {
                        enMaterno.ErrorText = "";
                    }
                }
            }
            else if (sender == enPassword)
            {
                await Task.Yield();
                await scroll2.ScrollToAsync(enPassword2, ScrollToPosition.End, true);
                if (string.IsNullOrEmpty(enPassword.Text) || enPassword.Text.Length < 8 || enPassword.Text.Length > 16)
                {
                    enPassword.ErrorText = "contraseña inválida (mínimo 8 caracteres)";

                }
            }
            else if (sender == enPassword2)
            {
                await Task.Yield();
                await scroll2.ScrollToAsync(enPassword2, ScrollToPosition.End, true);
                if (!(string.IsNullOrEmpty(enPassword.Text)) && !(string.IsNullOrEmpty(enPassword2.Text)))
                {
                    if (enPassword.Text.Equals(enPassword2.Text))
                    {
                        enPassword2.ErrorText = "";

                    }
                    else
                    {
                        enPassword2.ErrorText = "Las contraseñas no concuerdan";
                    }

                }
            }else if (sender== enCorreo){
                if (string.IsNullOrEmpty(enCorreo.Text) || !Regex.Match(enCorreo.Text, "^(?(\")(\".+?(?<!\\\\)\"@)|(([0-9A-Za-z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9A-Za-z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-z][-\\w]*[0-9a-z]*\\.)+[a-z0-9][\\-a-z0-9]{0,22}[a-z0-9]))$").Success)
                {
                    enCorreo.ErrorText = "Introduzca un correo electrónico válido";
                 
                }
                else
                {
                    enCorreo.ErrorText = "";

                }
            }
        }

   
        /// <summary>
        /// evento click del boton "siguiente" no permite acceder a la siguiente pantalla
        /// hasta tener validados todos los campos 
        /// </summary>
        /// <param name="sender">objeto que hace referencia al evento</param>
        /// <param name="e">argumentos que son posibles de obtener apartir del objeto que hace llamada al evento</param>
       
        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            

            Boolean a1 = false, a2 = false, a4 = false, a5 = false, a6 = false, a7 = false, a8 = true,a9=true;
            Boolean comodin = true;
            if (string.IsNullOrEmpty(enNombre.Text) || !Regex.Match(enNombre.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚüÜ\s]+$").Success)
            {
                enNombre.ErrorText = "Introduzca un nombre valido";
                await DisplayAlert("Advertencia","Introduzca un nombre valido","OK");
                a1 = false;
            }
            else
            {
                enNombre.ErrorText = "";
                a1 = true;
            }
            if (string.IsNullOrEmpty(enPaterno.Text) || !Regex.Match(enPaterno.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚüÜ\s]+$").Success)
            {
                enPaterno.ErrorText = "Introduzca un apellido valido";
                await DisplayAlert("Advertencia", "Introduzca un apellido valido", "OK");
                a2 = false;

            }
            else
            {
                enPaterno.ErrorText = "";
                a2 = true;
            }

            if(!string.IsNullOrEmpty(enMaterno.Text)){
                if (!Regex.Match(enMaterno.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚüÜ\s]+$").Success)
                {
                    enMaterno.ErrorText = "Introduzca un apellido valido";
                    await DisplayAlert("Advertencia", "Introduzca un nombre valido", "OK");
                    comodin = false;

                }else{
                    comodin = true;
                }
            }else
            {
                comodin = true;
            }

            if (string.IsNullOrEmpty(enCorreo.Text) || !Regex.Match(enCorreo.Text, "^(?(\")(\".+?(?<!\\\\)\"@)|(([0-9A-Za-z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9A-Za-z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-z][-\\w]*[0-9a-z]*\\.)+[a-z0-9][\\-a-z0-9]{0,22}[a-z0-9]))$").Success)
            {
                enCorreo.ErrorText = "Introduzca un correo electrónico valido";
                await DisplayAlert("Advertencia", "Introduzca un correo electrónico valido", "OK");
                a4 = false;
            }
            else
            {
                enCorreo.ErrorText = "";
                a4 = true;
            }
            if (string.IsNullOrEmpty(enCURP.Text)||!Regex.Match(enCURP.Text, "[A-Z][A,E,I,O,U,X][A-Z]{2}[0-9]{2}[0-1][0-9][0-3][0-9][M,H][A-Z]{2}[B,C,D,F,G,H,J,K,L,M,N,Ñ,P,Q,R,S,T,V,W,X,Y,Z]{3}[0-9,A-Z][0-9]").Success)
            {
                enCURP.ErrorText = "Introduzca un CURP valido";
                await DisplayAlert("Advertencia", "Introduzca un CURP valido", "OK");
                a5 = false;

            }
            else
            {
                enCURP.ErrorText = "";
                a5 = true;
            }
            if (!(pkEstCvl.SelectedIndex > -1))
            {
                await DisplayAlert("Campo vacio", "seleccion su estado civil", "ok");
                a6 = false;

            }
            else
            {
                a6 = true;
            }

            if (string.IsNullOrEmpty(enPassword.Text) || enPassword.Text.Length < 8 || enPassword.Text.Length > 16)
            {
                enPassword.ErrorText = "contraseña inválida (mínimo 8 caracteres)";
                await DisplayAlert("Advertencia", "contraseña inválida (mínimo 8 caracteres)", "OK");
              
                a8 = false;
            }
            else
            {
                enPassword.ErrorText = "";
                a8 = true;
            }
            if(!(string.IsNullOrEmpty(enPassword.Text))&&!(string.IsNullOrEmpty(enPassword2.Text))){
                if (enPassword.Text.Equals(enPassword2.Text))
                {   
                    enPassword2.ErrorText = "";
                    a9 = true;

                }
                else
                {

                    enPassword2.ErrorText = "Las contraseñas no concuerdan";
                    await DisplayAlert("Advertencia", "Las contraseñas no concuerdan", "OK");
                    a9 = false;


                }
            }else{ await DisplayAlert("Campo vacio", "Deslice su pantalla para ver todas las opciones", "Ok"); }

           
            if (!(pkSexo.SelectedIndex > -1))
            {
                await DisplayAlert("Campo vacio", "seleccione su sexo", "ok");
                a7 = false;

            }
            else
            {
                a7 = true;
            }
            if(a1&&a2&& a4&& a5&& a6&& a7&&a8&&a9&&comodin){


                users.tipousuario = "CONTRIBUYENTE";
                users.contrasena = enPassword.Text;
                users.persona = new Persona
                {
                    nombre = enNombre.Text,
                    apaterno = enPaterno.Text,
                    amaterno = enMaterno.Text,
                    sexo = pkSexo.Items[pkSexo.SelectedIndex],
                    curp = enCURP.Text,
                    edoCivil = pkEstCvl.Items[pkEstCvl.SelectedIndex],
                    fechanac = dtFecha.Date.ToString("yyyy-MM-dd")
                };
                users.email = new Email
                {
                    correoe = enCorreo.Text,
                    tipo = "PERSONAL"
                };

 
          

                await Navigation.PushAsync(new RegistroPage2(users,dire,catDir));
            }

        }
        public void borrarError(Object sender, TextChangedEventArgs args)
        {
            (sender as Xfx.XfxEntry).ErrorText = "";
        }



       

        /// <summary>
        /// Cliente de para hacer uso de "Cognitive services de microsoft" para poder añadir la funcionalidad
        /// de OCR, es necesaria obtener una clave personal a traves de su sitio web
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
       async void OCR_Clicked(object sender, System.EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var cameraPage = new CameraPage();
                cameraPage.OnPhotoResult += CameraPage_OnPhotoResult;
                //await CameraPage_OnPhotoResult();
                await Navigation.PushModalAsync(cameraPage);
            }else{
                await DisplayAlert("Error de conexión", "Es necesario estar conectado a internet para acceder este servicio", "Ok");
            }   
         
        }


        /// <summary>
        /// obtiene la imagen capturada apartir de la foto que el usuario tomó hace una instancia al metodo GetTextDescription
        /// mismo que obtiene un resultado de texto apartir de la imagen enviada (en este caso adaptado para IFE/INE)
        /// la variable OCR contiene una coleccion de texto dividio en regiones, lineas y palabras mismos que son recorridos
        /// para obtener texto especifico dentro del resultado y llenar los campos de entrada
        /// </summary>
        /// <param name="result">Imagen resultante al tomar la foto</param>
        async void CameraPage_OnPhotoResult(PhotoResultEventArgs result)
        {
            await Navigation.PopModalAsync();
            if (!result.Success)
                return;

            //Photo.Source = ImageSource.FromStream(() => new MemoryStream(result.Image));


           

            bool match = false, match2 = false, gender = false, respaldo = false;
            int cont = 0, cont2 = 0,respcont=0;
            var nombre = "";
            var genero = "";
            var respnom = new string[2];

           


                indicador.IsRunning = true;
                string texto = "", CURP = "";
                bool hasTwoNames = false, respTwoNames = false;
                var fecha = "";
                string calle = "", domicilio = "";
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
                            texto = texto + words.Text + "\n";
                            if (words.Text.Equals("NOMBRE"))
                                {
                                texto = words.Text;
                                    match = true;
                                    break;
                                }

                                if (match && cont < 4)
                                {
                                    if (line.Words.Length > 1 && cont == 3)
                                    {
                                        hasTwoNames = true;
                                        System.Diagnostics.Debug.WriteLine("tiene dos nombres");
                                    }
                                    else
                                    {
                                        System.Diagnostics.Debug.WriteLine("tiene un nombre");
                                    }
                                    nombre += words.Text + "#";

                                    cont++;

                                }
                                if (words.Text.Equals("SEXO"))
                                {
                                    gender = true;
                                }
                                if (gender && (words.Text.Equals("M") || words.Text.Equals("H")))
                                {
                                    genero = words.Text;
                                    gender = false;
                                    respaldo = true;
                                    break;
                                }

                            if(respaldo&&respcont<2){
                                if(line.Words.Length>1){
                                    respTwoNames = true;
                                    respnom[respcont] = words.Text;
                                    respcont++;
                                }else{
                                    respnom[0] = words.Text;
                                    respcont = 2;
                                }
                            }
                                if (words.Text.Length == 18)
                                {


                                     CURP = words.Text;

                                    System.Diagnostics.Debug.WriteLine(CURP.Substring(4, 6));
                                    fecha = CURP.Substring(8, 2) + "/" + CURP.Substring(6, 2) + "/" + "19" + CURP.Substring(4, 2);

                                }
                                // texto = texto + words.Text +"\n";

                                if (words.Text.Equals("DOMICILIO"))
                                {
                                    match2 = true;
                                    break;
                                }

                                if (cont2 == 1)
                                {

                                    calle = calle + words.Text + "%";
                                    //match = false;
                                }
                                if (cont2 == 2)
                                {
                                    domicilio = domicilio + words.Text + "#";
                                }

                               
                            }
                            if (match2)
                            {
                                cont2++;
                            }



                        }
                    }
                    String[] nombrex = nombre.Split('#');
                System.Diagnostics.Debug.WriteLine(texto);
                    try
                    {
                    enCURP.Text = CURP;

                        enPaterno.Text = nombrex[0];
                        enMaterno.Text = nombrex[1];
                        if (!nombre.Contains("INSTI"))
                        {
                            enNombre.Text = nombrex[2];
                            if (hasTwoNames)
                            {
                                enNombre.Text = nombrex[2] + "\t" + nombrex[3];
                            }
                        }
                        else
                        {
                          if(respTwoNames){
                            enNombre.Text = respnom[0] + "\t" + respnom[1];
                        }else{
                            enNombre.Text = respnom[0];
                        } 
                        }
                        
                        String[] callex = calle.Split('%');

                        String[] domiciliox = domicilio.Split('#');
                 
                      //quite la abreviatura del inicio
                    dire.calle = callex[1];
                        dire.numero = callex[callex.Length - 2];
                    //cambie el inicio del vector para que omita el tipo de asentamineto
                        for (int i = 1; i < domiciliox.Length - 2; i++)
                        {
                        catDir.asentamiento = catDir.asentamiento + domiciliox[i] + "\t";
                        }


                        catDir.cp = domiciliox[domiciliox.Length - 2];
                      

                    }
                    catch (IndexOutOfRangeException)
                    {
                        await DisplayAlert("Advertencia", "No fue posible capturar algunos campos", "OK");
                    System.Diagnostics.Debug.WriteLine(texto);
                    }
                    try
                    {
                        System.Diagnostics.Debug.WriteLine(fecha);
                        dtFecha.Date = DateTime.ParseExact(fecha, "dd/MM/yyyy", null);

                    }
                    catch (Exception e)
                    {
                        await DisplayAlert("Fallo de captura", "No fue posible capturar la fecha", "Ok");
                        System.Diagnostics.Debug.WriteLine(e.Message);
                    }
                    if (genero.Equals("M"))
                    {
                        pkSexo.SelectedIndex = 1;

                    }
                    else
                    {
                        pkSexo.SelectedIndex = 0;
                    }
                }
                catch (ClientException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
                //System.Diagnostics.Debug.WriteLine(texto);
                indicador.IsRunning = false;

             
            
        }

        /// <summary>
        /// convierte el texto ingresado en la entrada de manera dinámica CURP en mayusculas
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void MayusChanged(object sender, TextChangedEventArgs e) { 
            
            (sender as Entry).Text = e.NewTextValue.ToUpper();

        }
    }
}
