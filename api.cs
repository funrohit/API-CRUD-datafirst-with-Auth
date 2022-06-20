using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Authentication;
using PureAPICodeFirst.dbdata0;
using PureAPICodeFirst.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace PureAPICodeFirst.Controllers
{
    public class MyApiController : ApiController
    {
        public object TempData { get; private set; }

        [HttpGet]
        [Route("get/data")]
        public List<mytable> get()
        {

            WebApiEntities1 database = new WebApiEntities1();

            List<mymodel> mm = new List<mymodel>();

            var res = database.mytables.ToList();


            return res;

        }

        [HttpPost]
        [Route("post/data")]
        public HttpResponseMessage fun(mytable obj)
        {
            WebApiEntities1 database = new WebApiEntities1();


            mytable tt = new mytable();

            if (obj.Id == 0)
            {
                database.mytables.Add(obj);
                database.SaveChanges();
            }
            else
            {
                database.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                database.SaveChanges();

            }

            HttpResponseMessage res = new HttpResponseMessage(HttpStatusCode.OK);

            return res;
        }


        //======================================= test ===========================//only right pass

        [HttpPost]
        [Route("login/data")]
        public HttpResponseMessage log(mytable obj1)
        {

            WebApiEntities1 database = new WebApiEntities1();

            var res = database.mytables.Where(a => a.Name == obj1.Name).FirstOrDefault();

           

            if (res.Name == obj1.Name && res.age == obj1.age)
            {

                var claims = new[] { new Claim(ClaimTypes.Name, res.Name) };
                
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };


            }
           
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            return response;
        }


    }
}
