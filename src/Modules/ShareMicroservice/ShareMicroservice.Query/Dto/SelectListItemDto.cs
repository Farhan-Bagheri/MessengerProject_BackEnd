namespace ShareMicroservice.Query.Dto;

public class SelectListItemDto
{
    public SelectListItemDto()
    {

    }

    public SelectListItemDto(object value, string text)
    {
        Value = value;
        Text = text;
    }

    public object Value { get; set; }
    public string Text { get; set; }
}
