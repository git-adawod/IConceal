using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextSteganographyLibrary
{
    public class ZeroWidthExtractor
    {
        public static string Extract(string steganoText)
        {
            StringBuilder zeroWidthString = new StringBuilder();

            for(int i = 0; i < steganoText.Length; i++)
            {
                switch (steganoText[i])
                {
                    case '\u200B':
                        zeroWidthString.Append(steganoText[i]);
                        break;
                    case '\u200C':
                        zeroWidthString.Append(steganoText[i]);
                        break;
                    case '\u200D':
                        zeroWidthString.Append(steganoText[i]);
                        break;
                    case '\uFEFF':
                        zeroWidthString.Append(steganoText[i]);
                        break;
                }
            }

            return ZeroWidthToString(zeroWidthString.ToString());
        }

        private static string ZeroWidthToString(string zeroWidthString)
        {
            StringBuilder decipheredWords = new StringBuilder();
            string[] distinctWords = zeroWidthString.Split('\uFEFF');


            for(int i = 0; i < distinctWords.Length - 1; i++)
            {
                decipheredWords.Append(ZeroWidthToWord(distinctWords[i]));
            }

            decipheredWords.Remove(decipheredWords.Length - 1, 1);

            return decipheredWords.ToString();
        }

        private static string ZeroWidthToWord(string word)
        {
            string[] distinctCharacters = word.Split('\u200D');
            StringBuilder decipheredWord = new StringBuilder();

            for(int i = 0; i < distinctCharacters.Length; i++)
            {
                decipheredWord.Append(ZeroWidthToCharacter(distinctCharacters[i]));
            }

            decipheredWord.Append(' ');

            return decipheredWord.ToString();
        }

        private static char ZeroWidthToCharacter(string zeroWidthLetter)
        {
            StringBuilder letterInBinary = new StringBuilder();

            for(int i = 0; i < zeroWidthLetter.Length; i++)
            {
                if (zeroWidthLetter[i] == '\u200B')
                {
                    letterInBinary.Append('1');
                }
                else if (zeroWidthLetter[i] == '\u200C')
                {
                    letterInBinary.Append('0');
                }
            }

            return BinaryToCharacter(letterInBinary.ToString());
        }

        private static char BinaryToCharacter(string characterInBinary)
        {
            List<byte> bytes = new List<byte>();
            bytes.Add(Convert.ToByte(characterInBinary.Substring(0, 8), 2));
            return Encoding.ASCII.GetString(bytes.ToArray())[0];
        }
    }
}
