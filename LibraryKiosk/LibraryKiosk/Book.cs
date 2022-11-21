///////////////////////////////////////////////////////////////////////////////
//
// Author: Chris Boyle, boylec1@etsu.edu
// Course: CSCI-2210-001 - Data Structures
// Assignment: Project 6
// Description: A Book object, having a Title, Author, number of Pages, and
// Publisher. Has a Print() method to format printing properly. Implements the
// IComparable interface to be able to compare two Book objects. 
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DataStructures;

namespace LibraryKiosk
{    
    public class Book : IComparable<Book>
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int Pages { get; set; }
        public string Publisher { get; set; }

        // Allows a user to set their preferred sorting choice for the tree.
        public string sortChoice = "";

        public Book(string title, string author, int pages, string publisher)
        {
            Title = title;
            Author = author;
            Pages = pages;
            Publisher = publisher;
        }

        /// <summary>
        /// Prints each book with a specific format
        /// </summary>
        public void Print()
        {
            Console.WriteLine("Title: " + Title);
            Console.WriteLine("Author: " + Author);
            Console.WriteLine("Pages: " + Pages);
            Console.WriteLine("Publisher: " + Publisher);
        }

        /// <summary>
        /// Compares two books, and checks property "sortChoice" to determine by what property
        /// to compare books (Title, Author, or Publisher). 
        /// </summary>
        /// <param name="book">A Book object passed through.</param>
        /// <returns>An int value representing the comparison between the two Book objects.</returns>
        public int CompareTo(Book? book)
        {
            if (book is Book)
            {
                Book otherBook = book;
                if (sortChoice.Equals("title"))
                {
                    return Title.CompareTo(book.Title);
                }
                else if (sortChoice.Equals("author"))
                {
                    return Author.CompareTo(book.Author);
                }
                else if (sortChoice.Equals("publisher"))
                {
                    return Publisher.CompareTo(book.Publisher);
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }
        }
    }
}
