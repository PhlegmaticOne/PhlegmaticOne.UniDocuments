namespace PhlegmaticOne.OperationResults.Mediatr;

public abstract class IdentityOperationResultQuery<T> : IOperationResultQuery<T>
{
    protected IdentityOperationResultQuery(Guid profileId)
    {
        ProfileId = profileId;
    }

    public Guid ProfileId { get; }
}