﻿using Newtonsoft.Json;
using System;

namespace ReceptiAPI.DTO
{
    public class SastojakDTO
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "idRecepta")]
        public string IdRecepta { get; set; }
        [JsonProperty(PropertyName = "idNamirnice")]
        public string IdNamirnice { get; set; }
        [JsonProperty(PropertyName = "kolicina")]
        public uint Kolicina { get; set; }
        [JsonProperty(PropertyName = "jedinicaMere")]
        public string JedinicaMere { get; set; }
        [JsonProperty(PropertyName = "kolicinaUGramima")]
        public uint KolicinaUGramima { get; set; }
        [JsonProperty(PropertyName = "datumKreiranja")]
        public DateTime DatumKreiranja { get; set; }
        [JsonProperty(PropertyName = "datumAzuriranja")]
        public DateTime DatumAzuriranja { get; set; }
    }
}
