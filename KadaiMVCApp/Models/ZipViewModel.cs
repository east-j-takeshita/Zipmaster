using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Collections;

namespace KadaiMVCApp.Models
{
    [Serializable]
    public class ZipViewModel
    {
        public List<Zip> ZipsData { get; set; }

        //public Message Message { get; set; }
        public Keyword Keyword { get; set; }
    }
}
