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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ADFSDPhamaV2
{
    /// <summary>
    /// Interaction logic for Admin_Medication.xaml
    /// </summary>
    public partial class Admin_Medication : Window
    {
        public Admin_Medication()
        {
            InitializeComponent();
            init();
        }
        private void init()
        {
            Tbl_Title.Text = "Medication";
            BtnUpdate.IsEnabled = false;
            BtnDelete.IsEnabled = false;
            Tbx_id.Text = "";
            Tbx_description.Text = "";
            Tbx_name.Text = "";
            Tbx_unit.Text = "";
            Cmb_photo.SelectedIndex = -1;
            Cmb_suplier.SelectedIndex = -1;
            
            PharmaConn pharmaConn = new PharmaConn();

            Cmb_suplier.ItemsSource = pharmaConn.Supliers.ToList();
            Cmb_suplier.SelectedValuePath = "id";
            Cmb_suplier.DisplayMemberPath = "name";

            Cmb_photo.ItemsSource = pharmaConn.Photos.ToList();
            Cmb_photo.SelectedValuePath = "id";
            Cmb_photo.DisplayMemberPath = "name";

            LvList.ItemsSource = pharmaConn.Medications.ToList();
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
            Medication currSelected = LvList.SelectedItem as Medication;
            BtnUpdate.IsEnabled = (currSelected != null);
            BtnDelete.IsEnabled = (currSelected != null);
            if (currSelected == null)
            {
                init();
            }
            else
            {
                Tbx_id.Text = currSelected.id.ToString();
                Tbx_description.Text = currSelected.description;
                Tbx_name.Text = currSelected.name;
                Tbx_unit.Text = currSelected.unit.ToString();
                Cmb_suplier.SelectedValue = currSelected.suplier_id;
                Cmb_photo.SelectedValue = currSelected.photo_id;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Medication medication = new Medication();
                medication.name = Tbx_name.Text;
                medication.description = Tbx_description.Text;
                medication.suplier_id = (int)Cmb_suplier.SelectedValue;
                medication.photo_id = (int)Cmb_photo.SelectedValue;
                if (int.TryParse(Tbx_unit.Text, out int rs))
                {
                    medication.unit = rs;
                } 
                else
                {
                    MessageBox.Show("Check unit.");
                    return;
                }
                

                if (Verify(medication))
                {
                    PharmaConn pharmaConn = new PharmaConn();
                    pharmaConn.Medications.Add(medication);
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
                MessageBox.Show("Medication code already exist. You may choose the code to modify quantity.");
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Medication medication = new Medication();
                medication.id = int.Parse(Tbx_id.Text);
                medication.name = Tbx_name.Text;
                medication.description = Tbx_description.Text;
                medication.suplier_id = (int)Cmb_suplier.SelectedValue;
                medication.photo_id = (int)Cmb_photo.SelectedValue;
                if (int.TryParse(Tbx_unit.Text, out int rs))
                {
                    medication.unit = rs;
                }
                else
                {
                    MessageBox.Show("Check unit.");
                }

                if (Verify(medication))
                {
                    PharmaConn pharmaConn = new PharmaConn();
                    pharmaConn.Medications.AddOrUpdate(medication);
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
                Medication currSelected = LvList.SelectedItem as Medication;
                Medication medication = new Medication { id = currSelected.id };
                PharmaConn pharmaConn = new PharmaConn();
                pharmaConn.Medications.Attach(medication);
                pharmaConn.Entry(medication).State = System.Data.Entity.EntityState.Deleted;
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

        private bool Verify(Medication medication)
        {
            bool rs = true;

            return rs;
        }
    }
}

