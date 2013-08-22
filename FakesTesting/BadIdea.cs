using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Helpers;

namespace FakesTesting
{
    public class BadIdea
    {
        /// <summary>
        /// public wrapper, returns part of the object
        /// </summary>
        /// <returns></returns>
        public string GetNewBadIdea()
        {
            try
            {
                var json = GetBadIdea();
                return json.badidea;
            }
            catch (Exception)
            {
                return "Uh Oh. We just had an accident.";
            }
        }


        /// <summary>
        /// private method to handle network
        /// </summary>
        /// <returns></returns>
        private dynamic GetBadIdea(bool weirdBool = false)
        {
            if (!weirdBool){
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = httpClient.GetAsync("http://badideas.herokuapp.com/").Result;
                response.EnsureSuccessStatusCode(); 
                var responseContent = response.Content;
                var responseBody = responseContent.ReadAsStringAsync().Result;
                var json = Json.Decode(responseBody);
                return json;
            }
            dynamic d = new
                {
                    badidea = "It's like soylent for foodies"
                };
            return d;
        }
    }
}
