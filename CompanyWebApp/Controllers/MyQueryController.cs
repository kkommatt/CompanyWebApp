using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompanyWebApp.Controllers
{
    public class MyQueryController : Controller
    {
        // GET: MyQueryController
        public ActionResult Index()
        {
            return View();
        }

        // GET: MyQueryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MyQueryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MyQueryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MyQueryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MyQueryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MyQueryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MyQueryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
