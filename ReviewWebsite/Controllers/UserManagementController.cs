using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ReviewWebsite.Data;
using ReviewWebsite.Helpers;
using ReviewWebsite.Models.Db;
using ReviewWebsite.Models.ViewModel;
using ReviewWebsite.Utils;
using System.Text;
using System.Text.Json;
namespace ReviewWebsite.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly AppDbContext _context;

        public UserManagementController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(String from = "user")
        {
            var viewModel = new UserManagementViewModel();

            if (from == "unit")
            {
                viewModel.Units = await _context.Unit.ToListAsync();
            }
            else
            {
                viewModel.Users =  await _context.User
                    .Include(u => u.Unit) 
                    .ToListAsync();
                byte[] byteArray = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(viewModel.Users[0]));
                string base64String = Convert.ToBase64String(byteArray);
                //測試使用者
                Cookie.SetCookie(Response, "token", base64String);
                
            }
         
            return this.ResolveView(nameof(Index), viewModel);
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> EditUser([FromBody] User User)
        {

            if (User == null)
            {
                return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_400);
            }

            try
            {
                var orgUser = await _context.User.FirstAsync(m => m.UserId == User.UserId);
                orgUser.AccessRight = User.AccessRight;
                _context.Update(orgUser);
                await _context.SaveChangesAsync();
                return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_200);
            }
            catch (Exception ex)
            {
                return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_500, message: ex.Message);
            }
        }


        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateUnit([FromBody] Unit Unit)
        {
            if (Unit == null || Unit.Name.IsNullOrEmpty())
            {
                return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_400);
            }

            try
            {
                Unit.UnitId = IdGenerator.GenerateUnitId();
                _context.Add(Unit);
                await _context.SaveChangesAsync();
                return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_200);
            }
            catch (Exception ex)
            {
                return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_500, message: ex.Message);
            }
        }








        // GET: UsersManage/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: UsersManage/Create
        public IActionResult Create()
        {
            return View();
        }

        // GET: UsersManage/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // GET: UsersManage/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: UsersManage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.User.Any(e => e.UserId == id);
        }
    }
}
