using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Transactions;
using System.Xml.Linq;

namespace ATMS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            String fileName = "users.txt";
            Bank bank = new Bank(fileName);
            menu(bank);

        }

        // Begining of the program. Shows menu bar and do all the work.
        private static void menu(Bank bank)
        {
            while (true)
            {
                Console.WriteLine("Welcome to the Bank.");
                Console.WriteLine("1. Enter in your account");
                Console.WriteLine("2. Register");
                Console.WriteLine("3. Quit");
                Console.Write("Enter number of operation : ");
                char op = getValidOp('1', '3');
                bool quit = false;
                switch (op)
                {
                    case '1':
                        enter(bank);
                        break;
                    case '2':
                        register(bank);
                        break;
                    case '3':
                        quit = true;
                        bank.reWriteFile();
                        break;

                }
                if (quit) break;
                Console.WriteLine();
            }
        }



        // This method is called when user wants to register. reads information and registers user.
        private static void register(Bank bank)
        {
            Console.WriteLine("To register fill the following form.");
            Console.Write("1. Name : ");
            string name = getValidString("name");
            Console.Write("2. Password : ");
            String password = getValidString("password");
            Console.Write("3. Initial deposit : ");
            double amo = getValidNumber();

            if (!bank.addAccount(name, password, amo)) Console.WriteLine("User with this name already exist");
            else Console.WriteLine("Registered successfully!");

            Console.WriteLine();
        }


        // This method is called when user wants to enter in his account.
        // Reads users informations checks if it is valid  and do operations user wants to do.
        private static void enter(Bank bank)
        {
            Console.Write("Enter user name : ");
            string name = getValidString("name");
            Console.Write("Enter password : ");
            string password = getValidString("password");

            if (!bank.userExist(name, password))
            {
                Console.WriteLine("Invalid input! password is incorrect or account does not exist.");
                return;
            }

            checkAccount(bank, name, password);

        }

        //This methods is called by  enter() method and is responsible for users operations.
        private static void checkAccount(Bank bank, string name, string password)
        {
            Console.WriteLine("Entered successfully");
            Console.WriteLine("1. check balance");
            Console.WriteLine("2. withdraw money");
            Console.WriteLine("3. deposit money");
            Console.WriteLine("4. quit");

            while (true)
            {
                bool quit = false;
                Console.Write("Choose the number of operation : ");
                char op = getValidOp('1', '4');

                switch (op)
                {
                    case '1':
                        Console.WriteLine("your balance is " + bank.checkBalance(name, password));
                        break;
                    case '2':
                        Console.Write("Enter amount of money you want to withdraw : ");
                        double amo = getValidNumber();
                        if (bank.withdrawMoney(name, password, amo)) Console.WriteLine("Take your money");
                        else Console.WriteLine("You do not have enough money");
                        break;
                    case '3':
                        Console.Write("Enter amount of money you want to deposit : ");
                        double depo = getValidNumber();
                        if (bank.depositMoney(name, password, depo)) Console.WriteLine("Deposited successfully");
                        else Console.WriteLine("Deposited unsuccessfully!");
                        break;
                    case '4':
                        quit = true;
                        break;
                    default:
                        Console.WriteLine("please enter valid number!");
                        break;

                }
                if (quit) break;
            }
        }
        //This method is responisbe for getting valid number of operation which is between start and end (inclusive)
        private static char getValidOp(char start, char end)
        {
            char op;
            while (true)
            {
                if (char.TryParse(Console.ReadLine(), out op) && op >= start && op <= end) return op;
                Console.Write("please enter valid number of operation : ");
            }
        }
        //This method is responisbe for getting validstring input for password and name
        private static String getValidString(string st)
        {
            string name;
            while (true)
            {
                name = Console.ReadLine();
                if (name.Length >= 4) return name;
                Console.WriteLine($"Enter valid {st}, {st} should contain at least 4 letter");
            }
        }


        //This method is responisbe for getting valid amount of money user want to withdraw or deposit.
        private static double getValidNumber()
        {
            double res;
            while (true)
            {
                if (double.TryParse(Console.ReadLine(), out res)) return res;
                Console.WriteLine("Please enter valid number.");
            }


        }


        class Bank
        {

            private List<Account> Users;
            private string FileName { get; set; }


            public Bank(string fileName)
            {
                FileName = fileName;
                Users = new List<Account>();
                using (FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
                using (StreamReader sr = new StreamReader(fs))
                {
                    string usersJson = sr.ReadToEnd();
                    if (usersJson != "") Users = JsonSerializer.Deserialize<List<Account>>(usersJson);

                }
                // This is made because in the file IDs should be distinct 
                int maxID = Users.Max(x => x.ID);
                Account account = new Account();
                account.setID(maxID);

            }

            //This method writes changed information into file.
            public void reWriteFile()
            {
                File.Delete(FileName);
                string changed = JsonSerializer.Serialize(Users, new JsonSerializerOptions { WriteIndented = true });
                using (FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate))
                {
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        writer.Write(changed);
                    }
                }

            }

            //Adds users account in the bank
            public bool addAccount(string name, string password, double amo)
            {
                bool alreadyExist = Users.Any(x => x.Name == name);
                if (alreadyExist) return false;
                Users.Add(new Account(name, password, amo));
                return true;
            }
            
            // returns object of account class (if exist) from the bank
            private Account getAccount(String name, String password)
            {
                var u = Users.Where(x => (x.Name == name && x.Password == password)).FirstOrDefault();
                return (Account)u;

            }
            //Checks if there are any account with this name and password
            public bool userExist(String name, String password)
            {
                return Users.Any(x => x.Name == name && x.Password == password);

            }
            // Deposits money in the accout which belongs to the user who has this name and password
            public bool depositMoney(String name, String password, double amount)
            {
                Account user = getAccount(name, password);
                if (amount < 0) return false;
                user.Amount += amount;
                return true;
            }
            // Witdraw money from the accout which belongs to the user who has this name and password
            public bool withdrawMoney(String name, String password, double amount)
            {
                Account user = getAccount(name, password);
                if (amount < 0 || amount > user.Amount) return false;
                user.Amount -= amount;
                return true;
            }
            // Checks the balance in the accout which belongs to the user who has this name and password
            public double checkBalance(String name, String password)
            {
                Account user = getAccount(name, password);
                if (user == null) return -1;
                return user.Amount;
            }
        }


        class Account
        {
            private static int Idhelper = 0;
            public int ID { get; set; }
            public string Name { get; set; }
            public string Password { get; set; }

            [JsonInclude]
            public double Amount { get; set; }
            public Account() { }
            public Account(string name, string password, double initialDeposit = 0)
            {
                ID = ++Idhelper;
                Name = name;
                Password = password;
                Amount = initialDeposit;
            }

            [JsonConstructor]
            public Account(int id, string name, string password, double amount)
            {
                ID = id;
                Name = name;
                Password = password;
                Amount = amount;
            }
            public void setID(int id) { Idhelper = id; }

        }


    }
}
