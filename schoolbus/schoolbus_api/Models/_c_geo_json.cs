using System.Text.Json.Serialization;

namespace schoolbus_api.Models
{
    public class _c_geo_json
    {
        [JsonPropertyName("type")]
        public string g_typ { get; set; } = "Feature";
        [JsonPropertyName("geometry")]
        public _c_geo_point g_geo { get; set; } = new _c_geo_point();
    }

    public class _c_geo_point
    {
        [JsonPropertyName("type")]
        public string g_typ { get; set; } = "Point";
        [JsonPropertyName("coordinates")]
        public double[] g_crd { get; set; } = { 30.0798637, 31.3588937 };
    }
}