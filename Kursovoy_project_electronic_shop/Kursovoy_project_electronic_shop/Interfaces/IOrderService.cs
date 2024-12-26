namespace Kursovoy_project_electronic_shop.Interfaces
{
    public interface IOrderService
    {
        bool CreateOrder(Guid userUid, Guid productUid);
        List<Contracts.Order>? GetAllOrders();
        List<Contracts.UserOrder>? GetUserOrders(Guid userUid);
        public List<Contracts.UserOrder>? GetPurchasedUserOrders(Guid userUid);
        List<Contracts.Order>? GetProductOrders(Guid productUid);
        public bool UpdateOrderStatus(Guid orderUid);
        bool DeleteOrder(Guid orderUid);
        bool IsOrderExists(Guid orderUid);
    }
}
