using System;
using System.Collections.Generic;

namespace LibraryManagement 
{
    public abstract class LibraryItem
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }

        public LibraryItem(string title, string author, int year)
        {
            Title = title;
            Author = author;
            Year = year;
        }

        public abstract void DisplayInfo();
    }

    public class Book : LibraryItem, IBorrowable
    {
        public int PageCount { get; set; }
        public bool IsBorrowed { get; private set; }
        private BorrowInfo _borrowInfo;

        public Book(string title, string author, int year, int pageCount)
            : base(title, author, year)
        {
            PageCount = pageCount;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Book: {Title} by {Author}, Year: {Year}, Pages: {PageCount}");
            if (IsBorrowed)
            {
                Console.WriteLine($"Currently borrowed by {_borrowInfo.BorrowerName} on {_borrowInfo.BorrowDate.ToShortDateString()}");
            }
            else
            {
                Console.WriteLine("Available for borrowing.");
            }
        }

        public void Borrow(string borrower)
        {
            if (!IsBorrowed)
            {
                IsBorrowed = true;
                _borrowInfo = new BorrowInfo { BorrowerName = borrower, BorrowDate = DateTime.Now };
                Console.WriteLine($"The book '{Title}' has been borrowed by {borrower}.");
            }
            else
            {
                Console.WriteLine($"The book '{Title}' is already borrowed.");
            }
        }

        public void Return()
        {
            if (IsBorrowed)
            {
                IsBorrowed = false;
                Console.WriteLine($"The book '{Title}' has been returned.");
            }
            else
            {
                Console.WriteLine($"The book '{Title}' was not borrowed.");
            }
        }
    }

    public class Journal : LibraryItem, IBorrowable
    {
        public string Issue { get; set; }
        public bool IsBorrowed { get; private set; }
        private BorrowInfo _borrowInfo;

        public Journal(string title, string author, int year, string issue)
            : base(title, author, year)
        {
            Issue = issue;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Journal: {Title} by {Author}, Year: {Year}, Issue: {Issue}");
            if (IsBorrowed)
            {
                Console.WriteLine($"Currently borrowed by {_borrowInfo.BorrowerName} on {_borrowInfo.BorrowDate.ToShortDateString()}");
            }
            else
            {
                Console.WriteLine("Available for borrowing.");
            }
        }

        public void Borrow(string borrower)
        {
            if (!IsBorrowed)
            {
                IsBorrowed = true;
                _borrowInfo = new BorrowInfo { BorrowerName = borrower, BorrowDate = DateTime.Now };
                Console.WriteLine($"The journal '{Title}' has been borrowed by {borrower}.");
            }
            else
            {
                Console.WriteLine($"The journal '{Title}' is already borrowed.");
            }
        }

        public void Return()
        {
            if (IsBorrowed)
            {
                IsBorrowed = false;
                Console.WriteLine($"The journal '{Title}' has been returned.");
            }
            else
            {
                Console.WriteLine($"The journal '{Title}' was not borrowed.");
            }
        }
    }

    public class EBook : LibraryItem
    {
        public string FileFormat { get; set; }

        public EBook(string title, string author, int year, string fileFormat)
            : base(title, author, year)
        {
            FileFormat = fileFormat;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"EBook: {Title} by {Author}, Year: {Year}, Format: {FileFormat}");
            Console.WriteLine("EBook is not available for borrowing.");
        }
    }

    public interface IBorrowable
    {
        bool IsBorrowed { get; }
        void Borrow(string borrower);
        void Return();
    }

    public struct BorrowInfo
    {
        public string BorrowerName;
        public DateTime BorrowDate;
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<LibraryItem> libraryItems = new List<LibraryItem>
            {
                new Book("C# Programming", "John Doe", 2020, 300),
                new Book("Mastering Algorithms", "Jane Smith", 2018, 450),
                new Journal("Tech Journal", "Alice Johnson", 2023, "Vol. 5"),
                new EBook("Digital Transformation", "Michael Lee", 2022, "PDF"),
            };

            while (true)
            {
                Console.WriteLine("\nLibrary Management System");
                Console.WriteLine("1. Show all items");
                Console.WriteLine("2. Show available items for borrowing");
                Console.WriteLine("3. Borrow an item");
                Console.WriteLine("4. Return an item");
                Console.WriteLine("5. Exit");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllItems(libraryItems);
                        break;
                    case "2":
                        ShowAvailableItems(libraryItems);
                        break;
                    case "3":
                        BorrowItem(libraryItems);
                        break;
                    case "4":
                        ReturnItem(libraryItems);
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void ShowAllItems(List<LibraryItem> items)
        {
            foreach (var item in items)
            {
                item.DisplayInfo();
            }
        }

        static void ShowAvailableItems(List<LibraryItem> items)
        {
            foreach (var item in items)
            {
                if (item is IBorrowable borrowableItem && !borrowableItem.IsBorrowed)
                {
                    item.DisplayInfo();
                }
            }
        }

        static void BorrowItem(List<LibraryItem> items)
        {
            Console.Write("Enter the title of the item you want to borrow: ");
            string title = Console.ReadLine();

            LibraryItem item = items.Find(i => i.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            if (item != null && item is IBorrowable borrowableItem && !borrowableItem.IsBorrowed)
            {
                Console.Write("Enter your name: ");
                string borrower = Console.ReadLine();
                borrowableItem.Borrow(borrower);
            }
            else
            {
                Console.WriteLine("Item is not available for borrowing or does not exist.");
            }
        }

        static void ReturnItem(List<LibraryItem> items)
        {
            Console.Write("Enter the title of the item you want to return: ");
            string title = Console.ReadLine();

            LibraryItem item = items.Find(i => i.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            if (item != null && item is IBorrowable borrowableItem && borrowableItem.IsBorrowed)
            {
                borrowableItem.Return();
            }
            else
            {
                Console.WriteLine("Item was not borrowed or does not exist.");
            }
        }
    }
}
