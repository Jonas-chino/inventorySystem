using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace InventorySystem
{
    // ---  STRUCTURES REQUIREMENT ---
    public struct Location
    {
        public string Aisle;
        public int Shelf;

        public Location(string aisle, int shelf)
        {
            Aisle = aisle;
            Shelf = shelf;
        }
    }

    // ---  CLASSES REQUIREMENT ---
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Batch { get; set; }
        public int BoxQuantity { get; set; }

        // Using the struct inside our class
        public Location StorageLocation { get; set; }
    }

    class Program
    {
        // Global variables for the class
        static List<Product> inventory = new List<Product>();
        // The file where will save our data
        static string filePath = "inventory_data.txt";

        static void Main(string[] args)
        {
            // Load existing data when the program starts
            LoadDataFromFile();

            bool isRunning = true;

            // --- 3. LOOPS REQUIREMENT ---
            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("=== WAREHOUSE INVENTORY SYSTEM ===");
                Console.WriteLine("1. Register new product");
                Console.WriteLine("2. View inventory");
                Console.WriteLine("3. Remove stock");
                Console.WriteLine("4. Save and Exit");
                Console.Write("Choose an option (1-4): ");

                string option = Console.ReadLine();

                // ---  CONDITIONALS REQUIREMENT ---
                switch (option)
                {
                    case "1":
                        AddProduct();
                        break;
                    case "2":
                        ViewInventory();
                        break;
                    case "3":
                        RemoveStock();
                        break;
                    case "4":
                        // Save data before closing
                        SaveDataToFile();
                        isRunning = false;
                        Console.WriteLine("Saving data... Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }

                if (isRunning)
                {
                    Console.WriteLine("\nPress ENTER to continue...");
                    Console.ReadLine();
                }
            }
        }

        // ---  FUNCTIONS REQUIREMENT ---

        // Function to add a product
        static void AddProduct()
        {
            Console.WriteLine("\n--- REGISTER PRODUCT ---");
            Product newProduct = new Product();

            Console.Write("Enter ID (e.g., 101): ");
            newProduct.Id = int.Parse(Console.ReadLine());

            Console.Write("Enter product name: ");
            newProduct.Name = Console.ReadLine();

            Console.Write("Enter batch number: ");
            newProduct.Batch = Console.ReadLine();

            Console.Write("Enter box quantity: ");
            newProduct.BoxQuantity = int.Parse(Console.ReadLine());

            Console.Write("Enter Storage Aisle (e.g., A): ");
            string aisle = Console.ReadLine();

            Console.Write("Enter Storage Shelf (e.g., 3): ");
            int shelf = int.Parse(Console.ReadLine());

            // Assigning the struct to the product
            newProduct.StorageLocation = new Location(aisle, shelf);

            inventory.Add(newProduct);
            Console.WriteLine("Product registered successfully!");
        }

        // Function to display the inventory
        static void ViewInventory()
        {
            Console.WriteLine("\n--- CURRENT INVENTORY ---");
            if (inventory.Count == 0)
            {
                Console.WriteLine("The warehouse is empty.");
            }
            else
            {
                foreach (Product p in inventory)
                {
                    Console.WriteLine($"ID: {p.Id} | Name: {p.Name} | Boxes: {p.BoxQuantity} | Location: Aisle {p.StorageLocation.Aisle}, Shelf {p.StorageLocation.Shelf}");
                }
            }
        }

        // Function to remove stock
        static void RemoveStock()
        {
            Console.WriteLine("\n--- REMOVE STOCK ---");
            Console.Write("Enter the ID of the product to remove: ");
            int searchId = int.Parse(Console.ReadLine());

            var product = inventory.FirstOrDefault(p => p.Id == searchId);

            if (product != null)
            {
                Console.Write($"How many boxes of {product.Name} are you removing? (Current stock: {product.BoxQuantity}): ");
                int boxesToRemove = int.Parse(Console.ReadLine());

                // --- 6. EXPRESSIONS REQUIREMENT ---
                if (boxesToRemove <= product.BoxQuantity)
                {
                    product.BoxQuantity -= boxesToRemove;
                    Console.WriteLine("Stock updated successfully.");
                }
                else
                {
                    Console.WriteLine("Error: Not enough stock in the warehouse.");
                }
            }
            else
            {
                Console.WriteLine("Error: Product not found.");
            }
        }

        // ---  READ AND WRITE TO A FILE REQUIREMENT ---

        // Function to save data to a text file
        static void SaveDataToFile()
        {
            // StreamWriter writes text to a file
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (Product p in inventory)
                {
                    // save the data separated by commas (CSV style)
                    writer.WriteLine($"{p.Id},{p.Name},{p.Batch},{p.BoxQuantity},{p.StorageLocation.Aisle},{p.StorageLocation.Shelf}");
                }
            }
        }

        // Function to load data from a text file
        static void LoadDataFromFile()
        {
            // Check if the file exists before trying to read it
            if (File.Exists(filePath))
            {
                // ReadAllLines returns an array with all the lines in the file
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    // Split the line by commas to get each piece of data
                    string[] data = line.Split(',');

                    if (data.Length == 6)
                    {
                        Product loadedProduct = new Product();
                        loadedProduct.Id = int.Parse(data[0]);
                        loadedProduct.Name = data[1];
                        loadedProduct.Batch = data[2];
                        loadedProduct.BoxQuantity = int.Parse(data[3]);

                        // Recreate the Location struct
                        loadedProduct.StorageLocation = new Location(data[4], int.Parse(data[5]));

                        inventory.Add(loadedProduct);
                    }
                }
            }
        }
    }
}