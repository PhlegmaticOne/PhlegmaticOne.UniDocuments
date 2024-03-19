namespace PhlegmaticOne.OperationResults.Mediatr;

public abstract class IdentityOperationResultQuery<T> : IOperationResultQuery<T>, IIdentity
{
    protected IdentityOperationResultQuery(Guid profileId)
    {
        ProfileId = profileId;
    }

    public Guid ProfileId { get; }
}