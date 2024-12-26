namespace Kursovoy_project_electronic_shop.Contracts
{
    public class Product
    {
        public required Guid ProductUid { get; init; }

        public required string Name { get; init; }

        public required int Price { get; init; }

        public required List<string> Manufacturers { get; init; }

        public required List<string> Types { get; init; }
    }
}
