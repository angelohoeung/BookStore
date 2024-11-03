using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BookStoreLIB
{
    [TestClass]
    public class UnitTest2
    {
        BookOrder bookOrder = new BookOrder();


        [TestMethod]
        public void TestMethod1()
        {
            OrderItem item = new OrderItem("1234567890", "Test Book", 10.99, 2);
            bookOrder.AddItem(item);

            Assert.AreEqual(1, bookOrder.OrderItemList.Count);
            Assert.AreEqual(21.98, bookOrder.GetOrderTotal());
            Assert.AreEqual("1234567890", item.BookID);
            Assert.AreEqual("Test Book", item.BookTitle);

        }

        [TestMethod]
        public void TestMethod2()
        {

            //OrderItem item = new OrderItem("1234567890", "Test Book", 10.99, 0);

            //bookOrder.AddItem(item);

            //Assert.AreEqual(0, bookOrder.OrderItemList.Count);
        }

        [TestMethod]
        public void TestMethod3()
        {
            OrderItem item = new OrderItem("1234567890", "Test Book", 15.99, 1);
            bookOrder.AddItem(item);


            bookOrder.RemoveItem("1234567890");

            Assert.AreEqual(0, bookOrder.OrderItemList.Count);
            Assert.AreEqual(0.0, bookOrder.GetOrderTotal());

        }

        [TestMethod]
        public void TestMethod4()
        {
            OrderItem item = new OrderItem("1234567890", "Test Book", 10.99, 1);
            bookOrder.AddItem(item);
            bookOrder.SetQuantity(item, 3);

            Assert.AreEqual(3, bookOrder.OrderItemList[0].Quantity);
            Assert.AreEqual(32.97, bookOrder.GetOrderTotal());


        }

        [TestMethod]
        public void TestMethod5()
        {
            //OrderItem item = new OrderItem("1234567890", "Test Book", 15.99, 1);
            //bookOrder.AddItem(item);

            //bookOrder.SetQuantity(item, -1);

            //Assert.AreEqual(0, bookOrder.OrderItemList.Count);


        }

        [TestMethod]
        public void TestMethod6()
        {
            OrderItem item = new OrderItem("1234567890", "Test Book", 10.99, 1);
            OrderItem item2 = new OrderItem("123123123", "Dictionary", 7.05, 1);

            bookOrder.AddItem(item);
            bookOrder.AddItem(item2);

            Assert.AreEqual(18.04, bookOrder.GetOrderTotal());

        }


        [TestMethod]
        public void TestMethod7()
        {
            


        }

        [TestMethod]
        public void TestMethod8()
        {
           


        }

        
    }
}
