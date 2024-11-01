namespace InTouch.Infrastructure.Data;

public enum IDbContextState
{
    Closed,
    Open,
    Committed,
    RolledBack 
}