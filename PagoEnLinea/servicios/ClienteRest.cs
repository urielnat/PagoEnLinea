using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PagoEnLinea.Modelos;
using PagoEnLinea.Paginas;
using Xamarin.Forms;

namespace PagoEnLinea.servicios
{
    public class ClienteRest
    {
        public async Task<T> GET<T>(string url)
        {

            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync(url);
                System.Diagnostics.Debug.WriteLine(response);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    System.Diagnostics.Debug.WriteLine("SE HIZO LA CONEXION");
                    var jsonString = await response.Content.ReadAsStringAsync();
                  
                    System.Diagnostics.Debug.WriteLine(jsonString);
                    return JsonConvert.DeserializeObject<T>(jsonString); 

                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(response);
                    MessagingCenter.Send(this, "error");
                }
            }
            catch (HttpRequestException e)
            {
                System.Diagnostics.Debug.WriteLine(e.InnerException.Message);
            }


            return default(T);
        }

        public async void POST(string url, object obj,int tipo){
            HttpResponseMessage response;
           
            string ContentType = "application/json"; // or application/xml
            var jsonstring = JsonConvert.SerializeObject(obj);

          
            try{
                HttpClient cliente = new HttpClient();
                response = await cliente.PostAsync(url, new StringContent(jsonstring, Encoding.UTF8, ContentType));
                var y = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine(y);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    System.Diagnostics.Debug.WriteLine("SE GUARDO POR POST");
                    if (tipo == 0)
                    {
                        MessagingCenter.Send(this, "OKR");
                    }

                    else
                    {
                        MessagingCenter.Send(this, "OK");
                    }


                }
                else if(tipo ==0)
                {
                    System.Diagnostics.Debug.WriteLine(response.ToString());
                    MessagingCenter.Send(this, "errorp");
                }else{
                    
                        System.Diagnostics.Debug.WriteLine(response.ToString());
                        MessagingCenter.Send(this, "error");
                    
                }
            }catch(HttpRequestException e){
                System.Diagnostics.Debug.WriteLine(e.InnerException.Message);
                MessagingCenter.Send(this, "error");
            }
        }

        public async void PUT(string url, object obj)
        {
            HttpResponseMessage response;

            string ContentType = "application/json"; // or application/xml
            var jsonstring = JsonConvert.SerializeObject(obj);

            //jsonstring = jsonstring.Substring(1, jsonstring.Length - 2);
            System.Diagnostics.Debug.WriteLine(jsonstring);
            try
            {
                HttpClient cliente = new HttpClient();
                response = await cliente.PutAsync(url, new StringContent(jsonstring, Encoding.UTF8, ContentType));

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    System.Diagnostics.Debug.WriteLine("ENTRO");
                    System.Diagnostics.Debug.WriteLine("SE GUARDO POR PUT");
                    MessagingCenter.Send(this, "putcontraseña");
                    MessagingCenter.Send(this, "putdireccion");
                    MessagingCenter.Send(this, "putcorreo");
                    MessagingCenter.Send(this, "putpersona");

                }else{
                    MessagingCenter.Send(this, "errorContraseña");
                    MessagingCenter.Send(this, "errorDireccion");
                    MessagingCenter.Send(this, "errorCorreo");
                    MessagingCenter.Send(this, "errorPersona");
                    System.Diagnostics.Debug.WriteLine(response);}
            }
            catch (HttpRequestException e)
            {
                System.Diagnostics.Debug.WriteLine(e.InnerException.Message);
            }
        }

        public async Task<T> InfoUsuario<T>(string user)
        {
            try
            {
                HttpResponseMessage response;
                string sUrl = "http://192.168.0.18:8080/api/info-contribuyente";
                string sContentType = "application/json"; // or application/xml
                info inf = new info();
                inf.usuario = user;




                var jsonstring = JsonConvert.SerializeObject(inf);





                HttpClient oHttpClient = new HttpClient();
                response = await oHttpClient.PostAsync(sUrl, new StringContent(jsonstring, Encoding.UTF8, sContentType));
                //oHttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiI1OWRiYWI3MGVhNmQzNDI0OTg1M2UxODgiLCJuYW1lIjoiUEVQRSIsInN1cm5hbWUiOiJQRVBFWCIsImVtYWlsIjoiQUxHT0BHTUFJTC5DT00iLCJyb2xlIjoiUk9MRV9VU0VSIiwiaW1hZ2UiOm51bGwsImlhdCI6MTUwNzU3MjEyNX0.chuQ7nrZ8sIqVQV1ODZpaW1jiA7pjGAQkjveRqtos0s");
                var y = await response.Content.ReadAsStringAsync();

                //System.Diagnostics.Debug.WriteLine                         ("{\"codigoBarras\":\"1231231389888\",\"nombre\":\"PRUEBAPRODUCT\",\"descripcion\":\"PRUEBAPRODUCTPRUEBAPRODUCT\",\"cantidad\":2,\"precioCompra\":12.22,\"precioVenta\":13.22,\"categoria\":\"VEGETALES\"}");
                var oTaskPostAsync = oHttpClient.PostAsync(sUrl, new StringContent(jsonstring, Encoding.UTF8, sContentType));
                //System.Diagnostics.Debug.WriteLine(response.Content.ToString());
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                  

                    var users = JsonConvert.DeserializeObject<InfoUsuario>(y);
                    System.Diagnostics.Debug.WriteLine(users.persona.nombre);
                    return JsonConvert.DeserializeObject<T>(y);


                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(response);
                }
            }catch(HttpRequestException e)
            {
                System.Diagnostics.Debug.WriteLine(e.InnerException.Message);
            }
            return default(T);
        }
    }
}
