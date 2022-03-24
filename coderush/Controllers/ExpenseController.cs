using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using coderush.Data;
using coderush.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace coderush.Controllers
{
    [Authorize(Roles = Services.App.Pages.Expense.RoleName)]
    public class ExpenseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Services.App.ICommon _app;

        //dependency injection through constructor, to directly access services
        public ExpenseController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            Services.App.ICommon app
            )
        {
            _context = context;
            _userManager = userManager;
            _app = app;
        }

        //fill viewdata as dropdownlist datasource for form
        private void FillDropdownListWithData()
        {
            ViewData["ExpenseType"] = _app.GetExpenseTypeSelectList();
            ViewData["OnBehalf"] = _app.GetEmployeeSelectList();
        }

        //display list of expenses
        public IActionResult Index(string period)
        {
            var Expenses = _context.Expense                
                .Include(x => x.ExpenseType)
                .Include(x => x.OnBehalf)
                .OrderByDescending(x => x.CreatedAtUtc)
                .ToList()
                .Where(x => x.FromDate.ToString("yyyy-MM").Equals(period));
            return View(Expenses.ToList());
        }

        //display Expense create edit form
        [HttpGet]
        public IActionResult Form(string id)
        {

            //create new
            if (id == null)
            {
                //dropdownlist type
                FillDropdownListWithData();

                Expense newExpense = new Expense();
                newExpense.FromDate = DateTime.Now;
                newExpense.ToDate = DateTime.Now;
                return View(newExpense);
            }

            //edit Expense
            Expense Expense = new Expense();
            Expense = _context.Expense.Where(x => x.ExpenseId.Equals(id)).FirstOrDefault();

            if (Expense == null)
            {
                return NotFound();
            }
            //dropdownlist type
            FillDropdownListWithData();

            return View(Expense);

        }

        //post submitted Expense data. if Expense.ExpenseId is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitForm([Bind(
            "ExpenseId",
            "ExpenseName",
            "IsApproved",
            "ExpenseTypeId",
            "FromDate",
            "ToDate",
            "Description",
            "OnBehalfId",
            "ExpenseAmount",
            "IsCashAdvance"
            )]Expense Expense)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(Form), new { id = Expense.ExpenseId ?? "" });
                }

                //create new
                if (Expense.ExpenseId == null)
                {
                    Expense newExpense = new Expense();
                    newExpense.ExpenseId = Guid.NewGuid().ToString();
                    newExpense.ExpenseName = Expense.ExpenseName;
                    newExpense.ExpenseTypeId = Expense.ExpenseTypeId;
                    newExpense.FromDate = Expense.FromDate;
                    newExpense.ToDate = Expense.ToDate;
                    newExpense.ExpenseAmount = Expense.ExpenseAmount;
                    newExpense.IsCashAdvance = Expense.IsCashAdvance;
                    newExpense.IsApproved = Expense.IsApproved;
                    newExpense.Description = Expense.Description;
                    newExpense.OnBehalfId = Expense.OnBehalfId;
                    newExpense.CreatedBy = await _userManager.GetUserAsync(User);
                    newExpense.CreatedAtUtc = DateTime.UtcNow;
                    newExpense.UpdatedBy = newExpense.CreatedBy;
                    newExpense.UpdatedAtUtc = newExpense.CreatedAtUtc;

                    _context.Expense.Add(newExpense);
                    _context.SaveChanges();

                    //dropdownlist type
                    FillDropdownListWithData();

                    TempData[StaticString.StatusMessage] = "Create new Expense item success.";
                    return RedirectToAction(nameof(Form), new { id = newExpense.ExpenseId ?? "" });
                }

                //edit existing
                Expense editExpense = new Expense();
                editExpense = _context.Expense.Where(x => x.ExpenseId.Equals(Expense.ExpenseId)).FirstOrDefault();
                editExpense.ExpenseName = Expense.ExpenseName;
                editExpense.ExpenseTypeId = Expense.ExpenseTypeId;
                editExpense.FromDate = Expense.FromDate;
                editExpense.ToDate = Expense.ToDate;
                editExpense.ExpenseAmount = Expense.ExpenseAmount;
                editExpense.IsCashAdvance = Expense.IsCashAdvance;
                editExpense.IsApproved = Expense.IsApproved;
                editExpense.Description = Expense.Description;
                editExpense.OnBehalfId = Expense.OnBehalfId;
                editExpense.UpdatedBy = await _userManager.GetUserAsync(User);
                editExpense.UpdatedAtUtc = DateTime.UtcNow;
                _context.Update(editExpense);
                _context.SaveChanges();

                //dropdownlist type
                FillDropdownListWithData();

                TempData[StaticString.StatusMessage] = "Edit existing Expense item success.";
                return RedirectToAction(nameof(Form), new { id = Expense.ExpenseId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Form), new { id = Expense.ExpenseId ?? "" });
            }
        }

        //display Expense item for deletion
        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //dropdownlist type
            FillDropdownListWithData();

            var Expense = _context.Expense
                .Include(x => x.ExpenseType)
                .Include(x => x.OnBehalf)
                .Where(x => x.ExpenseId.Equals(id)).FirstOrDefault();
            return View(Expense);
        }

        //delete submitted Expense item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitDelete([Bind("ExpenseId")]Expense Expense)
        {
            try
            {
                var deleteExpense = _context.Expense.Where(x => x.ExpenseId.Equals(Expense.ExpenseId)).FirstOrDefault();
                if (deleteExpense == null)
                {
                    return NotFound();
                }

                _context.Expense.Remove(deleteExpense);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete Expense item success.";
                return RedirectToAction(nameof(Index), new { period = DateTime.Now.ToString("yyyy-MM") });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Delete), new { id = Expense.ExpenseId ?? "" });
            }
        }


        //display of ExpenseType
        public IActionResult ExpenseTypeIndex()
        {
            var objs = _context.ExpenseType.OrderByDescending(x => x.CreatedAtUtc).ToList();
            return View(objs);
        }

        //display ExpenseType create edit form
        [HttpGet]
        public IActionResult ExpenseTypeForm(string id)
        {
            //create new
            if (id == null)
            {
                ExpenseType newObj = new ExpenseType();
                return View(newObj);
            }

            //edit ExpenseType
            ExpenseType obj = new ExpenseType();
            obj = _context.ExpenseType.Where(x => x.ExpenseTypeId.Equals(id)).FirstOrDefault();

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //post submitted ExpenseType data. if id is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitExpenseTypeForm([Bind("ExpenseTypeId", "Name", "Description")]ExpenseType ExpenseType)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(ExpenseTypeForm), new { id = ExpenseType.ExpenseTypeId ?? "" });
                }

                //create new
                if (ExpenseType.ExpenseTypeId == null)
                {
                    if (await _context.ExpenseType.AnyAsync(x => x.Name.Equals(ExpenseType.Name)))
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + ExpenseType.Name + " already exist";
                        return RedirectToAction(nameof(ExpenseTypeForm), new { id = ExpenseType.ExpenseTypeId ?? "" });
                    }

                    ExpenseType newObj = new ExpenseType();
                    newObj.ExpenseTypeId = Guid.NewGuid().ToString();
                    newObj.Name = ExpenseType.Name;
                    newObj.Description = ExpenseType.Description;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.ExpenseType.Add(newObj);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new item success.";
                    return RedirectToAction(nameof(ExpenseTypeForm), new { id = newObj.ExpenseTypeId ?? "" });
                }

                //edit existing
                ExpenseType editObj = new ExpenseType();
                ExpenseType existObj = new ExpenseType();
                editObj = await _context.ExpenseType.Where(x => x.ExpenseTypeId.Equals(ExpenseType.ExpenseTypeId)).FirstOrDefaultAsync();
                existObj = await _context.ExpenseType.Where(x => x.Name.Equals(ExpenseType.Name)).FirstOrDefaultAsync();

                if (existObj != null)
                {
                    if (editObj.ExpenseTypeId != existObj.ExpenseTypeId)
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + ExpenseType.Name + " already exist";
                        return RedirectToAction(nameof(ExpenseTypeForm), new { id = ExpenseType.ExpenseTypeId ?? "" });
                    }

                }

                editObj.Name = ExpenseType.Name;
                editObj.Description = ExpenseType.Description;
                editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                editObj.UpdatedAtUtc = DateTime.UtcNow;

                _context.Update(editObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Edit existing item success.";
                return RedirectToAction(nameof(ExpenseTypeForm), new { id = ExpenseType.ExpenseTypeId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(ExpenseTypeForm), new { id = ExpenseType.ExpenseTypeId ?? "" });
            }
        }

        //display item for deletion
        [HttpGet]
        public async Task<IActionResult> ExpenseTypeDelete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = await _context.ExpenseType.Where(x => x.ExpenseTypeId.Equals(id)).FirstOrDefaultAsync();
            return View(obj);
        }

        //delete submitted item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitExpenseTypeDelete([Bind("ExpenseTypeId")]ExpenseType ExpenseType)
        {
            try
            {
                var deleteObj = await _context.ExpenseType.Where(x => x.ExpenseTypeId.Equals(ExpenseType.ExpenseTypeId)).FirstOrDefaultAsync();
                if (deleteObj == null)
                {
                    return NotFound();
                }

                //cek existing expense
                Expense objCheck = new Expense();
                objCheck = await _context.Expense
                    .Where(x => x.ExpenseTypeId.Equals(deleteObj.ExpenseTypeId))
                    .FirstOrDefaultAsync();

                if (objCheck != null)
                {
                    TempData[StaticString.StatusMessage] = "Error: already used on transaction expense.";
                    return RedirectToAction(nameof(ExpenseTypeDelete), new { id = ExpenseType.ExpenseTypeId ?? "" });
                }

                //cek existing cash advance
                PayrollLineCashAdvance objCheckCA = new PayrollLineCashAdvance();
                objCheckCA = await _context.PayrollLineCashAdvance
                    .Where(x => x.ExpenseTypeId.Equals(deleteObj.ExpenseTypeId))
                    .FirstOrDefaultAsync();

                if (objCheckCA != null)
                {
                    TempData[StaticString.StatusMessage] = "Error: already used on transaction payroll cash advance.";
                    return RedirectToAction(nameof(ExpenseTypeDelete), new { id = ExpenseType.ExpenseTypeId ?? "" });
                }

                //cek existing reimbursement
                PayrollLineReimburse objCheckR = new PayrollLineReimburse();
                objCheckR = await _context.PayrollLineReimburse
                    .Where(x => x.ExpenseTypeId.Equals(deleteObj.ExpenseTypeId))
                    .FirstOrDefaultAsync();

                if (objCheckR != null)
                {
                    TempData[StaticString.StatusMessage] = "Error: already used on transaction payroll reimbursement.";
                    return RedirectToAction(nameof(ExpenseTypeDelete), new { id = ExpenseType.ExpenseTypeId ?? "" });
                }

                _context.ExpenseType.Remove(deleteObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete item success.";
                return RedirectToAction(nameof(ExpenseTypeIndex));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(ExpenseTypeDelete), new { id = ExpenseType.ExpenseTypeId ?? "" });
            }
        }

    }
}