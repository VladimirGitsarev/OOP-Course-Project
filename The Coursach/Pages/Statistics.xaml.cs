using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Configuration;
using The_Coursach.Classes;
namespace The_Coursach
{
    /// <summary>
    /// Логика взаимодействия для Statistics.xaml
    /// </summary>
    public partial class Statistics : UserControl
    {
        List<Employees> employees = new List<Employees>();
        List<Orders> orders = new List<Orders>();
        public int currentAccess;
        public int curId;
        public Statistics(int Id, int access)
        {
            InitializeComponent();
            currentAccess = access;
            curId = Id;
            //MessageBox.Show(access.ToString());
            Window_Loaded();
            from.SelectedDate = DateTime.Now.AddDays(-4);
            to.SelectedDate = DateTime.Now.AddDays(3);
        }

        private void Show_Click(object sender, RoutedEventArgs e)
        {
            orders.Clear();
            ordersList.Items.Clear();
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            if (currentAccess == 1)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("Select * from Orders", connection);
                    if (employeesList.SelectedIndex == 0)
                        command.CommandText = "Select * from Orders where Date between '" + from.SelectedDate + "' and '" + to.SelectedDate + "'";
                    else
                        command.CommandText = "Select* from Orders where Date between '" + from.SelectedDate + "' and '" + to.SelectedDate + "' and PersonId = " + employees[employeesList.SelectedIndex - 1].Id;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        orders.Add(new Orders
                        {
                            Id = (int)reader.GetValue(0),
                            OrderName = reader.GetValue(1).ToString(),
                            PersonId = (int)reader.GetValue(2),
                            Name = reader.GetValue(3).ToString(),
                            Surname = reader.GetValue(4).ToString(),
                            Total = (double)reader.GetValue(5),
                            Paid = (double)reader.GetValue(6),
                            Change = (double)reader.GetValue(7),
                            Date = reader.GetValue(8).ToString(),
                            Time = reader.GetValue(9).ToString()
                        });
                        ordersList.Items.Add(new ListBoxItem { Content = reader.GetValue(0) + " " + reader.GetValue(1).ToString() + " " + reader.GetValue(8).ToString().Substring(0, 10) });
                    }
                }
                double totalAll = 0;
                for (int i = 0; i < orders.Count; i++)
                    totalAll += orders[i].Total;
                if (employeesList.SelectedIndex == 0)
                {
                    IdInfo.Text = "All";
                    OrdersInfo.Text = orders.Count.ToString();
                    EarningsInfo.Text = totalAll.ToString();
                    if (orders.Count == 0)
                        AverageInfo.Text = "0";
                    else
                        AverageInfo.Text = Math.Round(totalAll / orders.Count, 2).ToString();
                }
                else
                {
                    IdInfo.Text = employees[employeesList.SelectedIndex - 1].Id.ToString();
                    OrdersInfo.Text = orders.Count.ToString();
                    EarningsInfo.Text = totalAll.ToString();
                    if (employees[employeesList.SelectedIndex - 1].Orders == 0)
                        AverageInfo.Text = "0";
                    else
                        AverageInfo.Text = Math.Round(totalAll / orders.Count, 2).ToString();
                }
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("Select * from Orders", connection);
                    command.CommandText = "Select* from Orders where Date between '" + from.SelectedDate + "' and '" + to.SelectedDate + "' and PersonId = " + employees[employeesList.SelectedIndex].Id;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        orders.Add(new Orders
                        {
                            Id = (int)reader.GetValue(0),
                            OrderName = reader.GetValue(1).ToString(),
                            PersonId = (int)reader.GetValue(2),
                            Name = reader.GetValue(3).ToString(),
                            Surname = reader.GetValue(4).ToString(),
                            Total = (double)reader.GetValue(5),
                            Paid = (double)reader.GetValue(6),
                            Change = (double)reader.GetValue(7),
                            Date = reader.GetValue(8).ToString(),
                            Time = reader.GetValue(9).ToString()
                        });
                        ordersList.Items.Add(new ListBoxItem { Content = reader.GetValue(0) + " " + reader.GetValue(1).ToString() + " " + reader.GetValue(8).ToString().Substring(0, 10) });
                    }
                }
                double totalAll = 0;
                for (int i = 0; i < orders.Count; i++)
                    totalAll += orders[i].Total;
                IdInfo.Text = employees[employeesList.SelectedIndex].Id.ToString();
                OrdersInfo.Text = orders.Count.ToString();
                EarningsInfo.Text = totalAll.ToString();
                if (orders.Count == 0)
                    AverageInfo.Text = "0";
                else
                    AverageInfo.Text = Math.Round(totalAll / orders.Count, 2).ToString();
            }
        }

        private void Window_Loaded()
        {
            if (currentAccess == 1)
            {
                employeesList.Items.Clear();
                employees.Clear();
                Stats.Visibility = Visibility.Hidden;
                orderStats.Visibility = Visibility.Hidden;
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("Select * from Stats", connection);
                    SqlDataReader reader = command.ExecuteReader();
                    employeesList.Items.Add(new ListBoxItem { Content = "All" });
                    employeesList.SelectedIndex = 0;
                    while (reader.Read())
                    {
                        employees.Add(new Employees
                        {
                            Id = (int)reader.GetValue(0),
                            Name = reader.GetValue(1).ToString(),
                            Surname = reader.GetValue(2).ToString(),
                            Earnings = (double)reader.GetValue(3),
                            Orders = (int)reader.GetValue(4)
                        });
                        employeesList.Items.Add(new ListBoxItem { Content = reader.GetValue(1).ToString() + " " + reader.GetValue(2).ToString() });
                    }
                }
            }
            else
            {
                employeesList.Items.Clear();
                employees.Clear();
                Stats.Visibility = Visibility.Hidden;
                orderStats.Visibility = Visibility.Hidden;
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("Select * from Stats where Id = " + curId, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    employeesList.SelectedIndex = 0;
                    while (reader.Read())
                    {
                        employees.Add(new Employees
                        {
                            Id = (int)reader.GetValue(0),
                            Name = reader.GetValue(1).ToString(),
                            Surname = reader.GetValue(2).ToString(),
                            Earnings = (double)reader.GetValue(3),
                            Orders = (int)reader.GetValue(4)
                        });
                        employeesList.Items.Add(new ListBoxItem { Content = reader.GetValue(1).ToString() + " " + reader.GetValue(2).ToString() });
                    }
                }
            }

        }

        private void EmployeesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show(employees[employeesList.SelectedIndex].Earnings.ToString());
            closeOrder.Visibility = Visibility.Hidden;
            Stats.Visibility = Visibility.Visible;
            orderStats.Visibility = Visibility.Hidden;
            orders.Clear();
            ordersList.Items.Clear();
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            if (currentAccess == 1)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    Stats.Visibility = Visibility.Visible;
                    orderStats.Visibility = Visibility.Hidden;
                    closeOrder.Visibility = Visibility.Hidden;
                    connection.Open();
                    try
                    {
                        if (employeesList.SelectedIndex == 0)
                        {
                            SqlCommand command0 = new SqlCommand("Select * from Orders where Date between '" + from.SelectedDate + "' and '" + to.SelectedDate + "'", connection);
                            SqlDataReader reader0 = command0.ExecuteReader();
                            while (reader0.Read())
                            {
                                orders.Add(new Orders
                                {
                                    Id = (int)reader0.GetValue(0),
                                    OrderName = reader0.GetValue(1).ToString(),
                                    PersonId = (int)reader0.GetValue(2),
                                    Name = reader0.GetValue(3).ToString(),
                                    Surname = reader0.GetValue(4).ToString(),
                                    Total = (double)reader0.GetValue(5),
                                    Paid = (double)reader0.GetValue(6),
                                    Change = (double)reader0.GetValue(7),
                                    Date = reader0.GetValue(8).ToString(),
                                    Time = reader0.GetValue(9).ToString()
                                });
                                ordersList.Items.Add(new ListBoxItem { Content = reader0.GetValue(0) + " " + reader0.GetValue(1).ToString() + " " + reader0.GetValue(8).ToString().Substring(0, 10) });
                            }
                            double totalAll = 0;
                            for (int i = 0; i < orders.Count; i++)
                                totalAll += orders[i].Total;
                            IdInfo.Text = "All";
                            OrdersInfo.Text = orders.Count.ToString();
                            EarningsInfo.Text = totalAll.ToString();
                            if (orders.Count == 0)
                                AverageInfo.Text = "0";
                            else
                                AverageInfo.Text = Math.Round(totalAll / orders.Count, 2).ToString();
                        }
                        else
                        {
                            Stats.Visibility = Visibility.Visible;
                            orderStats.Visibility = Visibility.Hidden;
                            SqlCommand command = new SqlCommand("Select * from Orders where PersonId = " + employees[employeesList.SelectedIndex - 1].Id + " and Date between '" + from.SelectedDate + "' and '" + to.SelectedDate + "'", connection);
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                orders.Add(new Orders
                                {
                                    Id = (int)reader.GetValue(0),
                                    OrderName = reader.GetValue(1).ToString(),
                                    PersonId = (int)reader.GetValue(2),
                                    Name = reader.GetValue(3).ToString(),
                                    Surname = reader.GetValue(4).ToString(),
                                    Total = (double)reader.GetValue(5),
                                    Paid = (double)reader.GetValue(6),
                                    Change = (double)reader.GetValue(7),
                                    Date = reader.GetValue(8).ToString(),
                                    Time = reader.GetValue(9).ToString()
                                });
                                ordersList.Items.Add(new ListBoxItem { Content = reader.GetValue(0) + " " + reader.GetValue(1).ToString() + " " + reader.GetValue(8).ToString().Substring(0, 10) });
                            }
                            double totalAll = 0;
                            for (int i = 0; i < orders.Count; i++)
                                totalAll += orders[i].Total;
                            IdInfo.Text = employees[employeesList.SelectedIndex - 1].Id.ToString();
                            OrdersInfo.Text = orders.Count.ToString();
                            EarningsInfo.Text = totalAll.ToString();
                            if (orders.Count == 0)
                                AverageInfo.Text = "0";
                            else
                                AverageInfo.Text = Math.Round(totalAll / orders.Count, 2).ToString();
                        }
                    }
                    catch
                    {
                        Stats.Visibility = Visibility.Visible;
                        orderStats.Visibility = Visibility.Hidden;
                        closeOrder.Visibility = Visibility.Hidden;
                    }
                }
            }
            else
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("Select * from Orders where PersonId = " + curId + " and Date between '" + from.SelectedDate + "' and '" + to.SelectedDate + "'", connection);
                    //employeesList.SelectedIndex = 0;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        orders.Add(new Orders
                        {
                            Id = (int)reader.GetValue(0),
                            OrderName = reader.GetValue(1).ToString(),
                            PersonId = (int)reader.GetValue(2),
                            Name = reader.GetValue(3).ToString(),
                            Surname = reader.GetValue(4).ToString(),
                            Total = (double)reader.GetValue(5),
                            Paid = (double)reader.GetValue(6),
                            Change = (double)reader.GetValue(7),
                            Date = reader.GetValue(8).ToString(),
                            Time = reader.GetValue(9).ToString()
                        });
                        ordersList.Items.Add(new ListBoxItem { Content = reader.GetValue(0) + " " + reader.GetValue(1).ToString() + " " + reader.GetValue(8).ToString().Substring(0, 10) });
                    }
                    //MessageBox.Show(employeesList.SelectedIndex.ToString());
                    double totalAll = 0;
                    for (int i = 0; i < orders.Count; i++)
                        totalAll += orders[i].Total;
                    IdInfo.Text = employees[0].Id.ToString();
                    OrdersInfo.Text = orders.Count.ToString();
                    EarningsInfo.Text = totalAll.ToString();
                    if (orders.Count == 0)
                        AverageInfo.Text = "0";
                    else
                        AverageInfo.Text = Math.Round(totalAll / orders.Count, 2).ToString();
                }
            }
        }

        private void OrdersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                closeOrder.Visibility = Visibility.Visible;
                Stats.Visibility = Visibility.Hidden;
                orderStats.Visibility = Visibility.Visible;
                orderId.Text = orders[ordersList.SelectedIndex].Id.ToString();
                orderName.Text = orders[ordersList.SelectedIndex].OrderName.ToString();
                orderPersonId.Text = orders[ordersList.SelectedIndex].PersonId.ToString();
                orderWaiterName.Text = orders[ordersList.SelectedIndex].Name.ToString();
                orderWaiterSurname.Text = orders[ordersList.SelectedIndex].Surname.ToString();
                orderTotal.Text = orders[ordersList.SelectedIndex].Total.ToString();
                orderPaid.Text = orders[ordersList.SelectedIndex].Paid.ToString();
                orderChange.Text = orders[ordersList.SelectedIndex].Change.ToString();
                orderDate.Text = orders[ordersList.SelectedIndex].Date.ToString().Substring(0, 10);
                orderTime.Text = orders[ordersList.SelectedIndex].Time.ToString().Substring(0, orders[ordersList.SelectedIndex].Time.ToString().LastIndexOf('.'));
            }
            catch { ordersList.SelectedIndex = -1; }
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Window_Loaded();
        }

        private void CloseOrder_Click(object sender, RoutedEventArgs e)
        {
            ordersList.SelectedIndex = -1;
            closeOrder.Visibility = Visibility.Hidden;
            orderStats.Visibility = Visibility.Hidden;
            Stats.Visibility = Visibility.Visible;
        }
    }

}
