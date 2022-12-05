using System.Text.Json.Serialization;

namespace ConsoleApp.Models;

internal abstract class TermsAbstraction<T>
{
    [JsonPropertyName("tanks")]
    public T Tanks { get; set; }

    [JsonPropertyName("apv")]
    public T APV { get; set; }

    [JsonPropertyName("artillery")]
    public T Artillery { get; set; }

    [JsonPropertyName("mlrs")]
    public T MLRS { get; set; }

    [JsonPropertyName("aaws")]
    public T AAWS { get; set; }

    [JsonPropertyName("aircraft")]
    public T Aircraft { get; set; }

    [JsonPropertyName("helicopters")]
    public T Helicopters { get; set; }

    [JsonPropertyName("uav")]
    public T UAV { get; set; }

    [JsonPropertyName("vehicles")]
    public T Vehicles { get; set; }

    [JsonPropertyName("boats")]
    public T Boats { get; set; }

    [JsonPropertyName("se")]
    public T SE { get; set; }

    [JsonPropertyName("missiles")]
    public T Missiles { get; set; }

    [JsonPropertyName("personnel")]
    public T Personnel { get; set; }

    [JsonPropertyName("captive")]
    public T Captive { get; set; }
}
