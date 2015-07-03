using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqUnitTest.BookOrderingSystem.Model
{
    public enum Genre { HORROR, SIFI, DOCUMENTARY, COMEDY }

    public class Book
    {
        public int Id { get; set; }
        public String Author { get; set; }
        public String Title { get; set; }
        public double Price { get; set; }
        public Genre Genre { get; set; }

        public Book(String _author, String _title, double _price, Genre _genre) 
        {
            Author = _author;
            Title = _title;
            Price = _price;
            Genre = _genre;
        }
    }
}
