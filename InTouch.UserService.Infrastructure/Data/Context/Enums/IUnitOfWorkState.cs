namespace InTouch.Infrastructure.Data;

public enum IUnitOfWorkState
{
    Open,
    Committed,
    RolledBack
}