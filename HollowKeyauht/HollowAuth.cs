using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Management;
using System.Net.Http;
using System.Text;

namespace HollowAuth
{
    public class api
    {
        private readonly string name;
        private readonly string appId;
        private readonly string secret;
        private readonly string version;
        private readonly string apiUrl;

        private static readonly HttpClient client = new HttpClient();

        public response_class response = new response_class();
        public user_data_class user_data = new user_data_class();

        public api(string name, string appId, string secret, string version, string apiUrl)
        {
            this.name = name;
            this.appId = appId;
            this.secret = secret;
            this.version = version;
            this.apiUrl = apiUrl.TrimEnd('/');
        }

        public void init()
        {
            var data = new Dictionary<string, string>
            {
                { "name", name },
                { "appId", appId },
                { "secret", secret },
                { "version", version }
            };

            var result = post("/init", data);

            response.success = result.success;
            response.message = result.message;
        }

        public void register(string username, string password, string licenseKey)
        {
            var data = new Dictionary<string, string>
            {
                { "appId", appId },
                { "secret", secret },
                { "username", username },
                { "password", password },
                { "license", licenseKey },
                { "hwid", get_hwid() }
            };

            var result = post("/register", data);

            response.success = result.success;
            response.message = result.message;

            if (result.success)
            {
                user_data.username = result.username;
                user_data.subscription = result.subscription;
                user_data.expires = result.expiresAt;
            }
        }

        public void login(string username, string password)
        {
            var data = new Dictionary<string, string>
            {
                { "appId", appId },
                { "secret", secret },
                { "username", username },
                { "password", password },
                { "hwid", get_hwid() }
            };

            var result = post("/login", data);

            response.success = result.success;
            response.message = result.message;

            if (result.success)
            {
                user_data.username = result.username;
                user_data.subscription = result.subscription;
                user_data.expires = result.expiresAt;
            }
        }

        public void license(string licenseKey)
        {
            var data = new Dictionary<string, string>
            {
                { "appId", appId },
                { "secret", secret },
                { "license", licenseKey },
                { "hwid", get_hwid() }
            };

            var result = post("/license", data);

            response.success = result.success;
            response.message = result.message;

            if (result.success)
            {
                user_data.username = "License User";
                user_data.subscription = result.subscription;
                user_data.expires = result.expiresAt;
            }
        }

        public string var(string variableName)
        {
            var data = new Dictionary<string, string>
            {
                { "appId", appId },
                { "secret", secret },
                { "name", variableName }
            };

            var result = post("/variable", data);

            response.success = result.success;
            response.message = result.message;

            if (!result.success)
                return "";

            return result.value;
        }

        private api_response post(string endpoint, Dictionary<string, string> data)
        {
            try
            {
                string json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage http = client
                    .PostAsync(apiUrl + endpoint, content)
                    .GetAwaiter()
                    .GetResult();

                string text = http.Content
                    .ReadAsStringAsync()
                    .GetAwaiter()
                    .GetResult();

                if (string.IsNullOrWhiteSpace(text))
                {
                    return new api_response
                    {
                        success = false,
                        message = "Respuesta vacía del servidor."
                    };
                }

                var result = JsonConvert.DeserializeObject<api_response>(text);

                if (result == null)
                {
                    return new api_response
                    {
                        success = false,
                        message = "Respuesta inválida del servidor."
                    };
                }

                return result;
            }
            catch (Exception ex)
            {
                return new api_response
                {
                    success = false,
                    message = "Error de conexión: " + ex.Message
                };
            }
        }

        private string get_hwid()
        {
            try
            {
                string hwid = "";

                ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                    "SELECT UUID FROM Win32_ComputerSystemProduct"
                );

                foreach (ManagementObject obj in searcher.Get())
                {
                    hwid = obj["UUID"]?.ToString();
                    break;
                }

                if (string.IsNullOrWhiteSpace(hwid))
                    hwid = Environment.MachineName + "-" + Environment.UserName;

                return hwid;
            }
            catch
            {
                return Environment.MachineName + "-" + Environment.UserName;
            }
        }

        private class api_response
        {
            public bool success { get; set; }
            public string message { get; set; } = "";

            public string username { get; set; } = "";
            public string subscription { get; set; } = "";
            public string expiresAt { get; set; } = "";
            public string value { get; set; } = "";
        }

        public class response_class
        {
            public bool success { get; set; }
            public string message { get; set; } = "";
        }

        public class user_data_class
        {
            public string username { get; set; } = "";
            public string subscription { get; set; } = "";
            public string expires { get; set; } = "";
        }
    }
}
