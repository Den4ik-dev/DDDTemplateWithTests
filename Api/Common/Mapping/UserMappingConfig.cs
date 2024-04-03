using Contracts.Users;
using Domain.User;
using Mapster;

namespace Api.Common.Mapping;

public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<User, UserProfileDto>()
            .Map(dest => dest.Login, src => src.Login)
            .Map(dest => dest.Role, src => src.Role.Value);
    }
}
