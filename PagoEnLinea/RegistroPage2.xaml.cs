using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using PagoEnLinea.Modelos;
using PagoEnLinea.servicios;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;


namespace PagoEnLinea
{
    public partial class RegistroPage2 : ContentPage
    {
        public Usuario user { set; get; }
        public RegistroPage2(Usuario u)
        {
            user = u;
            InitializeComponent();
            enCod.TextChanged += OnTextChanged;
            enNumero.TextChanged += validarNumero;

            enColonia.TextChanged += borrarError;
            enDomicilio.TextChanged += borrarError;
            enCiudad.TextChanged += borrarError;
            enTelefono.TextChanged += borrarError;

        }

       

        async void registrar_Clicked(object sender, System.EventArgs e)
        {
            bool a2 = false, a3 = false, a4 = false, a5 = false, a6 = false, a7 = false, a8 = false, a9 = false, a10 = false, a11 = false;
         
            if (ValidarVacio(enDomicilio.Text))
            {
                enDomicilio.ErrorText = "Introduzca un domicilio";
                a2 = false;

            }
            else
            {
                enDomicilio.ErrorText = "";
                a2 = true;
            }
            if (ValidarVacio(enNumero.Text))
            {
                lblnum.TextColor = Xamarin.Forms.Color.Red;
                a3 = false;

            }
            else
            {
                lblnum.TextColor = Xamarin.Forms.Color.Black;
                a3 = true;
            }

            if (ValidarVacio(enCod.Text)||enCod.Text.Length<5)
            {
                lblCod.TextColor = Xamarin.Forms.Color.Red;
                a4 = false;
            }
            else
            {
                lblCod.TextColor = Xamarin.Forms.Color.Black;
                a4 = true;
            }

            if (ValidarVacio(enColonia.Text))
            {
                enColonia.ErrorText = "Introduzca una colonia";
                a5 = false;

            }
            else
            {
                enColonia.ErrorText = "";
                a5 = true;
            }


            if (!(pkEstado.SelectedIndex > -1))
            {
                await DisplayAlert("Campo vacio", "selecciona su estado", "ok");
                a6 = false;

            }
            else
            {
                a6 = true;
            }

            if (ValidarVacio(enCiudad.Text))
            {
                enCiudad.ErrorText = "Introduzca su ciudad";
                a7 = false;

            }
            else
            {
                enCiudad.ErrorText = "";
                a7 = true;
            }
            if (ValidarVacio(enTelefono.Text))
            {  await DisplayAlert("Sin número telefónico","Deslice la pantalla para ver todas las opciones","ok");
                enTelefono.ErrorText = "Introduzca su teléfono";
                a8 = false;

            }
            else
            {
                enTelefono.ErrorText = "";
                a8 = true;
            }
            if (ValidarVacio(enMunicipio.Text))
            {
                enMunicipio.ErrorText = "Introduzca su municipio";
                a9 = false;

            }
            else
            {
                enMunicipio.ErrorText = "";
                a9 = true;
            }

            if (ValidarVacio(enLADA.Text))
            {
                enLADA.ErrorText = "LADA";
                a10 = false;

            }
            else
            {
                enLADA.ErrorText = "";
                a10 = true;
            }
            if (ValidarVacio(enCelular.Text))
            {
                enCelular.ErrorText = "Ingrese su Número de celular";
                a11 = false;

            }
            else
            {
                enCelular.ErrorText = "";
                a11 = true;
            }
            if ( a2 && a3 && a4 && a5 && a6 && a7 &&a8&&a9&&a10&&a11)
            { 
                user.razonSocial = enMunicipio.Text;
                user.domicilio = enDomicilio.Text;
                user.numero = enNumero.Text;
                user.codigoPostal = enCod.Text;
                user.colonia = enColonia.Text;
                user.estado = pkEstado.Items[pkEstado.SelectedIndex];
                user.ciudad = enCiudad.Text;
                user.telefono = enTelefono.Text;

                //ClienteRest cliente = new ClienteRest();
                //cliente.PUT("", user);

                await Navigation.PopToRootAsync();
            }  
        }
        public void OnTextChanged(object sender, TextChangedEventArgs args)
        {
            if (!Regex.IsMatch(args.NewTextValue, "^[0-9]+$", RegexOptions.CultureInvariant))
                (sender as Entry).Text = Regex.Replace(args.NewTextValue, "[^0-9]", string.Empty);
            Entry entry = sender as Entry;
            String val = entry.Text;

            if (val.Length > 5)
            {
                val = val.Remove(val.Length - 1);
                entry.Text = val;
            }
        }

        public void validarNumero(object sender, TextChangedEventArgs args)
        {
            if (!Regex.IsMatch(args.NewTextValue, "^[0-9A-Fa-f/-]+$", RegexOptions.CultureInvariant))
                (sender as Entry).Text = Regex.Replace(args.NewTextValue, "[^0-9]", string.Empty);
            Entry entry = sender as Entry;
            String val = entry.Text;


        }

        public void borrarError(Object sender, TextChangedEventArgs args){
            (sender as Xfx.XfxEntry).ErrorText = "";
        }

        bool ValidarVacio(string x){
            var auth = (String.IsNullOrEmpty(x)) ? true : false;
            return auth;
        }

        VisionServiceClient visionClient = new VisionServiceClient("7fa718b2312047ec92cf07211dd72b50");

        private async Task<OcrResults> GetTextDescription(Stream imageStream)
        {




            return await visionClient.RecognizeTextAsync(imageStream, "es", true);
        }

        private async Task TakePicture()
        {    

            await CrossMedia.Current.Initialize();
            MediaFile photo;
            bool match = false;
           
          

            if (CrossMedia.Current.IsCameraAvailable)
            {
                photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Directory = "Invoices",
                    Name = "Invoice.jpg"
                });


                var cont = 0;
               indicador.IsRunning = true;
                string calle = "", domicilio="",texto="";
                //var hasTwoNames = false;
                try
                {

                    var ocr = await GetTextDescription(photo.GetStream());


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

                                if (cont==1)
                                {
                                   
                                    calle = calle + words.Text+"%";
                                    //match = false;
                                }
                                if(cont==2){
                                    domicilio = domicilio + words.Text+"#";
                                }

                                texto = texto + words.Text;
                            }
                            if(match){
                                cont++;
                            }

                        }
                    }


                    try
                    {  
                        String[] callex = calle.Split('%');

                        String[] domiciliox = domicilio.Split('#');
                           
                        System.Diagnostics.Debug.WriteLine(callex[callex.Length-2]+" "+callex[1]+" "+callex[2]+" #" +callex.Length);
                        System.Diagnostics.Debug.WriteLine(domicilio);
                        System.Diagnostics.Debug.WriteLine(texto);
                        enDomicilio.Text = callex[0] +"\t"+ callex[1];
                        enNumero.Text = callex[callex.Length - 2];
                        for (int i = 0; i < domiciliox.Length-2;i++){
                            enColonia.Text = enColonia.Text + domiciliox[i]+"\t";  
                        }


                        enCod.Text = domiciliox[domiciliox.Length - 2];

                    }
                    catch (IndexOutOfRangeException)
                    {
                        await DisplayAlert("Error", "No Fue posible capturar los campos", "OK");
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
                photo = await CrossMedia.Current.PickPhotoAsync();
            }
        }

        async void OCR_Clicked(object sender, System.EventArgs e)
        {
            enColonia.Text = ""; 
            await TakePicture();
        }
    }
}
