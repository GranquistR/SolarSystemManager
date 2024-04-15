using System.Text;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
namespace SolarSystemManager.RESTAPI.Controllers
{


public static class EncryptionModule

    {
        private static uint[] SHA256Constants = new uint[] {
        0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5, 0x3956c25b, 0x59f111f1, 0x923f82a4,
        0xab1c5ed5, 0xd807aa98, 0x12835b01, 0x243185be, 0x550c7dc3, 0x72be5d74, 0x80deb1fe,
        0x9bdc06a7, 0xc19bf174, 0xe49b69c1, 0xefbe4786, 0x0fc19dc6, 0x240ca1cc, 0x2de92c6f,
        0x4a7484aa, 0x5cb0a9dc, 0x76f988da, 0x983e5152, 0xa831c66d, 0xb00327c8, 0xbf597fc7,
        0xc6e00bf3, 0xd5a79147, 0x06ca6351, 0x14292967, 0x27b70a85, 0x2e1b2138, 0x4d2c6dfc,
        0x53380d13, 0x650a7354, 0x766a0abb, 0x81c2c92e, 0x92722c85, 0xa2bfe8a1, 0xa81a664b,
        0xc24b8b70, 0xc76c51a3, 0xd192e819, 0xd6990624, 0xf40e3585, 0x106aa070, 0x19a4c116,
        0x1e376c08, 0x2748774c, 0x34b0bcb5, 0x391c0cb3, 0x4ed8aa4a, 0x5b9cca4f, 0x682e6ff3,
        0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208, 0x90befffa, 0xa4506ceb, 0xbef9a3f7, 0xc67178f2
    };
        private static Random rand = new Random(); // Declare Random object outside the method

        private static uint[] InitialHashValues = new uint[] {
        0x6a09e667, 0xbb67ae85, 0x3c6ef372, 0xa54ff53a,
        0x510e527f, 0x9b05688c, 0x1f83d9ab, 0x5be0cd19
    };

        public static uint Encrypt256(string input)
        {
            byte[] message = Encoding.UTF8.GetBytes(input);
            uint[] words = new uint[64];

            ulong len = (ulong)(message.Length * 8);
            int KLen = 64 - (int)((len + 1 + 64) % 64);

            words[0] = 0x80000000;

            for (int i = 0; i < message.Length; i++)
            {
                words[i >> 2] |= (uint)(message[i] << (24 - (i % 4) * 8));
            }

            words[((len + 1 + 64) >> 9 << 4) + 14] = (uint)len;

            uint[] H = new uint[8];
            InitialHashValues.CopyTo(H, 0);

            for (int i = 0; i < words.Length; i += 16)
            {
                uint[] w = new uint[16];
                Array.Copy(words, i, w, 0, 16);

                for (int t = 16; t < 64; t++)
                {
                    uint s0 = RightRotate(7, w[t - 15]) ^ RightRotate(18, w[t - 15]) ^ (w[t - 15] >> 3);
                    uint s1 = RightRotate(17, w[t - 2]) ^ RightRotate(19, w[t - 2]) ^ (w[t - 2] >> 10);
                    w[t] = (uint)(((ulong)w[t - 16] + s0 + w[t - 7] + s1) & 0xFFFFFFFF);
                }

                uint a = H[0], b = H[1], c = H[2], d = H[3], e = H[4], f = H[5], g = H[6], h = H[7];

                for (int t = 0; t < 64; t++)
                {
                    uint S1 = RightRotate(6, e) ^ RightRotate(11, e) ^ RightRotate(25, e);
                    uint ch = (e & f) ^ (~e & g);
                    uint temp1 = (uint)((ulong)h + S1 + ch + SHA256Constants[t] + w[t]);
                    uint S0 = RightRotate(2, a) ^ RightRotate(13, a) ^ RightRotate(22, a);
                    uint maj = (a & b) ^ (a & c) ^ (b & c);
                    uint temp2 = S0 + maj;

                    h = g;
                    g = f;
                    f = e;
                    e = (uint)((ulong)d + temp1);
                    d = c;
                    c = b;
                    b = a;
                    a = temp1 + temp2;
                }

                H[0] = (H[0] + a) & 0xFFFFFFFF;
                H[1] = (H[1] + b) & 0xFFFFFFFF;
                H[2] = (H[2] + c) & 0xFFFFFFFF;
                H[3] = (H[3] + d) & 0xFFFFFFFF;
                H[4] = (H[4] + e) & 0xFFFFFFFF;
                H[5] = (H[5] + f) & 0xFFFFFFFF;
                H[6] = (H[6] + g) & 0xFFFFFFFF;
                H[7] = (H[7] + h) & 0xFFFFFFFF;
            }

            return H[0];
        }

