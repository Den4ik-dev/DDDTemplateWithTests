using FluentValidation;

namespace Application.Users.Queries.GetUserProfile;

public class GetUserProfileQueryValidator : AbstractValidator<GetUserProfileQuery>
{
    public GetUserProfileQueryValidator()
    {
        RuleFor(query => query.UserId).NotNull().NotEmpty();
    }
}
