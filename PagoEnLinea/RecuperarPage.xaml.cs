﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using PagoEnLinea.Modelos;
using Xamarin.Forms;

namespace PagoEnLinea
{
    public partial class RecuperarPage : ContentPage
    {
        public RecuperarPage()
        {
            InitializeComponent();
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
           

            HttpResponseMessage response;

            string ContentType = "application/json"; // or application/xml
            var jsonstring = enCorreo.Text;

            System.Diagnostics.Debug.WriteLine(jsonstring);
            try
            {
                HttpClient cliente = new HttpClient();

                response = await cliente.PostAsync(Constantes.URL+"/account/reset-password?movil=true", new StringContent(jsonstring, Encoding.UTF8, ContentType));
                var y = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine(y);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    
                    System.Diagnostics.Debug.WriteLine("SE ENVIO POR POST");
                   
                    await DisplayAlert("Información", "Se ha enviado un enlace a su correo electónico una vez que lo verifique podrá establecer una nueva contraseña", "OK");
                    await Navigation.PopToRootAsync();

                }
               
                else
                {

                    var resp = JsonConvert.DeserializeObject<Respuesta>(y);

                    await DisplayAlert("Error",resp.respuesta,"OK");


                }
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.InnerException.Message);

            }


          //  await  Navigation.PushAsync(new RecuperarContraseñaPage());
        }
    }
}