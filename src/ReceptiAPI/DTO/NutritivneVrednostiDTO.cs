using Newtonsoft.Json;

namespace ReceptiAPI.DTO
{
    public class NutritivneVrednostiDTO
    {
        [JsonProperty(PropertyName = "id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "idNamirnice", NullValueHandling = NullValueHandling.Ignore)]
        public string IdNamirnice { get; set; }
        [JsonProperty(PropertyName = "kalorije")]
        public decimal Kalorije { get; set; }
        [JsonProperty(PropertyName = "proteini")]
        public decimal Proteini { get; set; }
        [JsonProperty(PropertyName = "masti")]
        public decimal Masti { get; set; }
        [JsonProperty(PropertyName = "zasiceneMasti")]
        public decimal ZasiceneMasti { get; set; }
        [JsonProperty(PropertyName = "seceri")]
        public decimal Seceri { get; set; }
        [JsonProperty(PropertyName = "vlakna")]
        public decimal Vlakna { get; set; }
    }
}
