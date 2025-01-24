using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReviewWebsite.Data;
using ReviewWebsite.Helpers;
using ReviewWebsite.Models.Db;
using ReviewWebsite.Models.ViewModel;

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
            var viewModel = new UserManagementViewModel
            {
                Users = await _context.User.ToListAsync(),
                Units = await _context.Unit.ToListAsync()
            };
            return this.ResolveView(nameof(Index), viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([Bind("UserId,AccessRight")] User user)
        {
            try
            {
                var orgUser = await _context.User.FirstAsync(m => m.UserId == user.UserId);
                orgUser.AccessRight = user.AccessRight;
                _context.Update(orgUser);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index), new { from = "user" });
        }

        [HttpPost]
        public async Task<IActionResult> Create(Unit unit)
        {
            unit.UnitId = IdGenerator.GenerateUnitId();
            _context.Add(unit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { from = "unit" });
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
