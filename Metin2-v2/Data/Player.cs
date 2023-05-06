using Metin2_v2.BaseDb;

namespace Metin2_v2.Data
{
    public class Player
    {
        #region List

        public static IList<Player> List()
        {
            return DbBase.Get<Player>("spr_ListAllCharacters");
        }

        #endregion
    }
}
