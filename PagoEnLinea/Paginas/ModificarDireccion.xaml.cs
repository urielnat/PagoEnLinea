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
        
        string ID,catID,tipoAsenta;
        public static int tipos;
        public static List<string> tipoas;
        public static List<CargaCP> asentmientos;
        public static bool cargaCP;
        public static int cargaID;
        public ModificarDireccion(string id, string idcat, int tipo,string estado, string tipoAsentamiento)
        {
            ID = id;
            catID = idcat;
            tipos = tipo;
            tipoAsenta = tipoAsentamiento;
            InitializeComponent();
            cargaCP = false;

            enCP.TextChanged += OnTextChanged;
            pkpais.SelectedIndex = 0;
            llenarPicker();

            if(tipo==1){
                btnAgregar.IsVisible = true;
                btnModificar.IsVisible = false;


            }

            if (tipo == 0)
            {
                llenarPicker();
                btnAgregar.IsVisible = false;
                btnModificar.IsVisible = true;
               
               
            }
            enNumero.TextChanged += validarNumero;
            enCP.Completed += algo_Completed;
            pkAsentamiento.SelectedIndexChanged += onIndexChange;
        }

        private void onIndexChange(object sender, EventArgs e)
        {
            int position = pkAsentamiento.SelectedIndex;
            if(pkAsentamiento.SelectedIndex>-1){
                for (var i = 0; i < pkTipoAsentamiento.Items.Count; i++)
                {
                  
                    if (pkTipoAsentamiento.Items[i].Equals(asentmientos[position].tipo))
                    {
                        pkTipoAsentamiento.SelectedIndex = i;
                        break;
                    }
                }
                cargaID = asentmientos[position].catID;
            }

        }

        async void Añadir()
        {

            bool a1 = false, a2 = false, a3 = false, a4 = false, a5 = false, a6 = false,a7=false;


            if (ValidarVacio(enCalle.Text)|| !Regex.Match(enCalle.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚüÜ\s0-9]+$").Success)
            {
                await DisplayAlert("Campo no valida","Introduza una calle válida","Ok");
                a1 = false;

            }
            else
            {
               
                a1 = true;
            }

            if (ValidarVacio(enNumero.Text) || !Regex.Match(enNumero.Text, "^[a-zA-Z0-9/-]*$").Success)
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

            if (ValidarVacio(enMunicipio.Text)|| !Regex.Match(enMunicipio.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚ\s0-9]+$").Success)
            {
                await  DisplayAlert("Campo incorrecto", "Introduzca un municipio valido", "ok");
                a5 = false;

            }
            else
            {
             
                a5 = true;
            }

            if (ValidarVacio(enEstado.Text) || !Regex.Match(enEstado.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚÜü\s0-9]+$").Success)
            {
                await DisplayAlert("Campo vacio", "Introduzca un estado valido", "ok");
                a6 = false;

            }
            else
            {

                a6 = true;
            }
            if (!(pkAsentamiento.SelectedIndex > -1))
            {
                await DisplayAlert("Campo vacio", "seleccione su asentamiento", "ok");
                a7 = false;

            }else{
                a7 = true;
            }

            if(a1&&a2&&a3&&a4&&a5&&a6&&a7){

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
                            id= (cargaCP)?cargaID.ToString():null,
                            asentamiento = pkAsentamiento.Items[pkAsentamiento.SelectedIndex],
                            tipoasentamiento = pkTipoAsentamiento.Items[pkTipoAsentamiento.SelectedIndex],
                            cp = enCP.Text,
                            ciudad = enCiudad.Text,
                            municipio = enMunicipio.Text,
                            estado = enEstado.Text,
                            pais = pkpais.Items[pkpais.SelectedIndex]

                        }

                    };
                }





               
           
               


                cliente.POST(Constantes.URL+"/direccion/agregar",agredire,1);

                MessagingCenter.Subscribe<ClienteRest>(this, "OK", (Sender) =>
                {
                    DisplayAlert("Guardado", "¡Direccion Añadida con Exito!", "OK");
                    Navigation.PopAsync();
                    MessagingCenter.Unsubscribe<ClienteRest>(this, "OK");
                });
                MessagingCenter.Subscribe<ClienteRest>(this, "error", (Sender) => {
                    DisplayAlert("Advertencia", "¡No fué posible añadir la dirección actual!", "error");
                    Navigation.PopAsync();
                    MessagingCenter.Unsubscribe<ClienteRest>(this, "error");

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
            System.Diagnostics.Debug.WriteLine("dlansdlansdpñ");
            bool a1 = false, a2 = false, a3 = false, a4 = false, a5 = false, a6 = false,a7=false,a8=true;


            if (ValidarVacio(enCalle.Text)|| !Regex.Match(enCalle.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚüÜ\s0-9]+$").Success)
            {
                DisplayAlert("Campo no valida", "Introduza una calle válida", "Ok");
                a1 = false;

            }
            else
            {

                a1 = true;
            }

            if (ValidarVacio(enNumero.Text) || !Regex.Match(enNumero.Text, "^[a-zA-Z0-9/-]*$").Success)
            {
                DisplayAlert("Campo no valido", "Introduza una número valido", "Ok");
                a2 = false;

            }
            else
            {

                a2 = true;
            }


            if (ValidarVacio(enCP.Text) || enCP.Text.Length < 5)
            {
                DisplayAlert("Campo no valido", "Introduza una código postal valido", "Ok");
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

            if (ValidarVacio(enMunicipio.Text)|| !Regex.Match(enMunicipio.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚÜü\s0-9]+$").Success)
            {
                DisplayAlert("Campo vacio", "Introduzca un municipio valido", "ok");
                a5 = false;

            }
            else
            {

                a5 = true;
            }

            if (ValidarVacio(enEstado.Text) || !Regex.Match(enEstado.Text, @"^[a-zA-ZñÑáéíóúÁÉÍÓÚÜü\s0-9]+$").Success)
            {
                 DisplayAlert("Campo incorrecto", "Introduzaca un estado valido", "ok");
                a6 = false;

            }
            else
            {

                a6 = true;
            }

            if (ValidarVacio(enAsentamiento.Text))
            {
                DisplayAlert("Campo vacio", "Introduzaca un asentamiento", "ok");
                a7 = false;

            }
            else
            {

                a7 = true;
            }

            if(cargaCP){
                if (!(pkAsentamiento.SelectedIndex > -1))
                {
                    DisplayAlert("Campo vacio", "seleccione su asentamiento", "ok");
                    a8 = false;

                }
                else
                {
                    a8 = true;
                }
            }

            if (a1 && a2 && a3 && a4 && a5 && a6 &&a7&&a8)
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
                    asentamiento = (cargaCP)?pkAsentamiento.Items[pkAsentamiento.SelectedIndex]:enAsentamiento.Text,
                    tipoasentamiento =pkTipoAsentamiento.Items[pkTipoAsentamiento.SelectedIndex] ,
                    cp = enCP.Text,
                    ciudad = enCiudad.Text,
                    municipio = enMunicipio.Text,
                    estado = enEstado.Text,
                    pais = pkpais.Items[pkpais.SelectedIndex]

                };


                client.PUT(Constantes.URL+"/direccion/actualizar", dir);

                MessagingCenter.Subscribe<ClienteRest>(this, "OK", (Sender) => {
                    MessagingCenter.Unsubscribe<ClienteRest>(this, "OK");
                    DisplayAlert("Guardado", "¡Direccion Modificada con Exito!", "OK");
                    Navigation.PopAsync();

                });


                MessagingCenter.Subscribe<ClienteRest>(this, "error", (Sender) => {
                    MessagingCenter.Unsubscribe<ClienteRest>(this, "error");
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

                        if(tipos==0){
                            for (var i = 0; i < pkTipoAsentamiento.Items.Count; i++)
                            {
                                if (pkTipoAsentamiento.Items[i].Equals(tipoAsenta))
                                {
                                    pkTipoAsentamiento.SelectedIndex = i;
                                }
                            }
                        }
                       


                    }


                }
                else { await DisplayAlert("Error de conexion", "No hay conexión a internet", "ok"); }

            });
        }


        async void algo_Completed(object sender, System.EventArgs e)
        {

           
            var cp = ((Entry)sender).Text;

            if (CrossConnectivity.Current.IsConnected)
            {
                enAsentamiento.IsVisible = false;
                pkAsentamiento.IsVisible = true;

                cargaCP = true;
                ClienteRest cliente = new ClienteRest();
                var resp = await cliente.GET<CodigoPostal>(Constantes.URL + "/catalogo-dirs/mostrarCatalogo/" + cp);

                if (resp != null)
                {
                    pkAsentamiento.Items.Clear();

                    asentmientos = new List<CargaCP>();
                    foreach (var item in resp.respuesta)
                    {
                        asentmientos.Add(new CargaCP
                        {

                               tipo = item.tipoasentamiento,
                            asentamiento = item.asentamiento,
                            catID = item.id
                        });

                    }

                 
                    foreach(var asent in asentmientos){
                        pkAsentamiento.Items.Add(asent.asentamiento);
                    }


                 
                    enCiudad.Text = resp.respuesta[0].ciudad;
                    enAsentamiento.Text = resp.respuesta[0].asentamiento;
                    enMunicipio.Text = resp.respuesta[0].municipio;
                    enEstado.Text = resp.respuesta[0].estado;
                    //enTipoAsentamiento.Text = resp.respuesta[0].tipoasentamiento;
                    //enPais.Text = resp.respuesta[0].pais;




                }
            }
            else { await DisplayAlert("Error de conexion", "No hay conexión a internet", "ok"); }
        }

        void Handle_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            for (var i = 0; i < pkTipoAsentamiento.Items.Count; i++)
            {
                int position = pkAsentamiento.SelectedIndex;
                if (pkTipoAsentamiento.Items[i].Equals(asentmientos[position]))
                {
                    pkTipoAsentamiento.SelectedIndex = i;
                    break;
                }
            }


        }

        public void validarNumero(object sender, TextChangedEventArgs args)
        {
            if (!Regex.IsMatch(args.NewTextValue, "^[0-9/-A-Fa-f]+$", RegexOptions.CultureInvariant))
                (sender as Entry).Text = Regex.Replace(args.NewTextValue, "[^0-9]", string.Empty);
            Entry entry = sender as Entry;
            String val = entry.Text;
            if (val.Length > 5)
            {
                val = val.Remove(val.Length - 1);
                entry.Text = val;
            }

        }
    }
}
