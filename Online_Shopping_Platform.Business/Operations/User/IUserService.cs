using Online_Shopping_Platform.Business.Operations.User.Dtos;
using Online_Shopping_Platform.Business.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Shopping_Platform.Business.Operations.User
{
    public interface IUserService
    {
        Task<ServiceMessage> AddUser(AddUserDto user);//Async, because UnitOfWork will be used.

        ServiceMessage<UserInfoDto> LoginUser(LoginUserDto user);
    }
}
