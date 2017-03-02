using LiteDB;
using System;
using System.Linq;

namespace dev_Squares.Backend
{
    public class PointCartContainerRepository
    {
        const string defaultDb = @"MyDatabase.db";
        public PointCartContainerRepository(string dbName = defaultDb)
        {
            this.databaseName = dbName;
        }
        private readonly string databaseName;
      
        public PointContainer GetOrCreateById(string userId)
        {
            using (var db = new LiteDatabase(databaseName))
            {
                var pointsCollections = db.GetCollection<PointCartContainerDTO>(PointCartContainerDTO.TableName);
                PointCartContainerDTO result = null;

                var search = pointsCollections.Find(x => x.UserId.Equals(userId));

                if (search.Any())
                {
                    result = search.Single();
                }
                else
                {
                    result = new PointCartContainerDTO { DateCreated = DateTime.UtcNow, Points = new PointDTO[] { }, UserId = userId };
                    pointsCollections.Insert(result);
                }

                var container = new PointContainer();

                foreach (var pointDto in result.Points)
                    container.Add(Point.TryCreate(pointDto.X, pointDto.Y));

                return container;
            }
        }

        public void Save(string userID, PointContainer container)
        {
            using (var db = new LiteDatabase(databaseName))
            {
                var pointsCollections = db.GetCollection<PointCartContainerDTO>(PointCartContainerDTO.TableName);
                var pointContainerEntity = pointsCollections.FindOne(x => x.UserId == userID);

                if (pointContainerEntity == null)
                {
                    pointContainerEntity = new PointCartContainerDTO { DateCreated = DateTime.UtcNow, UserId = userID };
                    pointsCollections.Insert(pointContainerEntity);
                }

                pointContainerEntity.Points = container.Select(x => new PointDTO(x.X, x.Y)).ToArray();

                pointsCollections.Update(pointContainerEntity);
            }
        }

        public void CleanAllDb()
        {
            using (var db = new LiteDatabase(databaseName))
            {
                var pointsCollections = db.DropCollection(PointContainerDTO.TableName);
            }
        }
    }
}