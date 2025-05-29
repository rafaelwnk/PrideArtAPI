namespace PrideArtAPI.Exceptions;

public class EmailNotFoundException : Exception
{
    public EmailNotFoundException() : base("E-mail não encontrado.") { }
}