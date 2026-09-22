namespace Shop.Application.Dto.Request.Category;

public class RequestCreateCategoryDto
{
    public string Name { get; set; }
    public string Slug { get; set; }
    public int StatusTypeId { get; set; }
    public long ParentId { get; set; }
}
