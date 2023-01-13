using apiconsume.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Configuration;



namespace apiconsume.Controllers
{
    public class EmployeeController : Controller
    {
        string Baseurl = ConfigurationManager.AppSettings["baseurl"];
        public async Task<ActionResult> Index()
        {
            List<Employee> EmpInfo = new List<Employee>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/Employee/");
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    EmpInfo = JsonConvert.DeserializeObject<List<Employee>>(EmpResponse);
                }
                //returning the employee list to view
                return View(EmpInfo);
            }
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Employee emp)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                //HTTP POST
                var postTask = client.PostAsJsonAsync<Employee>("api/Employee/", emp);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(emp);

        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Employee listemp = new Employee();
             HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Baseurl);           
            HttpResponseMessage response = client.GetAsync("api/Employee?id=" + id.ToString()).Result;
            response.EnsureSuccessStatusCode();            
            listemp = response.Content.ReadAsAsync<Employee>().Result;
            //string jsonstr = JsonConvert.SerializeObject(llistemp);
            //listemp = JsonConvert.DeserializeObject<Employee>(llistemp);            
            return View(listemp);
        }
        [HttpPost]
        public ActionResult Edit(Employee emp,int id) 
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Baseurl);
            var response = client.PutAsJsonAsync("api/Employee?id="+id.ToString(), emp).Result;
            response.EnsureSuccessStatusCode();
            return RedirectToAction("Index");
        }
        public ActionResult Details(int id)
        {
            Employee listemp = new Employee();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Baseurl);
            HttpResponseMessage response = client.GetAsync("api/Employee?id=" + id.ToString()).Result;
            response.EnsureSuccessStatusCode();
            listemp = response.Content.ReadAsAsync<Employee>().Result;
            //string jsonstr = JsonConvert.SerializeObject(llistemp);
            //listemp = JsonConvert.DeserializeObject<Employee>(llistemp);            
            return View(listemp);
        }
        //[HttpPut]
        //public ActionResult Edit(Employee emp)
        //{
        //    using (var client = new HttpClient()) 
        //    {
        //    client.BaseAddress = new Uri(Baseurl);
        //        client.DefaultRequestHeaders.Clear();
        //        //Define request data format
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        var putTask = client.PutAsJsonAsync<Employee>("api/Employee", emp);
        //        putTask.Wait();
        //        var result = putTask.Result;
        //        if (result.IsSuccessStatusCode)
        //        return RedirectToAction("Index");
        //        return View(emp);


        //    }
        //}
        public ActionResult Delete(int id)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Baseurl);
            HttpResponseMessage response = client.DeleteAsync("api/Employee?id=" + id.ToString()).Result;
            response.EnsureSuccessStatusCode();
            return View();
        }
    }
}