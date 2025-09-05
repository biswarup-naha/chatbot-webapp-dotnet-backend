using System;
using System.Text.Json.Serialization;

namespace platychat_dotnet.Utils;

public class ApiResponse<T>
{
    public bool Success { get; set; } = false;
    public string Message { get; set; } = "Somehting went wrong";

    public T? Result { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Token { get; set; }
}
