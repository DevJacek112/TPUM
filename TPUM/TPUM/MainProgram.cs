using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPUM
{
    internal class MainProgram
    {
        public static void Main()
        {
            Calculator calculatorObject = new Calculator();
            Console.Write(calculatorObject.Add(2, 3));
        }
    }
}
