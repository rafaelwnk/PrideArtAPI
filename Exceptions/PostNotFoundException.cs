namespace PrideArtAPI.Exceptions;

public class PostNotFoundException : Exception
{
    public PostNotFoundException() : base("Post não encontrado.") { }
}