namespace Shop.Application.Dto.Response.Category;

public class GetCategoryByIdDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public int StatusTypeId { get; set; }
    public string StatusTypeText { get; set; }
    public long ParentId { get; set; }
    public string ParentName { get; set; }
    public string CreatedAt { get; set; }
    public string UpdatedAt { get; set; }
}
