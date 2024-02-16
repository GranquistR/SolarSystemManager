using Microsoft.AspNetCore.Identity.Data;
using SolarSystemManager.RESTAPI.Entities;
using System;
using System.Collections;
using System.Linq;
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
        /// using System;


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





            public bool ValidateUser(Entities.LoginRequest cred)
            {
                try
                {
                    //replace with actual login logic
                    //just need to check if the credentials are in our list of users
                    if (cred.username == "admin" && cred.password == "admin")
                    {
                        return true;
                    }
                    else
                    {
                        throw new BadHttpRequestException("Invalid username or password");
                    }
                }
                catch
                {
                    return false;
                }

            }

        }
    }
}

