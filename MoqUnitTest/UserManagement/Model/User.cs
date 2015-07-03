using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqUnitTest.Model
{
    public enum UserPrivileges {
        GUEST, MODERATOR, ADMIN
    }

    public class User
    {
        public User(int? _id, string _name, string _surname, string _address, UserPrivileges _privilege) 
        {
            Id = _id;
            Name = _name;
            Surname = _surname;
            Address = _address;
            Privilege = _privilege;
        }

        //properties
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public UserPrivileges Privilege { get; set; }
    }
}
