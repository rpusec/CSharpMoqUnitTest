using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqUnitTest.BookOrderingSystem.Business
{
    public interface IBookOrderingSystem
    {
        public bool OrderBookCopy(Model.Customer customer);
        public bool ReturnBookCopy(Model.Customer customer);
        public IList<Model.BookCopy> ViewAvailableBookCopies();
        private void PremiumPurchase(Model.Customer customer);
        private void NormalPurchase(Model.Customer customer);
    }
}
