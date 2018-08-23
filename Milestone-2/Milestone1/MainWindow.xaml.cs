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
using System.Device.Location; // Add a reference to System.Device.dll
using System.Data;
using System.ComponentModel;

namespace Milestone1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CLocation myLocation;
        public List<string> Attributes = new List<string>();
        public List<string> Prices = new List<string>();
        public Business selectedBusiness;

        string id;
        //string friendId;

        public class Business
        {
            public Business(string business_id, string name, string address, string state, string city, string distance, string stars, string reviewCount, string avgReview, string numCheckins)
            {
                this.business_id = business_id;
                this.name = name;
                this.address = address;
                this.state = state;
                this.city = city;
                this.distance = distance;
                this.stars = stars;
                this.reviewCount = reviewCount;
                this.avgReview = avgReview;
                this.numCheckins = numCheckins;
            }

            public string business_id { get; set; }
            public string name { get; set; }
            public string address { get; set; }
            public string state { get; set; }
            public string city { get; set; }
            public string distance { get; set; }
            public string stars { get; set; }
            public string reviewCount { get; set; }
            public string avgReview { get; set; }
            public string numCheckins { get; set; }

        }


        public class User
        {
            public User(string name, string user_id, string yelping_since, int fans, double average_stars, int funny, int useful, int cool)
            {
                this.name = name;
                this.user_id = user_id;
                this.yelping_since = yelping_since;
                this.fans = fans;
                this.average_stars = average_stars;
                this.funny = funny;
                this.useful = useful;
                this.cool = cool;

            }

            public string name { get; set; }
            public string user_id { get; set; }
            public string yelping_since { get; set; }

            public int fans { get; set; }
            public double average_stars { get; set; }
            public int funny { get; set; }
            public int useful { get; set; }
            public int cool { get; set; }

        }

        public class Friend
        {
            public Friend(string name, string avgStar, string yelpSince, string friend_id)
            {
                this.name = name;
                this.avgStar = avgStar;
                this.yelpSince = yelpSince;
                this.friend_id = friend_id;
            }
            public string name { get; set; }
            public string avgStar { get; set; }
            public string yelpSince { get; set; }
            public string friend_id { get; set; }
        }

        public class FriendReview
        {
            public FriendReview(string name, string businessName, string city, string text)
            {
                this.name = name;
                this.businessName = businessName;
                this.city = city;
                this.text = text;
            }

            public string name { get; set; }
            public string businessName { get; set; }
            public string city { get; set; }
            public string text { get; set; }
        }

        public class CLocation
        {
            GeoCoordinateWatcher watcher;
            EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>> e;
            public GeoCoordinate location;

            public CLocation()
            {
                e = new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
            }

            public void GetLocationEvent()
            {
                this.watcher = new GeoCoordinateWatcher();
                this.watcher.PositionChanged += e;
                bool started = this.watcher.TryStart(false, TimeSpan.FromMilliseconds(2000));
                if (!started)
                {
                    Console.WriteLine("GeoCoordinateWatcher timed out on start.");
                }
            }

            void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
            {
                this.location = e.Position.Location;
                this.watcher.PositionChanged -= this.e;
            }

            void PrintPosition(double Latitude, double Longitude)
            {
                Console.WriteLine("Latitude: {0}, Longitude {1}", Latitude, Longitude);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            AddStates();
            AddColumnsToGrid();
            AddColumnsToFriendsGrid();
            AddColumnsToFriendsTipsGrid();
            addDays();
            addTimes();
            addSorts();
            addRatings();
            SearchButton.Background = Brushes.LightGray;

            myLocation = new CLocation();
            myLocation.GetLocationEvent();
            Console.WriteLine("Enter any key to quit.");
            Console.ReadLine();
            checkinButton.IsEnabled = false;
            showReviewsButton.IsEnabled = false;
            numberOfBusinessesButton.IsEnabled = false;
            showCheckinsButton.IsEnabled = false;
            addReviewButton.IsEnabled = false;
            SearchButton.IsEnabled = false;


        }
        private void addDays()
        {
            string[] days = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            dayOfWeekComboBox.ItemsSource = days;
        }

        private void addRatings()
        {
            string[] ratings = new string[] { "1", "2", "3", "4", "5" };
            ratingComboBox.ItemsSource = ratings;
            ratingComboBox.IsEnabled = false;
            ratingComboBox.SelectedIndex = 0;
        }

        private void addSorts()
        {
            sortComboBox.DisplayMemberPath = "Key";
            sortComboBox.SelectedValuePath = "Value";

            sortComboBox.Items.Add(new KeyValuePair<string, string>("Business name (default)", "name"));
            sortComboBox.Items.Add(new KeyValuePair<string, string>("Highest rating (stars)", "stars"));
            sortComboBox.Items.Add(new KeyValuePair<string, string>("Most reviewed", "review_count"));
            sortComboBox.Items.Add(new KeyValuePair<string, string>("Best review rating", "reviewrating"));
            sortComboBox.Items.Add(new KeyValuePair<string, string>("Most check-ins", "numcheckins"));
            sortComboBox.Items.Add(new KeyValuePair<string, string>("Nearest", "distance"));

            sortComboBox.SelectedIndex = 0;
        }
        private void addTimes()
        {
            fromComboBox.DisplayMemberPath = "Key";
            fromComboBox.SelectedValuePath = "Value";
            toComboBox.DisplayMemberPath = "Key";
            toComboBox.SelectedValuePath = "Value";
            float time;
            string timeString;
            for (int i = 0; i <= 48; i++)
            {
                time = (float)i / 2;
                timeString = (i / 2).ToString() + ":";
                if (i % 2 == 0)
                {
                    timeString += "00";
                }
                else
                {
                    timeString += "30";
                }
                fromComboBox.Items.Add(new KeyValuePair<string, float>(timeString, time));
                toComboBox.Items.Add(new KeyValuePair<string, float>(timeString, time));
            }
        }

        private string BuildConnString()
        {
            //return "Server=localhost; Database=yelpdb; Port=5433; Username=postgres; Password=Bix53z7h4m";
            return "Server=localhost; Database=yelpdb; Port=5433; Username=postgres; Password=Bix53z7h4m";
        }

        public void AddStates()
        {
            using (var conn = new NpgsqlConnection(BuildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT DISTINCT state FROM businessTable ORDER BY state;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
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
            BusinessGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Address";
            col2.Binding = new Binding("address");
            BusinessGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "City";
            col3.Binding = new Binding("city");
            BusinessGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "State";
            col4.Binding = new Binding("state");
            //col4.Width = 50;
            BusinessGrid.Columns.Add(col4);

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Header = "Distance\n(miles)";
            col5.Binding = new Binding("distance");
            BusinessGrid.Columns.Add(col5);

            DataGridTextColumn col6 = new DataGridTextColumn();
            col6.Header = "Stars";
            col6.Binding = new Binding("stars");
            BusinessGrid.Columns.Add(col6);

            DataGridTextColumn col7 = new DataGridTextColumn();
            col7.Header = "# of\nReviews";
            col7.Binding = new Binding("reviewCount");
            BusinessGrid.Columns.Add(col7);

            DataGridTextColumn col8 = new DataGridTextColumn();
            col8.Header = "Avg\nReview\nRating";
            col8.Binding = new Binding("avgReview");
            BusinessGrid.Columns.Add(col8);

            DataGridTextColumn col9 = new DataGridTextColumn();
            col9.Header = "Total\nCheckins";
            col9.Binding = new Binding("numCheckins");
            BusinessGrid.Columns.Add(col9);
        }




        public void getUserIds(string userName)
        {
            using (var conn = new NpgsqlConnection(BuildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT user_id,name,average_stars,fans,yelping_since,funny,useful,cool FROM userTable WHERE name = '" + userName + "';";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userIds.Items.Add(reader.GetString(0));

                        }
                    }
                }

                conn.Close();
            }
        }

        private void searchUserIdButton_Click(object sender, RoutedEventArgs e)
        {
            getUserIds(searchName.Text);

        }

        public void getUserFriends(string id)
        {
            using (var conn = new NpgsqlConnection(BuildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT temp.name, temp.average_stars, temp.yelping_since, F.friend_id FROM userTable as U, friendsTable as F, (SELECT * FROM userTable) as temp WHERE U.user_id = '" + id + "' AND U.user_id = F.user_id AND F.friend_id = temp.user_id ORDER BY temp.name;";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userFriendsList.Items.Add(new Friend(reader.GetString(0), reader.GetDouble(1).ToString(), reader.GetString(2), reader.GetString(3)));
                        }
                    }
                }

                conn.Close();
            }
        }

        public void AddColumnsToFriendsGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Name";
            col1.Binding = new Binding("name");
            col1.Width = 140;
            userFriendsList.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Avg Stars";
            col2.Binding = new Binding("avgStar");
            col2.Width = 80;
            userFriendsList.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "Yelping Since";
            col3.Binding = new Binding("yelpSince");
            col3.Width = 150;
            userFriendsList.Columns.Add(col3);
        }

        private void userIds_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            id = userIds.SelectedValue.ToString();

            getUserInformation(id);

            userFriendsList.Items.Clear();
            getUserFriends(id);

            userFriendTipsGrid.Items.Clear();
            getUserFriendsReviews(id);
        }



        public void AddColumnsToFriendsTipsGrid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "User Name";
            col1.Binding = new Binding("name");
            col1.Width = 80;
            userFriendTipsGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "Business";
            col2.Binding = new Binding("businessName");
            col2.Width = 140;
            userFriendTipsGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "City";
            col3.Binding = new Binding("city");
            col3.Width = 80;
            userFriendTipsGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "Text";
            col4.Binding = new Binding("text");
            col4.Width = 380;
            userFriendTipsGrid.Columns.Add(col4);

        }

        public void getUserInformation(string userId)
        {
            using (var conn = new NpgsqlConnection(BuildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT user_id,name,average_stars,fans,yelping_since,funny,useful,cool FROM userTable WHERE user_id = '" + userId + "';";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userNameTextBox.Text = reader.GetString(1);
                            userStarsTextBox.Text = reader.GetDouble(2).ToString();
                            userFansTextBox.Text = reader.GetDouble(3).ToString();
                            userYelpSinceTextBox.Text = reader.GetString(4);
                            funnyTextBox.Text = reader.GetDouble(5).ToString();
                            coolTextBox.Text = reader.GetDouble(6).ToString();
                            usefulTextBox.Text = reader.GetDouble(7).ToString();
                            latitudeTextBox.Text = myLocation.location.Latitude.ToString();
                            longitudeTextBox.Text = myLocation.location.Longitude.ToString();

                        }
                    }
                }

                conn.Close();
            }
        }

        public void getUserFriendsReviews(string id)
        {
            using (var conn = new NpgsqlConnection(BuildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT temp.name, B.name as businessName, B.city, temp.text " +
                                       "FROM businessTable as B, (SELECT temp.name, R.user_id, R.business_id, R.text " +
                                                                 "FROM reviewTable as R, (SELECT temp.user_id, temp.name, temp.average_stars, temp.yelping_since " +
                                                                                         "FROM userTable as U, friendsTable as F, (SELECT * FROM userTable) as temp " +
                                                                                         "WHERE U.user_id = '" + id + "' AND U.user_id = F.user_id AND F.friend_id = temp.user_id) as temp " +
                                                                 "WHERE R.user_id = temp.user_id) as temp " +
                                       "WHERE B.business_id = temp.business_id " +
                                       "ORDER BY temp.name;";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userFriendTipsGrid.Items.Add(new FriendReview(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)));
                        }
                    }
                }

                conn.Close();
            }
        }

        /////////////////////////// Business Table //////////////////////////////////
        private void StateListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CityList.Items.Clear();
            ZipList.Items.Clear();
            CategoryList.Items.Clear();
            SelectedCategoriesList.Items.Clear();
            numberOfBusinessesButton.IsEnabled = false;
            SearchButton.IsEnabled = false;


            using (var conn = new NpgsqlConnection(BuildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT DISTINCT city FROM businessTable WHERE state = '" + StateList.SelectedItem.ToString() + "' ORDER BY city;";
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

        private void CityList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ZipList.Items.Clear();
            CategoryList.Items.Clear();
            SelectedCategoriesList.Items.Clear();
            numberOfBusinessesButton.IsEnabled = true;
            SearchButton.IsEnabled = false;


            if (CityList.SelectedItem != null)
            {
                ZipList.Items.Clear();
                using (var conn = new NpgsqlConnection(BuildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT DISTINCT postal_code FROM businessTable WHERE city = '" + CityList.SelectedItem.ToString() + "' AND state = '" + StateList.SelectedItem.ToString() + "' ORDER BY postal_code;";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ZipList.Items.Add(reader.GetString(0));
                            }
                        }
                    }

                    conn.Close();
                }
            }
        }

        private void ZipList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CategoryList.Items.Clear();
            SelectedCategoriesList.Items.Clear();
            SearchButton.IsEnabled = true;

            if (ZipList.SelectedItem != null)
            {
                CategoryList.Items.Clear();
                using (var conn = new NpgsqlConnection(BuildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT DISTINCT category_type " +
                                          "FROM categoriesTable, " +
                                          "(SELECT business_id FROM businessTable WHERE state = '" + StateList.SelectedItem.ToString() + "' AND city = '" + CityList.SelectedItem.ToString() + "' AND postal_code = " + ZipList.SelectedItem.ToString() + ") " +
                                          "AS temp " +
                                          "WHERE temp.business_id = categoriesTable.business_id;";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CategoryList.Items.Add(reader.GetString(0));
                            }
                        }
                    }

                    /*
                    BusinessGrid.Items.Clear();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT * FROM businessTable WHERE city = '" + CityList.SelectedItem.ToString() + "' AND state = '" + StateList.SelectedItem.ToString() + "' AND postal_code = '" + ZipList.SelectedItem.ToString() + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //BusinessGrid.Items.Add(new Business(reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), distance, reader.GetDouble(8).ToString(), reader.GetString(9), reader.GetDouble(12).ToString(), reader.GetString(11)));
                            }
                        }
                    }
                    */

                    numberOfBusinessesLabel.Content = "# of Businesses: " + BusinessGrid.Items.Count.ToString();
                    conn.Close();
                }
            }
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryList.SelectedItem != null && !SelectedCategoriesList.Items.Contains(CategoryList.SelectedItem.ToString()))
            {
                SelectedCategoriesList.Items.Add(CategoryList.SelectedItem.ToString());
            }

        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCategoriesList.SelectedItem != null)
            {
                SelectedCategoriesList.Items.Remove(SelectedCategoriesList.SelectedItem.ToString());
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            CheckAttributeCheckboxes();
            bool FilterApplied = false;

            
            using (var conn = new NpgsqlConnection(BuildConnString()))
            {
                conn.Open();
                BusinessGrid.Items.Clear();
                using (var cmd = new NpgsqlCommand())
                {
                    double latitude, longitude;
                    cmd.Connection = conn;


                    cmd.CommandText = "SELECT * " +
                                      "FROM " +
                                      "(SELECT * FROM businessTable WHERE state = '" + StateList.SelectedItem.ToString() + "' AND city = '" + CityList.SelectedItem.ToString() + "' AND postal_code = " + ZipList.SelectedItem.ToString() + ") " +
                                      "AS b ";
                    

                    if (SelectedCategoriesList.Items.Count > 0)
                    {
                        FilterApplied = true;
                        cmd.CommandText += "WHERE b IN(" +
                                              "SELECT b " +
                                              "FROM categoriesTable " +
                                              "WHERE b.business_id = categoriesTable.business_id AND categoriesTable.category_type = '" + SelectedCategoriesList.Items.GetItemAt(0).ToString() + "'";

                        for (int i = 1; i < SelectedCategoriesList.Items.Count; i++)
                        {
                            cmd.CommandText += "AND b IN(" +
                                               "SELECT b " +
                                               "FROM categoriesTable " +
                                               "WHERE b.business_id = categoriesTable.business_id AND categoriesTable.category_type = '" + SelectedCategoriesList.Items.GetItemAt(i).ToString() + "'";
                        }
                    }

                    if (dayOfWeekComboBox.SelectedItem != null && fromComboBox.SelectedItem != null && toComboBox.SelectedItem != null)
                    {
                        if (FilterApplied)
                        {

                            cmd.CommandText += "AND b IN (" +
                                               "SELECT b " +
                                               "FROM businessTimesTable " +
                                               "WHERE b.business_id = businessTimesTable.business_id AND businessTimesTable.day = '" + dayOfWeekComboBox.SelectedItem.ToString() + "'" +
                                               "AND(businessTimesTable.open <= " + ((KeyValuePair<string, float>)fromComboBox.SelectedItem).Value.ToString() + ") AND(businessTimesTable.close >= " + ((KeyValuePair<string, float>)toComboBox.SelectedItem).Value.ToString() + " OR businessTimesTable.close = 0))";
                        }

                        else
                        {
                            FilterApplied = true;
                            cmd.CommandText += "WHERE b IN (" +
                                               "SELECT b " +
                                               "FROM businessTimesTable " +
                                               "WHERE b.business_id = businessTimesTable.business_id AND businessTimesTable.day = '" + dayOfWeekComboBox.SelectedItem.ToString() + "'" +
                                               "AND(businessTimesTable.open <= " + ((KeyValuePair<string, float>)fromComboBox.SelectedItem).Value.ToString() + ") AND(businessTimesTable.close >= " + ((KeyValuePair<string, float>)toComboBox.SelectedItem).Value.ToString() + " OR businessTimesTable.close = 0))";
                        }
                    }

                    if (Attributes.Count > 0)
                    {
                        if (!FilterApplied)
                        {
                            cmd.CommandText += "WHERE b IN ( " +
                                               "SELECT b " +
                                               "FROM attributesTable AS a " +
                                               "WHERE a.business_id = b.business_id AND a.attribute_type = '" + Attributes[0] + "' AND a.attribute_value = 'True' ";
                            FilterApplied = true;
                        }
                        else
                        {
                            cmd.CommandText += "AND b IN ( " +
                                               "SELECT b " +
                                               "FROM attributesTable AS a " +
                                               "WHERE a.business_id = b.business_id AND a.attribute_type = '" + Attributes[0] + "' AND a.attribute_value = 'True' ";
                        }

                        for (int i = 1; i < Attributes.Count; i++)
                        {
                            cmd.CommandText += "AND b IN ( " +
                                               "SELECT b " +
                                               "FROM attributesTable AS a " +
                                               "WHERE a.business_id = b.business_id AND a.attribute_type = '" + Attributes[i] + "' AND a.attribute_value = 'True' ";
                        }
                    }

                    if (Prices.Count > 0)
                    {
                        if (!FilterApplied)
                        {
                            cmd.CommandText += "WHERE b IN ( " +
                                               "SELECT b " +
                                               "FROM attributesTable AS a " +
                                               "WHERE a.business_id = b.business_id AND a.attribute_type = 'RestaurantsPriceRange2' AND a.attribute_value = '" + Prices[0] + "' ";
                        }

                        else
                        {
                            cmd.CommandText += "AND b IN ( " +
                                               "SELECT b " +
                                               "FROM attributesTable AS a " +
                                               "WHERE a.business_id = b.business_id AND a.attribute_type = 'RestaurantsPriceRange2' AND a.attribute_value = '" + Prices[0] + "' ";
                        }


                        for (int i = 1; i < Prices.Count; i++)
                        {
                            cmd.CommandText += "AND b IN ( " +
                                               "SELECT b " +
                                               "FROM attributesTable AS a " +
                                               "WHERE a.business_id = b.business_id AND a.attribute_type = 'RestaurantsPriceRange2' AND a.attribute_value = '" + Prices[i] + "' ";
                        }
                    }

                    for (int j = 0; j < (SelectedCategoriesList.Items.Count + Attributes.Count + Prices.Count); j++)
                    {
                        cmd.CommandText += ")";
                    }

                    string sortBy = ((KeyValuePair<string, string>)sortComboBox.SelectedItem).Value.ToString();

                    if (sortComboBox.SelectedItem != null && sortBy!="distance")
                    {
                        cmd.CommandText += " ORDER BY " + sortBy;
                    }

                    //descending order for all sorts besides name and distance
                    if (sortBy != "name" && sortBy != "distance")
                    {
                        cmd.CommandText += " DESC";
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            latitude = reader.GetDouble(6);
                            longitude = reader.GetDouble(7);
                            var avgReview = reader.GetDouble(12);
                            string avgReviewString = string.Format("{0:F2}", avgReview);
                            var businessLocation = new GeoCoordinate(latitude, longitude);
                            var distance = (this.myLocation.location.GetDistanceTo(businessLocation) * 0.00062137119223733);
                            var distanceString = string.Format("{0:F2}", distance);
                            BusinessGrid.Items.Add(new Business(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), distanceString, reader.GetDouble(8).ToString(), reader.GetString(9), avgReviewString, reader.GetString(11)));

                        }
                    }

                    //have to sort by distance programatically because distance is a calculated value based on coordinates from the query (can't query the sort)
                    if (sortBy == "distance")
                    {
                        BusinessGrid.Items.SortDescriptions.Add(new SortDescription("distance", ListSortDirection.Ascending));
                    }

                }
                numberOfBusinessesLabel.Content = "# of Businesses: " + BusinessGrid.Items.Count.ToString();
                conn.Close();
            }
        }

        private void CheckAttributeCheckboxes()
        {
            Attributes.Clear();
            Prices.Clear();

            //prices
            if (oneDollar.IsChecked == true)
            {
                Prices.Add("1");
            }
            if (twoDollar.IsChecked == true)
            {
                Prices.Add("2");
            }
            if (threeDollar.IsChecked == true)
            {
                Prices.Add("3");
            }
            if (fourDollar.IsChecked == true)
            {
                Prices.Add("4");
            }

            //meal types
            if (breakfastCheckBox.IsChecked == true)
            {
                Attributes.Add("breakfast");
            }

            if (brunchCheckBox.IsChecked == true)
            {
                Attributes.Add("brunch");
            }
            if (lunchCheckbox.IsChecked == true)
            {
                Attributes.Add("lunch");
            }
            if (dinnerCheckBox.IsChecked == true)
            {
                Attributes.Add("dinner");
            }
            if (dessertrCheckBox.IsChecked == true)
            {
                Attributes.Add("dessert");
            }
            if (lateNightCheckBox.IsChecked == true)
            {
                Attributes.Add("latenight");
            }

            //misc attributes    
            if (creditCardCheckBox.IsChecked == true)
            {
                Attributes.Add("BusinessAcceptsCreditCards");
            }
            if (reservationCheckBox.IsChecked == true)
            {
                Attributes.Add("RestaurantsReservations");
            }
            if (wheelchairAccesibleCheckBox.IsChecked == true)
            {
                Attributes.Add("WheelchairAccessible");
            }
            if (outdoorCheckBox.IsChecked == true)
            {
                Attributes.Add("OutdoorSeating");
            }
            if (goodForKidsCheckBox.IsChecked == true)
            {
                Attributes.Add("GoodForKids");
            }
            if (goodForGroupsCheckBox.IsChecked == true)
            {
                Attributes.Add("RestaurantsGoodForGroups");
            }
            if (deliveryCheckBox.IsChecked == true)
            {
                Attributes.Add("RestaurantsDelivery");
            }
            if (takeOutCheckBox.IsChecked == true)
            {
                Attributes.Add("RestaurantsTakeOut");
            }
            if (wifiCheckBox.IsChecked == true)
            {
                Attributes.Add("WiFi");
            }
            if (bikeParkingCheckBox.IsChecked == true)
            {
                Attributes.Add("BikeParking");
            }

        }

        private void SearchButton_MouseEnter(object sender, MouseEventArgs e)
        {
        }

        private void SearchButton_MouseLeave(object sender, MouseEventArgs e)
        {
            SearchButton.Background = Brushes.LightGray;
        }

        private void checkinButton_Click(object sender, RoutedEventArgs e)
        {
            if (businessTextBox.Text != "Business Name")
            {

                var time = DateTime.Now.TimeOfDay;
                var splitTime = time.ToString().Split(':');
                var hour = Int32.Parse(splitTime[0]);

                DateTime ClockInfoFromSystem = DateTime.Now;
                var dayOfWeek = (int)ClockInfoFromSystem.DayOfWeek;
                string day = "";
                string timeOfDay = "";
                string business_id = "";
                switch (dayOfWeek)
                {
                    case 0:
                        day = "Monday";
                        break;
                    case 1:
                        day = "Tuesday";
                        break;
                    case 2:
                        day = "Wednesday";
                        break;
                    case 3:
                        day = "Thursday";
                        break;
                    case 4:
                        day = "Friday";
                        break;
                    case 5:
                        day = "Saturday";
                        break;
                    case 6:
                        day = "Sunday";
                        break;
                }

                if (6 <= hour && hour < 12)
                {
                    timeOfDay = "morning";
                }
                if (12 <= hour && hour < 17)
                {
                    timeOfDay = "afternoon";
                }

                if (17 <= hour && hour < 23)
                {
                    timeOfDay = "evening";
                }
                if (6 > hour || hour >= 23)
                {
                    timeOfDay = "night";
                }

                using (var conn = new NpgsqlConnection(BuildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT business_id from businesstable WHERE name = '" + selectedBusiness.name + "' AND address = '" + selectedBusiness.address + "'";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                business_id = reader.GetString(0);
                            }
                        }
                    }

                    conn.Close();
                }

                using (var conn = new NpgsqlConnection(BuildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "UPDATE checkintable " +
                                          "SET " + timeOfDay + " = " + timeOfDay + " + 1 " +
                                          "WHERE business_id = '" + business_id + "' AND day = '" + day + "'";
                        using (var reader = cmd.ExecuteReader())
                        {

                        }
                    }

                    conn.Close();
                }

            }
        }

        private void BusinessGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (BusinessGrid.SelectedItems.Count > 0)
            {
                selectedBusiness = (Business)BusinessGrid.SelectedItems[0];
                businessTextBox.Text = selectedBusiness.name;
                checkinButton.IsEnabled = true;
                showReviewsButton.IsEnabled = true;
                showCheckinsButton.IsEnabled = true;
                addReviewButton.IsEnabled = true;
                ratingComboBox.IsEnabled = true;
            }

            else
            {
                showReviewsButton.IsEnabled = false;
                checkinButton.IsEnabled = false;
                showCheckinsButton.IsEnabled = false;
                addReviewButton.IsEnabled = false;
                ratingComboBox.IsEnabled = false;
                businessTextBox.Text = "Business Name";
            }
        }

        private void showReviewsButton_Click(object sender, RoutedEventArgs e)
        {
            ReviewWindow window = new ReviewWindow(selectedBusiness.name, selectedBusiness.address);
            window.Show();
        }

        private void numberOfBusinessesButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = new BusinessPerZipWindow(StateList.SelectedValue.ToString(), CityList.SelectedValue.ToString());
            window.Show();
        }

        private void showCheckinsButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBusiness != null)
            {
                Window window = new CheckInWindow(selectedBusiness.business_id);
                window.Show();
            }

        }

        private void searchName_TextChanged(object sender, TextChangedEventArgs e)
        {
            userIds.SelectedValue = null;
            userIds.Items.Clear();
            userFriendsList.Items.Clear();
            userFriendTipsGrid.Items.Clear();

            userNameTextBox.Text = String.Empty;
            userStarsTextBox.Text = String.Empty;
            userFansTextBox.Text = String.Empty;
            userYelpSinceTextBox.Text = String.Empty;
            funnyTextBox.Text = String.Empty;
            coolTextBox.Text = String.Empty;
            usefulTextBox.Text = String.Empty;

        }

        private void removeFriendButton_Click(object sender, RoutedEventArgs e)
        {
            var row_item = (Friend)userFriendsList.SelectedItem;
            string friendId = row_item.friend_id;

            removeUserFriend(friendId);

            userFriendsList.Items.Clear();
            userFriendTipsGrid.Items.Clear();

            getUserFriends(id);
            getUserFriendsReviews(id);

        }

        public void removeUserFriend(string friendId)
        {
            using (var conn = new NpgsqlConnection(BuildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "DELETE FROM friendsTable WHERE user_id = '" + id + "' AND friend_id = '" + friendId + "'; ";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                        }
                    }
                }

                conn.Close();
            }
        }

        private void addReviewButton_Click(object sender, RoutedEventArgs e)
        {
            var guid = Guid.NewGuid().ToString();

            if (userIds.SelectedItem != null)
            {
                using (var conn = new NpgsqlConnection(BuildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "INSERT INTO reviewTable " +
                                          "VALUES('" + guid + "', '" + userIds.SelectedItem.ToString() + "', '" + selectedBusiness.business_id + "', '" + reviewTextBox.Text + "', '" + ratingComboBox.SelectedItem.ToString() + "', current_date, 0, 0, 0);";

                        Console.Out.Write(cmd.CommandText);
                        using (var reader = cmd.ExecuteReader())
                        {

                        }
                    }

                    conn.Close();
                }
            }

            else
            {
                MessageBox.Show("Error: Must choose user prior to adding review", "Milestone 3");
            }
        }

        private void setLocationButton_Click(object sender, RoutedEventArgs e)
        {
            double latitude, longitude;
            if(double.TryParse(latitudeTextBox.Text, out latitude) && double.TryParse(longitudeTextBox.Text, out longitude))
            {
                myLocation.location.Latitude = latitude;
                myLocation.location.Longitude = longitude;
            }
            else
            {
                MessageBox.Show("Error: Must enter valid coordinates", "Milestone 3");
            }
        }
    }
}
