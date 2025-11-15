namespace backend_disc.Exceptions
{
    public class DatabaseException : Exception
    {

        public DatabaseException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
