using System.Xml.Linq;

namespace TestProjectLinq
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestGetAllUsers()
        {
            // Arrange
            string filePath = @"C:\Users\Liza\source\repos\practice_04\Linq\bin\Debug\net8.0\users.xml";
            XElement usersXml = XElement.Load(filePath);
            // Act
            var allUsers = from user in usersXml.Elements("User")
                           select new
                           {
                               Id = (int)user.Element("Id"),
                               Name = (string)user.Element("Name"),
                               Category = (string)user.Element("Category")
                           };

            // Assert
            Assert.IsNotNull(allUsers);
            Assert.AreEqual(2, allUsers.Count()); // we have 2 users
        }

        [TestMethod]
        public void TestGetUsersWithCategoryAdmin()
        {
            
            var filePath = @"C:\Users\Liza\source\repos\practice_04\Linq\bin\Debug\net8.0\users.xml";
            XElement usersXml = XElement.Load(filePath);

            // users with the category "Admin"
            var adminUsers = from user in usersXml.Elements("User")
                             where (string)user.Element("Category") == "Admin"
                             select user;

           
            Assert.IsNotNull(adminUsers);
            Assert.AreEqual(1, adminUsers.Count()); //  there is 1 user with the category "Admin"
        }

        [TestMethod]
        public void TestTotalConnectionTimePerUser()
        {
          
            var filePath = @"C:\Users\Liza\source\repos\practice_04\Linq\bin\Debug\net8.0\users.xml";
            XElement usersXml = XElement.Load(filePath);

            // the total connection time for each user
            var totalConnectionTimePerUser = from user in usersXml.Elements("User")
                                             let totalConnectionTime = user.Elements("Applications")
                                                                           .Elements("Application")
                                                                           .Elements("Connections")
                                                                           .Elements("Connection")
                                                                           .Sum(conn => (int)conn.Element("Time"))
                                             select new
                                             {
                                                 UserName = (string)user.Element("Name"),
                                                 TotalTime = totalConnectionTime
                                             };

            Assert.IsNotNull(totalConnectionTimePerUser);

            
            var expectedTotalTimes = new[] { (UserName: "User1", TotalTime: 20), (UserName: "User2", TotalTime: 25) };

            foreach (var user in totalConnectionTimePerUser)
            {
                var expectedTotalTime = expectedTotalTimes.FirstOrDefault(u => u.UserName == user.UserName)?.TotalTime;
                Assert.AreEqual(expectedTotalTime, user.TotalTime);
            }
        }


    }
}
