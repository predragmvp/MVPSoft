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
    [Authorize(Roles = Services.App.Pages.Leave.RoleName)]
    public class LeaveController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Services.App.ICommon _app;

        //dependency injection through constructor, to directly access services
        public LeaveController(
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
            ViewData["LeaveType"] = _app.GetLeaveTypeSelectList();
            ViewData["OnBehalf"] = _app.GetEmployeeSelectList();
        }

        //show leave request
        public IActionResult Index(string period)
        {
            var Leaves = _context.Leave                
                .Include(x => x.LeaveType)
                .Include(x => x.OnBehalf)
                .OrderByDescending(x => x.CreatedAtUtc)
                .ToList()
                .Where(x => x.FromDate.ToString("yyyy-MM").Equals(period));
            return View(Leaves.ToList());
        }

        //display Leave create edit form
        [HttpGet]
        public IActionResult Form(string id)
        {

            //create new
            if (id == null)
            {
                //dropdownlist type
                FillDropdownListWithData();

                Leave newLeave = new Leave();
                newLeave.FromDate = DateTime.Now;
                newLeave.ToDate = DateTime.Now;
                return View(newLeave);
            }

            //edit Leave
            Leave Leave = new Leave();
            Leave = _context.Leave.Where(x => x.LeaveId.Equals(id)).FirstOrDefault();

            if (Leave == null)
            {
                return NotFound();
            }
            //dropdownlist type
            FillDropdownListWithData();

            return View(Leave);

        }

        //post submitted Leave data. if Leave.LeaveId is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitForm([Bind(
            "LeaveId",
            "LeaveName",
            "IsApproved",
            "LeaveTypeId",
            "FromDate",
            "ToDate",
            "Description",
            "OnBehalfId",
            "EmergencyCall",
            "IsPaidLeave"
            )]Leave Leave)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(Form), new { id = Leave.LeaveId ?? "" });
                }

                //create new
                if (Leave.LeaveId == null)
                {
                    Leave newLeave = new Leave();
                    newLeave.LeaveId = Guid.NewGuid().ToString();
                    newLeave.LeaveName = Leave.LeaveName;
                    newLeave.LeaveTypeId = Leave.LeaveTypeId;
                    newLeave.FromDate = Leave.FromDate;
                    newLeave.ToDate = Leave.ToDate;
                    newLeave.IsApproved = Leave.IsApproved;
                    newLeave.IsPaidLeave = Leave.IsPaidLeave;
                    newLeave.Description = Leave.Description;
                    newLeave.OnBehalfId = Leave.OnBehalfId;
                    newLeave.EmergencyCall = Leave.EmergencyCall;
                    newLeave.CreatedBy = await _userManager.GetUserAsync(User);
                    newLeave.CreatedAtUtc = DateTime.UtcNow;
                    newLeave.UpdatedBy = newLeave.CreatedBy;
                    newLeave.UpdatedAtUtc = newLeave.CreatedAtUtc;

                    _context.Leave.Add(newLeave);
                    _context.SaveChanges();

                    //dropdownlist type
                    FillDropdownListWithData();

                    TempData[StaticString.StatusMessage] = "Create new Leave item success.";
                    return RedirectToAction(nameof(Form), new { id = newLeave.LeaveId ?? "" });
                }

                //edit existing
                Leave editLeave = new Leave();
                editLeave = _context.Leave.Where(x => x.LeaveId.Equals(Leave.LeaveId)).FirstOrDefault();
                editLeave.LeaveName = Leave.LeaveName;
                editLeave.LeaveTypeId = Leave.LeaveTypeId;
                editLeave.FromDate = Leave.FromDate;
                editLeave.ToDate = Leave.ToDate;
                editLeave.IsApproved = Leave.IsApproved;
                editLeave.IsPaidLeave = Leave.IsPaidLeave;
                editLeave.Description = Leave.Description;
                editLeave.OnBehalfId = Leave.OnBehalfId;
                editLeave.EmergencyCall = Leave.EmergencyCall;
                editLeave.UpdatedBy = await _userManager.GetUserAsync(User);
                editLeave.UpdatedAtUtc = DateTime.UtcNow;
                _context.Update(editLeave);
                _context.SaveChanges();

                //dropdownlist type
                FillDropdownListWithData();

                TempData[StaticString.StatusMessage] = "Edit existing Leave item success.";
                return RedirectToAction(nameof(Form), new { id = Leave.LeaveId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Form), new { id = Leave.LeaveId ?? "" });
            }
        }

        //display Leave item for deletion
        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //dropdownlist type
            FillDropdownListWithData();

            var Leave = _context.Leave
                .Include(x => x.LeaveType)
                .Include(x => x.OnBehalf)
                .Where(x => x.LeaveId.Equals(id)).FirstOrDefault();
            return View(Leave);
        }

        //delete submitted Leave item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitDelete([Bind("LeaveId")]Leave Leave)
        {
            try
            {
                var deleteLeave = _context.Leave.Where(x => x.LeaveId.Equals(Leave.LeaveId)).FirstOrDefault();
                if (deleteLeave == null)
                {
                    return NotFound();
                }

                _context.Leave.Remove(deleteLeave);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete Leave item success.";
                return RedirectToAction(nameof(Index), new { period = DateTime.Now.ToString("yyyy-MM")});
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Delete), new { id = Leave.LeaveId ?? "" });
            }
        }


        public IActionResult LeaveTypeIndex()
        {
            var objs = _context.LeaveType.OrderByDescending(x => x.CreatedAtUtc).ToList();
            return View(objs);
        }

        //display LeaveType create edit form
        [HttpGet]
        public IActionResult LeaveTypeForm(string id)
        {
            //create new
            if (id == null)
            {
                LeaveType newObj = new LeaveType();
                return View(newObj);
            }

            //edit LeaveType
            LeaveType obj = new LeaveType();
            obj = _context.LeaveType.Where(x => x.LeaveTypeId.Equals(id)).FirstOrDefault();

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //post submitted LeaveType data. if id is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitLeaveTypeForm([Bind("LeaveTypeId", "Name", "Description")]LeaveType leaveType)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(LeaveTypeForm), new { id = leaveType.LeaveTypeId ?? "" });
                }

                //create new
                if (leaveType.LeaveTypeId == null)
                {
                    if (await _context.LeaveType.AnyAsync(x => x.Name.Equals(leaveType.Name)))
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + leaveType.Name + " already exist";
                        return RedirectToAction(nameof(LeaveTypeForm), new { id = leaveType.LeaveTypeId ?? "" });
                    }

                    LeaveType newObj = new LeaveType();
                    newObj.LeaveTypeId = Guid.NewGuid().ToString();
                    newObj.Name = leaveType.Name;
                    newObj.Description = leaveType.Description;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.LeaveType.Add(newObj);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new item success.";
                    return RedirectToAction(nameof(LeaveTypeForm), new { id = newObj.LeaveTypeId ?? "" });
                }

                //edit existing
                LeaveType editObj = new LeaveType();
                LeaveType existObj = new LeaveType();
                editObj = await _context.LeaveType.Where(x => x.LeaveTypeId.Equals(leaveType.LeaveTypeId)).FirstOrDefaultAsync();
                existObj = await _context.LeaveType.Where(x => x.Name.Equals(leaveType.Name)).FirstOrDefaultAsync();

                if (existObj != null)
                {
                    if (editObj.LeaveTypeId != existObj.LeaveTypeId)
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + leaveType.Name + " already exist";
                        return RedirectToAction(nameof(LeaveTypeForm), new { id = leaveType.LeaveTypeId ?? "" });
                    }

                }

                editObj.Name = leaveType.Name;
                editObj.Description = leaveType.Description;
                editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                editObj.UpdatedAtUtc = DateTime.UtcNow;

                _context.Update(editObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Edit existing item success.";
                return RedirectToAction(nameof(LeaveTypeForm), new { id = leaveType.LeaveTypeId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(LeaveTypeForm), new { id = leaveType.LeaveTypeId ?? "" });
            }
        }

        //display item for deletion
        [HttpGet]
        public async Task<IActionResult> LeaveTypeDelete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = await _context.LeaveType.Where(x => x.LeaveTypeId.Equals(id)).FirstOrDefaultAsync();
            return View(obj);
        }

        //delete submitted item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitLeaveTypeDelete([Bind("LeaveTypeId")]LeaveType leaveType)
        {
            try
            {
                var deleteObj = await _context.LeaveType.Where(x => x.LeaveTypeId.Equals(leaveType.LeaveTypeId)).FirstOrDefaultAsync();
                if (deleteObj == null)
                {
                    return NotFound();
                }

                //cek existing leave
                Leave objCheck = new Leave();
                objCheck = await _context.Leave
                    .Where(x => x.LeaveTypeId.Equals(deleteObj.LeaveTypeId))
                    .FirstOrDefaultAsync();

                if (objCheck != null)
                {
                    TempData[StaticString.StatusMessage] = "Error: already used on transaction.";
                    return RedirectToAction(nameof(LeaveTypeDelete), new { id = leaveType.LeaveTypeId ?? "" });
                }

                _context.LeaveType.Remove(deleteObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete item success.";
                return RedirectToAction(nameof(LeaveTypeIndex));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(LeaveTypeDelete), new { id = leaveType.LeaveTypeId ?? "" });
            }
        }

    }
}