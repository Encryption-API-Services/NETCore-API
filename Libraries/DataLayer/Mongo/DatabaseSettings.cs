namespace DataLayer.Mongo
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string Connection { get; set; }
        public string DatabaseName { get; set; }
        public string UserCollectionName { get; set; }
    }
}
