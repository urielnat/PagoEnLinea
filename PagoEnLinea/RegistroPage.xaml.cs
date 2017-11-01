using System;
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

namespace PagoEnLinea
{
    public partial class RegistroPage : ContentPage
    {
        public Usuario user = new Usuario();
        public RegistroPage()
        {
            InitializeComponent();
            enNombre.TextChanged += borrarError;
            enPaterno.TextChanged += borrarError;
            enMaterno.TextChanged += borrarError;
            enCorreo.TextChanged += borrarError;
            enPassword.TextChanged += borrarError;
            enPassword2.TextChanged += borrarError;
        }





        async void Handle_Clicked(object sender, System.EventArgs e)
        {

            Boolean a1 = false, a2 = false, a3 = false, a4 = false, a5 = false, a6 = false, a7 = false, a8 = true,a9=true;
            if (string.IsNullOrEmpty(enNombre.Text) || !Regex.Match(enNombre.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚ\s]+$").Success)
            {
                enNombre.ErrorText = "Introduzca un nombre valido";
                a1 = false;
            }
            else
            {
                enNombre.ErrorText = "";
                a1 = true;
            }
            if (string.IsNullOrEmpty(enPaterno.Text) || !Regex.Match(enPaterno.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚ\s]+$").Success)
            {
                enPaterno.ErrorText = "Introduzca un apellido valido";
                a2 = false;

            }
            else
            {
                enPaterno.ErrorText = "";
                a2 = true;
            }
          

            if (string.IsNullOrEmpty(enCorreo.Text) || !Regex.Match(enCorreo.Text, "^(?(\")(\".+?(?<!\\\\)\"@)|(([0-9A-Za-z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9A-Za-z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-z][-\\w]*[0-9a-z]*\\.)+[a-z0-9][\\-a-z0-9]{0,22}[a-z0-9]))$").Success)
            {
                enCorreo.ErrorText = "Introduzca un correo electrónico valido";
                a4 = false;
            }
            else
            {
                enCorreo.ErrorText = "";
                a4 = true;
            }
            if (string.IsNullOrEmpty(enCURP.Text))
            {
                enCURP.ErrorText = "Introduzca su CURP";
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

            if (string.IsNullOrEmpty(enPassword.Text) || enPassword.Text.Length < 3 || enPassword.Text.Length > 16)
            {
                enPassword.ErrorText = "contraseña invalida ";
                a8 = false;
            }
            else
            {
                enPassword.ErrorText = "";
                a8 = true;
            }
            if(!(string.IsNullOrEmpty(enPassword.Text)&&string.IsNullOrEmpty(enPassword2.Text))){
                if (enPassword.Text.Equals(enPassword2.Text))
                {   
                    enPassword2.ErrorText = "";
                    a9 = true;

                }
                else
                {

                    enPassword2.ErrorText = "Las contraseñas no concuerdan";
                    a9 = false;


                }
            }else{ await DisplayAlert("Campo vacio", "Deslice su pantalla para ver todas las opciones", "Ok"); }

           
            if (!(pkSexo.SelectedIndex > -1))
            {
                await DisplayAlert("Campo vacio", "seleccion su sexo", "ok");
                a7 = false;

            }
            else
            {
                a7 = true;
            }
            if(a1&&a2&& a4&& a5&& a6&& a7&&a8&&a9){


                user.nombre = enNombre.Text;
                user.apellidoPaterno = enPaterno.Text;
                user.apellidoMaterno = enMaterno.Text;
                user.sexo = pkSexo.Items[pkSexo.SelectedIndex];
                user.correo = enCorreo.Text;
                 user.contraseña = enPassword.Text;
                user.confirmarContraseña = enPassword2.Text;


                await Navigation.PushAsync(new RegistroPage2(user));
            }

        }
        public void borrarError(Object sender, TextChangedEventArgs args)
        {
            (sender as Xfx.XfxEntry).ErrorText = "";
        }
        VisionServiceClient visionClient = new VisionServiceClient("7fa718b2312047ec92cf07211dd72b50");

        private async Task<OcrResults> GetTextDescription(Stream imageStream)
        {
            
            return await visionClient.RecognizeTextAsync(imageStream, "es", true);
        }

       
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



                        user.domicilio = callex[0] + "\t" + callex[1];
                        user.numero = callex[callex.Length - 2];
                        for (int i = 0; i < domiciliox.Length - 2; i++)
                        {
                            user.colonia = user.colonia + domiciliox[i] + "\t";
                        }


                        user.codigoPostal = domiciliox[domiciliox.Length - 2];
                      

                    }
                    catch (IndexOutOfRangeException)
                    {
                        await DisplayAlert("Advertencia", "No fue posible capturar algunos campos", "OK");
                    System.Diagnostics.Debug.WriteLine(texto + "$");
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
    }
}
