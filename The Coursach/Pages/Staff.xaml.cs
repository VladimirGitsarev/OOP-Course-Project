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
using The_Coursach.Classes;

namespace The_Coursach
{
    /// <summary>
    /// Логика взаимодействия для Staff.xaml
    /// </summary>
    
    public partial class Staff : UserControl
    {
        public List<Staff0> staffs = new List<Staff0>();
        public Staff()
        {
            InitializeComponent();
            Window_Loaded();
        }
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        SqlDataAdapter adapter;
        DataTable StaffTable;
        private void Window_Loaded()
        {
            staffs.Clear();
            string sql = "SELECT Id, Name, Surname, Access, Status FROM Staff order by Access desc";
            StaffTable = new DataTable();
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);
                adapter.InsertCommand = new SqlCommand("sp_InsertStaff", connection);
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 0, "Id"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 50, "Name"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@surname", SqlDbType.NVarChar, 50, "Surname"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@access", SqlDbType.Int, 0, "Access"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@status", SqlDbType.NVarChar, 50, "Status"));
                connection.Open();
                adapter.Fill(StaffTable);
                StaffGrid.ItemsSource = StaffTable.DefaultView;

                SqlCommand command0 = new SqlCommand("Select * from Staff order by Access desc", connection);
                SqlDataReader reader = command0.ExecuteReader();
                while (reader.Read())
                    staffs.Add(new Staff0 { Name = reader.GetValue(1).ToString(), Surname = reader.GetValue(2).ToString(), Password = reader.GetValue(3).ToString(), Access = (int)reader.GetValue(4), Status = reader.GetValue(5).ToString() });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        private void UpdateDB()
        {
            SqlCommandBuilder comandbuilder = new SqlCommandBuilder(adapter);
            adapter.Update(StaffTable);
            StaffGrid.ItemsSource = StaffTable.DefaultView;
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (StaffGrid.SelectedItems != null)
                {
                    MsgBox msgbox = new MsgBox("Are you sure you want to delete this employee? All data will be deleted");
                    if ((bool)msgbox.ShowDialog())
                    {
                        if (StaffGrid.SelectedItems != null)
                        {
                            for (int i = 0; i < StaffGrid.SelectedItems.Count; i++)
                            {
                                DataRowView datarowView = StaffGrid.SelectedItems[i] as DataRowView;
                                if (datarowView != null)
                                {
                                    SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                                    String str = "Delete from Stats Where Surname =  '" + staffs[StaffGrid.SelectedIndex].Surname + "'";
                                    String str0 = "Delete from Orders Where Surname =  '" + staffs[StaffGrid.SelectedIndex].Surname + "'";
                                    sqlConnection.Open();
                                    SqlCommand cmd = new SqlCommand(str, sqlConnection);
                                    SqlCommand cmd0 = new SqlCommand(str0, sqlConnection);
                                    cmd.ExecuteNonQuery();
                                    cmd0.ExecuteNonQuery();
                                    DataRow dataRow = (DataRow)datarowView.Row;
                                    dataRow.Delete();
                                }
                            }
                        }
                        UpdateDB();
                    }
                    Window_Loaded();
                }
            }
            catch { }
        }

        private void NewUser_Click(object sender, RoutedEventArgs e)
        {
            newStck.Visibility = Visibility.Hidden;
            addStck.Visibility = Visibility.Visible;
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            bool correct = false;
            if (nameAdd.Text.Length == 0)
                nameAdd.BorderBrush = Brushes.Red;
            if (surnameAdd.Text.Length == 0)
                surnameAdd.BorderBrush = Brushes.Red;
            if (passwordAdd.Password.Length == 0)
            {
                passwordAdd.BorderBrush = Brushes.Red;
                passworConfirm.BorderBrush = Brushes.Red;
            }
            if (passwordAdd.Password != passworConfirm.Password)
                passworConfirm.BorderBrush = Brushes.Red;
            if (nameAdd.Text.Length != 0 && surnameAdd.Text.Length != 0
                && passwordAdd.Password.Length != 0 && passwordAdd.Password == passworConfirm.Password)
                correct = true;
            if (correct == true)
            {
                nameAdd.BorderBrush = Brushes.Green;
                surnameAdd.BorderBrush = Brushes.Green;
                passwordAdd.BorderBrush = Brushes.Green;
                passworConfirm.BorderBrush = Brushes.Green;
                int check = 0;
                if (IsManager.IsChecked == true)
                    check = 1;
                SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                String str = "Insert into Staff(Name, Surname, Password, Access, Status) values ('" + nameAdd.Text + "', '" + surnameAdd.Text + "', '" + passwordAdd.Password.GetHashCode() + "','" + check + "', 'Working')";
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(str, sqlConnection);
                cmd.ExecuteNonQuery();
                String str0 = "Insert into Stats(Name, Surname) values ('" + nameAdd.Text + "', '" + surnameAdd.Text + "')";
                SqlCommand cmd0 = new SqlCommand(str0, sqlConnection);
                cmd0.ExecuteNonQuery();
                Window_Loaded();
                newStck.Visibility = Visibility.Visible;
                addStck.Visibility = Visibility.Hidden;
                ClearBoxes();
            }
        }


