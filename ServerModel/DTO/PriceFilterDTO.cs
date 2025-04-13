namespace ServerModel;

public class PriceFilterDTO
{
    public float MinPrice { get; set; }
    public float MaxPrice { get; set; }

    public PriceFilterDTO()
    {
        MinPrice = float.MinValue;
        MaxPrice = float.MaxValue;
    }

    public PriceFilterDTO(float minPrice, float maxPrice)
    {
        MinPrice = minPrice;
        MaxPrice = maxPrice;
    }
}