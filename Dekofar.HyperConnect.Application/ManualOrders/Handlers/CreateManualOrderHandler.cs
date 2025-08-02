using Dekofar.HyperConnect.Application.Common.Interfaces;
using Dekofar.HyperConnect.Application.ManualOrders.Commands;
using Dekofar.HyperConnect.Domain.Entities;
using MediatR;
using System;
using System.Linq;

namespace Dekofar.HyperConnect.Application.ManualOrders.Handlers
{
    public class CreateManualOrderHandler : IRequestHandler<CreateManualOrderCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public CreateManualOrderHandler(IApplicationDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<Guid> Handle(CreateManualOrderCommand request, CancellationToken cancellationToken)
        {
            if (_currentUser.UserId == null)
                throw new UnauthorizedAccessException();

            var order = new ManualOrder
            {
                CustomerName = request.CustomerName,
                CustomerSurname = request.CustomerSurname,
                Phone = request.Phone,
                Email = request.Email,
                Address = request.Address,
                City = request.City,
                District = request.District,
                PaymentType = request.PaymentType,
                OrderNote = request.OrderNote,
                CreatedByUserId = _currentUser.UserId.Value,
                CreatedAt = DateTime.UtcNow,
                Status = ManualOrderStatus.Pending,
                DiscountName = request.DiscountName,
                DiscountType = request.DiscountType,
                DiscountValue = request.DiscountValue
            };

            foreach (var item in request.Items)
            {
                order.Items.Add(new ManualOrderItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Total = item.Price * item.Quantity
                });
            }

            var subTotal = order.Items.Sum(i => i.Total);
            decimal discount = 0m;
            if (!string.IsNullOrEmpty(request.DiscountType))
            {
                if (request.DiscountType.Equals("Percentage", StringComparison.OrdinalIgnoreCase))
                {
                    discount = subTotal * request.DiscountValue / 100m;
                }
                else
                {
                    discount = request.DiscountValue;
                }
            }

            order.TotalAmount = subTotal - discount;
            order.BonusAmount = order.TotalAmount * 0.1m; // simple commission calculation

            await _context.ManualOrders.AddAsync(order, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return order.Id;
        }
    }
}

