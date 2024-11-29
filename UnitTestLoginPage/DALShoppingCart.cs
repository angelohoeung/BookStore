using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreLIB
{
    //Handles operations for the persistent shopping cart feature
    //Functions return bools for testing purposes
    public class DALShoppingCart
    {
        public bool EditCartItem(int userId, OrderItem item)
        {
            if (userId < 1)
                throw new ArgumentException($"Invalid ID passed in DALShoppingCart.EditCartItem({userId})");

            string query = @"
            UPDATE ShoppingCart
            SET Quantity = @NewQuantity
            WHERE UserID = @UserID AND ISBN = @ISBN;";

            try
            {
                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.Connection))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userId);
                        command.Parameters.AddWithValue("@ISBN", item.BookID);
                        command.Parameters.AddWithValue("@NewQuantity", item.Quantity);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected < 1)
                            return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }

            return true;
        }
        public bool RemoveCartItem(int userId, OrderItem item)
        {
            if (userId < 1)
                throw new ArgumentException($"Invalid ID passed in DALShoppingCart.RemoveCartItem({userId})");

            string query = @"
            DELETE FROM ShoppingCart
            WHERE UserID = @UserID AND ISBN = @ISBN;";

            try
            {
                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.Connection))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userId);
                        command.Parameters.AddWithValue("@ISBN", item.BookID);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected < 1)
                            return false;
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }

            return true;
        }
        public bool AddCartItem(int userId, OrderItem item)
        {
            if (userId < 1)
                throw new ArgumentException($"Invalid ID passed in DALShoppingCart.AddCartItem({userId})");

            string query = @"
                            INSERT INTO ShoppingCart (UserID, ISBN, Quantity, DateAdded)
                            VALUES (@UserID, @ISBN, @Quantity, GETDATE());";

            try
            {
                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.Connection))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userId);
                        command.Parameters.AddWithValue("@ISBN", item.BookID);
                        command.Parameters.AddWithValue("@Quantity", item.Quantity);

                        int rowsAffected = command.ExecuteNonQuery();

                        // Success?!?!?!?!
                        if (rowsAffected < 1)
                        {
                            return false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

            return true;
        }
        //Get all cart items for a specified userID
        public List<OrderItem> GetCartItems(int userId)
        {
            if (userId < 1)
                throw new ArgumentException($"Invalid ID passed in DALShoppingCart.GetCartItems({userId})");

            List<OrderItem> orderItems = new List<OrderItem>();
            
            string query = @"
                            SELECT 
                                SC.Quantity, 
                                SC.ISBN AS BookID, 
                                BD.Title AS BookTitle, 
                                BD.Price AS UnitPrice
                            FROM 
                                ShoppingCart SC
                            INNER JOIN 
                                BookData BD ON SC.ISBN = BD.ISBN
                            WHERE 
                                SC.UserID = @LoggedInUserID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.Connection))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LoggedInUserID", userId);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //Create our order items
                                int quantity = reader.GetInt32(reader.GetOrdinal("Quantity"));
                                string bookId = reader.GetString(reader.GetOrdinal("BookID"));
                                string bookTitle = reader.GetString(reader.GetOrdinal("BookTitle"));
                                decimal unitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice"));

                                //Add OrderItem to list
                                orderItems.Add(new OrderItem(bookId, bookTitle, (double) unitPrice, quantity));

                                //Console.WriteLine($"BookID: {bookId}, Title: {bookTitle}, Quantity: {quantity}, Unit Price: {unitPrice:C}");
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return orderItems;
        }
        
    }
}
