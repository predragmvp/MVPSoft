using coderush.Data;
using coderush.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using coderush.Services.App;

namespace coderush.Controllers
{
    public class WebsiteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public WebsiteController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Services()
        {
            return View();
        }
        public IActionResult Development()
        {
            return View();
        }
        public IActionResult QA()
        {
            return View();
        }
        public IActionResult Team()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }


        [Authorize(Roles = Pages.JobApplications.RoleName)]
        public IActionResult Job()
        {
            var objs = _context.JobPost
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAtUtc).ToList();
            return View(objs);
        }
        //display Job create edit form
        [HttpGet]
        public IActionResult Form(string id)
        {
            //create new
            if (id == null)
            {
                JobPost newObj = new JobPost();
                return View(newObj);
            }
            //edit object
            JobPost editObj = new JobPost();
            editObj = _context.JobPost.Where(x => x.JobPostId.Equals(id)).FirstOrDefault();
            if (editObj == null)
            {
                return NotFound();
            }
            return View(editObj);
        }

        //post submitted Job data. if JobId is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitForm([Bind(
            "JobPostId",
            "JobTitle",
            "JobDescription",
            "IsActive"
            )]JobPost jobPost)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(Form), new { id = jobPost.JobPostId ?? "" });
                }

                //create new
                if (jobPost.JobPostId == null)
                {
                    JobPost newJobPost = new JobPost();
                    newJobPost.JobPostId = Guid.NewGuid().ToString();
                    newJobPost.JobTitle = jobPost.JobTitle;
                    newJobPost.JobDescription = jobPost.JobDescription;
                    newJobPost.IsActive = jobPost.IsActive;
                    
                    newJobPost.CreatedBy = await _userManager.GetUserAsync(User);
                    newJobPost.CreatedAtUtc = DateTime.UtcNow;
                    newJobPost.UpdatedBy = newJobPost.CreatedBy;
                    newJobPost.UpdatedAtUtc = newJobPost.CreatedAtUtc;

                    _context.JobPost.Add(newJobPost);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new job success.";
                    return RedirectToAction(nameof(Form), new { id = jobPost.JobPostId ?? "" });
                }

                //edit existing
                JobPost editJobPost = new JobPost();
                editJobPost = _context.JobPost.Where(x => x.JobPostId.Equals(jobPost.JobPostId)).FirstOrDefault();

                if (editJobPost != null)
                {

                    editJobPost.JobTitle = jobPost.JobTitle;
                    editJobPost.JobDescription = jobPost.JobDescription;
                    editJobPost.IsActive = jobPost.IsActive;
                    editJobPost.UpdatedBy = await _userManager.GetUserAsync(User);
                    editJobPost.UpdatedAtUtc = DateTime.UtcNow;
                    _context.Update(editJobPost);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Edit existing job item success.";
                    return RedirectToAction(nameof(Form), new { id = editJobPost.JobPostId ?? "" });
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Form), new { id = jobPost.JobPostId ?? "" });
            }
        }

        //display job item for deletion
        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var del = _context.JobPost
                .AsNoTracking()
                .Where(x => x.JobPostId.Equals(id)).FirstOrDefault();
            return View(del);
        }

        //delete submitted Job if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitDelete([Bind("JobPostId")] JobPost jobPost)
        {
            try
            {
                var del = _context.JobPost.Where(x => x.JobPostId.Equals(jobPost.JobPostId)).FirstOrDefault();
                if (del == null)
                {
                    return NotFound();
                }

                _context.JobPost.Remove(del);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete job success.";
                return RedirectToAction(nameof(Job));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Delete), new { id = jobPost.JobPostId ?? "" });
            }

        }

        public IActionResult Carer()
        {
            var objs = _context.JobPost
               .AsNoTracking()
               .Where(x => x.IsActive.Equals(true))
               .OrderByDescending(x => x.CreatedAtUtc).ToList();
            return View(objs);
        }


        //display Job create edit form
        [HttpGet]
        public IActionResult JobSubmit(string id)
        {
            JobApplication submitObj = new JobApplication();
            JobPost job = _context.JobPost.Where(x => x.JobPostId.Equals(id)).FirstOrDefault();
            if (job == null)
            {
                return NotFound();
            }
            submitObj.JobPostId = id;
            return View(submitObj);
        }

        //post submitted Job Apply data
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SubmitJobSubmit([Bind(
            "JobPostId",
            "Name",
            "Email",
            "Description",
            "Resume"
            )]JobApplication jobSubmit)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(JobSubmit), new { id = jobSubmit.JobPostId ?? "" });
                }

                //create new
                if (jobSubmit.JobPostId != null)
                {
                    JobApplication newJobSubmit = new JobApplication();
                    newJobSubmit.JobApplicationId = Guid.NewGuid().ToString();
                    newJobSubmit.Name = jobSubmit.Name;
                    newJobSubmit.Email = jobSubmit.Email;
                    newJobSubmit.Description = jobSubmit.Description;
                    newJobSubmit.JobPostId = jobSubmit.JobPostId;

                    var file = jobSubmit.Resume;
                    if (file != null && file.Length > 0)
                    {

                        string fileFullPath = "";
                        string c_name = "";
                        c_name = "UploadedCvs";
                        string FileNameInDb = $"/upload/{c_name}/";
                        string dirPath = $"./wwwroot/upload/{c_name}/";
                        bool isDirectoryExists = Directory.Exists(dirPath);
                        if (isDirectoryExists == false)
                        {
                            Directory.CreateDirectory(dirPath);
                        }
                        string fileName = Guid.NewGuid().ToString("N").Substring(0, 5) + "_" + file.FileName;
                        string fileFullPath_ = $"{dirPath}{fileName}";
                        FileNameInDb = FileNameInDb + $"{fileName}";
                        fileFullPath = fileFullPath_.Replace(" ", "");
                        FileNameInDb = FileNameInDb.Replace(" ", "");
                        using (var fileStream = new FileStream(fileFullPath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        newJobSubmit.ResumePath = FileNameInDb;
                    }
                    else
                    {
                        TempData[StaticString.StatusMessage] = "Error: Please Upload Cv.";
                        return RedirectToAction(nameof(JobSubmit), new { id = jobSubmit.JobPostId ?? "" });
                    }

                    newJobSubmit.CreatedBy = await _userManager.GetUserAsync(User);
                    newJobSubmit.CreatedAtUtc = DateTime.UtcNow;
                    newJobSubmit.UpdatedBy = newJobSubmit.CreatedBy;
                    newJobSubmit.UpdatedAtUtc = newJobSubmit.CreatedAtUtc;

                    _context.JobApplication.Add(newJobSubmit);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Apply on job success.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(JobSubmit), new { id = jobSubmit.JobPostId ?? "" });
            }
        }

        // show list of apply applicant on jobs
        [Authorize(Roles = Pages.JobApplications.RoleName)]
        public IActionResult JobApplications()
        {
            var JobAppliications = _context.JobApplication
                .AsNoTracking()
                .Include(x => x.JobPost)
                .OrderByDescending(x => x.CreatedAtUtc).ToList();
            return View(JobAppliications);
        }


    }
}
