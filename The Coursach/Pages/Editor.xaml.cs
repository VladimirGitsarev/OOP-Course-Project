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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Configuration;
using System.Data;
using System.IO;
using Microsoft.Win32;
using The_Coursach.Classes;

namespace The_Coursach
{
    /// <summary>
    /// Логика взаимодействия для Editor.xaml
    /// </summary>
    public partial class Editor : UserControl
    {
        public string choose = null;
        public List<Menus> menu = new List<Menus>();
        public string delName;
        public Editor()
        {
            InitializeComponent();
        }
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        SqlDataAdapter adapter;
        DataTable StaffTable;
        private void Window_Loaded()
        {
            menu.Clear();
            string sql = "SELECT Price, Name, Type, Image from " + choose + " order by Type" ;
            StaffTable = new DataTable();
            SqlConnection connection = null;
           
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);

            // команды на добавление для вызова хранимой процедуры
            adapter.InsertCommand = new SqlCommand("sp_InsertMenu", connection);
            adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
            adapter.InsertCommand.Parameters.Add(new SqlParameter("@name", SqlDbType.Int, 50, "Name"));
            adapter.InsertCommand.Parameters.Add(new SqlParameter("@price", SqlDbType.NChar, 10, "Price"));
            adapter.InsertCommand.Parameters.Add(new SqlParameter("@type", SqlDbType.NChar, 10, "Type"));
            adapter.InsertCommand.Parameters.Add(new SqlParameter("@image", SqlDbType.Binary, 0, "Image"));
            adapter.InsertCommand.Parameters.Add(new SqlParameter("@path", SqlDbType.Binary, 0, "Path"));
            connection.Open();
            adapter.Fill(StaffTable);
            MenuGrid.ItemsSource = StaffTable.DefaultView;

