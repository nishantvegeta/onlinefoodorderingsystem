using System;
using onlinefood.Dto.PaymentDtos;
using onlinefood.Services.Interfaces;
using System.Collections.Generic;
using onlinefood.Data;
using onlinefood.Entity;
using onlinefood.Enums;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using onlinefood.ViewModels.PaymentVms;

namespace onlinefood.Services;

public class PaymentService : IPaymentService
{
    private readonly FirstRunDbContext dbContext;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public PaymentService(FirstRunDbContext dbContext, IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
        this.dbContext = dbContext;
    }

    public async Task<bool> ProcessPayment(CreatePaymentDto dto)
    {
        if (dto.PaymentMethod == PaymentMethod.Khalti)
        {
            var isPaymentVerified = await VerifyKhaltiPayment(dto.PaymentToken, dto.Amount);
            if (!isPaymentVerified)
            {
                return false; // Payment failed
            }
        }
        else if (dto.PaymentMethod == PaymentMethod.CashOnDelivery)
        {

        }

        var payment = new Payments();
        payment.OrderId = dto.OrderId;
        payment.PaymentMethod = dto.PaymentMethod;
        payment.Amount = dto.Amount;
        payment.PaidAt = DateTime.UtcNow;
        payment.PaymentToken = dto.PaymentToken;
        payment.Status = "Success";

        dbContext.Payments.Add(payment);

        var order = await dbContext.Orders.FindAsync(dto.OrderId);
        if (order != null)
        {
            order.Status = "Paid";
            dbContext.Orders.Update(order);
        }

        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<PaymentVm>> GetAllPayments()
    {
        var payments = await dbContext.Payments
            .Include(p => p.Order)
            .ToListAsync();

        return payments.Select(p => new PaymentVm
        {
            PaymentId = p.PaymentId,
            OrderId = p.OrderId,
            OrderStatus = p.Order.Status,
            PaymentMethod = p.PaymentMethod,
            PaymentToken = p.PaymentToken,
            Amount = p.Amount,
            Status = p.Status,
            PaidAt = p.PaidAt
        }).ToList();
    }
    
    public async Task<PaymentVm> GetPaymentById(int id)
    {
        var payment = await dbContext.Payments
            .Include(p => p.Order)
            .FirstOrDefaultAsync(p => p.PaymentId == id);

        if (payment == null)
        {
            throw new Exception("Payment not found");
        }

        return new PaymentVm
        {
            PaymentId = payment.PaymentId,
            OrderId = payment.OrderId,
            OrderStatus = payment.Order.Status,
            PaymentMethod = payment.PaymentMethod,
            PaymentToken = payment.PaymentToken,
            Amount = payment.Amount,
            Status = payment.Status,
            PaidAt = payment.PaidAt
        };
    }


    private async Task<bool> VerifyKhaltiPayment(string token, decimal amount)
    {
        var verifyUrl = _configuration["Khalti:VerifyUrl"];
        var secretKey = _configuration["Khalti:SecretKey"];

        var payload = new Dictionary<string, string>
            {
                { "token", token },
                { "amount", ((int)(amount * 100)).ToString() } // Khalti expects amount in paisa
            };

        var request = new HttpRequestMessage(HttpMethod.Post, verifyUrl)
        {
            Content = new FormUrlEncodedContent(payload)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Key", secretKey);

        var response = await _httpClient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            // You can parse the JSON response to check status
            return true;
        }
        else
        {
            return false;
        }
    }
}
