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
    [Authorize(Roles = Services.App.Pages.Information.RoleName)]
    public class InformationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Services.App.ICommon _app;

        //dependency injection through constructor, to directly access services
        public InformationController(
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
            ViewData["InformationType"] = _app.GetInformationTypeSelectList();
            
        }

        //display information list
        public IActionResult Index(string period)
        {
            var Informations = _context.Information                
                .Include(x => x.InformationType)
                .ToList()
                .Where(x => x.ReleaseDate.ToString("yyyy-MM").Equals(period))
                .OrderByDescending(x => x.CreatedAtUtc)
                .ToList();
            return View(Informations);
        }

        //display Information create edit form
        [HttpGet]
        public IActionResult Form(string id)
        {

            //create new
            if (id == null)
            {
                //dropdownlist type
                FillDropdownListWithData();

                Information newInformation = new Information();
                newInformation.ReleaseDate = DateTime.Now;
                return View(newInformation);
            }

            //edit Information
            Information Information = new Information();
            Information = _context.Information.Where(x => x.InformationId.Equals(id)).FirstOrDefault();

            if (Information == null)
            {
                return NotFound();
            }
            //dropdownlist type
            FillDropdownListWithData();

            return View(Information);

        }

        //post submitted Information data. if Information.InformationId is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitForm([Bind(
            "InformationId",
            "InformationName",
            "IsActive",
            "InformationTypeId",
            "ReleaseDate",
            "Description",
            "ExternalLink"
            )]Information Information)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(Form), new { id = Information.InformationId ?? "" });
                }

                //create new
                if (Information.InformationId == null)
                {
                    Information newInformation = new Information();
                    newInformation.InformationId = Guid.NewGuid().ToString();
                    newInformation.InformationName = Information.InformationName;
                    newInformation.InformationTypeId = Information.InformationTypeId;
                    newInformation.ReleaseDate = Information.ReleaseDate;
                    newInformation.IsActive = Information.IsActive;
                    newInformation.Description = Information.Description;
                    newInformation.ExternalLink = Information.ExternalLink;
                    newInformation.CreatedBy = await _userManager.GetUserAsync(User);
                    newInformation.CreatedAtUtc = DateTime.UtcNow;
                    newInformation.UpdatedBy = newInformation.CreatedBy;
                    newInformation.UpdatedAtUtc = newInformation.CreatedAtUtc;

                    _context.Information.Add(newInformation);
                    _context.SaveChanges();

                    //dropdownlist type
                    FillDropdownListWithData();

                    TempData[StaticString.StatusMessage] = "Create new Information item success.";
                    return RedirectToAction(nameof(Form), new { id = newInformation.InformationId ?? "" });
                }

                //edit existing
                Information editInformation = new Information();
                editInformation = _context.Information.Where(x => x.InformationId.Equals(Information.InformationId)).FirstOrDefault();
                editInformation.InformationName = Information.InformationName;
                editInformation.InformationTypeId = Information.InformationTypeId;
                editInformation.ReleaseDate = Information.ReleaseDate;
                editInformation.IsActive = Information.IsActive;
                editInformation.Description = Information.Description;
                editInformation.ExternalLink = Information.ExternalLink;
                editInformation.UpdatedBy = await _userManager.GetUserAsync(User);
                editInformation.UpdatedAtUtc = DateTime.UtcNow;
                _context.Update(editInformation);
                _context.SaveChanges();

                //dropdownlist type
                FillDropdownListWithData();

                TempData[StaticString.StatusMessage] = "Edit existing Information item success.";
                return RedirectToAction(nameof(Form), new { id = Information.InformationId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Form), new { id = Information.InformationId ?? "" });
            }
        }

        //display Information item for deletion
        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //dropdownlist type
            FillDropdownListWithData();

            var Information = _context.Information
                .Include(x => x.InformationType)
                .Where(x => x.InformationId.Equals(id)).FirstOrDefault();

            return View(Information);
        }

        //delete submitted Information item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitDelete([Bind("InformationId")]Information Information)
        {
            try
            {
                var deleteInformation = _context.Information.Where(x => x.InformationId.Equals(Information.InformationId)).FirstOrDefault();
                if (deleteInformation == null)
                {
                    return NotFound();
                }

                _context.Information.Remove(deleteInformation);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete Information item success.";
                return RedirectToAction(nameof(Index), new { period = DateTime.Now.ToString("yyyy-MM") });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Delete), new { id = Information.InformationId ?? "" });
            }
        }


        //display of InformationType
        public IActionResult InformationTypeIndex()
        {
            var objs = _context.InformationType.OrderByDescending(x => x.CreatedAtUtc).ToList();
            return View(objs);
        }

        //display InformationType create edit form
        [HttpGet]
        public IActionResult InformationTypeForm(string id)
        {
            //create new
            if (id == null)
            {
                InformationType newObj = new InformationType();
                return View(newObj);
            }

            //edit InformationType
            InformationType obj = new InformationType();
            obj = _context.InformationType.Where(x => x.InformationTypeId.Equals(id)).FirstOrDefault();

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //post submitted InformationType data. if id is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitInformationTypeForm([Bind("InformationTypeId", "Name", "Description")]InformationType InformationType)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(InformationTypeForm), new { id = InformationType.InformationTypeId ?? "" });
                }

                //create new
                if (InformationType.InformationTypeId == null)
                {
                    if (await _context.InformationType.AnyAsync(x => x.Name.Equals(InformationType.Name)))
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + InformationType.Name + " already exist";
                        return RedirectToAction(nameof(InformationTypeForm), new { id = InformationType.InformationTypeId ?? "" });
                    }

                    InformationType newObj = new InformationType();
                    newObj.InformationTypeId = Guid.NewGuid().ToString();
                    newObj.Name = InformationType.Name;
                    newObj.Description = InformationType.Description;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.InformationType.Add(newObj);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new item success.";
                    return RedirectToAction(nameof(InformationTypeForm), new { id = newObj.InformationTypeId ?? "" });
                }

                //edit existing
                InformationType editObj = new InformationType();
                InformationType existObj = new InformationType();
                editObj = await _context.InformationType.Where(x => x.InformationTypeId.Equals(InformationType.InformationTypeId)).FirstOrDefaultAsync();
                existObj = await _context.InformationType.Where(x => x.Name.Equals(InformationType.Name)).FirstOrDefaultAsync();

                if (existObj != null)
                {
                    if (editObj.InformationTypeId != existObj.InformationTypeId)
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + InformationType.Name + " already exist";
                        return RedirectToAction(nameof(InformationTypeForm), new { id = InformationType.InformationTypeId ?? "" });
                    }

                }

                editObj.Name = InformationType.Name;
                editObj.Description = InformationType.Description;
                editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                editObj.UpdatedAtUtc = DateTime.UtcNow;

                _context.Update(editObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Edit existing item success.";
                return RedirectToAction(nameof(InformationTypeForm), new { id = InformationType.InformationTypeId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(InformationTypeForm), new { id = InformationType.InformationTypeId ?? "" });
            }
        }

        //display item for deletion
        [HttpGet]
        public async Task<IActionResult> InformationTypeDelete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = await _context.InformationType.Where(x => x.InformationTypeId.Equals(id)).FirstOrDefaultAsync();
            return View(obj);
        }

        //delete submitted item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitInformationTypeDelete([Bind("InformationTypeId")]InformationType InformationType)
        {
            try
            {
                var deleteObj = await _context.InformationType.Where(x => x.InformationTypeId.Equals(InformationType.InformationTypeId)).FirstOrDefaultAsync();
                if (deleteObj == null)
                {
                    return NotFound();
                }

                //cek existing information
                Information objCheck = new Information();
                objCheck = await _context.Information
                    .Where(x => x.InformationTypeId.Equals(deleteObj.InformationTypeId))
                    .FirstOrDefaultAsync();

                if (objCheck != null)
                {
                    TempData[StaticString.StatusMessage] = "Error: already used on transaction.";
                    return RedirectToAction(nameof(InformationTypeDelete), new { id = InformationType.InformationTypeId ?? "" });
                }

                _context.InformationType.Remove(deleteObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete item success.";
                return RedirectToAction(nameof(InformationTypeIndex));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(InformationTypeDelete), new { id = InformationType.InformationTypeId ?? "" });
            }
        }

    }
}