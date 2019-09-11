using System;
using System.Text;


namespace TextSteganographyLibrary
{
    public class ZeroWidthConverter
    {
        public static string Embed(string payload, string secret)
        {
            string zeroWidthString = StringToZeroWidth(payload, secret);
            return zeroWidthString + payload;
        }

        private static string StringToZeroWidth(string payload, string secret)
        {
            string[] distinctWords = secret.Trim().Split(' ');

            StringBuilder zeroWidthString = new StringBuilder();

            foreach(string s in distinctWords)
            {
                zeroWidthString.Append(WordToZeroWidth(s));
                zeroWidthString.Append('\uFEFF');
            }

            return zeroWidthString.ToString();

        }

        private static string WordToZeroWidth(string word)
        {
            char[] distinctCharacters = word.ToCharArray();
            StringBuilder zeroWidthWord = new StringBuilder();

            foreach(char c in distinctCharacters)
            {
                zeroWidthWord.Append(CharacterToZeroWidth(c));
                zeroWidthWord.Append('\u200D');
            }

            zeroWidthWord.Remove(zeroWidthWord.Length - 1, 1);

            return zeroWidthWord.ToString();
        }

        private static string CharacterToZeroWidth(char c)
        {
            string characterInBinary = CharacterToBinary(c);

            StringBuilder zeroWidthCharacter = new StringBuilder();
            
            foreach(char bit in characterInBinary)
            {
                if(bit == '1')
                {
                    zeroWidthCharacter.Append('\u200B');
                }
                else if(bit == '0')
                {
                    zeroWidthCharacter.Append('\u200C');
                }
            }

            return zeroWidthCharacter.ToString();
        }

        private static string CharacterToBinary(char c)
        {
            return Convert.ToString(c, 2).PadLeft(8, '0');
        }
    }
}
