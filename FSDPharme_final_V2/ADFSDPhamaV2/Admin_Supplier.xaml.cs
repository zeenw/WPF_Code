using ADFSDPhamaV2.Model;
using System;
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
    /// Interaction logic for Admin_Supplier.xaml
    /// </summary>
    public partial class Admin_Supplier : Window
    {
        public Admin_Supplier()
        {
            InitializeComponent();
            init();
        }


        private void init()
        {
            Tbl_Title.Text = "Supplier";
            BtnUpdate.IsEnabled = false;
            BtnDelete.IsEnabled = false;
            Tbx_id.Text = "";
            Tbx_name.Text = "";
            Tbx_email.Text = "";
            Tbx_phone.Text = "";
            Tbx_address.Text = "";

            PharmaConn pharmaConn = new PharmaConn();
            LvList.ItemsSource = pharmaConn.Supliers.ToList();
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
            Suplier currSelected = LvList.SelectedItem as Suplier;
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
                Suplier suplier = new Suplier();

                suplier.name = Tbx_name.Text;
                suplier.email = Tbx_email.Text;
                suplier.phone = Tbx_phone.Text;
                suplier.address = Tbx_address.Text;

                if (Verify(suplier))
                {
                    PharmaConn pharmaConn = new PharmaConn();
                    pharmaConn.Supliers.Add(suplier);
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
                Suplier suplier = new Suplier();
                suplier.id = int.Parse(Tbx_id.Text);
                suplier.name = Tbx_name.Text;
                suplier.email = Tbx_email.Text;
                suplier.phone = Tbx_phone.Text;
                suplier.address = Tbx_address.Text;

                if (Verify(suplier))
                {
                    PharmaConn pharmaConn = new PharmaConn();
                    pharmaConn.Supliers.AddOrUpdate(suplier);
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
                Suplier currSelected = LvList.SelectedItem as Suplier;
                Suplier suplier = new Suplier { id = currSelected.id };
                PharmaConn pharmaConn = new PharmaConn();
                pharmaConn.Supliers.Attach(suplier);
                pharmaConn.Entry(suplier).State = System.Data.Entity.EntityState.Deleted;
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


        private bool Verify(Suplier suplier)
        {
            bool rs = true;

            return rs;
        }



    }
}
