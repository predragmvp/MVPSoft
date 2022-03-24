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
    [Authorize(Roles = Services.App.Pages.Recruitment.RoleName)]
    public class RecruitmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        //dependency injection through constructor, to directly access services
        public RecruitmentController(
            ApplicationDbContext context
            , UserManager<ApplicationUser> userManager
            )
        {
            _context = context;
            _userManager = userManager;
        }

        // display open position / vacancy
        public IActionResult Index()
        {
            var objs = _context
                .JobVacancy.OrderByDescending(x => x.CreatedAtUtc).ToList();
            return View(objs);
        }

        //display Recruitment / JobVacancy create edit form
        [HttpGet]
        public IActionResult Form(string id)
        {
            //create new
            if (id == null)
            {
                JobVacancy newObj = new JobVacancy();
                return View(newObj);
            }

            //edit JobVacancy
            JobVacancy obj = new JobVacancy();
            obj = _context
                .JobVacancy
                .Where(x => x.JobVacancyId.Equals(id)).FirstOrDefault();

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //post submitted JobVacancy data. if id is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitForm([Bind("JobVacancyId", "Name", "Description")]JobVacancy JobVacancy)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(Form), new { id = JobVacancy.JobVacancyId ?? "" });
                }

                //create new
                if (JobVacancy.JobVacancyId == null)
                {
                    if (await _context.JobVacancy.AnyAsync(x => x.Name.Equals(JobVacancy.Name)))
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + JobVacancy.Name + " already exist";
                        return RedirectToAction(nameof(Form), new { id = JobVacancy.JobVacancyId ?? "" });
                    }

                    JobVacancy newObj = new JobVacancy();
                    newObj.JobVacancyId = Guid.NewGuid().ToString();
                    newObj.Name = JobVacancy.Name;
                    newObj.Description = JobVacancy.Description;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.JobVacancy.Add(newObj);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new item success.";
                    return RedirectToAction(nameof(Form), new { id = newObj.JobVacancyId ?? "" });
                }

                //edit existing
                JobVacancy editObj = new JobVacancy();
                JobVacancy existObj = new JobVacancy();
                editObj = await _context.JobVacancy.Where(x => x.JobVacancyId.Equals(JobVacancy.JobVacancyId)).FirstOrDefaultAsync();
                existObj = await _context.JobVacancy.Where(x => x.Name.Equals(JobVacancy.Name)).FirstOrDefaultAsync();

                if (existObj != null)
                {
                    if (editObj.JobVacancyId != existObj.JobVacancyId)
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + JobVacancy.Name + " already exist";
                        return RedirectToAction(nameof(Form), new { id = JobVacancy.JobVacancyId ?? "" });
                    }

                }

                editObj.Name = JobVacancy.Name;
                editObj.Description = JobVacancy.Description;
                editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                editObj.UpdatedAtUtc = DateTime.UtcNow;

                _context.Update(editObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Edit existing item success.";
                return RedirectToAction(nameof(Form), new { id = JobVacancy.JobVacancyId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Form), new { id = JobVacancy.JobVacancyId ?? "" });
            }
        }

        //display item for deletion
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = await _context
                .JobVacancy
                .Where(x => x.JobVacancyId.Equals(id)).FirstOrDefaultAsync();
            return View(obj);
        }

        //delete submitted item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitDelete([Bind("JobVacancyId")]JobVacancy JobVacancy)
        {
            try
            {
                var deleteObj = await _context
                    .JobVacancy
                    .Where(x => x.JobVacancyId.Equals(JobVacancy.JobVacancyId)).FirstOrDefaultAsync();
                if (deleteObj == null)
                {
                    return NotFound();
                }

                //todo: cek existing ke ...

                _context.JobVacancy.Remove(deleteObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete item success.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Delete), new { id = JobVacancy.JobVacancyId ?? "" });
            }
        }


        public IActionResult ApplicantIndex()
        {
            var objs = _context.Applicant.OrderByDescending(x => x.CreatedAtUtc).ToList();
            return View(objs);
        }

        //display Applicant create edit form
        [HttpGet]
        public IActionResult ApplicantForm(string id)
        {
            //create new
            if (id == null)
            {
                Applicant newObj = new Applicant();
                return View(newObj);
            }

            //edit Applicant
            Applicant obj = new Applicant();
            obj = _context.Applicant.Where(x => x.ApplicantId.Equals(id)).FirstOrDefault();

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //post submitted Applicant data. if id is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitApplicantForm([Bind("ApplicantId", "Name", "Description")]Applicant Applicant)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(ApplicantForm), new { id = Applicant.ApplicantId ?? "" });
                }

                //create new
                if (Applicant.ApplicantId == null)
                {
                    if (await _context.Applicant.AnyAsync(x => x.Name.Equals(Applicant.Name)))
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + Applicant.Name + " already exist";
                        return RedirectToAction(nameof(ApplicantForm), new { id = Applicant.ApplicantId ?? "" });
                    }

                    Applicant newObj = new Applicant();
                    newObj.ApplicantId = Guid.NewGuid().ToString();
                    newObj.Name = Applicant.Name;
                    newObj.Description = Applicant.Description;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.Applicant.Add(newObj);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new item success.";
                    return RedirectToAction(nameof(ApplicantForm), new { id = newObj.ApplicantId ?? "" });
                }

                //edit existing
                Applicant editObj = new Applicant();
                Applicant existObj = new Applicant();
                editObj = await _context.Applicant.Where(x => x.ApplicantId.Equals(Applicant.ApplicantId)).FirstOrDefaultAsync();
                existObj = await _context.Applicant.Where(x => x.Name.Equals(Applicant.Name)).FirstOrDefaultAsync();

                if (existObj != null)
                {
                    if (editObj.ApplicantId != existObj.ApplicantId)
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + Applicant.Name + " already exist";
                        return RedirectToAction(nameof(ApplicantForm), new { id = Applicant.ApplicantId ?? "" });
                    }

                }

                editObj.Name = Applicant.Name;
                editObj.Description = Applicant.Description;
                editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                editObj.UpdatedAtUtc = DateTime.UtcNow;

                _context.Update(editObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Edit existing item success.";
                return RedirectToAction(nameof(ApplicantForm), new { id = Applicant.ApplicantId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(ApplicantForm), new { id = Applicant.ApplicantId ?? "" });
            }
        }

        //display item for deletion
        [HttpGet]
        public async Task<IActionResult> ApplicantDelete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = await _context.Applicant.Where(x => x.ApplicantId.Equals(id)).FirstOrDefaultAsync();
            return View(obj);
        }

        //delete submitted item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitApplicantDelete([Bind("ApplicantId")]Applicant Applicant)
        {
            try
            {
                var deleteObj = await _context.Applicant.Where(x => x.ApplicantId.Equals(Applicant.ApplicantId)).FirstOrDefaultAsync();
                if (deleteObj == null)
                {
                    return NotFound();
                }

                //todo: cek existing ...

                _context.Applicant.Remove(deleteObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete item success.";
                return RedirectToAction(nameof(ApplicantIndex));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(ApplicantDelete), new { id = Applicant.ApplicantId ?? "" });
            }
        }


        public IActionResult OnBoardingIndex()
        {
            var objs = _context.OnBoarding.OrderByDescending(x => x.CreatedAtUtc).ToList();
            return View(objs);
        }

        //display OnBoarding create edit form
        [HttpGet]
        public IActionResult OnBoardingForm(string id)
        {
            //create new
            if (id == null)
            {
                OnBoarding newObj = new OnBoarding();
                return View(newObj);
            }

            //edit OnBoarding
            OnBoarding obj = new OnBoarding();
            obj = _context.OnBoarding.Where(x => x.OnBoardingId.Equals(id)).FirstOrDefault();

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //post submitted OnBoarding data. if id is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitOnBoardingForm([Bind("OnBoardingId", "Name", "Description")]OnBoarding OnBoarding)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(OnBoardingForm), new { id = OnBoarding.OnBoardingId ?? "" });
                }

                //create new
                if (OnBoarding.OnBoardingId == null)
                {
                    if (await _context.OnBoarding.AnyAsync(x => x.Name.Equals(OnBoarding.Name)))
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + OnBoarding.Name + " already exist";
                        return RedirectToAction(nameof(OnBoardingForm), new { id = OnBoarding.OnBoardingId ?? "" });
                    }

                    OnBoarding newObj = new OnBoarding();
                    newObj.OnBoardingId = Guid.NewGuid().ToString();
                    newObj.Name = OnBoarding.Name;
                    newObj.Description = OnBoarding.Description;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.OnBoarding.Add(newObj);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new item success.";
                    return RedirectToAction(nameof(OnBoardingForm), new { id = newObj.OnBoardingId ?? "" });
                }

                //edit existing
                OnBoarding editObj = new OnBoarding();
                OnBoarding existObj = new OnBoarding();
                editObj = await _context.OnBoarding.Where(x => x.OnBoardingId.Equals(OnBoarding.OnBoardingId)).FirstOrDefaultAsync();
                existObj = await _context.OnBoarding.Where(x => x.Name.Equals(OnBoarding.Name)).FirstOrDefaultAsync();

                if (existObj != null)
                {
                    if (editObj.OnBoardingId != existObj.OnBoardingId)
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + OnBoarding.Name + " already exist";
                        return RedirectToAction(nameof(OnBoardingForm), new { id = OnBoarding.OnBoardingId ?? "" });
                    }

                }

                editObj.Name = OnBoarding.Name;
                editObj.Description = OnBoarding.Description;
                editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                editObj.UpdatedAtUtc = DateTime.UtcNow;

                _context.Update(editObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Edit existing item success.";
                return RedirectToAction(nameof(OnBoardingForm), new { id = OnBoarding.OnBoardingId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(OnBoardingForm), new { id = OnBoarding.OnBoardingId ?? "" });
            }
        }

        //display item for deletion
        [HttpGet]
        public async Task<IActionResult> OnBoardingDelete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = await _context.OnBoarding.Where(x => x.OnBoardingId.Equals(id)).FirstOrDefaultAsync();
            return View(obj);
        }

        //delete submitted item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitOnBoardingDelete([Bind("OnBoardingId")]OnBoarding OnBoarding)
        {
            try
            {
                var deleteObj = await _context.OnBoarding.Where(x => x.OnBoardingId.Equals(OnBoarding.OnBoardingId)).FirstOrDefaultAsync();
                if (deleteObj == null)
                {
                    return NotFound();
                }

                //todo: cek existing ke ...

                _context.OnBoarding.Remove(deleteObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete item success.";
                return RedirectToAction(nameof(OnBoardingIndex));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(OnBoardingDelete), new { id = OnBoarding.OnBoardingId ?? "" });
            }
        }


        public IActionResult ResignationIndex()
        {
            var objs = _context.Resignation.OrderByDescending(x => x.CreatedAtUtc).ToList();
            return View(objs);
        }

        //display Resignation create edit form
        [HttpGet]
        public IActionResult ResignationForm(string id)
        {
            //create new
            if (id == null)
            {
                Resignation newObj = new Resignation();
                return View(newObj);
            }

            //edit Resignation
            Resignation obj = new Resignation();
            obj = _context.Resignation.Where(x => x.ResignationId.Equals(id)).FirstOrDefault();

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //post submitted Resignation data. if id is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitResignationForm([Bind("ResignationId", "Name", "Description")]Resignation Resignation)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(ResignationForm), new { id = Resignation.ResignationId ?? "" });
                }

                //create new
                if (Resignation.ResignationId == null)
                {
                    if (await _context.Resignation.AnyAsync(x => x.Name.Equals(Resignation.Name)))
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + Resignation.Name + " already exist";
                        return RedirectToAction(nameof(ResignationForm), new { id = Resignation.ResignationId ?? "" });
                    }

                    Resignation newObj = new Resignation();
                    newObj.ResignationId = Guid.NewGuid().ToString();
                    newObj.Name = Resignation.Name;
                    newObj.Description = Resignation.Description;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.Resignation.Add(newObj);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new item success.";
                    return RedirectToAction(nameof(ResignationForm), new { id = newObj.ResignationId ?? "" });
                }

                //edit existing
                Resignation editObj = new Resignation();
                Resignation existObj = new Resignation();
                editObj = await _context.Resignation.Where(x => x.ResignationId.Equals(Resignation.ResignationId)).FirstOrDefaultAsync();
                existObj = await _context.Resignation.Where(x => x.Name.Equals(Resignation.Name)).FirstOrDefaultAsync();

                if (existObj != null)
                {
                    if (editObj.ResignationId != existObj.ResignationId)
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + Resignation.Name + " already exist";
                        return RedirectToAction(nameof(ResignationForm), new { id = Resignation.ResignationId ?? "" });
                    }

                }

                editObj.Name = Resignation.Name;
                editObj.Description = Resignation.Description;
                editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                editObj.UpdatedAtUtc = DateTime.UtcNow;

                _context.Update(editObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Edit existing item success.";
                return RedirectToAction(nameof(ResignationForm), new { id = Resignation.ResignationId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(ResignationForm), new { id = Resignation.ResignationId ?? "" });
            }
        }

        //display item for deletion
        [HttpGet]
        public async Task<IActionResult> ResignationDelete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = await _context.Resignation.Where(x => x.ResignationId.Equals(id)).FirstOrDefaultAsync();
            return View(obj);
        }

        //delete submitted item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitResignationDelete([Bind("ResignationId")]Resignation Resignation)
        {
            try
            {
                var deleteObj = await _context.Resignation.Where(x => x.ResignationId.Equals(Resignation.ResignationId)).FirstOrDefaultAsync();
                if (deleteObj == null)
                {
                    return NotFound();
                }

                //todo: cek existing ke ...

                _context.Resignation.Remove(deleteObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete item success.";
                return RedirectToAction(nameof(ResignationIndex));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(ResignationDelete), new { id = Resignation.ResignationId ?? "" });
            }
        }


        public IActionResult LayoffIndex()
        {
            var objs = _context.Layoff.OrderByDescending(x => x.CreatedAtUtc).ToList();
            return View(objs);
        }

        //display Layoff create edit form
        [HttpGet]
        public IActionResult LayoffForm(string id)
        {
            //create new
            if (id == null)
            {
                Layoff newObj = new Layoff();
                return View(newObj);
            }

            //edit Layoff
            Layoff obj = new Layoff();
            obj = _context.Layoff.Where(x => x.LayoffId.Equals(id)).FirstOrDefault();

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //post submitted Layoff data. if id is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitLayoffForm([Bind("LayoffId", "Name", "Description")]Layoff Layoff)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(LayoffForm), new { id = Layoff.LayoffId ?? "" });
                }

                //create new
                if (Layoff.LayoffId == null)
                {
                    if (await _context.Layoff.AnyAsync(x => x.Name.Equals(Layoff.Name)))
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + Layoff.Name + " already exist";
                        return RedirectToAction(nameof(LayoffForm), new { id = Layoff.LayoffId ?? "" });
                    }

                    Layoff newObj = new Layoff();
                    newObj.LayoffId = Guid.NewGuid().ToString();
                    newObj.Name = Layoff.Name;
                    newObj.Description = Layoff.Description;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.Layoff.Add(newObj);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new item success.";
                    return RedirectToAction(nameof(LayoffForm), new { id = newObj.LayoffId ?? "" });
                }

                //edit existing
                Layoff editObj = new Layoff();
                Layoff existObj = new Layoff();
                editObj = await _context.Layoff.Where(x => x.LayoffId.Equals(Layoff.LayoffId)).FirstOrDefaultAsync();
                existObj = await _context.Layoff.Where(x => x.Name.Equals(Layoff.Name)).FirstOrDefaultAsync();

                if (existObj != null)
                {
                    if (editObj.LayoffId != existObj.LayoffId)
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + Layoff.Name + " already exist";
                        return RedirectToAction(nameof(LayoffForm), new { id = Layoff.LayoffId ?? "" });
                    }

                }

                editObj.Name = Layoff.Name;
                editObj.Description = Layoff.Description;
                editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                editObj.UpdatedAtUtc = DateTime.UtcNow;

                _context.Update(editObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Edit existing item success.";
                return RedirectToAction(nameof(LayoffForm), new { id = Layoff.LayoffId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(LayoffForm), new { id = Layoff.LayoffId ?? "" });
            }
        }

        //display item for deletion
        [HttpGet]
        public async Task<IActionResult> LayoffDelete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = await _context.Layoff.Where(x => x.LayoffId.Equals(id)).FirstOrDefaultAsync();
            return View(obj);
        }

        //delete submitted item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitLayoffDelete([Bind("LayoffId")]Layoff Layoff)
        {
            try
            {
                var deleteObj = await _context.Layoff.Where(x => x.LayoffId.Equals(Layoff.LayoffId)).FirstOrDefaultAsync();
                if (deleteObj == null)
                {
                    return NotFound();
                }

                //todo: cek existing ke ...

                _context.Layoff.Remove(deleteObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete item success.";
                return RedirectToAction(nameof(LayoffIndex));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(LayoffDelete), new { id = Layoff.LayoffId ?? "" });
            }
        }

        public IActionResult ThirdPartyIndex()
        {
            var objs = _context.ThirdParty.OrderByDescending(x => x.CreatedAtUtc).ToList();
            return View(objs);
        }

        //display ThirdParty create edit form
        [HttpGet]
        public IActionResult ThirdPartyForm(string id)
        {
            //create new
            if (id == null)
            {
                ThirdParty newObj = new ThirdParty();
                return View(newObj);
            }

            //edit ThirdParty
            ThirdParty obj = new ThirdParty();
            obj = _context.ThirdParty.Where(x => x.ThirdPartyId.Equals(id)).FirstOrDefault();

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //post submitted ThirdParty data. if id is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitThirdPartyForm([Bind("ThirdPartyId", "Name", "Description")]ThirdParty ThirdParty)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(ThirdPartyForm), new { id = ThirdParty.ThirdPartyId ?? "" });
                }

                //create new
                if (ThirdParty.ThirdPartyId == null)
                {
                    if (await _context.ThirdParty.AnyAsync(x => x.Name.Equals(ThirdParty.Name)))
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + ThirdParty.Name + " already exist";
                        return RedirectToAction(nameof(ThirdPartyForm), new { id = ThirdParty.ThirdPartyId ?? "" });
                    }

                    ThirdParty newObj = new ThirdParty();
                    newObj.ThirdPartyId = Guid.NewGuid().ToString();
                    newObj.Name = ThirdParty.Name;
                    newObj.Description = ThirdParty.Description;
                    newObj.CreatedBy = await _userManager.GetUserAsync(User);
                    newObj.CreatedAtUtc = DateTime.UtcNow;
                    newObj.UpdatedBy = newObj.CreatedBy;
                    newObj.UpdatedAtUtc = newObj.CreatedAtUtc;

                    _context.ThirdParty.Add(newObj);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new item success.";
                    return RedirectToAction(nameof(ThirdPartyForm), new { id = newObj.ThirdPartyId ?? "" });
                }

                //edit existing
                ThirdParty editObj = new ThirdParty();
                ThirdParty existObj = new ThirdParty();
                editObj = await _context.ThirdParty.Where(x => x.ThirdPartyId.Equals(ThirdParty.ThirdPartyId)).FirstOrDefaultAsync();
                existObj = await _context.ThirdParty.Where(x => x.Name.Equals(ThirdParty.Name)).FirstOrDefaultAsync();

                if (existObj != null)
                {
                    if (editObj.ThirdPartyId != existObj.ThirdPartyId)
                    {
                        TempData[StaticString.StatusMessage] = "Error: " + ThirdParty.Name + " already exist";
                        return RedirectToAction(nameof(ThirdPartyForm), new { id = ThirdParty.ThirdPartyId ?? "" });
                    }

                }

                editObj.Name = ThirdParty.Name;
                editObj.Description = ThirdParty.Description;
                editObj.UpdatedBy = await _userManager.GetUserAsync(User);
                editObj.UpdatedAtUtc = DateTime.UtcNow;

                _context.Update(editObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Edit existing item success.";
                return RedirectToAction(nameof(ThirdPartyForm), new { id = ThirdParty.ThirdPartyId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(ThirdPartyForm), new { id = ThirdParty.ThirdPartyId ?? "" });
            }
        }

        //display item for deletion
        [HttpGet]
        public async Task<IActionResult> ThirdPartyDelete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = await _context.ThirdParty.Where(x => x.ThirdPartyId.Equals(id)).FirstOrDefaultAsync();
            return View(obj);
        }

        //delete submitted item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitThirdPartyDelete([Bind("ThirdPartyId")]ThirdParty ThirdParty)
        {
            try
            {
                var deleteObj = await _context.ThirdParty.Where(x => x.ThirdPartyId.Equals(ThirdParty.ThirdPartyId)).FirstOrDefaultAsync();
                if (deleteObj == null)
                {
                    return NotFound();
                }

                //todo: cek existing ke employee

                _context.ThirdParty.Remove(deleteObj);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete item success.";
                return RedirectToAction(nameof(ThirdPartyIndex));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(ThirdPartyDelete), new { id = ThirdParty.ThirdPartyId ?? "" });
            }
        }



    }
}