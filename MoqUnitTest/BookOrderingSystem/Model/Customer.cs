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
        public CustomerLevel CustomerLevel { get; set; }

        public Customer(CustomerLevel _customerLevel) 
        {
            OrderedBooks = new List<Model.BookCopy>();
            CustomerLevel = _customerLevel;
        }
    }
}
