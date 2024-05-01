export default class Keys {
    private publicKey: any;
    private privateKey: any;
  
    constructor(publicKey: any, privateKey: any) {
        this.publicKey = publicKey;
        this.privateKey = privateKey;
    }
  
    getPublicKey(): string { return this.publicKey; }
    getPrivateKey(): string { return this.privateKey; }
  }
  