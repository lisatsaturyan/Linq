using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;


class Program
{
    static void Main(string[] args)
    {
        var users = new List<User>
        {
            new User
            {
                Id = 1,
                Name = "Liza Tsaturyan",
                Category = "Admin",
                Applications = new List<Application>
                {
                    new Application
                    {
                        Name = "App1",
                        Connections = new List<Connection>
                        {
                            new Connection { Ip = "190.170.0.2", Time = 10 },
                            new Connection { Ip = "191.160.1.3", Time = 20 }
                        }
                    },
                    new Application
                    {
                        Name = "App2",
                        Connections = new List<Connection>
                        {
                            new Connection { Ip = "192.161.0.3", Time = 30 }
                        }
                    }
                }
            },
            new User
            {
                Id = 2,
                Name = "Samanta Johnes",
                Category = "User",
                Applications = new List<Application>
                {
                    new Application
                    {
                        Name = "App1",
                        Connections = new List<Connection>
                        {
                            new Connection { Ip = "220.168.0.4", Time = 50 }
                        }
                    }
                }
            }
        };

        var xml = new XElement("Users",
            from user in users
            select new XElement("User",
                new XElement("Id", user.Id),
                new XElement("Name", user.Name),
                new XElement("Category", user.Category),
                new XElement("Applications",
                    from app in user.Applications
                    select new XElement("Application",
                        new XElement("Name", app.Name),
                        new XElement("Connections",
                            from conn in app.Connections
                            select new XElement("Connection",
                                new XElement("Ip", conn.Ip),
                                new XElement("Time", conn.Time)
                            )
                        )
                    )
                )
            )
        );

        Console.WriteLine(xml);
        xml.Save("users.xml");


        //excercise2

        Console.WriteLine("------------------------------------------------------");
        XElement usersXml = XElement.Load("users.xml");

        // Query: Get All Users
        var allUsers = from user in usersXml.Elements("User")
                       select new
                       {
                           Id = (int)user.Element("Id"),
                           Name = (string)user.Element("Name"),
                           Category = (string)user.Element("Category")
                       };
        
        Console.WriteLine("All Users:");
        foreach (var user in allUsers)
        {
            Console.WriteLine($"Id: {user.Id}, Name: {user.Name}, Category: {user.Category}");
        }

        Console.WriteLine("------------------------------------------------------");
        // Query: Get Users with an "admin" Category
        var category = "Admin";
        var usersInCategory = from user in usersXml.Elements("User")
                              where (string)user.Element("Category") == category
                              select new
                              {
                                  Id = (int)user.Element("Id"),
                                  Name = (string)user.Element("Name")
                              };

        Console.WriteLine($"\nUsers in Category {category}:");
        foreach (var user in usersInCategory)
        {
            Console.WriteLine($"Id: {user.Id}, Name: {user.Name}");
        }

        Console.WriteLine("------------------------------------------------------");
        // Query: Total Connection Time for Each User
        var totalConnectionTimePerUser = from user in usersXml.Elements("User")
                                         let totalConnectionTime = (from app in user.Element("Applications").Elements("Application")
                                                                    from conn in app.Element("Connections").Elements("Connection")
                                                                    select (int)conn.Element("Time")).Sum()
                                         select new
                                         {
                                             Name = (string)user.Element("Name"),
                                             TotalTime = totalConnectionTime
                                         };

        Console.WriteLine("\nTotal Connection Time per User:");
        foreach (var user in totalConnectionTimePerUser)
        {
            Console.WriteLine($"Name: {user.Name}, TotalTime: {user.TotalTime}");
        }


    }
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public List<Application> Applications { get; set; }
}

public class Application
{
    public string Name { get; set; }
    public List<Connection> Connections { get; set; }
}

public class Connection
{
    public string Ip { get; set; }
    public int Time { get; set; }
}

