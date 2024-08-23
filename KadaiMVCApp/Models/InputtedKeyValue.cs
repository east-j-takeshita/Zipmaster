using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Collections;

namespace KadaiMVCApp.Models
{
    [Serializable]
    public class InputtedKeyValue
    {
        public string? InputtedPostCode { get; set; }
        public string? InputtedKeyWord { get; set; }

    }
}
