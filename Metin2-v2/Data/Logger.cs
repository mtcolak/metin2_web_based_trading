namespace Metin2_v2.Data
{
    public static class DataLogging
    {
        internal static ILoggerFactory LoggerFactory { get; set; }// = new LoggerFactory();
        internal static ILogger CreateLogger<T>() => LoggerFactory.CreateLogger<T>();
        internal static ILogger CreateLogger(string categoryName) => LoggerFactory.CreateLogger(categoryName);

        public static void SetLogger(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
        }
    }
}
