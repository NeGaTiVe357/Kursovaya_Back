using Kursovoy_project_electronic_shop.Contracts;
using Kursovoy_project_electronic_shop.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;

namespace Kursovoy_project_electronic_shop.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        private readonly IUserService _userService;

        private readonly IProductService _productService;

        public OrderController(IOrderService orderService, IUserService userService, IProductService productService)
        {
            _orderService = orderService;

            _userService = userService;

            _productService = productService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public ActionResult CreateOrder(Guid userUid, Guid productUid)
        {
            if (!_userService.IsUserExists(userUid))
            {
                ModelState.AddModelError("", "User not found");

                return BadRequest(ModelState);
            }

            if (!_productService.IsProductExists(productUid))
            {
                ModelState.AddModelError("", "Product not found");

                return BadRequest(ModelState);
            }

            if (!_orderService.CreateOrder(userUid, productUid))
            {
                ModelState.AddModelError("", "Failed to create order");

                return BadRequest(ModelState);
            }

            return Ok("Order created");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<Order>> GetAllOrders()
        {
            var orders = _orderService.GetAllOrders();

            if (orders == null)
            {
                return NotFound("No orders found");
            }

            return Ok(orders);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public ActionResult<List<UserOrder>> GetUserOrders(Guid userUid)
        {
            if (!_userService.IsUserExists(userUid))
            {
                return NotFound("User not found");
            }

            var orders = _orderService.GetUserOrders(userUid);

            if (orders == null)
            {
                return NotFound("No orders found");
            }

            return Ok(orders);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public ActionResult<List<UserOrder>> GetPurchasedUserOrders(Guid userUid)
        {
            if (!_userService.IsUserExists(userUid))
            {
                return NotFound("User not found");
            }

            var orders = _orderService.GetPurchasedUserOrders(userUid);

            if (orders == null)
            {
                return NotFound("No purchased orders found");
            }

            return Ok(orders);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<Order>> GetProductOrders(Guid productUid)
        {
            if (!_productService.IsProductExists(productUid))
            {
                return NotFound("Screning not found");
            }

            var orders = _orderService.GetProductOrders(productUid);

            if (orders == null)
            {
                return NotFound("No orders found");
            }

            return Ok(orders);
        }

        [HttpPut]
        [Authorize(Roles = "Admin, User")]
        public ActionResult UpdateOrderStatus(Guid orderUid)
        {
            if (!_orderService.UpdateOrderStatus(orderUid))
            {
                ModelState.AddModelError("", "Failed to update order status");

                return BadRequest(ModelState);
            }

            return Ok("Order status updated");
        }

        [HttpDelete]
        [Authorize(Roles = "Admin, User")]
        public ActionResult DeleteOrder(Guid orderUid)
        {
            if (!_orderService.DeleteOrder(orderUid))
            {
                ModelState.AddModelError("", "Failed to delete order");

                return BadRequest(ModelState);
            }

            return Ok("Order deleted");
        }

    }
}
