using BaranMasterDataService.Database;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text.Json;



namespace BaranMasterDataService.Server
{
    public class ServerCommands
    {
        private ServerPath _serverPath;
        private BaranMasterDataDBPath _baranMasterDataDBPath;
        private const int CORRECT = 4;    
        private const int INCORRECT = 0;  
        public ServerCommands(ServerPath serverPath, BaranMasterDataDBPath baranMasterDataDBPath)
        {
            _serverPath= serverPath;
            _baranMasterDataDBPath= baranMasterDataDBPath;
        }
        private ResponseFromServer[] deSerialize(string responseFromServer)
        {
            ResponseFromServer[] result = JsonSerializer.Deserialize<ResponseFromServer[]>(responseFromServer);   
            return result;

        }

        private string serialize(List <JsonToPost.Root> listRoot)
        {
            string roots = "";
            foreach (var item in listRoot)
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonFormat = JsonSerializer.Serialize(item, options);
                roots += jsonFormat;
            }
          
            return roots;
        }
        public string postToServer(List<CNMaterials>cnMaterialsList)
        {
            DatabaseCommands databaseCommands = new DatabaseCommands( _baranMasterDataDBPath);
            RestResponse response = new RestResponse();
            try
            {
                string jsonFormat = serialize(createJsonTemplate(cnMaterialsList));
                var client = new RestClient(_serverPath.Path);
                var request = new RestRequest("uploadMaterials", Method.Post);
                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(jsonFormat);
                response = client.Execute(request);
                
            }
            
            catch (Exception ex)
            {
                string error = "Error occurred while transferring data to server";
                foreach (var item in cnMaterialsList)
                {           
                    databaseCommands.insertToTransactionLog(item,INCORRECT);
                    databaseCommands.insertToErrorLog(item,error,ex);
                }
            }

            int isCorrect = deSerialize(response.Content)[0].Type; 
            if (isCorrect==CORRECT)
            {              
                databaseCommands.updateLogs(cnMaterialsList,isCorrect);             
            }
            else if (isCorrect == INCORRECT)
            {
                CNMaterials cnMaterials = new CNMaterials();
                
                try
                {
                    foreach (var item in cnMaterialsList)
                    {
                        cnMaterials = item;
                        databaseCommands.insertToTransactionLog(item, isCorrect);
                        string error = "Response failed";
                        databaseCommands.insertToErrorLog(item, error);
                    }

                }
                catch (Exception ex)
                {
                    string error = "Error occured while proccesing response failed operation";
                    databaseCommands.insertToTransactionLog(cnMaterials,INCORRECT);
                    databaseCommands.insertToErrorLog(cnMaterials,error,ex);

                }
            }

            else
            {
                foreach (var item in cnMaterialsList)
                {
                    string error = "The server respond (type) is not found";
                    databaseCommands.insertToErrorLog(item,error);

                }
            }
            return response.Content;
        }

        public List<JsonToPost.Root> createJsonTemplate(List<CNMaterials> cnMaterials)
        {
            List<JsonToPost.Root> rootList = new List<JsonToPost.Root>();

            foreach (var item in cnMaterials)
            {
                JsonToPost.Root root = new JsonToPost.Root();
                JsonToPost.Material material = new JsonToPost.Material();
                JsonToPost.Unit unit = new JsonToPost.Unit();
                
                List<JsonToPost.Root> listRoot = new List<JsonToPost.Root>();
                List<JsonToPost.Material> listMaterial = new List<JsonToPost.Material>();
                List<JsonToPost.Unit> listUnit = new List<JsonToPost.Unit>();

                root.PlantCode = "";
                root.DebugMode = "";

                material.material = item.Material;
                material.definition = item.SText;
                material.main_unit = "AD";
                material.expiration_period = "";
                material.expiration_unit = "DAY";
                material.p_batch_tracking = "false";
                material.text_mg01 = "";
                material.text_mg02 = "";
                material.ean_code = "";
                material.manufacturer_code = "";
                material.legacy_code = "";
                material.ex_code = "";
                material.code_mg01 = "";
                material.code_mg02 = "";
                listMaterial.Add(material);

                unit.material = item.Material;
                unit.operation_unit = item.QUnit;
                unit.barcode = item.Material + item.QUnit;
                unit.factor = "1";
                unit.divisor = "1";
                unit.p_material_unit = "true";
                unit.length = "0";
                unit.width = "0";
                unit.height = "0";
                unit.area = "0";
                unit.volume = "0";
                unit.net_weight = "0";
                unit.gross_weight = "0";
                listUnit.Add(unit);

                material.Units = listUnit;
                root.materials = listMaterial;

                rootList.Add(root);
            }           
            return rootList;
        }
    }
}
