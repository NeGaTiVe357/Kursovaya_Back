using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Kursovoy_project_electronic_shop.Interfaces;
using System.Text.RegularExpressions;

namespace Kursovoy_project_electronic_shop.Services
{
    public class ManufacturerService : IManufacturerService
    {
        private readonly ElectronicShopDbContext _electronicShopDbContext;

        public ManufacturerService(ElectronicShopDbContext electronicShopDbContext)
        {
            _electronicShopDbContext = electronicShopDbContext;
        }

        public bool CreateManufacturer(string name)
        {
            var manufacturer = new Manufacturer
            {
                ManufacturerUid = Guid.NewGuid(),
                Name = name
            };

            _electronicShopDbContext.Add(manufacturer);

            return _electronicShopDbContext.SaveChanges() > 0;
        }

        public List<Contracts.Manufacturer>? GetManufacturers()
        {
            var manufacturers = _electronicShopDbContext.Set<Manufacturer>().ToList();

            if (manufacturers.Count == 0) { return null; }

            return manufacturers.Select(manufacturer => new Contracts.Manufacturer
            {
                ManufacturerUid = manufacturer.ManufacturerUid,
                Name = manufacturer.Name
            }).ToList();
        }

        public bool UpdateManufacturer(Guid manufacturerUid, string name)
        {
            var manufacturer = _electronicShopDbContext.Set<Manufacturer>().SingleOrDefault(x => x.ManufacturerUid == manufacturerUid);

            if (manufacturer == null) { return false; }

            manufacturer.Name = name;

            return _electronicShopDbContext.SaveChanges() > 0;
        }

        public bool DeleteManufacturer(Guid manufacturerUid)
        {
            var manufacturer = _electronicShopDbContext.Set<Manufacturer>().SingleOrDefault(x => x.ManufacturerUid == manufacturerUid);

            if (manufacturer == null) { return false; }

            _electronicShopDbContext.Remove(manufacturer);

            return _electronicShopDbContext.SaveChanges() > 0;
        }

        public bool CheckManufacturerName(string name)
        {
            var manufacturer = _electronicShopDbContext.Set<Manufacturer>().SingleOrDefault(x => x.Name == name);

            if (manufacturer == null) { return false; }

            return true;
        }

        public bool IsManufacturerExists(Guid manufacturerUid)
        {
            var manufacturer = _electronicShopDbContext.Set<Manufacturer>().SingleOrDefault(x => x.ManufacturerUid == manufacturerUid);

            if (manufacturer == null) { return false; }

            return true;
        }

        public bool CheckRegex(string name)
        {
            var regex = new Regex(@"^[a-zA-Zа-яА-Я][a-zA-Zа-яА-Я -]{1,}$");

            if (!regex.IsMatch(name))
            {
                return false;
            }

            return true;
        }

    }
}
