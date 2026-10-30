namespace SmartRecruitment.Models;

public class Application
{
    public int Id { get; set; }

    public int CandidateId { get; set; }

    public int JobId { get; set; }

    public DateTime AppliedDate { get; set; }
}