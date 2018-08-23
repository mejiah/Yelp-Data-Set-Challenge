using Npgsql;
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

namespace Milestone1
{
    /// <summary>
    /// Interaction logic for BusinessPerZipWindow.xaml
    /// </summary>
    public partial class BusinessPerZipWindow : Window
    {
        public BusinessPerZipWindow(string state, string city)
        {
            InitializeComponent();
            columnChart(state, city);
        }

        private void columnChart(string state, string city)
        {

            List<KeyValuePair<string, int>> zipcodeData = new List<KeyValuePair<string, int>>();
            using (var conn = new NpgsqlConnection("Server=localhost; Database=yelpdb; Port=5433; Username=postgres; Password=Bix53z7h4m"))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT postal_code, COUNT(business_id) FROM businessTable WHERE state = '" + state + "' AND city = '" + city + "' GROUP BY postal_code ORDER BY postal_code;";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            zipcodeData.Add(new KeyValuePair<string, int>(reader.GetString(0), reader.GetInt32(1)));
                        }
                        zipcodeChart.DataContext = zipcodeData;
                    }
                }

                conn.Close();
            }
        }
    }
}
