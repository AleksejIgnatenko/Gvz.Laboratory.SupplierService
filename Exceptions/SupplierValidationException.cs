namespace Gvz.Laboratory.SupplierService.Exceptions
{
    public class SupplierValidationException : Exception
    {
        public Dictionary<string, string> Errors { get; set; }

        public SupplierValidationException(Dictionary<string, string> errors)
        {
            Errors = errors;
        }
    }
}
