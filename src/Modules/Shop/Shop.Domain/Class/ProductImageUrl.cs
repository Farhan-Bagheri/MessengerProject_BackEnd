using ShareMicroservice.Domain.Class;

namespace Shop.Domain.Class;

public class ProductImageUrl : ImageUrl
{
    public bool IsMain { get; set; }
}
