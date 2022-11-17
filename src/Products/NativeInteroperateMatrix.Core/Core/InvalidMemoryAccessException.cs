using System.Runtime.Serialization;

namespace Nima;

[Serializable()]
public class InvalidMemoryAccessException : Exception
{
    public InvalidMemoryAccessException()
        : base()
    { }

    public InvalidMemoryAccessException(string message)
        : base(message)
    { }

    public InvalidMemoryAccessException(string message, Exception innerException)
        : base(message, innerException)
    { }

    protected InvalidMemoryAccessException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    { }
}
