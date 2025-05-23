﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.SqlServer.Server;
using ReviewWebsite.Data;
using ReviewWebsite.Helpers;
using ReviewWebsite.Models.Db;
using ReviewWebsite.Models.ViewModel;
using ReviewWebsite.Models.ViewModel.Request;
using System;
using System.Text.Json;

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
            var model = await _context.FormList.ToListAsync();
            return this.ResolveView(nameof(Index), model);
        }

        // GET: Form/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {

            var form = await _context.Form.OrderByDescending(f => f.CreateTime).FirstOrDefaultAsync();
            if (form == null)
            {
                form = new Form()
                {
                    FormId = IdGenerator.GenerateUnitId(),

                    Data = JsonSerializer.Serialize(new List<List<object>>
                {
                    new List<object> { (DateTime.Now.Year-1911).ToString(),"新評鑑表單","","","","" },
                    new List<object> { "查核指標", "查核標準", "總分","應檢附資料", "評核基準","KPI連動" },
                })
                };
            }
            ViewData["Title"] = "新建表單";
            ViewData["Units"] = await _context.Unit.ToListAsync();
            return View("CreateOrEdit", form);
        }


        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Create([FromBody] FormViewModel formViewModel)
        {
            if (formViewModel == null ||
                formViewModel.Data.IsNullOrEmpty())
            {
                return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_400);
            }

            var form = new Form()
            {
                FormId = IdGenerator.GenerateUnitId(),
                Data = formViewModel.Data

            };
            var formListData = new FormList()
            {
                FormListId = IdGenerator.GenerateUnitId(),
                FormId = form.FormId,
                Name = formViewModel.Name,
                Year = formViewModel.Year,
            };

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.Form.AddAsync(form);
                    await _context.FormList.AddAsync(formListData);
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
            var form = await _context.Form.FirstOrDefaultAsync(m => m.FormId == id);
            ViewData["Title"] = "編輯表單";
            return View("CreateOrEdit", form);
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Edit([FromBody] FormViewModel formViewModel)
        {

            if (formViewModel == null ||
                formViewModel.Data.IsNullOrEmpty())
            {
                return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_400);
            }
            using (var transaction = await _context.Database.BeginTransactionAsync()) // 開始非同步交易
            {
                try
                {
                    // 更新 Form 表
                    var form = await _context.Form.FirstOrDefaultAsync(p => p.FormId == formViewModel.FormId);
                    if (form == null) {
                        return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_400);
                    }

                    if (DateTime.Parse(formViewModel.UpdateTime).TotalSeconds() != form.UpdateTime.TotalSeconds()) {
                        return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_501);
                    }
           
                    form.Data = formViewModel.Data;
                    form.UpdateTime = DateTime.Now;
                 
                    // 更新 FormList 表
                    var formList = await _context.FormList.FirstOrDefaultAsync(p => p.FormId == formViewModel.FormId);
                    if (formList == null)
                    {
                        return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_400);
                    }
                    formList.Name = formViewModel.Name;
                    formList.Year = formViewModel.Year;
                    formList.UpdateTime = DateTime.Now;

                    // 儲存所有變更
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_200);
                }
                catch (Exception ex)
                {
                    // 若有錯誤，回滾交易
                    await transaction.RollbackAsync();
                    return this.ResponseJson(ControllerExtensions.RESPONCE_CODE_500,message:ex.Message);

                }
            }

        }

    }
}
