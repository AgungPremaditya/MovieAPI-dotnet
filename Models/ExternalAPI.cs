
using System.Text.Json.Serialization;

namespace MovieAPI_dotnet.Models
{
    public class MediaType
    {
        [JsonPropertyName("name")]
        public required string name { get; set; }
        [JsonPropertyName("route")]
        public required string route { get; set; }
    }
    public class StreamPlatform
    {
        [JsonPropertyName("youtube")]
        public string? youtube { get; set; }
        [JsonPropertyName("crunchyroll")]
        public string? crunchyroll { get; set; }
        [JsonPropertyName("hulu")]
        public string? hulu { get; set; }
        [JsonPropertyName("amazon")]
        public string? amazon { get; set; }
        [JsonPropertyName("funimation")]
        public string? funimation { get; set; }
        [JsonPropertyName("netflix")]
        public string? netflix { get; set; }
        [JsonPropertyName("hidive")]
        public string? hidive { get; set; }
    }
    public class ExternalAPI
    {
        [JsonPropertyName("title")]
        public required string title { get; set; }
        [JsonPropertyName("route")]
        public required string route { get; set; }
        [JsonPropertyName("romaji")]
        public required string romaji { get; set; }
        [JsonPropertyName("english")]
        public required string english { get; set; }
        [JsonPropertyName("native")]
        public required string native { get; set; }
        [JsonPropertyName("delayedFrom")]
        public required string delayedFrom { get; set; }
        [JsonPropertyName("delayedUntil")]
        public required string delayedUntil { get; set; }
        [JsonPropertyName("status")]
        public required string status { get; set; }
        [JsonPropertyName("episodeDate")]
        public required string episodeDate { get; set; }
        [JsonPropertyName("episodeNumber")]
        public required int episodeNumber { get; set; }
        [JsonPropertyName("episodes")]
        public int? episodes { get; set; }
        [JsonPropertyName("lengthMin")]
        public required int lengthMin { get; set; }
        [JsonPropertyName("donghua")]
        public required bool donghua { get; set; }
        [JsonPropertyName("airType")]
        public required string airType { get; set; }
        [JsonPropertyName("mediaTypes")]
        public required List<MediaType> mediaTypes { get; set; }
        [JsonPropertyName("imageVersionRoute")]
        public required string imageVersionRoute { get; set; }
        [JsonPropertyName("streams")]
        public StreamPlatform? streams { get; set; }
        [JsonPropertyName("airingStatus")]
        public required string airingStatus { get; set; }

    }
}
