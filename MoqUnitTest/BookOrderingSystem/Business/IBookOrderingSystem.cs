using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqUnitTest.BookOrderingSystem.Business
{
    public interface IBookOrderingSystem
    {
        bool OrderBookCopy(Model.Customer customer);
        bool ReturnBookCopy(Model.Customer customer);
        IList<Model.BookCopy> ViewAvailableBookCopies();
        void PremiumPurchase(Model.Customer customer);
        void NormalPurchase(Model.Customer customer);
    }
}
