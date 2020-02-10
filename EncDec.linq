<Query Kind="Program">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

void Main()
{
	Crypt cr = new Crypt();

	var versch = cr.EncryptString("test", "xyz");
	versch.Dump("Encrypt");

	var ensch = cr.DecryptString(versch, "xyz");
	ensch.Dump("Decrypt");
}

public class Crypt
{
	private static readonly string  key = "b14ca5898a4e4133bbce2ea2315a1916";
	
public string EncryptString(string original, string password)
	{
		try
		{
			byte[] encrypted = EncryptStringToBytes(original, password);
			return Convert.ToBase64String(encrypted);
		}
		catch (Exception ex)
		{
			return ex.ToString();
		}
	}

	public string DecryptString(string original, string password)
	{
		try
		{
			return DecryptStringFromBytes(original, password);
		}
		catch (Exception ex)
		{
			return ex.ToString();
		}
	}



	private byte[] EncryptStringToBytes(string plainText, string password)
	{
		// Check arguments.
		if (plainText == null || plainText.Length <= 0)
			throw new ArgumentNullException("plainText");
		if (password == null || password.Length <= 0)
			throw new ArgumentNullException("password");
		byte[] encrypted;
		// Create an RijndaelManaged object
		// with the specified key and IV.
		using (RijndaelManaged rijAlg = new RijndaelManaged())
		{
			Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(StringToByteArray(password), StringToByteArray(key), 32768);
			rijAlg.Key = pdb.GetBytes(32);
			rijAlg.IV = pdb.GetBytes(16);

			// Create an encryptor to perform the stream transform.
			ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

			// Create the streams used for encryption.
			using (MemoryStream msEncrypt = new MemoryStream())
			{
				using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
				{
					using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
					{

						//Write all data to the stream.
						swEncrypt.Write(plainText);
					}
					encrypted = msEncrypt.ToArray();
				}
			}
		}

		// Return the encrypted bytes from the memory stream.
		return encrypted;
	}

	private string DecryptStringFromBytes(string plainText, string password)
	{
		// Check arguments.
		if (plainText == null || plainText.Length <= 0)
			throw new ArgumentNullException("plainText");
		if (password == null || password.Length <= 0)
			throw new ArgumentNullException("password");

		// Declare the string used to hold
		// the decrypted text.
		string plaintext = null;

		// Create an RijndaelManaged object
		// with the specified key and IV.
		using (RijndaelManaged rijAlg = new RijndaelManaged())
		{
			Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(StringToByteArray(password), StringToByteArray(key), 32768);
			rijAlg.Key = pdb.GetBytes(32);
			rijAlg.IV = pdb.GetBytes(16);

			// Create a decryptor to perform the stream transform.
			ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

			// Create the streams used for decryption.
			using (MemoryStream msDecrypt = new MemoryStream(StringToByteArray(plainText)))
			{
				using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
				{
					using (StreamReader srDecrypt = new StreamReader(csDecrypt))
					{
						// Read the decrypted bytes from the decrypting stream
						// and place them in a string.
						plaintext = srDecrypt.ReadToEnd();
					}
				}
			}
		}

		return plaintext;
	}

}

static byte[] StringToByteArray(string str)
{
	return Encoding.UTF8.GetBytes(str);
}

static string ByteArrayToString(byte[] arr)
{
	return Encoding.UTF8.GetString(arr);
}
