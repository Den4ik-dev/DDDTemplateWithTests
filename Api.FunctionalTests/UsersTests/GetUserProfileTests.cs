using System.Net.Http.Headers;
using System.Net.Http.Json;
using Api.FunctionalTests.Common;
using Contracts.Authentication;
using Contracts.Users;
using Domain.User;
using FluentAssertions;
using Infrastructure.Authentication;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Api.FunctionalTests.UsersTests;

public class GetUserProfileTests : BaseFunctionalTest
{
    public GetUserProfileTests(FunctionalWebApplicationFactory factory)
        : base(factory) { }

    [Fact]
    public async Task Successfully_get_user_profile()
    {
        // Arrange
        using (ApplicationDbContext context = CreateDbContext())
        {
            User user = User.Create("john", "john12345");

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

        var loginDto = new LoginDto("john", "john12345");

        HttpResponseMessage responseWithToken = await Client.PostAsJsonAsync(
            "api/auth/login",
            loginDto
        );

        responseWithToken.EnsureSuccessStatusCode();

        Token token = (await responseWithToken.Content.ReadFromJsonAsync<Token>())!;

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            JwtBearerDefaults.AuthenticationScheme,
            token.AccessToken
        );

        // Act
        HttpResponseMessage responseWithResult = await Client.GetAsync("api/users");

        // Assert
        responseWithResult.EnsureSuccessStatusCode();

        UserProfileDto userProfile = (
            await responseWithResult.Content.ReadFromJsonAsync<UserProfileDto>()
        )!;

        userProfile.Login.Should().Be("john");
        userProfile.Role.Should().Be("standard_user");
    }
}
