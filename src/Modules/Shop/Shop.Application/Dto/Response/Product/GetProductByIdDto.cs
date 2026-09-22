using Shop.Domain.Class;

namespace Shop.Application.Dto.Response.Product;

public class GetProductByIdDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string CreatedAt { get; set; }
    public string UpdatedAt { get; set; }
    public List<ProductImageUrl> Images { get; set; } = [];
}
