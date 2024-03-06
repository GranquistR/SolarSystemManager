using Microsoft.AspNetCore.Identity.Data;
using SolarSystemManager.RESTAPI.Entities;
using System.Linq;
using System.Text.Json;
using SolarSystemManager.RESTAPI.Repos;
using static SolarSystemManager.RESTAPI.Services.UserService;

namespace SolarSystemManager.RESTAPI.Services
{
    public class UserService
    {
        /// <summary>
        /// This validates the user
        /// Will be replaced with actual login logic
        /// WIll be used to validate the user on every endpoint that requires authentication
        /// </summary>
        /// <param name="cred"></param>
        /// <returns></returns>
        ///         
        BaseRepo _baseRepo = BaseRepo.Instance();
        public User? ValidateUser(Entities.LoginRequest cred)
        {
            return _baseRepo.GetAllUsers().Find(p => (p.username == cred.username) && (p.password == cred.password));           
        }
        public User GetUserSettingsData(Entities.LoginRequest cred)
        {
            var user = _baseRepo.GetAllUsers().FirstOrDefault(p => (p.username == cred.username) && (p.password == cred.password));

            if (user != default)
            {
                return user;
            }
            else
            {
                throw new BadHttpRequestException("No user found with given credentials");
            }
        }

        public void CreateAccount(Entities.LoginRequest newAccount)
        {
            if (_baseRepo.GetAllUsers().Any(p => p.username == newAccount.username))
            {
                throw new BadHttpRequestException("Username already exists");
            }
            if (newAccount.username.Length < 1)
            {
                throw new BadHttpRequestException("Username too short!");

            }
            if (newAccount.password.Length < 1)
            {
                throw new BadHttpRequestException("Password too short!");
            }

            _baseRepo.CreateUser(new User { username = newAccount.username, password = newAccount.password, role = Role.Member });

        }

        public int UserCount()
        {
            return _baseRepo.Count("User");
        }
        public class AVLTree
        {
            public class Node
            {
                public string username;
                public string password;
                public string salt;
                public int height;
                public Node left, right;

                public Node(string username, string password, string salt)
                {
                    this.username = username;
                    this.password = password;
                    this.salt = salt;
                    height = 1;
                }
            }

            private Node root;

            // Helper function to get the height of the tree
            private int Height(Node node)
            {
                if (node == null)
                    return 0;
                return node.height;
            }

            // Helper function to get the balance factor of a node
            private int BalanceFactor(Node node)
            {
                if (node == null)
                    return 0;
                return Height(node.left) - Height(node.right);
            }

            // Helper function to perform right rotation
            private Node RightRotate(Node y)
            {
                Node x = y.left;
                Node T2 = x.right;

                // Perform rotation
                x.right = y;
                y.left = T2;

                // Update heights
                y.height = Math.Max(Height(y.left), Height(y.right)) + 1;
                x.height = Math.Max(Height(x.left), Height(x.right)) + 1;

                return x;
            }

            // Helper function to perform left rotation
            private Node LeftRotate(Node x)
            {
                Node y = x.right;
                Node T2 = y.left;

                // Perform rotation
                y.left = x;
                x.right = T2;

                // Update heights
                x.height = Math.Max(Height(x.left), Height(x.right)) + 1;
                y.height = Math.Max(Height(y.left), Height(y.right)) + 1;

                return y;
            }

