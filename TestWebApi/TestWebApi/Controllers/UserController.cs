using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using TestWebApi.DbContext;
using TestWebApi.Models;

namespace TestWebApi.Controllers
{
    public class UserController : ApiController
    {
        [HttpGet]
        [Route("api/user/get")]
        public IHttpActionResult Get()
        {
            try
            {
                var users = UserDb.GetAll();
                return Ok(users);
            }catch(Exception err)
            {
                return Ok(err.Message);
            }
        }
        [HttpGet]
        [Route("api/user/get/{id}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var user = UserDb.GetbyId(id);
                return Ok(user);
            }
            catch (Exception err)
            {
                return Ok(err.Message);
            }
        }
        [HttpPost]
        [Route("api/user/save")]
        public IHttpActionResult Save(User user)
        {
            try
            {
                bool isSave = UserDb.Save(user);
                return Ok(isSave);
            }
            catch (Exception err)
            {
                return Ok(err.Message);
            }
        }
        [HttpPut]
        [Route("api/user/update")]
        public IHttpActionResult Update(User user)
        {
            try
            {
                bool isUpdate = UserDb.Update(user);
                return Ok(isUpdate);
            }
            catch (Exception err)
            {
                return Ok(err.Message);
            }
        }
        [HttpDelete]
        [Route("api/user/delete/{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                bool isDelete = UserDb.Delete(id);
                return Ok(isDelete);
            }
            catch (Exception err)
            {
                return Ok(err.Message);
            }
        }



        [Authorize]
        [HttpGet]
        [Route("api/user/authenticate")]
        public IHttpActionResult GetForAuthenticate()
        {
            var identity = (ClaimsIdentity)User.Identity;
            return Ok("Hello " + identity.Name);
        }

        [Authorize(Roles ="4")]
        [HttpGet]
        [Route("api/user/admin")]
        public IHttpActionResult GetAdmin()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value);
            return Ok("Hello " + identity.Name + " Role: " + string.Join(",", roles.ToList()));
        }
    }
}
