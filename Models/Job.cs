namespace SmartRecruitment.Models;

public class Job
{
    public int Id { get; set; }

    public string CompanyName { get; set; } = "";

    public int RecruiterId { get; set; }

    public string Title { get; set; } = "";

    public string Description { get; set; } = "";

    public string RequiredSkills { get; set; } = "";

    public string Experience { get; set; } = "";

    public string Location { get; set; } = "";
}