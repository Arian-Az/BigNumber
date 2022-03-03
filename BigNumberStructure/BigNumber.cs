using System;
using System.Linq;
using System.Text;

namespace BigNumberStructure
{
    // by Arian Azizian
    // github: https://github.com/Arian-Az

    public class BigNumber : IComparable
    {
        // Contains integer digits of the number
        private StringBuilder integerDigits;

        // Contains floating point digits of the number
        private StringBuilder floatDigits;

        // If number is positive sign = 1 otherwise sign = -1
        internal int sign;
        // If number is decimal = true
        internal bool IsFloat = false;

        #region Constructors
        // Empty constructor for empty definitions 
        public BigNumber()
        {
            integerDigits = new StringBuilder();
            floatDigits = new StringBuilder();
            sign = 1;
        }

        // Constructor with string parameter (for long-digit entry numbers) - O(n) for filing and RegEx Pattern
        public BigNumber(string value)
        {
            if (value == string.Empty || !System.Text.RegularExpressions.Regex.IsMatch(value, @"^[-+]?\d(\.?\d)*$"))
                throw new Exception("Value doesn't represent a number!");

            if (value[0] == '-')
                sign = -1;
            else
                sign = 1;

            integerDigits = new StringBuilder();
            floatDigits = new StringBuilder();

            int i = value.Length - 1;
            int pointPos = value.IndexOf('.');
            if (pointPos > 0)
            {
                IsFloat = true;
                for (; i > pointPos; i--)
                {
                    floatDigits.Append(value[i]);
                }

                i = pointPos - 1;
            }

            for (; i >= (sign > 0 ? 0 : 1); i--)
            {
                integerDigits.Append(value[i]);
            }
        }

        // Constructor with object parameter (for other number type entries) - O(n) for ToBigNumber function...
        public BigNumber(object value)
        {
            BigNumber convertedValue = ToBigNumber(value);
            this.integerDigits = convertedValue.integerDigits;
            this.floatDigits = convertedValue.floatDigits;
            this.sign = convertedValue.sign;
            this.IsFloat = convertedValue.IsFloat;
            convertedValue = default;
        }
        #endregion

        #region Operators Overloading
        // Overloads for comparing 2 BigNumber class object
        public static bool operator >(BigNumber a, BigNumber b) => (a.CompareTo(b) > 0);
        public static bool operator <(BigNumber a, BigNumber b) => (a.CompareTo(b) < 0);
        public static bool operator >=(BigNumber a, BigNumber b) => (a.CompareTo(b) >= 0);
        public static bool operator <=(BigNumber a, BigNumber b) => (a.CompareTo(b) <= 0);
        public static bool operator ==(BigNumber a, BigNumber b) => (a.CompareTo(b) == 0);
        public static bool operator !=(BigNumber a, BigNumber b) => (a.CompareTo(b) != 0);

        // Comparing other Decimal/Integer DataTypes with BigNumber
        public static bool operator >(BigNumber a, object b) => (a.CompareTo(b) > 0);
        public static bool operator <(BigNumber a, object b) => (a.CompareTo(b) < 0);
        public static bool operator >=(BigNumber a, object b) => (a.CompareTo(b) >= 0);
        public static bool operator <=(BigNumber a, object b) => (a.CompareTo(b) <= 0);
        public static bool operator ==(BigNumber a, object b) => (a.CompareTo(b) == 0);
        public static bool operator !=(BigNumber a, object b) => (a.CompareTo(b) != 0);
        public static bool operator >(object a, BigNumber b) => (b.CompareTo(a) < 0);
        public static bool operator <(object a, BigNumber b) => (b.CompareTo(a) > 0);
        public static bool operator >=(object a, BigNumber b) => (b.CompareTo(a) <= 0);
        public static bool operator <=(object a, BigNumber b) => (b.CompareTo(a) >= 0);
        public static bool operator ==(object a, BigNumber b) => (b.CompareTo(a) == 0);
        public static bool operator !=(object a, BigNumber b) => (b.CompareTo(a) != 0);

        public static BigNumber operator +(BigNumber a) => a;

        // Sum of 2 BigNumbers
        public static BigNumber operator +(BigNumber a, BigNumber b) => Add(a, b);

