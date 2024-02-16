const EncryptionModule = (function () {
  function encrypt(input: string, salt: string): string {
    function rightRotate(n: number, x: number): number {
      return (x >>> n) | (x << (32 - n));
    }

    function toWordArray(buffer: string): number[] {
      const words: number[] = [];
      for (let i = 0; i < buffer.length * 8; i += 8) {
        words[i >> 5] |= (buffer.charCodeAt(i / 8) & 255) << (24 - (i % 32));
      }
      return words;
    }

    const H: number[] = [
      0x6a09e667, 0xbb67ae85, 0x3c6ef372, 0xa54ff53a,
      0x510e527f, 0x9b05688c, 0x1f83d9ab, 0x5be0cd19
    ];

    const K: number[] = [
      0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5,
      0x3956c25b, 0x59f111f1, 0x923f82a4, 0xab1c5ed5,
      0xd807aa98, 0x12835b01, 0x243185be, 0x550c7dc3,
      0x72be5d74, 0x80deb1fe, 0x9bdc06a7, 0xc19bf174,
      0xe49b69c1, 0xefbe4786, 0x0fc19dc6, 0x240ca1cc,
      0x2de92c6f, 0x4a7484aa, 0x5cb0a9dc, 0x76f988da,
      0x983e5152, 0xa831c66d, 0xb00327c8, 0xbf597fc7,
      0xc6e00bf3, 0xd5a79147, 0x06ca6351, 0x14292967,
      0x27b70a85, 0x2e1b2138, 0x4d2c6dfc, 0x53380d13,
      0x650a7354, 0x766a0abb, 0x81c2c92e, 0x92722c85,
      0xa2bfe8a1, 0xa81a664b, 0xc24b8b70, 0xc76c51a3,
      0xd192e819, 0xd6990624, 0xf40e3585, 0x106aa070,
      0x19a4c116, 0x1e376c08, 0x2748774c, 0x34b0bcb5,
      0x391c0cb3, 0x4ed8aa4a, 0x5b9cca4f, 0x682e6ff3,
      0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208,
      0x90befffa, 0xa4506ceb, 0xbef9a3f7, 0xc67178f2
    ];

    let message = input + salt;
    const blocks: string[] = [];
    const byteLength = message.length * 8;

    message += String.fromCharCode(0x80);

    while (message.length % 64 !== 56) {
      message += String.fromCharCode(0x00);
    }

    message += String.fromCharCode((byteLength >>> 56) & 0xff);
    message += String.fromCharCode((byteLength >>> 48) & 0xff);
    message += String.fromCharCode((byteLength >>> 40) & 0xff);
    message += String.fromCharCode((byteLength >>> 32) & 0xff);
    message += String.fromCharCode((byteLength >>> 24) & 0xff);
    message += String.fromCharCode((byteLength >>> 16) & 0xff);
    message += String.fromCharCode((byteLength >>> 8) & 0xff);
    message += String.fromCharCode(byteLength & 0xff);

    for (let i = 0; i < message.length; i += 64) {
      blocks.push(message.slice(i, i + 64));
    }

    for (let j = 0; j < blocks.length; j++) {
      const block = blocks[j];
      const words = toWordArray(block);

      for (let i = 16; i < 64; i++) {
        const s0 =
          rightRotate(7, words[i - 15]) ^ rightRotate(18, words[i - 15]) ^ (words[i - 15] >>> 3);
        const s1 =
          rightRotate(17, words[i - 2]) ^ rightRotate(19, words[i - 2]) ^ (words[i - 2] >>> 10);
        words[i] = (words[i - 16] + s0 + words[i - 7] + s1) & 0xffffffff;
      }

      let a = H[0], b = H[1], c = H[2], d = H[3], e = H[4], f = H[5], g = H[6], h = H[7];

      for (let i = 0; i < 64; i++) {
        const S1 = rightRotate(6, e) ^ rightRotate(11, e) ^ rightRotate(25, e);
        const ch = (e & f) ^ (~e & g);
        const temp1 = (h + S1 + ch + K[i] + words[i]) & 0xffffffff;
        const S0 = rightRotate(2, a) ^ rightRotate(13, a) ^ rightRotate(22, a);
        const maj = (a & b) ^ (a & c) ^ (b & c);
        const temp2 = (S0 + maj) & 0xffffffff;

        h = g;
        g = f;
        f = e;
        e = (d + temp1) & 0xffffffff;
        d = c;
        c = b;
        b = a;
        a = (temp1 + temp2) & 0xffffffff;
      }

      H[0] = (H[0] + a) & 0xffffffff;
      H[1] = (H[1] + b) & 0xffffffff;
      H[2] = (H[2] + c) & 0xffffffff;
      H[3] = (H[3] + d) & 0xffffffff;
      H[4] = (H[4] + e) & 0xffffffff;
      H[5] = (H[5] + f) & 0xffffffff;
      H[6] = (H[6] + g) & 0xffffffff;
      H[7] = (H[7] + h) & 0xffffffff;
    }

    const hash: string[] = [];
    for (let i = 0; i < H.length; i++) {
      hash.push(((H[i] >> 24) & 0xff).toString(16));
      hash.push(((H[i] >> 16) & 0xff).toString(16));
      hash.push(((H[i] >> 8) & 0xff).toString(16));
      hash.push((H[i] & 0xff).toString(16));
    }

    return hash.join('');
  }

  function generateSalt(length: number): string {
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    let randomString = '';

    for (let i = 0; i < length; i++) {
      const randomIndex = Math.floor(Math.random() * characters.length);
      randomString += characters.charAt(randomIndex);
    }

    return randomString;
  }

  return {
    encrypt: encrypt,
    generateSalt: generateSalt
  }
})();

function test() {
  for (let i: number = 0; i < 10000; ++i) {
  const message: string = `UnGodly1secure!Password3^Boss${i}`;
  const salt: string = EncryptionModule.generateSalt(8);
  const hashing: string = EncryptionModule.encrypt(message, salt);
  console.log(hashing);
  console.log(salt);
  }
}

test();
export default EncryptionModule;
