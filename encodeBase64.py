import sys, os
import bz2

if __name__ == "__main__":
    try:
        def getBase64Char(bitSequence):
            alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"
            baseIndex = int(bitSequence, 2)

            return alphabet[baseIndex]

        def encode(text):
            binText = ""
            baseBinChar = ""
            res = ""
            pad = "="
            wrapCount = 0

            for char in text:
                binChar = format(ord(char), 'b')

                for i in range(8 - len(format(ord(char), 'b'))):
                    binChar = "0" + binChar
    
                binText += binChar

            print(f"Source text:\n\n\t{text}\n")

            for char in binText:
                if (len(baseBinChar) < 6):
                    baseBinChar += char

                    if (len(baseBinChar) == 6):
                        res += getBase64Char(baseBinChar)
                        baseBinChar = ""
                        wrapCount += 1

                        if (wrapCount == 64):
                            res += "\n"
                            wrapCount = 0
            
            if (len(baseBinChar) > 0):
                missedBitIndex = len(baseBinChar)
                while (missedBitIndex < 6):
                    baseBinChar += "0"
                    missedBitIndex += 1
                
                res += getBase64Char(baseBinChar)

            while ((len(text) % 3) != 0):
                text += " "
                res += pad

            return res

        textFile = sys.argv[1]
        fileName = os.path.basename(textFile)
        print(f"File Name: {fileName}\n")
        resultPath = os.path.abspath(textFile).replace(fileName, "")
        
        #if ("gz" in fileName):
        if (fileName.endswith(".bz2")):
            with bz2.open(textFile, 'rb') as compressedFile:
                inputText = compressedFile.read()
                result = encode(str(inputText))
                print(f"Encoded text:\n\n{result}")

                fileName = fileName.replace(".bz2", "")

                with open(os.path.join(resultPath, f"{fileName}.arc.encoded"), 'w') as encodedFile:
                    encodedFile.write(result)
        else:
            with open(textFile) as analyzedFile:
                inputText = analyzedFile.read()
                result = encode(inputText)
                print(f"Encoded text:\n\n{result}")

                with open(os.path.join(resultPath, f"{fileName}.encoded"), 'w') as encodedFile:
                    encodedFile.write(result)

    except IOError:
        print("Error: file not found")