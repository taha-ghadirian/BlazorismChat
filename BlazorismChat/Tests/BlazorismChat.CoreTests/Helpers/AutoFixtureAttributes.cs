using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using BlazorismChat.ClientLibraries.DTOs;
using BlazorismChat.DbLayer.Entities.Users;

namespace BlazorismChat.CoreTests.Helpers;

[System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
sealed class UserAutoData : AutoDataAttribute
{
    public UserAutoData()
    {
        Fixture.Customize<User>(p => p.Without(x => x.UserRoles).Without(x => x.UserId).Without(x => x.IsDeleted));
    }
}

sealed class RegisterDTOAutoData : AutoDataAttribute
{
    public RegisterDTOAutoData()
    {
        Fixture.Customize<RegisterDTO>(p => p.With(x => x.Email, "NewEmail@gmail.com").With(x => x.Password, "Password").With(x => x.UserName, "newuser"));
    }
}