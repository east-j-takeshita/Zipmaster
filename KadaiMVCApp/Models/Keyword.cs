using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Collections;

namespace KadaiMVCApp.Models
{
    [Serializable]
    public class Keyword
    {
        public string? PostCode { get; set; }
        public string? KeyWord { get; set; }

    }
}
