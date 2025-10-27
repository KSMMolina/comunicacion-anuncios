namespace comunicacion_anuncios.Application.Abstractions;

public interface ICurrentUser
{
    Guid GetUserId();
    string GetUserEmail();
    string GetUserRole();
    string GetUserName();
}