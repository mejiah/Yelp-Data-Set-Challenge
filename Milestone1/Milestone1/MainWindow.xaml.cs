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
using Npgsql;

namespace Milestone1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Business
        {
            public Business (string name, string state, string city)
            {
                this.name = name;
                this.state = state;
                this.city = city;
            }
            public string name { get; set; }
            public string state { get; set; }
            public string city { get; set; }


        }
        public MainWindow()
        {
            InitializeComponent();
            AddStates();
            AddColumnsToGrid();
        }

        private string BuildConnString()
        {
            return "Server=localhost; Database=Milestone1DB; Port=5433; Username=postgres; Password=Bix53z7h4m";
        }

        public void AddStates()
        {
            using (var conn = new NpgsqlConnection(BuildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT DISTINCT state FROM business ORDER BY state;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            StateList.Items.Add(reader.GetString(0));
                        }
                    }
                }

                conn.Close();
            }
        }

        public void AddColumnsToGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Business Name";
            col1.Binding = new Binding("name");
            col1.Width = 250;
            BusinessGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "State";
            col2.Binding = new Binding("state");
            col2.Width = 50;
            BusinessGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "City";
            col3.Binding = new Binding("city");
            col3.Width = 170;
            BusinessGrid.Columns.Add(col3);
        }

        private void StateListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CityList.Items.Clear();

            using (var conn = new NpgsqlConnection(BuildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT DISTINCT city FROM business WHERE state = '" + StateList.SelectedItem.ToString() + "' ORDER BY city;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CityList.Items.Add(reader.GetString(0));
                        }
                    }
                }

                conn.Close();
            }

        }

        private void CityListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CityList.SelectedItem != null)
            {
                BusinessGrid.Items.Clear();
                using (var conn = new NpgsqlConnection(BuildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT name FROM business WHERE city = '" + CityList.SelectedItem.ToString() + "' AND state = '" + StateList.SelectedItem.ToString() + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BusinessGrid.Items.Add(new Business(reader.GetString(0), StateList.SelectedItem.ToString(), CityList.SelectedItem.ToString()));
                            }
                        }
                    }

                    conn.Close();
                }

            }
        }
    }
}
