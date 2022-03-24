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
    [Authorize(Roles = Services.App.Pages.Attendance.RoleName)]
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Services.App.ICommon _app;

        //dependency injection through constructor, to directly access services
        public AttendanceController(
            ApplicationDbContext context
            , UserManager<ApplicationUser> userManager
            , Services.App.ICommon app
            )
        {
            _context = context;
            _userManager = userManager;
            _app = app;
        }

        private void FillDropdownListWithData()
        {
            ViewData["OnBehalf"] = _app.GetEmployeeSelectList();
        }

        //display daily attendance, go to details for details for that day
        public IActionResult Index(string period)
        {
            List<Attendance> Attendances = new List<Attendance>();

            if (!String.IsNullOrEmpty(period))
            {
                Attendances = _context.Attendance                        
                        .Include(x => x.OnBehalf)
                        .OrderByDescending(x => x.CreatedAtUtc)
                        .ToList()
                        .Where(x => x.Clock.ToString("yyyy-MM").Equals(period))
                        .ToList();

                List<PublicHolidayLine> publicHolidayLines = new List<PublicHolidayLine>();
                publicHolidayLines = _context.PublicHolidayLine
                    .ToList()
                    .Where(x => x.PublicHolidayDate.ToString("yyyy-MM").Equals(period))
                    .ToList();

                List<string> holidays = new List<string>();
                foreach (var item in publicHolidayLines)
                {
                    holidays.Add(item.PublicHolidayDate.ToString("yyyy-MM-dd"));
                }
                ViewData["holidays"] = holidays;
            }


            return View(Attendances);
        }

        //display Attendance create edit form
        [HttpGet]
        public IActionResult Form(string id)
        {

            //create new
            if (id == null)
            {
                //dropdownlist type
                FillDropdownListWithData();

                Attendance newAttendance = new Attendance();
                newAttendance.Clock = DateTime.Now;
                return View(newAttendance);
            }

            //edit Attendance
            Attendance Attendance = new Attendance();
            Attendance = _context.Attendance.Where(x => x.AttendanceId.Equals(id)).FirstOrDefault();

            if (Attendance == null)
            {
                return NotFound();
            }

            //dropdownlist type
            FillDropdownListWithData();

            return View(Attendance);

        }

        //post submitted Attendance data. if Attendance.AttendanceId is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitForm([Bind(
            "AttendanceId",
            "AttendanceName",
            "Clock",
            "Description",
            "OnBehalfId"
            )]Attendance Attendance)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(Form), new { id = Attendance.AttendanceId ?? "" });
                }

                //create new
                if (Attendance.AttendanceId == null)
                {
                    Attendance newAttendance = new Attendance();
                    newAttendance.AttendanceId = Guid.NewGuid().ToString();
                    newAttendance.AttendanceName = Attendance.AttendanceName;
                    newAttendance.Clock = Attendance.Clock;
                    newAttendance.Description = Attendance.Description;
                    newAttendance.OnBehalfId = Attendance.OnBehalfId;
                    newAttendance.CreatedBy = await _userManager.GetUserAsync(User);
                    newAttendance.CreatedAtUtc = DateTime.UtcNow;
                    newAttendance.UpdatedBy = newAttendance.CreatedBy;
                    newAttendance.UpdatedAtUtc = newAttendance.CreatedAtUtc;

                    _context.Attendance.Add(newAttendance);
                    _context.SaveChanges();

                    //dropdownlist type
                    FillDropdownListWithData();

                    TempData[StaticString.StatusMessage] = "Create new Attendance item success.";
                    return RedirectToAction(nameof(Form), new { id = newAttendance.AttendanceId ?? "" });
                }

                //edit existing
                Attendance editAttendance = new Attendance();
                editAttendance = _context.Attendance.Where(x => x.AttendanceId.Equals(Attendance.AttendanceId)).FirstOrDefault();
                editAttendance.AttendanceName = Attendance.AttendanceName;
                editAttendance.Clock = Attendance.Clock;
                editAttendance.Description = Attendance.Description;
                editAttendance.OnBehalfId = Attendance.OnBehalfId;
                editAttendance.UpdatedBy = await _userManager.GetUserAsync(User);
                editAttendance.UpdatedAtUtc = DateTime.UtcNow;
                _context.Update(editAttendance);
                _context.SaveChanges();

                //dropdownlist type
                FillDropdownListWithData();

                TempData[StaticString.StatusMessage] = "Edit existing Attendance item success.";
                return RedirectToAction(nameof(Form), new { id = Attendance.AttendanceId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Form), new { id = Attendance.AttendanceId ?? "" });
            }
        }

        //display Attendance item for deletion
        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //dropdownlist type
            FillDropdownListWithData();

            var Attendance = _context.Attendance
                .Include(x => x.OnBehalf)
                .Where(x => x.AttendanceId.Equals(id)).FirstOrDefault();

            return View(Attendance);
        }

        //delete submitted Attendance item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitDelete([Bind("AttendanceId")]Attendance Attendance)
        {
            try
            {
                var deleteAttendance = _context.Attendance.Where(x => x.AttendanceId.Equals(Attendance.AttendanceId)).FirstOrDefault();
                if (deleteAttendance == null)
                {
                    return NotFound();
                }

                _context.Attendance.Remove(deleteAttendance);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete Attendance item success.";
                return RedirectToAction(nameof(Index), new { period = DateTime.Now.ToString("yyyy-MM") });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Delete), new { id = Attendance.AttendanceId ?? "" });
            }
        }


        public IActionResult PublicHolidayIndex()
        {
            var objs = _context.PublicHoliday.OrderByDescending(x => x.CreatedAtUtc).ToList();
            return View(objs);
        }

        //display PublicHoliday create edit form
        [HttpGet]
        public IActionResult PublicHolidayForm(string id)
        {
            //create new
            if (id == null)
            {
                PublicHoliday newObj = new PublicHoliday();
               
                return View(newObj);
            }

            //edit PublicHoliday
            PublicHoliday obj = new PublicHoliday();
            obj = _context.PublicHoliday
                .Include(x => x.Lines)
                .Where(x => x.PublicHolidayId.Equals(id))
                .FirstOrDefault();

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //post submitted PublicHoliday data. if id is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitPublicHolidayForm([Bind(
            "PublicHolidayId", 
            "Name", 
            "Description")]PublicHoliday PublicHoliday)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(PublicHolidayForm), new { id = PublicHoliday.PublicHolidayId ?? "" });
                }

                //create new
                if (PublicHoliday.PublicHolidayId == null)
                {
                    if (await _context.PublicHoliday.AnyAsync(x => x.Name.Equals(PublicHoliday.Name)))
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + PublicHoliday.Name + " already exist";
                        return RedirectToAction(nameof(PublicHolidayForm), new { id = PublicHoliday.PublicHolidayId ?? "" });
                    }

                    PublicHoliday newObj = new PublicHoliday();
                    newObj.PublicHolidayId = Guid.NewGuid().ToString();
                    newObj.Name = PublicHoliday.Name;
                    newObj.Description = PublicHoliday.Description;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.PublicHoliday.Add(newObj);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new item success.";
                    return RedirectToAction(nameof(PublicHolidayForm), new { id = newObj.PublicHolidayId ?? "" });
                }

                //edit existing
                PublicHoliday editObj = new PublicHoliday();
                PublicHoliday existObj = new PublicHoliday();
                editObj = await _context.PublicHoliday.Where(x => x.PublicHolidayId.Equals(PublicHoliday.PublicHolidayId)).FirstOrDefaultAsync();
                existObj = await _context.PublicHoliday.Where(x => x.Name.Equals(PublicHoliday.Name)).FirstOrDefaultAsync();

                if (existObj != null)
                {
                    if (editObj.PublicHolidayId != existObj.PublicHolidayId)
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + PublicHoliday.Name + " already exist";
                        return RedirectToAction(nameof(PublicHolidayForm), new { id = PublicHoliday.PublicHolidayId ?? "" });
                    }

                }

                editObj.Name = PublicHoliday.Name;
                editObj.Description = PublicHoliday.Description;
                editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                editObj.UpdatedAtUtc = DateTime.UtcNow;

                _context.Update(editObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Edit existing item success.";
                return RedirectToAction(nameof(PublicHolidayForm), new { id = PublicHoliday.PublicHolidayId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(PublicHolidayForm), new { id = PublicHoliday.PublicHolidayId ?? "" });
            }
        }

        //display item for deletion
        [HttpGet]
        public async Task<IActionResult> PublicHolidayDelete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = await _context.PublicHoliday.Where(x => x.PublicHolidayId.Equals(id)).FirstOrDefaultAsync();
            return View(obj);
        }

        //delete submitted item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitPublicHolidayDelete([Bind("PublicHolidayId")]PublicHoliday PublicHoliday)
        {
            try
            {
                var deleteObj = await _context.PublicHoliday.Where(x => x.PublicHolidayId.Equals(PublicHoliday.PublicHolidayId)).FirstOrDefaultAsync();
                if (deleteObj == null)
                {
                    return NotFound();
                }
                

                _context.PublicHoliday.Remove(deleteObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete item success.";
                return RedirectToAction(nameof(PublicHolidayIndex));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(PublicHolidayDelete), new { id = PublicHoliday.PublicHolidayId ?? "" });
            }
        }


        //display PublicHolidayLine create edit form
        [HttpGet]
        public IActionResult PublicHolidayLineForm(string id, string header)
        {
            //create new
            if (id == null)
            {
                PublicHolidayLine newObj = new PublicHolidayLine();
                newObj.PublicHolidayId = header;
                newObj.PublicHolidayDate = DateTime.Now;
                newObj.PublicHolidayYear = DateTime.Now.Year;
                return View(newObj);
            }

            //edit PublicHolidayLine
            PublicHolidayLine obj = new PublicHolidayLine();
            obj = _context.PublicHolidayLine.Where(x => x.PublicHolidayLineId.Equals(id)).FirstOrDefault();

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //post submitted PublicHolidayLine data. if id is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitPublicHolidayLineForm([Bind(
            "PublicHolidayLineId", 
            "PublicHolidayId",
            "PublicHolidayDate",
            "PublicHolidayYear",
            "Description")]PublicHolidayLine PublicHolidayLine)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(PublicHolidayLineForm), new { id = PublicHolidayLine.PublicHolidayLineId ?? "", header = PublicHolidayLine.PublicHolidayId });
                }

                //create new
                if (PublicHolidayLine.PublicHolidayLineId == null)
                {
                  

                    PublicHolidayLine newObj = new PublicHolidayLine();
                    newObj.PublicHolidayLineId = Guid.NewGuid().ToString();
                    newObj.PublicHolidayId = PublicHolidayLine.PublicHolidayId;
                    newObj.Description = PublicHolidayLine.Description;
                    newObj.PublicHolidayDate = PublicHolidayLine.PublicHolidayDate;
                    newObj.PublicHolidayYear = PublicHolidayLine.PublicHolidayYear;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.PublicHolidayLine.Add(newObj);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new item success.";
                    return RedirectToAction(nameof(PublicHolidayLineForm), new { id = newObj.PublicHolidayLineId ?? "", header = newObj.PublicHolidayId });
                }

                //edit existing
                PublicHolidayLine editObj = new PublicHolidayLine();
                PublicHolidayLine existObj = new PublicHolidayLine();
                editObj = await _context.PublicHolidayLine.Where(x => x.PublicHolidayLineId.Equals(PublicHolidayLine.PublicHolidayLineId)).FirstOrDefaultAsync();               
                editObj.Description = PublicHolidayLine.Description;
                editObj.PublicHolidayDate = PublicHolidayLine.PublicHolidayDate;
                editObj.PublicHolidayYear = PublicHolidayLine.PublicHolidayYear;
                editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                editObj.UpdatedAtUtc = DateTime.UtcNow;

                _context.Update(editObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Edit existing item success.";
                return RedirectToAction(nameof(PublicHolidayLineForm), new { id = PublicHolidayLine.PublicHolidayLineId ?? "", header = PublicHolidayLine.PublicHolidayId });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(PublicHolidayLineForm), new { id = PublicHolidayLine.PublicHolidayLineId ?? "", header = PublicHolidayLine.PublicHolidayId });
            }
        }

        //display item for deletion
        [HttpGet]
        public async Task<IActionResult> PublicHolidayLineDelete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = await _context.PublicHolidayLine.Where(x => x.PublicHolidayLineId.Equals(id)).FirstOrDefaultAsync();
            return View(obj);
        }

        //delete submitted item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitPublicHolidayLineDelete([Bind("PublicHolidayLineId")]PublicHolidayLine PublicHolidayLine)
        {
            try
            {
                var deleteObj = await _context.PublicHolidayLine.Where(x => x.PublicHolidayLineId.Equals(PublicHolidayLine.PublicHolidayLineId)).FirstOrDefaultAsync();
                if (deleteObj == null)
                {
                    return NotFound();
                }

                //todo: cek existing ke ..

                _context.PublicHolidayLine.Remove(deleteObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete item success.";
                return RedirectToAction(nameof(PublicHolidayForm), new { id = deleteObj.PublicHolidayId});
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(PublicHolidayLineDelete), new { id = PublicHolidayLine.PublicHolidayLineId ?? "", header = PublicHolidayLine.PublicHolidayId });
            }
        }


    }
}