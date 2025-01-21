using UniversityLibrary.Api.Model;

namespace UniversityLibrary.Api.Auth.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
}