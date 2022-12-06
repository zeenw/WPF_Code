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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Admin_Stock : Window
    {
        public Admin_Stock()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            Tbl_Title.Text = "Stock";
            BtnUpdate.IsEnabled = false;
            BtnDelete.IsEnabled = false;
            Tbx_id.Text = "";
            Tbx_quantity.Text = "";
            Combo_Medication.SelectedIndex = 0;
            
            PharmaConn pharmaConn = new PharmaConn();
            ArrayList list = new ArrayList();
            foreach(Medication med in pharmaConn.Medications.ToList())
            {
                list.Add(med.id);
            }

            Combo_Medication.ItemsSource = list;
            LvList.ItemsSource = pharmaConn.Stocks.ToList();
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
            BtnAdd.IsEnabled = false;
            Stock currSelected = LvList.SelectedItem as Stock;
            BtnUpdate.IsEnabled = (currSelected != null);
            BtnDelete.IsEnabled = (currSelected != null);
            if (currSelected == null)
            {
                init();
            }
            else
            {
                Tbx_id.Text = currSelected.med.ToString();
                Tbx_quantity.Text = currSelected.quantity.ToString();

            }
        }

        private void Combo_Medication_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnAdd.IsEnabled = true;
            BtnUpdate.IsEnabled = false;
            BtnDelete.IsEnabled = false;
            Tbx_id.Text = "";
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Stock stock = new Stock();
                stock.med = int.Parse(Combo_Medication.SelectedItem.ToString());

                if (int.TryParse(Tbx_quantity.Text, out int rs))
                {
                    stock.quantity = rs;
                }

                if (Verify(stock))
                {
                    PharmaConn pharmaConn = new PharmaConn();
                    pharmaConn.Stocks.Add(stock);
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
            catch(Exception ex)
            {
                MessageBox.Show("Item code already exist. You may choose the code to modify information.");
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Stock stock = new Stock();
                stock.med = int.Parse(Tbx_id.Text);
                if (int.TryParse(Tbx_quantity.Text, out int rs))
                {
                    stock.quantity = rs;
                }

                if (Verify(stock))
                {
                    PharmaConn pharmaConn = new PharmaConn();
                    pharmaConn.Stocks.AddOrUpdate(stock);
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
            Stock currSelected = LvList.SelectedItem as Stock;
            Stock stock = new Stock { med = currSelected.med };
            PharmaConn pharmaConn = new PharmaConn();
            pharmaConn.Stocks.Attach(stock);
            pharmaConn.Entry(stock).State = System.Data.Entity.EntityState.Deleted;
            pharmaConn.SaveChanges();
            init();
            LvList.SelectedItem = null;
            MessageBox.Show("Information delete.");
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

        private bool Verify(Stock stock)
        {
            bool rs = true;

            return rs;
        }



    }
}
