using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqUnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            IList<string> test = new List<string>();

            var res = test.Where(x => x == "yay");

            System.Diagnostics.Debug.WriteLine(res);
        }
    }
}
