using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notification_service.Domain.Entities
{
    public class MailTemplate
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Value { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdateAt { get; set; }
        public bool isActive { get; set; }
        public bool isDeleted { get; set; }
    }
}
