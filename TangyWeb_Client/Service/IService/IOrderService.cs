using Tangy_Models;

namespace TangyWeb_Client.Service.IService;

public interface IOrderService
{
    public Task<IEnumerable<OrderDTO>> GetAll(string? userId);
    public Task<OrderDTO> Get(int productId);

    public Task<OrderDTO> Create(StripePaymentDTO paymentDTO);
}
