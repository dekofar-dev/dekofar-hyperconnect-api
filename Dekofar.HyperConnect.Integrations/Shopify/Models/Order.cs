using Newtonsoft.Json;

public class Order
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("order_number")]
    public string OrderNumber { get; set; }

    [JsonProperty("created_at")]
    public string CreatedAt { get; set; }

    [JsonProperty("total_price")]
    public string TotalPrice { get; set; }

    [JsonProperty("currency")]
    public string Currency { get; set; }

    [JsonProperty("financial_status")]
    public string FinancialStatus { get; set; }

    [JsonProperty("fulfillment_status")]
    public string FulfillmentStatus { get; set; }

    [JsonProperty("customer")]
    public Customer? Customer { get; set; }
}
