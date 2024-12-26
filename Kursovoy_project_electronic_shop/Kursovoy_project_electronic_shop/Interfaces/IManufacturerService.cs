namespace Kursovoy_project_electronic_shop.Interfaces
{
    public interface IManufacturerService
    {
        bool CreateManufacturer(string name);
        List<Contracts.Manufacturer>? GetManufacturers();
        bool UpdateManufacturer(Guid manufacturerUid, string name);
        bool DeleteManufacturer(Guid manufacturerUid);
        bool CheckManufacturerName(string name);
        bool IsManufacturerExists(Guid manufacturerUid);
        bool CheckRegex(string name);
    }
}
