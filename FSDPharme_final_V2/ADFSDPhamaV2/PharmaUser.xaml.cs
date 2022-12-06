using ADFSDPhamaV2.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ADFSDPhamaV2
{
    /// <summary>
    /// Interaction logic for PharmaUser.xaml
    /// </summary>
    public partial class PharmaUser : Window
    {
        Usr currentUsr = new Usr();
        ArrayList itemList = new ArrayList();
        public PharmaUser(Usr usr)
        {
            InitializeComponent();
            TblUser.Text = usr.email;
            currentUsr = usr;
        }    

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PharmaConn pharmaConn = new PharmaConn();
            Cbo_Customer.ItemsSource = pharmaConn.Customers.ToList();
            Cbo_Customer.DisplayMemberPath = "name";
            Cbo_Customer.SelectedValuePath = "id";
            Cbo_Customer.SelectedIndex = -1;

            Cbo_Medication.ItemsSource = pharmaConn.Medications.ToList();
            Cbo_Medication.DisplayMemberPath = "name";
            Cbo_Medication.SelectedValuePath = "id";
            Cbo_Medication.SelectedIndex = -1;
        }

        private void init()
        {
            Cbo_Medication.SelectedIndex = -1;
            Tbx_price.Text = "";
            Tbx_quantity.Text = "";

            Cbo_Customer.SelectedIndex = -1;
            Tbx_email.Text = "";
            Tbx_address.Text = "";
            Tbx_phone.Text = "";
            Tbx_name.Text = "";
            Tbx_id.Text = "";
            itemList.Clear();
            LvList.ItemsSource = null;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.Visibility = Visibility.Visible;
            this.Close();
        }

        private void Cbo_Customer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(Cbo_Customer.SelectedIndex != -1)
            {
                PharmaConn pharmaConn = new PharmaConn();
                int id = int.Parse(Cbo_Customer.SelectedValue.ToString());
                Customer customer = pharmaConn.Customers.Find(id);

                Tbx_id.Text = customer.id.ToString();
                Tbx_name.Text = customer.name.ToString();
                Tbx_phone.Text = customer.phone.ToString();
                Tbx_email.Text = customer.email.ToString();
                Tbx_address.Text = customer.address.ToString();
            }

        }

        private void BtnDeleteTask_Click(object sender, RoutedEventArgs e)
        {
            Order_details currSelected = LvList.SelectedItem as Order_details;
            itemList.Remove(currSelected);
            LvList.ItemsSource = null;
            LvList.ItemsSource = itemList;
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(itemList.Count == 0)
                {
                    MessageBox.Show("Not add any medication yet.");
                    return;
                }
                Customer customer = new Customer();
                PharmaConn pharmaConn = new PharmaConn();
                Order order = new Order();

                if (Cbo_Customer.Text == "")
                {
                    customer.phone = Tbx_phone.Text;
                    customer.email = Tbx_email.Text;
                    customer.name = Tbx_name.Text;
                    customer.address = Tbx_address.Text;
                    if (Verify(customer))
                    {
                        pharmaConn.Customers.Add(customer);
                        pharmaConn.SaveChanges();
                        order.customer_id = customer.id;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    order.customer_id = (int)Cbo_Customer.SelectedValue;
                }

                order.user_id = currentUsr.id;
                order.date = DateTime.Now;
                pharmaConn.Orders.Add(order);
                pharmaConn.SaveChanges();

                foreach (Order_details item in itemList)
                {
                    item.Medication1 = null;
                    item.order = order.id;
                    pharmaConn.Order_details.Add(item);
                    pharmaConn.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Check your information input.");
            }
            init();
            MessageBox.Show("Information saved.");
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Order_details item = new Order_details();
                if (Cbo_Medication.SelectedIndex == -1)
                {
                    MessageBox.Show("Choose medication please.");
                    return;
                }
                else
                {
                    item.medication = (int)Cbo_Medication.SelectedValue;
                    Medication medication = new Medication();
                    medication.name = Cbo_Medication.Text;
                    item.Medication1 = medication;
                }
                
                if (Tbx_price.Text != "" && float.TryParse(Tbx_price.Text, out float price))
                {
                    item.price = price;
                }
                else
                {
                    MessageBox.Show("Check price please.");
                    return;
                }

                if (Tbx_quantity.Text != "" && int.TryParse(Tbx_quantity.Text, out int quantity))
                {
                    item.quantity = quantity;
                }
                else
                {
                    MessageBox.Show("Check quantity number please.");
                    return;
                }

                if (Verify(item))
                {
                    foreach(Order_details elem in itemList)
                    {
                        if (elem.medication == item.medication)
                        {
                            MessageBox.Show("You choose a same medication.");
                            return;
                        }
                    }
                    itemList.Add(item);
                    LvList.ItemsSource = null;
                    LvList.ItemsSource = itemList;
                }
                else
                {
                    return;
                }

                LvList.SelectedItem = null;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Medication code already exist. You may choose the code to modify quantity.");
            }
        }

        private bool Verify(Order_details order_details)
        {
            bool rs = true;
            if (order_details.medication == null)
            {
                MessageBox.Show("Medication can not be empty");
                rs = false;
            }
            if (order_details.price == null)
            {
                MessageBox.Show("Price can not be empty");
                rs = false;
            }
            if (order_details.quantity == null)
            {
                MessageBox.Show("Quantity can not be empty");
                rs = false;
            }
            return rs;
        }

        private bool Verify(Customer customer)
        {
            if (customer.email == "")
            {
                MessageBox.Show("Email can not be empty");
                return false;
            }

            if (customer.phone == "")
            {
                MessageBox.Show("Phone number can not be empty");
                return false;
            }

            if (customer.name == "")
            {
                MessageBox.Show("Name can not be empty");
                return false;
            }

            if (customer.address == "")
            {
                MessageBox.Show("Address can not be empty");
                return false;
            }

            return true;
        }
    }
}