        //Sum of a BigNumber with a Decimal/Integer dataType and vice versa
        public static BigNumber operator +(BigNumber a, object b) => Add(a, ToBigNumber(b));
        public static BigNumber operator +(object a, BigNumber b) => Add(ToBigNumber(a), b);
        public static BigNumber operator ++(BigNumber a) => Add(a, One);

        public static BigNumber operator -(BigNumber a)
        {
            a.sign *= -1;
            return a;
        }

        // Subtract of 2 BigNumbers
        public static BigNumber operator -(BigNumber a, BigNumber b) => Subtract(a, b);
        //Sum of a BigNumber with a Decimal/Integer dataType and vice versa
        public static BigNumber operator -(BigNumber a, object b) => Subtract(a, ToBigNumber(b));
        public static BigNumber operator -(object a, BigNumber b) => Subtract(ToBigNumber(a), b);
        public static BigNumber operator --(BigNumber a) => Subtract(a, One);


        // Multiplication of 2 BigNumbers
        public static BigNumber operator *(BigNumber a, BigNumber b) => Multiply(a, b);
        // Multiplication of a BigNumber with a Decimal/Integer dataType and vice versa
        public static BigNumber operator *(BigNumber a, object b) => Multiply(a, ToBigNumber(b));
        public static BigNumber operator *(object a, BigNumber b) => Multiply(ToBigNumber(a), b);


        // Division of 2 BigNumbers
        public static BigNumber operator /(BigNumber a, BigNumber b) => Divide(a, b);
        // Division of a BigNumber with a Decimal/Integer dataType and vice versa
        public static BigNumber operator /(BigNumber a, object b) => Divide(a, ToBigNumber(b));
        public static BigNumber operator /(object a, BigNumber b) => Divide(ToBigNumber(a), b);
        #endregion

        #region Math Functions
        // Addition of 2 BigNumber - Normal addition algorithm - O(n)
        public static BigNumber Add(BigNumber a, BigNumber b)
        {
            if (a.sign != b.sign)
                return Subtract(a, b);

            BigNumber res = new BigNumber();

            int ni = a.IntegerDigitsCount, mi = b.IntegerDigitsCount;
            int t = 0;

            if (a.IsFloat || b.IsFloat)
            {
                int nd = a.FloatDigitsCount, md = b.FloatDigitsCount;

                if (md > nd)
                {
                    for (int i = 0; i < md - nd; i++)
                        a.floatDigits.Insert(0, '0');
                    nd = a.FloatDigitsCount;
                }
                else if (nd > md)
                {
                    for (int i = 0; i < nd - md; i++)
                        b.floatDigits.Insert(0, '0');
                    md = b.FloatDigitsCount;
                }

                bool continuesZero = true;
                for (int i = 0; i < nd; i++)
                {
                    int digitSum;
                    digitSum = (a.floatDigits[i] - '0') + (b.floatDigits[i] - '0') + t;
                    t = digitSum / 10;

                    if ((continuesZero && digitSum % 10 != 0) || !continuesZero)
                    {
                        res.floatDigits.Append(digitSum % 10);
                        continuesZero = false;
                    }
                }

                if (res.floatDigits.Length > 0)
                    res.IsFloat = true;
            }

            if (mi > ni)
            {
                a.integerDigits.Append('0', mi - ni);
                ni = a.IntegerDigitsCount;
            }

            for (int i = 0; i < ni; i++)
            {
                int digitSum;
                if (i < mi)
                    digitSum = (a.integerDigits[i] - '0') + (b.integerDigits[i] - '0') + t;
                else
                    digitSum = (a.integerDigits[i] - '0') + t;

                t = digitSum / 10;
                res.integerDigits.Append(digitSum % 10);
            }
            if (t > 0)
                res.integerDigits.Append(t);

            res.sign = a.sign;
            return res;
        }

