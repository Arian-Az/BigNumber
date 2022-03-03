using System;

namespace BigNumberStructure
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter numbers a:");
            // a way to create and convert to BigNumber
            BigNumber a = BigNumber.ToBigNumber(Console.ReadLine());


            Console.WriteLine("Enter numbers b:");
            // another way to create BigNumber
            string s = Console.ReadLine();
            BigNumber b = new BigNumber(s);

            // You  can also define a BigNumber with All decimal/integer DataTypes:
            // BigNumber b = new BigNumber(12356.789);

            // Comparing Operators
            Console.WriteLine("\nComparing Operators:");
            Console.WriteLine("a > b = {0}", a > b);
            Console.WriteLine("a >= b = {0}", a >= b);
            Console.WriteLine("a < b = {0}", a < b);
            Console.WriteLine("a <= b = {0}", a <= b);
            Console.WriteLine("a == b = {0}", a == b);
            Console.WriteLine("a != b = {0}", a != b);

            // Math Operator
            Console.WriteLine("\nMath Operators:");
            Console.WriteLine("-a = {0}", -a);
            Console.WriteLine("|b - a| = {0}", BigNumber.Abs(b - a));
            Console.WriteLine("Max(a , b) = {0}", BigNumber.Max(a, b));
            Console.WriteLine("Min(a , b) = {0}", BigNumber.Min(a, b));
            Console.WriteLine("a + b = {0}", a + b);
            Console.WriteLine("a - b = {0}", a - b);
            Console.WriteLine("a * b = {0}", a * b);

            //Doesn't work properly for complicated values!
            //Console.WriteLine("a / b = {0}", a / b);

            Console.WriteLine("a ^ b = {0}", BigNumber.Pow(a, b));


            // a for-loop test
            Console.WriteLine("\nEnter n for nth Fibonacci number:");
            int n = Convert.ToInt32(Console.ReadLine());

            BigNumber fn = new BigNumber();
            BigNumber fl1 = new BigNumber("1");
            BigNumber fl2 = new BigNumber("1");

            for (int i = 3; i <= n; i++)
            {

                fn = fl1 + fl2;
                fl2 = fl1;
                fl1 = fn;

            }
            Console.WriteLine("\nFibonacci({1}) is:\n{0}", fn, n);

            Console.ReadKey();
        }
    }
}
