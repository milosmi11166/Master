using Newtonsoft.Json;

namespace ReceptiAPI.DTO
{
    public class NamirnicaDTO
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "naziv")]
        public string Naziv { get; set; }
        [JsonProperty(PropertyName = "opis")]
        public string Opis { get; set; }
        [JsonProperty(PropertyName = "kategorija")]
        public string Kategorija { get; set; }
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
