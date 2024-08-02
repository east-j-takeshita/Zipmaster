using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Collections;

namespace KadaiMVCApp.Models
{
    [Serializable]
    public class Keyword
    {
        public string? KeyPostCode { get; set; }
        public string? KeyWord { get; set; }

    }
}
