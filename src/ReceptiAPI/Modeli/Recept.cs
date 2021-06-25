using Cosmonaut;
using Cosmonaut.Attributes;
using Newtonsoft.Json;
using System;

namespace ReceptiAPI.Modeli
{
    [SharedCosmosCollection("Recepti")]
    public class Recept : ISharedCosmosEntity
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

        [JsonProperty(PropertyName = "cosmosEntityName")]
        public string CosmosEntityName { get; set; }
    }
}
