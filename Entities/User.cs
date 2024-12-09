using Microsoft.AspNetCore.Identity;

namespace ProjectAlfa.Entities;

public class User: IdentityUser

{
	public string? Name { get; set; }
	public string? Role { get; set; }

	public User() { }
	public User(string _name, string? _role, string password)
	{
		Name = _name;
		Role = _role;

	}
}