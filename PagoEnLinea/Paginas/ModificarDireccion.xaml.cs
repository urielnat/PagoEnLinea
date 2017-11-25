using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PagoEnLinea.Modelos;
using PagoEnLinea.servicios;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace PagoEnLinea.Paginas
{
    public partial class ModificarDireccion : ContentPage
    {
        
        string ID,catID;
        public static int tipo;
        public static List<string> tipoas;
        public ModificarDireccion(string id, string idcat, int tipo)
        {
            ID = id;
            catID = idcat;
            InitializeComponent();

            enCP.TextChanged += OnTextChanged;
            pkpais.SelectedIndex = 0;
            llenarPicker();

            if(tipo==0){
                btnModificar.IsVisible = true;
                btnAgregar.IsVisible = false;
            }
            if(tipo==1){
                btnAgregar.IsVisible = true;
                btnModificar.IsVisible = false;
            }
        }

        async void Añadir()
        {

            bool a1 = false, a2 = false, a3 = false, a4 = false, a5 = false, a6 = false;


            if (ValidarVacio(enCalle.Text))
            {
                await DisplayAlert("Campo no valida","Introduza una calle válida","Ok");
                a1 = false;

            }
            else
            {
               
                a1 = true;
            }

            if (ValidarVacio(enNumero.Text))
            { 
                await DisplayAlert("Campo no valido", "Introduza una número válido", "Ok");  
                a2 = false;

            }
            else
            {
                
                a2 = true;
            }
           

            if (ValidarVacio(enCP.Text) || enCP.Text.Length < 5)
            {
                await  DisplayAlert("Campo no valido", "Introduza una código postal válido", "Ok"); 
                a3 = false;
            }
            else
            {
                
                a3 = true;
            }


            if (!(pkTipoAsentamiento.SelectedIndex > -1))
            {
                await DisplayAlert("Campo vacio", "selecciona un tipo de asentamiento", "ok");
                a4 = false;

            }
            else
            {
                a4 = true;
            }

            if (ValidarVacio(enMunicipio.Text))
            {
                await  DisplayAlert("Campo vacio", "Introduzca un municipio válido", "ok");
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


            if(a1&&a2&&a3&&a4&&a5&&a6){

                ClienteRest cliente = new ClienteRest();
                Modelos.AgregarDireccion agredire = new Modelos.AgregarDireccion();
                var inf = await cliente.InfoUsuario<InfoUsuario>(Application.Current.Properties["token"] as string);
                if (inf != null)
                {

                    agredire.persona = new Persona
                    {
                        id= inf.persona.id,
                        nombre = inf.persona.nombre,
                        apaterno = inf.persona.apaterno,
                        amaterno = inf.persona.amaterno,
                        curp = inf.persona.curp,
                        edoCivil = inf.persona.edoCivil,
                        sexo = inf.persona.sexo,
                        fechanac = inf.persona.fechanac

                    };

                           
                    agredire.direccion = new Modelos.Direccion()
                    {
                        calle = enCalle.Text,
                        numero = enNumero.Text,
                        numeroInterior = enNumeroInterior.Text,
                        catalogoDir = new CatalogoDir()
                        {

                            asentamiento = enAsentamiento.Text,
                            tipoasentamiento = pkTipoAsentamiento.Items[pkTipoAsentamiento.SelectedIndex],
                            cp = enCP.Text,
                            ciudad = enCiudad.Text,
                            municipio = enMunicipio.Text,
                            estado = pkEstado.Items[pkEstado.SelectedIndex],
                            pais = pkpais.Items[pkpais.SelectedIndex]

                        }

                    };
                }





               
           
               


                cliente.POST(Constantes.URL+"/direccion/agregar",agredire,1);

                MessagingCenter.Subscribe<ClienteRest>(this, "OK", (Sender) =>
                {
                    DisplayAlert("Guardado", "¡Direccion Añadida con Exito!", "OK");
                    Navigation.PopAsync();

                });
                MessagingCenter.Subscribe<ClienteRest>(this, "error", (Sender) => {
                    DisplayAlert("Advertencia", "¡No fué posible añadir la dirección actual!", "error");
                    Navigation.PopAsync();

                });


            }
        }
        protected override void OnAppearing()
        {
            
        }
        bool ValidarVacio(string x)
        {
            var auth = (String.IsNullOrEmpty(x)) ? true : false;
            return auth;
        }

       
        void Handle_Clicked(object sender, System.EventArgs e)
        {
            Modificar(); 
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


        void Modificar(){
            bool a1 = false, a2 = false, a3 = false, a4 = false, a5 = false, a6 = false;


            if (ValidarVacio(enCalle.Text))
            {
                DisplayAlert("Campo no valida", "Introduza una calle válida", "Ok");
                a1 = false;

            }
            else
            {

                a1 = true;
            }

            if (ValidarVacio(enNumero.Text))
            {
                DisplayAlert("Campo no valido", "Introduza una número válido", "Ok");
                a2 = false;

            }
            else
            {

                a2 = true;
            }


            if (ValidarVacio(enCP.Text) || enCP.Text.Length < 5)
            {
                DisplayAlert("Campo no valido", "Introduza una código postal válido", "Ok");
                a3 = false;
            }
            else
            {

                a3 = true;
            }


            if (!(pkTipoAsentamiento.SelectedIndex > -1))
            {
                 DisplayAlert("Campo vacio", "selecciona un tipo de asentamiento", "ok");

                a4 = false;

            }
            else
            {
                a4 = true;
            }

            if (ValidarVacio(enMunicipio.Text))
            {
                DisplayAlert("Campo vacio", "Introduzca un municipio válido", "ok");
                a5 = false;

            }
            else
            {

                a5 = true;
            }

            if (!(pkEstado.SelectedIndex > -1))
            {
                 DisplayAlert("Campo vacio", "selecciona tu estado", "ok");
                a6 = false;

            }
            else
            {

                a6 = true;
            }


            if (a1 && a2 && a3 && a4 && a5 && a6)
            {
                ClienteRest client = new ClienteRest();

                Modelos.Direccion dir = new Modelos.Direccion();

                dir.id = ID;
                dir.calle = enCalle.Text;
                dir.numero = enNumero.Text;
                dir.numeroInterior = enNumeroInterior.Text;
                dir.catalogoDir = new CatalogoDir()
                {
                    id = catID,
                    asentamiento = enAsentamiento.Text,
                    tipoasentamiento = pkTipoAsentamiento.Items[pkTipoAsentamiento.SelectedIndex],
                    cp = enCP.Text,
                    ciudad = enCiudad.Text,
                    municipio = enMunicipio.Text,
                    estado = pkEstado.Items[pkEstado.SelectedIndex],
                    pais = pkpais.Items[pkpais.SelectedIndex]

                };


                client.PUT(Constantes.URL+"/direccion/actualizar", dir);

                MessagingCenter.Subscribe<ClienteRest>(this, "putDireccion", (Sender) => {
                    MessagingCenter.Unsubscribe<ClienteRest>(this, "putDireccion");
                    DisplayAlert("Guardado", "¡Direccion Modificada con Exito!", "OK");
                    Navigation.PopAsync();

                });


                MessagingCenter.Subscribe<ClienteRest>(this, "errorDireccion", (Sender) => {
                    MessagingCenter.Unsubscribe<ClienteRest>(this, "errorDireccion");
                    DisplayAlert("Advertencia", "¡No fue posible modificar la dirección actual!", "OK");
                    Navigation.PopAsync();

                });
            }
        }
       

        void Guardar_Clicked(object sender, System.EventArgs e)
        {
            Añadir();
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
        }
    }
}
