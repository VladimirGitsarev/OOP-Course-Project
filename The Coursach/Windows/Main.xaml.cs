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
using The_Coursach.Classes;


namespace The_Coursach
{

    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        Editor editor = new Editor();
        Staff staff = new Staff();
        
        public Main(int Id, string Name, string Surname, int access)
        {
            InitializeComponent();
            Order order = new Order(Id, Name, Surname);
            Statistics stats = new Statistics(Id, access);
            Welcome.Text += Name;
            ordrWnd.Children.Add(order);
            curWnd.Children.Add(editor);
            curWnd.Children.Add(staff);
            statsWnd.Children.Add(stats);
            order.Visibility = Visibility.Visible;
            editor.Visibility = Visibility.Hidden;
            staff.Visibility = Visibility.Hidden;
            stats.Visibility = Visibility.Visible;
            waiterName.Text = Name + " " + Surname;
            timeValue.Text = DateTime.Now.ToString();
            if (access == 1)
                person.Text = "Manager: ";
            else
            {
                person.Text = "Waiter: ";
                Editor.IsEnabled = false;
                Staff.IsEnabled = false;
            }

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            MsgBox msgbox = new MsgBox("Do you want to exit? All usaved data will be deleted");
            if ((bool)msgbox.ShowDialog())
            {
                this.Close();
            }
        }

        private void Orders_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Start.Visibility = Visibility.Hidden;
            switch (btn.Content.ToString())
            {
                case "Orders":
                    //order.Visibility = Visibility.Visible;
                    ordrWnd.Visibility = Visibility.Visible;
                    editor.Visibility = Visibility.Hidden;
                    staff.Visibility = Visibility.Hidden;
                    statsWnd.Visibility = Visibility.Hidden;
                    Orders.BorderThickness = new Thickness(3, 0, 0, 0);
                    Orders.BorderBrush = Brushes.DarkGreen;
                    Editor.BorderThickness = new Thickness(0);
                    Staff.BorderThickness = new Thickness(0);
                    Stats.BorderThickness = new Thickness(0);
                    break;
                case "Menu Editor":
                    //order.Visibility = Visibility.Hidden;
                    ordrWnd.Visibility = Visibility.Hidden;
                    curWnd.Visibility = Visibility.Visible;
                    editor.Visibility = Visibility.Visible;
                    staff.Visibility = Visibility.Hidden;
                    statsWnd.Visibility = Visibility.Hidden;
                    Editor.BorderThickness = new Thickness(3, 0, 0, 0);
                    Editor.BorderBrush = Brushes.DarkGreen;
                    Orders.BorderThickness = new Thickness(0);
                    Staff.BorderThickness = new Thickness(0);
                    Stats.BorderThickness = new Thickness(0);
                    break;
                case "Staff":
                    //order.Visibility = Visibility.Hidden;
                    ordrWnd.Visibility = Visibility.Hidden;
                    curWnd.Visibility = Visibility.Visible;
                    editor.Visibility = Visibility.Hidden;
                    staff.Visibility = Visibility.Visible;
                    statsWnd.Visibility = Visibility.Hidden;
                    Staff.BorderThickness = new Thickness(3, 0, 0, 0);
                    Staff.BorderBrush = Brushes.DarkGreen;
                    Orders.BorderThickness = new Thickness(0);
                    Editor.BorderThickness = new Thickness(0);
                    Stats.BorderThickness = new Thickness(0);
                    break;
                case "Statistics":
                    //order.Visibility = Visibility.Hidden;
                    ordrWnd.Visibility = Visibility.Hidden;
                    curWnd.Visibility = Visibility.Visible;
                    editor.Visibility = Visibility.Hidden;
                    staff.Visibility = Visibility.Hidden;
                    statsWnd.Visibility = Visibility.Visible;
                    Stats.BorderThickness = new Thickness(3, 0, 0, 0);
                    Stats.BorderBrush = Brushes.DarkGreen;
                    Orders.BorderThickness = new Thickness(0);
                    Editor.BorderThickness = new Thickness(0);
                    Staff.BorderThickness = new Thickness(0);
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MsgBox msgbox = new MsgBox("Do you want to logout? All unsaved data will be deleted");
            if ((bool)msgbox.ShowDialog())
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }
    }

    
}          

