namespace Kursovoy_project_electronic_shop.Interfaces
{
    public interface ITypeService
    {
        bool CreateType(string name);
        List<Contracts.Type>? GetTypes();
        bool UpdateType(Guid typeUid, string name);
        bool DeleteType(Guid typeUid);
        bool CheckTypeName(string name);
        bool IsTypeExists(Guid typeUid);
        bool CheckRegex(string name);
    }
}
