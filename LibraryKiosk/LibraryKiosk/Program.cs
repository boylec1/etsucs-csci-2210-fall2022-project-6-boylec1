///////////////////////////////////////////////////////////////////////////////
//
// Author: Chris Boyle, boylec1@etsu.edu
// Course: CSCI-2210-001 - Data Structures
// Assignment: Project 6
// Description: Represents a Library Kiosk function. Reads books into Book 
// objects from a file, properly formatted with titles, authors, page counts,
// and publisher. Then adds this "library" to an AVL Tree, and demonstrates
// the functions of that tree (wrapped in, if I may be so bold, a fairly
// pretty user functionality). 
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.DataStructures;

namespace LibraryKiosk
{
    internal class Program
    {
        static void Main(string[] args)
        {         

            string bookFile = @"C:\Users\judge\Desktop\School\Fall 2022\Data Structures\labs\project6\books.csv";
            StreamReader fileReader = new StreamReader(bookFile);

            // Will contain the initial information from the file
            string[][] bookDataLines = new string[CountLinesInFile(bookFile)][];

            // Lists for A) created Books, B) Books that are checked out, C) Library Catalogue
            // in List form
            List<Book> books = new List<Book>();
            List<Book> checkedOut = new();
            List<Book> updatedBooksList = new();

            for (int i = 0; i < CountLinesInFile(bookFile); i++)
            {
                // Reads file and populates jagged array with arrays made from file lines
                string[] line = fileReader.ReadLine().Split(',');
                string[] splitArray = new string[line.Length];
                for (int j = 0; j < splitArray.Length; j++)
                {
                    splitArray[j] = line[j];
                    bookDataLines[i] = splitArray;                    
                }

                // Checks items in each array in the jagged array for the presence of quotation
                // marks in several fields. If these exist, runs a method to fix the split
                // information from lines such that each array has the format:
                // Index = 0 (Title), 1 (Author), 2 (Page count), 3 (Publisher)

                // Yes, it's much uglier than yours, but I already had it written and didn't want
                // to copy yours just to make it prettier. It works. 
                if (bookDataLines[i][0].StartsWith("\"") && 
                    (bookDataLines[i][2].StartsWith("\"") || bookDataLines[i][2].Equals("")))
                {
                    if (bookDataLines[i][2].Equals(""))
                    {
                        TitleQuotesAuthorEmpty(bookDataLines[i]);
                    }
                    else
                    {
                        TitleAndAuthorQuotes(bookDataLines[i]);
                    }
                }
                else if (bookDataLines[i][0].StartsWith("\""))
                {
                    TitleQuotes(bookDataLines[i]);
                }
                else if (bookDataLines[i][1].StartsWith("\""))
                {
                    AuthorQuotes(bookDataLines[i]);
                }

                int pageCount = 0;
                if (bookDataLines[i][2] != null)
                {
                    pageCount = Int32.Parse(bookDataLines[i][2]);
                }
                else
                {
                    pageCount = 0;
                }

                // Creates a Book out of each array once it's formatted properly and adds it
                // to the overall list of Books.
                books.Add(new Book(bookDataLines[i][0], bookDataLines[i][1], pageCount, bookDataLines[i][3]));
            }

            // Allows the user to choose what they want the library sorted by:
            // Title, Author, or Publisher.
            bool validChoice = false;
            string choice = "";
            while (validChoice == false)
            {
                CyanText("What do you want to sort by? Your choices are Title, Author, or Publisher.");
                string userInput = Console.ReadLine().ToLower();

                if (userInput.Equals("title") 
                    || userInput.Equals("author") 
                    || userInput.Equals("publisher"))
                {
                    choice = userInput;
                    validChoice = true;
                }
                else
                {
                    CyanText("Please type one of the following:\n Title \n Author \n Publisher");
                }
            }
            foreach (Book book in books)
            {
                book.sortChoice = choice;
            }

            // Creates an AVL Tree from the final list of Books.
            AvlTree<Book> library = new();
            foreach (Book book in books)
            {
                library.Add(book);
            }

            Console.WriteLine();
            Console.WriteLine("====================");
            Console.WriteLine("|                  |");
            Console.WriteLine("|     Library      |");
            Console.WriteLine("|        Is        |");
            Console.WriteLine("|       Ready      |");
            Console.WriteLine("|                  |");
            Console.WriteLine("====================");
            Console.WriteLine();
            
            // Library Kiosk mode. Prompts user for input and performs the requested action, including:
            // Continue or End, Book check in, Book check out
            bool stillCheckingOut = true;
            while (stillCheckingOut = true)
            {
                CyanText("Would you like to check a book in or out? Y/N");
                string userChoice = Console.ReadLine();

                // End sequence
                if (userChoice.ToLower().Equals("n"))
                {
                    CyanText("Thank you for stopping by!");
                    break;
                }
                // Asks if user is checking in or out
                else if (userChoice.ToLower().Equals("y"))
                {
                    CyanText("Are you checking a book in or out?");
                    string inOrOut = Console.ReadLine();

                    // Provides a list and prompt, then removes the chosen Book from the checkedOut
                    // list and adds it back to the AVL Tree
                    if (inOrOut.ToLower().Equals("in") && (checkedOut.Count() > 0))
                    {
                        CyanText("What book number are you checking in?");

                        CyanText("Books currenty checked out: ");
                        Console.WriteLine();
                        for (int i = 0; i < checkedOut.Count(); i++)
                        {
                            Console.WriteLine("==========================");
                            Console.WriteLine($"Book {i + 1}", i);
                            checkedOut[i].Print();
                            Console.WriteLine("==========================");
                        }

                        int checkedOutBook = Int32.Parse(Console.ReadLine());

                        library.Add(checkedOut[checkedOutBook - 1]);
                        checkedOut.RemoveAt(checkedOutBook - 1);
                    }
                    // In case there's nothing checked out
                    else if ((inOrOut.ToLower().Equals("in") && (checkedOut.Count() == 0)))
                    {
                        CyanText("Sorry, you don't have any books checked out!");
                    }
                    // Provides check out services. Prints a list of the library for user to
                    // peruse, and then asks which Book they want to check out. Adds the selected
                    // Book to the checkedOut list, then removes the Book from the library.
                    else if (inOrOut.ToLower().Equals("out"))
                    {
                        CyanText("Please consult the following list of available books.");
                        Thread.Sleep(2000);

                        updatedBooksList = library.GetInorderEnumerator().ToList();

                        for (int i = 0; i < updatedBooksList.Count(); i++)
                        {
                            Console.WriteLine($"========================= Book {i + 1} =========================", i);
                            updatedBooksList[i].Print();
                            Console.WriteLine("============================================================");
                        }

                        Console.WriteLine();
                        CyanText("Please type the number of the book you wish to check out.");

                        int bookChoice = Int32.Parse(Console.ReadLine());

                        Book checkingOut = updatedBooksList[bookChoice - 1];
                        checkedOut.Add(checkingOut);
                        library.Remove(checkingOut);
                        
                    }
                    else
                    {
                        CyanText("In or out?");
                    }

                }
                else
                {
                    Console.WriteLine("Sorry, I didn't get that. Y/N?");
                }

            } // End of while loop for library

        } // End of Main

