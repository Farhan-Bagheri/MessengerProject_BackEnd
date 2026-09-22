namespace Shop.Application.Dto.Request.Category;

public class RequestUpdateCategoryByIdDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public int StatusTypeId { get; set; }
    public long ParentId { get; set; }
}
