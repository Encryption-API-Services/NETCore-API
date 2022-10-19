namespace DataLayer.Mongo
{
    public class UserDatabaseSettings : IUserDatabaseSettings
    {
        public string Connection { get; set; }
        public string DatabaseName { get; set; }
        public string UserCollectionName { get; set; }
    }
}
