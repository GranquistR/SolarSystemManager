export default class EncryptedMessage {
    private publicKey: any;
    private data: any;
  
    constructor(publicKey: any, data: any) {
        this.publicKey = publicKey;
        this.data = data;
    }
  
    getPublicKey(): string { return this.publicKey; }
    getData(): string { return this.data; }
  }