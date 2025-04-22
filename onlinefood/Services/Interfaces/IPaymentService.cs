using System;
using onlinefood.Dto.PaymentDtos;

namespace onlinefood.Services.Interfaces;

public interface IPaymentService
{
    Task<bool> ProcessPayment(CreatePaymentDto dto);
    Task<List<PaymentDto>> GetAllPayments(); // For admin
}
