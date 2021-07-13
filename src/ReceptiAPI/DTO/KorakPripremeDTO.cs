using Newtonsoft.Json;
using System;

namespace ReceptiAPI.DTO
{
    public class KorakPripremeDTO
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "idRecepta")]
        public string IdRecepta { get; set; }
        [JsonProperty(PropertyName = "redniBroj")]
        public uint RedniBroj { get; set; }
        [JsonProperty(PropertyName = "opis")]
        public string Opis { get; set; }
        [JsonProperty(PropertyName = "savet")]
        public string Savet { get; set; }
        [JsonProperty(PropertyName = "datumKreiranja")]
        public DateTime DatumKreiranja { get; set; }
        [JsonProperty(PropertyName = "datumAzuriranja")]
        public DateTime DatumAzuriranja { get; set; }
    }
}
