using System;

namespace onlinefood.ViewModels.ContactVms
{
    public class ContactVm
    {
        public int ContactId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public DateTime SubmittedAt { get; set; }
        public bool IsResolved { get; set; }

        // Optionally show user info if needed
        public int UserId { get; set; }
        public string CustomerName { get; set; } = string.Empty; // From related Users table
    }
}
