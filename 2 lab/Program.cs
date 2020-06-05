using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    class Program
    {
        static int counterOverFlow = 0;
        static bool flag;

        static void Main(string[] args)
        {
            List<byte> a = new List<byte>() { 1,0,0,1,1, 0, 1, 0 };
            List<byte> b = new List<byte>() { 0,0,0,0,0, 1, 0, 1 };
            
            float m = Convert.ToSingle(2.11);
            float k = Convert.ToSingle(5.21);

            Console.WriteLine("\nMultiplication\n");
            Multiply(a, b);
            Console.WriteLine("\nDivision\n");
            Result res = Divide(a, b);
            Console.WriteLine($"RESULT:::: quotient:{ListToString(res.Quotient)} remain:{ListToString(res.Remain)}");
            Console.WriteLine("\nMultiplication of floats\n");
            Console.WriteLine (MultiplyFloat(m, k));

        }


        //a bit bad methods because of containing printing inside
        static void Multiply(List<byte> a, List<byte> b)
        {
            List<byte> register = new List<byte>() { 0, 0, 0, 0 };
            register.AddRange(b);
            foreach (byte B in register)
                Console.Write(B);
            byte last = register[register.Count - 1];
            for (int j = 0; j <= register.Count / 2; j++)
            {
                for (int i = register.Count - 1; i >= 0; i--)
                {
                    if (i == 0) { register[i] = 0; }
                    else register[i] = register[i - 1];
                }
                if (last == 1)
                {
                    Console.WriteLine($"\n ****the last symbol is 1. lets shift right and add to the left part the first multiplicand****");
                    int left = 0;
                    for (int i = a.Count - 1; i >= 0; i--)
                    {
                        if ((a[i] == 0 && register[i] == 1 && left == 0) || (a[i] == 1 && register[i] == 0 && left == 0) || (a[i] == 0 && register[i] == 0 && left == 1))
                        {
                            register[i] = 1;
                            left = 0;
                        }
                        else if ((a[i] == 0 && register[i] == 1 && left == 1) || (a[i] == 1 && register[i] == 0 && left == 1) || (a[i] == 1 && register[i] == 1 && left == 0))
                        {
                            register[i] = 0;
                            left = 1;
                        }
                        else if (a[i] == 0 && register[i] == 0 && left == 0)
                        {
                            register[i] = 0;
                            left = 0;
                        }
                        else
                        {
                            register[i] = 1;
                            left = 1;
                        }
                    }
                }
                else { Console.WriteLine("\n****Nothing to do. Last Symbol in register is 0. Just shift right****"); }
                last = register[register.Count - 1];
                foreach (byte B in register)
                    Console.Write(B);
            }
        }

        static Result Divide(List<byte> numb1, List<byte> numb2)
        {
            Result result = new Result();
            result.Quotient = new List<byte>() { 0 };
            result.Remain = new List<byte>();

            List<byte> numb1InBits = numb1;
            List<byte> numb2InBits = numb2;

            Console.WriteLine($"divided: {ListToString(numb1InBits)} *** divisor: {ListToString(numb2InBits)} *** quotient: {ListToString(result.Quotient)}");

            int counter = 0;
            List<byte> portionOfDivided = new List<byte>();
            for (int i = 0; i < numb2InBits.Count; i++)
            {
                portionOfDivided.Add(numb1InBits[i]);
                counter++;
            }

            do
            {
                if (NumberFromBinaryList(portionOfDivided) >= NumberFromBinaryList(numb2InBits))
                {
                    result.Quotient.Add(1);
                    List<byte> helper = new List<byte>();
                    helper.AddRange(portionOfDivided);
                    portionOfDivided.Clear();
                    portionOfDivided.AddRange(Substract(helper, numb2InBits));
                    if (counter >= numb1InBits.Count)
                    {
                        result.Remain.AddRange(portionOfDivided);
                        break;
                    }
                    portionOfDivided.Add(numb1InBits[counter]);
                    counter++;
                }
                else
                {
                    result.Quotient.Add(0);
                    if (counter >= numb1InBits.Count)
                    {
                        result.Remain.AddRange(portionOfDivided);
                        break;
                    }
                    portionOfDivided.Add(numb1InBits[counter]);
                    counter++;
                }
                RightShift(numb2InBits);

                Console.WriteLine($"divided: {ListToString(numb1InBits)} *** divisor: {ListToString(numb2InBits)} *** quotient: {ListToString(result.Quotient)}");
            }
            while (NumberFromBinaryList(numb1InBits) >= NumberFromBinaryList(numb2InBits));

            return result;
        }
        static float MultiplyFloat(float numb1, float numb2)
        {

            FloatPointNumb first = new FloatPointNumb();
            FloatPointNumb second = new FloatPointNumb();

            FloatPointNumb result = new FloatPointNumb();

            string numb1InBin = FloatToBin(numb1);
            string numb2InBin = FloatToBin(numb2);

            first.Sign = Convert.ToInt32(numb1InBin.Split(' ')[0]);
            first.Mantisa = numb1InBin.Split(' ')[2].Select(c => Int32.Parse(c.ToString())).ToList();
            first.Exponent = numb1InBin.Split(' ')[1].Select(c => Int32.Parse(c.ToString())).ToList();
            Console.WriteLine($"multiplicand *** sign: {first.Sign} *** exponent: {ListToString(first.Exponent)} *** mantisa: {ListToString(first.Mantisa)} ");
            first.Exponent = Subtract(first.Exponent, new List<int>() { 0, 1, 1, 1, 1, 1, 1, 1 });

            second.Sign = Convert.ToInt32(numb2InBin.Split(' ')[0]);
            second.Mantisa = numb2InBin.Split(' ')[2].Select(c => Int32.Parse(c.ToString())).ToList();
            second.Exponent = numb2InBin.Split(' ')[1].Select(c => Int32.Parse(c.ToString())).ToList();
            Console.WriteLine($"multiplier *** sign: {second.Sign} *** exponent: {ListToString(second.Exponent)} *** mantisa: {ListToString(second.Mantisa)}");
            second.Exponent = Subtract(second.Exponent, new List<int>() { 0, 1, 1, 1, 1, 1, 1, 1 });

            

            result.Exponent = AddInBinary(first.Exponent, second.Exponent, 0);
            if (counterOverFlow >= 1 && (first.Sign == 0 && second.Sign == 0))
                result.Exponent = AddInBinary(result.Exponent, new List<int>() { 0, 0, 0, 0, 0, 0, 0, 1 }, 0);
            Console.WriteLine($"counting exponents: {ListToString(result.Exponent)}");


            result.Mantisa = GetProduct(first.Mantisa, second.Mantisa);
            Console.WriteLine($"multiplied mantisas: {ListToString(result.Mantisa)}");

            result.Sign = ((first.Sign == 0 && second.Sign == 0) || (first.Sign == 1 && second.Sign == 1)) ? 0 : 1;
            Console.WriteLine($"setting the sign: {result.Sign}");

            Console.WriteLine($"product *** sign: {result.Sign} *** exponent: {ListToString(result.Exponent)} *** mantisa: {ListToString(result.Mantisa)}");
            result.Exponent = AddInBinary(result.Exponent, new List<int>() { 0, 1, 1, 1, 1, 1, 1, 1 }, 0);
            string resultStr = result.Sign + ListToString(result.Exponent) + ListToString(result.Mantisa);
            float res = BinaryStringToSingle(resultStr);
            return res;
        }
        static List<int> GetProduct(List<int> first, List<int> second)
        {
            List<int> result = new List<int>();
            bool flag = false;

            List<int> help = new List<int>() { 1 };
            help.AddRange(first);
            first.Clear();
            first.AddRange(help);
            help.Clear();
            help.Add(1);
            help.AddRange(second);
            second.Clear();
            second.AddRange(help);

            List<int> helper1 = new List<int>();
            List<int> helper2 = new List<int>();

            if (second[second.Count - 1] == 0)
            {
                helper1.AddRange(new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            }
            else
            {
                helper1.AddRange(first);
            }

            List<int> helper = new List<int>();
            int shift = 1;
            for (int i = second.Count - 2; i >= 0; i--)
            {
                helper2.Clear();
                if (second[i] == 0)
                {
                    helper2.AddRange(new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
                }
                else
                {
                    helper2.AddRange(first);
                }
                helper.AddRange(helper1);
                helper1.Clear();
                shift = helper.Count - helper2.Count + 1;
                helper1.AddRange(AddInBinary(helper, helper2, shift));
                Console.WriteLine($"first term: {ListToString(helper)} *** second term: {ListToString(helper2)} *** sum: {ListToString(helper1)}");
                helper.Clear();
            }

            for (int i = 0; i < helper1.Count; i++)
            {
                while (helper1[0] == 0)
                {
                    helper1.RemoveAt(0);
                }
                helper1.RemoveAt(0);
                break;
            }
            List<int> helperForHelp = new List<int>();
            helperForHelp.AddRange(helper1);
            helper1.Clear();
            for (int i = 0; i < 23; i++)
            {
                helper1.Add(helperForHelp[i]);
            }
            Console.WriteLine($"both mantisa product: {ListToString(helper1)}");

            return helper1;
        }
        static string FloatToBin(float f)
        {
            StringBuilder sb = new StringBuilder();
            Byte[] ba = BitConverter.GetBytes(f);
            foreach (Byte b in ba)
                for (int i = 0; i < 8; i++)
                {
                    sb.Insert(0, ((b >> i) & 1) == 1 ? "1" : "0");
                }
            string s = sb.ToString();
            string r = s.Substring(0, 1) + " " + s.Substring(1, 8) + " " + s.Substring(9);
            return r;
        }

        static void RightShift(List<byte> divisor)
        {
            List<byte> helper = new List<byte>() { 0 };
            helper.AddRange(divisor);
            divisor.Clear();
            divisor.AddRange(helper);
        }
        static float BinaryStringToSingle(string s)
        {
            long i = Convert.ToInt64(s, 2);
            byte[] b = BitConverter.GetBytes(i);
            return BitConverter.ToSingle(b, 0);
        }
        static List<byte> Substract(List<byte> portionOfDivided, List<byte> divisor)
        {
            List<byte> result = new List<byte>();

            List<byte> helper;
            while (divisor[0] == 0)
            {
                divisor.RemoveAt(0);
            }
            while (divisor.Count < portionOfDivided.Count)
            {
                helper = new List<byte>() { 0 };
                helper.AddRange(divisor);
                divisor.Clear();
                divisor.AddRange(helper);
                helper.Clear();
            }

            int i = divisor.Count - 1;
            while (i >= 0)
            {
                if (portionOfDivided[i] == 0 && divisor[i] == 0)
                {
                    result.Add(0);
                }
                else if (portionOfDivided[i] == 1 && divisor[i] == 1)
                {
                    result.Add(0);
                }
                else if (portionOfDivided[i] == 1 && divisor[i] == 0)
                {
                    result.Add(1);
                }
                else
                {
                    for (int j = i - 1; j >= 0; j--)
                    {
                        if (portionOfDivided[j] == 1)
                        {
                            for (int k = j + 1; k <= i; k++)
                            {
                                if (portionOfDivided[k] == 0)
                                    portionOfDivided[k] = 1;
                            }
                            portionOfDivided[j] = 0;
                            result.Add(1);
                            break;
                        }
                    }
                }
                i--;
            }

            result.Reverse();
            while (result.Count != 1 && result[0] == 0)
            {
                result.RemoveAt(0);
            }
            return result;
        }

        static List<int> Subtract(List<int> exponent, List<int> numb)
        {
            List<int> result = new List<int>();

            for (int i = exponent.Count - 1; i >= 0; i--)
            {
                if (exponent[i] == 0 && numb[i] == 0)
                {
                    result.Add(0);
                }
                else if (exponent[i] == 1 && numb[i] == 1)
                {
                    result.Add(0);
                }
                else if (exponent[i] == 1 && numb[i] == 0)
                {
                    result.Add(1);
                }
                else
                {
                    for (int j = i - 1; j >= 0; j--)
                    {
                        if (exponent[j] == 1)
                        {
                            for (int k = j + 1; k <= i; k++)
                            {
                                if (exponent[k] == 0)
                                    exponent[k] = 1;
                            }
                            exponent[j] = 0;
                            result.Add(1);
                            break;
                        }
                    }
                }
            }

            result.Reverse();
            return result;
        }
        static string ListToString(List<byte> list)
        {
            string result = "";
            for (int i = 0; i < list.Count; i++)
            {
                result += list[i];
            }
            return result;
        }
        static string ListToString(List<int> list)
        {
            string result = "";
            for (int i = 0; i < list.Count; i++)
            {
                result += list[i];
            }
            return result;
        }


        static List<int> AddInBinary(List<int> first, List<int> second, int shift)
        {
            List<int> result = new List<int>();

            if (shift > 0 && !Program.flag)
            {
                List<int> help = new List<int>() { 0 };
                help.AddRange(first);
                first.Clear();
                first.AddRange(help);
            }
            Program.flag = false;

            while (shift > 0)
            {
                second.Add(0);
                shift--;
            }

            int mind = 0;
            for (int i = first.Count - 1; i >= 0; i--)
            {
                if (first[i] == 0 && second[i] == 0 && mind == 0)
                {
                    result.Add(0);
                }
                else if ((first[i] == 0 && second[i] == 0 && mind == 1) ||
                        (first[i] == 0 && second[i] == 1 && mind == 0) ||
                        (first[i] == 1 && second[i] == 0 && mind == 0))
                {
                    result.Add(1);
                    mind = 0;
                }
                else if ((first[i] == 0 && second[i] == 1 && mind == 1) ||
                        (first[i] == 1 && second[i] == 0 && mind == 1) ||
                        (first[i] == 1 && second[i] == 1 && mind == 0))
                {
                    result.Add(0);
                    mind = 1;
                }
                else
                {
                    result.Add(1);
                    mind = 1;
                }
            }
            if (mind == 1)
            {
                result.Add(1);
                Program.flag = true;
                Program.counterOverFlow++;
            }

            result.Reverse();
            return result;
        }

        static int NumberFromBinaryList(List<byte> list)
        {
            int result = 0;
            int counter = 0;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i] == 1)
                {
                    result += (int)Math.Pow(2, counter);
                }
                counter++;
            }
            return result;
        }
    }
    class Result
    {
        public List<byte> Quotient { get; set; }
        public List<byte> Remain { get; set; }
    }
    class FloatPointNumb
    {
        public List<int> Exponent { get; set; }
        public List<int> Mantisa { get; set; }
        public int Sign { get; set; }
    }
}
