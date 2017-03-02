namespace dev_Squares.Backend
{

    static class ResultCacheProvider
    {
        private static ResultCache cache;

        static public ResultCache Get()
        {
            if (cache == null)
                cache = new ResultCache();

            return cache;
        }
    }
}