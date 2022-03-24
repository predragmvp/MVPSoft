using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using coderush.Data;
using coderush.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace coderush.Controllers
{
    [Authorize(Roles = Services.App.Pages.Award.RoleName)]
    public class AwardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Services.App.ICommon _app;

        //dependency injection through constructor, to directly access services
        public AwardController(
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
            ViewData["AwardType"] = _app.GetAwardTypeSelectList();
            ViewData["AwardRecipient"] = _app.GetEmployeeSelectList();
        }

        //display list employee that got award
        public IActionResult Index(string period)
        {
            var Awards = _context.Award                
                .Include(x => x.AwardType)
                .Include(x => x.AwardRecipient)
                .OrderByDescending(x => x.CreatedAtUtc)
                .ToList()
                .Where(x => x.ReleaseDate.ToString("yyyy-MM").Equals(period));
            return View(Awards.ToList());
        }

        //display Award create edit form
        [HttpGet]
        public IActionResult Form(string id)
        {

            //create new
            if (id == null)
            {
                //dropdownlist type
                FillDropdownListWithData();

                Award newAward = new Award();
                newAward.ReleaseDate = DateTime.Now;
                return View(newAward);
            }

            //edit Award
            Award Award = new Award();
            Award = _context.Award.Where(x => x.AwardId.Equals(id)).FirstOrDefault();

            if (Award == null)
            {
                return NotFound();
            }
            //dropdownlist type
            FillDropdownListWithData();

            return View(Award);

        }

        //post submitted Award data. if Award.AwardId is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitForm([Bind(
            "AwardId",
            "AwardName",
            "IsApproved",
            "AwardTypeId",
            "ReleaseDate",
            "Description",
            "AwardRecipientId"
            )]Award Award)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(Form), new { id = Award.AwardId ?? "" });
                }

                //create new
                if (Award.AwardId == null)
                {
                    Award newAward = new Award();
                    newAward.AwardId = Guid.NewGuid().ToString();
                    newAward.AwardName = Award.AwardName;
                    newAward.AwardTypeId = Award.AwardTypeId;
                    newAward.ReleaseDate = Award.ReleaseDate;
                    newAward.IsApproved = Award.IsApproved;
                    newAward.Description = Award.Description;
                    newAward.AwardRecipientId = Award.AwardRecipientId;
                    newAward.CreatedBy = await _userManager.GetUserAsync(User);
                    newAward.CreatedAtUtc = DateTime.UtcNow;
                    newAward.UpdatedBy = newAward.CreatedBy;
                    newAward.UpdatedAtUtc = newAward.CreatedAtUtc;

                    _context.Award.Add(newAward);
                    _context.SaveChanges();

                    //dropdownlist type
                    FillDropdownListWithData();

                    TempData[StaticString.StatusMessage] = "Create new Award item success.";
                    return RedirectToAction(nameof(Form), new { id = newAward.AwardId ?? "" });
                }

                //edit existing
                Award editAward = new Award();
                editAward = _context.Award.Where(x => x.AwardId.Equals(Award.AwardId)).FirstOrDefault();
                editAward.AwardName = Award.AwardName;
                editAward.AwardTypeId = Award.AwardTypeId;
                editAward.ReleaseDate = Award.ReleaseDate;
                editAward.IsApproved = Award.IsApproved;
                editAward.Description = Award.Description;
                editAward.AwardRecipientId = Award.AwardRecipientId;
                editAward.UpdatedBy = await _userManager.GetUserAsync(User);
                editAward.UpdatedAtUtc = DateTime.UtcNow;
                _context.Update(editAward);
                _context.SaveChanges();

                //dropdownlist type
                FillDropdownListWithData();

                TempData[StaticString.StatusMessage] = "Edit existing Award item success.";
                return RedirectToAction(nameof(Form), new { id = Award.AwardId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Form), new { id = Award.AwardId ?? "" });
            }
        }

        //display Award item for deletion
        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //dropdownlist type
            FillDropdownListWithData();

            var Award = _context.Award
                .Include(x => x.AwardType)
                .Include(x => x.AwardRecipient)
                .Where(x => x.AwardId.Equals(id)).FirstOrDefault();
            return View(Award);
        }

        //delete submitted Award item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitDelete([Bind("AwardId")]Award Award)
        {
            try
            {
                var deleteAward = _context.Award
                    .Where(x => x.AwardId.Equals(Award.AwardId)).FirstOrDefault();
                if (deleteAward == null)
                {
                    return NotFound();
                }

                _context.Award.Remove(deleteAward);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete Award item success.";
                return RedirectToAction(nameof(Index), new { period = DateTime.Now.ToString("yyyy-MM") });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Delete), new { id = Award.AwardId ?? "" });
            }
        }


        //display of award type
        public IActionResult AwardTypeIndex()
        {
            var objs = _context.AwardType.OrderByDescending(x => x.CreatedAtUtc).ToList();
            return View(objs);
        }

        //display AwardType create edit form
        [HttpGet]
        public IActionResult AwardTypeForm(string id)
        {
            //create new
            if (id == null)
            {
                AwardType newObj = new AwardType();
                return View(newObj);
            }

            //edit AwardType
            AwardType obj = new AwardType();
            obj = _context.AwardType.Where(x => x.AwardTypeId.Equals(id)).FirstOrDefault();

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //post submitted AwardType data. if id is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitAwardTypeForm([Bind("AwardTypeId", "Name", "Description")]AwardType awardType)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(AwardTypeForm), new { id = awardType.AwardTypeId ?? "" });
                }

                //create new
                if (awardType.AwardTypeId == null)
                {
                    if (await _context.AwardType.AnyAsync(x => x.Name.Equals(awardType.Name)))
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + awardType.Name + " already exist";
                        return RedirectToAction(nameof(AwardTypeForm), new { id = awardType.AwardTypeId ?? "" });
                    }

                    AwardType newObj = new AwardType();
                    newObj.AwardTypeId = Guid.NewGuid().ToString();
                    newObj.Name = awardType.Name;
                    newObj.Description = awardType.Description;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.AwardType.Add(newObj);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new item success.";
                    return RedirectToAction(nameof(AwardTypeForm), new { id = newObj.AwardTypeId ?? "" });
                }

                //edit existing
                AwardType editObj = new AwardType();
                AwardType existObj = new AwardType();
                editObj = await _context.AwardType.Where(x => x.AwardTypeId.Equals(awardType.AwardTypeId)).FirstOrDefaultAsync();
                existObj = await _context.AwardType.Where(x => x.Name.Equals(awardType.Name)).FirstOrDefaultAsync();

                if (existObj != null)
                {
                    if (editObj.AwardTypeId != existObj.AwardTypeId)
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + awardType.Name + " already exist";
                        return RedirectToAction(nameof(AwardTypeForm), new { id = awardType.AwardTypeId ?? "" });
                    }

                }

                editObj.Name = awardType.Name;
                editObj.Description = awardType.Description;
                editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                editObj.UpdatedAtUtc = DateTime.UtcNow;

                _context.Update(editObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Edit existing item success.";
                return RedirectToAction(nameof(AwardTypeForm), new { id = awardType.AwardTypeId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(AwardTypeForm), new { id = awardType.AwardTypeId ?? "" });
            }
        }

        //display item for deletion
        [HttpGet]
        public async Task<IActionResult> AwardTypeDelete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = await _context.AwardType.Where(x => x.AwardTypeId.Equals(id)).FirstOrDefaultAsync();
            return View(obj);
        }

        //delete submitted item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitAwardTypeDelete([Bind("AwardTypeId")]AwardType awardType)
        {
            try
            {
                var deleteObj = await _context.AwardType.Where(x => x.AwardTypeId.Equals(awardType.AwardTypeId)).FirstOrDefaultAsync();
                if (deleteObj == null)
                {
                    return NotFound();
                }

                //cek existing ke award
                Award objCheck = new Award();
                objCheck = await _context.Award
                    .Where(x => x.AwardTypeId.Equals(deleteObj.AwardTypeId))
                    .FirstOrDefaultAsync();

                if (objCheck != null)
                {
                    TempData[StaticString.StatusMessage] = "Error: already used on transaction.";
                    return RedirectToAction(nameof(AwardTypeDelete), new { id = awardType.AwardTypeId ?? "" });
                }

                _context.AwardType.Remove(deleteObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete item success.";
                return RedirectToAction(nameof(AwardTypeIndex));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(AwardTypeDelete), new { id = awardType.AwardTypeId ?? "" });
            }
        }

    }
}