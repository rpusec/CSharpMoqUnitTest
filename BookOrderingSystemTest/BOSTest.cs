﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoqUnitTest.BookOrderingSystem.Business;
using MoqUnitTest.BookOrderingSystem.Model;
using Moq;
using System.Linq;
using System.Collections.Generic;

namespace BookOrderingSystemTest
{
    [TestClass]
    public class BOSTest
    {
        private IDictionary<Book, int> bookCopiesData;
        private Mock<IBookOrderingSystem> mockBookOrderingSystem;

        [TestInitialize]
        public void Initialize() 
        {
            //creating mock instance
            mockBookOrderingSystem = new Mock<IBookOrderingSystem>();

            //preparing books
            IList<Book> books = new List<Book>
            {
                new Book("J. K. Rownling", "Harry Potter", 9.99, BookGenre.FANTASY),
                new Book("Richard Dawkins", "On the Origin of Species", 5.99, BookGenre.DOCUMENTARY),
                new Book("Charles Lutwidge Dodgson", "Alice's Adventures in Wonderland", 8.99, BookGenre.FANTASY),
                new Book("Frank Herbert", "Dune", 7.99, BookGenre.SIFI)
            };

            //setting the amount of copies for all of the books
            bookCopiesData = new Dictionary<Book, int>();
            bookCopiesData[books[0]] = 5;
            bookCopiesData[books[1]] = 7;
            bookCopiesData[books[2]] = 3;
            bookCopiesData[books[3]] = 4;

            //preparing book copies
            IList<BookCopy> bookCopies = new List<BookCopy>();

            foreach(var bookCopyInfo in bookCopiesData)
            {
                //adds as many copies of the book as required
                for (var i = 0; i < bookCopyInfo.Value; i++)
                    bookCopies.Add(new BookCopy(bookCopyInfo.Key));
            }

            //view all book copies
            mockBookOrderingSystem.Setup(x => x.ViewAvailableBookCopies()).Returns(bookCopies);
            
            //view all books
            mockBookOrderingSystem.Setup(x => x.ViewAvailableBooks()).Returns(books);

            //returns list of book copies by target book
            mockBookOrderingSystem.Setup(x => x.ViewBookCopiesByBook(It.IsAny<Book>()))
                .Returns((Book book) => bookCopies.Where(x => x.Book == book).ToList<BookCopy>());

            //orders a book copy and excludes the copy from the list
            mockBookOrderingSystem.Setup(x => x.OrderBookCopy(It.IsAny<Customer>(), It.IsAny<Book>()))
                .Returns((Customer customer, Book book) => {

                    //getting all available books
                    var retrievedBookCopies = bookCopies.Where(b => b.Book == book);
                    var bookCopyList = retrievedBookCopies.ToList<BookCopy>();
                    
                    //true if no books are available
                    if (bookCopyList.Count == 0)
                        return false;
                    else 
                    {
                        //give the customer the book and remove it from the list
                        customer.OrderedBooks.Add(bookCopyList[0]);
                        bookCopies.Remove(bookCopyList[0]);

                        //gold users have a special advantage
                        if (customer.CustomerLevel == CustomerLevel.GOLD)
                            mockBookOrderingSystem.Object.PremiumPurchase(customer);
                        else
                            mockBookOrderingSystem.Object.NormalPurchase(customer);
                    }
                                        
                    return true;
                });

            //returns a copy back to the store
            mockBookOrderingSystem.Setup(x => x.ReturnBookCopy(It.Is<Customer>(
                c => c.OrderedBooks.Count != 0), It.IsAny<Book>()))
                .Returns((Customer customer, Book book) => {

                    //gets a single bookcopy element
                    BookCopy customersBookCopy = customer.OrderedBooks.Where(x => x.Book == book).Single();

                    //removes the book copy from the customer and adds it back to the store
                    if (customer.OrderedBooks.Contains(customersBookCopy))
                    {
                        customer.OrderedBooks.Remove(customersBookCopy);
                        bookCopies.Add(customersBookCopy);
                        return false;
                    }

                    return true;
                });
        }

        [TestMethod]
        public void ViewAllAvailableBooksTest() {
            Assert.AreEqual(mockBookOrderingSystem.Object.ViewAvailableBooks().Count, 4, "Not the same amount of books. ");
            
            var sum = 0; //sum of all of the book copies

            foreach(var bookCopyInfo in bookCopiesData)
                sum += bookCopyInfo.Value;

            Assert.AreEqual(mockBookOrderingSystem.Object.ViewAvailableBookCopies().Count, sum, "Not the same amount of book copies");
        }

        [TestMethod]
        public void BookCopiesByBookTest() {
            //harry potter copies
            var harryPotterCopies = mockBookOrderingSystem.Object
                .ViewBookCopiesByBook(mockBookOrderingSystem.Object.ViewAvailableBooks()[0]);

            //alice in wonderland copies
            var alicePotterCopies = mockBookOrderingSystem.Object
                .ViewBookCopiesByBook(mockBookOrderingSystem.Object.ViewAvailableBooks()[2]);

            //checking book copy quantity
            Assert.AreEqual(harryPotterCopies.Count, 5);
            Assert.AreEqual(alicePotterCopies.Count, 3);
        }
    }
}