        private static uint RightRotate(int n, uint x)
        {
            return (x >> n) | (x << (32 - n));
        }

        public static (uint publicKey, uint privateKey) GenerateKeyPair(int bits)
        {
            uint p = GeneratePrimeNumber(bits);
            uint q = GeneratePrimeNumber(bits);
            uint n = p * q;
            uint phi = (p - 1) * (q - 1);
            uint e = 2;
            while (e < phi)
            {
                if (GCD(e, phi) == 1)
                {
                    break;
                }
                e++;
            }
            uint d = ModInverse(e, phi);
            return (publicKey: n, privateKey: d);
        }

        public static uint EncryptRSA(uint message, uint publicKey, uint modulus)
        {
            return ModExp(message, publicKey, modulus);
        }

        public static uint DecryptRSA(uint ciphertext, uint privateKey, uint modulus)
        {
            return ModExp(ciphertext, privateKey, modulus);
        }

        public static uint GeneratePrimeNumber(int bits)
        {
            Random rand = new Random();
            uint min = (uint)(1 << (bits - 1));
            uint max = (uint)((1 << bits) - 1);
            while (true)
            {
                uint num = (uint)rand.Next((int)min, (int)max);
                if (IsProbablePrime(num, 20))
                {
                    return num;
                }
            }
        }

        public static bool IsProbablePrime(uint n, int k)
        {
            if (n == 2 || n == 3) return true;
            if (n <= 1 || n % 2 == 0) return false;
            int s = 0;
            uint d = n - 1;
            while (d % 2 == 0)
            {
                s++;
                d /= 2;
            }
            for (int i = 0; i < k; i++)
            {
                uint a = (uint)rand.Next(2, (int)n - 2);
                uint x = ModExp(a, d, n);
                if (x == 1 || x == n - 1) continue;
                for (int r = 1; r < s; r++)
                {
                    x = ModExp(x, 2, n);
                    if (x == 1) return false;
                    if (x == n - 1) break;
                }
                if (x != n - 1) return false;
            }
            return true;
        }

        public static uint GCD(uint a, uint b)
        {
            while (b != 0)
            {
                uint temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public static uint ModInverse(uint a, uint m)
        {
            uint m0 = m;
            uint x0 = 0;
            uint x1 = 1;
            while (a > 1)
            {
                uint q = a / m;
                uint temp = m;
                m = a % m;
                a = temp;
                temp = x0;
                x0 = x1 - q * x0;
                x1 = temp;
            }
            if (x1 < 0) x1 += m0;
            return x1;
        }

        public static uint ModExp(uint baseValue, uint exponent, uint modulus)
        {
            uint result = 1;
            baseValue = baseValue % modulus;
            while (exponent > 0)
            {
                if (exponent % 2 == 1)
                {
                    result = (result * baseValue) % modulus;
                }
                exponent >>= 1;
                baseValue = (baseValue * baseValue) % modulus;
            }
            return result;
        }

        public static string Encrypt(string input, string salt)
        {
            uint hash = Encrypt256(input + salt);
            return hash.ToString("X");
        }

        public static string SignData(string data, uint privateKey, uint modulus)
        {
            uint hash = Encrypt256(data);
            uint signature = EncryptRSA(hash, privateKey, modulus);
            return signature.ToString();
        }

        public static bool VerifySignature(uint encryptedMessage, uint publicKey, uint modulus, string signature)
        {
            uint decryptedMessage = DecryptRSA(encryptedMessage, publicKey, modulus);
            uint decryptedSignature = uint.Parse(signature);
            uint hash = Encrypt256(decryptedMessage.ToString());
            return hash == decryptedSignature;
        }

    }

}
