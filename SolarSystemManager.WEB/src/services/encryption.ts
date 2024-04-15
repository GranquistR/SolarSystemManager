const EncryptionModule = (() => {
  // Generate AES key
  function generateAESKey(): string {
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    let key = '';
    for (let i = 0; i < 32; i++) {
      const randomIndex = Math.floor(Math.random() * characters.length);
      key += characters.charAt(randomIndex);
    }
    return key;
  }
  function encryptAES(input: string, key: string): string {
    // Convert input and key to Uint8Array
    const inputBytes = new TextEncoder().encode(input);
    const keyBytes = new TextEncoder().encode(key);

    // Key Expansion
    const expandedKey = keyExpansion(keyBytes);

    // Initial Round
    addRoundKey(inputBytes, keyBytes);

    // Main Rounds
    for (let round = 1; round < expandedKey.length / 16 - 1; round++) {
        subBytes(inputBytes);
        shiftRows(inputBytes);
        mixColumns(inputBytes);
        addRoundKey(inputBytes, expandedKey.subarray(round * 16, (round + 1) * 16));
    }

    // Final Round
    subBytes(inputBytes);
    shiftRows(inputBytes);
    addRoundKey(inputBytes, expandedKey.subarray(expandedKey.length - 16));

    // Convert encrypted bytes to hexadecimal string
    const encryptedHex = Array.from(inputBytes, byte => ('0' + (byte & 0xFF).toString(16)).slice(-2)).join('');

    return encryptedHex;
}

// Key Expansion
function keyExpansion(key: Uint8Array): Uint8Array {
    const expandedKey = new Uint8Array(176);
    expandedKey.set(key);

    let temp = new Uint8Array(4);

    for (let i = 16; i < 176; i += 4) {
        temp.set(expandedKey.subarray(i - 4, i));

        if (i % 16 === 0) {
            // RotWord
            const t = temp[0];
            temp[0] = temp[1];
            temp[1] = temp[2];
            temp[2] = temp[3];
            temp[3] = t;

            // SubWord
            for (let j = 0; j < 4; j++) {
                temp[j] = sBox[temp[j]];
            }

            temp[0] ^= rCon[i / 16];
        }

        for (let j = 0; j < 4; j++) {
            expandedKey[i + j] = expandedKey[i - 16 + j] ^ temp[j];
        }
    }

    return expandedKey;
}

// SubBytes
function subBytes(state: Uint8Array) {
    for (let i = 0; i < state.length; i++) {
        state[i] = sBox[state[i]];
    }
}

// ShiftRows
function shiftRows(state: Uint8Array) {
    for (let i = 1; i < 4; i++) {
        const temp = state.slice(i * 4, i * 4 + i);
        state.set(state.slice(i * 4 + i, i * 4 + 4), i * 4);
        state.set(temp, i * 4 + 4 - i);
    }
}

// MixColumns
function mixColumns(state: Uint8Array) {
    for (let i = 0; i < 4; i++) {
        const s0 = state[i];
        const s1 = state[i + 4];
        const s2 = state[i + 8];
        const s3 = state[i + 12];

        state[i] = mul(0x02, s0) ^ mul(0x03, s1) ^ s2 ^ s3;
        state[i + 4] = s0 ^ mul(0x02, s1) ^ mul(0x03, s2) ^ s3;
        state[i + 8] = s0 ^ s1 ^ mul(0x02, s2) ^ mul(0x03, s3);
        state[i + 12] = mul(0x03, s0) ^ s1 ^ s2 ^ mul(0x02, s3);
    }
}

// AddRoundKey
function addRoundKey(state: Uint8Array, roundKey: Uint8Array) {
    for (let i = 0; i < 16; i++) {
        state[i] ^= roundKey[i];
    }
}

// Multiplication in Galois Field
function mul(a: number, b: number) {
    let result = 0;
    while (b) {
        if (b & 0x01) {
            result ^= a;
        }
        a <<= 1;
        if (a & 0x0100) {
            a ^= 0x011b;
        }
        b >>= 1;
    }
    return result;
}

// S-box Lookup Table
const sBox = [
    0x63, 0x7c, 0x77, 0x7b, 0xf2, 0x6b, 0x6f, 0xc5, 0x30, 0x01, 0x67, 0x2b, 0xfe, 0xd7, 0xab, 0x76,
    0xca, 0x82, 0xc9, 0x7d, 0xfa, 0x59, 0x47, 0xf0, 0xad, 0xd4, 0xa2, 0xaf, 0x9c, 0xa4, 0x72, 0xc0,
    0xb7, 0xfd, 0x93, 0x26, 0x36, 0x3f, 0xf7, 0xcc, 0x34, 0xa5, 0xe5, 0xf1, 0x71, 0xd8, 0x31, 0x15,
    0x04, 0xc7, 0x23, 0xc3, 0x18, 0x96, 0x05, 0x9a, 0x07, 0x12, 0x80, 0xe2, 0xeb, 0x27, 0xb2, 0x75,
    0x09, 0x83, 0x2c, 0x1a, 0x1b, 0x6e, 0x5a, 0xa0, 0x52, 0x3b, 0xd6, 0xb3, 0x29, 0xe3, 0x2f, 0x84,
    0x53, 0xd1, 0x00, 0xed, 0x20, 0xfc, 0xb1, 0x5b, 0x6a, 0xcb, 0xbe, 0x39, 0x4a, 0x4c, 0x58, 0xcf,
    0xd0, 0xef, 0xaa, 0xfb, 0x43, 0x4d, 0x33, 0x85, 0x45, 0xf9, 0x02, 0x7f, 0x50, 0x3c, 0x9f, 0xa8,
    0x51, 0xa3, 0x40, 0x8f, 0x92, 0x9d, 0x38, 0xf5, 0xbc, 0xb6, 0xda, 0x21, 0x10, 0xff, 0xf3, 0xd2,
    0xcd, 0x0c, 0x13, 0xec, 0x5f, 0x97, 0x44, 0x17, 0xc4, 0xa7, 0x7e, 0x3d, 0x64, 0x5d, 0x19, 0x73,
    0x60, 0x81, 0x4f, 0xdc, 0x22, 0x2a, 0x90, 0x88, 0x46, 0xee, 0xb8, 0x14, 0xde, 0x5e, 0x0b, 0xdb,
    0xe0, 0x32, 0x3a, 0x0a, 0x49, 0x06, 0x24, 0x5c, 0xc2, 0xd3, 0xac, 0x62, 0x91, 0x95, 0xe4, 0x79,
    0xe7, 0xc8, 0x37, 0x6d, 0x8d, 0xd5, 0x4e, 0xa9, 0x6c, 0x56, 0xf4, 0xea, 0x65, 0x7a, 0xae, 0x08,
    0xba, 0x78, 0x25, 0x2e, 0x1c, 0xa6, 0xb4, 0xc6, 0xe8, 0xdd, 0x74, 0x1f, 0x4b, 0xbd, 0x8b, 0x8a,
    0x70, 0x3e, 0xb5, 0x66, 0x48, 0x03, 0xf6, 0x0e, 0x61, 0x35, 0x57, 0xb9, 0x86, 0xc1, 0x1d, 0x9e,
    0xe1, 0xf8, 0x98, 0x11, 0x69, 0xd9, 0x8e, 0x94, 0x9b, 0x1e, 0x87, 0xe9, 0xce, 0x55, 0x28, 0xdf,
    0x8c, 0xa1, 0x89, 0x0d, 0xbf, 0xe6, 0x42, 0x68, 0x41, 0x99, 0x2d, 0x0f, 0xb0, 0x54, 0xbb, 0x16
];

// Round Constants
const rCon = [
    0x00, 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80, 0x1b, 0x36
];

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
        encryptAES: encryptAES,
        encryptRSA: encryptRSA,
        decryptRSA: decryptRSA,
        generateAESKey: generateAESKey,
        generateKeyPair: generateKeyPair,
        encrypt: encrypt,
        generateSalt: generateSalt,
        signData: signData,
        verifySignature: verifySignature
    };
  })();
  
  export default EncryptionModule;