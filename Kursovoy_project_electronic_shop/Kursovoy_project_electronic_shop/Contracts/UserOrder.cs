namespace Kursovoy_project_electronic_shop.Contracts
{
    public class UserOrder
    {
        public required Guid OrderUid { get; init; }

        public required string ProductName { get; init; }

        public required int ProductPrice { get; init; }

        public required List<string> ProductType { get; init; }

        public required List<string> ProductManufacturer { get; init; }

        public required string ProductImage { get; init; }
    }
}
