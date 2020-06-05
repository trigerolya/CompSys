#include <iostream>
#include <bitset>

using namespace std;

int main()
{
   unsigned int dd, dr;
   long long RQ;
   cout << "Enter dividend and divisor" << endl;
   do {
      if(cin.fail())
         {
            cin.clear();
            cin.ignore();
            cout << "You have entered wrong input" << endl;
         }
      cin >> dd >> dr;
   }
   while(cin.fail());

   /*Here problem is that with variables we can't simulate subtracting from a part of register
   (older 32 bits)
   and also can not shift in 32 int so need buffer to add shifted value*/
   long long crutch = dr;
   RQ = dd;
   crutch <<= 32;
   //interface stuff

   cout << "Dividend:\n" << bitset<32>(dd) << endl;
   cout << "Divisor:\n" << bitset<32>(dr) << endl;
   cout << "Remainder(with dividend in right):\n" << bitset<32>(RQ) << endl;
   cout << "*Shifting remainder left*\n";
   RQ <<= 1;
   for (int i = 0; i < 32; i++) {
      RQ -= crutch;
      cout << "*Subtracted divisor from remainder*\n"
      << bitset<64>(RQ) << endl;
      if (RQ < 0) {
         cout << "*Remainder < 0 -- restoring*\nShifting and adding 0\n";
         RQ += crutch;
         RQ <<= 1;
      }
      else {
         cout << "*Remainder > 0 -- Shifting and adding 1\n";
         RQ = (RQ << 1) | 1;
      }
      cout << bitset<64>(RQ) << endl;
   }
   cout << "*Shifting \"left half\" of remainder right*\n";
   unsigned int rema = RQ >> 33;
   unsigned int quo = RQ & UINT32_MAX;
   cout << "Quotient part:\n" <<bitset<32>(quo) <<
   "\nRemainder part:\n" << bitset<32>(rema) << endl;
   cout << "Quotient:\n" << quo << "\nRemainder:\n" << rema;
}
