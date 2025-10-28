namespace comunicacion_anuncios.Application.Abstractions;

public interface IPasswordVerifier
{
    bool Verify(string plain, string hash);
}