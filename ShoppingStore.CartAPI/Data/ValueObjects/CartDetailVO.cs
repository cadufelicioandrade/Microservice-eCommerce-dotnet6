﻿using ShoppingStore.CartAPI.Model;
using ShoppingStore.CartAPI.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingStore.CartAPI.Data.ValueObjects
{
    public class CartDetailVO
    {
        public long Id { get; set; }

        public long CartHeaderId { get; set; }
        public CartHeaderVO? CartHeader { get; set; }

        public long ProductId { get; set; }
        public ProductVO? Product { get; set; }

        public int Count { get; set; }

    }
}
