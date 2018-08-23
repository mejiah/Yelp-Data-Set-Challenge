import json
import psycopg2

def cleanStr4SQL(s):
    return s.replace("'","`").replace("\n"," ")

def int2BoolStr (value):
    if value == 0:
        return 'False'
    else:
        return 'True'

def insert2BusinessTable():
    #reading the JSON file
    with open('.//yelp_dataset//yelp_business.JSON','r') as f:    #TODO: update path for the input file
        #outfile =  open('.//yelp_dataset//yelp_user.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        #TODO: update the database name, username, and password
        try:
            conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='Compaq27'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            #Generate the INSERT statement for the cussent business
            #TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statment based on your own table schema ans
            # include values for all businessTable attributes
            sql_str = "INSERT INTO businessTable (business_id,name,address,state,city,postal_code,latitude,longitude,stars,review_count,is_open,numCheckins,reviewrating) " \
                      "VALUES ('" + cleanStr4SQL(data['business_id']) + "','" + cleanStr4SQL(data["name"]) + "','" + cleanStr4SQL(data["address"]) + "','" + \
                      cleanStr4SQL(data["state"]) + "','" + cleanStr4SQL(data["city"]) + "','" + cleanStr4SQL(data["postal_code"]) + "'," + str(data["latitude"]) + "," + \
                      str(data["longitude"]) + "," + str(data["stars"]) + "," + str(data["review_count"]) + "," + str(data["is_open"]) + ",0," + "0.0" + ");"

            try:
                cur.execute(sql_str)
            except:
                print("Insert to businessTable failed!")
            conn.commit()
            # optionally you might write the INSERT statement to a file.
            # outfile.write(sql_str)

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()

def insert2BusinessTableCategories():

    #reading the JSON file
    with open('.//yelp_dataset//yelp_business.JSON','r') as f:    #TODO: update path for the input file
        #outfile =  open('.//yelp_dataset//yelp_user.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        #TODO: update the database name, username, and password
        try:
            conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='Compaq27'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            #Generate the INSERT statement for the cussent business
            #TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statment based on your own table schema ans
            # include values for all businessTable attributes

            #inserts friends list of user to friendsTable
            for category in data['categories']:
                sql_str = "INSERT INTO categoriesTable (business_id,category_type) " \
                        "VALUES ('" + cleanStr4SQL(data['business_id']) + "','" + cleanStr4SQL(category) + "');"
                try:
                    cur.execute(sql_str)
                except:
                    print("Insert to friendsTable failed!")
                conn.commit()

                # optionally you might write the INSERT statement to a file.
                # outfile.write(sql_str)

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()

