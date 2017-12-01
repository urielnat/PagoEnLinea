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
  
    public partial class RegistroPage2 : ContentPage
    {
        public static List<string> tipoas;
        public Usuarios user { set; get; }
        public Direccion dire { set; get; }
        public CatalogoDir catdire { set; get; }
        public Direccion direc = new Direccion();
        public RegistroPage2(Usuarios u,Direccion d, CatalogoDir cd)
        {
            user = u;
            catdire = cd;

            InitializeComponent();
            enCod.TextChanged += OnTextChanged;
            enNumero.TextChanged += validarNumero;

            enColonia.TextChanged += borrarError;
            enDomicilio.TextChanged += borrarError;
            enCiudad.TextChanged += borrarError;
            enTelefono.TextChanged += borrarError;
            enDomicilio.Text = d.calle;
            enLADA.TextChanged += OnLadaChanged;
            enLADA2.TextChanged += OnLadaChanged;
            var jsonstring = JsonConvert.SerializeObject(u);
            //System.Diagnostics.Debug.WriteLine(jsonstring);
            jsonstring = jsonstring.Substring(1, jsonstring.Length - 2);

            pkpais.SelectedIndex = 0; 
            enTelefono.TextChanged += OnTelefonoChanged;
            enCelular.TextChanged += OnTelefonoChanged;
            llenarPicker();
            enColonia.Text = cd.asentamiento;
            enCod.Text = cd.cp;
            enNumero.Text = d.numero;


        }

       

        async void registrar_Clicked(object sender, System.EventArgs e)
        {
            bool a1 = false, a2 = false, a3 = false, a4 = false, a5 = false, a6 = false, a7 = false, a8 = false, a9 = false, a10 = false, a11 = false, a12 = false;
            bool comodin = true, comodin2=true,com=true,com2=true;

            if(!string.IsNullOrEmpty(enCiudad.Text)){
                if(!Regex.Match(enCiudad.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚ\s0-9]+$").Success){
                    comodin = false;
                }else
                {
                    comodin = true;
                }
            }else{
                comodin=true;
            }

            if (!string.IsNullOrEmpty(enNumInt.Text))
            {
                if (!Regex.Match(enNumInt.Text, "^[a-zA-Z0-9]*$").Success)
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
                if (enLADA2.Text.Length!=3)
                {
                    com = false;
                    enLADA2.ErrorText = "LADA incorrecta";
                }
                else
                {
                    com = true;
                    enLADA2.ErrorText = "";
                }
            }
            else
            {
                com = true;
                enLADA2.ErrorText = "";
            }


            if (!string.IsNullOrEmpty(enCelular.Text))
            {
                if (enCelular.Text.Length < 7 || enCelular.Text.Length>10)
                {
                    com2 = false;
                    enCelular.ErrorText= "Celular incorrecto";

                }
                else
                {
                    com2 = true;
                    enCelular.ErrorText = "";
                }
            }
            else
            {
                com2 = true;
                enCelular.ErrorText = "";
            }


            if (string.IsNullOrEmpty(enColonia.Text)|| !Regex.Match(enColonia.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚ\s0-9]+$").Success)
            {
                enColonia.ErrorText = "Introduzca un asentamiento válido";
                a1 = false;

            }
            else
            {
                enDomicilio.ErrorText = "";
                a1 = true;
            }

            if (ValidarVacio(enDomicilio.Text)||!Regex.Match(enDomicilio.Text,@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ\s0-9]+$").Success)
            {
                enDomicilio.ErrorText = "Introduzca una calle válida";
                a2 = false;

            }
            else
            {
                enDomicilio.ErrorText = "";
                a2 = true;
            }
            if (ValidarVacio(enNumero.Text)||!Regex.Match(enNumero.Text,"^[a-zA-Z0-9]*$").Success)
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


            if (!(pkTipoAsentamiento.SelectedIndex > -1))
            {
                await DisplayAlert("Campo vacio", "selecciona un tipo de asentamiento", "ok");
                a5 = false;

            }
            else
            {
                a5 = true;
            }

            if (!(pkEstado.SelectedIndex > -1))
            {
                await DisplayAlert("Campo vacio", "selecciona tu estado", "ok");
                a6 = false;

            }
            else
            {
                a6 = true;
            }
            /*
            if (ValidarVacio(enCiudad.Text))
            {
                enCiudad.ErrorText = "Introduzca su ciudad";
                a7 = false;

            }
            else
            {
                enCiudad.ErrorText = "";
                a7 = true;
            }*/
            if (ValidarVacio(enTelefono.Text)|| enTelefono.Text.Length <7|| enTelefono.Text.Length > 10)
            {  await DisplayAlert("Sin número telefónico valido","Deslice la pantalla para ver todas las opciones","ok");
                enTelefono.ErrorText = "Introduzca un teléfono valido";
                a8 = false;

            }
            else
            {
                enTelefono.ErrorText = "";
                a8 = true;
            }

            if (ValidarVacio(enMunicipio.Text)|| !Regex.Match(enMunicipio.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚ\s0-9]+$").Success)
            {
                enMunicipio.ErrorText = "Introduzca un municipio válido";
                a9 = false;

            }
            else
            {
                enMunicipio.ErrorText = "";
                a9 = true;
            }

            if (ValidarVacio(enLADA.Text)||enLADA.Text.Length<3)
            {
                enLADA.ErrorText = "LADA";
                a10 = false;

            }
            else
            {
                enLADA.ErrorText = "";
                a10 = true;
            }

          
            if ( a1 && a2 && a3 && a4 && a5 && a6 && a8 && a9 && a10 &&comodin&&comodin2&&com&&com2)
            {
                user.direccion = new Direccion { calle = enDomicilio.Text,
                        numero = enNumero.Text,
                    numeroInterior = enNumInt.Text,
                        tipo = "DOMICILIO",
                        catalogoDir = new CatalogoDir{
                            asentamiento = enColonia.Text,
                      cp = enCod.Text,
                    ciudad = enCiudad.Text,
                    estado = pkEstado.Items[pkEstado.SelectedIndex],
                    municipio = enMunicipio.Text,
                        tipoasentamiento = pkTipoAsentamiento.Items[pkTipoAsentamiento.SelectedIndex],
                        pais = pkpais.Items[pkpais.SelectedIndex]


                        }};



                user.telefono = new List<Telefono>();


                user.telefono.Add(new Telefono{telefono = enTelefono.Text,
                    lada = enLADA.Text,
                    tipo = "FIJO"});

                if(com&&com2){
                    user.telefono.Add(new Telefono
                    {
                        telefono = enCelular.Text,
                        lada = enLADA2.Text,
                        tipo = "MOVIL"
                    });
                }




                //System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(user));

                ClienteRest cliente = new ClienteRest();

                cliente.POST(Constantes.URL+"/registrar?movil=true", user,1);

                //FALTAN TELÉFONOS
                /*

                 if(!(pkEstado.SelectedIndex > -1)){
                     user.estado = null;    
                 }else{
                     user.estado = pkEstado.Items[pkEstado.SelectedIndex]; 
                 }

                 user.ciudad = enCiudad.Text;
                 user.telefono = enTelefono.Text;

                 //ClienteRest cliente = new ClienteRest();
                 //cliente.PUT("", user);
                 **/
                MessagingCenter.Subscribe<ClienteRest>(this, "OK", async (Sender) => {
                    await DisplayAlert("Guardado", "Usuario registrado con exito", "Ok");
                    await DisplayAlert("Información", "Verifique su correo electrónico para iniciar sesión", "Ok");
                    await Navigation.PopToRootAsync(); });
                MessagingCenter.Subscribe<ClienteRest>(this, "error", async (Sender) => {
                    MessagingCenter.Unsubscribe<ClienteRest>(this,"error");
                    await DisplayAlert("Error", "No fue posible dar de alta al usuario, posiblemente este correo o CURP ya esta registrado", "Ok");});
                //await Navigation.PopToRootAsync();
            }  
        }
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


        public void OnLadaChanged(object sender, TextChangedEventArgs args)
        {
            if (!Regex.IsMatch(args.NewTextValue, "^[0-9]+$", RegexOptions.CultureInvariant))
                (sender as Xfx.XfxEntry).Text = Regex.Replace(args.NewTextValue, "[^0-9]", string.Empty);
            Xfx.XfxEntry entry = sender as Xfx.XfxEntry;
            entry.ErrorText = "";
            String val = entry.Text;

            if (val.Length > 3)
            {
                val = val.Remove(val.Length - 1);
                entry.Text = val;
            }
        }

        public void OnTelefonoChanged(object sender, TextChangedEventArgs args)
        {
            if (!Regex.IsMatch(args.NewTextValue, "^[0-9]+$", RegexOptions.CultureInvariant))
                (sender as Entry).Text = Regex.Replace(args.NewTextValue, "[^0-9]", string.Empty);
            Entry entry = sender as Entry;
            String val = entry.Text;

            if (val.Length > 7)
            {
                val = val.Remove(val.Length - 1);
                entry.Text = val;
            }
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
            if (CrossConnectivity.Current.IsConnected){ 
               try{

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
                    enDomicilio.Text = callex[0] + "\t" + callex[1];
                    enNumero.Text = callex[callex.Length - 2];
                    for (int i = 0; i < domiciliox.Length - 2; i++)
                    {
                        enColonia.Text = enColonia.Text + domiciliox[i] + "\t";
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
            }else{
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
            }else{
                await DisplayAlert("Error de conexión", "Es necesario estar conectado a internet para acceder este servicio", "Ok");
            }  
        }

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
                            if(!String.IsNullOrEmpty(dato)){
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
        }
    }
}
