﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PagoEnLinea.Modelos;
using PagoEnLinea.servicios;
using Xamarin.Forms;

namespace PagoEnLinea.Paginas
{
   
    /// <summary>
    /// Esta clase muestra una pantalla ya sea para modificar o añadir información de facturación
    /// según el tipo de parámetros que recibe ya que muestra diferentes botones con eventos asociados.
    /// </summary>
    public partial class Modificarfacturacion : ContentPage
    {
        public static List<Modelos.infodir> listDir;
        public static List<Modelos.Telefono> listTel = new List<Telefono>();
        public static List<Email> listEmail;
        public static DatosFacturacion facturacion;

        string ID, IDDIR, IDCATDIR,email,direccion;
        int tipos;

        /// <summary>
        /// inicializa los componentes visuales de su XAML
        /// muestra u oculta componentes según el tipo de parámetro recibido
        /// añade evento de textChanged a la entrada RFC 
        /// <param name="id">id de los datos de facturación a modificar</param>
        /// <param name="idDir">id de la direccion asociada a los datos de facturación a modificar</param>
        /// <param name="idcat">Id del catalogo de direcciones asociados a los datos de facturacion a modificar</param>
        /// <param name="tipo">tipo de pantalla que se mostrará, 0 para tipo modificar 1 para tipo añadir</param>
        /// <param name="correo">Correo a modificar asociado a los datos de facturación</param>
        /// <param name="dir">direccón a modificar asociada a los datos de facturación </param>
        public Modificarfacturacion(string id,string idDir, string idcat,int tipo,string correo,string dir)
        {
            facturacion = new DatosFacturacion();
            ID = id;
            IDDIR = idDir;
            IDCATDIR = idcat;
            tipos = tipo;
            email = correo;
            direccion = dir;

            InitializeComponent();
            enRFC.TextChanged += OnRFCChanged;
            if (tipo == 0)
            {
                btnModificar.IsVisible = true;
                btnAgregar.IsVisible = false;
            }
            if (tipo == 1)
            {
                btnAgregar.IsVisible = true;
                btnModificar.IsVisible = false;
            }

        }

        /// <summary>
        /// consume al servicio que contiene la información del usuario para mostrarla y pueda ser seleccionada como datos de facturación
        /// entre este información se encuentra sus direcciones y correos añadidos
        /// </summary>
        async void conectar()
        {
            
            if (Application.Current.Properties.ContainsKey("token"))
            {
                var cont = 1;

                ClienteRest cliente = new ClienteRest();
                var inf = await cliente.InfoUsuario<InfoUsuario>(Application.Current.Properties["token"] as string);
                listDir = new List<Modelos.infodir>();
                listEmail = new List<Email>();


                if (inf != null)
                {
                    foreach (var dato in inf.direcciones)
                    {

                        listDir.Add(new Modelos.infodir
                        {
                            NumerodeDireccion = "Dirección " + cont + ":",
                            id = dato.id,
                            calle = dato.calle,
                            numero = dato.numero,
                            numeroInterior = dato.numeroInterior,
                            tipo = dato.tipo,

                            cp = dato.catalogoDir.cp,
                            asentamiento = dato.catalogoDir.asentamiento,
                            municipio = dato.catalogoDir.municipio,
                            estado = dato.catalogoDir.estado,
                            pais = dato.catalogoDir.pais,
                            tipoasentamiento = dato.catalogoDir.tipoasentamiento,
                            ciudad = dato.catalogoDir.ciudad,
                            idCat = dato.catalogoDir.id




                        });

                        cont++;
                       
                    }

                   



                    foreach (var dato in inf.email)
                    {

                        listEmail.Add(new Email
                        {
                            id = dato.id,
                            correoe = dato.correoe,
                            tipo = dato.tipo,



                        });

                    }

                    System.Diagnostics.Debug.WriteLine(listEmail.Count);
                    foreach (var corr in listEmail){

                        pkCorreo.Items.Add(corr.correoe);
                    }

                    //System.Diagnostics.Debug.WriteLine(listEmail[3].id);
                       
                    

                    foreach(var dir in listDir){
                        pkDireccion.Items.Add(dir.asentamiento + " " + dir.calle + " " + dir.numero);
                    }

                  
                    facturacion.id = ID;
                    facturacion.persona = inf.persona;


                    if (tipos == 0)
                    {
                        for (var i = 0; i < pkCorreo.Items.Count; i++)
                        {
                            
                                if (pkCorreo.Items[i].Equals(email))
                                {
                                    pkCorreo.SelectedIndex = i;
                                }


                        }


                        for (var i = 0; i < pkDireccion.Items.Count; i++)
                        {
                            
                                if (pkDireccion.Items[i].Contains(direccion))
                                {
                                    pkDireccion.SelectedIndex = i;
                                }
                            

                        }
                    }
                
                }

            }
        }
        /// <summary>
        /// Llama al método conectar una vez que la pantalla para Modificar o agregar datos de facturación
        /// </summary>
        protected override void OnAppearing()
        {
            conectar();
        }
      

