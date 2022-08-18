
namespace BaranMasterDataService.Server
{
    public class ResponseFromServer
    {
        public int Type { get; set; }
        public object FunctionCode { get; set; }
        public string MessageClass { get; set; }
        public string MessageCode { get; set; }
        public string Definition { get; set; }
        public string DocNo { get; set; }
        public object DocItemNo { get; set; }
        public object EdiMessageType { get; set; }
        public object EdiMessageDefinition { get; set; }
    }
}