def insert2BusinessTableAtrributes():

    #reading the JSON file
    with open('.//yelp_dataset//yelp_business.JSON','r') as f:    #TODO: update path for the input file
        #outfile =  open('.//yelp_dataset//yelp_user.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        #TODO: update the database name, username, and password
        try:
            conn = psycopg2.connect("dbname='yelpdb' port=5433 user='postgres' host='localhost' password='Bix53z7h4m'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            #Generate the INSERT statement for the cussent business
            #TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statment based on your own table schema ans
            # include values for all businessTable attributes

            attributes = data['attributes']
            for attr, val in attributes.items():
                # non-nested attribute
                if type(val) != dict:
                    if (type(val) == int and attr!="RestaurantsPriceRange2"):
                        val = int2BoolStr(val)
                    if val=="no":
                        val = False
                    if val=="yes":
                        val = True

                    sql_str = "INSERT INTO attributesTable (business_id,attribute_type, attribute_value) " \
                              "VALUES ('" + cleanStr4SQL(data['business_id']) + "','" + str(attr) + "','" + str(val) + "');"

                    try:
                        cur.execute(sql_str)
                    except:
                        print("Insert to attributesTable failed!")
                    conn.commit()
                # nested attributes
                if type(val) == dict:
                    for nested_attr, nested_val in val.items():
                        if (type(val) == int):
                            val = int2BoolStr(nested_val)
                        sql_str = "INSERT INTO attributesTable (business_id,attribute_type, attribute_value) " \
                                      "VALUES ('" + cleanStr4SQL(data['business_id']) + "','" + str(nested_attr) + "','" + str(nested_val) + "');"
                        try:
                            cur.execute(sql_str)
                        except:
                            print("Insert to attributesTable failed!")
                        conn.commit()
                # optionally you might write the INSERT statement to a file.
                # outfile.write(sql_str)

            line = f.readline()
            count_line +=1
            print(count_line)

        cur.close()
        conn.close()

    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()

def insert2BusinessTableHours():

    #reading the JSON file
    with open('.//yelp_dataset//yelp_business.JSON','r') as f:    #TODO: update path for the input file
        #outfile =  open('.//yelp_dataset//yelp_user.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        #TODO: update the database name, username, and password
        try:
            conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' port= 5433 password='Bix53z7h4m'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            #Generate the INSERT statement for the cussent business
            #TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statment based on your own table schema ans
            # include values for all businessTable attributes

            hours = data['hours']
            for day, hours in hours.items():
                split_hours = hours.split('-')
                openSplit = split_hours[0].split(':')
                openTime = float(openSplit[0])
                if openSplit[1] == '30':
                    openTime+=.5
                closeSplit = split_hours[1].split(':')
                closeTime = float(closeSplit[0])
                if closeSplit[1] == '30':
                    closeTime += .5
                sql_str = "INSERT INTO businessTimesTable (business_id,day,open,close) " \
                          "VALUES ('" + cleanStr4SQL(data['business_id']) + "','" + str(day) + "'," + str(openTime) + "," + str(closeTime) + ");"
                try:
                    cur.execute(sql_str)
                except:
                    print(sql_str)
                    print("Insert to businessTimesTable failed!")
                conn.commit()

                # optionally you might write the INSERT statement to a file.
                # outfile.write(sql_str)

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()

def insert2UserTable():
    #reading the JSON file
    with open('.//yelp_dataset//yelp_user.JSON','r') as f:    #TODO: update path for the input file
        #outfile =  open('.//yelp_dataset//yelp_user.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        #TODO: update the database name, username, and password
        try:
            conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='Compaq27'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            #Generate the INSERT statement for the cussent business
            #TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statment based on your own table schema ans
            # include values for all businessTable attributes

            sql_str = "INSERT INTO userTable (name,user_id,yelping_since,review_count,fans,average_stars,funny,useful,cool) " \
                      "VALUES ('" + cleanStr4SQL(data['name']) + "','" + cleanStr4SQL(data["user_id"]) + "','" + cleanStr4SQL(data["yelping_since"]) + "'," + \
                      str(data["review_count"]) + "," + str(data["fans"]) + "," + str(data["average_stars"]) + "," + str(data["funny"]) + "," + \
                      str(data["useful"]) + "," + str(data["cool"]) + ");"
            print(sql_str)
            try:
                cur.execute(sql_str)
            except:
                print("Insert to userTable failed!")
            conn.commit()
            # optionally you might write the INSERT statement to a file.
            # outfile.write(sql_str)

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()

def insert2FriendsTable():

    #reading the JSON file
    with open('.//yelp_dataset//yelp_user.JSON','r') as f:    #TODO: update path for the input file
        #outfile =  open('.//yelp_dataset//yelp_user.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        #TODO: update the database name, username, and password
        try:
            conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='Compaq27'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            #Generate the INSERT statement for the cussent business
            #TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statment based on your own table schema ans
            # include values for all businessTable attributes

            #userId = cleanStr4SQL(data['user_id'])
            for friend in data['friends']:
                sql_str = "INSERT INTO friendsTable (user_id,friend_id) " \
                        "VALUES ('" + cleanStr4SQL(data['user_id']) + "','" + cleanStr4SQL(friend) + "');"
                try:
                    cur.execute(sql_str)
                except:
                    print("Insert to friendsTable failed!")
                conn.commit()
                # optionally you might write the INSERT statement to a file.
                # outfile.write(sql_str)

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()

def insert2ReviewTable():
    #reading the JSON file
    with open('.//yelp_dataset//yelp_review.JSON','r') as f:    #TODO: update path for the input file
        #outfile =  open('.//yelp_dataset//yelp_review.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0

        #connect to yelpdb database on postgres server using psycopg2
        #TODO: update the database name, username, and password
        try:
            conn = psycopg2.connect("dbname='yelpdb' user='postgres' host='localhost' password='Compaq27'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            #Generate the INSERT statement for the cussent business
            #TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statment based on your own table schema ans
            # include values for all businessTable attributes
            sql_str = "INSERT INTO reviewTable (review_id,user_id,business_id,text,stars,date,funny,useful,cool) " \
                      "VALUES ('" + cleanStr4SQL(data['review_id']) + "','" + cleanStr4SQL(data["user_id"]) + "','" + cleanStr4SQL(data["business_id"]) + "','" + \
                      cleanStr4SQL(data["text"]) + "'," + str(data["stars"]) + ",'" + cleanStr4SQL(data["date"]) + "'," + str(data["funny"]) + "," + \
                      str(data["useful"]) + "," + str(data["cool"]) + ");"
            try:
                cur.execute(sql_str)
            except:
                print("Insert to reviewTable failed!")
            conn.commit()
            # optionally you might write the INSERT statement to a file.
            # outfile.write(sql_str)

            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()

def insert2CheckinTable():
    #reading the JSON file
    with open('.//yelp_dataset//yelp_checkin.JSON','r') as f:    #TODO: update path for the input file
        #outfile =  open('.//yelp_dataset//yelp_review.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0
        morning = 0
        afternoon = 0
        evening = 0
        night = 0

        #connect to yelpdb database on postgres server using psycopg2
        #TODO: update the database name, username, and password
        try:
            conn = psycopg2.connect("dbname='yelpdb' port=5433 user='postgres' host='localhost' password='Bix53z7h4m'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            #Generate the INSERT statement for the cussent business
            #TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statment based on your own table schema ans
            # include values for all businessTable attributes
            time = data['time']
            for days, entries in time.items():
                morning=afternoon=evening=night=0
                for entry, val in entries.items():
                    split_entry = entry.split(':')
                    if (6 <= int(split_entry[0]) < 12):#6-12
                        morning += int(val)
                    if (12 <= int(split_entry[0]) < 17):#12-5
                        afternoon += int(val)
                    if (17 <= int(split_entry[0]) < 23):#5-11
                        evening += int(val)
                    if(6 > int(split_entry[0]) or int(split_entry[0]) >= 23): #11-6
                        night += int(val)

                sql_str = "INSERT INTO checkintable (business_id,day,morning,afternoon,evening,night) " \
                      "VALUES ('" + cleanStr4SQL(data['business_id']) + "','" + cleanStr4SQL(days) + "', " + str(morning) + ', ' + str(afternoon) + ', ' + str(evening) + ', ' + str(night) + ");"

                try:
                    cur.execute(sql_str)
                except:
                    print("Insert to checkinTable failed!")
                conn.commit()
            # optionally you might write the INSERT statement to a file.
            # outfile.write(sql_str)
            morning = 0
            afternoon = 0
            evening = 0
            night = 0
            line = f.readline()
            count_line +=1

        cur.close()
        conn.close()

    print(count_line)
    #outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()

#insert2BusinessTable()
#insert2BusinessTableCategories()
#insert2BusinessTableAtrributes()
#insert2BusinessTableHours()
#insert2UserTable()
#insert2FriendsTable()
#insert2ReviewTable()
insert2CheckinTable()

