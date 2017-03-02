using dev_Squares.Backend;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using System.Text;

namespace dev_Squares.Controllers
{
    public class PointsController: ApiController
    {
        private readonly PointCartService service;
        public PointsController()
        {
            this.service = new PointCartService(new PointCartContainerRepository(), new PointContainerRepository(), ResultCacheProvider.Get(), GetSessionId);
        }

        [Route("api/Points/Lists")]
        public string[] GetSavedNames()
        {
            var pointNames = service.GetSavedPointListNames();
            return pointNames;
        }

        [Route("api/Points/Restrictions")]
        [HttpGet]
        public object GetRestrictions()
        {
            return new { max = Restrictions.maxConstraint, min = Restrictions.minConstraint, size = Restrictions.maxSize } ;
        }

        [HttpGet]
        [Route("api/Points/GetPoints/{pageSize}/{pageNumber}")]
        public IHttpActionResult GetPoints(int pageSize, int pageNumber)
        {
            var points = service.GetCurrentUserPoints();
            var totalPages = Math.Ceiling((double)points.Length / pageSize);

            var takePoints = points.Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();

            var result = new
            {
                TotalCount = points.Length,
                totalPages = totalPages,
                results = takePoints
            };

            return Ok(result);
        }

        [HttpGet]
        [Route("api/Points/GetResults/{pageSize}/{pageNumber}")]
        public HttpResponseMessage GetResult(HttpRequestMessage request, int pageSize, int pageNumber)
        {
            var results = service.GetCurrentResultContainer();
            var finished = results.Finished;
            var started = results.Started;

            var totalPages = Math.Ceiling((double)results.Length / pageSize);

            var takePoints = results.Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();

            var result = new
            {
                TotalCount = results.Length,
                totalPages = totalPages,
                results = takePoints,
            };

            if (finished)
                return request.CreateResponse(HttpStatusCode.OK, result);
            else 
                return request.CreateResponse(HttpStatusCode.Created, result);

        }

        [HttpGet]
        [Route("api/Points/Save/{name}")]
        public IHttpActionResult Save(string name) 
        {
            service.Save(name);
            return Ok();
        }

        [HttpGet]
        [Route("api/Points/Load/{name}")]
        public IHttpActionResult Load(string name)
        {
            service.Load(name);
            return Ok();
        }

        [HttpGet]
        [Route("api/Points/Delete/{name}")]
        public IHttpActionResult Delete(string name)
        {
            service.Delete(name);
            return Ok();
        }

        [HttpGet]
        [Route("api/Points/Solve")]
        public IHttpActionResult Solve()
        {
            Task.Factory.StartNew(service.Solve);
            return Ok();
        }

        [HttpGet]
        [Route("api/Points/Clear")]
        public IHttpActionResult Clear()
        {
            service.Clear();
            return Ok();
        }

        [Route("api/Points/Upload")]
        [HttpPost]
        public IHttpActionResult Upload()
        {
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var parsingInfo = new List<ParsingInfo>();
            var provider = new MultipartMemoryStreamProvider();
            Request.Content.ReadAsMultipartAsync(provider).Wait();

            foreach (var file in provider.Contents)
            {
                var read =  file.ReadAsByteArrayAsync();
                read.Wait();
                var fileAsString = System.Text.Encoding.ASCII.GetString(read.Result);
                service.AddFile(fileAsString, parsingInfo.Add);
            }

            return Ok(parsingInfo.ToArray());
        }


        [HttpGet]
        [Route("api/Points/Add/{x}/{y}")]
        public IHttpActionResult Add(int x, int y)
        {
            var isSuccess = true;
            var response = "point added succesfully";
            try
            {
                service.Add(x, y);
            }
            catch(Exception e)
            {
                response = e.Message;
                isSuccess = false;
            }
            if (isSuccess)
                return Ok(response);
            else
                return InternalServerError(new Exception(response));

        }

        [HttpGet]
        [Route("api/Points/Remove/{x}/{y}")]
        public IHttpActionResult Remove(int x, int y)
        {
            var isSuccess = true;
            var response = "point removed succesfully";
            try
            {
                service.Remove(x, y);
            }
            catch (Exception e)
            {
                response = e.Message;
                isSuccess = false;
            }
            if (isSuccess)
                return Ok(response);
            else
                return InternalServerError(new Exception(response));

        }

        [HttpGet]
        [Route("api/Points/Download")]
        public HttpResponseMessage Download()
        {
            var result = Request.CreateResponse(HttpStatusCode.OK);
            var rows = service.GetCurrentUserPoints().Select(x => x.X.ToString() + " " + x.Y.ToString());
            result.Content = new StringContent(string.Join(Environment.NewLine, rows), Encoding.UTF8, "text/plain");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "points.txt"
            };

            return result;
        }

        public const string UserSessionID = "UserSessionId";
        string GetSessionId()
        {
            return this.Request.Headers.GetCookies(UserSessionID).First().Cookies.First().Value;
        }
    }
}
