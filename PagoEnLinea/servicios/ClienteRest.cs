using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
                    jsonString = "{\"data\":" + jsonString + "}";
                    System.Diagnostics.Debug.WriteLine(jsonString);
                    return JsonConvert.DeserializeObject<T>(jsonString); ;

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

        public async void POST(string url, object obj){
            HttpResponseMessage response;
           
            string ContentType = "application/json"; // or application/xml
            var jsonstring = JsonConvert.SerializeObject(obj);

            jsonstring = jsonstring.Substring(1, jsonstring.Length - 2);
            System.Diagnostics.Debug.WriteLine(jsonstring);
            try{
                HttpClient cliente = new HttpClient();
                response = await cliente.PostAsync(url, new StringContent(jsonstring, Encoding.UTF8, ContentType));

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    System.Diagnostics.Debug.WriteLine("SE GUARDO POR POST");
                }
            }catch(HttpRequestException e){
                System.Diagnostics.Debug.WriteLine(e.InnerException.Message);
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
                    System.Diagnostics.Debug.WriteLine("SE GUARDO POR PUT");
                    MessagingCenter.Send(this, "error");

                }else{MessagingCenter.Send(this, "error"); System.Diagnostics.Debug.WriteLine(response);}
            }
            catch (HttpRequestException e)
            {
                System.Diagnostics.Debug.WriteLine(e.InnerException.Message);
            }
        }
    }
}
