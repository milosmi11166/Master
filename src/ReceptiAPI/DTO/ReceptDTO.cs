using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ReceptiAPI.DTO
{
    public class ReceptDTO
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "naziv")]
        public string Naziv { get; set; }
        [JsonProperty(PropertyName = "opis")]
        public string Opis { get; set; }
        [JsonProperty(PropertyName = "datumKreiranja")]
        public DateTime DatumKreiranja { get; set; }
        [JsonProperty(PropertyName = "datumAzuriranja")]
        public DateTime DatumAzuriranja { get; set; }
        [JsonProperty(PropertyName = "koraciPripreme")]
        public List<KorakPripremeDTO> KoraciPripreme;
        [JsonProperty(PropertyName = "sastojci")]
        public List<SastojakDTO> Sastojci { get; set; }
        [JsonProperty(PropertyName = "nutritivneVrednosti")]
        public NutritivneVrednostiDTO NutritivneVrednosti { get; set; }
    }
}
