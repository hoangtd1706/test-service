using System.Collections.Generic;
using Ecoba.IdentityService.Data.Model;
using System.Collections;

namespace Ecoba.IdentityService.Services.UserService;

public interface IUserService
{
    Task<IEnumerable<User>> GetAll();
    bool CheckAdmin();
}