        private void UpdButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (StaffGrid.SelectedItem != null)
                {
                    newStck.Visibility = Visibility.Hidden;
                    addStck.Visibility = Visibility.Hidden;
                    updStck.Visibility = Visibility.Visible;
                    nameUpd.Text = staffs[StaffGrid.SelectedIndex].Name;
                    surnameUpd.Text = staffs[StaffGrid.SelectedIndex].Surname;
                    //passwordUpd.Password = staffs[StaffGrid.SelectedIndex].Password;
                    if (staffs[StaffGrid.SelectedIndex].Access == 1)
                        IsManagerUpd.IsChecked = true;
                    else
                        IsManagerUpd.IsChecked = false;
                }
            }
            catch { }
        }

        private void SaveUser_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            bool correct = false;
            if (nameUpd.Text.Length == 0)
                nameUpd.BorderBrush = Brushes.Red;
            if (surnameUpd.Text.Length == 0)
                surnameUpd.BorderBrush = Brushes.Red;
            if (passwordUpd.Password.Length == 0)
            {
                passwordUpd.BorderBrush = Brushes.Red;
                passwordConfirmUpd.BorderBrush = Brushes.Red;
            }
            if (passwordUpd.Password != passwordConfirmUpd.Password)
                passwordConfirmUpd.BorderBrush = Brushes.Red;
            if (nameUpd.Text.Length != 0 && surnameUpd.Text.Length != 0
                && passwordUpd.Password.Length != 0 && passwordUpd.Password == passwordConfirmUpd.Password)
                correct = true;
            if (correct == true)
            {
                nameUpd.BorderBrush = Brushes.Green;
                surnameUpd.BorderBrush = Brushes.Green;
                passwordUpd.BorderBrush = Brushes.Green;
                passwordConfirmUpd.BorderBrush = Brushes.Green;
                int manager = 0;
                if (IsManagerUpd.IsChecked == true)
                    manager = 1;
                String str = "Update Staff Set Name = '" + nameUpd.Text + "', Surname = '" + surnameUpd.Text + "', Password = '" + passwordUpd.Password.GetHashCode() + "', Access = " + manager + " Where Surname =  '" + staffs[StaffGrid.SelectedIndex].Surname + "'";
                String str0 = "Update Stats Set Name = '" + nameUpd.Text + "', Surname = '" + surnameUpd.Text + "' Where Surname =  '" + staffs[StaffGrid.SelectedIndex].Surname + "'";
                String str1 = "Update Orders Set Name = '" + nameUpd.Text + "', Surname = '" + surnameUpd.Text + "' Where Surname =  '" + staffs[StaffGrid.SelectedIndex].Surname + "'";
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(str, sqlConnection);
                SqlCommand cmd0 = new SqlCommand(str0, sqlConnection);
                SqlCommand cmd1 = new SqlCommand(str1, sqlConnection);
                cmd.ExecuteNonQuery();
                cmd0.ExecuteNonQuery();
                cmd1.ExecuteNonQuery();
                Window_Loaded();
                newStck.Visibility = Visibility.Visible;
                addStck.Visibility = Visibility.Hidden;
                updStck.Visibility = Visibility.Hidden;
            }
        }

        private void StaffGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearBoxes();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ClearBoxes();
        }

        public void ClearBoxes()
        {
            newStck.Visibility = Visibility.Visible;
            addStck.Visibility = Visibility.Hidden;
            updStck.Visibility = Visibility.Hidden;
            nameAdd.BorderBrush = Brushes.Green;
            surnameAdd.BorderBrush = Brushes.Green;
            passwordAdd.BorderBrush = Brushes.Green;
            passworConfirm.BorderBrush = Brushes.Green;
            nameAdd.Clear();
            surnameAdd.Clear();
            passwordAdd.Clear();
            passworConfirm.Clear();
            nameUpd.BorderBrush = Brushes.Green;
            surnameUpd.BorderBrush = Brushes.Green;
            passwordUpd.BorderBrush = Brushes.Green;
            passwordConfirmUpd.BorderBrush = Brushes.Green;
            nameUpd.Clear();
            surnameUpd.Clear();
            passwordUpd.Clear();
            passwordConfirmUpd.Clear();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ClearBoxes();
        }

        private void LayoffButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (StaffGrid.SelectedItems != null)
                {
                    MsgBox msgbox = new MsgBox("Are you sure you want to fire this employee?");
                    if ((bool)msgbox.ShowDialog())
                    {
                        SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                        //MessageBox.Show(staffs[StaffGrid.SelectedIndex].Surname);
                        string str = "update Staff set Status = 'Fired' where Surname = '" + staffs[StaffGrid.SelectedIndex].Surname + "'";
                        sqlConnection.Open();
                        SqlCommand cmd = new SqlCommand(str, sqlConnection);
                        cmd.ExecuteNonQuery();
                        Window_Loaded();
                    }

                }
            }
            catch { }
        }
    }

}
    