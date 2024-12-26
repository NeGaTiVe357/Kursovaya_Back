using DatabaseAccessLayer;
using Kursovoy_project_electronic_shop.Interfaces;
using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;
using DatabaseAccessLayer.Entities;

namespace Kursovoy_project_electronic_shop.Services
{
    public class TypeService : ITypeService
    {
        private readonly ElectronicShopDbContext _electronicShopDbContext;

        public TypeService(ElectronicShopDbContext electronicShopDbContext)
        {
            _electronicShopDbContext = electronicShopDbContext;
        }

        public bool CreateType(string name)
        {
            var type = new DatabaseAccessLayer.Entities.Type
            {
                TypeUid = Guid.NewGuid(),
                Name = name
            };

            _electronicShopDbContext.Add(type);

            return _electronicShopDbContext.SaveChanges() > 0;
        }

        public List<Contracts.Type>? GetTypes()
        {
            var types = _electronicShopDbContext.Set<DatabaseAccessLayer.Entities.Type>().ToList();

            if (types.Count == 0) { return null; }

            return types.Select(type => new Contracts.Type
            {
                TypeUid = type.TypeUid,
                Name = type.Name
            }).ToList();
        }

        public bool UpdateType(Guid typeUid, string name)
        {
            var type = _electronicShopDbContext.Set<DatabaseAccessLayer.Entities.Type>().SingleOrDefault(x => x.TypeUid == typeUid);

            if (type == null) { return false; }

            type.Name = name;

            return _electronicShopDbContext.SaveChanges() > 0;
        }

        public bool DeleteType(Guid typeUid)
        {
            var type = _electronicShopDbContext.Set<DatabaseAccessLayer.Entities.Type>().SingleOrDefault(x => x.TypeUid == typeUid);

            if (type == null) { return false; }

            _electronicShopDbContext.Remove(type);

            return _electronicShopDbContext.SaveChanges() > 0;
        }

        public bool CheckTypeName(string name)
        {
            var type = _electronicShopDbContext.Set<DatabaseAccessLayer.Entities.Type>().SingleOrDefault(x => x.Name == name);

            if (type == null) { return false; }

            return true;
        }

        public bool IsTypeExists(Guid typeUid)
        {
            var type = _electronicShopDbContext.Set<DatabaseAccessLayer.Entities.Type>().SingleOrDefault(x => x.TypeUid == typeUid);

            if (type == null) { return false; }

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
