namespace comunicacion_anuncios.Application.Abstractions;

public interface IPasswordHasher
{
    string Hash(string plain);
}