using System;
using System.Collections.Generic;
using PagoEnLinea.Modelos;
using PagoEnLinea.servicios;
using Xamarin.Forms;

namespace PagoEnLinea.Paginas
{
    public partial class Modificarfacturacion : ContentPage
    {
        public static List<Modelos.infodir> listDir = new List<infodir>();
        public static List<Modelos.Telefono> listTel = new List<Telefono>();
        public static List<Email> listEmail = new List<Email>();
        public static DatosFacturacion facturacion = new DatosFacturacion();

        string ID, IDDIR, IDCATDIR;

        public Modificarfacturacion(string id,string idDir, string idcat,int tipo)
        {
            ID = id;
            IDDIR = idDir;
            IDCATDIR = idcat;
            InitializeComponent();
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


        async void conectar()
        {
            
            if (Application.Current.Properties.ContainsKey("token"))
            {
                var cont = 1;

                ClienteRest cliente = new ClienteRest();
                var inf = await cliente.InfoUsuario<InfoUsuario>(Application.Current.Properties["token"] as string);
                listDir = new List<Modelos.infodir>();



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
                        System.Diagnostics.Debug.WriteLine(dato.catalogoDir.municipio);
                    }

                    foreach (var dato in inf.telefonos)
                    {
                        //catdir.cp = dato.catalogoDir.cp;
                        listTel.Add(new Telefono
                        {

                            id = dato.id,
                            telefono = dato.telefono,
                            lada = dato.lada,
                            tipo = dato.tipo



                        });
                        //pkTelefono.Items.Add(dato.telefono);
                    }


                   
                        foreach (var dato in inf.email)
                        {
                            
                            listEmail.Add(new Email
                            {
                                id = dato.id,
                                correoe = dato.correoe,
                                tipo = dato.tipo,



                            });
                        pkCorreo.Items.Add(dato.correoe);
                        }


                       
                    

                    foreach (var email in listEmail)
                    {
                        
                    }

                    foreach(var direccion in listDir){
                        pkDireccion.Items.Add(direccion.asentamiento + " " + direccion.calle + " " + direccion.numero);
                    }

                    foreach (var telefono in listTel )
                    {
                     
                    }
                    facturacion.id = ID;
                    facturacion.persona = inf.persona;
                }

            }
        }
        protected override void OnAppearing()
        {
            conectar();
        }
        /**
        async void telefono_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ModificarTelefono(""));
        }

        async void correo_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ModificarCorreo(null));
        }**/

        void Direccion_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if(pkDireccion.SelectedIndex>-1){
                facturacion.direccion = new Modelos.Direccion()
                {
                    id = listDir[pkDireccion.SelectedIndex].id
                };
            }  
        }

        void Correo_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if(pkCorreo.SelectedIndex>-1){
                facturacion.email = new Email()
                {
                    id = listEmail[pkCorreo.SelectedIndex].id
                };
            }
        }

        void Agregar_Clicked(object sender, System.EventArgs e)
        {
            facturacion.rfc = enRFC.Text;
            facturacion.nomrazonSocial = enRazon.Text;

            ClienteRest client = new ClienteRest();

            if(!(string.IsNullOrEmpty(enRFC.Text))&&!(string.IsNullOrEmpty(enRazon.Text))&&pkCorreo.SelectedIndex > -1&&pkDireccion.SelectedIndex > -1){
                client.POST(Constantes.URL+"/datos-facturacion/agregar", facturacion, 1); 
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


            if(pkCorreo.SelectedIndex>-1&&pkDireccion.SelectedIndex > -1){
                client.PUT(Constantes.URL+"/datos-facturacion/actualizar", facturacion);
            }else{
                DisplayAlert("Advertencia", "¡Seleccione el resto de campos!", "OK");
            }

          

            MessagingCenter.Subscribe<ClienteRest>(this, "putfacturacion", (Sender) => {
                MessagingCenter.Unsubscribe<ClienteRest>(this, "putfacturacion");
                DisplayAlert("Modificado", "¡Información de facturación modificada con exito!", "OK");
                Navigation.PopAsync();

            });

            MessagingCenter.Subscribe<ClienteRest>(this, "errorfacturacion", (Sender) => {
                MessagingCenter.Unsubscribe<ClienteRest>(this, "errorfacturacion");
                DisplayAlert("Error", "¡No fue posible modificar la información actual!", "OK");
                Navigation.PopAsync();

            });




            
        }
    }
}
