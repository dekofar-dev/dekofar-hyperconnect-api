using System.Text.Json.Serialization;

public class LineItem
{
    [JsonPropertyName("product_id")]
    public long ProductId { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("variant_title")]
    public string? VariantTitle { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
}
