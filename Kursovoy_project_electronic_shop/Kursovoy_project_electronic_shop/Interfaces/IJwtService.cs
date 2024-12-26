namespace Kursovoy_project_electronic_shop.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(Guid userUid, string login, bool IsAdmin);
    }
}
