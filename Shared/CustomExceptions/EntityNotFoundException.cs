namespace FictionalEnterprise.Shared.CustomExceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException() 
        : base("Entity could not be found and returned null")

    {
    }
}