using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using System.Security.Cryptography;
using System.Text;

namespace Task3;

public class SHA3_256
{
	public static byte[] GenerateRandomKey(int length)
	{
		byte[] key = new byte[length];

		using (var rng = RandomNumberGenerator.Create())
		{
			rng.GetBytes(key);
		}

		return key;
	}

	public static string ComputeHmacSha3_256(string move, byte[] keyBytes)
	{
		byte[] moveBytes = Encoding.UTF8.GetBytes(move);

		HMac hmac = new(new Sha3Digest(256));
		hmac.Init(new KeyParameter(keyBytes));

		byte[] result = new byte[hmac.GetMacSize()];
		
		hmac.BlockUpdate(moveBytes, 0, moveBytes.Length);
		hmac.DoFinal(result, 0);

		StringBuilder sb = new (result.Length * 2);
		
		foreach (byte b in result)
		{
			sb.Append(b.ToString("x2"));
		}

		return sb.ToString();
	}

	public static string ComputeHmacSha3_256(byte[] keyBytes)
	{
		Sha3Digest sha3Digest = new (256);

		byte[] result = new byte[sha3Digest.GetDigestSize()];
		
		sha3Digest.BlockUpdate(keyBytes, 0, keyBytes.Length);
		sha3Digest.DoFinal(result, 0);

		StringBuilder sb = new (result.Length * 2);

		foreach (byte b in result)
		{
			sb.Append(b.ToString("x2"));
		}

		return sb.ToString();
	}
}
