using Metin2_v2.BaseDb;
using Metin2_v2.Models;

namespace Metin2_v2.Data
{
    public static class dbMemberShip_User
    {
        public static int GetUserId(string username, string password)
        {
            return DbBase.ExecuteScalar("spr_GetAccountInformation", new object[] { username, password });
        }
    }
}

