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
    [Authorize(Roles = Services.App.Pages.Ticket.RoleName)]
    public class TicketController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Services.App.ICommon _app;

        //dependency injection through constructor, to directly access services
        public TicketController(
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
            ViewData["TicketType"] = _app.GetTicketTypeSelectList();
            ViewData["OnBehalf"] = _app.GetEmployeeSelectList();
            ViewData["Agent"] = _app.GetEmployeeSelectList();
            ViewData["ParentTicketThread"] = _app.GetTicketSelectList();

        }

        //consume db context service, display all Ticket items
        public IActionResult Index(string period)
        {
            var Tickets = _context.Ticket                
                .Include(x => x.TicketType)
                .Include(x => x.OnBehalf)
                .Include(x => x.Agent)
                .Include(x => x.ParentTicketThread)
                .OrderByDescending(x => x.CreatedAtUtc)
                .ToList()
                .Where(x => x.SubmitDate.ToString("yyyy-MM").Equals(period));
            return View(Tickets.ToList());
        }

        //display Ticket create edit form
        [HttpGet]
        public IActionResult Form(string id)
        {

            //create new
            if (id == null)
            {
                //dropdownlist type
                FillDropdownListWithData();

                Ticket newTicket = new Ticket();
                newTicket.SubmitDate = DateTime.Now;
                return View(newTicket);
            }

            //edit Ticket
            Ticket Ticket = new Ticket();
            Ticket = _context.Ticket.Where(x => x.TicketId.Equals(id)).FirstOrDefault();

            if (Ticket == null)
            {
                return NotFound();
            }
            //dropdownlist type
            FillDropdownListWithData();

            return View(Ticket);

        }

        //post submitted Ticket data. if Ticket.TicketId is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitForm([Bind(
            "TicketId", 
            "TicketName", 
            "IsSolve", 
            "TicketTypeId", 
            "SubmitDate", 
            "Description",
            "OnBehalfId",
            "AgentId",
            "ParentTicketThreadId",
            "SolutionNote"
            )]Ticket Ticket)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(Form), new { id = Ticket.TicketId ?? "" });
                }

                //create new
                if (Ticket.TicketId == null)
                {
                    Ticket newTicket = new Ticket();
                    newTicket.TicketId = Guid.NewGuid().ToString();
                    newTicket.TicketName = Ticket.TicketName;
                    newTicket.TicketTypeId = Ticket.TicketTypeId;
                    newTicket.SubmitDate = Ticket.SubmitDate;
                    newTicket.IsSolve = Ticket.IsSolve;
                    newTicket.SolutionNote = Ticket.SolutionNote;
                    newTicket.Description = Ticket.Description;
                    newTicket.OnBehalfId = Ticket.OnBehalfId;
                    newTicket.AgentId = Ticket.AgentId;
                    newTicket.ParentTicketThreadId = Ticket.ParentTicketThreadId;
                    newTicket.CreatedBy = await _userManager.GetUserAsync(User);
                    newTicket.CreatedAtUtc = DateTime.UtcNow;
                    newTicket.UpdatedBy = newTicket.CreatedBy;
                    newTicket.UpdatedAtUtc = newTicket.CreatedAtUtc;

                    _context.Ticket.Add(newTicket);
                    _context.SaveChanges();

                    //dropdownlist type
                    FillDropdownListWithData();

                    TempData[StaticString.StatusMessage] = "Create new Ticket item success.";
                    return RedirectToAction(nameof(Form), new { id = newTicket.TicketId ?? "" });
                }

                //edit existing
                Ticket editTicket = new Ticket();
                editTicket = _context.Ticket.Where(x => x.TicketId.Equals(Ticket.TicketId)).FirstOrDefault();
                editTicket.TicketName = Ticket.TicketName;
                editTicket.TicketTypeId = Ticket.TicketTypeId;
                editTicket.SubmitDate = Ticket.SubmitDate;
                editTicket.IsSolve = Ticket.IsSolve;
                editTicket.SolutionNote = Ticket.SolutionNote;
                editTicket.Description = Ticket.Description;
                editTicket.OnBehalfId = Ticket.OnBehalfId;
                editTicket.AgentId = Ticket.AgentId;
                editTicket.ParentTicketThreadId = Ticket.ParentTicketThreadId;
                editTicket.UpdatedBy = await _userManager.GetUserAsync(User);
                editTicket.UpdatedAtUtc = DateTime.UtcNow;
                _context.Update(editTicket);
                _context.SaveChanges();

                //dropdownlist type
                FillDropdownListWithData();

                TempData[StaticString.StatusMessage] = "Edit existing Ticket item success.";
                return RedirectToAction(nameof(Form), new { id = Ticket.TicketId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Form), new { id = Ticket.TicketId ?? "" });
            }
        }

        //display Ticket item for deletion
        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //dropdownlist type
            FillDropdownListWithData();

            var Ticket = _context.Ticket
                 .Include(x => x.TicketType)
                .Include(x => x.OnBehalf)
                .Include(x => x.Agent)
                .Include(x => x.ParentTicketThread)
                .Where(x => x.TicketId.Equals(id)).FirstOrDefault();
            return View(Ticket);
        }

        //delete submitted Ticket item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitDelete([Bind("TicketId")]Ticket Ticket)
        {
            try
            {
                var deleteTicket = _context.Ticket.Where(x => x.TicketId.Equals(Ticket.TicketId)).FirstOrDefault();
                if (deleteTicket == null)
                {
                    return NotFound();
                }

                _context.Ticket.Remove(deleteTicket);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete Ticket item success.";
                return RedirectToAction(nameof(Index), new { period = DateTime.Now.ToString("yyyy-MM") });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Delete), new { id = Ticket.TicketId ?? "" });
            }
        }


        //display of TicketType
        public IActionResult TicketTypeIndex()
        {
            var objs = _context.TicketType.OrderByDescending(x => x.CreatedAtUtc).ToList();
            return View(objs);
        }

        //display TicketType create edit form
        [HttpGet]
        public IActionResult TicketTypeForm(string id)
        {
            //create new
            if (id == null)
            {
                TicketType newObj = new TicketType();
                return View(newObj);
            }

            //edit TicketType
            TicketType obj = new TicketType();
            obj = _context.TicketType.Where(x => x.TicketTypeId.Equals(id)).FirstOrDefault();

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //post submitted TicketType data. if id is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitTicketTypeForm([Bind("TicketTypeId", "Name", "Description")]TicketType TicketType)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(TicketTypeForm), new { id = TicketType.TicketTypeId ?? "" });
                }

                //create new
                if (TicketType.TicketTypeId == null)
                {
                    if (await _context.TicketType.AnyAsync(x => x.Name.Equals(TicketType.Name)))
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + TicketType.Name + " already exist";
                        return RedirectToAction(nameof(TicketTypeForm), new { id = TicketType.TicketTypeId ?? "" });
                    }

                    TicketType newObj = new TicketType();
                    newObj.TicketTypeId = Guid.NewGuid().ToString();
                    newObj.Name = TicketType.Name;
                    newObj.Description = TicketType.Description;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.TicketType.Add(newObj);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new item success.";
                    return RedirectToAction(nameof(TicketTypeForm), new { id = newObj.TicketTypeId ?? "" });
                }

                //edit existing
                TicketType editObj = new TicketType();
                TicketType existObj = new TicketType();
                editObj = await _context.TicketType.Where(x => x.TicketTypeId.Equals(TicketType.TicketTypeId)).FirstOrDefaultAsync();
                existObj = await _context.TicketType.Where(x => x.Name.Equals(TicketType.Name)).FirstOrDefaultAsync();

                if (existObj != null)
                {
                    if (editObj.TicketTypeId != existObj.TicketTypeId)
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + TicketType.Name + " already exist";
                        return RedirectToAction(nameof(TicketTypeForm), new { id = TicketType.TicketTypeId ?? "" });
                    }

                }

                editObj.Name = TicketType.Name;
                editObj.Description = TicketType.Description;
                editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                editObj.UpdatedAtUtc = DateTime.UtcNow;

                _context.Update(editObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Edit existing item success.";
                return RedirectToAction(nameof(TicketTypeForm), new { id = TicketType.TicketTypeId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(TicketTypeForm), new { id = TicketType.TicketTypeId ?? "" });
            }
        }

        //display item for deletion
        [HttpGet]
        public async Task<IActionResult> TicketTypeDelete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = await _context.TicketType.Where(x => x.TicketTypeId.Equals(id)).FirstOrDefaultAsync();
            return View(obj);
        }

        //delete submitted item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitTicketTypeDelete([Bind("TicketTypeId")]TicketType TicketType)
        {
            try
            {
                var deleteObj = await _context.TicketType.Where(x => x.TicketTypeId.Equals(TicketType.TicketTypeId)).FirstOrDefaultAsync();
                if (deleteObj == null)
                {
                    return NotFound();
                }

                //cek existing ticket
                Ticket objCheck = new Ticket();
                objCheck = await _context.Ticket
                    .Where(x => x.TicketTypeId.Equals(deleteObj.TicketTypeId))
                    .FirstOrDefaultAsync();

                if (objCheck != null)
                {
                    TempData[StaticString.StatusMessage] = "Error: already used on transaction.";
                    return RedirectToAction(nameof(TicketTypeDelete), new { id = TicketType.TicketTypeId ?? "" });
                }

                _context.TicketType.Remove(deleteObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete item success.";
                return RedirectToAction(nameof(TicketTypeIndex));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(TicketTypeDelete), new { id = TicketType.TicketTypeId ?? "" });
            }
        }

    }
}