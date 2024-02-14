﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Application;
using InventoryManagement.Application.Contracts.Inventory;
using ShopManagement.Domain.OrderAgg;
using ShopManagement.Domain.Services;

namespace ShopManagement.Infrastructure.InventoryAcl
{
    public class ShopInventoryAcl : IShopInventoryAcl
    {
        
        private readonly IInventoryApplication _inventoryApplication;

        public ShopInventoryAcl(IInventoryApplication inventoryApplication)
        {
            _inventoryApplication = inventoryApplication;
        }

        public bool ReduceFromInventory(List<OrderItem> items)
        {
            var command = items.Select(orderItem =>
                    new DecreaseInventory(orderItem.ProductId, orderItem.Count, "خرید مشتری", orderItem.OrderId))
                .ToList();

            return _inventoryApplication.Reduce(command).IsSucceeded;
            
        }
    }
    
}