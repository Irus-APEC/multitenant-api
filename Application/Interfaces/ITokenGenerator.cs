using Domain.Entities;

namespace Application.Interfaces;

public interface ITokenGenerator
{
    string CreateToken(User user);
}