            // Insert a node with given parameters into the AVL tree
            public Node Insert(Node node, string username, string password, string salt)
            {
                // Perform standard BST insertion
                if (node == null)
                    return new Node(username, password, salt);

                if (string.Compare(username, node.username) < 0)
                    node.left = Insert(node.left, username, password, salt);
                else if (string.Compare(username, node.username) > 0)
                    node.right = Insert(node.right, username, password, salt);
                else // Duplicate usernames are not allowed
                    throw new ArgumentException("Username already exists");

                // Update height of this ancestor node
                node.height = 1 + Math.Max(Height(node.left), Height(node.right));

                // Get the balance factor of this ancestor node
                int balance = BalanceFactor(node);

                // If this node becomes unbalanced, there are four cases

                // Left Left Case
                if (balance > 1 && string.Compare(username, node.left.username) < 0)
                    return RightRotate(node);

                // Right Right Case
                if (balance < -1 && string.Compare(username, node.right.username) > 0)
                    return LeftRotate(node);

                // Left Right Case
                if (balance > 1 && string.Compare(username, node.left.username) > 0)
                {
                    node.left = LeftRotate(node.left);
                    return RightRotate(node);
                }

                // Right Left Case
                if (balance < -1 && string.Compare(username, node.right.username) < 0)
                {
                    node.right = RightRotate(node.right);
                    return LeftRotate(node);
                }

                // return the (unchanged) node pointer
                return node;
            }

            // Wrapper function to insert a node into the AVL tree
            public void Insert(string username, string password, string salt)
            {
                root = Insert(root, username, password, salt);
            }
            public void PrintTree(Node node)
            {
                if (node != null)
                {
                    PrintTree(node.left);
                    Console.WriteLine($"Username: {node.username}, Password: {node.password}, Salt: {node.salt}");
                    PrintTree(node.right);
                }
            }
            public Node Search(Node node, string username)
            {
                if (node == null || node.username == username)
                    return node;

                if (string.Compare(username, node.username) < 0)
                    return Search(node.left, username);
                else
                    return Search(node.right, username);
            }

            // Wrapper function to search for a username in the AVL tree
            public Node Search(string username)
            {
                return Search(root, username);
            }
            /*
            public Node findPass(string username, string password)
            {
                AVLTree.Node result = tree.Search(username);
                if (result != null)
                {
                    if (result.password == password)
                    {
                        return result;
                    } else
                    {
                        return null;
                    }
                } else
                {
                    return null;
                }
            }
            */
            // Wrapper function to print the AVL tree
            public void PrintTree()
            {
                PrintTree(root);
            }

            // Main function to test the AVL tree
            static void tester()
            {
                AVLTree tree = new AVLTree();
                for (int i = 0; i < 100; ++i)
                {
                    tree.Insert($"user{i}", $"password{i}salt", "salt");
                }
                for (int i = 0; i < 100; ++i)
                {
                    AVLTree.Node result = tree.Search($"user{i}");
                    if (result != null)
                    {
                        Console.WriteLine(result.username + " found! Password: " + result.password);
                    }

                }
            }
            static void Main(string[] args)
            {

                tester();

                // You can continue inserting nodes or perform other operations here
            }
        }

        AVLTree tree = new AVLTree();
        List<User> users = new List<User> {
                new User
                {
                    userID = 1234,
                    username = "user",
                    password = "password"
                },

                new User
                {
                    userID = 4321,
                    username = "admin",
                    password = "e4cbf2232a1090956d07d6197acce64830392162187d2a35b419b5fc9d9c8"
                }
        };
        private readonly AVLTree _avlTree = new AVLTree();


        public string GetSalts(string username)
        {
            try
            {
                tree.Insert("admin", "8437f108d53f42d93199da22074b2b595216d2c51d2a7ccbe072bad7f3897", "adminSalt");
                //password adminPass

                AVLTree.Node result = tree.Search(username);
                if (result != null)
                {
                    return result.salt;

                }
                else
                {
                    throw new BadHttpRequestException("Invalid username or password");

                }
            }
            catch
            {
                throw new BadHttpRequestException("Invalid username or password");
            }
        }
        public bool ValidatePass(Entities.LoginRequest cred)
        {
            tree.Insert("admin", "8437f108d53f42d93199da22074b2b595216d2c51d2a7ccbe072bad7f3897", "adminSalt");

            try
            {
                AVLTree.Node result = tree.Search(cred.username);
                if (result.password == cred.password)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            } catch
            {
                return false;
            }
            }
    }

}
