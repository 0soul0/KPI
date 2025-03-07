using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;
using ReviewWebsite.Data;
using ReviewWebsite.Helpers;
using ReviewWebsite.Models.Db;
using ReviewWebsite.Models.ViewModel;
using ReviewWebsite.Models.ViewModel.Request;

namespace ReviewWebsite.Controllers
{
    public class EvaluationController : Controller
    {
        private readonly AppDbContext _context;

        public EvaluationController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Evaluations
        public async Task<IActionResult> Index()
        {

            var model = await _context.EvaluationList.ToListAsync();
            return this.ResolveView(nameof(Index), model);
        }


        //GET: Evaluation/Create
        public async Task<IActionResult> Create()
        {

            var model = new EvaluationCreateOrEditViewModel
            {
                FormList = await _context.FormList.ToListAsync(),
                Units = await _context.Unit.ToListAsync()
            };
            ViewData["Title"] = "新增評鑑表單";
            return View(model);
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateFrom([FromBody] CreateFromRequest request)
        {

            if (request == null ||
                  request.SelectedFromId.IsNullOrEmpty() ||
                  request.SelectedUnits.IsNullOrEmpty())
            {
                return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_400);
            }

            //取得表單
            var form = await _context.Form
              .Where(fh => fh.FormId == request.SelectedFromId)
              .FirstOrDefaultAsync();

            //新增單位或是中心欄位
            List<List<object>> lists = JsonSerializer.Deserialize<List<List<object>>>(form.Data);
            for (int i = 0; i < request.SelectedUnits.Count; i++)
            {
                var unit = request.SelectedUnits.ElementAt(i);
                for (int j = 0; j < lists.Count; j++)
                {
                    if (j == 1)
                    {
                        lists[j].Add(unit.Name);
                    }
                    else {
                        lists[j].Add("");
                    }
                }
            }
            form.Data = JsonSerializer.Serialize(lists);

            return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_200, data: JsonSerializer.Serialize(form));
        }


        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Create([FromBody] EvaluationViewModel viewModel)
        {
            if (viewModel == null ||
                viewModel.Data.IsNullOrEmpty())
            {
                return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_400);
            }


            var evaluation = new Evaluation()
            {
                EvaluationId = IdGenerator.GenerateUnitId(),
                Data = viewModel.Data

            };
            var evaluationListData = new EvaluationList()
            {
                EvaluationListId = IdGenerator.GenerateUnitId(),
                EvaluationId = evaluation.EvaluationId,
                FromName = viewModel.FromName,
                Year = viewModel.Year,
                Units=viewModel.Units
            };

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.Evaluation.AddAsync(evaluation);
                    await _context.EvaluationList.AddAsync(evaluationListData);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_200);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_500);
                }
            }

        }


        //GET: Users/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var evaluation = await _context.Evaluation.FirstOrDefaultAsync(m => m.EvaluationId == id);
            ViewData["Title"] = "評鑑表單";
            return View(evaluation);
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Edit([FromBody] EvaluationViewModel viewModel)
        {
            if (viewModel == null ||
               viewModel.Data.IsNullOrEmpty())
            {
                return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_400);
            }
            using (var transaction = await _context.Database.BeginTransactionAsync()) // 開始非同步交易
            {
                try
                {

                    var evaluation = await _context.Evaluation.FirstOrDefaultAsync(p => p.EvaluationId == viewModel.EvaluationId);
                    evaluation.Data = viewModel.Data;
                    evaluation.UpdateTime = DateTime.Now;

                    var evaluationList = await _context.EvaluationList.FirstOrDefaultAsync(p => p.EvaluationId == viewModel.EvaluationId);
                    evaluationList.FromName = viewModel.FromName;
                    evaluationList.Year = viewModel.Year;
                    evaluationList.UpdateTime = DateTime.Now;

                    // 儲存所有變更
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_200);
                }
                catch (Exception ex)
                {
                    // 若有錯誤，回滾交易
                    await transaction.RollbackAsync();
                    return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_500);

                }
            }
        }



        //[HttpPost]
        //[Consumes("application/json")]
        //public async Task<IActionResult> Create([FromBody] EvaluationCreateOrEditViewModel viewModel)
        //{
        //    if (viewModel == null ||
        //        viewModel.SelectedFromId.IsNullOrEmpty() ||
        //        viewModel.SelectedUnits.IsNullOrEmpty())
        //    {
        //        return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_400);
        //    }

        //    try
        //    {
        //        _context.Add(formViewModel.FormHead);
        //        _context.AddRange(formViewModel.FormContentList);
        //        await _context.SaveChangesAsync();
        //        return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_200);
        //    }
        //    catch (Exception ex)
        //    {
        //        return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_500);
        //    }

        //}


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
        //public async Task<IActionResult> Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    //var evaluations = await _context.Evaluation.FindAsync(id);
        //    //if (evaluations == null)
        //    //{
        //    //    return NotFound();
        //    //}
        //    return View();
        //}

        //// POST: Evaluations/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, [Bind("EvaluationId,FormHeadId,Rating,UpdateTime,CreateTime")] Evaluation evaluations)
        //{
        //    if (id != evaluations.EvaluationId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(evaluations);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!EvaluationsExists(evaluations.EvaluationId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(evaluations);
        //}

        //// GET: Evaluations/Delete/5
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var evaluations = await _context.Evaluations
        //        .FirstOrDefaultAsync(m => m.EvaluationId == id);
        //    if (evaluations == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(evaluations);
        //}

        //// POST: Evaluations/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(string id)
        //{
        //    var evaluations = await _context.Evaluations.FindAsync(id);
        //    if (evaluations != null)
        //    {
        //        _context.Evaluations.Remove(evaluations);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool EvaluationsExists(string id)
        //{
        //    return _context.Evaluations.Any(e => e.EvaluationId == id);
        //}
    }
}
