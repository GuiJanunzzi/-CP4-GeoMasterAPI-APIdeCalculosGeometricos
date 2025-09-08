using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeoMaster.API.DTOs
{
    public class FormaRequestDto
    {
        // O [JsonPropertyName] garante que o nome no JSON seja mapeado corretamente.
        [JsonPropertyName("tipoForma")]
        public string TipoForma { get; set; }

        [JsonPropertyName("propriedades")]
        public JsonElement Propriedades { get; set; }
    }
}
