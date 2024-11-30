using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSysClass
{
    public class Account
    {
        public int Id { get; set; } // Уникален идентификатор за акаунта

        public string Name { get; set; } // Име на акаунта

        public ICollection<Transaction> Transactions { get; set; } // Навигационно свойство за транзакциите към акаунта
    }
}
