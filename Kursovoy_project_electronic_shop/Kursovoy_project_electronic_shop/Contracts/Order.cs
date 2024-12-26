namespace Kursovoy_project_electronic_shop.Contracts
{
    public class Order
    {
        public required Guid OrderUid { get; init; }

        public required string UserLogin { get; init; }

        public required string ProductName { get; init; }

        public required int ProductPrice { get; init; }

        public required List<string> ProductType { get; init; }

        public required List<string> ProductManufacturer { get; init; }
    }
}
