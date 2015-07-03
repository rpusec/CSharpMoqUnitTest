using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqUnitTest.BookOrderingSystem.Model
{
    public enum CustomerLevel { BRONZE, SILVER, GOLD }

    public class Customer
    {
        public IList<Model.BookCopy> OrderedBooks { get; private set; }

        public Customer() 
        {
            OrderedBooks = new List<Model.BookCopy>();
        }
    }
}
