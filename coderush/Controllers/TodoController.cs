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
    [Authorize(Roles = Services.App.Pages.Todo.RoleName)]
    public class TodoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Services.App.ICommon _app;

        //dependency injection through constructor, to directly access services
        public TodoController(
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
            ViewData["TodoType"] = _app.GetTodoTypeSelectList();
            ViewData["OnBehalf"] = _app.GetEmployeeSelectList();

        }

        //consume db context service, display all todo items
        public IActionResult Index(string period)
        {
            var todos = _context.Todo
                .Include(x => x.TodoType)
                .Include(x => x.OnBehalf)
                .OrderByDescending(x => x.CreatedAtUtc)
                .ToList()
                .Where(x => x.StartDate.ToString("yyyy-MM").Equals(period));
            return View(todos.ToList());
        }

        //display todo create edit form
        [HttpGet]
        public IActionResult Form(string id)
        {

            FillDropdownListWithData();

            //create new
            if (id == null)
            {
                

                Todo newTodo = new Todo();
                newTodo.StartDate = DateTime.Now;
                newTodo.EndDate = newTodo.StartDate.AddDays(3);
                return View(newTodo);
            }

            //edit todo
            Todo todo = new Todo();
            todo = _context.Todo.Where(x => x.TodoId.Equals(id)).FirstOrDefault();

            if (todo == null)
            {
                return NotFound();
            }
          
            return View(todo);

        }

        //post submitted todo data. if todo.TodoId is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitForm([Bind(
            "TodoId", 
            "TodoItem",
            "Description",
            "OnBehalfId",
            "IsDone", 
            "TodoTypeId", 
            "StartDate", 
            "EndDate")]Todo todo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(Form), new { id = todo.TodoId ?? "" });
                }

                //create new
                if (todo.TodoId == null)
                {
                    Todo newTodo = new Todo();
                    newTodo.TodoId = Guid.NewGuid().ToString();
                    newTodo.TodoItem = todo.TodoItem;
                    newTodo.Description = todo.Description;
                    newTodo.OnBehalfId = todo.OnBehalfId;
                    newTodo.TodoTypeId = todo.TodoTypeId;
                    newTodo.StartDate = todo.StartDate;
                    newTodo.EndDate = todo.EndDate;
                    newTodo.IsDone = todo.IsDone;
                    newTodo.CreatedBy = await _userManager.GetUserAsync(User);
                    newTodo.CreatedAtUtc = DateTime.UtcNow;
                    newTodo.UpdatedBy = newTodo.CreatedBy;
                    newTodo.UpdatedAtUtc = newTodo.CreatedAtUtc;

                    _context.Todo.Add(newTodo);
                    _context.SaveChanges();

                    //dropdownlist type
                    FillDropdownListWithData();

                    TempData[StaticString.StatusMessage] = "Create new todo item success.";
                    return RedirectToAction(nameof(Form), new { id = newTodo.TodoId ?? "" });
                }

                //edit existing
                Todo editTodo = new Todo();
                editTodo = _context.Todo.Where(x => x.TodoId.Equals(todo.TodoId)).FirstOrDefault();
                editTodo.TodoItem = todo.TodoItem;
                editTodo.Description = todo.Description;
                editTodo.OnBehalfId = todo.OnBehalfId;
                editTodo.TodoTypeId = todo.TodoTypeId;
                editTodo.StartDate = todo.StartDate;
                editTodo.EndDate = todo.EndDate;
                editTodo.IsDone = todo.IsDone;
                editTodo.UpdatedBy = await _userManager.GetUserAsync(User);
                editTodo.UpdatedAtUtc = DateTime.UtcNow;
                _context.Update(editTodo);
                _context.SaveChanges();

                //dropdownlist type
                FillDropdownListWithData();

                TempData[StaticString.StatusMessage] = "Edit existing todo item success.";
                return RedirectToAction(nameof(Form), new { id = todo.TodoId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Form), new { id = todo.TodoId ?? "" });
            }
        }

        //display todo item for deletion
        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            FillDropdownListWithData();
            var todo = _context.Todo.Include(x => x.TodoType).Where(x => x.TodoId.Equals(id)).FirstOrDefault();
            return View(todo);
        }

        //delete submitted todo item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitDelete([Bind("TodoId")]Todo todo)
        {
            try
            {
                var deleteTodo = _context.Todo.Where(x => x.TodoId.Equals(todo.TodoId)).FirstOrDefault();
                if (deleteTodo == null)
                {
                    return NotFound();
                }

                _context.Todo.Remove(deleteTodo);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete todo item success.";
                return RedirectToAction(nameof(Index), new { period = DateTime.Now.ToString("yyyy-MM") });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Delete), new { id = todo.TodoId ?? "" });
            }
        }

        //display of TodoType
        public IActionResult TodoTypeIndex()
        {
            var objs = _context.TodoType.OrderByDescending(x => x.CreatedAtUtc).ToList();
            return View(objs);
        }

        //display TodoType create edit form
        [HttpGet]
        public IActionResult TodoTypeForm(string id)
        {
            //create new
            if (id == null)
            {
                TodoType newObj = new TodoType();
                return View(newObj);
            }

            //edit TodoType
            TodoType obj = new TodoType();
            obj = _context.TodoType.Where(x => x.TodoTypeId.Equals(id)).FirstOrDefault();

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //post submitted TodoType data. if id is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitTodoTypeForm([Bind("TodoTypeId", "Name", "Description")]TodoType TodoType)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(TodoTypeForm), new { id = TodoType.TodoTypeId ?? "" });
                }

                //create new
                if (TodoType.TodoTypeId == null)
                {
                    if (await _context.TodoType.AnyAsync(x => x.Name.Equals(TodoType.Name)))
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + TodoType.Name + " already exist";
                        return RedirectToAction(nameof(TodoTypeForm), new { id = TodoType.TodoTypeId ?? "" });
                    }

                    TodoType newObj = new TodoType();
                    newObj.TodoTypeId = Guid.NewGuid().ToString();
                    newObj.Name = TodoType.Name;
                    newObj.Description = TodoType.Description;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.TodoType.Add(newObj);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new item success.";
                    return RedirectToAction(nameof(TodoTypeForm), new { id = newObj.TodoTypeId ?? "" });
                }

                //edit existing
                TodoType editObj = new TodoType();
                TodoType existObj = new TodoType();
                editObj = await _context.TodoType.Where(x => x.TodoTypeId.Equals(TodoType.TodoTypeId)).FirstOrDefaultAsync();
                existObj = await _context.TodoType.Where(x => x.Name.Equals(TodoType.Name)).FirstOrDefaultAsync();

                if (existObj != null)
                {
                    if (editObj.TodoTypeId != existObj.TodoTypeId)
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + TodoType.Name + " already exist";
                        return RedirectToAction(nameof(TodoTypeForm), new { id = TodoType.TodoTypeId ?? "" });
                    }

                }

                editObj.Name = TodoType.Name;
                editObj.Description = TodoType.Description;
                editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                editObj.UpdatedAtUtc = DateTime.UtcNow;

                _context.Update(editObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Edit existing item success.";
                return RedirectToAction(nameof(TodoTypeForm), new { id = TodoType.TodoTypeId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(TodoTypeForm), new { id = TodoType.TodoTypeId ?? "" });
            }
        }

        //display item for deletion
        [HttpGet]
        public async Task<IActionResult> TodoTypeDelete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = await _context.TodoType.Where(x => x.TodoTypeId.Equals(id)).FirstOrDefaultAsync();
            return View(obj);
        }

        //delete submitted item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitTodoTypeDelete([Bind("TodoTypeId")]TodoType TodoType)
        {
            try
            {
                var deleteObj = await _context.TodoType.Where(x => x.TodoTypeId.Equals(TodoType.TodoTypeId)).FirstOrDefaultAsync();
                if (deleteObj == null)
                {
                    return NotFound();
                }

                //check existing todo
                Todo objCheck = new Todo();
                objCheck = await _context.Todo
                    .Where(x => x.TodoTypeId.Equals(deleteObj.TodoTypeId))
                    .FirstOrDefaultAsync();

                if (objCheck != null)
                {
                    TempData[StaticString.StatusMessage] = "Error: already used on transaction.";
                    return RedirectToAction(nameof(TodoTypeDelete), new { id = TodoType.TodoTypeId ?? "" });
                }

                _context.TodoType.Remove(deleteObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete item success.";
                return RedirectToAction(nameof(TodoTypeIndex));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(TodoTypeDelete), new { id = TodoType.TodoTypeId ?? "" });
            }
        }

    }
}