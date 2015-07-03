using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqUnitTest.BookOrderingSystem.Business
{
    public interface IBookOrderingSystem
    {
        bool OrderBookCopy(Model.Customer customer, Model.Book book);
        bool ReturnBookCopy(Model.Customer customer, Model.Book book);
        IList<Model.BookCopy> ViewAvailableBookCopies();
        IList<Model.Book> ViewAvailableBooks();
        IList<Model.BookCopy> ViewBookCopiesByBook(Model.Book book);
        void PremiumPurchase(Model.Customer customer);
        void NormalPurchase(Model.Customer customer);
    }
}
