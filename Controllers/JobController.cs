using Microsoft.AspNetCore.Mvc;
using SmartRecruitment.Data;
using SmartRecruitment.Models;


namespace SmartRecruitment.Controllers;

public class JobController : Controller
{
    private readonly ApplicationDbContext _context;

    public JobController(ApplicationDbContext context)
    {
        _context = context;
    }


    [HttpGet]
    public IActionResult AddJob()
    {
        if (HttpContext.Session.GetString("Role") != "Recruiter")
        {
            return RedirectToAction("Login", "Account");
        }

        return View();
    }



    [HttpPost]
    public IActionResult AddJob(Job job)
    {
        if (HttpContext.Session.GetString("Role") != "Recruiter")
        {
            return RedirectToAction("Login", "Account");
        }

        job.RecruiterId = HttpContext.Session.GetInt32("UserId") ?? 0;//like Get the recruiter ID from the session and assign it to the job's RecruiterId property

        _context.Jobs.Add(job);

        _context.SaveChanges();

        TempData["JobSuccess"] = "Job Added Successfully";

        return RedirectToAction("ManageJobs");
    }

    public IActionResult ViewJobs(string location)
    {
        if (HttpContext.Session.GetString("UserName") == null)
        {
            return RedirectToAction("Login", "Account");
        }

        // AsQueryable() mule query lagach execute hot nahi.
        // Pudhe filter, search kiwa sorting lavaychi asel tar query build karta yete.
        // ToList() call kelyavarach database var query execute hote.
        // Yamule fakt required data database madhun fetch hoto ani performance improve hote.

        var jobs = _context.Jobs.AsQueryable();// Get all jobs as a queryable collection

        if (!string.IsNullOrWhiteSpace(location))
        {
            jobs = jobs.Where(j => j.Location.ToLower().Contains(location.ToLower()));
        }

        return View(jobs.ToList());
    }


    public IActionResult ApplyJob(int id)
    {
        if (HttpContext.Session.GetString("Role") != "Candidate")
        {
            return RedirectToAction("Login", "Account");
        }

        int candidateId = HttpContext.Session.GetInt32("UserId") ?? 0;

        // Check Resume Uploaded Or Not

        var resume = _context.Resumes.FirstOrDefault(x => x.CandidateId == candidateId);

        if (resume == null)
        {
            TempData["Message"] = "Please Upload Resume First";

            return RedirectToAction("UploadResume", "Candidate");
        }

        // Check Already Applied Or Not

        bool alreadyApplied = _context.Applications.Any(x => x.CandidateId == candidateId && x.JobId == id);

        if (alreadyApplied)
        {
            TempData["Message"] = "You Already Applied For This Job";

            return RedirectToAction("ViewJobs");
        }

        Application application = new Application();

        application.CandidateId = candidateId;

        application.JobId = id;

        application.AppliedDate = DateTime.Now;

        _context.Applications.Add(application);

        _context.SaveChanges();

        TempData["Message"] = "Job Applied Successfully";

        return RedirectToAction("ViewJobs");
    }

    public IActionResult JobDetails(int id)
    {

        if (HttpContext.Session.GetString("UserName") == null)
        {
            return RedirectToAction("Login", "Account");
        }
        var job = _context.Jobs.Find(id);

        return View(job);
    }


    public IActionResult ManageJobs()
    {
        if (HttpContext.Session.GetString("Role") != "Recruiter")
        {
            return RedirectToAction("Login", "Account");
        }
        int recruiterId = HttpContext.Session.GetInt32("UserId") ?? 0;

        var jobs = _context.Jobs.Where(j => j.RecruiterId == recruiterId).ToList();// like Get all jobs posted by the logged-in recruiter from the database and convert it to a list

        return View(jobs);
    }

    public IActionResult ViewApplicants()
    {
        if (HttpContext.Session.GetString("Role") != "Recruiter")
        {
            return RedirectToAction("Login", "Account");
        }
        int recruiterId = HttpContext.Session.GetInt32("UserId") ?? 0;// Get the recruiter ID from the session

        var applicants = (from a in _context.Applications
                          join j in _context.Jobs
                              on a.JobId equals j.Id

                          join u in _context.Users
                              on a.CandidateId equals u.Id

                          join r in _context.Resumes
                              on u.Id equals r.CandidateId

                          where j.RecruiterId == recruiterId

                          select new ApplicantViewModel
                          {
                              JobTitle = j.Title,
                              CandidateName = u.Name,
                              AppliedDate = a.AppliedDate,
                              ResumePath = r.ResumePath,
                              MobileNumber = r.MobileNumber
                          })
             .ToList();

        return View(applicants);
    }

    [HttpGet]
    public IActionResult EditJob(int id)
    {
        if (HttpContext.Session.GetString("Role") != "Recruiter")
        {
            return RedirectToAction("Login", "Account");
        }
        var job = _context.Jobs.Find(id);

        return View(job);
    }

    [HttpPost]
    public IActionResult EditJob(Job job)
    {
        if (HttpContext.Session.GetString("Role") != "Recruiter")
        {
            return RedirectToAction("Login", "Account");
        }
        _context.Jobs.Update(job);// Hite apan database madhil job record update karat aahot. Job object madhe updated values astil, tyala update method madhe pass kelyavar database madhil corresponding record update hote.

        _context.SaveChanges();

        return RedirectToAction("ManageJobs");
    }
    public IActionResult DeleteJob(int id)
    {
        if (HttpContext.Session.GetString("Role") != "Recruiter")
        {
            return RedirectToAction("Login", "Account");
        }
        var job = _context.Jobs.Find(id);

        if (job != null)
        {
            _context.Jobs.Remove(job);

            _context.SaveChanges();
        }

        return RedirectToAction("ManageJobs");
    }


}