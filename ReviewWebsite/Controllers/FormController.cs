using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ReviewWebsite.Data;
using ReviewWebsite.Helpers;
using ReviewWebsite.Models.Db;
using ReviewWebsite.Models.ViewModel;
using ReviewWebsite.Models.ViewModel.Request;

namespace ReviewWebsite.Controllers
{   /*
        1. 建立標單時,如果標題或是item 都是空時需要保留嗎?
        2. KPI連動是要連動到哪個資料庫
     
     
     */
    public class FormController : Controller
    {
        private readonly AppDbContext _context;

        public FormController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Form
        public async Task<IActionResult> Index()
        {
            var model = await _context.FormHead.ToListAsync();
            return this.ResolveView(nameof(Index), model);
        }

        // GET: Form/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {

            var formHead = await _context.FormHead.OrderByDescending(f => f.CreateTime).FirstOrDefaultAsync();
        
            var viewModel = new FormViewModel() {
                FormContentList = new List<FormContent> {
                new FormContent() { FormContentId="",FormHeadId="",RowIndex=1,Type="title" },
                new FormContent() { FormContentId = "", FormHeadId = "", RowIndex = 2,Type="item" } }
            };
            if (formHead != null)
            {
                List<FormContent> formContentList = _context.FormContent.Where(f => f.FormHeadId == formHead.FormHeadId).OrderBy(f => f.RowIndex).ToList();
                viewModel.FormHead = formHead;
                viewModel.FormContentList = formContentList;
            }
            ViewData["Title"] = "新建表單";
            return RedirectToCreateOrEdit(viewModel);
        }

        // GET: Users/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }
            FormHead formHead = await _context.FormHead.FindAsync(id);
            List<FormContent> formContentList = await _context.FormContent.Where(f => f.FormHeadId == formHead.FormHeadId).OrderBy(f=>f.RowIndex).ToListAsync();

            var viewModel = new FormViewModel
            {
                FormHead = formHead,
                FormContentList = formContentList
            };
            ViewData["Title"] = "編輯表單";
            return RedirectToCreateOrEdit(viewModel);
        }


        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Create([FromBody] FormViewModel formViewModel)
        {
            if (formViewModel == null ||
                formViewModel.FormHead == null ||
                formViewModel.FormContentList.IsNullOrEmpty())
            {
                return Json(new ResponseViewModel
                {
                    Code = "400",
                    Message = "資料錯誤"
                });
            }
            //重新設定GUID
            formViewModel.FormHead.FormHeadId = IdGenerator.GenerateUnitId();
            formViewModel.FormContentList.ForEach(item =>
                {
                    item.FormHeadId = formViewModel.FormHead.FormHeadId;
                    item.FormContentId = IdGenerator.GenerateUnitId();
                });

            try
            {
                _context.Add(formViewModel.FormHead);
                _context.AddRange(formViewModel.FormContentList);
                await _context.SaveChangesAsync();
                return Json(new ResponseViewModel
                {
                    Code = "200",
                    Message = ""
                });
            }
            catch (Exception ex)
            {
                return Json(new ResponseViewModel
                {
                    Code = "500",
                    Message = "新增資料失敗,請重新嘗試"
                });
            }

        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Edit([FromBody] FormViewModel formViewModel)
        {

            if (formViewModel == null ||
              formViewModel.FormHead == null ||
              formViewModel.FormContentList.IsNullOrEmpty())
            {
                return Json(new ResponseViewModel
                {
                    Code = "400",
                    Message = "資料錯誤"
                });
            }



            //設定GUID
            formViewModel.FormContentList.ForEach(item =>
            {
                item.FormHeadId = formViewModel.FormHead.FormHeadId;
                item.FormContentId = IdGenerator.GenerateUnitId();
            });

            try
            {
                _context.Update(formViewModel.FormHead);
                var itemsToRemove = _context.FormContent.Where(t => t.FormHeadId == formViewModel.FormHead.FormHeadId).ToList();
                _context.FormContent.RemoveRange(itemsToRemove);
                _context.AddRange(formViewModel.FormContentList);
                await _context.SaveChangesAsync();
                return Json(new ResponseViewModel
                {
                    Code = "200",
                    Message = ""
                });
            }
            catch (Exception ex)
            {
                return Json(new ResponseViewModel
                {
                    Code = "500",
                    Message = ex.Message
                });
            }
        }


        [HttpPost]
        [Consumes("application/json")]
        public IActionResult AddItemRow([FromBody] AddItemRowRequest data)
        {

            string type = data.Type ?? "item";
            int rowCount = data.RowCount > 0 ? data.RowCount : 1;

            var viewModel = new FormContent() { FormContentId = "", FormHeadId = "", RowIndex = rowCount, Type = type };

            ViewData["Widths"] = getTabeHeadsWidth();
            ViewData["SubTitleWidths"] = getTabeHeadsWidth().Skip(1).Take(5).Sum();
            // 返回新的行的 HTML 結構
            return PartialView("_ItemRow", viewModel);
        }


        private List<float> getTabeHeadsWidth()
        {
            List<float> widths = new List<float>();
            widths.Add(2);
            widths.Add(13);
            widths.Add(23);
            widths.Add(20);
            widths.Add(25);
            widths.Add(10);
            widths.Add(5);
            return widths;
        }

        private IActionResult RedirectToCreateOrEdit(FormViewModel viewModel)
        {
            ViewData["Widths"] = getTabeHeadsWidth();
            ViewData["SubTitleWidths"] = getTabeHeadsWidth().Skip(1).Take(5).Sum();
            return View("CreateOrEdit", viewModel);
        }










        // GET: Form/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formHead = await _context.FormHead
                .FirstOrDefaultAsync(m => m.FormHeadId == id);
            if (formHead == null)
            {
                return NotFound();
            }

            return View(formHead);
        }


        // GET: Form/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var formHead = await _context.FormHead
                .FirstOrDefaultAsync(m => m.FormHeadId == id);
            if (formHead == null)
            {
                return NotFound();
            }

            return View(formHead);
        }

        // POST: Form/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var formHead = await _context.FormHead.FindAsync(id);
            if (formHead != null)
            {
                _context.FormHead.Remove(formHead);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FormHeadExists(string id)
        {
            return _context.FormHead.Any(e => e.FormHeadId == id);
        }
    }
}
