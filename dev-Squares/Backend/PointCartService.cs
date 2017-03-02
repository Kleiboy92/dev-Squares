using System;
using System.Threading;
using System.Threading.Tasks;

namespace dev_Squares.Backend
{

    public class PointCartService
    {
        private readonly PointCartContainerRepository pointCartContainerRepository;
        private readonly PointContainerRepository pointContainerRepository;
        private readonly ResultCache resultCache;
        private readonly Func<string> getCurrentUserId;

        public PointCartService(PointCartContainerRepository pointCartContainerRepository, PointContainerRepository pointContainerRepository, ResultCache resultCache, Func<string> getCurrentUserId)
        {
            this.pointCartContainerRepository = pointCartContainerRepository;
            this.pointContainerRepository = pointContainerRepository;
            this.getCurrentUserId = getCurrentUserId;
            this.resultCache = resultCache;
        }

        public PointContainer GetCurrentUserPoints()
        {
            return GetCurrentCart();
        }

        public ResultContainer GetCurrentResultContainer()
        {
            return resultCache.GetContainer(getCurrentUserId());
        }

        public string[] GetSavedPointListNames()
        {
            return pointContainerRepository.GetNames();
        }

        public void Add(int x, int y)
        {
            var point = Point.TryCreate(x, y);
            var updatedCart = GetCurrentCart();
            updatedCart.Add(point);
            SyncCart(updatedCart);
        }

        public Task Solve()
        {
            var ts = new CancellationTokenSource();
            CancellationToken ct = ts.Token;
            return Task.Factory.StartNew(() =>
            {
                var container = resultCache.GetContainer(this.getCurrentUserId());
                container.Clear();

                container.Start(ts);

                foreach (var result in Solver.Solve(GetCurrentCart()))
                {
                    if (ct.IsCancellationRequested)
                        break;

                    container.Add(result);
                    Thread.Sleep(10);
                }

                container.End();
            });
        }

        public void AddFile(string file, Action<ParsingInfo> infoOutput)
        {

            var updatedCart = GetCurrentCart();
            updatedCart.AddRange(PointParser.ParseFile(file, infoOutput), infoOutput);
            SyncCart(updatedCart);
        }

        public void Solve(string file, Action<ParsingInfo> infoOutput)
        {

            var updatedCart = GetCurrentCart();
            updatedCart.AddRange(PointParser.ParseFile(file, infoOutput), infoOutput);
            SyncCart(updatedCart);
        }

        public void Remove(int x, int y)
        {
            var point = Point.TryCreate(x, y);
            var updatedCart = GetCurrentCart();
            updatedCart.Remove(point);
            SyncCart(updatedCart);
        }

        public void Clear()
        {
            var updatedCart = GetCurrentCart();
            updatedCart.Clear();
            SyncCart(updatedCart);
        }

        public void Save(string name)
        {
            pointContainerRepository.Save(name, GetCurrentCart());
        }

        public void Load(string name)
        {
            var updatedCart = GetCurrentCart();
            updatedCart.Clear();
            updatedCart.AddRange(pointContainerRepository.GetByName(name), (x) => { });
            SyncCart(updatedCart);
        }

        public void Delete(string name)
        {
            pointContainerRepository.DeleteByName(name);
        }

        private PointContainer GetCurrentCart()
        {
            return pointCartContainerRepository.GetOrCreateById(getCurrentUserId());
        }

        private void SyncCart(PointContainer cntnrt)
        {
            pointCartContainerRepository.Save(getCurrentUserId(), cntnrt);
        }
    }
}