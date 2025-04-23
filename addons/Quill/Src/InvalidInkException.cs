#nullable enable

namespace Quill;

public class InvalidInkException : System.Exception
{
    public InvalidInkException()
    {
    }

    public InvalidInkException(string message) : base(message)
    {
    }
}
