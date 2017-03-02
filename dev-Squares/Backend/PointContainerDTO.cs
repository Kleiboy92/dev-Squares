using LiteDB;
using System;

namespace dev_Squares.Backend
{

    public class PointContainerDTO
    {
        public static readonly string TableName = "PointsContainer";
        public PointContainerDTO()
        {

        }

        [BsonId]
        public string Name { get; set; }
        public PointDTO[] Points { get; set; }
    }

    public class PointCartContainerDTO
    {
        public static readonly string TableName = "PointsCartsContainer";
        public PointCartContainerDTO()
        {

        }

        [BsonId]
        public string UserId { get; set; }

        public DateTime DateCreated { get; set; }

        public PointDTO[] Points { get; set; }
    }
}