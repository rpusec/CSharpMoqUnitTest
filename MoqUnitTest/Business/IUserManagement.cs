using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqUnitTest.Business
{
    public interface IUserManagement
    {
        IList<Model.User> FindAll();
        IList<Model.User> FindByName(string name, string surname);
        Model.User FindById(int id);
        IList<Model.User> FindByPrivilege(Model.UserPrivileges privilege);
        bool SaveUser(Model.User newUser);
        bool CheckPrivilege(Model.UserPrivileges privilege, Model.User targetUser);
    }
}
