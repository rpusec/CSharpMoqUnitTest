using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqUnitTest.BookOrderingSystem.Model
{
    public class BookCopy
    {
        public Book Book { get; set; }

        public BookCopy(Book _book) 
        {
            Book = _book;
        }
    }
}
