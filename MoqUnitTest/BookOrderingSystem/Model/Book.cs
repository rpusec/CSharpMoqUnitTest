using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqUnitTest.BookOrderingSystem.Model
{
    public enum BookGenre { HORROR, SIFI, DOCUMENTARY, COMEDY, FANTASY }

    public class Book
    {
        public int Id { get; set; }
        public String Author { get; set; }
        public String Title { get; set; }
        public double Price { get; set; }
        public BookGenre Genre { get; set; }

        public Book(String _author, String _title, double _price, BookGenre _genre) 
        {
            Author = _author;
            Title = _title;
            Price = _price;
            Genre = _genre;
        }
    }
}