        /// <summary>
        /// Takes an array of Book information where the Author is split in two fields, and
        /// corrects this to allow for proper Book formatting.
        /// </summary>
        /// <param name="array">An array of Book information with two Author fields.</param>
        /// <returns>An array of Book information with proper formatting (1 field each for Title,
        /// Author, Page count, and Publisher).</returns>
        public static string[] AuthorQuotes(string[] array)
        {
            array[1] = array[1] + "," + array[2];
            array[1] = array[1].Substring(1, (array[1].Length - 2));
            array[2] = array[3];
            array[3] = array[4];
            return array;

        } // End of AuthorQuotes

        /// <summary>
        /// Takes an array of Book information where the Title is split in two fields, and
        /// corrects this to allow for proper Book formatting.
        /// </summary>
        /// <param name="array">An array of Book information with two Title fields.</param>
        /// <returns>An array of Book information with proper formatting (1 field each for Title,
        /// Author, Page count, and Publisher).</returns>
        public static string[] TitleQuotes(string[] array)
        {
            array[0] = array[0] + "," + array[1];
            array[0] = array[0].Substring(1, (array[0].Length - 2));
            array[1] = array[2];
            array[2] = array[3];
            array[3] = array[4];
            return array;

        } // End of TitleQuotes

        /// <summary>
        /// Takes an array of Book information where both the Title and Author are split in 
        /// two fields, and corrects this to allow for proper Book formatting.
        /// </summary>
        /// <param name="array">An array of Book information with two Title and Author fields.</param>
        /// <returns>An array of Book information with proper formatting (1 field each for Title,
        /// Author, Page count, and Publisher).</returns>
        public static string[] TitleAndAuthorQuotes(string[] array)
        {            
            array[0] = array[0] + "," + array[1];
            array[0] = array[0].Substring(1, (array[0].Length - 2));
            array[1] = array[2] + "," + array[3];
            array[1] = array[1].Substring(1, (array[1].Length - 2));
            array[2] = array[4];
            array[3] = array[5];
            return array;
            
        } // End of TitleAndAuthorQuotes

