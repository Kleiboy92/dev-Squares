using System.Collections.Generic;

namespace dev_Squares.Backend
{
    public class ResultCache
    {
        private readonly Dictionary<string, ResultContainer> resultContainer = new Dictionary<string, ResultContainer>();

        public ResultContainer GetContainer(string sessionId)
        {
            if (!resultContainer.ContainsKey(sessionId))
                resultContainer.Add(sessionId, new ResultContainer());

            return resultContainer[sessionId];
        }
    }
}