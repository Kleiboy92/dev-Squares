using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace dev_Squares.Backend
{


    public class ResultContainer : IEnumerable<Result>
    {
        public bool Finished { get; private set; } = false;
        public bool Started { get; private set; } = false;
        public int Length { get { return results.Count; } }

        private ConcurrentQueue<Result> results = new ConcurrentQueue<Result>();

        public ResultContainer()
        {

        }

        public void Add(Result result)
        {
            this.results.Enqueue(result);
        }

        private CancellationTokenSource ct = new CancellationTokenSource();

        public void Start(CancellationTokenSource ct)
        {
            this.Started = true;
            this.Finished = false;
            this.ct = ct;
        }

        public void End()
        {
            this.Started = false;
            this.Finished = true;
        }

        public void Clear()
        {
            this.ct.Cancel();
            this.results = new ConcurrentQueue<Result>();
            Finished = false;
            Started = false;
        }

        public IEnumerator<Result> GetEnumerator()
        {
            return results.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return results.GetEnumerator();
        }
    }
}