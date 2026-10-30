using Microsoft.AspNetCore.Mvc;
using SmartRecruitment.Data;
using SmartRecruitment.Models;

namespace SmartRecruitment.Controllers;

public class CandidateController : Controller
{
    private readonly ApplicationDbContext _context;

    private bool IsCandidateLoggedIn()
    {
        return HttpContext.Session.GetString("Role") == "Candidate";
    }

    public CandidateController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Dashboard()
    {
        if (HttpContext.Session.GetString("Role") != "Candidate")
        {
            return RedirectToAction("Login", "Account");
        }


        return View();
    }

    [HttpGet]
    public IActionResult UploadResume()
    {
        if (HttpContext.Session.GetString("Role") != "Candidate")
        {
            return RedirectToAction("Login", "Account");
        }

        int candidateId = HttpContext.Session.GetInt32("UserId") ?? 0;

        var existingResume = _context.Resumes.FirstOrDefault(x => x.CandidateId == candidateId);

        return View(existingResume);// Pass existing resume to the view if it exists
    }

    [HttpPost]
    public IActionResult UploadResume(IFormFile resumeFile, string mobileNumber)
    {
        if (HttpContext.Session.GetString("Role") != "Candidate")
        {
            return RedirectToAction("Login", "Account");
        }

        int candidateId = HttpContext.Session.GetInt32("UserId") ?? 0;

        if (resumeFile == null)
        {
            TempData["Error"] = "Please select a resume file.";

            return RedirectToAction("UploadResume");
        }
       //path.combine is used to combine the current directory with the "wwwroot" folder and the "Resumes" subfolder. This ensures that the resume files are stored in a specific location within the web application.
        string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Resumes");// Path to the Resumes folder in wwwroot

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string fileName = Guid.NewGuid() + Path.GetExtension(resumeFile.FileName);

        string filePath = Path.Combine(folderPath, fileName);

        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
            resumeFile.CopyTo(stream);
        }

        var existingResume = _context.Resumes.FirstOrDefault(x => x.CandidateId == candidateId);

        // Resume already exists then Update
        if (existingResume != null)
        {
            existingResume.ResumePath = "/Resumes/" + fileName;

            existingResume.MobileNumber = mobileNumber;

            _context.Resumes.Update(existingResume);

            _context.SaveChanges();

            TempData["Success"] = "Resume Updated Successfully.";

            return RedirectToAction("UploadResume");
        }

        // First time upload

        Resume resume = new Resume();

        resume.CandidateId = candidateId;

        resume.ResumePath = "/Resumes/" + fileName;

        resume.MobileNumber = mobileNumber;

        _context.Resumes.Add(resume);

        _context.SaveChanges();

        TempData["Success"] = "Resume Uploaded Successfully.";

        return RedirectToAction("UploadResume");
    }



    public IActionResult AppliedJobs()
    {

        if (HttpContext.Session.GetString("Role") != "Candidate")
        {
            return RedirectToAction("Login", "Account");
        }
        int candidateId = HttpContext.Session.GetInt32("UserId") ?? 0;

        var jobs = from a in _context.Applications
                   join j in _context.Jobs
                   on a.JobId equals j.Id
                   where a.CandidateId == candidateId
                   select new
                   {
                       JobId = j.Id,
                       Title = j.Title,
                       Location = j.Location,
                       AppliedDate = a.AppliedDate
                   };

        return View(jobs.ToList());
    }
}