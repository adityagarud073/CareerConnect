namespace SmartRecruitment.Models;

public class Resume
{
    public int Id { get; set; }

    public int CandidateId { get; set; }

    public string ResumePath { get; set; } = "";

    public string MobileNumber { get; set; } = "";
}