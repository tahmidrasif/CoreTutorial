using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DLL.Model
{
    public class CustomerBalance
    {
        public int CustomerBalanceId { get; set; }
        public decimal Balance { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
