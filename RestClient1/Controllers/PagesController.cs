using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestClient1.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RestClient1.Controllers
{
    public class PagesController : Controller
    {
        // Get /pages
        public async Task<IActionResult> Index()
        {
            var pages = new List<Page>();
            using (var httpclient = new HttpClient())
            {
                var request = await httpclient.GetAsync("https://localhost:44323/api/pages");
                string response = await request.Content.ReadAsStringAsync();
                pages = JsonConvert.DeserializeObject<List<Page>>(response);
            }
            return View(pages);
        }

        // Get /pages/edit/id
        public async Task<IActionResult> Edit(int id)
        {
            var page = new Page();
            using (var httpclient = new HttpClient())
            {
                var request = await httpclient.GetAsync($"https://localhost:44323/api/pages/{id}");
                string response = await request.Content.ReadAsStringAsync();
                page = JsonConvert.DeserializeObject<Page>(response);
            }
            return View(page);
        }
        [HttpPost]
        // post /pages/edit/id
        public async Task<IActionResult> Edit(Page page)
        {
            page.Slug = page.Title.Replace(" ", "-").ToLower();

            using (var httpclient = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(page), System.Text.Encoding.UTF8, "application/JSON");
                var request = await httpclient.PutAsync($"https://localhost:44323/api/pages/{page.Id}", content);
                string response = await request.Content.ReadAsStringAsync();
                page = JsonConvert.DeserializeObject<Page>(response);
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }
        // get /pages/Create
        public IActionResult Create()
        { 

            return View();
            
}
        [HttpPost]
        // post /pages/edit/id
        public async Task<IActionResult> Create(Page page)
        {
            page.Slug = page.Title.Replace(" ", "-").ToLower();
            page.Sorting= 100;
            using (var httpclient = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(page), System.Text.Encoding.UTF8, "application/JSON");
                var request = await httpclient.PostAsync($"https://localhost:44323/api/pages", content);
                string response = await request.Content.ReadAsStringAsync();
                page = JsonConvert.DeserializeObject<Page>(response);
            }
            return RedirectToAction("Index");
        }
        
        // Get /pages/delete/id
        public async Task<IActionResult> Delete(int id)
        {
       
            using (var httpclient = new HttpClient())
            {
             using  var request = await httpclient.DeleteAsync($"https://localhost:44323/api/pages/{id}");
               
            }
            return RedirectToAction("Index");
        }
    }
}
