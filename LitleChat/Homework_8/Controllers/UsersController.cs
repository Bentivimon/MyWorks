using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Homework_8.Models;
using Homework_8.Abstract;

namespace Homework_8.Controllers
{
    public class UsersController : ApiController
    {
        // GET: api/Pokemons
        public UsersController(IDataManager dataManager)
        {
            _dataManager = dataManager;
        }

        // GET: api/Pokemons
        public IEnumerable<User> Get()
        {
            return _dataManager.GetUsers();
        }

        // GET: api/Pokemons/5
        public IHttpActionResult Get(int id)
        {
            if (id < 0 || id > _dataManager.GetUsers().Count())
            {
                return BadRequest();
            }
            return Json(_dataManager.GetUsers().FirstOrDefault(x => x.Id == id));
        }

        // POST: api/Pokemons
        public HttpResponseMessage Post(int id, [FromBody]string fName, [FromBody]string lName, [FromBody]int age)
        {
            if (id < _dataManager.GetUsers().Count() || String.IsNullOrWhiteSpace(fName) || String.IsNullOrWhiteSpace(lName))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            if (_dataManager.GetUsers().FirstOrDefault(x => x.Id == id) != null)
            {
                var response = new HttpResponseMessage
                {
                    Content = new StringContent("There is pokemon with such Id")
                };
                return response;
            }

            _dataManager.GetUsers().ToList().Add(new User
            {
                Id = id,
                FName = fName,
                LName = lName,
                Age = age
            });

            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        // PUT: api/Pokemons/5
        public IHttpActionResult Put([FromBody]User user)
        {
            if (user.Id < 0 || user.Id >= _dataManager.GetUsers().Count())
            {
                return BadRequest();
            }
            var listUsers = _dataManager.GetUsers().FirstOrDefault(x => x.Id == user.Id);
            if (listUsers == null)
            {
                return InternalServerError();
            }
            listUsers.FName = user.FName;
            return Ok();
        }

        // DELETE: api/Pokemons/5
        public IHttpActionResult Delete(int id)
        {
            IEnumerable<String> headerValues = Request.Headers.GetValues("Authorization");
            var token = headerValues.FirstOrDefault();
            if (token == "1234")
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            return Unauthorized();
        }

        private readonly IDataManager _dataManager;
    }
}
