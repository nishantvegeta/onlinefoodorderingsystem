using System;
using System.ComponentModel.DataAnnotations;

namespace onlinefood.Dto.OrderDtos;

public class UpdateOrderStatusDto
{
    [Required]
    public int OrderId { get; set; }

    [Required]
    public string Status { get; set; }
}
