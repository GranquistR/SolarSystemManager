
const EncryptionModule = (() => {
  function encrypt256(input: string): number {
    // SHA-256 constants
    const K: number[] = [
      0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5, 0x3956c25b, 0x59f111f1, 0x923f82a4,
      0xab1c5ed5, 0xd807aa98, 0x12835b01, 0x243185be, 0x550c7dc3, 0x72be5d74, 0x80deb1fe,
      0x9bdc06a7, 0xc19bf174, 0xe49b69c1, 0xefbe4786, 0x0fc19dc6, 0x240ca1cc, 0x2de92c6f,
      0x4a7484aa, 0x5cb0a9dc, 0x76f988da, 0x983e5152, 0xa831c66d, 0xb00327c8, 0xbf597fc7,
      0xc6e00bf3, 0xd5a79147, 0x06ca6351, 0x14292967, 0x27b70a85, 0x2e1b2138, 0x4d2c6dfc,
      0x53380d13, 0x650a7354, 0x766a0abb, 0x81c2c92e, 0x92722c85, 0xa2bfe8a1, 0xa81a664b,
      0xc24b8b70, 0xc76c51a3, 0xd192e819, 0xd6990624, 0xf40e3585, 0x106aa070, 0x19a4c116,
      0x1e376c08, 0x2748774c, 0x34b0bcb5, 0x391c0cb3, 0x4ed8aa4a, 0x5b9cca4f, 0x682e6ff3,
      0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208, 0x90befffa, 0xa4506ceb, 0xbef9a3f7, 0xc67178f2
    ];

    // Initial hash values
    const H: number[] = [
      0x6a09e667, 0xbb67ae85, 0x3c6ef372, 0xa54ff53a,
      0x510e527f, 0x9b05688c, 0x1f83d9ab, 0x5be0cd19
    ];

    // Convert input and salt to UTF-8 encoded bytes
    const message: Uint8Array = new TextEncoder().encode(input);

    // SHA-256 helper functions
    function rightRotate(n: number, x: number): number {
      return (x >>> n) | (x << (32 - n));
    }

    function sha256(message: Uint8Array): Uint32Array {
      const words: Uint32Array = new Uint32Array(64);

      let len = message.length * 8;
      let KLen = 64 - ((len + 1 + 64) % 64);

      words[0] = 0x80000000;

      for (let i = 0; i < message.length; i++) {
        words[i >> 2] |= message[i] << (24 - (i % 4) * 8);
      }

      words[((len + 1 + 64) >> 9 << 4) + 14] = len;

      let [a, b, c, d, e, f, g, h] = [...H];

      for (let i = 0; i < words.length; i += 16) {
        const w = words.slice(i, i + 16);

        for (let t = 16; t < 64; t++) {
          const s0 = rightRotate(7, w[t - 15]) ^ rightRotate(18, w[t - 15]) ^ (w[t - 15] >>> 3);
          const s1 = rightRotate(17, w[t - 2]) ^ rightRotate(19, w[t - 2]) ^ (w[t - 2] >>> 10);
          w[t] = (w[t - 16] + s0 + w[t - 7] + s1) >>> 0;
        }

        for (let t = 0; t < 64; t++) {
          const S1 = rightRotate(6, e) ^ rightRotate(11, e) ^ rightRotate(25, e);
          const ch = (e & f) ^ (~e & g);
          const temp1 = (h + S1 + ch + K[t] + w[t]) >>> 0;
          const S0 = rightRotate(2, a) ^ rightRotate(13, a) ^ rightRotate(22, a);
          const maj = (a & b) ^ (a & c) ^ (b & c);
          const temp2 = (S0 + maj) >>> 0;

          h = g;
          g = f;
          f = e;
          e = (d + temp1) >>> 0;
          d = c;
          c = b;
          b = a;
          a = (temp1 + temp2) >>> 0;
        }

        H[0] = (H[0] + a) >>> 0;
        H[1] = (H[1] + b) >>> 0;
        H[2] = (H[2] + c) >>> 0;
        H[3] = (H[3] + d) >>> 0;
        H[4] = (H[4] + e) >>> 0;
        H[5] = (H[5] + f) >>> 0;
        H[6] = (H[6] + g) >>> 0;
        H[7] = (H[7] + h) >>> 0;
      }

      return new Uint32Array(H);
    }

    // Perform SHA-256 hashing
    const hash: Uint32Array = sha256(message);

    // Convert hash to hexadecimal string
    const hexHash: string[] = Array.from(hash, n => n.toString(16).padStart(8, '0'));
    return Number(hexHash.join(''));
  }
    function generateKeyPair(bits: number): { publicKey: number, privateKey: number } {
        const p = generatePrimeNumber(bits);
        const q = generatePrimeNumber(bits);
        const n = p * q;
        const phi = (p - 1) * (q - 1);
        let e = 2;
        while (e < phi) {
            if (gcd(e, phi) === 1) {
                break;
            }
            e++;
        }
        const d = modInverse(e, phi);
        return { publicKey: n, privateKey: d };
    }

    function encryptRSA(message: number, publicKey: number, modulus: number): number {
        return modExp(message, publicKey, modulus);
    }

    function decryptRSA(ciphertext: number, privateKey: number, modulus: number): number {
        return modExp(ciphertext, privateKey, modulus);
    }

    function generatePrimeNumber(bits: number): number {
        const min = 2 ** (bits - 1);
        const max = (2 ** bits) - 1;
        while (true) {
            const num = getRandomInt(min, max);
            if (isProbablePrime(num, 20)) {
                return num;
            }
        }
    }

    function getRandomInt(min: number, max: number): number {
        return Math.floor(Math.random() * (max - min + 1)) + min;
    }

    function isProbablePrime(n: number, k: number): boolean {
        if (n === 2 || n === 3) return true;
        if (n <= 1 || n % 2 === 0) return false;
        let s = 0;
        let d = n - 1;
        while (d % 2 === 0) {
            s++;
            d /= 2;
        }
        WitnessLoop: for (let i = 0; i < k; i++) {
            const a = getRandomInt(2, n - 2);
            let x = modExp(a, d, n); // Corrected this line
            if (x === 1 || x === n - 1) continue;
            for (let r = 1; r < s; r++) {
                x = (x * x) % n;
                if (x === 1) return false;
                if (x === n - 1) continue WitnessLoop;
            }
            return false;
        }
        return true;
    }

    function gcd(a: number, b: number): number {
        while (b !== 0) {
            const temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    function modInverse(a: number, m: number): number {
        let m0 = m;
        let x0 = 0;
        let x1 = 1;
        while (a > 1) {
            const q = Math.floor(a / m);
            let t = m;
            m = a % m;
            a = t;
            t = x0;
            x0 = x1 - q * x0;
            x1 = t;
        }
        if (x1 < 0) x1 += m0;
        return x1;
    }

    function modExp(base: number, exponent: number, modulus: number): number {
        let result = 1;
        base = base % modulus;
        while (exponent > 0) {
            if (exponent % 2 === 1) {
                result = (result * base) % modulus;
            }
            exponent = Math.floor(exponent / 2);
            base = (base * base) % modulus;
        }
        return result;
    }


    function encrypt(input: string, salt: string): string {
      // SHA-256 constants
      const K: number[] = [
        0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5, 0x3956c25b, 0x59f111f1, 0x923f82a4,
        0xab1c5ed5, 0xd807aa98, 0x12835b01, 0x243185be, 0x550c7dc3, 0x72be5d74, 0x80deb1fe,
        0x9bdc06a7, 0xc19bf174, 0xe49b69c1, 0xefbe4786, 0x0fc19dc6, 0x240ca1cc, 0x2de92c6f,
        0x4a7484aa, 0x5cb0a9dc, 0x76f988da, 0x983e5152, 0xa831c66d, 0xb00327c8, 0xbf597fc7,
        0xc6e00bf3, 0xd5a79147, 0x06ca6351, 0x14292967, 0x27b70a85, 0x2e1b2138, 0x4d2c6dfc,
        0x53380d13, 0x650a7354, 0x766a0abb, 0x81c2c92e, 0x92722c85, 0xa2bfe8a1, 0xa81a664b,
        0xc24b8b70, 0xc76c51a3, 0xd192e819, 0xd6990624, 0xf40e3585, 0x106aa070, 0x19a4c116,
        0x1e376c08, 0x2748774c, 0x34b0bcb5, 0x391c0cb3, 0x4ed8aa4a, 0x5b9cca4f, 0x682e6ff3,
        0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208, 0x90befffa, 0xa4506ceb, 0xbef9a3f7, 0xc67178f2
      ];
  
      // Initial hash values
      const H: number[] = [
        0x6a09e667, 0xbb67ae85, 0x3c6ef372, 0xa54ff53a,
        0x510e527f, 0x9b05688c, 0x1f83d9ab, 0x5be0cd19
      ];
  
      // Convert input and salt to UTF-8 encoded bytes
      const message: Uint8Array = new TextEncoder().encode(input + salt);
  
      // SHA-256 helper functions
      function rightRotate(n: number, x: number): number {
        return (x >>> n) | (x << (32 - n));
      }
  
      function sha256(message: Uint8Array): Uint32Array {
        const words: Uint32Array = new Uint32Array(64);
  
        let len = message.length * 8;
        let KLen = 64 - ((len + 1 + 64) % 64);
  
        words[0] = 0x80000000;
  
        for (let i = 0; i < message.length; i++) {
          words[i >> 2] |= message[i] << (24 - (i % 4) * 8);
        }
  
        words[((len + 1 + 64) >> 9 << 4) + 14] = len;
  
        let [a, b, c, d, e, f, g, h] = [...H];
  
        for (let i = 0; i < words.length; i += 16) {
          const w = words.slice(i, i + 16);
  
          for (let t = 16; t < 64; t++) {
            const s0 = rightRotate(7, w[t - 15]) ^ rightRotate(18, w[t - 15]) ^ (w[t - 15] >>> 3);
            const s1 = rightRotate(17, w[t - 2]) ^ rightRotate(19, w[t - 2]) ^ (w[t - 2] >>> 10);
            w[t] = (w[t - 16] + s0 + w[t - 7] + s1) >>> 0;
          }
  
          for (let t = 0; t < 64; t++) {
            const S1 = rightRotate(6, e) ^ rightRotate(11, e) ^ rightRotate(25, e);
            const ch = (e & f) ^ (~e & g);
            const temp1 = (h + S1 + ch + K[t] + w[t]) >>> 0;
            const S0 = rightRotate(2, a) ^ rightRotate(13, a) ^ rightRotate(22, a);
            const maj = (a & b) ^ (a & c) ^ (b & c);
            const temp2 = (S0 + maj) >>> 0;
  
            h = g;
            g = f;
            f = e;
            e = (d + temp1) >>> 0;
            d = c;
            c = b;
            b = a;
            a = (temp1 + temp2) >>> 0;
          }
  
          H[0] = (H[0] + a) >>> 0;
          H[1] = (H[1] + b) >>> 0;
          H[2] = (H[2] + c) >>> 0;
          H[3] = (H[3] + d) >>> 0;
          H[4] = (H[4] + e) >>> 0;
          H[5] = (H[5] + f) >>> 0;
          H[6] = (H[6] + g) >>> 0;
          H[7] = (H[7] + h) >>> 0;
        }
  
        return new Uint32Array(H);
      }
  
      // Perform SHA-256 hashing
      const hash: Uint32Array = sha256(message);
  
      // Convert hash to hexadecimal string
      const hexHash: string[] = Array.from(hash, n => n.toString(16).padStart(8, '0'));
      return hexHash.join('');
    }
      function signData(data: string, privateKey: number, modulus: number): string {
        // Hash data using SHA-256
        const hash = encrypt256(data);

        // Encrypt the hash using RSA with the private key
        const signature = encryptRSA(hash, privateKey, modulus);

        return signature.toString(); // Convert to string for easy transmission
    }

    function verifySignature(encryptedMessage: number, publicKey: number, modulus: number, signature: string): boolean {
      // Decrypt the encrypted message using the public key
      const decryptedMessage = decryptRSA(encryptedMessage, publicKey, modulus);

      // Decrypt the signature using the public key
      const decryptedSignature = decryptRSA(parseInt(signature), publicKey, modulus);

      // Hash the decrypted message
      const hash = encrypt256(decryptedMessage.toString());

      // Compare the decrypted signature with the hashed message
      if (hash === decryptedSignature) {
        return true;
      } else {
        return false;
      }
  }

    function generateSalt(length: number): string {
      const characters: string = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
      let randomString: string = '';
  
      for (let i = 0; i < length; i++) {
        const randomIndex: number = Math.floor(Math.random() * characters.length);
        randomString += characters.charAt(randomIndex);
      }
  
      return randomString;
    }
  
    return {
        encryptRSA: encryptRSA,
        decryptRSA: decryptRSA,
        generateKeyPair: generateKeyPair,
        encrypt: encrypt,
        generateSalt: generateSalt,
        signData: signData,
        verifySignature: verifySignature
    };
  })();
  
  export default EncryptionModule;