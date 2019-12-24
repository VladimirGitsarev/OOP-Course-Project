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

namespace The_Coursach
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool check = false;
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = "Select Name, Surname, Password, Access, Id, Status from Staff order by Id desc";
            SqlDataReader thisReader = sqlCommand.ExecuteReader();
            while (thisReader.Read())
            {
                if ((thisReader.GetValue(0).ToString() + " " + thisReader.GetValue(1).ToString()) == lgn.Text && thisReader.GetValue(2).ToString() == psw.Password.GetHashCode().ToString() && thisReader.GetValue(5).ToString() != "Fired")
                {
                    check = true;
                    int access = (int)thisReader.GetValue(3);
                    Main main = new Main((int)thisReader.GetValue(4), thisReader.GetValue(0).ToString(), thisReader.GetValue(1).ToString(), access);
                    main.Show();
                    this.Close();
                }
            }
            if (check == false)
            {
                wrng.Visibility = Visibility.Visible;
            }
            //Main main = new Main(1, "Vladimir", "Gitsarev", 1);
            //main.Show();
            //this.Close();
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }


}
