using System;
using onlinefood.Dto.PaymentDtos;
using onlinefood.Services.Interfaces;
using System.Collections.Generic;
using onlinefood.Data;
using onlinefood.Entity;

namespace onlinefood.Services;

public class PaymentService : IPaymentService
{
    private readonly FirstRunDbContext dbContext;

    public PaymentService(FirstRunDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public Task<bool> ProcessPayment(CreatePaymentDto dto)
    {
        // payment processing logic here
        throw new NotImplementedException();
    }

    public Task<List<PaymentDto>> GetAllPayments()
    {
        // logic to retrieve all payments for admin here
        throw new NotImplementedException();
    }
}