        /// <summary>
        /// Takes an array of Book information where the Title is split in two fields and 
        /// the Author field is blank. CSorrects this to allow for proper Book formatting.
        /// </summary>
        /// <param name="array">An array of Book information with two Title fields and a blank
        /// Author field.</param>
        /// <returns>An array of Book information with proper formatting (1 field each for Title,
        /// Author, Page count, and Publisher).</returns>
        public static string[] TitleQuotesAuthorEmpty(string[] array)
        {            
            array[0] = array[0] + "," + array[1];
            array[0] = array[0].Substring(1, (array[0].Length - 2));
            array[1] = array[2];
            array[2] = array[3];
            array[3] = array[4];
            return array;
            
        } // End of TitleAndAuthorQuotes

        /// <summary>
        /// Prints a given List of Books.
        /// </summary>
        /// <param name="books">A List of Books.</param>
        public static void BookTest(List<Book> books)
        {
            int bookCount = 1;
            foreach (Book book in books)
            {
                Console.WriteLine("------- Book " + bookCount++ + " --------");
                book.Print();
                Console.WriteLine("----------------------");
            }
        } // End of BookTest

        /// <summary>
        /// Counts the number of lines in a file. 
        /// </summary>
        /// <param name="fileName">The file path of the given file.</param>
        /// <returns>The line count of the given file.</returns>
        static int CountLinesInFile(string fileName)
        {
            int count = 0;
            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    count++;
                }
            }
            return count;
        } // End of CountLineInFile

        /// <summary>
        /// Prints the AVL Tree based on index.
        /// </summary>
        /// <param name="library">A given AVL Tree.</param>
        /// <param name="amountOfBooks">The amount of Books in a given AVL Tree.</param>
        public static void PrintLibrary(AvlTree<Book> library, int amountOfBooks)
        {            
            for (int i = 0; i < amountOfBooks; i++)
            {
                Console.WriteLine($"====== Book {i + 1} ======", i);
                library.ElementAt(i).Print();
                Console.WriteLine("======================");
            }
        } // End of PrintLibrary

        /// <summary>
        /// Colors given text in Cyan. Used to highlight program messages for readability.
        /// </summary>
        /// <param name="statement">The given text to be highlighted in Cyan.</param>
        public static void CyanText(string statement)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(statement);
            Console.ForegroundColor = ConsoleColor.Gray;
        } // End of CyanText


    }
}