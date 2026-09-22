namespace Shop.Application.Dto.Response.Category;

public class GetCategoryListByParentIdDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public int StatusTypeId { get; set; }
    public string StatusTypeText { get; set; }
}
