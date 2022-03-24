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
    [Authorize(Roles = Services.App.Pages.SelfService.RoleName)]
    public class SelfServiceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Services.Security.ICommon _security;
        private readonly Services.App.ICommon _app;

        //dependency injection through constructor, to directly access services
        public SelfServiceController(ApplicationDbContext context
            , UserManager<ApplicationUser> userManager
            , Services.Security.ICommon security
            , Services.App.ICommon app
            )
        {
            _context = context;
            _userManager = userManager;
            _security = security;
            _app = app;
        }

        //fill viewdata as dropdownlist datasource 
        private async Task FillDropdownListWithData()
        {
            var employeeId = await _security.GetCurrentEmployeeLoginId(User);

            ViewData["OnBehalf"] = _app.GetEmployeeSelectList();
            ViewData["Designation"] = _app.GetDesignationSelectList();
            ViewData["Department"] = _app.GetDepartmentSelectList();
            ViewData["Gender"] = _app.GetGenderSelectList();
            ViewData["MaritalStatus"] = _app.GetMaritalStatusSelectList();
            ViewData["Supervisor"] = _app.GetEmployeeSelectList();
            ViewData["SystemUser"] = _app.GetSystemUserSelectList();
            ViewData["LeaveType"] = _app.GetLeaveTypeSelectList();
            ViewData["BenefitTemplate"] = _app.GetBenefitTemplateSelectList();
            ViewData["Allowance"] = _app.GetAllowanceTypeSelectList();
            ViewData["Deduction"] = _app.GetDeductionTypeSelectList();
            ViewData["ExpenseType"] = _app.GetExpenseTypeSelectList();
            ViewData["TicketType"] = _app.GetTicketTypeSelectList();
            ViewData["Agent"] = _app.GetEmployeeSelectList();
            ViewData["InformationType"] = _app.GetInformationTypeSelectList();
            ViewData["ParentTicketThread"] = _app.GetTicketSelectListByEmployeeId(employeeId);
        }



        //clock in or out
        [HttpGet]
        public async Task<IActionResult> Form(string id)
        {
            Employee currentEmployeeLogin = await _security.GetCurrentEmployeeLogin(User);

            if (currentEmployeeLogin == null)
            {
                return NotFound();
            }

            //create new
            if (id == null)
            {
                //dropdownlist type
                await FillDropdownListWithData();

                Attendance newAttendance = new Attendance();
                newAttendance.Clock = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
                newAttendance.AttendanceName = newAttendance.Clock.ToString("yyyy-MM-dd HH:mm:ss") + " " + currentEmployeeLogin.EmployeeIDNumber;
                newAttendance.OnBehalfId = await _security.GetCurrentEmployeeLoginId(User);
                return View(newAttendance);
            }

            //edit Attendance not allowed
            Attendance Attendance = new Attendance();
            Attendance = _context.Attendance.Where(x => x.AttendanceId.Equals(id)).FirstOrDefault();

            if (Attendance == null)
            {
                return NotFound();
            }

            //dropdownlist type
            await FillDropdownListWithData();

            return View(Attendance);

        }

        //post submitted Attendance data. if Attendance.AttendanceId is null then create new, otherwise edit (edit not allowed)
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
                    await FillDropdownListWithData();

                    TempData[StaticString.StatusMessage] = "Clock In / Out success.";
                    return RedirectToAction(nameof(Form), new { id = Attendance.AttendanceId ?? "" });
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
                await FillDropdownListWithData();

                TempData[StaticString.StatusMessage] = "Edit existing Attendance item success.";
                return RedirectToAction(nameof(Form), new { id = Attendance.AttendanceId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Form), new { id = Attendance.AttendanceId ?? "" });
            }
        }

        //display employee profile
        [HttpGet]
        public async Task<IActionResult> EmployeeProfile(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //edit object
            Employee editObj = new Employee();
            editObj = _context.Employee
                .Where(x => x.EmployeeId.Equals(id))
                .Include(x => x.Designation)
                .FirstOrDefault();

            if (editObj == null)
            {
                return NotFound();
            }

            //dropdownlist 
            await FillDropdownListWithData();

            return View(editObj);
        }

        //display attendance
        public async Task<IActionResult> Index(string period)
        {
            string currentEmployeeLoginId = await _security.GetCurrentEmployeeLoginId(User);

            List<Attendance> Attendances = new List<Attendance>();

            if (!String.IsNullOrEmpty(period))
            {
                Attendances = _context.Attendance
                        .Where(x => x.OnBehalfId.Equals(currentEmployeeLoginId))
                        .Include(x => x.OnBehalf)
                        .OrderByDescending(x => x.CreatedAtUtc)
                        .ToList()
                        .Where(x => x.Clock.ToString("yyyy-MM").Equals(period)).ToList();

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

        //display sallary
        public async Task<IActionResult> Sallary(string period)
        {
            string currentEmployeeLoginId = await _security.GetCurrentEmployeeLoginId(User);

            var Payrolls = _context.Payroll
                  .Where(x => x.OnBehalfId.Equals(currentEmployeeLoginId))
                  .Include(x => x.OnBehalf)
                  .OrderByDescending(x => x.CreatedAtUtc)
                  .ToList()
                  .Where(x => x.Periode.ToString("yyyy-MM").Equals(period));
            return View(Payrolls.ToList());
        }

        //display Payroll form
        [HttpGet]
        public async Task<IActionResult> SallaryForm(string id)
        {

            //create new
            if (id == null)
            {
                //dropdownlist type
                await FillDropdownListWithData();

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
            await FillDropdownListWithData();

            return View(Payroll);

        }

        //display Leave
        public async Task<IActionResult> Leave(string period)
        {
            string currentEmployeeLoginId = await _security.GetCurrentEmployeeLoginId(User);

            var Leaves = _context.Leave
                .Where(x => x.OnBehalfId.Equals(currentEmployeeLoginId))
                .Include(x => x.LeaveType)
                .Include(x => x.OnBehalf)
                .OrderByDescending(x => x.CreatedAtUtc)
                .ToList()
                .Where(x => x.FromDate.ToString("yyyy-MM").Equals(period));

            return View(Leaves.ToList());
        }

        //display Leave create edit form
        [HttpGet]
        public async Task<IActionResult> LeaveForm(string id)
        {
            string currentEmployeeLoginId = await _security.GetCurrentEmployeeLoginId(User);


            //create new
            if (id == null)
            {
                //dropdownlist type
                await FillDropdownListWithData();

                Leave newLeave = new Leave();
                newLeave.FromDate = DateTime.Now;
                newLeave.ToDate = DateTime.Now;
                newLeave.OnBehalfId = currentEmployeeLoginId;
                newLeave.LeaveName = "-";
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
            await FillDropdownListWithData();

            return View(Leave);

        }

        //post submitted Leave data. if Leave.LeaveId is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitLeaveForm([Bind(
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
                    return RedirectToAction(nameof(LeaveForm), new { id = Leave.LeaveId ?? "" });
                }

                Employee onBehalf = new Employee();
                onBehalf = _context.Employee.Where(x => x.EmployeeId.Equals(Leave.OnBehalfId)).FirstOrDefault();

                //create new
                if (Leave.LeaveId == null)
                {
                    Leave newLeave = new Leave();

                    newLeave.LeaveId = Guid.NewGuid().ToString();
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
                    newLeave.LeaveName = Leave.FromDate.ToString("yyyy-MM-dd") + " " + onBehalf.EmployeeIDNumber;

                    _context.Leave.Add(newLeave);
                    _context.SaveChanges();

                    //dropdownlist type
                    await FillDropdownListWithData();

                    TempData[StaticString.StatusMessage] = "Create new Leave item success.";
                    return RedirectToAction(nameof(LeaveForm), new { id = newLeave.LeaveId ?? "" });
                }

                //edit existing
                Leave editLeave = new Leave();
                editLeave = _context.Leave.Where(x => x.LeaveId.Equals(Leave.LeaveId)).FirstOrDefault();                
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
                editLeave.LeaveName = Leave.FromDate.ToString("yyyy-MM-dd") + " " + onBehalf.EmployeeIDNumber;

                _context.Update(editLeave);
                _context.SaveChanges();

                //dropdownlist type
                await FillDropdownListWithData();

                TempData[StaticString.StatusMessage] = "Edit existing Leave item success.";
                return RedirectToAction(nameof(LeaveForm), new { id = Leave.LeaveId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(LeaveForm), new { id = Leave.LeaveId ?? "" });
            }
        }

        //display Leave item for deletion
        [HttpGet]
        public IActionResult LeaveDelete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Leave = _context.Leave
                .Include(x => x.LeaveType)
                .Include(x => x.OnBehalf)
                .Where(x => x.LeaveId.Equals(id)).FirstOrDefault();
            return View(Leave);
        }

        //delete submitted Leave item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitLeaveDelete([Bind("LeaveId")]Leave Leave)
        {
            try
            {
                var deleteLeave = _context.Leave
                    .Where(x => x.LeaveId.Equals(Leave.LeaveId)).FirstOrDefault();
                if (deleteLeave == null)
                {
                    return NotFound();
                }

                _context.Leave.Remove(deleteLeave);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete Leave item success.";
                return RedirectToAction(nameof(Leave), new { period = DateTime.Now.ToString("yyyy-MM") });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(LeaveDelete), new { id = Leave.LeaveId ?? "" });
            }
        }

        //display list of expenses
        public async Task<IActionResult> Expense(string period)
        {
            var employeeId = await _security.GetCurrentEmployeeLoginId(User);

            var Expenses = _context.Expense
                .Where(x => x.OnBehalfId.Equals(employeeId))
                .Include(x => x.ExpenseType)
                .Include(x => x.OnBehalf)
                .OrderByDescending(x => x.CreatedAtUtc)
                .ToList()
                .Where(x => x.FromDate.ToString("yyyy-MM").Equals(period));
            return View(Expenses.ToList());
        }

        //display Expense create edit form
        [HttpGet]
        public async Task<IActionResult> ExpenseForm(string id)
        {
            var employeeId = await _security.GetCurrentEmployeeLoginId(User);

            //create new
            if (id == null)
            {
                //dropdownlist type
                await FillDropdownListWithData();

                Expense newExpense = new Expense();
                newExpense.FromDate = DateTime.Now;
                newExpense.ToDate = DateTime.Now;
                newExpense.OnBehalfId = employeeId;
                newExpense.ExpenseName = "-";
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
            await FillDropdownListWithData();

            return View(Expense);

        }

        //post submitted Expense data. if Expense.ExpenseId is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitExpenseForm([Bind(
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
                    return RedirectToAction(nameof(ExpenseForm), new { id = Expense.ExpenseId ?? "" });
                }

                Employee onBehalf = await _security.GetCurrentEmployeeLogin(User);


                //create new
                if (Expense.ExpenseId == null)
                {
                    Expense newExpense = new Expense();
                    newExpense.ExpenseId = Guid.NewGuid().ToString();
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
                    newExpense.ExpenseName = Expense.FromDate.ToString("yyyy-MM-dd") + " " + onBehalf.EmployeeIDNumber;

                    _context.Expense.Add(newExpense);
                    _context.SaveChanges();

                    //dropdownlist type
                    await FillDropdownListWithData();

                    TempData[StaticString.StatusMessage] = "Create new Expense item success.";
                    return RedirectToAction(nameof(ExpenseForm), new { id = newExpense.ExpenseId ?? "" });
                }

                //edit existing
                Expense editExpense = new Expense();
                editExpense = _context.Expense.Where(x => x.ExpenseId.Equals(Expense.ExpenseId)).FirstOrDefault();
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
                editExpense.ExpenseName = Expense.FromDate.ToString("yyyy-MM-dd") + " " + onBehalf.EmployeeIDNumber;

                _context.Update(editExpense);
                _context.SaveChanges();

                //dropdownlist type
                await FillDropdownListWithData();

                TempData[StaticString.StatusMessage] = "Edit existing Expense item success.";
                return RedirectToAction(nameof(ExpenseForm), new { id = Expense.ExpenseId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(ExpenseForm), new { id = Expense.ExpenseId ?? "" });
            }
        }

        //display Expense item for deletion
        [HttpGet]
        public async Task<IActionResult> ExpenseDelete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //dropdownlist type
            await FillDropdownListWithData();

            var Expense = _context.Expense
                .Include(x => x.ExpenseType)
                .Include(x => x.OnBehalf)
                .Where(x => x.ExpenseId.Equals(id)).FirstOrDefault();
            return View(Expense);
        }

        //delete submitted Expense item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitExpenseDelete([Bind("ExpenseId")]Expense Expense)
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
                return RedirectToAction(nameof(Expense), new { period = DateTime.Now.ToString("yyyy-MM") });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(ExpenseDelete), new { id = Expense.ExpenseId ?? "" });
            }
        }

        //display all Ticket items
        public async Task<IActionResult> Ticket(string period)
        {
            var employeeId = await _security.GetCurrentEmployeeLoginId(User);

            var Tickets = _context.Ticket
                .Where(x => x.OnBehalfId.Equals(employeeId))
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
        public async Task<IActionResult> TicketForm(string id)
        {
            var employeeId = await _security.GetCurrentEmployeeLoginId(User);

            //create new
            if (id == null)
            {
                //dropdownlist type
                await FillDropdownListWithData();

                Ticket newTicket = new Ticket();
                newTicket.SubmitDate = DateTime.Now;
                newTicket.OnBehalfId = employeeId;
                newTicket.TicketName = "-";
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
            await FillDropdownListWithData();

            return View(Ticket);

        }

        //post submitted Ticket data. if Ticket.TicketId is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitTicketForm([Bind(
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
                    return RedirectToAction(nameof(TicketForm), new { id = Ticket.TicketId ?? "" });
                }

                Employee onBehalf = await _security.GetCurrentEmployeeLogin(User);

                //create new
                if (Ticket.TicketId == null)
                {
                    Ticket newTicket = new Ticket();
                    newTicket.TicketId = Guid.NewGuid().ToString();
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
                    newTicket.TicketName = Ticket.SubmitDate.ToString("yyyy-MM-dd") + " " + onBehalf.EmployeeIDNumber;

                    _context.Ticket.Add(newTicket);
                    _context.SaveChanges();

                    //dropdownlist type
                    await FillDropdownListWithData();

                    TempData[StaticString.StatusMessage] = "Create new Ticket item success.";
                    return RedirectToAction(nameof(TicketForm), new { id = newTicket.TicketId ?? "" });
                }

                //edit existing
                Ticket editTicket = new Ticket();
                editTicket = _context.Ticket.Where(x => x.TicketId.Equals(Ticket.TicketId)).FirstOrDefault();
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
                editTicket.TicketName = Ticket.SubmitDate.ToString("yyyy-MM-dd") + " " + onBehalf.EmployeeIDNumber;

                _context.Update(editTicket);
                _context.SaveChanges();

                //dropdownlist type
                await FillDropdownListWithData();

                TempData[StaticString.StatusMessage] = "Edit existing Ticket item success.";
                return RedirectToAction(nameof(TicketForm), new { id = Ticket.TicketId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(TicketForm), new { id = Ticket.TicketId ?? "" });
            }
        }


        //display Ticket item for deletion
        [HttpGet]
        public async Task<IActionResult> TicketDelete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //dropdownlist type
            await FillDropdownListWithData();

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
        public IActionResult SubmitTicketDelete([Bind("TicketId")]Ticket Ticket)
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
                return RedirectToAction(nameof(Ticket), new { period = DateTime.Now.ToString("yyyy-MM") });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(TicketDelete), new { id = Ticket.TicketId ?? "" });
            }
        }

        //display information list
        public IActionResult Information(string period)
        {
            var Informations = _context.Information                
                .Include(x => x.InformationType)  
                .ToList()
                .Where(x => x.ReleaseDate.ToString("yyyy-MM").Equals(period))
                .OrderByDescending(x => x.CreatedAtUtc)
                .ToList();
            return View(Informations);
        }


    }
}