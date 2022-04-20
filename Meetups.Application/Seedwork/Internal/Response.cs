namespace Meetups.Application.Seedwork.Internal;

public class Response<TPayload, TErrorType>
    where TErrorType : System.Enum
{
    public bool Success { get; }
    
    public TPayload Payload { get; }
    
    public TErrorType ErrorType { get; }

    public Response(TPayload payload)
    {
        Success = true;
        Payload = payload;
        ErrorType = default;
    }

    public Response(TErrorType errorType)
    {
        Success = false;
        Payload = default;
        ErrorType = errorType;
    }
}