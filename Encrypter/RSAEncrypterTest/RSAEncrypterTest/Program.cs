using System;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Enter the encrypted text: ");
        string encryptedText = Console.ReadLine();

        Console.Write("Enter the RSA token (XML format): ");
        string rsaXml = Console.ReadLine();

        try
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

            using (RSA rsa = RSA.Create())
            {
                rsa.FromXmlString(rsaXml);

                byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);
                string decryptedText = Encoding.UTF8.GetString(decryptedBytes);

                Console.WriteLine($"Decrypted Text: {decryptedText}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
