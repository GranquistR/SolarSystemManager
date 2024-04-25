

  const EncryptionModule = (() => {
    let prime: Set<number> = new Set<number>();

    function stringToList(number: string): number[] {
        const digits: number[] = [];
        for (const c of number) {
            if (/\d/.test(c)) {
                digits.push(parseInt(c));
            }
        }
        return digits;
    }

    function listToString(numbers: number[]): string {
        let result = '';
        for (const num of numbers) {
            result += num.toString();
        }
        return result;
    }
    function eRSA(message: string): { coded: number[], privateKey: number, n: number } {
        PrimeFiller();
        const { publicKey, privateKey, n } = SetKeys();
        const coded: number[] = Encoder(message, publicKey, n);
        return { coded, privateKey, n };
    }

    function dRSA(message: number[], key: number, n: number): string {
        const dMessage: string = Decoder(message, key, n);
        return dMessage;
    }

    function PrimeFiller(): void {
        const sieve: boolean[] = Array(250).fill(true);
        sieve[0] = false;
        sieve[1] = false;
        for (let i = 2; i < 250; i++) {
            for (let j = i * 2; j < 250; j += i) {
                sieve[j] = false;
            }
        }
        for (let i = 0; i < sieve.length; i++) {
            if (sieve[i]) {
                prime.add(i);
            }
        }
    }

    function PickRandomPrime(): number {
        const primeArray: number[] = Array.from(prime);
        const k: number = Math.floor(Math.random() * primeArray.length);
        const ret: number = primeArray[k];
        prime.delete(ret);
        return ret;
    }

    function SetKeys(): { publicKey: number, privateKey: number, n: number } {
        const prime1: number = PickRandomPrime();
        const prime2: number = PickRandomPrime();
        let n: number = prime1 * prime2;
        const fi: number = (prime1 - 1) * (prime2 - 1);
        let e: number = 2;
        while (true) {
            if (GCD(e, fi) == 1) {
                break;
            }
            e++;
        }
        const publicKey: number = e;
        let d: number = 2;
        while (true) {
            if ((d * e) % fi == 1) {
                break;
            }
            d++;
        }
        const privateKey: number = d;
        return { publicKey, privateKey, n };
    }

    function Encrypt(message: number, publicKey: number, n: number): number {
        let e: number = publicKey;
        let encrypted_text: number = 1;
        while (e > 0) {
            encrypted_text *= message;
            encrypted_text %= n;
            e--;
        }
        return encrypted_text;
    }

    function Decrypt(encrypted_text: number, privateKey: number, n: number): number {
        let d: number = privateKey;
        let decrypted: number = 1;
        while (d > 0) {
            decrypted *= encrypted_text;
            decrypted %= n;
            d--;
        }
        return decrypted;
    }

    function GCD(a: number, b: number): number {
        while (b != 0) {
            const temp: number = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    function Encoder(message: string, publicKey: number, n: number): number[] {
        const encoded: number[] = [];
        for (const letter of message) {
            encoded.push(Encrypt(letter.charCodeAt(0), publicKey, n));
        }
        return encoded;
    }

    function Decoder(encryptedText: number[], privateKey: number, n: number): string {
        let decrypted: string = "";
        for (const num of encryptedText) {
            const decryptedNum: number = Decrypt(num, privateKey, n);
            decrypted += String.fromCharCode(decryptedNum);
        }
        return decrypted;
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
        eRSA: eRSA,
        encrypt: encrypt,
        generateSalt: generateSalt,
        dRSA: dRSA
    };
  })();
  
  export default EncryptionModule;