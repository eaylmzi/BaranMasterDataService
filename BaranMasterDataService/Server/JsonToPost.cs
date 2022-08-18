
using System.Collections.Generic;


namespace BaranMasterDataService.Server
{
    public class JsonToPost
    {
        public class Material
        {
            public string material { get; set; }
            public string definition { get; set; }
            public string main_unit { get; set; }
            public string expiration_period { get; set; }
            public string expiration_unit { get; set; }
            public string p_batch_tracking { get; set; }
            public string text_mg01 { get; set; }
            public string text_mg02 { get; set; }
            public string ean_code { get; set; }
            public string manufacturer_code { get; set; }
            public string legacy_code { get; set; }
            public string ex_code { get; set; }
            public string code_mg01 { get; set; }
            public string code_mg02 { get; set; }
            public List<Unit> Units { get; set; }
        }

        public class Root
        {
            public string PlantCode { get; set; }
            public string DebugMode { get; set; }
            public List<Material> materials { get; set; }
        }

        public class Unit
        {
            public string material { get; set; }
            public string operation_unit { get; set; }
            public string barcode { get; set; }
            public string factor { get; set; }
            public string divisor { get; set; }
            public string p_material_unit { get; set; }
            public string length { get; set; }
            public string width { get; set; }
            public string height { get; set; }
            public string area { get; set; }
            public string volume { get; set; }
            public string net_weight { get; set; }
            public string gross_weight { get; set; }
        }

    }

}
