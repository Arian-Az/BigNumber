# Introduction
BigNumber DataTypec (class) for long-digit numbers, implemented by C#.

# Some tipps for dear readers
- This project was made for a (my) university class!
- There is already a [BigInteger struct](https://docs.microsoft.com/en-us/dotnet/api/system.numerics.biginteger?view=net-6.0) (dll, in system.Numerics) in .NET Framework and Core wich is operating by saving and implementing with bytes.
- In this code we use [StringBuilder](https://docs.microsoft.com/en-us/dotnet/api/system.text.stringbuilder?view=net-6.0) in order to store and work with the number.
- There are some problem with division operation that you can read in Operations part.
- If you are a student or you have some question about how to implement BigNumber yourself, i hope this code helps you ;)

# Structure
All methods and... have a comment inside the code to explane them but there are something you may want to know about:
there are 2 StringBuilder in order to store integer part and floating-point wich be filled reversly! (from last digit to first digit) , and a sign for sign of the number.
and 3 overloads for Constructor to declare a BigNumber:
```
// An empty BigNumber refrence wich returns 0 for operations and more.
BigNumber x = new BigNumber();

// With string Argument: in order to have a long-digit number without others DataTypes limits.
BigNumber y = new BigNumber("123456789.987654321");

// With object Argument so we can initializing our BigNumber with a Decimal/Integer dataType (int, float, ...)
BigNumber z = new BigNumber(12345);
```
- ## Operations
| operation     | existence |
| :---:      | :---:        |
| + | ![#c5f015](https://via.placeholder.com/15/c5f015/000000?text=+) YES  |
| - | ![#c5f015](https://via.placeholder.com/15/c5f015/000000?text=+) YES  |
| &times; | ![#c5f015](https://via.placeholder.com/15/c5f015/000000?text=+) YES   |
| / | ![#ffa600](https://via.placeholder.com/15/ffa600/000000?text=+) YES  |
