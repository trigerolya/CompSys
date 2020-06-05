def binaryAddition(a, b):
        print("\n\t\tADDITION:\n")
        diff = len(a)-len(b)

        if (diff > 0):
            b = ([0,] * diff) + b   
        elif (diff < 0):
            a = ([0,] * (-diff)) + a 

        print(f"\t\tTerm 'a': {a}",
              f"\n\t\tTerm 'b': {b}\n")

        carry = 0
        sum = [0] * len(a)

        for i in reversed(range(len(a))):
                d = (a[i] + b[i] + carry) // 2
                sum[i] = (a[i] + b[i] + carry) - 2*d
                carry = d
                # print(f"\t\t+Partial sum: {sum}     Carry: {carry}")
        
        if (carry == 1):
            sum = [carry] + sum

        print(f"\n\t\t+Final sum:   {sum}")
        return sum

def binarySubtraction(a, b):
        print("\n\t\tSUBTRACTION:\n")
        diff = len(a)-len(b)

        if (diff > 0):
            b = ([0,] * diff) + b   
        elif (diff < 0):
            a = ([0,] * (-diff)) + a 

        print(f"\t\tTerm 'a': {a}",
              f"\n\t\tTerm 'b': {b}\n")

        sign = 0
        subtr = [0,] * len(a)
        carry = 0

        for i in reversed(range(len(a))):
            d = a[i] - b[i]
            if d <= 0:
                if (carry == 0 and d != 0):
                    carry = 1
                    subtr[i] = 1
                elif (carry == 1 and d == 0):
                    subtr[i] = 1
                elif (carry == 1 and d != 0):
                    subtr[i] = 0
                else:
                    subtr[i] = 0
            else:
                if (carry == 1):
                    carry = 0
                    subtr[i] = 0
                else:
                    subtr[i] = 1

        if (carry == 1):
            sign = 1                        

        return subtr, sign

def shiftRight(reg):
        return [0,] + reg

def shiftLeft(reg, value = 0):
        return reg + [value]

def binaryMultiplication(multiplicand, multiplier):
        print("MULTIPLICATION:\n")
        register = [0,] * 8
        halfRegisterSize = int(len(register) / 2)
        multiplicand = [0,] * (halfRegisterSize - len(multiplicand)) + multiplicand
        multiplier = [0,] * (halfRegisterSize - len(multiplier)) + multiplier
        print(f'\tMultiplicand: {multiplicand}',
              f'\n\tMultiplier:   {multiplier}\n')
        print(f"\t*Register: {register}")
        print(f"\t+Add multiplier to register")
        register = binaryAddition(register, multiplier)

        iteration = 0

        while (iteration <= len(multiplicand)):
            currentBit = register.pop()
            print("\t*Shift register right")
            print(f"\tPop bit: {currentBit}")
            register = shiftRight(register)
            print(f"\t*Register: {register}")

            if (currentBit == 1):
                print("\t+Add multiplicand to first half of register")
                register = binaryAddition(register[:halfRegisterSize], multiplicand) + register[halfRegisterSize:]
                print(f"\t*Register: {register}")

            iteration += 1

        return register

if __name__ == "__main__":
        multiplicationRes = binaryMultiplication([1,1,1,1], [1,1,1,1])
        print(f"\n=RESULT: {multiplicationRes}\n")