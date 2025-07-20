using System.Text.Json.Serialization;

public class BillingAddress
{
    [JsonPropertyName("address1")]
    public string? Address1 { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("province")]
    public string? Province { get; set; }

    [JsonPropertyName("phone")]
    public string? Phone { get; set; }
}
