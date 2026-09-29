namespace FictionalEnterprise.Shared.CustomExceptions;

public class ValueAlreadyInDatabaseException : Exception
{
    public ValueAlreadyInDatabaseException()
        : base("The value is already in the database")
    {
    }
}