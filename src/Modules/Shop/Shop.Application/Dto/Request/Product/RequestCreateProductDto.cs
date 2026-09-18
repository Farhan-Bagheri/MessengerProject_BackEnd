namespace Shop.Application.Dto.Request.Product;

public class RequestCreateProductDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<string> Images { get; set; } = [];
}