        // Subtraction of 2 BigNumber - Normal subtraction algorithm - O(n)
        public static BigNumber Subtract(BigNumber a, BigNumber b)
        {
            if (a.sign == -1 && a.sign == b.sign)
                return Add(a, b);

            BigNumber res = new BigNumber();

            if (a.sign == b.sign)
            {
                if (a < b)
                    Swap(ref a, ref b);
            }
            else if (Abs(a) < Abs(b))
            {
                Swap(ref a, ref b);
            }

            int ni = a.IntegerDigitsCount, mi = b.IntegerDigitsCount;
            int t = 0;

            if (a.IsFloat || b.IsFloat)
            {
                int nd = a.FloatDigitsCount, md = b.FloatDigitsCount;

                if (md > nd)
                {
                    for (int i = 0; i < md - nd; i++)
                        a.floatDigits.Insert(0, '0');
                    nd = a.FloatDigitsCount;
                }
                else if (nd > md)
                {
                    for (int i = 0; i < nd - md; i++)
                        b.floatDigits.Insert(0, '0');
                    md = b.FloatDigitsCount;
                }

                bool continuesZero = true;
                for (int i = 0; i < nd; i++)
                {
                    int digitSum;
                    digitSum = (a.floatDigits[i] - '0') - (b.floatDigits[i] - '0') + t;

                    if (digitSum < 0)
                    {
                        digitSum += 10;
                        t = -1;
                    }
                    else
                        t = 0;

                    if ((continuesZero && digitSum % 10 != 0) || !continuesZero)
                    {
                        res.floatDigits.Append(digitSum);
                        continuesZero = false;
                    }
                }

                if (res.floatDigits.Length > 0)
                    res.IsFloat = true;
            }

            for (int i = 0; i < ni; i++)
            {
                int digitSum;
                if (i < mi)
                    digitSum = (a.integerDigits[i] - '0') - (b.integerDigits[i] - '0') + t;
                else
                    digitSum = (a.integerDigits[i] - '0') + t;

                if (digitSum < 0)
                {
                    digitSum += 10;
                    t = -1;
                }
                else
                    t = 0;

                res.integerDigits.Append(digitSum);
            }

            if (res.integerDigits[0] == '0')
                res.integerDigits.Remove(0, 1);

            res.sign = a.sign;
            return res;
        }

        // Multiplication of 2 BigNumber - Using "Schonhage-Strassen Algorithm" - O(n * logn * logn) + O(n to less than n^2) for format converting and...
        public static BigNumber Multiply(BigNumber a, BigNumber b)
        {
            BigNumber x = MergeFloatingPointToDecimal(a);
            BigNumber y = MergeFloatingPointToDecimal(b);

            BigNumber res = BigNumber.Zero;
            int size = x.IntegerDigitsCount + y.IntegerDigitsCount - 1;
            short[] linearConvolution = new short[size];

            for (int i = 0; i < x.IntegerDigitsCount; i++)
            {
                for (int j = 0; j < y.IntegerDigitsCount; j++)
                {
                    linearConvolution[i + j] += (short)((x.integerDigits[i] - '0') * (y.integerDigits[j] - '0'));
                }
            }

            int t = 0;
            StringBuilder powerCount = new StringBuilder("");
            for (int i = 0; i < size; i++)
            {
                linearConvolution[i] += (short)t;
                if (i > 0 && linearConvolution[i] % 10 > 0)
                    res = res + ((linearConvolution[i] % 10).ToString() + powerCount);
                else if (linearConvolution[i] % 10 > 0)
                    res = res + ((linearConvolution[i] % 10));

                t = linearConvolution[i] / 10;
                powerCount.Append("0");
            }

            if (t > 0)
                res.integerDigits.Append(t);

            if (a.FloatDigitsCount != 0 || b.FloatDigitsCount != 0)
            {
                res.IsFloat = true;
                res.floatDigits =
                    new StringBuilder(res.integerDigits.ToString(0, a.FloatDigitsCount + b.FloatDigitsCount));
                res.integerDigits.Remove(0, a.FloatDigitsCount + b.FloatDigitsCount);
                if (res.IntegerDigitsCount == 0)
                    res.integerDigits.Append(0);
            }

            while (res.FloatDigitsCount > 0 && res.floatDigits[0] == '0')
            {
                res.floatDigits.Remove(0, 1);
            }

            res.IsFloat = res.FloatDigitsCount > 0 ? true : false;

            return res;
        }

