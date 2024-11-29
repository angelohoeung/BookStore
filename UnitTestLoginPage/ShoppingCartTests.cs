using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BookStoreLIB
{
    [TestClass]
    public class ShoppingCartTests
    {
        BookOrder bookOrder = new BookOrder();

        [TestMethod]
        public void Addbook()
        {
            OrderItem item = new OrderItem("1234567890", "Test Book", 10.99, 2);
            bookOrder.AddItem(item);

            Assert.AreEqual(1, bookOrder.OrderItemList.Count);
            Assert.AreEqual(21.98, bookOrder.GetOrderTotal());
            Assert.AreEqual("1234567890", item.BookID);
            Assert.AreEqual("Test Book", item.BookTitle);
        }

        [TestMethod]
        public void RemoveBook()
        {
            OrderItem item = new OrderItem("1234567890", "Test Book", 15.99, 1);
            bookOrder.AddItem(item);


            bookOrder.RemoveItem("1234567890");

            Assert.AreEqual(0, bookOrder.OrderItemList.Count);
            Assert.AreEqual(0.0, bookOrder.GetOrderTotal());
        }

        [TestMethod]
        public void EditQuantity()
        {
            OrderItem item = new OrderItem("1234567890", "Test Book", 10.99, 1);
            bookOrder.AddItem(item);
            bookOrder.SetQuantity(item, 3);

            Assert.AreEqual(3, bookOrder.OrderItemList[0].Quantity);
            Assert.AreEqual(32.97, bookOrder.GetOrderTotal());
        }

        [TestMethod]
        public void SumPriceTotal()
        {
            OrderItem item = new OrderItem("1234567890", "Test Book", 10.99, 1);
            OrderItem item2 = new OrderItem("123123123", "Dictionary", 7.05, 1);

            bookOrder.AddItem(item);
            bookOrder.AddItem(item2);

            Assert.AreEqual(18.04, bookOrder.GetOrderTotal());
        }

        [TestMethod]
        public void SumPriceTotalWithMultiples()
        {
            OrderItem item = new OrderItem("1234567890", "Test Book", 10.99, 2);
            OrderItem item2 = new OrderItem("123123123", "Dictionary", 7.05, 3);

            bookOrder.AddItem(item);
            bookOrder.AddItem(item2);

            Assert.AreEqual(43.13, bookOrder.GetOrderTotal(), 0.001);
        }

        [TestMethod]
        public void EditQuantityToZero()
        {
            OrderItem item = new OrderItem("1234567890", "Test Book", 10.99, 1);
            bookOrder.AddItem(item);
            bookOrder.SetQuantity(item, 0);

            Assert.AreEqual(0, bookOrder.OrderItemList[0].Quantity);
            Assert.AreEqual(0.00, bookOrder.GetOrderTotal());
        }

        [TestMethod]
        public void QuantityZero()
        {
            OrderItem item = new OrderItem("1234567890", "Test Book", 10.99, 0);
            bookOrder.AddItem(item);

            Assert.AreEqual(0, bookOrder.OrderItemList[0].Quantity);
            Assert.AreEqual(0.00, bookOrder.GetOrderTotal());
        }

        [TestMethod]
        public void EditQuantityToNegative()
        {
            OrderItem item = new OrderItem("1234567890", "Test Book", 10.99, 1);
            bookOrder.AddItem(item);
            bookOrder.SetQuantity(item, -1);

            Assert.AreEqual(-1, bookOrder.OrderItemList[0].Quantity);
            Assert.AreEqual(-10.99, bookOrder.GetOrderTotal());
        }

        [TestMethod]
        public void QuantityNegative()
        {
            OrderItem item = new OrderItem("1234567890", "Test Book", 10.99, -1);
            bookOrder.AddItem(item);

            Assert.AreEqual(-1, bookOrder.OrderItemList[0].Quantity);
            Assert.AreEqual(-10.99, bookOrder.GetOrderTotal());
        }

        [TestMethod]
        public void SumQuantityTotal()
        {
            OrderItem item = new OrderItem("1234567890", "Test Book", 10.99, 1);
            OrderItem item2 = new OrderItem("1234567890", "Test Book", 10.99, 1);

            bookOrder.AddItem(item);
            bookOrder.AddItem(item2);

            Assert.AreEqual(2, bookOrder.OrderItemList[0].Quantity);
        }

        [TestMethod]
        public void QuantityTotalAfterRemoval()
        {
            OrderItem item = new OrderItem("1234567890", "Test Book", 10.99, 1);
            OrderItem item2 = new OrderItem("1234567890", "Test Book", 10.99, 2);

            bookOrder.AddItem(item);
            bookOrder.AddItem(item2);

            bookOrder.RemoveItem("1234567890");

            Assert.AreEqual(0, bookOrder.OrderItemList.Count);
            Assert.AreEqual(0.0, bookOrder.GetOrderTotal());
        }

        // Below are additional tests for the persistent shopping cart feature
        [TestMethod]
        public void PersistentShoppingCartAddRemoveItem()
        {
            OrderItem item = new OrderItem("1234345", "Some Title", 200.50, 2);
            DALShoppingCart cart = new DALShoppingCart();
            Assert.IsTrue(cart.AddCartItem(19, item));
            Assert.IsTrue(cart.GetCartItems(19).Count > 0);
            Assert.IsTrue(cart.RemoveCartItem(19, item));
        }

        [TestMethod]
        public void PersistentShoppingCartEditItem()
        {
            DALShoppingCart cart = new DALShoppingCart();
            cart.AddCartItem(19, new OrderItem("1234345", "Some Title", 200.50, 2));

            var items = cart.GetCartItems(19);

            Assert.IsTrue(items.Count > 0);
            items[0].Quantity = 20; // Edit quantity
            cart.EditCartItem(19, items[0]);

            //Check that updated quantity is saved
            items = cart.GetCartItems(19);
            Assert.IsTrue(items[0].Quantity == 20);

            // Clean test item from the DB
            cart.RemoveCartItem(19, items[0]);
        }

    }
}
