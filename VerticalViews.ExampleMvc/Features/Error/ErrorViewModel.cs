namespace VerticalViews.ExampleMvc.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; } = Guid.NewGuid().ToString();

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}

