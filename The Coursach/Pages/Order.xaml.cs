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
    /// Логика взаимодействия для Order.xaml
    /// </summary>
    public partial class Order : UserControl
    {
        public List<Menu> menus = new List<Menu>();
        public List<double> prices = new List<double>();
        public List<List<Menu>> orders = new List<List<Menu>>();
        public string menuButton;
        public string curName;
        public string curSurname;
        public int curId;
        public int count = 0;
        public Order(int Id, string Name, string Surname)
        {
            InitializeComponent();
            Add.IsEnabled = false;
            curId = Id;
            curName = Name;
            curSurname = Surname;
            //MessageBox.Show(Name + " " + Surname);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            customPrice.BorderThickness = new Thickness(0);
            try
            {
                if (menuList.SelectedItem != null)
                {
                    orders[newList.SelectedIndex].Add(menus[menuList.SelectedIndex]);
                    qeqList.ItemsSource = null;
                    qeqList.ItemsSource = orders[newList.SelectedIndex];
                    prices[newList.SelectedIndex] += menus[menuList.SelectedIndex].Price;
                    Total.Text = prices[newList.SelectedIndex].ToString();
                    menuList.SelectedIndex = -1;
                }
                else
                {
                    if (customName.Text != null && customPrice.Text != null)
                        orders[newList.SelectedIndex].Add(new Menu { Name = customName.Text, Price = Double.Parse(customPrice.Text.Replace(".", ",")) });
                    prices[newList.SelectedIndex] += Double.Parse(customPrice.Text.Replace(".", ","));
                    qeqList.ItemsSource = null;
                    qeqList.ItemsSource = orders[newList.SelectedIndex];
                    customGrid.Visibility = Visibility.Hidden;
                    Custom.Visibility = Visibility.Visible;
                    customName.Text = null;
                    customPrice.Text = null;
                    Total.Text = prices[newList.SelectedIndex].ToString();
                }

            }
            catch
            {
                if (customPrice.Text.Length > 0)
                {
                    customPrice.BorderBrush = Brushes.Red;
                    customPrice.BorderThickness = new Thickness(1);
                }
            }
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            Button item = sender as Button;
            menuButton = item.Name;
            Deserts.BorderBrush = Brushes.Gray;
            Deserts.BorderThickness = new Thickness(1);
            Deserts.Foreground = new SolidColorBrush(Colors.Gray);
            Appetizers.BorderBrush = Brushes.Gray;
            Appetizers.BorderThickness = new Thickness(1);
            Appetizers.Foreground = new SolidColorBrush(Colors.Gray);
            MainDishes.BorderBrush = Brushes.Gray;
            MainDishes.BorderThickness = new Thickness(1);
            MainDishes.Foreground = new SolidColorBrush(Colors.Gray);
            Drinks.BorderBrush = Brushes.Gray;
            Drinks.BorderThickness = new Thickness(1);
            Drinks.Foreground = new SolidColorBrush(Colors.Gray);
            switch (item.Name)
            {
                case "Drinks": Drinks.BorderBrush = Brushes.Green; Drinks.BorderThickness = new Thickness(2); Drinks.Foreground = new SolidColorBrush(Colors.Green); break;
                case "Deserts": Deserts.BorderBrush = Brushes.Green; Deserts.BorderThickness = new Thickness(2); Deserts.Foreground = new SolidColorBrush(Colors.Green); break;
                case "Appetizers": Appetizers.BorderBrush = Brushes.Green; Appetizers.BorderThickness = new Thickness(2); Appetizers.Foreground = new SolidColorBrush(Colors.Green); break;
                case "MainDishes": MainDishes.BorderBrush = Brushes.Green; MainDishes.BorderThickness = new Thickness(2); MainDishes.Foreground = new SolidColorBrush(Colors.Green); break;
            }
            menus.Clear();
            menuList.ItemsSource = null;
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Select * from " + item.Name + " order by Type", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                    menus.Add(new Menu { Name = (string)reader.GetValue(0), Price = (double)reader.GetValue(1), Image = (byte[])reader.GetValue(2) });
                menuList.ItemsSource = menus;
                connection.Close();
                connection.Open();
                SqlCommand command0 = new SqlCommand("Select distinct Type from " + item.Name, connection);
                reader = command0.ExecuteReader();
                types.Items.Clear();
                types.Items.Add(new ComboBoxItem { Content = "All" });
                types.SelectedIndex = 0;
                while (reader.Read())
                    types.Items.Add(new ComboBoxItem { Content = reader.GetValue(0) });
            }
        }

        private void NewOrder_Click(object sender, RoutedEventArgs e)
        {
            count++;
            string order = curName.Substring(0, 1) + curSurname.Substring(0, 1) + count;
            orders.Add(new List<Menu>());
            prices.Add(new double());
            if (orderName.Text.Length != 0)
                order = orderName.Text;
            newList.Items.Add(new ListBoxItem() { Content = order });
            newList.SelectedIndex = orders.Count - 1;
            orderName.Clear();
        }

        private void NewList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Add.IsEnabled = true;
                qeqList.ItemsSource = null;
                qeqList.Items.Clear();
                qeqList.ItemsSource = orders[newList.SelectedIndex];
                Total.Text = prices[newList.SelectedIndex].ToString();
            }
            catch
            {
                newList.SelectedIndex = 0;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void QeqList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (qeqList.Items.Count > 0)
                Total.Text = prices[newList.SelectedIndex].ToString();
            else
                Total.Text = "0";
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (newList.Items.Count != 0)
            {
                MsgBox msgbox = new MsgBox("Are you sure you want to delete this order?");
                if ((bool)msgbox.ShowDialog())
                {
                    Delete();
                }
            }
        }
        private void DelEl_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (qeqList.Items.Count != 0)
                {
                    MsgBox msgbox = new MsgBox("Are you sure you want to delete this item?");
                    if ((bool)msgbox.ShowDialog())
                    {
                        prices[newList.SelectedIndex] -= orders[newList.SelectedIndex][qeqList.SelectedIndex].Price;
                        orders[newList.SelectedIndex].RemoveAt(qeqList.SelectedIndex);
                        qeqList.ItemsSource = null;
                        qeqList.ItemsSource = orders[newList.SelectedIndex];
                        qeqList.SelectedIndex = 0;
                        if (qeqList.Items.Count > 0)
                            Total.Text = prices[newList.SelectedIndex].ToString();
                        else
                            Total.Text = "0";
                    }
                }
            }
            catch
            {
                qeqList.SelectedIndex = -1;
            }
        }

        public void Delete()
        {
                int select = newList.SelectedIndex;
                orders.RemoveAt(newList.SelectedIndex);
                prices.RemoveAt(newList.SelectedIndex);
                newList.Items.RemoveAt(newList.SelectedIndex);
                newList.SelectedIndex = select;
        }
        private void CloseOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string order = newList.SelectedItem.ToString().Remove(0, newList.SelectedItem.ToString().LastIndexOf(' '));
                PayWindow payWindow = new PayWindow(orders[newList.SelectedIndex], prices[newList.SelectedIndex], order, curId, curName, curSurname);
                MsgBox msgbox = new MsgBox("Close this order? It will be deleted from the list. The bill will be created.");
                if ((bool)msgbox.ShowDialog())
                {
                    payWindow.Show();
                    Delete();
                }
                if (newList.Items.Count == 0)
                    Total.Text = "0";
            }
            catch
            {
                qeqList.SelectedIndex = -1;
            }
        }

        private void Custom_Click(object sender, RoutedEventArgs e)
        {
            customGrid.Visibility = Visibility.Visible;
            Custom.Visibility = Visibility.Hidden;
            menuList.SelectedIndex = -1;
        }

        private void Types_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            //MessageBox.Show(selectedItem.Content.ToString());
            menus.Clear();
            menuList.ItemsSource = null;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                if (selectedItem.Content.ToString() == "All")
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("Select * from " + menuButton + " order by Type", connection);
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                            menus.Add(new Menu { Name = (string)reader.GetValue(0), Price = (double)reader.GetValue(1), Image = (byte[])reader.GetValue(2) });
                    }
                }
                else
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("Select * from " + menuButton + " where Type = '" + selectedItem.Content.ToString() + "'", connection);
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                            menus.Add(new Menu { Name = (string)reader.GetValue(0), Price = (double)reader.GetValue(1), Image = (byte[])reader.GetValue(2) });
                    }
                }
                menuList.ItemsSource = menus;
            }
            catch
            { types.SelectedIndex = 0; }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            customGrid.Visibility = Visibility.Hidden;
            Custom.Visibility = Visibility.Visible;
        }
    }
    public class Menu
    {
        public string Path { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
    }
}
