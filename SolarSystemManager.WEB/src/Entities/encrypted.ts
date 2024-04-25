export default class EncryptedMessage {
    private message: number[];
    private key: number;
    private n: number;
  
    constructor(message: number[], key: number, n: number) {
        this.key = key;
        this.message = message;
        this.n = n;
    }
    getMessage(): number[] { return this.message; }
    getKey(): number { return this.key; }
    getN(): number { return this.n; }
  }