namespace Kursovoy_project_electronic_shop.Interfaces
{
    public interface IProductService
    {
        bool CreateProduct(Contracts.ProductInfo productInfo);
        List<Contracts.Product>? GetAllProducts();
        List<Contracts.ProductInfo>? GetProductsInfo();
        bool UpdateProduct(Guid productUid, Contracts.ProductInfo productInfo);
        bool DeleteProduct(Guid productUid);
        public Contracts.Product? GetSingleProduct(string productName);
        bool IsProductExists(Guid productUid);
        bool CheckProductName(string productName);
        bool CheckProductInfo(Contracts.ProductInfo productInfo);
        bool CheckProductInfo(Guid productUid, Contracts.ProductInfo productInfo);
        bool CheckRegex(string name);
        bool CheckRegexList(List<string> list);
    }
}
