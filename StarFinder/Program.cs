using System;
using System.Text;
using NBitcoin;

namespace StarFinder
{
    class Program
    {
        public static byte[] StringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            BitcoinSecret privaSecret = new BitcoinSecret("KyS9U4ovbTZ1dCR8GmySCZjX1adutUX6HGsSiYK1w83qqKZ7Thev");
            // should yield 17jhnSGq36gPKNuTx23XH4gVdLfWuMnCBb
            Console.WriteLine(privaSecret.GetAddress(ScriptPubKeyType.Legacy));
        }
    }
}
