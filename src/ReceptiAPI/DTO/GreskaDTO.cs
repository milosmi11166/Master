using Newtonsoft.Json;

namespace ReceptiAPI.DTO
{
    public class GreskaDTO
    {
        [JsonProperty(PropertyName = "porukaGreske")]
        public string PorukaGreske { get; set; }
    }
}
