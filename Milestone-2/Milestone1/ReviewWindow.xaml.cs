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
using Npgsql;

namespace Milestone1
{
    /// <summary>
    /// Interaction logic for ReviewWindow.xaml
    /// </summary>
    public partial class ReviewWindow : Window
    {
        public string businessName;
        public string address;

        public ReviewWindow(string businessName, string address)
        {
            this.businessName = businessName;
            this.address = address; 
            InitializeComponent();
            AddColumnsToGrid();
            loadGrid();
        }

        public class Review
        {
            public Review(string date, string username, string stars, string text, string funny, string useful, string cool)
            {
                string finalText = "";
                this.date = date;
                this.username = username;
                this.stars = stars;
                var textStrings = text.Split(' ');
                for(int i = 0; i < textStrings.Count<string>(); i++)
                {
                    finalText += textStrings[i] + " ";
                    if((i+1)%20==0)
                    {
                        finalText += "\n";
                    }

                }
                this.text = finalText;
                this.funny = funny;
                this.useful = useful;
                this.cool = cool;
            }

            public string date { get; set; }
            public string username { get; set; }
            public string stars { get; set; }
            public string text { get; set; }
            public string funny { get; set; }
            public string useful { get; set; }
            public string cool { get; set; }


        }


        private string BuildConnString()
        {
            //return "Server=localhost; Database=yelpdb; Port=5433; Username=postgres; Password=Bix53z7h4m";
            return "Server=localhost; Database=yelpdb; Port=5433; Username=postgres; Password=Bix53z7h4m";
        }

        public void AddColumnsToGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Date";
            col1.Binding = new Binding("date");
            reviewGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "User Name";
            col2.Binding = new Binding("username");
            reviewGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "Stars";
            col3.Binding = new Binding("stars");
            reviewGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "Text";
            col4.Binding = new Binding("text");
            //col4.Width = 50;
            reviewGrid.Columns.Add(col4);

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Header = "Funny";
            col5.Binding = new Binding("funny");
            reviewGrid.Columns.Add(col5);

            DataGridTextColumn col6 = new DataGridTextColumn();
            col6.Header = "Useful";
            col6.Binding = new Binding("useful");
            reviewGrid.Columns.Add(col6);

            DataGridTextColumn col7 = new DataGridTextColumn();
            col7.Header = "Cool";
            col7.Binding = new Binding("cool");
            reviewGrid.Columns.Add(col7);


        }
        public void loadGrid()
        {
            string business_id = "";
            using (var conn = new NpgsqlConnection(BuildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT business_id from businesstable WHERE name = '" + businessName + "' AND address = '" + address + "'";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            business_id = reader.GetString(0);
                        }
                    }
                }


                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT r.date, u.name, r.stars, r.text, r.funny, r.useful, r.cool " +
                                      "FROM reviewtable AS r, usertable AS u " + 
                                      "WHERE r.business_id = '" + business_id + "' AND u.user_id=r.user_id " + 
                                      "ORDER BY u.name";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reviewGrid.Items.Add(new Review(reader.GetString(0), reader.GetString(1), reader.GetDouble(2).ToString(), reader.GetString(3), reader.GetDouble(4).ToString(), reader.GetDouble(5).ToString(), reader.GetDouble(5).ToString()));
                        }
                    }
                }

                conn.Close();
            }
        }
    }
}
