using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using MoqUnitTest;
using System.Collections.Generic;
using MoqUnitTest.Model;
using MoqUnitTest.Business;
using System.Collections;

namespace UserManagementTest
{
    /// <summary>
    /// Simple unit test example with Moq framework. 
    /// @author Roman Pusec
    /// </summary>
    [TestClass]
    public class UMTest
    {
        private IUserManagement userManagement;

        public UMTest() 
        {
            //defining user list
            IList<User> allUsers = new List<User>
            {
                new User(1, "joe", "doe", "addr1", UserPrivileges.GUEST),
                new User(2, "peter", "griffin", "addr2", UserPrivileges.MODERATOR),
                new User(3, "stewie", "griffin", "addr2", UserPrivileges.MODERATOR),
                new User(4, "joe", "marsh", "addr3", UserPrivileges.ADMIN)
            };

            Mock<IUserManagement> mockUserManagement = new Mock<IUserManagement>();

            //FindAll returns all of the users
            mockUserManagement.Setup(x => x.FindAll()).Returns(allUsers);

            /**
             * FindByName returns a list of user by their name and surname. 
             * If a null value was used for the name parameter, then the method
             * will search for users only by their surname (the same thing would happen if a null 
             * value was used for the surname parameter). 
             * If both parameters have a null value, then all users will be returned. 
             */
            mockUserManagement.Setup(x => x.FindByName(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string name, string surname) => 
                    {
                        return allUsers.Where(y =>
                                (name != null ? (y.Name == name) : true) && 
                                (surname != null ? (y.Surname == surname) : true)).ToList<User>();
                    });

            //FindById returns a single user by their id value
            mockUserManagement.Setup(x => x.FindById(It.IsAny<int>()))
                .Returns((int i) => 
                    {
                        return ReturnNullIfEntityNotFount(
                            allUsers.Where(u => u.Id == i));
                    });

            //returns all users by a particular privilege
            mockUserManagement.Setup(x => x.FindByPrivilege(It.IsAny<UserPrivileges>()))
                .Returns((UserPrivileges up) => allUsers.Where(u => u.Privilege == up).ToList<User>());

            mockUserManagement.Setup(x => x.SaveUser(It.IsAny<User>()))
                .Returns(
                    (User targetUser) => {

                        //if its id has a value of null, then update the user's information
                        if (targetUser.Id != null)
                        {
                            //searching for user with specified id and updating their information
                            foreach(User u in allUsers)
                            {
                                if (u.Id.Equals(targetUser.Id))
                                {
                                    u.Name = targetUser.Name;
                                    u.Surname = targetUser.Surname;
                                    u.Privilege = targetUser.Privilege;
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            targetUser.Id = allUsers.Count + 1;
                            allUsers.Add(targetUser);
                            return true;
                        }

                        return false;
                    }
                );

            //checks if the user has a specified privilege
            mockUserManagement.Setup(x => x.CheckPrivilege(It.IsAny<UserPrivileges>(), It.IsAny<User>()))
                .Returns((UserPrivileges privilege, User user) => user.Privilege == privilege ? true : false);

            userManagement = mockUserManagement.Object;
        }

        [TestMethod]
        public void TestFindAllUsersAndAddingUsers()
        {
            //check if there are exactly 4 users
            Assert.AreEqual(4, userManagement.FindAll().Count, "The expected amount of users matches the current amount of users. ");

            //adding new user to the list
            userManagement.SaveUser(new User(null, "NewJoe", "NewDoe", "123 Fakestreet", UserPrivileges.GUEST));

            //check if there are more users
            Assert.AreEqual(5, userManagement.FindAll().Count, "The user count was not updated. ");
        }

        [TestMethod]
        public void TestEditUser() 
        { 
            //getting a random user
            var allUsers = userManagement.FindAll();
            var randUserIndex = new Random().Next(allUsers.Count - 1);
            var prevUserName = allUsers[randUserIndex].Name;
            int? randUserId = allUsers[randUserIndex].Id;

            var newUserWithAnothersUsersId = new User(randUserId, "Barbara", "Smith", "SomeAddress", UserPrivileges.ADMIN);

            //should replace the user who's ID we injected in our new user 
            userManagement.SaveUser(newUserWithAnothersUsersId);

            //check if the amount of users is still the same
            Assert.AreEqual(4, userManagement.FindAll().Count, "The user count should've stayed the same after the new user (that has the same id as a random user) was added. ");

            //check if at least the user's name had been updated, should 
            //not be equal to the user's previous name
            Assert.AreNotEqual(prevUserName, userManagement.FindAll()[randUserIndex].Name, "The user's name was not edited. ");

            //should return false since a user with id 800 does not exist, therefore it cannot add it
            //nor can it find a user with the said id value 
            Assert.IsFalse(userManagement.SaveUser(new User(800, "arya", "stark", "addr", UserPrivileges.MODERATOR)));
        }

        [TestMethod]
        public void TestFindById() 
        {
            //check if it won't find any user when choosing 
            //an id that does not belong to any user
            //should return null if a user was not found
            Assert.IsNull(userManagement.FindById(800));

            //this one should exist
            Assert.IsNotNull(userManagement.FindById(1));
        }

        [TestMethod]
        public void TestFindByPrivilege() 
        {
            //finds users by privilege
            Assert.AreEqual(2, userManagement.FindByPrivilege(UserPrivileges.MODERATOR).Count, "Failed to find two moderators. ");
        }

        [TestMethod]
        public void TestCheckUserPrivilege() 
        {
            //checks if the user by specified id is a GUEST
            Assert.IsTrue(userManagement
                .CheckPrivilege(UserPrivileges.GUEST, userManagement.FindById(1)));

            //change privilege of the same user to ADMIN
            var sameUser = userManagement.FindById(1);
            sameUser.Privilege = UserPrivileges.ADMIN;
            userManagement.SaveUser(sameUser);

            //checks if the user privilege was changed
            Assert.IsTrue(userManagement
                .CheckPrivilege(UserPrivileges.ADMIN, userManagement.FindById(1)));
        }

        [TestMethod]
        public void TestFindByName() 
        {
            //check for one record
            Assert.AreEqual(1, userManagement.FindByName("peter", "griffin").Count, "Peter Griffin is not a unique record. ");

            //check for griffins
            Assert.AreEqual(2, userManagement.FindByName(null, "griffin").Count, "Should return two griffin users. ");

            //check for griffins
            Assert.AreEqual(2, userManagement.FindByName("joe", null).Count, "Should return two users named Joe. ");

            //get all users
            Assert.AreEqual(4, userManagement.FindByName(null, null).Count, "Should return all users. ");

            //return 0 users
            Assert.AreEqual(0, userManagement.FindByName("noname", "nosurname").Count, "Should return zero users. ");
        }

        /// <summary>
        /// Returns null if an entity was not found using Linq's Where() method. 
        /// </summary>
        /// <param name="result">The result of using Where() method (collection of entites). </param>
        /// <returns>Null if entity was not found, otherwise it would return the appropriate entity. </returns>
        private User ReturnNullIfEntityNotFount(IEnumerable<User> result) 
        {
            //return null if nothing was found
            if (result.ToList<User>().Count == 0)
                return null;
            else
                return result.Single();
        }
    }
}
