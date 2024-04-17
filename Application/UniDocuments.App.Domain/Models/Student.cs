using UniDocuments.App.Domain.Models.Base;

namespace UniDocuments.App.Domain.Models;

public class Student : EntityBase
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public Guid GroupId { get; set; }
    public Group Group { get; set; } = null!;
    public IList<StudyDocument> Documents { get; set; } = null!;
}
