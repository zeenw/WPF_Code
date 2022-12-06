using ADFSDPhamaV2.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ADFSDPhamaV2
{
    /// <summary>
    /// Interaction logic for Admin_User.xaml
    /// </summary>
    public partial class Admin_User : Window
    {
        public Admin_User()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            Tbl_User.Text = "User";
            BtnUpdate.IsEnabled = false;
            BtnDelete.IsEnabled = false;
            Tbx_email.Text = "";
            Tbx_password.Text = "";
            Tbx_id.Text = "";
            Combo_Role.SelectedIndex = 0;
            Combo_Role.ItemsSource = System.Enum.GetNames(typeof(EnumRole));

            PharmaConn pharmaConn = new PharmaConn();
            LvUser.ItemsSource = pharmaConn.Usrs.ToList();
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

        private void LvUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Usr currSelected = LvUser.SelectedItem as Usr;
            BtnUpdate.IsEnabled = (currSelected != null);
            BtnDelete.IsEnabled = (currSelected != null);
            if (currSelected == null)
            {
                init();
            }
            else
            {
                Tbx_id.Text = currSelected.id.ToString();
                Tbx_email.Text = currSelected.email;
                Combo_Role.Text = currSelected.role.ToString();
                Tbx_password.Text = currSelected.password;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string email = Tbx_email.Text;
            string pword = Tbx_password.Text;
            int role = Combo_Role.SelectedIndex;

            // validation
            Usr usr = new Usr();
            usr.email = email;
            usr.password = pword;

            switch (role)
            {
                case (int)EnumRole.admin:
                    usr.role = EnumRole.admin;
                    break;
                case (int)EnumRole.user:
                    usr.role = EnumRole.user;
                    break;
                default:
                    // code block
                    break;
            }



            //if (Verify(usr))
            if (AreUsrInputsValid())                
            {
            
                PharmaConn pharmaConn = new PharmaConn();
                pharmaConn.Usrs.Add(usr);
                pharmaConn.SaveChanges();
            }
            else
            {
                return;
            }

            init();
            LvUser.SelectedItem = null;
            MessageBox.Show("New user created.");
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Usr usr = new Usr();
            usr.email = Tbx_email.Text;
            usr.password = Tbx_password.Text;
            usr.id = int.Parse(Tbx_id.Text);

            switch (Combo_Role.SelectedIndex)
            {
                case (int)EnumRole.admin:
                    usr.role = EnumRole.admin;
                    break;
                case (int)EnumRole.user:
                    usr.role = EnumRole.user;
                    break;
                default:
                    // code block
                    break;
            }

            //if (Verify(usr))
            if (AreUsrUpdateInputValid())
            {
                PharmaConn pharmaConn = new PharmaConn();
                pharmaConn.Usrs.AddOrUpdate(usr);
                pharmaConn.SaveChanges();
            }
            else
            {
                return;
            }

            init();
            LvUser.SelectedItem = null;
            MessageBox.Show("User information updated.");
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Usr currSelected = LvUser.SelectedItem as Usr;
                Usr usr = new Usr { id = currSelected.id };
                PharmaConn pharmaConn = new PharmaConn();
                pharmaConn.Usrs.Attach(usr);
                pharmaConn.Entry(usr).State = System.Data.Entity.EntityState.Deleted;
                pharmaConn.SaveChanges();
                init();
                LvUser.SelectedItem = null;
                MessageBox.Show("User information delete.");
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

        private bool Verify(Usr usr)
        {
            bool rs = true;

            return rs;
        }

        private bool AreUsrInputsValid()
        {
            string email = Tbx_email.Text;
            string pword = Tbx_password.Text;

            if (!IsUsrEmailValid(email, out string errorEmail))
            {
                MessageBox.Show(this, errorEmail, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!IsUsrPwordValid(pword, out string errorPword))
            {
                MessageBox.Show(this, errorPword, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            
            
            if (!IsUsrEmailUnique(email, out string errorEmailNotUnique))
            {
                MessageBox.Show(this, errorEmailNotUnique, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            
            
            return true;
        }

        private bool AreUsrUpdateInputValid()
        {
            string email = Tbx_email.Text;
            string pword = Tbx_password.Text;

            if (!IsUsrEmailValid(email, out string errorEmail))
            {
                MessageBox.Show(this, errorEmail, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!IsUsrPwordValid(pword, out string errorPword))
            {
                MessageBox.Show(this, errorPword, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }




        // Usr email validate using MailAddress Class
        private bool IsUsrEmailValid(string email, out string errorEmail)
        {
            var valid = true;
            errorEmail = null;
            try
            {
                var emailAddress = new MailAddress(email);
            }
            catch //(Exception ex) when (ex is ArgumentNullException || ex is ArgumentException||ex is FormatException)
            {
                valid = false;
                errorEmail = @"Invalid Email. Please input a valid Email!";
                //MessageBox.Show(this, "Error:" + ex.Message, "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            return valid;

            // Use Regex to validate email
            /*
            if (Regex.IsMatch(email, @"^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.[a-zA-Z0-9]{2,6}$"))
            {
                errorEmail = null;
                return true;
            }
            errorEmail = @"Invalid Email. Please input a valid Email!";
            return false;
            */

        }

        // Usr password validate
        private bool IsUsrPwordValid(string password, out string errorPword)
        {
            //password length 3-30 characters long,contain uppercase and lowercase letters numbers and !@#$*
            if (Regex.IsMatch(password, @"^[a-zA-Z0-9\!\@\#\$\*]{3,30}$"))
            {
                errorPword = null;
                return true;
            }
            errorPword = @"Invalid Password. Your password must be 3-30 characters long, can contain uppercase and lowercase letters numbers and !@#$* ";
            return false;

        }

        
        // Usr email unique validate

        private bool IsUsrEmailUnique(string email, out string errorEmailNotUnique)
        {
            PharmaConn pharmaConn = new PharmaConn();
            List<Usr> list = pharmaConn.Usrs.ToList();

            string uemail = Tbx_email.Text;

            Usr rs = null;

            foreach (Usr usr in list)
            {
                if (usr.email == uemail)
                {
                    rs = usr;
                }
            }

            if (rs == null)
            {
                errorEmailNotUnique = null;
                return true;
            }
            else
            {
                errorEmailNotUnique = $"Email address:{uemail} is already used.";
                return false;

            }
        }
        

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            using (PharmaConn pharmaConn = new PharmaConn())
            {
                LvUser.ItemsSource = (from user in pharmaConn.Usrs
                                      where user.email.Contains(TbxSearch.Text)
                                      select user).ToList();                
            }
        }
    }

}
