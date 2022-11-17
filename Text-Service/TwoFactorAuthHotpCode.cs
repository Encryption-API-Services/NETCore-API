using DataLayer.Mongo;

namespace Text_Service
{
    public class TwoFactorAuthHotpCode
    {
        private readonly IDatabaseSettings _databaseSettings;
        public TwoFactorAuthHotpCode(IDatabaseSettings databaseSettings)
        {
            this._databaseSettings = databaseSettings;
        }


    }
}
