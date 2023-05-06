using Metin2_v2.BaseDb;

namespace Metin2_v2.Data
{
    public class Account
    {
        #region List

        public static IList<Account> List()
        {
            return DbBase.Get<Account>("spr_ListAllCharacters");
        }

        #endregion
    }
}
