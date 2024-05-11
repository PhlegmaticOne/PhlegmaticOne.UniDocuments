using PhlegmaticOne.PagedLists.Implementation;

namespace UniDocuments.App.Shared.Activities.Display;

public class ActivityDisplayList
{
    public PagedList<ActivityDisplayObject> Activities { get; set; } = null!;
}