namespace Models.Encryption
{
    public class DecryptAESRequest
    {
        public string Data { get; set; }
        public string Key { get; set; }
        public string IV { get; set; }
    }
}