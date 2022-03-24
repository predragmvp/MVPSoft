using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using coderush.Data;
using coderush.Models;
using coderush.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace coderush.Controllers
{
    [Authorize(Roles = Services.App.Pages.Payroll.RoleName)]
    public class PayrollController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Services.App.ICommon _app;

        //dependency injection through constructor, to directly access services
        public PayrollController(
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
            ViewData["OnBehalf"] = _app.GetEmployeeSelectList();
            ViewData["Allowance"] = _app.GetAllowanceTypeSelectList();
            ViewData["Deduction"] = _app.GetDeductionTypeSelectList();
            ViewData["ExpenseType"] = _app.GetExpenseTypeSelectList();
            ViewData["Leave"] = _app.GetLeaveSelectList();
        }

        //display payroll execution per month
        public IActionResult Index(string period)
        {
            var Payrolls = _context.Payroll                
                .Include(x => x.OnBehalf)
                .OrderByDescending(x => x.CreatedAtUtc)
                .ToList()
                .Where(x => x.Periode.ToString("yyyy-MM").Equals(period));
            return View(Payrolls.ToList());
        }

        //display Payroll create edit form
        [HttpGet]
        public IActionResult Form(string id)
        {

            //create new
            if (id == null)
            {
                //dropdownlist type
                FillDropdownListWithData();

                Payroll newPayroll = new Payroll();
                newPayroll.Periode = DateTime.Now;
                return View(newPayroll);
            }

            //edit Payroll
            Payroll Payroll = new Payroll();
            Payroll = _context.Payroll
                .Include(x => x.LinesBasic)
                .Include(x => x.LinesAllowance)
                    .ThenInclude(y => y.AllowanceType)
                .Include(x => x.LinesDeduction)
                    .ThenInclude(y => y.DeductionType)
                .Include(x => x.LinesCashAdvance)
                    .ThenInclude(y => y.ExpenseType)
                .Include(x => x.LinesReimburse)
                    .ThenInclude(y => y.ExpenseType)
                .Include(x => x.LinesUnpaidLeave)
                    .ThenInclude(y => y.Leave)
                .Include(x => x.OnBehalf)
                .Where(x => x.PayrollId.Equals(id)).FirstOrDefault();

            if (Payroll == null)
            {
                return NotFound();
            }
            //dropdownlist type
            FillDropdownListWithData();

            return View(Payroll);

        }

        //post submitted Payroll data. if Payroll.PayrollId is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitForm([Bind(
            "PayrollId",
            "PayrollName",
            "IsApproved",
            "IsPaid",
            "Periode",
            "Description",
            "OnBehalfId"
            )]Payroll Payroll)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(Form), new { id = Payroll.PayrollId ?? "" });
                }

                //create new
                if (Payroll.PayrollId == null)
                {
                    Payroll newPayroll = new Payroll();
                    newPayroll.PayrollId = Guid.NewGuid().ToString();
                    newPayroll.PayrollName = Payroll.PayrollName;
                    newPayroll.Periode = Payroll.Periode;
                    newPayroll.IsApproved = Payroll.IsApproved;
                    newPayroll.IsPaid = Payroll.IsPaid;
                    newPayroll.Description = Payroll.Description;
                    newPayroll.OnBehalfId = Payroll.OnBehalfId;
                    newPayroll.CreatedBy = await _userManager.GetUserAsync(User);
                    newPayroll.CreatedAtUtc = DateTime.UtcNow;
                    newPayroll.UpdatedBy = newPayroll.CreatedBy;
                    newPayroll.UpdatedAtUtc = newPayroll.CreatedAtUtc;

                    _context.Payroll.Add(newPayroll);
                    _context.SaveChanges();

                    //dropdownlist type
                    FillDropdownListWithData();

                    TempData[StaticString.StatusMessage] = "Create new Payroll item success.";
                    return RedirectToAction(nameof(Form), new { id = newPayroll.PayrollId ?? "" });
                }

                //edit existing
                Payroll editPayroll = new Payroll();
                editPayroll = _context.Payroll.Where(x => x.PayrollId.Equals(Payroll.PayrollId)).FirstOrDefault();
                editPayroll.PayrollName = Payroll.PayrollName;
                editPayroll.Periode = Payroll.Periode;
                editPayroll.IsApproved = Payroll.IsApproved;
                editPayroll.IsPaid = Payroll.IsPaid;
                editPayroll.Description = Payroll.Description;
                editPayroll.OnBehalfId = Payroll.OnBehalfId;
                editPayroll.UpdatedBy = await _userManager.GetUserAsync(User);
                editPayroll.UpdatedAtUtc = DateTime.UtcNow;
                _context.Update(editPayroll);
                _context.SaveChanges();

                //dropdownlist type
                FillDropdownListWithData();

                TempData[StaticString.StatusMessage] = "Edit existing Payroll item success.";
                return RedirectToAction(nameof(Form), new { id = Payroll.PayrollId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Form), new { id = Payroll.PayrollId ?? "" });
            }
        }

        //display Payroll item for deletion
        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //dropdownlist type
            FillDropdownListWithData();

            var Payroll = _context.Payroll
                .Include(x => x.OnBehalf)
                .Where(x => x.PayrollId.Equals(id)).FirstOrDefault();
            return View(Payroll);
        }

        //delete submitted Payroll item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitDelete([Bind("PayrollId")]Payroll Payroll)
        {
            try
            {
                var deletePayroll = _context.Payroll.Where(x => x.PayrollId.Equals(Payroll.PayrollId)).FirstOrDefault();
                if (deletePayroll == null)
                {
                    return NotFound();
                }

                _context.Payroll.Remove(deletePayroll);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete Payroll item success.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Delete), new { id = Payroll.PayrollId ?? "" });
            }
        }



        //display DeductionType
        public IActionResult DeductionTypeIndex()
        {
            var objs = _context.DeductionType.OrderByDescending(x => x.CreatedAtUtc).ToList();
            return View(objs);
        }

        //display DeductionType create edit form
        [HttpGet]
        public IActionResult DeductionTypeForm(string id)
        {
            //create new
            if (id == null)
            {
                DeductionType newObj = new DeductionType();
                return View(newObj);
            }

            //edit DeductionType
            DeductionType obj = new DeductionType();
            obj = _context.DeductionType.Where(x => x.DeductionTypeId.Equals(id)).FirstOrDefault();

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //post submitted DeductionType data. if id is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitDeductionTypeForm([Bind("DeductionTypeId", "Name", "Description")]DeductionType deductionType)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(DeductionTypeForm), new { id = deductionType.DeductionTypeId ?? "" });
                }

                //create new
                if (deductionType.DeductionTypeId == null)
                {
                    if (await _context.DeductionType.AnyAsync(x => x.Name.Equals(deductionType.Name)))
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + deductionType.Name + " already exist";
                        return RedirectToAction(nameof(DeductionTypeForm), new { id = deductionType.DeductionTypeId ?? "" });
                    }

                    DeductionType newObj = new DeductionType();
                    newObj.DeductionTypeId = Guid.NewGuid().ToString();
                    newObj.Name = deductionType.Name;
                    newObj.Description = deductionType.Description;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.DeductionType.Add(newObj);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new item success.";
                    return RedirectToAction(nameof(DeductionTypeForm), new { id = newObj.DeductionTypeId ?? "" });
                }

                //edit existing
                DeductionType editObj = new DeductionType();
                DeductionType existObj = new DeductionType();
                editObj = await _context.DeductionType.Where(x => x.DeductionTypeId.Equals(deductionType.DeductionTypeId)).FirstOrDefaultAsync();
                existObj = await _context.DeductionType.Where(x => x.Name.Equals(deductionType.Name)).FirstOrDefaultAsync();

                if (existObj != null)
                {
                    if (editObj.DeductionTypeId != existObj.DeductionTypeId)
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + deductionType.Name + " already exist";
                        return RedirectToAction(nameof(DeductionTypeForm), new { id = deductionType.DeductionTypeId ?? "" });
                    }

                }

                editObj.Name = deductionType.Name;
                editObj.Description = deductionType.Description;
                editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                editObj.UpdatedAtUtc = DateTime.UtcNow;

                _context.Update(editObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Edit existing item success.";
                return RedirectToAction(nameof(DeductionTypeForm), new { id = deductionType.DeductionTypeId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(DeductionTypeForm), new { id = deductionType.DeductionTypeId ?? "" });
            }
        }

        //display item for deletion
        [HttpGet]
        public async Task<IActionResult> DeductionTypeDelete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = await _context.DeductionType.Where(x => x.DeductionTypeId.Equals(id)).FirstOrDefaultAsync();
            return View(obj);
        }

        //delete submitted item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitDeductionTypeDelete([Bind("DeductionTypeId")]DeductionType deductionType)
        {
            try
            {
                var deleteObj = await _context.DeductionType.Where(x => x.DeductionTypeId.Equals(deductionType.DeductionTypeId)).FirstOrDefaultAsync();
                if (deleteObj == null)
                {
                    return NotFound();
                }

                //todo: cek existing ke payroll / template

                _context.DeductionType.Remove(deleteObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete item success.";
                return RedirectToAction(nameof(DeductionTypeIndex));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(DeductionTypeDelete), new { id = deductionType.DeductionTypeId ?? "" });
            }
        }

        //Display allowance type 
        public IActionResult AllowanceTypeIndex()
        {
            var objs = _context.AllowanceType.OrderByDescending(x => x.CreatedAtUtc).ToList();
            return View(objs);
        }

        //display AllowanceType create edit form
        [HttpGet]
        public IActionResult AllowanceTypeForm(string id)
        {
            //create new
            if (id == null)
            {
                AllowanceType newObj = new AllowanceType();
                return View(newObj);
            }

            //edit AllowanceType
            AllowanceType obj = new AllowanceType();
            obj = _context.AllowanceType.Where(x => x.AllowanceTypeId.Equals(id)).FirstOrDefault();

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //post submitted AllowanceType data. if id is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitAllowanceTypeForm([Bind("AllowanceTypeId", "Name", "Description")]AllowanceType allowanceType)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(AllowanceTypeForm), new { id = allowanceType.AllowanceTypeId ?? "" });
                }

                //create new
                if (allowanceType.AllowanceTypeId == null)
                {
                    if (await _context.AllowanceType.AnyAsync(x => x.Name.Equals(allowanceType.Name)))
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + allowanceType.Name + " already exist";
                        return RedirectToAction(nameof(AllowanceTypeForm), new { id = allowanceType.AllowanceTypeId ?? "" });
                    }

                    AllowanceType newObj = new AllowanceType();
                    newObj.AllowanceTypeId = Guid.NewGuid().ToString();
                    newObj.Name = allowanceType.Name;
                    newObj.Description = allowanceType.Description;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.AllowanceType.Add(newObj);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new item success.";
                    return RedirectToAction(nameof(AllowanceTypeForm), new { id = newObj.AllowanceTypeId ?? "" });
                }

                //edit existing
                AllowanceType editObj = new AllowanceType();
                AllowanceType existObj = new AllowanceType();
                editObj = await _context.AllowanceType.Where(x => x.AllowanceTypeId.Equals(allowanceType.AllowanceTypeId)).FirstOrDefaultAsync();
                existObj = await _context.AllowanceType.Where(x => x.Name.Equals(allowanceType.Name)).FirstOrDefaultAsync();

                if (existObj != null)
                {
                    if (editObj.AllowanceTypeId != existObj.AllowanceTypeId)
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + allowanceType.Name + " already exist";
                        return RedirectToAction(nameof(AllowanceTypeForm), new { id = allowanceType.AllowanceTypeId ?? "" });
                    }

                }

                editObj.Name = allowanceType.Name;
                editObj.Description = allowanceType.Description;
                editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                editObj.UpdatedAtUtc = DateTime.UtcNow;

                _context.Update(editObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Edit existing item success.";
                return RedirectToAction(nameof(AllowanceTypeForm), new { id = allowanceType.AllowanceTypeId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(AllowanceTypeForm), new { id = allowanceType.AllowanceTypeId ?? "" });
            }
        }

        //display item for AllowanceType
        [HttpGet]
        public async Task<IActionResult> AllowanceTypeDelete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = await _context.AllowanceType.Where(x => x.AllowanceTypeId.Equals(id)).FirstOrDefaultAsync();
            return View(obj);
        }

        //delete submitted item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitAllowanceTypeDelete([Bind("AllowanceTypeId")]AllowanceType allowanceType)
        {
            try
            {
                var deleteObj = await _context.AllowanceType.Where(x => x.AllowanceTypeId.Equals(allowanceType.AllowanceTypeId)).FirstOrDefaultAsync();
                if (deleteObj == null)
                {
                    return NotFound();
                }

                //todo: cek existing ke benefit package

                _context.AllowanceType.Remove(deleteObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete item success.";
                return RedirectToAction(nameof(AllowanceTypeIndex));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(AllowanceTypeDelete), new { id = allowanceType.AllowanceTypeId ?? "" });
            }
        }



        public IActionResult BenefitTemplateIndex()
        {
            var objs = _context.BenefitTemplate.OrderByDescending(x => x.CreatedAtUtc).ToList();
            return View(objs);
        }

        //display BenefitTemplate create edit form
        [HttpGet]
        public IActionResult BenefitTemplateForm(string id)
        {
            //create new
            if (id == null)
            {
                BenefitTemplate newObj = new BenefitTemplate();
                return View(newObj);
            }

            //edit BenefitTemplate
            BenefitTemplate obj = new BenefitTemplate();
            obj = _context.BenefitTemplate
                .Include(x => x.Lines)
                .Where(x => x.BenefitTemplateId.Equals(id))
                .FirstOrDefault();

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //post submitted BenefitTemplate data. if id is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitBenefitTemplateForm([Bind("BenefitTemplateId", "Name", "Description")]BenefitTemplate BenefitTemplate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(BenefitTemplateForm), new { id = BenefitTemplate.BenefitTemplateId ?? "" });
                }

                //create new
                if (BenefitTemplate.BenefitTemplateId == null)
                {
                    if (await _context.BenefitTemplate.AnyAsync(x => x.Name.Equals(BenefitTemplate.Name)))
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + BenefitTemplate.Name + " already exist";
                        return RedirectToAction(nameof(BenefitTemplateForm), new { id = BenefitTemplate.BenefitTemplateId ?? "" });
                    }

                    BenefitTemplate newObj = new BenefitTemplate();
                    newObj.BenefitTemplateId = Guid.NewGuid().ToString();
                    newObj.Name = BenefitTemplate.Name;
                    newObj.Description = BenefitTemplate.Description;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.BenefitTemplate.Add(newObj);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new item success.";
                    return RedirectToAction(nameof(BenefitTemplateForm), new { id = newObj.BenefitTemplateId ?? "" });
                }

                //edit existing
                BenefitTemplate editObj = new BenefitTemplate();
                BenefitTemplate existObj = new BenefitTemplate();
                editObj = await _context.BenefitTemplate.Where(x => x.BenefitTemplateId.Equals(BenefitTemplate.BenefitTemplateId)).FirstOrDefaultAsync();
                existObj = await _context.BenefitTemplate.Where(x => x.Name.Equals(BenefitTemplate.Name)).FirstOrDefaultAsync();

                if (existObj != null)
                {
                    if (editObj.BenefitTemplateId != existObj.BenefitTemplateId)
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + BenefitTemplate.Name + " already exist";
                        return RedirectToAction(nameof(BenefitTemplateForm), new { id = BenefitTemplate.BenefitTemplateId ?? "" });
                    }

                }

                editObj.Name = BenefitTemplate.Name;
                editObj.Description = BenefitTemplate.Description;
                editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                editObj.UpdatedAtUtc = DateTime.UtcNow;

                _context.Update(editObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Edit existing item success.";
                return RedirectToAction(nameof(BenefitTemplateForm), new { id = BenefitTemplate.BenefitTemplateId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(BenefitTemplateForm), new { id = BenefitTemplate.BenefitTemplateId ?? "" });
            }
        }

        //display item for deletion
        [HttpGet]
        public async Task<IActionResult> BenefitTemplateDelete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = await _context.BenefitTemplate.Where(x => x.BenefitTemplateId.Equals(id)).FirstOrDefaultAsync();
            return View(obj);
        }

        //delete submitted item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitBenefitTemplateDelete([Bind("BenefitTemplateId")]BenefitTemplate BenefitTemplate)
        {
            try
            {
                var deleteObj = await _context.BenefitTemplate.Where(x => x.BenefitTemplateId.Equals(BenefitTemplate.BenefitTemplateId)).FirstOrDefaultAsync();
                if (deleteObj == null)
                {
                    return NotFound();
                }

                //todo: cek existing ke employee

                _context.BenefitTemplate.Remove(deleteObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete item success.";
                return RedirectToAction(nameof(BenefitTemplateIndex));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(BenefitTemplateDelete), new { id = BenefitTemplate.BenefitTemplateId ?? "" });
            }
        }

        //display BenefitTemplateLine create edit form
        [HttpGet]
        public IActionResult BenefitTemplateLineForm(string id, string header)
        {
            //dropdownlist type
            FillDropdownListWithData();

            //create new
            if (id == null)
            {
                BenefitTemplateLine newObj = new BenefitTemplateLine();
                newObj.BenefitTemplateId = header;
                return View(newObj);
            }

            //edit BenefitTemplateLine
            BenefitTemplateLine obj = new BenefitTemplateLine();
            obj = _context.BenefitTemplateLine.Where(x => x.BenefitTemplateLineId.Equals(id)).FirstOrDefault();

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //post submitted BenefitTemplateLine data. if id is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitBenefitTemplateLineForm([Bind(
            "BenefitTemplateLineId", 
            "BenefitTemplateId",
            "AllowanceTypeId",
            "DeductionTypeId",
            "Amount",
            "Description")]BenefitTemplateLine BenefitTemplateLine)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(BenefitTemplateLineForm), new { id = BenefitTemplateLine.BenefitTemplateLineId ?? "", header = BenefitTemplateLine.BenefitTemplateId });
                }

                //dropdownlist type
                FillDropdownListWithData();

                //create new
                if (BenefitTemplateLine.BenefitTemplateLineId == null)
                {


                    BenefitTemplateLine newObj = new BenefitTemplateLine();
                    newObj.BenefitTemplateLineId = Guid.NewGuid().ToString();
                    newObj.BenefitTemplateId = BenefitTemplateLine.BenefitTemplateId;
                    newObj.Description = BenefitTemplateLine.Description;
                    newObj.AllowanceTypeId = BenefitTemplateLine.AllowanceTypeId;
                    newObj.DeductionTypeId = BenefitTemplateLine.DeductionTypeId;
                    newObj.Amount = BenefitTemplateLine.Amount;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    //set priority, per online online allowed one allowance or deduction or cash advance or reimburse
                    if (!String.IsNullOrEmpty(newObj.AllowanceTypeId))
                    {
                        newObj.DeductionTypeId = null;
                        //allowance always positif
                        newObj.Amount = newObj.Amount < 0 ? newObj.Amount * -1m : newObj.Amount * 1m;
                    }
                    if (!String.IsNullOrEmpty(newObj.DeductionTypeId))
                    {
                        //deduction always negatif
                        newObj.Amount = newObj.Amount > 0 ? newObj.Amount * -1m : newObj.Amount * 1m;
                    }

                    _context.BenefitTemplateLine.Add(newObj);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new item success.";
                    return RedirectToAction(nameof(BenefitTemplateLineForm), new { id = newObj.BenefitTemplateLineId ?? "", header = newObj.BenefitTemplateId });
                }

                //edit existing
                BenefitTemplateLine editObj = new BenefitTemplateLine();
                BenefitTemplateLine existObj = new BenefitTemplateLine();
                editObj = await _context.BenefitTemplateLine.Where(x => x.BenefitTemplateLineId.Equals(BenefitTemplateLine.BenefitTemplateLineId)).FirstOrDefaultAsync();
                editObj.Description = BenefitTemplateLine.Description;
                editObj.AllowanceTypeId = BenefitTemplateLine.AllowanceTypeId;
                editObj.DeductionTypeId = BenefitTemplateLine.DeductionTypeId;
                editObj.Amount = BenefitTemplateLine.Amount;
                editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                editObj.UpdatedAtUtc = DateTime.UtcNow;

                //set priority, per online online allowed one allowance or deduction or cash advance or reimburse
                if (!String.IsNullOrEmpty(editObj.AllowanceTypeId))
                {
                    editObj.DeductionTypeId = null;
                    //allowance always positif
                    editObj.Amount = editObj.Amount < 0 ? editObj.Amount * -1m : editObj.Amount * 1m;
                }
                if (!String.IsNullOrEmpty(editObj.DeductionTypeId))
                {
                    //deduction always negatif
                    editObj.Amount = editObj.Amount > 0 ? editObj.Amount * -1m : editObj.Amount * 1m;
                }

                _context.Update(editObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Edit existing item success.";
                return RedirectToAction(nameof(BenefitTemplateLineForm), new { id = BenefitTemplateLine.BenefitTemplateLineId ?? "", header = BenefitTemplateLine.BenefitTemplateId });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(BenefitTemplateLineForm), new { id = BenefitTemplateLine.BenefitTemplateLineId ?? "", header = BenefitTemplateLine.BenefitTemplateId });
            }
        }

        //display item for deletion
        [HttpGet]
        public async Task<IActionResult> BenefitTemplateLineDelete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //dropdownlist type
            FillDropdownListWithData();
            var obj = await _context.BenefitTemplateLine.Where(x => x.BenefitTemplateLineId.Equals(id)).FirstOrDefaultAsync();
            return View(obj);
        }

        //delete submitted item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitBenefitTemplateLineDelete([Bind("BenefitTemplateLineId")]BenefitTemplateLine BenefitTemplateLine)
        {
            try
            {
                var deleteObj = await _context.BenefitTemplateLine.Where(x => x.BenefitTemplateLineId.Equals(BenefitTemplateLine.BenefitTemplateLineId)).FirstOrDefaultAsync();
                if (deleteObj == null)
                {
                    return NotFound();
                }

                //todo: cek existing ke ..

                _context.BenefitTemplateLine.Remove(deleteObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete item success.";
                return RedirectToAction(nameof(BenefitTemplateForm), new { id = deleteObj.BenefitTemplateId });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(BenefitTemplateLineDelete), new { id = BenefitTemplateLine.BenefitTemplateLineId ?? "", header = BenefitTemplateLine.BenefitTemplateId });
            }
        }


        [HttpGet]
        public async Task<IActionResult> ComponentBasicForm(string payrollId, string id, string period)
        {
            //check to see heder id 
            if (String.IsNullOrEmpty(payrollId))
            {
                return NotFound();
            }

            //check if payroll is not paid
            Payroll payroll = new Payroll();
            payroll = await _context.Payroll
                .Include(x => x.OnBehalf)
                .Where(x => x.PayrollId.Equals(payrollId)).FirstOrDefaultAsync();
            if (payroll != null && payroll.IsPaid)
            {
                TempData[StaticString.StatusMessage] = "Error: Already paid payroll. Component basic can not add or edit.";
                return RedirectToAction(nameof(Form), new { id = payrollId, period });
            }

            PayrollLineBasic basic = new PayrollLineBasic();

            //new
            if (id == null)
            {
                basic.Payroll = payroll;
                basic.PayrollId = payroll.PayrollId;
                return View(basic);
            }

            
            basic = await _context.PayrollLineBasic.Where(x => x.PayrollLineBasicId.Equals(id)).FirstOrDefaultAsync();
            if (basic == null)
            {
                TempData[StaticString.StatusMessage] = "Error: PayrollLineBasic can not found.";
                return RedirectToAction(nameof(Form), new { id = payrollId, period });
            }

            return View(basic);
        }


        [HttpGet]
        public async Task<IActionResult> ComponentBasicDelete(string payrollId, string id, string period)
        {
            //check to see heder id 
            if (String.IsNullOrEmpty(payrollId))
            {
                return NotFound();
            }

            //check if payroll is not paid
            Payroll payroll = new Payroll();
            payroll = await _context.Payroll
                .Include(x => x.OnBehalf)
                .Where(x => x.PayrollId.Equals(payrollId)).FirstOrDefaultAsync();
            if (payroll != null && payroll.IsPaid)
            {
                TempData[StaticString.StatusMessage] = "Error: Already paid payroll. Component basic can not delete.";
                return RedirectToAction(nameof(Form), new { id = payrollId, period });
            }

            PayrollLineBasic basic = new PayrollLineBasic();
            

            basic = await _context.PayrollLineBasic.Where(x => x.PayrollLineBasicId.Equals(id)).FirstOrDefaultAsync();
            if (basic == null)
            {
                TempData[StaticString.StatusMessage] = "Error: PayrollLineBasic can not found.";
                return RedirectToAction(nameof(Form), new { id = payrollId, period });
            }

            return View(basic);
        }


        //post submitted component basic. if id is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitComponentBasicForm([Bind(
            "PayrollLineBasicId", 
            "PayrollId",
            "Description",
            "Amount"
            )]PayrollLineBasic PayrollLineBasic)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(ComponentBasicForm), new { id = PayrollLineBasic.PayrollLineBasicId ?? "", payrollId = PayrollLineBasic.PayrollId });
                }

                //create new
                if (PayrollLineBasic.PayrollLineBasicId == null)
                {

                    PayrollLineBasic newObj = new PayrollLineBasic();
                    newObj.PayrollLineBasicId = Guid.NewGuid().ToString();
                    newObj.PayrollId = PayrollLineBasic.PayrollId;
                    newObj.Description = PayrollLineBasic.Description;
                    newObj.Amount = PayrollLineBasic.Amount;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.PayrollLineBasic.Add(newObj);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new item success.";
                    return RedirectToAction(nameof(ComponentBasicForm), new { id = newObj.PayrollLineBasicId, payrollId = PayrollLineBasic.PayrollId});
                }

                //edit existing
                PayrollLineBasic editObj = new PayrollLineBasic();
                editObj = await _context.PayrollLineBasic
                    .Where(x => x.PayrollLineBasicId.Equals(PayrollLineBasic.PayrollLineBasicId)).FirstOrDefaultAsync();
                
                
                
                editObj.Description = PayrollLineBasic.Description;
                editObj.Amount = PayrollLineBasic.Amount;
                editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                editObj.UpdatedAtUtc = DateTime.UtcNow;

                _context.Update(editObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Edit existing item success.";
                return RedirectToAction(nameof(ComponentBasicForm), new { id = PayrollLineBasic.PayrollLineBasicId ?? "", payrollId = PayrollLineBasic.PayrollId });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(ComponentBasicForm), new { id = PayrollLineBasic.PayrollLineBasicId ?? "", payrollId = PayrollLineBasic.PayrollId });
            }
        }

        //delete submitted item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitComponentBasicDelete([Bind("PayrollLineBasicId")]PayrollLineBasic PayrollLineBasic)
        {
            try
            {
                var deleteObj = await _context.PayrollLineBasic
                    .Where(x => x.PayrollLineBasicId.Equals(PayrollLineBasic.PayrollLineBasicId))
                    .FirstOrDefaultAsync();

                if (deleteObj == null)
                {
                    return NotFound();
                }
                

                _context.PayrollLineBasic.Remove(deleteObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete item success.";
                return RedirectToAction(nameof(Form), new { id = deleteObj.PayrollId });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(ComponentBasicDelete), new { id = PayrollLineBasic.PayrollLineBasicId ?? "", payrollId = PayrollLineBasic.PayrollId });
            }
        }




        [HttpGet]
        public async Task<IActionResult> ComponentAllowanceForm(string payrollId, string id, string period)
        {
            //check to see heder id 
            if (String.IsNullOrEmpty(payrollId))
            {
                return NotFound();
            }

            //check if payroll is not paid
            Payroll payroll = new Payroll();
            payroll = await _context.Payroll
                .Include(x => x.OnBehalf)
                .Where(x => x.PayrollId.Equals(payrollId)).FirstOrDefaultAsync();
            if (payroll != null && payroll.IsPaid)
            {
                TempData[StaticString.StatusMessage] = "Error: Already paid payroll. Component allowance can not add or edit.";
                return RedirectToAction(nameof(Form), new { id = payrollId, period });
            }

            FillDropdownListWithData();
            PayrollLineAllowance basic = new PayrollLineAllowance();

            //new
            if (id == null)
            {
                basic.Payroll = payroll;
                basic.PayrollId = payroll.PayrollId;
                return View(basic);
            }


            basic = await _context.PayrollLineAllowance.Where(x => x.PayrollLineAllowanceId.Equals(id)).FirstOrDefaultAsync();
            if (basic == null)
            {
                TempData[StaticString.StatusMessage] = "Error: PayrollLineAllowance can not found.";
                return RedirectToAction(nameof(Form), new { id = payrollId, period });
            }

            return View(basic);
        }


        [HttpGet]
        public async Task<IActionResult> ComponentAllowanceDelete(string payrollId, string id, string period)
        {
            //check to see heder id 
            if (String.IsNullOrEmpty(payrollId))
            {
                return NotFound();
            }

            //check if payroll is not paid
            Payroll payroll = new Payroll();
            payroll = await _context.Payroll
                .Include(x => x.OnBehalf)
                .Where(x => x.PayrollId.Equals(payrollId)).FirstOrDefaultAsync();
            if (payroll != null && payroll.IsPaid)
            {
                TempData[StaticString.StatusMessage] = "Error: Already paid payroll. Component allowance can not delete.";
                return RedirectToAction(nameof(Form), new { id = payrollId, period });
            }

            FillDropdownListWithData();
            PayrollLineAllowance basic = new PayrollLineAllowance();


            basic = await _context.PayrollLineAllowance.Where(x => x.PayrollLineAllowanceId.Equals(id)).FirstOrDefaultAsync();
            if (basic == null)
            {
                TempData[StaticString.StatusMessage] = "Error: PayrollLineAllowance can not found.";
                return RedirectToAction(nameof(Form), new { id = payrollId, period });
            }

            return View(basic);
        }

        //post submitted component allowance. if id is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitComponentAllowanceForm([Bind(
            "PayrollLineAllowanceId",
            "PayrollId",
            "Description",
            "AllowanceTypeId",
            "Amount"
            )]PayrollLineAllowance PayrollLineAllowance)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(ComponentAllowanceForm), new { id = PayrollLineAllowance.PayrollLineAllowanceId ?? "", payrollId = PayrollLineAllowance.PayrollId });
                }

                //create new
                if (PayrollLineAllowance.PayrollLineAllowanceId == null)
                {

                    PayrollLineAllowance newObj = new PayrollLineAllowance();
                    newObj.PayrollLineAllowanceId = Guid.NewGuid().ToString();
                    newObj.PayrollId = PayrollLineAllowance.PayrollId;
                    newObj.Description = PayrollLineAllowance.Description;
                    newObj.Amount = PayrollLineAllowance.Amount;
                    newObj.AllowanceTypeId = PayrollLineAllowance.AllowanceTypeId;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.PayrollLineAllowance.Add(newObj);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new item success.";
                    return RedirectToAction(nameof(ComponentAllowanceForm), new { id = newObj.PayrollLineAllowanceId, payrollId = PayrollLineAllowance.PayrollId });
                }

                //edit existing
                PayrollLineAllowance editObj = new PayrollLineAllowance();
                editObj = await _context.PayrollLineAllowance
                    .Where(x => x.PayrollLineAllowanceId.Equals(PayrollLineAllowance.PayrollLineAllowanceId)).FirstOrDefaultAsync();



                editObj.Description = PayrollLineAllowance.Description;
                editObj.Amount = PayrollLineAllowance.Amount;
                editObj.AllowanceTypeId = PayrollLineAllowance.AllowanceTypeId;
                editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                editObj.UpdatedAtUtc = DateTime.UtcNow;

                _context.Update(editObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Edit existing item success.";
                return RedirectToAction(nameof(ComponentAllowanceForm), new { id = PayrollLineAllowance.PayrollLineAllowanceId ?? "", payrollId = PayrollLineAllowance.PayrollId });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(ComponentAllowanceForm), new { id = PayrollLineAllowance.PayrollLineAllowanceId ?? "", payrollId = PayrollLineAllowance.PayrollId });
            }
        }

        //delete submitted item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitComponentAllowanceDelete([Bind("PayrollLineAllowanceId")]PayrollLineAllowance PayrollLineAllowance)
        {
            try
            {
                var deleteObj = await _context.PayrollLineAllowance
                    .Where(x => x.PayrollLineAllowanceId.Equals(PayrollLineAllowance.PayrollLineAllowanceId))
                    .FirstOrDefaultAsync();

                if (deleteObj == null)
                {
                    return NotFound();
                }


                _context.PayrollLineAllowance.Remove(deleteObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete item success.";
                return RedirectToAction(nameof(Form), new { id = deleteObj.PayrollId });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(ComponentAllowanceDelete), new { id = PayrollLineAllowance.PayrollLineAllowanceId ?? "", payrollId = PayrollLineAllowance.PayrollId });
            }
        }


        [HttpGet]
        public async Task<IActionResult> ComponentDeductionForm(string payrollId, string id, string period)
        {
            //check to see heder id 
            if (String.IsNullOrEmpty(payrollId))
            {
                return NotFound();
            }

            //check if payroll is not paid
            Payroll payroll = new Payroll();
            payroll = await _context.Payroll
                .Include(x => x.OnBehalf)
                .Where(x => x.PayrollId.Equals(payrollId)).FirstOrDefaultAsync();
            if (payroll != null && payroll.IsPaid)
            {
                TempData[StaticString.StatusMessage] = "Error: Already paid payroll. Component deduction can not add or edit.";
                return RedirectToAction(nameof(Form), new { id = payrollId, period });
            }

            FillDropdownListWithData();
            PayrollLineDeduction basic = new PayrollLineDeduction();

            //new
            if (id == null)
            {
                basic.Payroll = payroll;
                basic.PayrollId = payroll.PayrollId;
                return View(basic);
            }


            basic = await _context.PayrollLineDeduction.Where(x => x.PayrollLineDeductionId.Equals(id)).FirstOrDefaultAsync();
            if (basic == null)
            {
                TempData[StaticString.StatusMessage] = "Error: PayrollLineDeduction can not found.";
                return RedirectToAction(nameof(Form), new { id = payrollId, period });
            }

            return View(basic);
        }


        [HttpGet]
        public async Task<IActionResult> ComponentDeductionDelete(string payrollId, string id, string period)
        {
            //check to see heder id 
            if (String.IsNullOrEmpty(payrollId))
            {
                return NotFound();
            }

            //check if payroll is not paid
            Payroll payroll = new Payroll();
            payroll = await _context.Payroll
                .Include(x => x.OnBehalf)
                .Where(x => x.PayrollId.Equals(payrollId)).FirstOrDefaultAsync();
            if (payroll != null && payroll.IsPaid)
            {
                TempData[StaticString.StatusMessage] = "Error: Already paid payroll. Component deduction can not delete.";
                return RedirectToAction(nameof(Form), new { id = payrollId, period });
            }

            FillDropdownListWithData();
            PayrollLineDeduction basic = new PayrollLineDeduction();


            basic = await _context.PayrollLineDeduction.Where(x => x.PayrollLineDeductionId.Equals(id)).FirstOrDefaultAsync();
            if (basic == null)
            {
                TempData[StaticString.StatusMessage] = "Error: PayrollLineDeduction can not found.";
                return RedirectToAction(nameof(Form), new { id = payrollId, period });
            }

            return View(basic);
        }

        //post submitted component deduction. if id is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitComponentDeductionForm([Bind(
            "PayrollLineDeductionId",
            "PayrollId",
            "Description",
            "DeductionTypeId",
            "Amount"
            )]PayrollLineDeduction PayrollLineDeduction)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(ComponentDeductionForm), new { id = PayrollLineDeduction.PayrollLineDeductionId ?? "", payrollId = PayrollLineDeduction.PayrollId });
                }

                //create new
                if (PayrollLineDeduction.PayrollLineDeductionId == null)
                {

                    PayrollLineDeduction newObj = new PayrollLineDeduction();
                    newObj.PayrollLineDeductionId = Guid.NewGuid().ToString();
                    newObj.PayrollId = PayrollLineDeduction.PayrollId;
                    newObj.Description = PayrollLineDeduction.Description;
                    //deduction should always negatif
                    newObj.Amount = PayrollLineDeduction.Amount > 0 ? PayrollLineDeduction.Amount * -1m : PayrollLineDeduction.Amount;
                    newObj.DeductionTypeId = PayrollLineDeduction.DeductionTypeId;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.PayrollLineDeduction.Add(newObj);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new item success.";
                    return RedirectToAction(nameof(ComponentDeductionForm), new { id = newObj.PayrollLineDeductionId, payrollId = PayrollLineDeduction.PayrollId });
                }

                //edit existing
                PayrollLineDeduction editObj = new PayrollLineDeduction();
                editObj = await _context.PayrollLineDeduction
                    .Where(x => x.PayrollLineDeductionId.Equals(PayrollLineDeduction.PayrollLineDeductionId)).FirstOrDefaultAsync();



                editObj.Description = PayrollLineDeduction.Description;
                //deduction should always negatif
                editObj.Amount = PayrollLineDeduction.Amount > 0 ? PayrollLineDeduction.Amount * -1m : PayrollLineDeduction.Amount;
                editObj.DeductionTypeId = PayrollLineDeduction.DeductionTypeId;
                editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                editObj.UpdatedAtUtc = DateTime.UtcNow;

                _context.Update(editObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Edit existing item success.";
                return RedirectToAction(nameof(ComponentDeductionForm), new { id = PayrollLineDeduction.PayrollLineDeductionId ?? "", payrollId = PayrollLineDeduction.PayrollId });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(ComponentDeductionForm), new { id = PayrollLineDeduction.PayrollLineDeductionId ?? "", payrollId = PayrollLineDeduction.PayrollId });
            }
        }

        //delete submitted item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitComponentDeductionDelete([Bind("PayrollLineDeductionId")]PayrollLineDeduction PayrollLineDeduction)
        {
            try
            {
                var deleteObj = await _context.PayrollLineDeduction
                    .Where(x => x.PayrollLineDeductionId.Equals(PayrollLineDeduction.PayrollLineDeductionId))
                    .FirstOrDefaultAsync();

                if (deleteObj == null)
                {
                    return NotFound();
                }


                _context.PayrollLineDeduction.Remove(deleteObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete item success.";
                return RedirectToAction(nameof(Form), new { id = deleteObj.PayrollId });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(ComponentDeductionDelete), new { id = PayrollLineDeduction.PayrollLineDeductionId ?? "", payrollId = PayrollLineDeduction.PayrollId });
            }
        }


        [HttpGet]
        public async Task<IActionResult> ComponentUnpaidLeaveForm(string payrollId, string id, string period)
        {
            //check to see heder id 
            if (String.IsNullOrEmpty(payrollId))
            {
                return NotFound();
            }

            //check if payroll is not paid
            Payroll payroll = new Payroll();
            payroll = await _context.Payroll
                .Include(x => x.OnBehalf)
                .Where(x => x.PayrollId.Equals(payrollId)).FirstOrDefaultAsync();
            if (payroll != null && payroll.IsPaid)
            {
                TempData[StaticString.StatusMessage] = "Error: Already paid payroll. Component unpaid leave can not add or edit.";
                return RedirectToAction(nameof(Form), new { id = payrollId, period });
            }

            FillDropdownListWithData();
            PayrollLineUnpaidLeave basic = new PayrollLineUnpaidLeave();

            //new
            if (id == null)
            {
                basic.Payroll = payroll;
                basic.PayrollId = payroll.PayrollId;
                basic.UnpaidPerDay = payroll.OnBehalf.UnpaidLeavePerDay;
                return View(basic);
            }


            basic = await _context.PayrollLineUnpaidLeave.Where(x => x.PayrollLineUnpaidLeaveId.Equals(id)).FirstOrDefaultAsync();
            if (basic == null)
            {
                TempData[StaticString.StatusMessage] = "Error: PayrollLineUnpaidLeave can not found.";
                return RedirectToAction(nameof(Form), new { id = payrollId, period });
            }

            return View(basic);
        }


        [HttpGet]
        public async Task<IActionResult> ComponentUnpaidLeaveDelete(string payrollId, string id, string period)
        {
            //check to see heder id 
            if (String.IsNullOrEmpty(payrollId))
            {
                return NotFound();
            }

            //check if payroll is not paid
            Payroll payroll = new Payroll();
            payroll = await _context.Payroll
                .Include(x => x.OnBehalf)
                .Where(x => x.PayrollId.Equals(payrollId)).FirstOrDefaultAsync();
            if (payroll != null && payroll.IsPaid)
            {
                TempData[StaticString.StatusMessage] = "Error: Already paid payroll. Component unpaid leave can not delete.";
                return RedirectToAction(nameof(Form), new { id = payrollId, period });
            }

            FillDropdownListWithData();
            PayrollLineUnpaidLeave basic = new PayrollLineUnpaidLeave();


            basic = await _context.PayrollLineUnpaidLeave.Where(x => x.PayrollLineUnpaidLeaveId.Equals(id)).FirstOrDefaultAsync();
            if (basic == null)
            {
                TempData[StaticString.StatusMessage] = "Error: PayrollLineUnpaidLeave can not found.";
                return RedirectToAction(nameof(Form), new { id = payrollId, period });
            }

            return View(basic);
        }

        //post submitted component unpaid leave. if id is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitComponentUnpaidLeaveForm([Bind(
            "PayrollLineUnpaidLeaveId",
            "PayrollId",
            "Description",
            "LeaveId",
            "Amount"
            )]PayrollLineUnpaidLeave PayrollLineUnpaidLeave)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(ComponentUnpaidLeaveForm), new { id = PayrollLineUnpaidLeave.PayrollLineUnpaidLeaveId ?? "", payrollId = PayrollLineUnpaidLeave.PayrollId });
                }

                //create new
                if (PayrollLineUnpaidLeave.PayrollLineUnpaidLeaveId == null)
                {

                    PayrollLineUnpaidLeave newObj = new PayrollLineUnpaidLeave();
                    newObj.PayrollLineUnpaidLeaveId = Guid.NewGuid().ToString();
                    newObj.PayrollId = PayrollLineUnpaidLeave.PayrollId;
                    newObj.Description = PayrollLineUnpaidLeave.Description;
                    //unpaid should always negatif
                    newObj.Amount = PayrollLineUnpaidLeave.Amount > 0 ? PayrollLineUnpaidLeave.Amount * -1m : PayrollLineUnpaidLeave.Amount;
                    Leave leave = new Leave();
                    leave = await _context.Leave
                        .Include(x => x.OnBehalf)
                        .Where(x => x.LeaveId.Equals(PayrollLineUnpaidLeave.LeaveId))
                        .FirstOrDefaultAsync();
                    if (leave != null)
                    {
                        newObj.Days = (leave.ToDate.Date - leave.FromDate.Date).Days + 1;
                        newObj.UnpaidPerDay = leave.OnBehalf.UnpaidLeavePerDay;
                        newObj.Amount = (newObj.Days * 1m) * newObj.UnpaidPerDay;
                        newObj.Amount = newObj.Amount > 0 ? newObj.Amount * -1m : newObj.Amount;
                    }                    
                    newObj.LeaveId = PayrollLineUnpaidLeave.LeaveId;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.PayrollLineUnpaidLeave.Add(newObj);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new item success.";
                    return RedirectToAction(nameof(ComponentUnpaidLeaveForm), new { id = newObj.PayrollLineUnpaidLeaveId, payrollId = PayrollLineUnpaidLeave.PayrollId });
                }

                //edit existing
                PayrollLineUnpaidLeave editObj = new PayrollLineUnpaidLeave();
                editObj = await _context.PayrollLineUnpaidLeave
                    .Where(x => x.PayrollLineUnpaidLeaveId.Equals(PayrollLineUnpaidLeave.PayrollLineUnpaidLeaveId)).FirstOrDefaultAsync();



                editObj.Description = PayrollLineUnpaidLeave.Description;
                //unpaid should always negatif
                editObj.Amount = PayrollLineUnpaidLeave.Amount > 0 ? PayrollLineUnpaidLeave.Amount * -1m : PayrollLineUnpaidLeave.Amount;
                Leave leaveObj = new Leave();
                leaveObj = await _context.Leave
                    .Include(x => x.OnBehalf)
                    .Where(x => x.LeaveId.Equals(PayrollLineUnpaidLeave.LeaveId))
                    .FirstOrDefaultAsync();
                if (leaveObj != null)
                {
                    editObj.Days = (leaveObj.ToDate.Date - leaveObj.FromDate.Date).Days + 1;
                    editObj.UnpaidPerDay = leaveObj.OnBehalf.UnpaidLeavePerDay;
                    editObj.Amount = (editObj.Days * 1m) * editObj.UnpaidPerDay;
                    editObj.Amount = editObj.Amount > 0 ? editObj.Amount * -1m : editObj.Amount;
                }
                editObj.LeaveId = PayrollLineUnpaidLeave.LeaveId;
                editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                editObj.UpdatedAtUtc = DateTime.UtcNow;

                _context.Update(editObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Edit existing item success.";
                return RedirectToAction(nameof(ComponentUnpaidLeaveForm), new { id = PayrollLineUnpaidLeave.PayrollLineUnpaidLeaveId ?? "", payrollId = PayrollLineUnpaidLeave.PayrollId });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(ComponentUnpaidLeaveForm), new { id = PayrollLineUnpaidLeave.PayrollLineUnpaidLeaveId ?? "", payrollId = PayrollLineUnpaidLeave.PayrollId });
            }
        }

        //delete submitted item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitComponentUnpaidLeaveDelete([Bind("PayrollLineUnpaidLeaveId")]PayrollLineUnpaidLeave PayrollLineUnpaidLeave)
        {
            try
            {
                var deleteObj = await _context.PayrollLineUnpaidLeave
                    .Where(x => x.PayrollLineUnpaidLeaveId.Equals(PayrollLineUnpaidLeave.PayrollLineUnpaidLeaveId))
                    .FirstOrDefaultAsync();

                if (deleteObj == null)
                {
                    return NotFound();
                }


                _context.PayrollLineUnpaidLeave.Remove(deleteObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete item success.";
                return RedirectToAction(nameof(Form), new { id = deleteObj.PayrollId });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(ComponentUnpaidLeaveDelete), new { id = PayrollLineUnpaidLeave.PayrollLineUnpaidLeaveId ?? "", payrollId = PayrollLineUnpaidLeave.PayrollId });
            }
        }



        [HttpGet]
        public async Task<IActionResult> ComponentCashAdvanceForm(string payrollId, string id, string period)
        {
            //check to see heder id 
            if (String.IsNullOrEmpty(payrollId))
            {
                return NotFound();
            }

            //check if payroll is not paid
            Payroll payroll = new Payroll();
            payroll = await _context.Payroll
                .Include(x => x.OnBehalf)
                .Where(x => x.PayrollId.Equals(payrollId)).FirstOrDefaultAsync();
            if (payroll != null && payroll.IsPaid)
            {
                TempData[StaticString.StatusMessage] = "Error: Already paid payroll. Component cash advance can not add or edit.";
                return RedirectToAction(nameof(Form), new { id = payrollId, period });
            }

            FillDropdownListWithData();
            PayrollLineCashAdvance basic = new PayrollLineCashAdvance();

            //new
            if (id == null)
            {
                basic.Payroll = payroll;
                basic.PayrollId = payroll.PayrollId;
                return View(basic);
            }


            basic = await _context.PayrollLineCashAdvance.Where(x => x.PayrollLineCashAdvanceId.Equals(id)).FirstOrDefaultAsync();
            if (basic == null)
            {
                TempData[StaticString.StatusMessage] = "Error: PayrollLineCashAdvance can not found.";
                return RedirectToAction(nameof(Form), new { id = payrollId, period });
            }

            return View(basic);
        }


        [HttpGet]
        public async Task<IActionResult> ComponentCashAdvanceDelete(string payrollId, string id, string period)
        {
            //check to see heder id 
            if (String.IsNullOrEmpty(payrollId))
            {
                return NotFound();
            }

            //check if payroll is not paid
            Payroll payroll = new Payroll();
            payroll = await _context.Payroll
                .Include(x => x.OnBehalf)
                .Where(x => x.PayrollId.Equals(payrollId)).FirstOrDefaultAsync();
            if (payroll != null && payroll.IsPaid)
            {
                TempData[StaticString.StatusMessage] = "Error: Already paid payroll. Component cash advance can not delete.";
                return RedirectToAction(nameof(Form), new { id = payrollId, period });
            }

            FillDropdownListWithData();
            PayrollLineCashAdvance basic = new PayrollLineCashAdvance();


            basic = await _context.PayrollLineCashAdvance.Where(x => x.PayrollLineCashAdvanceId.Equals(id)).FirstOrDefaultAsync();
            if (basic == null)
            {
                TempData[StaticString.StatusMessage] = "Error: PayrollLineCashAdvance can not found.";
                return RedirectToAction(nameof(Form), new { id = payrollId, period });
            }

            return View(basic);
        }

        //post submitted component cash advance. if id is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitComponentCashAdvanceForm([Bind(
            "PayrollLineCashAdvanceId",
            "PayrollId",
            "Description",
            "ExpenseTypeId",
            "Amount"
            )]PayrollLineCashAdvance PayrollLineCashAdvance)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(ComponentCashAdvanceForm), new { id = PayrollLineCashAdvance.PayrollLineCashAdvanceId ?? "", payrollId = PayrollLineCashAdvance.PayrollId });
                }

                //create new
                if (PayrollLineCashAdvance.PayrollLineCashAdvanceId == null)
                {

                    PayrollLineCashAdvance newObj = new PayrollLineCashAdvance();
                    newObj.PayrollLineCashAdvanceId = Guid.NewGuid().ToString();
                    newObj.PayrollId = PayrollLineCashAdvance.PayrollId;
                    newObj.Description = PayrollLineCashAdvance.Description;
                    newObj.Amount = PayrollLineCashAdvance.Amount;
                    newObj.ExpenseTypeId = PayrollLineCashAdvance.ExpenseTypeId;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.PayrollLineCashAdvance.Add(newObj);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new item success.";
                    return RedirectToAction(nameof(ComponentCashAdvanceForm), new { id = newObj.PayrollLineCashAdvanceId, payrollId = PayrollLineCashAdvance.PayrollId });
                }

                //edit existing
                PayrollLineCashAdvance editObj = new PayrollLineCashAdvance();
                editObj = await _context.PayrollLineCashAdvance
                    .Where(x => x.PayrollLineCashAdvanceId.Equals(PayrollLineCashAdvance.PayrollLineCashAdvanceId)).FirstOrDefaultAsync();



                editObj.Description = PayrollLineCashAdvance.Description;
                editObj.Amount = PayrollLineCashAdvance.Amount;
                editObj.ExpenseTypeId = PayrollLineCashAdvance.ExpenseTypeId;
                editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                editObj.UpdatedAtUtc = DateTime.UtcNow;

                _context.Update(editObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Edit existing item success.";
                return RedirectToAction(nameof(ComponentCashAdvanceForm), new { id = PayrollLineCashAdvance.PayrollLineCashAdvanceId ?? "", payrollId = PayrollLineCashAdvance.PayrollId });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(ComponentCashAdvanceForm), new { id = PayrollLineCashAdvance.PayrollLineCashAdvanceId ?? "", payrollId = PayrollLineCashAdvance.PayrollId });
            }
        }

        //delete submitted item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitComponentCashAdvanceDelete([Bind("PayrollLineCashAdvanceId")]PayrollLineCashAdvance PayrollLineCashAdvance)
        {
            try
            {
                var deleteObj = await _context.PayrollLineCashAdvance
                    .Where(x => x.PayrollLineCashAdvanceId.Equals(PayrollLineCashAdvance.PayrollLineCashAdvanceId))
                    .FirstOrDefaultAsync();

                if (deleteObj == null)
                {
                    return NotFound();
                }


                _context.PayrollLineCashAdvance.Remove(deleteObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete item success.";
                return RedirectToAction(nameof(Form), new { id = deleteObj.PayrollId });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(ComponentCashAdvanceDelete), new { id = PayrollLineCashAdvance.PayrollLineCashAdvanceId ?? "", payrollId = PayrollLineCashAdvance.PayrollId });
            }
        }



        [HttpGet]
        public async Task<IActionResult> ComponentReimburseForm(string payrollId, string id, string period)
        {
            //check to see heder id 
            if (String.IsNullOrEmpty(payrollId))
            {
                return NotFound();
            }

            //check if payroll is not paid
            Payroll payroll = new Payroll();
            payroll = await _context.Payroll
                .Include(x => x.OnBehalf)
                .Where(x => x.PayrollId.Equals(payrollId)).FirstOrDefaultAsync();
            if (payroll != null && payroll.IsPaid)
            {
                TempData[StaticString.StatusMessage] = "Error: Already paid payroll. Component reimbursement can not add or edit.";
                return RedirectToAction(nameof(Form), new { id = payrollId, period });
            }

            FillDropdownListWithData();
            PayrollLineReimburse basic = new PayrollLineReimburse();

            //new
            if (id == null)
            {
                basic.Payroll = payroll;
                basic.PayrollId = payroll.PayrollId;
                return View(basic);
            }


            basic = await _context.PayrollLineReimburse.Where(x => x.PayrollLineReimburseId.Equals(id)).FirstOrDefaultAsync();
            if (basic == null)
            {
                TempData[StaticString.StatusMessage] = "Error: PayrollLineReimburse can not found.";
                return RedirectToAction(nameof(Form), new { id = payrollId, period });
            }

            return View(basic);
        }


        [HttpGet]
        public async Task<IActionResult> ComponentReimburseDelete(string payrollId, string id, string period)
        {
            //check to see heder id 
            if (String.IsNullOrEmpty(payrollId))
            {
                return NotFound();
            }

            //check if payroll is not paid
            Payroll payroll = new Payroll();
            payroll = await _context.Payroll
                .Include(x => x.OnBehalf)
                .Where(x => x.PayrollId.Equals(payrollId)).FirstOrDefaultAsync();
            if (payroll != null && payroll.IsPaid)
            {
                TempData[StaticString.StatusMessage] = "Error: Already paid payroll. Component reimburse can not delete.";
                return RedirectToAction(nameof(Form), new { id = payrollId, period });
            }

            FillDropdownListWithData();
            PayrollLineReimburse basic = new PayrollLineReimburse();


            basic = await _context.PayrollLineReimburse.Where(x => x.PayrollLineReimburseId.Equals(id)).FirstOrDefaultAsync();
            if (basic == null)
            {
                TempData[StaticString.StatusMessage] = "Error: PayrollLineReimburse can not found.";
                return RedirectToAction(nameof(Form), new { id = payrollId, period });
            }

            return View(basic);
        }


        //post submitted component reimburse. if id is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitComponentReimburseForm([Bind(
            "PayrollLineReimburseId",
            "PayrollId",
            "Description",
            "ExpenseTypeId",
            "Amount"
            )]PayrollLineReimburse PayrollLineReimburse)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(ComponentReimburseForm), new { id = PayrollLineReimburse.PayrollLineReimburseId ?? "", payrollId = PayrollLineReimburse.PayrollId });
                }

                //create new
                if (PayrollLineReimburse.PayrollLineReimburseId == null)
                {

                    PayrollLineReimburse newObj = new PayrollLineReimburse();
                    newObj.PayrollLineReimburseId = Guid.NewGuid().ToString();
                    newObj.PayrollId = PayrollLineReimburse.PayrollId;
                    newObj.Description = PayrollLineReimburse.Description;
                    newObj.Amount = PayrollLineReimburse.Amount;
                    newObj.ExpenseTypeId = PayrollLineReimburse.ExpenseTypeId;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.PayrollLineReimburse.Add(newObj);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new item success.";
                    return RedirectToAction(nameof(ComponentReimburseForm), new { id = newObj.PayrollLineReimburseId, payrollId = PayrollLineReimburse.PayrollId });
                }

                //edit existing
                PayrollLineReimburse editObj = new PayrollLineReimburse();
                editObj = await _context.PayrollLineReimburse
                    .Where(x => x.PayrollLineReimburseId.Equals(PayrollLineReimburse.PayrollLineReimburseId)).FirstOrDefaultAsync();



                editObj.Description = PayrollLineReimburse.Description;
                editObj.Amount = PayrollLineReimburse.Amount;
                editObj.ExpenseTypeId = PayrollLineReimburse.ExpenseTypeId;
                editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                editObj.UpdatedAtUtc = DateTime.UtcNow;

                _context.Update(editObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Edit existing item success.";
                return RedirectToAction(nameof(ComponentReimburseForm), new { id = PayrollLineReimburse.PayrollLineReimburseId ?? "", payrollId = PayrollLineReimburse.PayrollId });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(ComponentReimburseForm), new { id = PayrollLineReimburse.PayrollLineReimburseId ?? "", payrollId = PayrollLineReimburse.PayrollId });
            }
        }

        //delete submitted item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitComponentReimburseDelete([Bind("PayrollLineReimburseId")]PayrollLineReimburse PayrollLineReimburse)
        {
            try
            {
                var deleteObj = await _context.PayrollLineReimburse
                    .Where(x => x.PayrollLineReimburseId.Equals(PayrollLineReimburse.PayrollLineReimburseId))
                    .FirstOrDefaultAsync();

                if (deleteObj == null)
                {
                    return NotFound();
                }


                _context.PayrollLineReimburse.Remove(deleteObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete item success.";
                return RedirectToAction(nameof(Form), new { id = deleteObj.PayrollId });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(ComponentReimburseDelete), new { id = PayrollLineReimburse.PayrollLineReimburseId ?? "", payrollId = PayrollLineReimburse.PayrollId });
            }
        }

        //display generate payroll screen
        [HttpGet]
        public IActionResult GeneratePayroll()
        {
            GeneratePayroll gp = new GeneratePayroll();
            gp.Periode = DateTime.Now;
            return View(gp);
        }

        //process submitted generate payroll
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitGeneratePayroll([Bind(
            "Periode",
            "IsApproved",
            "IsPaid"
            )]GeneratePayroll GeneratePayroll)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(GeneratePayroll));
                }

                //check payroll existing period
                List<Payroll> payrolls = new List<Payroll>();
                payrolls = await _context.Payroll
                    .Where(x => x.Periode.Year.Equals(GeneratePayroll.Periode.Year) && x.Periode.Month.Equals(GeneratePayroll.Periode.Month))
                    .ToListAsync();

                if (payrolls.Count > 0)
                {
                    TempData[StaticString.StatusMessage] = "Error: This period ["+GeneratePayroll.Periode.ToString("yyyy-MM")+"] already contains one or more payroll.";
                    return RedirectToAction(nameof(GeneratePayroll));
                }

                List<Employee> employees = new List<Employee>();
                employees = await _context.Employee.ToListAsync();
                int index = 0;
                foreach (var item in employees)
                {
                    await _app.GenerateSalaryByEmployeeByPeriod(item.EmployeeId, GeneratePayroll.Periode.ToString("yyyy-MM"), GeneratePayroll.IsApproved, GeneratePayroll.IsPaid);
                    index = index + 1;
                }
                

                TempData[StaticString.StatusMessage] = "Generate payroll for "+index.ToString()+" employees success";
                return RedirectToAction(nameof(Index), new { period = GeneratePayroll.Periode.ToString("yyyy-MM") });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(GeneratePayroll));
            }
        }




    }
}