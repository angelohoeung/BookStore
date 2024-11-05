using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Data;

namespace BookStoreLIB
{
    [TestClass]
    public class TestSearch
    {
        private SearchBooks searchBooks = new SearchBooks();
        private String query;
        private String searchType;
        [TestMethod]
        public void TestMethod1()
        {
            query = "";
            searchType = "Title";
            DataTable result = searchBooks.Search(query, searchType);
            int expectedLen = 22;
            Assert.AreEqual(expectedLen, result.Rows.Count);
        }
        [TestMethod]
        public void TestMethod2()
        {
            query = "Extreme";
            searchType = "Title";
            DataTable result = searchBooks.Search(query, searchType);
            DataTable expectedResult = new DataTable();
            bool areEqual = true;
            expectedResult.Columns.Add("Title", typeof(String));
            expectedResult.Columns.Add("Author", typeof(String));
            expectedResult.Columns.Add("Price", typeof(String));
            expectedResult.Columns.Add("Year", typeof(String));
            expectedResult.Rows.Add("Extreme Programming Explained: Embrace Change", "Kent Beck and Cynthia Andres", "44.63", "2004");
            if (result.Rows.Count == expectedResult.Rows.Count && result.Columns.Count == expectedResult.Columns.Count)
            {
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    for (int j = 0; j < result.Columns.Count; j++)
                    {
                        if (!result.Rows[i][j].Equals(expectedResult.Rows[i][j]))
                        {
                            areEqual = false;
                            break;
                        }
                    }
                }
            }
            Assert.IsTrue(areEqual, "The actual DataTable does not match the expected DataTable.");
        }
        [TestMethod]
        public void TestMethod3()
        {
            query = "Not in database";
            searchType = "Title";
            DataTable result = searchBooks.Search(query, searchType);
            Assert.AreEqual(0, result.DefaultView.Count);
        }
        [TestMethod]
        public void TestMethod4()
        { 
            query = "Mark";
            searchType = "Author";
            DataTable result = searchBooks.Search(query, searchType);
            DataTable expectedResult = new DataTable();
            bool areEqual = true;
            expectedResult.Columns.Add("Title", typeof(String));
            expectedResult.Columns.Add("Author", typeof(String));
            expectedResult.Columns.Add("Price", typeof(String));
            expectedResult.Columns.Add("Year", typeof(String));
            expectedResult.Rows.Add("Agile Project Management for Dummies", "“Mark C. Layton", "26.99", "2012");
            if (result.Rows.Count == expectedResult.Rows.Count && result.Columns.Count == expectedResult.Columns.Count)
            {
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    for (int j = 0; j < result.Columns.Count; j++)
                    {
                        if (!result.Rows[i][j].Equals(expectedResult.Rows[i][j]))
                        {
                            areEqual = false;
                            break;
                        }
                    }
                }
            }
            Assert.IsTrue(areEqual, "The actual DataTable does not match the expected DataTable.");
        }
        [TestMethod]
        public void TestMethod5()
        {
            query = "Not in Database";
            searchType = "Author";
            DataTable result = searchBooks.Search(query, searchType);
            Assert.AreEqual(0, result.DefaultView.Count);
        }
    }
}
