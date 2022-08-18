using BaranMasterDataService.Database;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BaranMasterDataService
{
    public class RestSharp
    {
        private readonly string _url = "https://jsonplaceholder.typicode.com/";     

        private RestResponse getResponse()
        {
            var client = new RestClient(_url);
            var request = new RestRequest("posts/{postid}", Method.Get);
            request.AddUrlSegment("postid", 2);

            RestResponse response = client.Execute(request);
            return response;

        }
        public CNMaterials deSerialize()
        {
            RestResponse response = getResponse();
            CNMaterials cnMaterials = JsonSerializer.Deserialize<CNMaterials>(response.Content);
            return cnMaterials;

        }
        private string serialize(CNMaterials cnMaterials)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonFormat = JsonSerializer.Serialize(cnMaterials, options);
            return jsonFormat;


        }
        public void postAndSerialize()
        {
            CNMaterials cnMaterials = new CNMaterials()
            {

                Material = "a",
                SText = "a",
                QUnit = "a",
                CNWDate = "a",
                DDWDate = "a",
                FSRDate = "a",
                CNRDate = "a",
                DDRDate = "a",
                FSWDate = "a",

            };
            string jsonFormat = serialize(cnMaterials);


            var client = new RestClient(_url);
            var request = new RestRequest("posts", Method.Post);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(jsonFormat);

        }
    }
}
