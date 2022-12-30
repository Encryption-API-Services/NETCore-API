namespace Models.Encryption
{
    public class RsaDecryptRequest
    {
        public string PublicKey { get; set; }
        public string DataToDecrypt { get; set; }
    }
}
