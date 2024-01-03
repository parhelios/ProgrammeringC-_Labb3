using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Labb3ProgTemplate.DataModels.Users;
using Labb3ProgTemplate.Enums;

namespace Labb3ProgTemplate.Managerrs;

public static class UserManager
{
    private static readonly IEnumerable<User>? _users = new List<User>();

    private static User _currentUser;

    public static IEnumerable<User>? Users
    {
        get { return _users; }
        set
        {
            UserListChanged.Invoke();
        }
    }

    public static User CurrentUser
    {
        get => _currentUser;
        set
        {
            _currentUser = value;
            CurrentUserChanged?.Invoke();
        }
    }

    public static event Action CurrentUserChanged;

    // Skicka detta efter att användarlistan ändrats eller lästs in
    public static event Action UserListChanged;

    public static bool IsAdminLoggedIn
    {
        get
        {
            if (CurrentUser is null)
            {
                return false;
            }
            return CurrentUser.Type is UserTypes.Admin;
        }
    }

    public static bool IsCustomerLoggedIn
    {
        get
        {
            if (CurrentUser is null)
            {
                return false;
            }

            return CurrentUser.Type is UserTypes.Customer;
        }
    }
    public static void ChangeCurrentUser(string name, string password, UserTypes type)
    {
        foreach (var user in Users)
        {
            if (user.Name == name && user.Password == password)
            {
                CurrentUser = user;

                if (type is UserTypes.Customer)
                {
                    MessageBox.Show($"You're logged in as a customer.");
                    break;
                }

                if (type is UserTypes.Admin)
                {
                    MessageBox.Show($"You're logged in as an admin.");
                    break;
                }
            }
        }
    }
    public static void AddUser(User user)
    {
        //Lägga till enskilda användare.

        ((List<User>)Users).Add(user);
    }

    public static void AddBulkUsers(List<User> users)
    {
        //Lägga till flera användare.

        if (Users is List<User> userList)
        {
            userList.AddRange(users);
        }
    }

    public static void LogOut()
    {
        //Nollställer amount i samtliga produkter och loggar sedan ut.

        foreach (var product in ProductManager.Products)
        {
            product.Amount = 0;
        }

        MessageBox.Show("Logged out");
        CurrentUser = null;
    }

    public static async Task SaveUsersToFile()
    {
        var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "parheliosLabb3");
        Directory.CreateDirectory(directory);
        var filepath = Path.Combine(directory, "users.json");

        var jsonOptions = new JsonSerializerOptions();
        jsonOptions.WriteIndented = true;

        if (File.Exists(filepath))
        {
            if (Users.Count() < 1)
            {
                return;
            }
            var json = JsonSerializer.Serialize(Users, jsonOptions);

            using var sw = new StreamWriter(filepath, false);
            sw.WriteLine(json);
        }

        if (!File.Exists(filepath))
        {
            var json = JsonSerializer.Serialize(Users, jsonOptions);

            using var sw = new StreamWriter(filepath);
            sw.WriteLine(json);
        }
    }

    public static async Task LoadUsersFromFile()
    {
        var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "parheliosLabb3");
        var filepath = Path.Combine(directory, "users.json");

        if (File.Exists(filepath))
        {
            using var sr = new StreamReader(filepath);
            var text = sr.ReadToEnd();

            var deserialisedUsers = new List<User>();
            using (var jsonDoc = JsonDocument.Parse(text))
            {
                if (jsonDoc.RootElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var jsonElement in jsonDoc.RootElement.EnumerateArray())
                    {
                        User a;
                        switch (jsonElement.GetProperty("Type").GetInt32())
                        {
                            case 0:
                                a = jsonElement.Deserialize<Admin>();
                                deserialisedUsers.Add(a);
                                break;
                            case 1:
                                a = jsonElement.Deserialize<Customer>();
                                deserialisedUsers.Add(a);
                                break;
                        }
                    }
                    AddBulkUsers(deserialisedUsers);
                }
            }
        }
    }
}