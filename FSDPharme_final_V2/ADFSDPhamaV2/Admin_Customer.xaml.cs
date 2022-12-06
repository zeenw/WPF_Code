using ADFSDPhamaV2.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
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
    /// Interaction logic for Admin_Customer.xaml
    /// </summary>
    public partial class Admin_Customer : Window
    {
        public Admin_Customer()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            Tbl_Title.Text = "Customer";
            BtnUpdate.IsEnabled = false;
            BtnDelete.IsEnabled = false;
            Tbx_id.Text = "";
            Tbx_name.Text = "";
            Tbx_email.Text = "";
            Tbx_phone.Text = "";
            Tbx_address.Text = "";

            PharmaConn pharmaConn = new PharmaConn();
            LvList.ItemsSource = pharmaConn.Customers.ToList();
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
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

        private void LvList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //BtnAdd.IsEnabled = false;
            Customer currSelected = LvList.SelectedItem as Customer;
            BtnUpdate.IsEnabled = (currSelected != null);
            BtnDelete.IsEnabled = (currSelected != null);
            if (currSelected == null)
            {
                init();
            }
            else
            {
                Tbx_id.Text = currSelected.id.ToString();
                Tbx_name.Text = currSelected.name;
                Tbx_email.Text = currSelected.email;
                Tbx_phone.Text = currSelected.phone;
                Tbx_address.Text = currSelected.address;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Customer customer = new Customer();

                customer.name = Tbx_name.Text;
                customer.email = Tbx_email.Text;
                customer.phone = Tbx_phone.Text;
                customer.address = Tbx_address.Text;

                if (Verify(customer))
                {
                    PharmaConn pharmaConn = new PharmaConn();
                    pharmaConn.Customers.Add(customer);
                    pharmaConn.SaveChanges();
                }
                else
                {
                    return;
                }

                init();
                LvList.SelectedItem = null;
                MessageBox.Show("New record created.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Item code already exist. You may choose the code to modify information.");
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Customer customer = new Customer();
                customer.id = int.Parse(Tbx_id.Text);
                customer.name = Tbx_name.Text;
                customer.email = Tbx_email.Text;
                customer.phone = Tbx_phone.Text;
                customer.address = Tbx_address.Text;

                if (Verify(customer))
                {
                    PharmaConn pharmaConn = new PharmaConn();
                    pharmaConn.Customers.AddOrUpdate(customer);
                    pharmaConn.SaveChanges();
                }
                else
                {
                    return;
                }

                init();
                LvList.SelectedItem = null;
                MessageBox.Show("Information updated.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            try 
            { 
                Customer currSelected = LvList.SelectedItem as Customer;
                Customer customer = new Customer { id = currSelected.id };
                PharmaConn pharmaConn = new PharmaConn();
                pharmaConn.Customers.Attach(customer);
                pharmaConn.Entry(customer).State = System.Data.Entity.EntityState.Deleted;
                pharmaConn.SaveChanges();
                init();
                LvList.SelectedItem = null;
                MessageBox.Show("Information delete.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Information you delete is in using.");
            }

        }


        private void BtnDash_Click(object sender, RoutedEventArgs e)
        {
            Admin window = new Admin();
            this.Visibility = Visibility.Hidden;
            window.Show();
        }

        private void BtnMedication_Click(object sender, RoutedEventArgs e)
        {
            Admin_Medication window = new Admin_Medication();
            this.Visibility = Visibility.Hidden;
            window.Show();
        }

        private void BtnSupplier_Click(object sender, RoutedEventArgs e)
        {
            Admin_Supplier window = new Admin_Supplier();
            this.Visibility = Visibility.Hidden;
            window.Show();
        }

        private void BtnPhoto_Click(object sender, RoutedEventArgs e)
        {
            Admin_Photo window = new Admin_Photo();
            this.Visibility = Visibility.Hidden;
            window.Show();
        }

        private void BtnStock_Click(object sender, RoutedEventArgs e)
        {
            Admin_Stock window = new Admin_Stock();
            this.Visibility = Visibility.Hidden;
            window.Show();
        }

        private void BtnCustomer_Click(object sender, RoutedEventArgs e)
        {
            Admin_Customer window = new Admin_Customer();
            this.Visibility = Visibility.Hidden;
            window.Show();
        }

        private void BtnUser_Click(object sender, RoutedEventArgs e)
        {
            Admin_User window = new Admin_User();
            this.Visibility = Visibility.Hidden;
            window.Show();
        }


        private bool Verify(Customer customer)
        {
            bool rs = true;

            return rs;
        }


    }
}
