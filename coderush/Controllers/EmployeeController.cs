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
    [Authorize(Roles = Services.App.Pages.Employee.RoleName)]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Services.Security.ICommon _security;
        private readonly Services.App.ICommon _app;
        private readonly SignInManager<ApplicationUser> _signInManager;

        //dependency injection through constructor, to directly access services
        public EmployeeController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            Services.Security.ICommon security,
            Services.App.ICommon app,
            SignInManager<ApplicationUser> signInManager
            )
        {
            _context = context;
            _userManager = userManager;
            _security = security;
            _app = app;
            _signInManager = signInManager;
        }

        //fill viewdata as dropdownlist datasource for employee form
        private void FillDropdownListForEmployeeForm()
        {
            ViewData["Designation"] = _app.GetDesignationSelectList();
            ViewData["Department"] = _app.GetDepartmentSelectList();
            ViewData["Gender"] = _app.GetGenderSelectList();
            ViewData["MaritalStatus"] = _app.GetMaritalStatusSelectList();
            ViewData["Supervisor"] = _app.GetEmployeeSelectList();
            ViewData["SystemUser"] = _app.GetSystemUserSelectList();
            ViewData["BenefitTemplate"] = _app.GetBenefitTemplateSelectList();
        }

        //consume db context service, display all employee
        public IActionResult Index()
        {
            var objs = _context.Employee
                .AsNoTracking()
                .Include(x => x.Designation)
                .Include(x => x.Department)
                .OrderByDescending(x => x.CreatedAtUtc).ToList();
            return View(objs);
        }

        [HttpGet]
        public async Task<IActionResult> LoginOnBehalf(string id)
        {
            Employee employee = new Employee();
            employee = _context.Employee.Where(x => x.EmployeeId.Equals(id)).FirstOrDefault();
            if (employee == null)
            {
                return NotFound();
            }

            ApplicationUser appUser = new ApplicationUser();
            appUser = await _userManager.FindByIdAsync(employee.SystemUserId);

            if (appUser == null)
            {
                return NotFound();
            }

            //attempt to sign in
            await _signInManager.SignInAsync(appUser, false);

            await _signInManager.RefreshSignInAsync(appUser);

            return RedirectToAction("Index", "SelfService", new { period = DateTime.Now.ToString("yyyy-MM") });
        }

        //display employee create edit form
        [HttpGet]
        public IActionResult Form(string id)
        {
            //create new
            if (id == null)
            {
                //dropdownlist 
                FillDropdownListForEmployeeForm();

                Employee newObj = new Employee();
                newObj.JoiningDate = DateTime.Now;
                newObj.DateOfBirth = DateTime.Now.AddYears(-20);
                return View(newObj);
            }

            //edit object
            Employee editObj = new Employee();
            editObj = _context.Employee.Where(x => x.EmployeeId.Equals(id)).FirstOrDefault();

            if (editObj == null)
            {
                return NotFound();
            }

            //dropdownlist 
            FillDropdownListForEmployeeForm();

            return View(editObj);

        }

        //post submitted employee data. if employeeId is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitForm([Bind(
            "EmployeeId", 
            "FirstName", 
            "LastName", 
            "Gender",
            "DateOfBirth",
            "PlaceOfBirth",
            "MaritalStatus",
            "Email",
            "Phone",
            "Address1",
            "Address2",
            "City",
            "StateProvince",
            "ZipCode",
            "Country",
            "EmployeeIDNumber",
            "DesignationId",
            "DepartmentId",
            "JoiningDate",
            "LeavingDate",
            "SupervisorId",
            "BasicSalary",
            "UnpaidLeavePerDay",
            "AccountTitle",
            "BankName",
            "AccountNumber",
            "SwiftCode",
            "SystemUserId",
            "BenefitTemplateId"
            )]Employee employee)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(Form), new { id = employee.EmployeeId ?? "" });
                }

                

                //create new
                if (employee.EmployeeId == null)
                {
                    //duplicate Employee Number ID is not allowed
                    Employee empNumber = await _context.Employee.Where(x => x.EmployeeIDNumber.Equals(employee.EmployeeIDNumber)).FirstOrDefaultAsync();
                    if (empNumber != null && !String.IsNullOrEmpty(employee.EmployeeIDNumber))
                    {
                        TempData[StaticString.StatusMessage] = "Error: Employee ID Number Can Not Duplicate. " + employee.EmployeeIDNumber;
                        return RedirectToAction(nameof(Form), new { id = employee.EmployeeId ?? "" });
                    }

                    //duplicate System user account is not allowed
                    Employee empSystem = await _context.Employee.Where(x => x.SystemUserId.Equals(employee.SystemUserId)).FirstOrDefaultAsync();
                    if (empSystem != null && !String.IsNullOrEmpty(employee.SystemUserId))
                    {
                        TempData[StaticString.StatusMessage] = "Error: Application System User Account Already Been Used. " + employee.SystemUserId;
                        return RedirectToAction(nameof(Form), new { id = employee.EmployeeId ?? "" });
                    }

                    Employee newEmployee = new Employee();
                    newEmployee.EmployeeId = Guid.NewGuid().ToString();
                    newEmployee.FirstName = employee.FirstName;
                    newEmployee.LastName = employee.LastName;
                    newEmployee.Gender = employee.Gender;
                    newEmployee.DateOfBirth = employee.DateOfBirth;
                    newEmployee.PlaceOfBirth = employee.PlaceOfBirth;
                    newEmployee.MaritalStatus = employee.MaritalStatus;
                    newEmployee.Email = employee.Email;
                    newEmployee.Phone = employee.Phone;
                    newEmployee.Address1 = employee.Address1;
                    newEmployee.Address2 = employee.Address2;
                    newEmployee.City = employee.City;
                    newEmployee.StateProvince = employee.StateProvince;
                    newEmployee.ZipCode = employee.ZipCode;
                    newEmployee.Country = employee.Country;
                    newEmployee.EmployeeIDNumber = employee.EmployeeIDNumber;
                    newEmployee.DesignationId = employee.DesignationId;
                    newEmployee.DepartmentId = employee.DepartmentId;
                    newEmployee.JoiningDate = employee.JoiningDate;
                    if (employee.LeavingDate.HasValue) newEmployee.LeavingDate = employee.LeavingDate.Value;
                    newEmployee.SupervisorId = employee.SupervisorId;
                    newEmployee.BasicSalary = employee.BasicSalary;
                    newEmployee.UnpaidLeavePerDay = employee.UnpaidLeavePerDay;
                    newEmployee.BenefitTemplateId = employee.BenefitTemplateId;
                    newEmployee.AccountTitle = employee.AccountTitle;
                    newEmployee.BankName = employee.BankName;
                    newEmployee.AccountNumber = employee.AccountNumber;
                    newEmployee.SwiftCode = employee.SwiftCode;
                    newEmployee.SystemUserId = employee.SystemUserId;
                    newEmployee.CreatedBy = await _userManager.GetUserAsync(User);
                    newEmployee.CreatedAtUtc = DateTime.UtcNow;
                    newEmployee.UpdatedBy = newEmployee.CreatedBy;
                    newEmployee.UpdatedAtUtc = newEmployee.CreatedAtUtc;

                    _context.Employee.Add(newEmployee);
                    _context.SaveChanges();

                    //dropdownlist 
                    FillDropdownListForEmployeeForm();

                    TempData[StaticString.StatusMessage] = "Create new employee success.";
                    return RedirectToAction(nameof(Form), new { id = newEmployee.EmployeeId ?? "" });
                }

                //edit existing
                Employee editEmployee = new Employee();
                editEmployee = _context.Employee.Where(x => x.EmployeeId.Equals(employee.EmployeeId)).FirstOrDefault();

                if (editEmployee != null)
                {
                    //duplicate Employee Number ID is not allowed
                    Employee empNumber = await _context.Employee.Where(x => x.EmployeeIDNumber.Equals(employee.EmployeeIDNumber)).FirstOrDefaultAsync();
                    if (empNumber != null && editEmployee.EmployeeIDNumber != employee.EmployeeIDNumber && !String.IsNullOrEmpty(employee.EmployeeIDNumber))
                    {
                        TempData[StaticString.StatusMessage] = "Error: Employee ID Number Can Not Duplicate. " + employee.EmployeeIDNumber;
                        return RedirectToAction(nameof(Form), new { id = employee.EmployeeId ?? "" });
                    }

                    //duplicate System user account is not allowed
                    Employee empSystem = await _context.Employee.Where(x => x.SystemUserId.Equals(employee.SystemUserId)).FirstOrDefaultAsync();
                    if (empSystem != null && editEmployee.SystemUserId != employee.SystemUserId && !String.IsNullOrEmpty(employee.SystemUserId))
                    {
                        TempData[StaticString.StatusMessage] = "Error: Application System User Account Already Been Used. " + employee.SystemUserId;
                        return RedirectToAction(nameof(Form), new { id = employee.EmployeeId ?? "" });
                    }

                    editEmployee.FirstName = employee.FirstName;
                    editEmployee.LastName = employee.LastName;
                    editEmployee.Gender = employee.Gender;
                    editEmployee.DateOfBirth = employee.DateOfBirth;
                    editEmployee.PlaceOfBirth = employee.PlaceOfBirth;
                    editEmployee.MaritalStatus = employee.MaritalStatus;
                    editEmployee.Email = employee.Email;
                    editEmployee.Phone = employee.Phone;
                    editEmployee.Address1 = employee.Address1;
                    editEmployee.Address2 = employee.Address2;
                    editEmployee.City = employee.City;
                    editEmployee.StateProvince = employee.StateProvince;
                    editEmployee.ZipCode = employee.ZipCode;
                    editEmployee.Country = employee.Country;
                    editEmployee.EmployeeIDNumber = employee.EmployeeIDNumber;
                    editEmployee.DesignationId = employee.DesignationId;
                    editEmployee.DepartmentId = employee.DepartmentId;
                    editEmployee.JoiningDate = employee.JoiningDate;
                    if (editEmployee.LeavingDate.HasValue) editEmployee.LeavingDate = employee.LeavingDate.Value;
                    editEmployee.SupervisorId = employee.SupervisorId;
                    editEmployee.BasicSalary = employee.BasicSalary;
                    editEmployee.UnpaidLeavePerDay = employee.UnpaidLeavePerDay;
                    editEmployee.BenefitTemplateId = employee.BenefitTemplateId;
                    editEmployee.AccountTitle = employee.AccountTitle;
                    editEmployee.BankName = employee.BankName;
                    editEmployee.AccountNumber = employee.AccountNumber;
                    editEmployee.SwiftCode = employee.SwiftCode;
                    editEmployee.SystemUserId = employee.SystemUserId;
                    editEmployee.UpdatedBy = await _userManager.GetUserAsync(User);
                    editEmployee.UpdatedAtUtc = DateTime.UtcNow;
                    _context.Update(editEmployee);
                    _context.SaveChanges();

                    //dropdownlist 
                    FillDropdownListForEmployeeForm();

                    TempData[StaticString.StatusMessage] = "Edit existing employee item success.";
                    return RedirectToAction(nameof(Form), new { id = editEmployee.EmployeeId ?? "" });
                }
                else
                {
                    return NotFound();
                }


            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Form), new { id = employee.EmployeeId ?? "" });
            }
        }

        //display employee item for deletion
        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var del = _context.Employee
                .AsNoTracking()
                .Include(x => x.Designation)
                .Include(x => x.Department)
                .Where(x => x.EmployeeId.Equals(id)).FirstOrDefault();

            //dropdownlist 
            FillDropdownListForEmployeeForm();

            return View(del);
        }

        //delete submitted employee if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitDelete([Bind("EmployeeId")]Employee employee)
        {
            try
            {
                var del = _context.Employee.Where(x => x.EmployeeId.Equals(employee.EmployeeId)).FirstOrDefault();
                if (del == null)
                {
                    return NotFound();
                }

                _context.Employee.Remove(del);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete employee success.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Delete), new { id = employee.EmployeeId ?? "" });
            }
        }

        public IActionResult DesignationIndex()
        {
            var objs = _context.Designation.OrderByDescending(x => x.CreatedAtUtc).ToList();
            return View(objs);
        }

        //display Designation create edit form
        [HttpGet]
        public IActionResult DesignationForm(string id)
        {
            //create new
            if (id == null)
            {
                Designation newObj = new Designation();
                return View(newObj);
            }

            //edit Designation
            Designation obj = new Designation();
            obj = _context.Designation.Where(x => x.DesignationId.Equals(id)).FirstOrDefault();

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //post submitted Designation data. if id is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitDesignationForm([Bind("DesignationId", "Name", "Description")]Designation designation)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(DesignationForm), new { id = designation.DesignationId ?? "" });
                }

                //create new
                if (designation.DesignationId == null)
                {
                    if (await _context.Designation.AnyAsync(x => x.Name.Equals(designation.Name)))
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + designation.Name + " already exist";
                        return RedirectToAction(nameof(DesignationForm), new { id = designation.DesignationId ?? "" });
                    }

                    Designation newObj = new Designation();
                    newObj.DesignationId = Guid.NewGuid().ToString();
                    newObj.Name = designation.Name;
                    newObj.Description = designation.Description;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.Designation.Add(newObj);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new item success.";
                    return RedirectToAction(nameof(DesignationForm), new { id = newObj.DesignationId ?? "" });
                }

                //edit existing
                Designation editObj = new Designation();
                Designation existObj = new Designation();
                editObj = await _context.Designation.Where(x => x.DesignationId.Equals(designation.DesignationId)).FirstOrDefaultAsync();
                existObj = await _context.Designation.Where(x => x.Name.Equals(designation.Name)).FirstOrDefaultAsync();

                if (existObj != null)
                {
                    if (editObj.DesignationId != existObj.DesignationId)
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + designation.Name + " already exist";
                        return RedirectToAction(nameof(DesignationForm), new { id = designation.DesignationId ?? "" });
                    }
                    
                }

                editObj.Name = designation.Name;
                editObj.Description = designation.Description;
                editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                editObj.UpdatedAtUtc = DateTime.UtcNow;

                _context.Update(editObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Edit existing item success.";
                return RedirectToAction(nameof(DesignationForm), new { id = designation.DesignationId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(DesignationForm), new { id = designation.DesignationId ?? "" });
            }
        }

        //display item for deletion
        [HttpGet]
        public async Task<IActionResult> DesignationDelete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = await _context.Designation.Where(x => x.DesignationId.Equals(id)).FirstOrDefaultAsync();
            return View(obj);
        }

        //delete submitted item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitDesignationDelete([Bind("DesignationId")]Designation designation)
        {
            try
            {
                var deleteObj = await _context.Designation.Where(x => x.DesignationId.Equals(designation.DesignationId)).FirstOrDefaultAsync();
                if (deleteObj == null)
                {
                    return NotFound();
                }

                //cek existing ke employee
                Employee objCheck = new Employee();
                objCheck = await _context.Employee
                    .Where(x => x.DesignationId.Equals(deleteObj.DesignationId))
                    .FirstOrDefaultAsync();

                if (objCheck != null)
                {
                    TempData[StaticString.StatusMessage] = "Error: already used on transaction.";
                    return RedirectToAction(nameof(DesignationDelete), new { id = designation.DesignationId ?? "" });
                }

                _context.Designation.Remove(deleteObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete item success.";
                return RedirectToAction(nameof(DesignationIndex));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(DesignationDelete), new { id = designation.DesignationId ?? "" });
            }
        }

        public IActionResult DepartmentIndex()
        {
            var objs = _context.Department.OrderByDescending(x => x.CreatedAtUtc).ToList();
            return View(objs);
        }

        //display Department create edit form
        [HttpGet]
        public IActionResult DepartmentForm(string id)
        {
            //create new
            if (id == null)
            {
                Department newObj = new Department();
                return View(newObj);
            }

            //edit Department
            Department obj = new Department();
            obj = _context.Department.Where(x => x.DepartmentId.Equals(id)).FirstOrDefault();

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //post submitted Department data. if id is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitDepartmentForm([Bind("DepartmentId", "Name", "Description")]Department department)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(DepartmentForm), new { id = department.DepartmentId ?? "" });
                }

                //create new
                if (department.DepartmentId == null)
                {
                    if (await _context.Department.AnyAsync(x => x.Name.Equals(department.Name)))
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + department.Name + " already exist";
                        return RedirectToAction(nameof(DepartmentForm), new { id = department.DepartmentId ?? "" });
                    }

                    Department newObj = new Department();
                    newObj.DepartmentId = Guid.NewGuid().ToString();
                    newObj.Name = department.Name;
                    newObj.Description = department.Description;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.Department.Add(newObj);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new item success.";
                    return RedirectToAction(nameof(DepartmentForm), new { id = newObj.DepartmentId ?? "" });
                }

                //edit existing
                Department editObj = new Department();
                Department existObj = new Department();
                editObj = await _context.Department.Where(x => x.DepartmentId.Equals(department.DepartmentId)).FirstOrDefaultAsync();
                existObj = await _context.Department.Where(x => x.Name.Equals(department.Name)).FirstOrDefaultAsync();

                if (existObj != null && editObj.DepartmentId != existObj.DepartmentId)
                {
                    TempData[StaticString.StatusMessage] = "Error: " + department.Name + " already exist";
                    return RedirectToAction(nameof(DepartmentForm), new { id = department.DepartmentId ?? "" });
                }

                editObj.Name = department.Name;
                editObj.Description = department.Description;
                editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                editObj.UpdatedAtUtc = DateTime.UtcNow;

                _context.Update(editObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Edit existing item success.";
                return RedirectToAction(nameof(DepartmentForm), new { id = department.DepartmentId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(DepartmentForm), new { id = department.DepartmentId ?? "" });
            }
        }


        //display item for deletion
        [HttpGet]
        public async Task<IActionResult> DepartmentDelete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = await _context.Department.Where(x => x.DepartmentId.Equals(id)).FirstOrDefaultAsync();
            return View(obj);
        }

        //delete submitted item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitDepartmentDelete([Bind("DepartmentId")]Department department)
        {
            try
            {
                var deleteObj = await _context.Department.Where(x => x.DepartmentId.Equals(department.DepartmentId)).FirstOrDefaultAsync();
                if (deleteObj == null)
                {
                    return NotFound();
                }

                //cek existing ke employee
                Employee objCheck = new Employee();
                objCheck = await _context.Employee
                    .Where(x => x.DepartmentId.Equals(deleteObj.DepartmentId))
                    .FirstOrDefaultAsync();

                if (objCheck != null)
                {
                    TempData[StaticString.StatusMessage] = "Error: already used on transaction.";
                    return RedirectToAction(nameof(DepartmentDelete), new { id = department.DepartmentId ?? "" });
                }

                _context.Department.Remove(deleteObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete item success.";
                return RedirectToAction(nameof(DepartmentIndex));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(DepartmentDelete), new { id = department.DepartmentId ?? "" });
            }
        }

    }
}