﻿using DiscountMS.Contracts.Enums;

namespace DiscountMS.Contracts
{
    public abstract class DiscountDTO
    {
        public long DiscountID { get; set; }
        public DiscountTypeEnum DiscountType { get; protected set; }
        public DiscountAmountTypeEnum DiscountAmountType { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public DiscountTerminationTypeEnum TerminationType { get; set; }
    }
}
