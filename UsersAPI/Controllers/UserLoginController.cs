using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UsersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : Controller
    {
        // GET: UserLoginController
        public ActionResult Post()
        {
            return View();
        }

        // GET: UserLoginController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserLoginController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserLoginController/Create
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

        // GET: UserLoginController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserLoginController/Edit/5
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

        // GET: UserLoginController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserLoginController/Delete/5
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
