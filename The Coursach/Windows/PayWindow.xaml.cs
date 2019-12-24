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
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Configuration;

namespace The_Coursach
{
    /// <summary>
    /// Логика взаимодействия для PayWindow.xaml
    /// </summary>
    public partial class PayWindow : Window
    {
        List<Menu> menus = new List<Menu>();
        public double total0;
        int check = -1;
        int curId;
        double paid;
        double change;
        string curName;
        string curSurname;
        string nameOrder;
        DateTime date;
        public PayWindow(List<Menu> menus, double total, string order, int Id, string Name, string Surname)
        {
            InitializeComponent();
            Close.IsEnabled = false;
            grid.ItemsSource = menus;
            total0 = total;
            orderName.Text = order.Remove(0,1);
            orderWaiter.Text = Name + " " + Surname;
            curId = Id;
            curName = Name;
            curSurname = Surname;
            nameOrder = order.Remove(0, 1);
            Total.Text = total0.ToString() + " BYN";
            discountIn.Text = "0";
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (check == -1)
                {
                    if (int.Parse(discountIn.Text) < 0 || int.Parse(discountIn.Text) > 100)
                    {
                        info.Text = "Wrong value percent";
                    }
                    else
                    {
                        total0 = Math.Round(total0 - (total0 * int.Parse(discountIn.Text) / 100), 2);
                        Pay.Content = "Pay";
                        Total.Text = total0.ToString() + " BYN";
                        check++;
                    }
                }
                if (check == 0)
                {
                    if (double.Parse(In.Text.Replace(".", ",")) - total0 >= 0 && In.Text.Length > 0)
                    {
                        info.Text = "Order succesfully paid";
                        Change.Text = Math.Round(double.Parse(In.Text.Replace(".", ",")) - total0, 2).ToString() + " BYN";
                        Pay.Content = "Close";
                        paid = double.Parse(In.Text.Replace(".", ","));
                        change = Math.Round(double.Parse(In.Text.Replace(".", ",")) - total0, 2);
                        In.Text += " BYN";
                        Close.IsEnabled = true;
                        date = DateTime.Now;
                        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            String cmd = "Insert into Orders(PersonId, Name, Surname, Total, Date, OrderName, Time, Paid, Change) Values("
                                + curId + ", '" + curName + "' , '" + curSurname + "' , " + total0.ToString().Replace(",", ".") +
                                " , Getdate(), '" + nameOrder + "', '" + date.TimeOfDay.ToString() + "' , " + paid.ToString().Replace(",", ".") + " , " + change.ToString().Replace(",", ".") + ")";
                            SqlCommand command = new SqlCommand("Update Stats set Earnings = Earnings + " + total0.ToString().Replace(",", ".") + " , Orders += 1 where Id = " + curId, connection);
                            SqlCommand command0 = new SqlCommand(cmd, connection);
                            command.ExecuteNonQuery();
                            command0.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        info.Text = "Wrong value";
                        check--;
                    }
                }
                else
                {
                    this.Close();
                }
                check++;
        }
            catch
            {
                if (In.Text.Length == 0 && check == -1)
                    info.Text = "Wrong value";
            }
        }
    }
}
