﻿using OrderManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResult?> CreateOrUpdatePaymentIntent(int basketId);

    }
}
