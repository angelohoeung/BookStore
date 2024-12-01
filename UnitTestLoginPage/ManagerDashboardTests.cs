using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace BookStoreLIB {
    [TestClass]
    public class DashboardUnitTests {
        DashboardManager dash = new DashboardManager();
        List<BookSales> returnedList;
        BookSales returnedTopBook;
        List<Inventory> returnedInventory;
        [TestMethod]
        public void TestMethod1() {
            int inputUserId = -1;

            bool actualReturn = dash.Display(inputUserId);
            bool expectedReturn = false;
            Assert.AreEqual(expectedReturn, actualReturn);
        }
        [TestMethod]
        public void TestMethod2() {
            int inputUserId = 2;

            bool actualReturn = dash.Display(inputUserId);
            bool expectedReturn = false;
            Assert.AreEqual(expectedReturn, actualReturn);
        }
        [TestMethod]
        public void TestMethod3() {
            int inputUserId = 3;

            bool actualReturn = dash.Display(inputUserId);
            bool expectedReturn = true;
            returnedList = dash.BookInfo;
            Assert.AreEqual(expectedReturn, actualReturn);
            Assert.IsNotNull(returnedList);
        }
        [TestMethod]
        public void TestMethod4() {
            int inputUserId = 3;

            bool actualReturn = dash.Display(inputUserId);
            bool expectedReturn = true;
            returnedList = dash.BookInfo;
            returnedTopBook = dash.TopBook;
            Assert.AreEqual(expectedReturn, actualReturn);
            Assert.IsNotNull(returnedList);
            Assert.IsNotNull(returnedTopBook);
        }
        [TestMethod]
        public void TestMethod5() {
            int inputUserId = 3;

            bool actualReturn = dash.Display(inputUserId);
            bool expectedReturn = true;
            returnedList = dash.BookInfo;
            returnedTopBook = dash.TopBook;
            returnedInventory = dash.BookInventory;
            Assert.AreEqual(expectedReturn, actualReturn);
            Assert.IsNotNull(returnedList);
            Assert.IsNotNull(returnedTopBook);
            Assert.IsNotNull(returnedInventory);
        }
    }
}

