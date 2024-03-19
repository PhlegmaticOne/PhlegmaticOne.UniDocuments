namespace PhlegmaticOne.OperationResults.Mediatr;

public abstract class IdentityOperationResultCommand : IOperationResultCommand, IIdentity
{
    protected IdentityOperationResultCommand(Guid profileId)
    {
        ProfileId = profileId;
    }

    public Guid ProfileId { get; }
}