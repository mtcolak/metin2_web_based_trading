namespace Metin2_v2.Data
{
    public static class dbParameters
    {
        #region dsnStore

        private static string _dsnStore = "";
        private static int _commandTimeOut = 0;

        public static string dsnStore
        {
            get
            {
                return _dsnStore;
            }
            set
            {
                _dsnStore = value;
            }
        }

        //todo start up da set edilcek config den okunup
        public static int commandTimeOut
        {
            get
            {
                return _commandTimeOut;
            }
            set
            {
                _commandTimeOut = value;
            }
        }

        #endregion
    }
}
