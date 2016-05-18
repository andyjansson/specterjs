using Microsoft.ClearScript;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Web;

namespace SpecterJS.Bindings.Modules.WebServer
{
    public class Request : PropertyBag
    {
        public Request(HttpListenerRequest request)
        {
            Method = request.HttpMethod;
            Url = request.Url.AbsolutePath;
            HttpVersion = request.ProtocolVersion.ToString();
            Headers = GetHeaders(request);
            using (StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding))
            {
                var data = reader.ReadToEnd();
                RawPostData = data;
                PostData = !string.IsNullOrEmpty(request.ContentType) && 
                    request.ContentType.Equals("application/x-www-form-urlencoded")
                    ? HttpUtility.UrlDecode(data)
                    : data;
            }
        }

        private static IDictionary<string, object> GetHeaders(HttpListenerRequest request)
        {
            var headers = (IDictionary<string, object>)new ExpandoObject();
            foreach (var key in request.Headers.AllKeys)
            {
                headers.Add(key, request.Headers[key]);
            }
            return headers;
        }

        [ScriptMember(Name = "method")]
        public string Method { get; set; }

        [ScriptMember(Name = "url")]
        public string Url { get; set; }

        [ScriptMember(Name = "httpVersion")]
        public string HttpVersion { get; set; }

        [ScriptMember(Name = "headers")]
        public dynamic Headers { get; set; }

        [ScriptMember(Name = "post")]
        public string PostData { get; set; }

        [ScriptMember(Name = "postRaw")]
        public string RawPostData { get; set; }
    }
}
