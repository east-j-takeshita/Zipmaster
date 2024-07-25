using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Collections;

namespace KadaiMVCApp.Models
{
    [Serializable]
    public class ZipViewModel
    {
        [Key]
        [JsonIgnore]
        public int PostOrderID { get; set; }//id
        public string GroupCode { get; set; }
        public string OldPostCode { get; set; }
        public string PostCode { get; set; }
        public string Prefecture_Kana { get; set; }
        public string City_Kana { get; set; }
        public string ShipToAddress_Kana { get; set; }
        public string Prefecture { get; set; }
        public string City { get; set; }
        public string ShipToAddress { get; set; }
        public byte SameShipToAddress { get; set; }
        public byte SubDistrictLevel { get; set; }
        public byte ChomeName { get; set; }
        public byte MultiplecityNumber { get; set; }
        public byte UpdateDate { get; set; }
        public byte UpdateReason { get; set; }

        public List<Zip> ZipsData {  get; set; }
    }
}
