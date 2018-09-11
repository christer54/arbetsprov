namespace MusicCollector.WebConnect
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    public class HttpConnector
    {
        public struct AuthKey {
            public string urlToken { get; set; }
            public string clientId { get; set; }
            public string clientSecret { get; set; }
        }
        private string _authFile = @"../MusicCollector/authKey.json";
        private AuthKey authKey = new AuthKey();
        public HttpConnector()
        {
            try
            {
                string sr = File.ReadAllText(_authFile);
                Console.WriteLine(sr);
                authKey = JsonConvert.DeserializeObject<AuthKey>(File.ReadAllText(_authFile));
            }
            catch
            {
                Console.WriteLine("Misslyckades läsa in {0} filen korrekt",_authFile);
            }
            
        }
 
        public struct SpotifyAuthenticationToken {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
        }
        private SpotifyAuthenticationToken _token;
        private DateTime _expires = DateTime.MinValue;

        private void set_Expires()
        {
            _expires = _expires = DateTime.Now.AddSeconds(_token.expires_in);
        }

        HttpClient _httpClient = new HttpClient();
        private HttpWebResponse webRequest(string url)
        {
            // https://developer.spotify.com/documentation/general/guides/authorization-guide/
            HttpWebRequest webRequest = WebRequest.CreateHttp(url);
            //var encode_clientid_clientsecret = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", _clientid, _clientsecret)));
            var encode_clientid_clientsecret = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", authKey.clientId, authKey.clientSecret)));
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Accept = "application/json";
            webRequest.Headers.Add("Authorization: Basic " + encode_clientid_clientsecret);

            var request = "grant_type=client_credentials"; /// grant_type=refresh_token
            /// 
            byte[] req_bytes = Encoding.ASCII.GetBytes(request);
            webRequest.ContentLength = req_bytes.Length;

            Stream strm = webRequest.GetRequestStream();
            strm.Write(req_bytes, 0, req_bytes.Length);
            strm.Close();
            return (HttpWebResponse)webRequest.GetResponse();
        }
        public AuthenticationHeaderValue getHeaderAuthentication() 
        {
            if(DateTime.Now>_expires)
            {
                HttpWebResponse resp = webRequest(authKey.urlToken);
                string json = string.Empty;
                using (Stream respStr = resp.GetResponseStream()) 
                {
                    using (StreamReader rdr = new StreamReader(respStr, Encoding.UTF8)) 
                    {
                        //should get back a string i can then turn to json and parse for accesstoken
                        json = rdr.ReadToEnd();
                        rdr.Close();
                    }
                }
                //Console.WriteLine(json);
                //Console.ReadKey();
                _token = JsonConvert.DeserializeObject<SpotifyAuthenticationToken>(json);
                set_Expires();
            }
            else
            {
                Console.WriteLine("No need to getToken");
            }
            return new AuthenticationHeaderValue("Authorization", "Bearer "+_token.access_token); //_token.access_token;
        }
        public void expires()
        {
            Console.WriteLine("Token expires: {0}",_expires);
        }
        public string getJsonData(string url) 
        {
            string webResponse = string.Empty;
            
            try 
            {
                var request = new HttpRequestMessage() // url måste vara korrekt annars krasch
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Get
                };
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                request.Headers.Authorization = getHeaderAuthentication();

                var task = _httpClient.SendAsync(request).ContinueWith((taskwithmsg) => 
                {
                    var response = taskwithmsg.Result;
                    var jsonTask = response.Content.ReadAsStringAsync();
                    webResponse = jsonTask.Result;
                });
                task.Wait();
            }
            catch (WebException ex) 
            {
                Console.WriteLine("Web Request Error: " + ex.Status);
            } 
            catch (TaskCanceledException tex) 
            {
                Console.WriteLine("Task Request Error: " + tex.Message);
            }
            return webResponse;
        }
    }
}