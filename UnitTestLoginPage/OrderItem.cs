using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreLIB
{
    public class OrderItem : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        #endregion
        private int quantity;
        private double subTotal;
        public string BookID { get; set; }
        public string BookTitle { get; set; }
        public double UnitPrice { get; set; }


        public int Quantity
        {
            get => quantity;
            set
            {
                if (quantity != value)
                {
                    quantity = value;
                    Notify(nameof(Quantity));
                    SubTotal = UnitPrice * quantity; // Update SubTotal whenever Quantity changes
                }
            }
        }

        public double SubTotal
        {
            get => subTotal;
            private set
            {
                if (subTotal != value)
                {
                    subTotal = value;
                    Notify(nameof(SubTotal));
                }
            }
        }
        public OrderItem(String isbn, String title,
            double unitPrice, int quantity)
        {
            BookID = isbn;
            BookTitle = title;
            UnitPrice = unitPrice;
            Quantity = quantity;
            SubTotal = UnitPrice * Quantity;
        }
        public override string ToString()
        {
            string xml = "<OrderItem ISBN='" + BookID + "'";
            xml += " Quantity='" + Quantity + "' />";
            return xml;
        }
    }
}
