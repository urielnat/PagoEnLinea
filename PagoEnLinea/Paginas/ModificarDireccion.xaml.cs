using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PagoEnLinea.Modelos;
using PagoEnLinea.servicios;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace PagoEnLinea.Paginas
{
    /// <summary>
    /// Esta clase muestra una pantalla ya sea para modificar o añadir una dirección
    /// según el tipo de parámetros que recibe ya que muestra diferentes botones con eventos asociados.
    /// </summary>
    public partial class ModificarDireccion : ContentPage
    {
        
        string ID,catID,tipoAsenta;
        public static int tipos;
        public static List<string> tipoas;
        public static List<CargaCP> asentmientos;
        public static bool cargaCP;
        public static int cargaID;

        /// <summary>
        /// inicializa los componentes visuales de su XAML
        /// muestra u oculta componentes según el tipo de parámetro recibido
        /// añade evento de textChanged a la entrada codigo postal y a la entrada número de dirección
        /// añade evento de tipo completed a la entrada codigo postal (cuando se presiona tecla de retorno en teclado virtual)
        /// </summary>
        /// <param name="id">id de la dirección a modificar</param>
        /// <param name="idcat">id del catálogo de direcciones a modificar</param>
        /// <param name="tipo">tipo de pantalla que se mostrará, 0 para tipo modificar 1 para tipo añadir</param>
        /// <param name="estado">No implementado actualmente</param>
        /// <param name="tipoAsentamiento">Tipo asentamiento a modificar</param>
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


        /// <summary>
        /// Obtiene el indice del tipo de asentamiento seleccionado apartir del picker
        /// este indice corresponde tambien a la posision de una lista temporal de tipo tipo CargaCP
        /// el cual tiene en sus propiedades el asentamiento, tipo e ID. Por lo que al seleccionar un elemento del picker
        ///es posible obtener su ID apartir del indice del picker seleccionado y mostrarle al usuario
        /// automáticamente un tipo de asentamiento si corresponde al cargado en la lista temporal
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
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


        /// <summary>
        /// este método valida que los campos ingresados en las entradas sean correctos antes de consumir al servicio
        /// de añadir una nueva dirección, en caso contrario se notifica al usuario
        /// </summary>
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





               
           
               


                cliente.POST(Constantes.URL_USUARIOS+"/direccion/agregar",agredire,1);

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
       

        /// <summary>
        /// Método para verificar si una entrada de texto esta vacia 
        /// </summary>
        /// <returns><c>true</c>si la entrada esta vacio<c>false</c>caso contrario</returns>
        /// <param name="x">texto en la entrada</param>
        bool ValidarVacio(string x)
        {
            var auth = (String.IsNullOrEmpty(x)) ? true : false;
            return auth;
        }

        /// <summary>
        ///  evento click de boton modificar que hace una llamada al método Modificar();
        /// </summary>
        /// <param name="sender">objeto que hace referencia al evento</param>
        /// <param name="e">argumentos que son posibles de obtener apartir del objeto que hace llamada al evento</param>
       
        void Handle_Clicked(object sender, System.EventArgs e)
        {
            Modificar(); 
        }


        /// <summary>
        /// evento de la entrada codigo postal permite validar dinámicamente el texto ingresado en el teclado virtual
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Arguments.</param>
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

        /// <summary>
        /// Este método permite verificar que el texto introducido en las entradas sea correcto antes de consumir el servicio para
        /// modificar los datos de facturación. enc aso contrario notifica al usuario
        /// </summary>
        void Modificar(){
            
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


                client.PUT(Constantes.URL_USUARIOS+"/direccion/actualizar", dir);

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
       
        /// <summary>
        /// evento click del boton Agregar que llama al método Añadir una vez que es presionado
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void Guardar_Clicked(object sender, System.EventArgs e)
        {
            Añadir();
        }

        /// <summary>
        /// LLena el contenido del picker que contiene el tipo de asentamiento al consumir un servicio
        /// que carga los tipos de asentamientos, si el tipo de pantalla es 0 (modificar datos)
        /// muestra automáticamente el tipo de asentamiento que el usuario tenia ingresado
        /// </summary>
        void llenarPicker()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    ClienteRest client = new ClienteRest();
                    var httpclient = await client.GET<TipoAsentamiento>(Constantes.URL_USUARIOS+"/catalogo-dirs/tipo-asentamiento");
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
                else { await DisplayAlert("Error de conexión", "No hay conexión a internet", "ok"); }

            });
        }

        /// <summary>
        /// evento completed que se incia una vez que el usuario presiona la tecla de retorno en el teclado virutual
        /// una vez que termina de escribir los digitos del código postal
        /// este evento llama al servicio que muestra un catálogo de direcciones a partir del código postal ingresado 
        /// para poder llenar automáticamente algunos de los campos de direcciones (asentamiento, tipo, ciudad, municipio etc.)
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async void algo_Completed(object sender, System.EventArgs e)
        {

           
            var cp = ((Entry)sender).Text;

            if (CrossConnectivity.Current.IsConnected)
            {
                enAsentamiento.IsVisible = false;
                pkAsentamiento.IsVisible = true;

                cargaCP = true;
                ClienteRest cliente = new ClienteRest();
                var resp = await cliente.GET<CodigoPostal>(Constantes.URL_USUARIOS + "/catalogo-dirs/mostrarCatalogo/" + cp);

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





        /// <summary>
        /// evento de la entrada Número de direccion valida que solo se puedan escribir cierto tipo de caracteres 
        /// a traves del teclado virtual
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Arguments.</param>
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
