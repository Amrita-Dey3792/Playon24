namespace Playon24.BusinessLayer.Exceptions
{
    public class InvalidUserInputException : Exception
    {

        public InvalidUserInputException() { }

        public InvalidUserInputException(string message) : base(message) { }

    }
}
