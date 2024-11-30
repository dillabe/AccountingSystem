using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSysClass
{
    public class Transaction
    {
        public int Id { get; set; } // Уникален идентификатор за транзакцията

        public int AccountId { get; set; } // Връзка към акаунт
        public Account Account { get; set; } // Свързаният акаунт (навигационно свойство)

        public decimal Amount { get; set; } // Сума на транзакцията

        public DateTime Date { get; set; } // Дата на транзакцията

        public string Description { get; set; } // Описание на транзакцията
    }
}
