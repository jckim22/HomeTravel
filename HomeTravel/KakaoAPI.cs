using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace HomeTravel
{
    static class KakaoAPI
    {
        internal static List<MyLocale> Search(string query)
        {
            string site = "https://dapi.kakao.com/v2/local/search/keyword.json";
            string rquery = string.Format("{0}?query={1}", site, query);
            WebRequest request = WebRequest.Create(rquery);
            string rKey = "{나의 REST API KEY}";
            string header = "KakaoAK " + rKey;
            request.Headers.Add("Authorization", header);

            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            String json = reader.ReadToEnd();
            
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            using JsonDocument document = JsonDocument.Parse(json);
            JsonElement root = document.RootElement;
            JsonElement documents = root.GetProperty("documents");

            List<MyLocale> mls = new List<MyLocale>();
            foreach (JsonElement doc in documents.EnumerateArray())
            {
                string lname = doc.GetProperty("place_name").GetString();
                double x = double.Parse(doc.GetProperty("x").GetString());
                double y = double.Parse(doc.GetProperty("y").GetString());
                mls.Add(new MyLocale(lname, y, x));
            }

            return mls;

        }
    }
}