        // Division of 2 BigNumber using "Newton–Raphson division" method for divition
        public static BigNumber Divide(BigNumber a, BigNumber b)
        {
            if (b == 0)
                throw new DivideByZeroException();
            if (Abs(b) == 1)
            {
                a.sign *= b.sign;
                return a;
            }

            BigNumber x = MergeFloatingPointToDecimal(a);
            BigNumber y = MergeFloatingPointToDecimal(b);

            BigNumber res = new BigNumber();
            BigNumber Xn_N = new BigNumber(1);
            BigNumber Xn_D = new BigNumber();

            for (int i = 0; i < y.IntegerDigitsCount; i++)
            {
                Xn_D.integerDigits.Append(0);
            }
            Xn_D.integerDigits.Append(1);


            for (int i = 1; i <= 10; i++)
            {
                Xn_N = ((2 * Xn_D) * Xn_N) - ((y * Xn_N) * Xn_N);
                Xn_D = Xn_D * Xn_D;
            }

            int floatShiftSize = -Xn_D.IntegerDigitsCount + 1 - a.FloatDigitsCount + b.FloatDigitsCount;

            if (floatShiftSize < 0)
            {
                res.integerDigits.Append(0);
                for (int i = 0; i < Xn_N.IntegerDigitsCount; i++)
                {
                    if (Xn_N.integerDigits[i] != 0)
                        res.floatDigits.Append(Xn_N.integerDigits[i]);
                }
                res.IsFloat = true;
                for (int i = 0; i < (-floatShiftSize) - Xn_N.IntegerDigitsCount; i++)
                {
                    res.floatDigits.Append(0);
                }
            }
            else if (floatShiftSize > 0)
            {
                if (floatShiftSize >= Xn_N.IntegerDigitsCount)
                {
                    res.IsFloat = false;
                    for (int i = 0; i < floatShiftSize; i++)
                    {
                        if (i < floatShiftSize - Xn_N.IntegerDigitsCount)
                            res.integerDigits.Append(0);
                        else
                            res.integerDigits.Append(Xn_N.integerDigits[floatShiftSize - (floatShiftSize - i)]);
                    }
                }
                else
                {
                    res.IsFloat = true;
                    for (int i = 0; i < Xn_N.IntegerDigitsCount; i++)
                    {
                        if (i < Xn_N.IntegerDigitsCount - floatShiftSize && Xn_N.integerDigits[i] != 0)
                            res.floatDigits.Append(Xn_N.integerDigits[i]);
                        else
                            res.integerDigits.Append(Xn_N.integerDigits[i]);
                    }
                }
            }
            else
            {
                res.integerDigits = Xn_N.integerDigits;
                res.IsFloat = false;
            }

            res.sign = b.sign;
            res = res * x;
            return res;
        }

        // Power a^n -> for loop ->O(n)
        public static BigNumber Pow(BigNumber a, BigNumber power)
        {
            if (power <= ulong.MaxValue)
            {
                var s = new string(power.integerDigits.ToString().Reverse().ToArray());
                return Pow(a, Convert.ToInt64(s));
            }

            BigNumber res = new BigNumber(1);
            for (BigNumber i = One; i <= power; i++)
            {
                res *= a;
            }
            return res;
        }

        // Power a^n -> Divide and Conquer -> O(log(n))
        public static BigNumber Pow(BigNumber a, long power)
        {
            if (power <= 0)
                return new BigNumber(1);
            if (power == 1)
                return a;

            BigNumber p = Pow(a, power / 2);
            if (power % 2 == 0)
                return p * p;
            else
                return p * p * a;
        }

        // Depends of object -> if it is not BigNumber => O(log(n)) else => O(n)
        public static BigNumber Pow(BigNumber a, object power) => Pow(a, ToBigNumber(power));

        public BigNumber Pow(BigNumber power) => Pow(this, power);
        public BigNumber Pow(long power) => Pow(this, power);
        public BigNumber Pow(object power) => Pow(this, power);

        // Because of Divide function it doesn't work properly
        //public static BigNumber PowLogn(BigNumber a, BigNumber power)
        //{
        //    if (power <= 0)
        //        return new BigNumber(1);
        //    if (power == 1)
        //        return a;
        //    power /= 2;
        //    power.IsFloat = false;
        //    power.floatDigits = null;
        //    BigNumber p = Pow(a, power / 2);
        //    if ((power.integerDigits[0] - '0') % 2 == 0 )
        //        return p * p;
        //    else
        //        return p * p * a;
        //}

