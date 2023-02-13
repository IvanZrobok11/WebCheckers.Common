namespace Common.Contract.RequestsAndResponses
{
    public abstract class BaseResponse
    {
        public bool IsException { get; set; }
        public string ExceptionMessage { get; set; }
    }
}
