using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ReviewWebsite.Data;
using ReviewWebsite.Models.Db;
using ReviewWebsite.Models.ViewModel.Request;
using X.PagedList.Extensions;

namespace ReviewWebsite.Controllers
{
    public class ReportController : Controller
    {
        private readonly AppDbContext _context;

        public ReportController(AppDbContext context)
        {
            _context = context;
        }

        //GET: Reports
        public IActionResult Index(int? page)
        {
            int pageSize = 2;
            int pageNumber = page ?? 1;

            var model = _context.EvaluationList
            .OrderBy(e => e.UpdateTime).ToPagedList(pageNumber, pageSize);
            return this.ResolveView(nameof(Index), model);
        }

        //GET: Reports
        [Consumes("application/json")]
        public Task<IActionResult> More(int page)
        {
            int pageSize = 2;
            int pageNumber = page;
            var model = _context.EvaluationList
            .OrderBy(e => e.UpdateTime).ToPagedList(pageNumber, pageSize);


            return Task.FromResult<IActionResult>(this.ResponseJson(ControllerExtensions.RESPONCE_CODE_200,
                data: JsonSerializer.Serialize(model), extraData: JsonSerializer.Serialize(model.IsLastPage)));
        }


        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateReport([FromBody] CreateReportRequest request)
        {

            if (request == null ||
                  request.SelectedEvaluations.IsNullOrEmpty())
            {
                return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_400);
            }

            // 所有的 table 數據
            List<List<List<object>>> allTables = new List<List<List<object>>>();
            //取得評估表單
            foreach (var item in request.SelectedEvaluations)
            {
                var evaluations = await _context.Evaluation
                  .Where(fh => fh.EvaluationId == item.EvaluationId)
                  .FirstOrDefaultAsync()?? new Evaluation { Data = "[]" };
                List<List<object>> table = JsonSerializer.Deserialize<List<List<object>>>(evaluations.Data) ?? [];
                allTables.Add(table);
            }

            List<List<object>> result = combineTable(allTables);

