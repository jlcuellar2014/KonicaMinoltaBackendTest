﻿using FoodDeliveryAPI.Model;
using FoodDeliveryAPI.Services;
using FoodDeliveryTests.Model;
using FoodDeliveryTests.Publishers;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryTests.Services
{

    [TestFixture]
    public class DeliveryVehiclesService_CreateDeliveryVehicleOrderTests
    {
        [Test]
        public async Task CreateDeliveryVehicleOrderAsync_ValidIds_UpdatesOrder()
        {
            // Arrange
            using var dbContext = new FakeDbContext();
            var deliveryService = new DeliveryVehiclesService(dbContext, new FakePublisher());
            var order = new Order { OrderId = 1 };

            dbContext.Orders.Add(order);
            await dbContext.SaveChangesAsync();

            // Act
            deliveryService.CreateDeliveryVehicleOrder(1, 1);

            // Assert
            var updatedOrder = await dbContext.Orders.FindAsync(1);

            Assert.IsNotNull(updatedOrder);
            Assert.That(updatedOrder.DeliveryVehicleId, Is.EqualTo(1));
        }

        [Test]
        public void CreateDeliveryVehicleOrderAsync_InvalidOrderId_ThrowsException()
        {
            // Arrange
            using var dbContext = new FakeDbContext();
            var deliveryService = new DeliveryVehiclesService(dbContext, new FakePublisher());

            // Act & Assert
            Assert.Throws<ArgumentException>(() => deliveryService.CreateDeliveryVehicleOrder(1, 1));

        }

        [Test]
        public async Task CreateDeliveryVehicleOrderAsync_OrderIdNotFound_ThrowsException()
        {
            // Arrange
            using var dbContext = new FakeDbContext();
            var deliveryService = new DeliveryVehiclesService(dbContext, new FakePublisher());
            var order = new Order { OrderId = 1 };

            dbContext.Orders.Add(order);
            await dbContext.SaveChangesAsync();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => deliveryService.CreateDeliveryVehicleOrder(1, 2));
        }

        [Test]
        public void CreateDeliveryVehicleOrder_OrderAlreadyAssigned_ThrowsInvalidOperationException()
        {
            // Arrange
            using var dbContext = new FakeDbContext();
            var deliveryService = new DeliveryVehiclesService(dbContext, new FakePublisher());
            var order = new Order { OrderId = 1, DeliveryVehicleId = 2 };
           
            dbContext.Orders.Add(order);
            dbContext.SaveChanges();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => deliveryService.CreateDeliveryVehicleOrder(1, 1));
        }
    }
}
