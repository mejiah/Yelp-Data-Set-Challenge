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
    /// Interaction logic for CheckInWindow.xaml
    /// </summary>
    public partial class CheckInWindow : Window
    {
        public CheckInWindow(string businessId)
        {
            InitializeComponent();
            columnChart(businessId);
        }

        private void columnChart(string businessId)
        {

            List<KeyValuePair<string, int>> checkInChartData = new List<KeyValuePair<string, int>>();
            using (var conn = new NpgsqlConnection("Server=localhost; Database=yelpdb; Port=5433; Username=postgres; Password=Bix53z7h4m"))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM checkintable WHERE business_id = '" + businessId + "';";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            checkInChartData.Add(new KeyValuePair<string, int>(reader.GetString(1), reader.GetInt32(2) + reader.GetInt32(3) + reader.GetInt32(4) + reader.GetInt32(5)));
                        }
                        checkInChart.DataContext = checkInChartData;
                    }
                }

                conn.Close();
            }
        }
    }
}
