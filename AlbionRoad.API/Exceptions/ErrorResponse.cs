namespace AlbionRoad.API.Exceptions;

public class ErrorResponse
{
    public string ErrorMessage { get; set; } = null!;
}

public class ErrorResponseWithException : ErrorResponse
{
    public string Exception { get; set; }
    public int Status { get; set; }

    public ErrorResponseWithException(string message, int status, string exception = null!)
    {
        ErrorMessage = message;
        Status = status;
        Exception = exception;
    }
}

public class ErrorDictionaryResponse
{
    public Dictionary<string, object> Errors { get; set; }

    public ErrorDictionaryResponse(string exceptionMessage)
    {
        Errors = new Dictionary<string, object>();
        Errors.Add("error", exceptionMessage);
    }

    public void AddErrors(Dictionary<string, object> errors)
    {
        Errors.Add("errors", errors);
    }
}