            SqlCommand command0 = new SqlCommand("Select Name, Price, Type, Image, Path from " + choose + " order by Type", connection);
                SqlDataReader reader = command0.ExecuteReader();
                while (reader.Read())
                    menu.Add(new Menus { Name = reader.GetValue(0).ToString(), Price = reader.GetValue(1).ToString(), Type = reader.GetValue(2).ToString(), Image = (byte[])reader.GetValue(3), Path = reader.GetValue(4).ToString()});
            
        }
        private void UpdateDB()
        {
            SqlCommandBuilder comandbuilder = new SqlCommandBuilder(adapter);
            adapter.Update(StaffTable);
            MenuGrid.ItemsSource = StaffTable.DefaultView;
        }
        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            Button item = sender as Button;
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
                case "Drinks": Drinks.BorderBrush = Brushes.Green; Drinks.BorderThickness = new Thickness(2); Drinks.Foreground = new SolidColorBrush(Colors.Green); choose = "Drinks"; break;
                case "Deserts": Deserts.BorderBrush = Brushes.Green; Deserts.BorderThickness = new Thickness(2); Deserts.Foreground = new SolidColorBrush(Colors.Green); choose = "Deserts"; break;
                case "Appetizers": Appetizers.BorderBrush = Brushes.Green; Appetizers.BorderThickness = new Thickness(2); Appetizers.Foreground = new SolidColorBrush(Colors.Green); choose = "Appetizers"; break;
                case "MainDishes": MainDishes.BorderBrush = Brushes.Green; MainDishes.BorderThickness = new Thickness(2); MainDishes.Foreground = new SolidColorBrush(Colors.Green); choose = "MainDishes"; break;
            }
            Window_Loaded();
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            nameAdd.BorderBrush = Brushes.Green;
            priceAdd.BorderBrush = Brushes.Green;
            typeAdd.BorderBrush = Brushes.Green;
            errorAdd.Text = "Wrong value! Enter again";
            errorAdd.Visibility = Visibility.Hidden;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                connection.Open();
                bool correct = false;
                if (nameAdd.Text.Length == 0)
                    nameAdd.BorderBrush = Brushes.Red;
                if (priceAdd.Text.Length == 0)
                    priceAdd.BorderBrush = Brushes.Red;
                if (typeAdd.Text.Length == 0)
                    typeAdd.BorderBrush = Brushes.Red;
                if (nameAdd.Text.Length != 0 && priceAdd.Text.Length != 0 && typeAdd.Text.Length != 0)
                    correct = true;
                try
                {
                    if (correct == true)
                    {
                        SqlCommand command = new SqlCommand();
                        command.Connection = connection;
                        command.CommandText = @"INSERT INTO " + choose + " VALUES (@Name, @Price, @Image, @Type, @Path)";
                        command.Parameters.Add("@Name", SqlDbType.NVarChar, 50);
                        command.Parameters.Add("@Price", SqlDbType.Float, 50);
                        command.Parameters.Add("@Type", SqlDbType.NVarChar, 50);
                        command.Parameters.Add("@Image", SqlDbType.Image, 1000000);
                        command.Parameters.Add("@Path", SqlDbType.NVarChar, 100);
                        string filename = null;
                    if (pathAdd.Text == "")
                        filename = @"D:\All\Универ\2 курс\ООП\The Coursach\Images\image.png";
                    else
                        filename = pathAdd.Text;
                        string name = nameAdd.Text;
                        string price = priceAdd.Text.Replace(".", ",");
                        string type = typeAdd.Text;
                        byte[] imageData;
                        using (System.IO.FileStream fs = new System.IO.FileStream(filename, FileMode.Open))
                        {
                            imageData = new byte[fs.Length];
                            fs.Read(imageData, 0, imageData.Length);
                        }
                        command.Parameters["@Name"].Value = name;
                        command.Parameters["@Price"].Value = price;
                        command.Parameters["@Type"].Value = type;
                        command.Parameters["@Image"].Value = imageData;
                        command.Parameters["@Path"].Value = filename;
                        command.ExecuteNonQuery();
                        ClearBoxes();
                        UpdateDB();
                        Window_Loaded();
                    }
                    else
                        errorAdd.Visibility = Visibility.Visible;
                }
                catch
                {
                    if (priceAdd.Text.Length > 0 && choose != null)
                        priceAdd.BorderBrush = Brushes.Red;
                    else
                        errorAdd.Text = "Chose table to add!";
                    errorAdd.Visibility = Visibility.Visible;
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ClearBoxes();
        }

        private void SaveItem_Click(object sender, RoutedEventArgs e)
        {
            nameUpd.BorderBrush = Brushes.Green;
            priceUpd.BorderBrush = Brushes.Green;
            pathUpd.BorderBrush = Brushes.Green;
            errorUpd.Visibility = Visibility.Hidden;
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            sqlConnection.Open();
            bool correct = false;
            if (nameUpd.Text.Length == 0)
                nameUpd.BorderBrush = Brushes.Red;
            if (priceUpd.Text.Length == 0)
                priceUpd.BorderBrush = Brushes.Red;
            if (nameUpd.Text.Length != 0 && priceUpd.Text.Length != 0)
                correct = true;
            //string filename = pathUpd.Text;
            //byte[] imageData;
            //using (System.IO.FileStream fs = new System.IO.FileStream(filename, FileMode.Open))
            //{
            //    imageData = new byte[fs.Length];
            //    fs.Read(imageData, 0, imageData.Length);
            //}
            try
            {
                if (correct == true)
                {

                    //String str = "Update " + choose + " Set Name = '" + nameUpd.Text + "', Type =  '" + typeUpd.Text + "', Price = '" + priceUpd.Text + "' , Path = '" + pathUpd.Text + "' , Image = '" + imageData + "' Where Name =  '" + menu[MenuGrid.SelectedIndex].Name + "'";
                    //sqlConnection.Open();
                    //SqlCommand cmd = new SqlCommand(str, sqlConnection);
                    //cmd.ExecuteNonQuery();
                    SqlCommand command = new SqlCommand();
                    SqlCommand command0 = new SqlCommand();
                    command0.Connection = sqlConnection;
                    //string delName = nameUpd.Text;
                    command0.CommandText = "Delete from " + choose + " Where Name = @delName";
                    command0.Parameters.Add("@delName", SqlDbType.NVarChar, 50);
                    command.Connection = sqlConnection;
                    command.CommandText = @"Insert into " + choose + " values(@Name, @Price, @Image, @Type, @Path)";
                    command.Parameters.Add("@Name", SqlDbType.NVarChar, 50);
                    command.Parameters.Add("@Price", SqlDbType.Float, 50);
                    command.Parameters.Add("@Type", SqlDbType.NVarChar, 50);
                    command.Parameters.Add("@Image", SqlDbType.Image, 1000000);
                    command.Parameters.Add("@Path", SqlDbType.NVarChar, 100);
                    string filename = pathUpd.Text;
                    string name = nameUpd.Text;
                    string price = priceUpd.Text.Replace(".", ",");
                    string type = typeUpd.Text;
                    byte[] imageData;
                    using (System.IO.FileStream fs = new System.IO.FileStream(filename, FileMode.Open))
                    {
                        imageData = new byte[fs.Length];
                        fs.Read(imageData, 0, imageData.Length);
                    }
                    command0.Parameters["@delName"].Value = delName;
                    command0.ExecuteNonQuery();
                    command.Parameters["@Name"].Value = name;
                    command.Parameters["@Price"].Value = price;
                    command.Parameters["@Type"].Value = type;
                    command.Parameters["@Image"].Value = imageData;
                    command.Parameters["@Path"].Value = filename;
                    command.ExecuteNonQuery();
                    Window_Loaded();
                    newStck.Visibility = Visibility.Visible;
                    addStck.Visibility = Visibility.Hidden;
                    updStck.Visibility = Visibility.Hidden;
                }
                else
                    errorUpd.Visibility = Visibility.Visible;
        }
            catch
            {
                if (priceUpd.Text.Length > 0)
                    priceUpd.BorderBrush = Brushes.Red;
                errorUpd.Visibility = Visibility.Visible;
            }
}

        private void MenuGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearBoxes();
        }

        private void UpdButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MenuGrid.SelectedItem != null)
                {
                    newStck.Visibility = Visibility.Hidden;
                    addStck.Visibility = Visibility.Hidden;
                    updStck.Visibility = Visibility.Visible;
                    nameUpd.Text = menu[MenuGrid.SelectedIndex].Name;
                    delName = nameUpd.Text;
                    priceUpd.Text = menu[MenuGrid.SelectedIndex].Price.ToString().Replace(',', '.');
                    pathUpd.Text = menu[MenuGrid.SelectedIndex].Path;
                    typeUpd.Text = menu[MenuGrid.SelectedIndex].Type;
                }
            }
            catch { }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MsgBox msgbox = new MsgBox("Are you sure you want to delete this item?");
                if ((bool)msgbox.ShowDialog())
                {
                    if (MenuGrid.SelectedItems != null)
                    {
                        for (int i = 0; i < MenuGrid.SelectedItems.Count; i++)
                        {
                            DataRowView datarowView = MenuGrid.SelectedItems[i] as DataRowView;
                            if (datarowView != null)
                            {
                                DataRow dataRow = (DataRow)datarowView.Row;
                                dataRow.Delete();
                            }
                        }
                    }
                    UpdateDB();
                }
                Window_Loaded();
            }
            catch { }
        }

        private void NewItem_Click(object sender, RoutedEventArgs e)
        {
            newStck.Visibility = Visibility.Hidden;
            addStck.Visibility = Visibility.Visible;
        }
        public void ClearBoxes()
        {
            newStck.Visibility = Visibility.Visible;
            addStck.Visibility = Visibility.Hidden;
            updStck.Visibility = Visibility.Hidden;
            nameAdd.BorderBrush = Brushes.Green;
            priceAdd.BorderBrush = Brushes.Green;
            pathAdd.BorderBrush = Brushes.Green;
            typeAdd.BorderBrush = Brushes.Green;
            nameAdd.Clear();
            priceAdd.Clear();
            pathAdd.Clear();
            typeAdd.Clear();
            nameUpd.BorderBrush = Brushes.Green;
            priceUpd.BorderBrush = Brushes.Green;
            pathUpd.BorderBrush = Brushes.Green;
            typeUpd.BorderBrush = Brushes.Green;
            nameUpd.Clear();
            priceUpd.Clear();
            pathUpd.Clear();
            typeUpd.Clear();
            errorAdd.Visibility = Visibility.Hidden;
            errorUpd.Visibility = Visibility.Hidden;
        }

        private void PathAdd_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == true)
            {
                pathAdd.Text = dlg.FileName;
            }
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ClearBoxes();
        }

        private void PathUpd_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == true)
            {
                pathUpd.Text = dlg.FileName;
            }
        }
    }

    
}
