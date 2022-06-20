using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using WebApiCodeFirst.DB;
using WebApiCodeFirst.Models;

namespace WebApiCodeFirst.Controllers
{
    public class HomeController : Controller
    {
        private readonly dbdata _db;

        Uri baseaddress = new Uri("http://localhost:56421/");

        HttpClient Client;

        public HomeController(dbdata db)
        {
            _db = db;

            Client = new HttpClient();

            Client.BaseAddress = baseaddress;
        }

        public IActionResult Index()
        {
            List<mymodel> mm = new List<mymodel>();
            List<mytable> tt = new List<mytable>();

            HttpResponseMessage res = Client.GetAsync(baseaddress + "get/data").Result;

            if (res.IsSuccessStatusCode)
            {
                string data = res.Content.ReadAsStringAsync().Result;

                tt = JsonConvert.DeserializeObject<List<mytable>>(data);

            }


            foreach (var item in tt)
            {
                mm.Add(new mymodel
                {
                    Id = item.Id,
                    Name = item.Name,
                    age = item.age,

                });

            }

            return View(mm);
        }

        [HttpGet]
        public IActionResult Add()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Add(mymodel obj)
        {

            mytable tt = new mytable();

            tt.Id = obj.Id;
            tt.Name = obj.Name;
            tt.age = obj.age;
            string data = JsonConvert.SerializeObject(tt);

            StringContent Con = new StringContent(data, Encoding.UTF8, "application/json");

            //jump

            HttpResponseMessage res = Client.PostAsync(Client.BaseAddress + "post/data", Con).Result;

            //get data all good or not

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Edit(int id)
        {

            mymodel tt = new mymodel();
            var editing = _db.mytables.Where(e => e.Id == id).FirstOrDefault();

            tt.Id = editing.Id;
            tt.Name = editing.Name;
            tt.age = editing.age;

            return View("Add", tt);
        }

        public IActionResult Del(int id)
        {

            var de = _db.mytables.Where(i => i.Id == id).FirstOrDefault();
            _db.mytables.Remove(de);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        //===========Testing=====================================Login============

        [HttpGet]

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]

        public IActionResult Login(mymodel obj)
        {
            mytable tt = new mytable();

            tt.Id = obj.Id;
            tt.Name = obj.Name;
            tt.age = obj.age;

            string data = JsonConvert.SerializeObject(tt);

            StringContent Con = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage res = Client.PostAsync(Client.BaseAddress + "login/data", Con).Result;


            return RedirectToAction("Index", "Home");
        }

           
    }
}
