using System;

namespace platychat_dotnet.DTOs;

public class UserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public List<string> Chats { get; set; } = new();
}
