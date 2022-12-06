using ADFSDPhamaV2.Model;
using System;
using System.Collections.Generic;
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
using DataVis = System.Windows.Forms.DataVisualization;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;
using System.Xml.Linq;


namespace ADFSDPhamaV2
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : System.Windows.Window
    {
        public Admin()
        {
            InitializeComponent();
            Chart1.Series[0].LabelBackColor = System.Drawing.Color.White;

            Chart1.Series[0].Font = new Font("Segoe UI", 12);
            Chart1.ChartAreas[0].AxisX.TitleFont = new Font("Segoe UI", 12);
            Chart1.ChartAreas[0].AxisY.TitleFont = new Font("Segoe UI", 12);
            Chart1.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Segoe UI", 12);
            Chart1.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Segoe UI", 12);
            Chart1.ChartAreas[0].AxisX.Title = "Medication";
            Chart1.ChartAreas[0].AxisY.Title = "Quantity";

            ChartDisplay();
        }

        private void ChartDisplay()
        {
            using (PharmaConn pharmaConn = new PharmaConn())
            {
                var stocks = (from m in pharmaConn.Medications
                             join s in pharmaConn.Stocks
                             on m.id equals s.med                
                             select new
                             {  
                                 medname = m.name,
                                 quantity = s.quantity
                             }).ToList();

                Chart1.Series[0].Points.Clear();
                foreach (var s in stocks)
                {
                    var label = s.medname;
                    var value = s.quantity;
                    Chart1.Series[0].Points.Add(value).AxisLabel = label;
                    Chart1.Series[0].BorderWidth = 3;
                }
            }           
        }

        private void ChartType_Click(object sender, RoutedEventArgs e)
        {
            switch (((RadioButton)sender).Name)
            {
                case "ColRadio":
                    Chart1.Series[0].ChartType = DataVis.Charting.SeriesChartType.Column;
                    break;
                case "BarRadio":
                    Chart1.Series[0].ChartType = DataVis.Charting.SeriesChartType.Bar;
                    break;
                case "PieRadio":
                    Chart1.Series[0].ChartType = DataVis.Charting.SeriesChartType.Pie;
                    break;
                case "LineRadio":
                    Chart1.Series[0].ChartType = DataVis.Charting.SeriesChartType.Line;
                    break;
                default:
                    break;
            }
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

        private async void Btn_Export_Click(object sender, RoutedEventArgs e)
        {
            using (PharmaConn pharmaConn = new PharmaConn())
            {
                var stocks = (from m in pharmaConn.Medications
                              join s in pharmaConn.Stocks
                              on m.id equals s.med
                              select new
                              {
                                  medname = m.name,
                                  meddes  = m.description,
                                  medunit = m.unit,
                                  quantity = s.quantity
                              }).ToList();

                //configure save file dialog box
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "Document"; //default file name
                dlg.DefaultExt = ".csv"; //default file extension
                dlg.Filter = "CSV|*.csv"; //filter files by extension

                // Show save file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {
                    using (var sw = new StreamWriter(dlg.FileName))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("Medication Name, Description, Unit, Quantity");
                        foreach (var s in stocks)
                        {
                            sb.AppendLine(string.Format("{0},{1},{2},{3}",s.medname, s.meddes, s.medunit, s.quantity));     
                        }
                        await sw.WriteAsync(sb.ToString());
                        MessageBox.Show("Data has been successfully exported", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }              
                
            }

        }
    }    
}
