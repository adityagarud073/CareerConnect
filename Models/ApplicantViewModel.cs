namespace SmartRecruitment.Models;

public class ApplicantViewModel
{
    public string JobTitle { get; set; }="";

    public string CandidateName { get; set; }="";

    public DateTime AppliedDate { get; set; }=DateTime.Now;

    public string ResumePath { get; set; }="";

    public string MobileNumber { get; set; }="";
}