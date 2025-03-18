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
            if (form?.Data is null)
            {
                return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_400);
            }
            //新增單位或是中心欄位
            List<List<object>> lists = JsonSerializer.Deserialize<List<List<object>>>(form.Data)?? [];
            for (int i = 0; i < request.SelectedUnits.Count; i++)
            {
                var unit = request.SelectedUnits.ElementAt(i);
                for (int j = 0; j < lists.Count; j++)
                {
                    if (j == 1)
                    {
                        lists[j].Add(unit.Name);
                    }
                    else
                    {
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
                Units = viewModel.Units
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
                    return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_500, message: ex.Message);
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
                    if (evaluation == null) {
                        throw new Exception("evaluation is null");
                    }
                    evaluation.Data = viewModel.Data;
                    evaluation.UpdateTime = DateTime.Now;

                    var evaluationList = await _context.EvaluationList.FirstOrDefaultAsync(p => p.EvaluationId == viewModel.EvaluationId);
                    if (evaluationList == null)
                    {
                        throw new Exception("evaluation is null");
                    }
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
                    return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_500,message: ex.Message);

                }
            }
        }
    }
}