            return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_200, data: JsonSerializer.Serialize(result));
        }

        private List<List<object>> combineTable(List<List<List<object>>> tables)
        {

            // 組合結果
            List<List<object>> result = new List<List<object>>();
            //標頭
            var titles = new List<object>();
            //子標題
            var subTitles = new HashSet<object>(tables[0][1]);
            //內容
            var mergedTable = new Dictionary<string, List<object>>();
            foreach (var table in tables)
            {
                titles.Add($"{table[0][0]}-{table[0][1]}");
                subTitles.Union(table[1]);
            }
            foreach (var table in tables)
            {

                foreach (var row in table.Skip(2))
                {
                    if (row[0] == null|| row[0].ToString().Equals("")) { continue; }
                    string key = row[0].ToString();

                    if (mergedTable.ContainsKey(key))
                    {
                        mergedTable[key].Union(row);
                    }
                    else
                    {
                        mergedTable[key] = new List<object>(row);
                    }

                }
            }
            int targetLength = subTitles.Count();
            var titleList = titles.ToList().Concat(Enumerable.Repeat("", targetLength - titles.Count()));
            result.Add(titleList.ToList());
            result.Add(subTitles.ToList());
            foreach (var entry in mergedTable)
            {
                var list = entry.Value;
                var paddedList = list.Concat(Enumerable.Repeat<object>("", targetLength - list.Count)).ToList();
                result.Add(paddedList);
            }
            return result;
        }

        ////GET: Report/Create
        //public async Task<IActionResult> Create()
        //{

        //    var model = new ReportCreateOrEditViewModel
        //    {
        //        FormList = await _context.FormList.ToListAsync(),
        //        Units = await _context.Unit.ToListAsync()
        //    };
        //    ViewData["Title"] = "新增評鑑表單";
        //    return View(model);
        //}

        //[HttpPost]
        //[Consumes("application/json")]
        //public async Task<IActionResult> CreateFrom([FromBody] CreateFromRequest request)
        //{

        //    if (request == null ||
        //          request.SelectedFromId.IsNullOrEmpty() ||
        //          request.SelectedUnits.IsNullOrEmpty())
        //    {
        //        return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_400);
        //    }

        //    //取得表單
        //    var form = await _context.Form
        //      .Where(fh => fh.FormId == request.SelectedFromId)
        //      .FirstOrDefaultAsync();

        //    //新增單位或是中心欄位
        //    List<List<object>> lists = JsonSerializer.Deserialize<List<List<object>>>(form.Data);
        //    for (int i = 0; i < request.SelectedUnits.Count; i++)
        //    {
        //        var unit = request.SelectedUnits.ElementAt(i);
        //        for (int j = 0; j < lists.Count; j++)
        //        {
        //            if (j == 1)
        //            {
        //                lists[j].Add(unit.Name);
        //            }
        //            else {
        //                lists[j].Add("");
        //            }
        //        }
        //    }
        //    form.Data = JsonSerializer.Serialize(lists);

        //    return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_200, data: JsonSerializer.Serialize(form));
        //}


        //[HttpPost]
        //[Consumes("application/json")]
        //public async Task<IActionResult> Create([FromBody] ReportViewModel viewModel)
        //{
        //    if (viewModel == null ||
        //        viewModel.Data.IsNullOrEmpty())
        //    {
        //        return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_400);
        //    }


        //    var Report = new Report()
        //    {
        //        ReportId = IdGenerator.GenerateUnitId(),
        //        Data = viewModel.Data

        //    };
        //    var ReportListData = new ReportList()
        //    {
        //        ReportListId = IdGenerator.GenerateUnitId(),
        //        ReportId = Report.ReportId,
        //        FromName = viewModel.FromName,
        //        Year = viewModel.Year,
        //        Units=viewModel.Units
        //    };

        //    using (var transaction = await _context.Database.BeginTransactionAsync())
        //    {
        //        try
        //        {
        //            await _context.Report.AddAsync(Report);
        //            await _context.ReportList.AddAsync(ReportListData);
        //            await _context.SaveChangesAsync();
        //            await transaction.CommitAsync();
        //            return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_200);
        //        }
        //        catch (Exception ex)
        //        {
        //            await transaction.RollbackAsync();
        //            return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_500);
        //        }
        //    }

        //}


        ////GET: Users/Edit/5
        //[HttpGet]
        //public async Task<IActionResult> Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    var Report = await _context.Report.FirstOrDefaultAsync(m => m.ReportId == id);
        //    ViewData["Title"] = "評鑑表單";
        //    return View(Report);
        //}

        //[HttpPost]
        //[Consumes("application/json")]
        //public async Task<IActionResult> Edit([FromBody] ReportViewModel viewModel)
        //{
        //    if (viewModel == null ||
        //       viewModel.Data.IsNullOrEmpty())
        //    {
        //        return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_400);
        //    }
        //    using (var transaction = await _context.Database.BeginTransactionAsync()) // 開始非同步交易
        //    {
        //        try
        //        {

        //            var Report = await _context.Report.FirstOrDefaultAsync(p => p.ReportId == viewModel.ReportId);
        //            Report.Data = viewModel.Data;
        //            Report.UpdateTime = DateTime.Now;

        //            var ReportList = await _context.ReportList.FirstOrDefaultAsync(p => p.ReportId == viewModel.ReportId);
        //            ReportList.FromName = viewModel.FromName;
        //            ReportList.Year = viewModel.Year;
        //            ReportList.UpdateTime = DateTime.Now;

        //            // 儲存所有變更
        //            await _context.SaveChangesAsync();
        //            await transaction.CommitAsync();
        //            return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_200);
        //        }
        //        catch (Exception ex)
        //        {
        //            // 若有錯誤，回滾交易
        //            await transaction.RollbackAsync();
        //            return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_500);

        //        }
        //    }
        //}



        //[HttpPost]
        //[Consumes("application/json")]
        //public async Task<IActionResult> Create([FromBody] ReportCreateOrEditViewModel viewModel)
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


        //// POST: Reports/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ReportId,FormHeadId,Rating,UpdateTime,CreateTime")] Reports Reports)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(Reports);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(Reports);
        //}

        // GET: Reports/Edit/5
        //public async Task<IActionResult> Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    //var Reports = await _context.Report.FindAsync(id);
        //    //if (Reports == null)
        //    //{
        //    //    return NotFound();
        //    //}
        //    return View();
        //}

        //// POST: Reports/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, [Bind("ReportId,FormHeadId,Rating,UpdateTime,CreateTime")] Report Reports)
        //{
        //    if (id != Reports.ReportId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(Reports);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ReportsExists(Reports.ReportId))
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
        //    return View(Reports);
        //}

        //// GET: Reports/Delete/5
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var Reports = await _context.Reports
        //        .FirstOrDefaultAsync(m => m.ReportId == id);
        //    if (Reports == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(Reports);
        //}

        //// POST: Reports/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(string id)
        //{
        //    var Reports = await _context.Reports.FindAsync(id);
        //    if (Reports != null)
        //    {
        //        _context.Reports.Remove(Reports);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ReportsExists(string id)
        //{
        //    return _context.Reports.Any(e => e.ReportId == id);
        //}
    }
}
