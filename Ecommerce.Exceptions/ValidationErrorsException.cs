using Ecommerce.Exceptions.ExceptionsBase;

namespace Ecommerce.Exceptions
{
    public class ValidationErrorsException : EcommerceException
    {
        public List<string> ErrorMessages { get; set; }

        public ValidationErrorsException(List<string> errorMessages) : base(string.Empty)
        {
            ErrorMessages = errorMessages;
        }
    }
}
