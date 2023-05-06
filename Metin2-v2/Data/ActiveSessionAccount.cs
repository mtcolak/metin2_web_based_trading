namespace Metin2_v2.Data
{
    #region Active Account

    public class ActiveAccount
    {
        public ActiveAccount()
        {
            id = -1;
            login = "";
        }

        public override string ToString()
        {
            return $"Id = {id}, login = {login}";
        }

        public int id { get; set; }
        public string login { get; set; }
    }

    #endregion
}
