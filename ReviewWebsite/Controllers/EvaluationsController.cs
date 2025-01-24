using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ReviewWebsite.Data;
using ReviewWebsite.Models.Db;
using ReviewWebsite.Models.ViewModel;

namespace ReviewWebsite.Controllers
{
    public class EvaluationsController : Controller
    {
        private readonly AppDbContext _context;

        public EvaluationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Evaluations
        public async Task<IActionResult> Index()
        {

            var model = await _context.Evaluations
                .FromSqlRaw("SELECT Evaluations.EvaluationId,Evaluations.FormHeadId,Evaluations.Rating,Evaluations." +
                "UpdateTime,Evaluations.CreateTime,FormHead.Name,FormHead.Year " +
                "FROM Evaluations LEFT OUTER JOIN FormHead ON Evaluations.FormHeadId=FormHead.FormHeadId;").ToListAsync();

            return this.ResolveView(nameof(Index), model);
        }


        // GET: Evaluations/Create
        public async Task<IActionResult> Create()
        {

            var model = new EvaluationsViewModel()
            {
                FormHeads = await _context.FormHead.ToListAsync(),
                Units = await _context.Unit.ToListAsync(),
            };

            return View(model);
        }

        //// POST: Evaluations/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("EvaluationId,FormHeadId,Rating,UpdateTime,CreateTime")] Evaluations evaluations)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(evaluations);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(evaluations);
        //}

        // GET: Evaluations/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evaluations = await _context.Evaluations.FindAsync(id);
            if (evaluations == null)
            {
                return NotFound();
            }
            return View(evaluations);
        }

        // POST: Evaluations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("EvaluationId,FormHeadId,Rating,UpdateTime,CreateTime")] Evaluations evaluations)
        {
            if (id != evaluations.EvaluationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(evaluations);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EvaluationsExists(evaluations.EvaluationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(evaluations);
        }

        // GET: Evaluations/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evaluations = await _context.Evaluations
                .FirstOrDefaultAsync(m => m.EvaluationId == id);
            if (evaluations == null)
            {
                return NotFound();
            }

            return View(evaluations);
        }

        // POST: Evaluations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var evaluations = await _context.Evaluations.FindAsync(id);
            if (evaluations != null)
            {
                _context.Evaluations.Remove(evaluations);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EvaluationsExists(string id)
        {
            return _context.Evaluations.Any(e => e.EvaluationId == id);
        }
    }
}
