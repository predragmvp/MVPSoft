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
    [Authorize(Roles = Services.App.Pages.Asset.RoleName)]
    public class AssetController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Services.App.ICommon _app;

        //dependency injection through constructor, to directly access services
        public AssetController(
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
            ViewData["AssetType"] = _app.GetAssetTypeSelectList();
            ViewData["UsedBy"] = _app.GetEmployeeSelectList();
        }

        //display asset list
        public IActionResult Index(string period)
        {
            var Assets = _context.Asset                
                .Include(x => x.AssetType)
                .Include(x => x.UsedBy)
                .ToList()
                .Where(x => x.PurchaseDate.ToString("yyyy-MM").Equals(period))
                .OrderByDescending(x => x.CreatedAtUtc).ToList();
            return View(Assets);
        }

        //display Asset create edit form
        [HttpGet]
        public IActionResult Form(string id)
        {

            //create new
            if (id == null)
            {
                //dropdownlist type
                FillDropdownListWithData();

                Asset newAsset = new Asset();
                newAsset.PurchaseDate = DateTime.Now;
                return View(newAsset);
            }

            //edit Asset
            Asset Asset = new Asset();
            Asset = _context.Asset.Where(x => x.AssetId.Equals(id)).FirstOrDefault();

            if (Asset == null)
            {
                return NotFound();
            }
            //dropdownlist type
            FillDropdownListWithData();

            return View(Asset);

        }

        //post submitted Asset data. if Asset.AssetId is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitForm([Bind(
            "AssetId",
            "AssetName",
            "IsActive",
            "AssetTypeId",
            "PurchaseDate",
            "PurchasePrice",
            "Description",
            "UsedById"
            )]Asset Asset)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(Form), new { id = Asset.AssetId ?? "" });
                }

                //create new
                if (Asset.AssetId == null)
                {
                    Asset newAsset = new Asset();
                    newAsset.AssetId = Guid.NewGuid().ToString();
                    newAsset.AssetName = Asset.AssetName;
                    newAsset.AssetTypeId = Asset.AssetTypeId;
                    newAsset.PurchaseDate = Asset.PurchaseDate;
                    newAsset.PurchasePrice = Asset.PurchasePrice;
                    newAsset.IsActive = Asset.IsActive;
                    newAsset.Description = Asset.Description;
                    newAsset.UsedById = Asset.UsedById;
                    newAsset.CreatedBy = await _userManager.GetUserAsync(User);
                    newAsset.CreatedAtUtc = DateTime.UtcNow;
                    newAsset.UpdatedBy = newAsset.CreatedBy;
                    newAsset.UpdatedAtUtc = newAsset.CreatedAtUtc;

                    _context.Asset.Add(newAsset);
                    _context.SaveChanges();

                    //dropdownlist type
                    FillDropdownListWithData();

                    TempData[StaticString.StatusMessage] = "Create new Asset item success.";
                    return RedirectToAction(nameof(Form), new { id = newAsset.AssetId ?? "" });
                }

                //edit existing
                Asset editAsset = new Asset();
                editAsset = _context.Asset.Where(x => x.AssetId.Equals(Asset.AssetId)).FirstOrDefault();
                editAsset.AssetName = Asset.AssetName;
                editAsset.AssetTypeId = Asset.AssetTypeId;
                editAsset.PurchaseDate = Asset.PurchaseDate;
                editAsset.PurchasePrice = Asset.PurchasePrice;
                editAsset.IsActive = Asset.IsActive;
                editAsset.Description = Asset.Description;
                editAsset.UsedById = Asset.UsedById;
                editAsset.UpdatedBy = await _userManager.GetUserAsync(User);
                editAsset.UpdatedAtUtc = DateTime.UtcNow;

                _context.Update(editAsset);
                _context.SaveChanges();

                //dropdownlist type
                FillDropdownListWithData();

                TempData[StaticString.StatusMessage] = "Edit existing Asset item success.";
                return RedirectToAction(nameof(Form), new { id = Asset.AssetId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Form), new { id = Asset.AssetId ?? "" });
            }
        }

        //display Asset item for deletion
        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //dropdownlist type
            FillDropdownListWithData();

            var Asset = _context.Asset
                .Include(x => x.AssetType)
                .Include(x => x.UsedBy)
                .Where(x => x.AssetId.Equals(id)).FirstOrDefault();
            return View(Asset);
        }

        //delete submitted Asset item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitDelete([Bind("AssetId")]Asset Asset)
        {
            try
            {
                var deleteAsset = _context.Asset.Where(x => x.AssetId.Equals(Asset.AssetId)).FirstOrDefault();
                if (deleteAsset == null)
                {
                    return NotFound();
                }

                _context.Asset.Remove(deleteAsset);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete Asset item success.";
                return RedirectToAction(nameof(Index), new { period = DateTime.Now.ToString("yyyy-MM") });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Delete), new { id = Asset.AssetId ?? "" });
            }
        }


        //display of AssetType
        public IActionResult AssetTypeIndex()
        {
            var objs = _context.AssetType.OrderByDescending(x => x.CreatedAtUtc).ToList();
            return View(objs);
        }

        //display AssetType create edit form
        [HttpGet]
        public IActionResult AssetTypeForm(string id)
        {
            //create new
            if (id == null)
            {
                AssetType newObj = new AssetType();
                return View(newObj);
            }

            //edit AssetType
            AssetType obj = new AssetType();
            obj = _context.AssetType.Where(x => x.AssetTypeId.Equals(id)).FirstOrDefault();

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //post submitted AssetType data. if id is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitAssetTypeForm([Bind("AssetTypeId", "Name", "Description")]AssetType AssetType)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(AssetTypeForm), new { id = AssetType.AssetTypeId ?? "" });
                }

                //create new
                if (AssetType.AssetTypeId == null)
                {
                    if (await _context.AssetType.AnyAsync(x => x.Name.Equals(AssetType.Name)))
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + AssetType.Name + " already exist";
                        return RedirectToAction(nameof(AssetTypeForm), new { id = AssetType.AssetTypeId ?? "" });
                    }

                    AssetType newObj = new AssetType();
                    newObj.AssetTypeId = Guid.NewGuid().ToString();
                    newObj.Name = AssetType.Name;
                    newObj.Description = AssetType.Description;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.AssetType.Add(newObj);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new item success.";
                    return RedirectToAction(nameof(AssetTypeForm), new { id = newObj.AssetTypeId ?? "" });
                }

                //edit existing
                AssetType editObj = new AssetType();
                AssetType existObj = new AssetType();
                editObj = await _context.AssetType.Where(x => x.AssetTypeId.Equals(AssetType.AssetTypeId)).FirstOrDefaultAsync();
                existObj = await _context.AssetType.Where(x => x.Name.Equals(AssetType.Name)).FirstOrDefaultAsync();

                if (existObj != null)
                {
                    if (editObj.AssetTypeId != existObj.AssetTypeId)
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + AssetType.Name + " already exist";
                        return RedirectToAction(nameof(AssetTypeForm), new { id = AssetType.AssetTypeId ?? "" });
                    }

                }

                editObj.Name = AssetType.Name;
                editObj.Description = AssetType.Description;
                editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                editObj.UpdatedAtUtc = DateTime.UtcNow;

                _context.Update(editObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Edit existing item success.";
                return RedirectToAction(nameof(AssetTypeForm), new { id = AssetType.AssetTypeId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(AssetTypeForm), new { id = AssetType.AssetTypeId ?? "" });
            }
        }

        //display item for deletion
        [HttpGet]
        public async Task<IActionResult> AssetTypeDelete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = await _context.AssetType.Where(x => x.AssetTypeId.Equals(id)).FirstOrDefaultAsync();
            return View(obj);
        }

        //delete submitted item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitAssetTypeDelete([Bind("AssetTypeId")]AssetType AssetType)
        {
            try
            {
                var deleteObj = await _context.AssetType.Where(x => x.AssetTypeId.Equals(AssetType.AssetTypeId)).FirstOrDefaultAsync();
                if (deleteObj == null)
                {
                    return NotFound();
                }

                //cek existing ke award
                Asset objCheck = new Asset();
                objCheck = await _context.Asset
                    .Where(x => x.AssetTypeId.Equals(deleteObj.AssetTypeId))
                    .FirstOrDefaultAsync();

                if (objCheck != null)
                {
                    TempData[StaticString.StatusMessage] = "Error: already used on transaction.";
                    return RedirectToAction(nameof(AssetTypeDelete), new { id = AssetType.AssetTypeId ?? "" });
                }

                _context.AssetType.Remove(deleteObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete item success.";
                return RedirectToAction(nameof(AssetTypeIndex));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(AssetTypeDelete), new { id = AssetType.AssetTypeId ?? "" });
            }
        }

    }
}