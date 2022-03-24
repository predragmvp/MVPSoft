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
    [Authorize(Roles = Services.App.Pages.Appraisal.RoleName)]
    public class AppraisalController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Services.App.ICommon _app;

        //dependency injection through constructor, to directly access services
        public AppraisalController(
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
            ViewData["AppraisalType"] = _app.GetAppraisalTypeSelectList();
            ViewData["OnBehalf"] = _app.GetEmployeeSelectList();
            ViewData["Supervisor"] = _app.GetEmployeeSelectList();
        }

        //display appraisal approval and increse results
        //the details will display KPI score and new KPI list
        public IActionResult Index(string period)
        {
            var Appraisals = _context.Appraisal                
                .Include(x => x.AppraisalType)
                .Include(x => x.OnBehalf)
                .OrderByDescending(x => x.CreatedAtUtc)
                .ToList()
                .Where(x => x.SubmitDate.ToString("yyyy-MM").Equals(period));
            return View(Appraisals.ToList());
        }

        //display Appraisal create edit form
        [HttpGet]
        public IActionResult Form(string id)
        {

            //create new
            if (id == null)
            {
                //dropdownlist type
                FillDropdownListWithData();

                Appraisal newAppraisal = new Appraisal();
                newAppraisal.SubmitDate = DateTime.Now;
                return View(newAppraisal);
            }

            //edit Appraisal
            Appraisal Appraisal = new Appraisal();
            Appraisal = _context.Appraisal.Where(x => x.AppraisalId.Equals(id)).FirstOrDefault();

            if (Appraisal == null)
            {
                return NotFound();
            }
            //dropdownlist type
            FillDropdownListWithData();

            return View(Appraisal);

        }

        //post submitted Appraisal data. if Appraisal.AppraisalId is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitForm([Bind(
            "AppraisalId",
            "AppraisalName",
            "IsApproved",
            "AppraisalTypeId",
            "SubmitDate",
            "Description",
            "OnBehalfId"
            )]Appraisal Appraisal)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(Form), new { id = Appraisal.AppraisalId ?? "" });
                }

                //create new
                if (Appraisal.AppraisalId == null)
                {
                    Appraisal newAppraisal = new Appraisal();
                    newAppraisal.AppraisalId = Guid.NewGuid().ToString();
                    newAppraisal.AppraisalName = Appraisal.AppraisalName;
                    newAppraisal.AppraisalTypeId = Appraisal.AppraisalTypeId;
                    newAppraisal.SubmitDate = Appraisal.SubmitDate;
                    newAppraisal.IsApproved = Appraisal.IsApproved;
                    newAppraisal.Description = Appraisal.Description;
                    newAppraisal.OnBehalfId = Appraisal.OnBehalfId;
                    newAppraisal.CreatedBy = await _userManager.GetUserAsync(User);
                    newAppraisal.CreatedAtUtc = DateTime.UtcNow;
                    newAppraisal.UpdatedBy = newAppraisal.CreatedBy;
                    newAppraisal.UpdatedAtUtc = newAppraisal.CreatedAtUtc;

                    _context.Appraisal.Add(newAppraisal);
                    _context.SaveChanges();

                    //dropdownlist type
                    FillDropdownListWithData();

                    TempData[StaticString.StatusMessage] = "Create new Appraisal item success.";
                    return RedirectToAction(nameof(Form), new { id = newAppraisal.AppraisalId ?? "" });
                }

                //edit existing
                Appraisal editAppraisal = new Appraisal();
                editAppraisal = _context.Appraisal.Where(x => x.AppraisalId.Equals(Appraisal.AppraisalId)).FirstOrDefault();
                editAppraisal.AppraisalName = Appraisal.AppraisalName;
                editAppraisal.AppraisalTypeId = Appraisal.AppraisalTypeId;
                editAppraisal.SubmitDate = Appraisal.SubmitDate;
                editAppraisal.IsApproved = Appraisal.IsApproved;
                editAppraisal.Description = Appraisal.Description;
                editAppraisal.OnBehalfId = Appraisal.OnBehalfId;
                editAppraisal.UpdatedBy = await _userManager.GetUserAsync(User);
                editAppraisal.UpdatedAtUtc = DateTime.UtcNow;
                _context.Update(editAppraisal);
                _context.SaveChanges();

                //dropdownlist type
                FillDropdownListWithData();

                TempData[StaticString.StatusMessage] = "Edit existing Appraisal item success.";
                return RedirectToAction(nameof(Form), new { id = Appraisal.AppraisalId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Form), new { id = Appraisal.AppraisalId ?? "" });
            }
        }

        //display Appraisal item for deletion
        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //dropdownlist type
            FillDropdownListWithData();

            var Appraisal = _context.Appraisal
                .Include(x => x.AppraisalType)
                .Include(x => x.OnBehalf)
                .Where(x => x.AppraisalId.Equals(id)).FirstOrDefault();
            return View(Appraisal);
        }

        //delete submitted Appraisal item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitDelete([Bind("AppraisalId")]Appraisal Appraisal)
        {
            try
            {
                var deleteAppraisal = _context.Appraisal.Where(x => x.AppraisalId.Equals(Appraisal.AppraisalId)).FirstOrDefault();
                if (deleteAppraisal == null)
                {
                    return NotFound();
                }

                _context.Appraisal.Remove(deleteAppraisal);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete Appraisal item success.";
                return RedirectToAction(nameof(Index), new { period = DateTime.Now.ToString("yyyy-MM") });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Delete), new { id = Appraisal.AppraisalId ?? "" });
            }
        }


        //display of AppraisalType
        public IActionResult AppraisalTypeIndex()
        {
            var objs = _context.AppraisalType.OrderByDescending(x => x.CreatedAtUtc).ToList();
            return View(objs);
        }

        //display AppraisalType create edit form
        [HttpGet]
        public IActionResult AppraisalTypeForm(string id)
        {
            //create new
            if (id == null)
            {
                AppraisalType newObj = new AppraisalType();
                return View(newObj);
            }

            //edit AppraisalType
            AppraisalType obj = new AppraisalType();
            obj = _context.AppraisalType.Where(x => x.AppraisalTypeId.Equals(id)).FirstOrDefault();

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //post submitted AppraisalType data. if id is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitAppraisalTypeForm([Bind("AppraisalTypeId", "Name", "Description")]AppraisalType AppraisalType)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(AppraisalTypeForm), new { id = AppraisalType.AppraisalTypeId ?? "" });
                }

                //create new
                if (AppraisalType.AppraisalTypeId == null)
                {
                    if (await _context.AppraisalType.AnyAsync(x => x.Name.Equals(AppraisalType.Name)))
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + AppraisalType.Name + " already exist";
                        return RedirectToAction(nameof(AppraisalTypeForm), new { id = AppraisalType.AppraisalTypeId ?? "" });
                    }

                    AppraisalType newObj = new AppraisalType();
                    newObj.AppraisalTypeId = Guid.NewGuid().ToString();
                    newObj.Name = AppraisalType.Name;
                    newObj.Description = AppraisalType.Description;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.AppraisalType.Add(newObj);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new item success.";
                    return RedirectToAction(nameof(AppraisalTypeForm), new { id = newObj.AppraisalTypeId ?? "" });
                }

                //edit existing
                AppraisalType editObj = new AppraisalType();
                AppraisalType existObj = new AppraisalType();
                editObj = await _context.AppraisalType.Where(x => x.AppraisalTypeId.Equals(AppraisalType.AppraisalTypeId)).FirstOrDefaultAsync();
                existObj = await _context.AppraisalType.Where(x => x.Name.Equals(AppraisalType.Name)).FirstOrDefaultAsync();

                if (existObj != null)
                {
                    if (editObj.AppraisalTypeId != existObj.AppraisalTypeId)
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + AppraisalType.Name + " already exist";
                        return RedirectToAction(nameof(AppraisalTypeForm), new { id = AppraisalType.AppraisalTypeId ?? "" });
                    }

                }

                editObj.Name = AppraisalType.Name;
                editObj.Description = AppraisalType.Description;
                editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                editObj.UpdatedAtUtc = DateTime.UtcNow;

                _context.Update(editObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Edit existing item success.";
                return RedirectToAction(nameof(AppraisalTypeForm), new { id = AppraisalType.AppraisalTypeId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(AppraisalTypeForm), new { id = AppraisalType.AppraisalTypeId ?? "" });
            }
        }

        //display item for deletion
        [HttpGet]
        public async Task<IActionResult> AppraisalTypeDelete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = await _context.AppraisalType.Where(x => x.AppraisalTypeId.Equals(id)).FirstOrDefaultAsync();
            return View(obj);
        }

        //delete submitted item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitAppraisalTypeDelete([Bind("AppraisalTypeId")]AppraisalType AppraisalType)
        {
            try
            {
                var deleteObj = await _context.AppraisalType.Where(x => x.AppraisalTypeId.Equals(AppraisalType.AppraisalTypeId)).FirstOrDefaultAsync();
                if (deleteObj == null)
                {
                    return NotFound();
                }

                //check existing appraisal
                Appraisal objCheck = new Appraisal();
                objCheck = await _context.Appraisal
                    .Where(x => x.AppraisalTypeId.Equals(deleteObj.AppraisalTypeId))
                    .FirstOrDefaultAsync();

                if (objCheck != null)
                {
                    TempData[StaticString.StatusMessage] = "Error: already used on transaction.";
                    return RedirectToAction(nameof(AppraisalTypeDelete), new { id = AppraisalType.AppraisalTypeId ?? "" });
                }
                

                _context.AppraisalType.Remove(deleteObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete item success.";
                return RedirectToAction(nameof(AppraisalTypeIndex));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(AppraisalTypeDelete), new { id = AppraisalType.AppraisalTypeId ?? "" });
            }
        }

    }
}