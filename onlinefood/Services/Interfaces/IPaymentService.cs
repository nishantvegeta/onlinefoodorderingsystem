using System;
using onlinefood.Dto.PaymentDtos;
using onlinefood.ViewModels.PaymentVms;

namespace onlinefood.Services.Interfaces;

public interface IPaymentService
{
    Task<bool> ProcessPayment(CreatePaymentDto dto);
    Task<List<PaymentVm>> GetAllPayments(); // For admin
    Task<PaymentVm> GetPaymentById(int id); // For admin
}
