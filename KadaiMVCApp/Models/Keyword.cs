using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Collections;

namespace KadaiMVCApp.Models
{
    [Serializable]
    public class Keyword
    {
        public string PostCode { get; set; }
        public string Prefecture { get; set; }
        public string City { get; set; }
        public string ShipToAddress { get; set; }

    }
}
