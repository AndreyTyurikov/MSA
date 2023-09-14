using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UserMS.DTO;

namespace UserMS.Client
{
    public interface IUserMsClient
    {
        public Task<UserDTO> GetUserByID(long id);
        public Task<UserDTO> CreateUser(AddUserDTO addUserDTO);
    }
}
