using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dev_Squares.Backend
{
    public class PointContainerRepository
    {
        const string defaultDb = @"MyDatabase.db";
        public PointContainerRepository(string dbName = defaultDb)
        {
            this.databaseName = dbName;
        }
        private readonly string databaseName;
        public string[] GetNames()
        {
            using (var db = new LiteDatabase(databaseName))
            {
                var pointsCollections = db.GetCollection<PointContainerDTO>(PointContainerDTO.TableName);
                return pointsCollections.FindAll().Select(x => x.Name).ToArray();
            }
        }

        public PointContainer GetByName(string name)
        {
            using (var db = new LiteDatabase(databaseName))
            {
                var pointsCollections = db.GetCollection<PointContainerDTO>(PointContainerDTO.TableName);
                var result = pointsCollections.FindOne(x => x.Name.StartsWith(name));

                var container = new PointContainer();

                foreach (var pointDto in result.Points)
                    container.Add(Point.TryCreate(pointDto.X, pointDto.Y));

                return container;
            }
        }
        public void DeleteByName(string name)
        {
            using (var db = new LiteDatabase(databaseName))
            {
                var pointsCollections = db.GetCollection<PointContainerDTO>(PointContainerDTO.TableName);
                var result = pointsCollections.FindOne(x => x.Name.StartsWith(name));

                if (result != null)
                    pointsCollections.Delete(x => x.Name == name);
            }
        }

        public void Save(string name, PointContainer container)
        {
            using (var db = new LiteDatabase(databaseName))
            {
                var pointsCollections = db.GetCollection<PointContainerDTO>(PointContainerDTO.TableName);
                var result = pointsCollections.FindOne(x => x.Name == name);

                if (result != null)
                    pointsCollections.Delete(x => x.Name == name);

                var containerDto = new PointContainerDTO
                {
                    Points = container.Select(x => new PointDTO(x.X, x.Y)).ToArray(),
                    Name = name
                };

                pointsCollections.Insert(containerDto);
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