        // Max or Min -> O(CompareTo()) -> near O(n)
        public static BigNumber Max(BigNumber a, BigNumber b) => (a > b ? a : b);
        public static BigNumber Min(BigNumber a, BigNumber b) => (a > b ? b : a);

        // |a| -> O(1)
        public static BigNumber Abs(BigNumber a)
        {
            BigNumber t = a;
            if (a.sign == -1)
                t.sign = 1;
            return t;
        }

        // Swap a with b -> O(1) - mostly depends of StringBuilder algorithms
        public static void Swap(ref BigNumber a, ref BigNumber b)
        {
            BigNumber t = a;
            a = b;
            b = t;
        }
        #endregion

        public static BigNumber Zero => new BigNumber(0);
        public static BigNumber One => new BigNumber(1);

        public bool IsNull()
        {
            return (integerDigits.Length == 0);
        }

        public int IntegerDigitsCount => !IsNull() ? integerDigits.Length : 0;
        public int FloatDigitsCount => (!IsNull() && IsFloat) ? floatDigits.Length : 0;

        private static BigNumber MergeFloatingPointToDecimal(BigNumber a)
        {
            var integerNumber = a.sign < 0 ? a.integerDigits.ToString() + "-" : a.integerDigits.ToString();
            var floatNumber = a.IsFloat ? a.floatDigits.ToString() : null;
            string number = a.IsFloat ? floatNumber + integerNumber : integerNumber;
            return !a.IsNull() ? new BigNumber(new string(number.Reverse().ToArray())) : Zero;
        }

        // Converts any number variable entries (using object) to BigNumber - O(n) for ToString method from object class
        public static BigNumber ToBigNumber(object value)
        {
            BigNumber number;
            if (value is sbyte || value is byte || value is short || value is ushort || value is int || value is uint ||
                value is long || value is ulong
                || value is float || value is double || value is decimal || value is string)
            {
                number = new BigNumber(value.ToString().Replace('/', '.'));
            }
            else
            {
                number = BigNumber.Zero;
            }
            return number;
        }

        // Compares current BigNumber object with external object obj - O(n)
        public int CompareTo(object obj)
        {
            if (!(obj is BigNumber other))
                other = ToBigNumber(obj);

            if (sign != other.sign)
                return sign;
            if (this.IntegerDigitsCount > other.IntegerDigitsCount)
                return sign;
            if (this.IntegerDigitsCount < other.IntegerDigitsCount)
                return -sign;

            for (int i = IntegerDigitsCount - 1; i >= 0; i--)
            {
                if (integerDigits[i] > other.integerDigits[i])
                    return sign;
                else if (integerDigits[i] < other.integerDigits[i])
                    return -sign;
            }

            if (IsFloat || other.IsFloat)
            {
                BigNumber t = this;
                if (FloatDigitsCount > other.FloatDigitsCount)
                    Swap(ref t, ref other);

                int maxRange = other.FloatDigitsCount - 1;
                for (int i = t.FloatDigitsCount - 1; i >= 0 || maxRange >= 0; i--)
                {
                    if (floatDigits[i] > other.floatDigits[maxRange])
                        return sign;
                    else if (floatDigits[i] < other.floatDigits[maxRange])
                        return -sign;
                    maxRange--;
                }
            }

            if (FloatDigitsCount > other.FloatDigitsCount)
                return sign;
            else if (FloatDigitsCount < other.FloatDigitsCount)
                return -sign;
            else
                return 0;
        }


        public override bool Equals(object obj)
        {
            if (!(obj is BigNumber other))
                other = ToBigNumber(obj);
            if (this == other)
                return true;
            else
                return false;
        }

        // ToString BigNumber object representation -> O(StringBuilder.ToString()) -> O(n)
        public override string ToString()
        {
            var integerNumber = sign < 0 ? integerDigits.ToString() + "-" : integerDigits.ToString();
            var floatNumber = IsFloat ? floatDigits.ToString() : null;
            string number = IsFloat ? floatNumber + "." + integerNumber : integerNumber;
            return !IsNull() ? new string(number.Reverse().ToArray()) : Zero.ToString();
        }
    }
}