        /// <summary>
        /// Obtiene el indice de la dirección seleccionado apartir del picker
        /// este indice corresponde tambien a la posision de una lista temporal de tipo email
        /// el cual tiene en sus propiedades el ID. Por lo que al seleccionar un elemento del picker
        /// es posible obtener un ID apartir del indice del picker seleccionado y añadirlo al objecto
        /// facturacion que posteriormente sera mandado como JSON al consumir los servicios correspondientes
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void Direccion_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            int position = pkDireccion.SelectedIndex;
            if(pkDireccion.SelectedIndex>-1){

                IDDIR = listDir[position].id;
                facturacion.direccion = new Modelos.Direccion
                {
                    id = IDDIR
                };
               
            }  
        }


        /// <summary>
        /// Obtiene el indice del correo seleccionado apartir del picker
        /// este indice corresponde tambien a la posision de una lista temporal de tipo infodir
        /// el cual tiene en sus propiedades el ID. Por lo que al seleccionar un elemento del picker
        /// es posible obtener un ID apartir del indice del picker seleccionado y añadirlo al objecto
        /// facturacion que posteriormente sera mandado como JSON al consumir los servicios correspondientes
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void Correo_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            int position = pkCorreo.SelectedIndex;
            if(pkCorreo.SelectedIndex>-1){
                facturacion.email = new Email()
                {
                    id = listEmail[position].id
                };
            }
        }

        /// <summary>
        /// Este evento corresponde al boton agregar cuando se hace click al boton
        /// primero valida que todos los campos añadidos sean validos y una vez que todo sea correcto
        /// consume al servicio para añadir nueva informacion de facturacion.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void Agregar_Clicked(object sender, System.EventArgs e)
        {
            facturacion.rfc = enRFC.Text;
            facturacion.nomrazonSocial = enRazon.Text;



            ClienteRest client = new ClienteRest();

            if(!(string.IsNullOrEmpty(enRFC.Text))&&!(string.IsNullOrEmpty(enRazon.Text))&&pkCorreo.SelectedIndex > -1&&pkDireccion.SelectedIndex > -1){
                

                if (!Regex.Match(enRFC.Text, "[A-Z,Ñ,&]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9]?[A-Z,0-9]?[0-9,A-Z]?").Success||enRFC.Text.Length<12||enRFC.Text.Length>13)
                {
                    DisplayAlert("Advertencia", "Introduzca un RFC valido", "OK");
                }else{
                    client.POST(Constantes.URL_USUARIOS+"/datos-facturacion/agregar", facturacion, 1); 
                }

            }else{
                DisplayAlert("Advertencia","Llene y/o seleccione todos los campos","OK");

            }


            MessagingCenter.Subscribe<ClienteRest>(this, "OK", (Sender) => {
                MessagingCenter.Unsubscribe<ClienteRest>(this,"OK");
                DisplayAlert("Guardado", "¡Información de facturación añadida con exito!", "OK");
                Navigation.PopAsync();

            });

            MessagingCenter.Subscribe<ClienteRest>(this, "error", (Sender) => {
                MessagingCenter.Unsubscribe<ClienteRest>(this, "error");
                DisplayAlert("Error", "¡No fue posible añadir la informació de facturación actual!", "OK");
                Navigation.PopAsync();

            });

        }

        /// <summary>
        /// Este evento corresponde al boton modificar cuando se hace click al boton
        /// primero valida que todos los campos añadidos sean validos y una vez que todo sea correcto
        /// consume al servicio para modificar la información de facturacion.
          /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void Modificar_Clicked(object sender, System.EventArgs e)
        {
            facturacion.rfc = enRFC.Text;
            facturacion.nomrazonSocial = enRazon.Text;

            facturacion.direccion = new Modelos.Direccion
            {
                id = IDDIR,
                catalogoDir = new CatalogoDir{
                    id= IDCATDIR
                }
            };
            ClienteRest client = new ClienteRest();


            if(!(string.IsNullOrEmpty(enRFC.Text)) && !(string.IsNullOrEmpty(enRazon.Text))&&pkCorreo.SelectedIndex>-1&&pkDireccion.SelectedIndex > -1){
              
                if (!Regex.Match(enRFC.Text, "[A-Z,Ñ,&]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9]?[A-Z,0-9]?[0-9,A-Z]?").Success|| enRFC.Text.Length < 12 || enRFC.Text.Length > 13)
                {
                    DisplayAlert("Advertencia", "Introduzca un RFC valido", "OK");
                }else{
                    client.PUT(Constantes.URL_USUARIOS + "/datos-facturacion/actualizar", facturacion);
                }

            }else{
                DisplayAlert("Advertencia", "¡Seleccione el resto de campos!", "OK");
            }

          

            MessagingCenter.Subscribe<ClienteRest>(this, "OK", (Sender) => {
                MessagingCenter.Unsubscribe<ClienteRest>(this, "OK");
                DisplayAlert("Modificado", "¡Información de facturación modificada con exito!", "OK");
                Navigation.PopAsync();

            });

            MessagingCenter.Subscribe<ClienteRest>(this, "error", (Sender) => {
                MessagingCenter.Unsubscribe<ClienteRest>(this, "error");
                DisplayAlert("Error", "¡No fue posible modificar la información actual!", "OK");
                Navigation.PopAsync();

            });




            
        }

        /// <summary>
        /// convierte el texto ingresado en la entrada de manera dinámica RFC en mayusculas
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
         public void OnRFCChanged(object sender, TextChangedEventArgs args)
        {
           
            (sender as Entry).Text = args.NewTextValue.ToUpper();

           
        }
    }
}
