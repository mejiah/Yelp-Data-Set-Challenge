import json

def cleanStr4SQL(s):
    return s.replace("'","`").replace("\n"," ")

def parseBusinessData():
    #read the JSON file
    with open('.\yelp_business.JSON','r') as f:  #Assumes that the data files are available in the current directory. If not, you should set the path for the yelp data files.
        outfile =  open('business.txt', 'w')
        outfile.write('business_id,  name,  address,  state,  city,  postal_code,  latitude,  longitude,  stars,  review_count,  is_open,  [categories],  [attributes],  [hours]\n')
        line = f.readline()
        count_line = 0
        #read each JSON abject and extract data
        while line:
            data = json.loads(line)
            outfile.write(cleanStr4SQL(data['business_id']) + ', ') #business id
            outfile.write(cleanStr4SQL(data['name']) + ', ') #name
            outfile.write(cleanStr4SQL(data['address']) + ', ') #full_address
            outfile.write(cleanStr4SQL(data['state']) + ', ') #state
            outfile.write(cleanStr4SQL(data['city']) + ', ') #city
            outfile.write(cleanStr4SQL(data['postal_code']) + ', ')  #zipcode
            outfile.write(str(data['latitude']) + ', ') #latitude
            outfile.write(str(data['longitude']) + ', ') #longitude
            outfile.write(str(data['stars']) + ', ') #stars
            outfile.write(str(data['review_count']) + ', ') #reviewcount
            outfile.write(str(data['is_open']) + '\n') #openstatus

            #process attributes (sample code included quote marks in parsed text)
            outfile.write('Categories:  [')
            for item in data['categories']:
                outfile.write(item + ', ')
            outfile.write(']\n')

            #process attributes
            attributes = data['attributes']
            outfile.write('Attributes: [')
            for attr, val in attributes.items():
                # non-nested attribute
                if type(val) == str or type(val) == bool:
                    outfile.write('(' + str(attr) + ', ' + str(val) + ')')
                # nested attributes
                if type(val) == dict:
                    for nested_attr, nested_val in val.items():
                        outfile.write('(' + str(nested_attr) + ', ' + str(nested_val) + ')')
            outfile.write(']\n')

            #process hours
            hours = data['hours']
            outfile.write('Hours: [')
            for day, hours in hours.items():
                split_hours = hours.split('-')
                outfile.write('(' + day + ', ' + split_hours[0] + ',' + split_hours[1] + ')')
            outfile.write(']\n');

            line = f.readline()
            count_line +=1
    print(count_line)
    outfile.close()
    f.close()

def parseUserData():

    with open('.\yelp_user.JSON', 'r') as f:  # Assumes that the data files are available in the current directory. If not, you should set the path for the yelp data files.
        outfile =  open('user.txt', 'w')
        outfile.write('user_id,  name,  yelping_since,  review_count,  fans,  average_stars,  funny,  useful,  cool,  friends\n')
        line = f.readline()
        count_line = 0
        #read each JSON abject and extract data
        while line:
            data = json.loads(line)
            outfile.write(cleanStr4SQL(data['user_id']) + ', ') #user_id
            outfile.write(cleanStr4SQL(data['name']) + ', ') #name
            outfile.write(cleanStr4SQL(data['yelping_since']) + ', ') #yelping_since
            outfile.write(str(data['review_count']) + ', ') #review_count
            outfile.write(str(data['fans']) + ', ') #fans
            outfile.write(str(data['average_stars']) + ', ')  #average_stars
            outfile.write(str(data['funny']) + ', ') #funny
            outfile.write(str(data['useful']) + ', ') #useful
            outfile.write(str(data['cool']) + '\n') #cool
            outfile.write('Friends: [')
            for value in data['friends']:
                outfile.write(value + ' , ')
            outfile.write(']\n')
            line = f.readline()
            count_line +=1
    print(count_line)
    outfile.close()
    f.close()


def parseCheckinData():
    #Steven's implementation
    with open('.\yelp_checkin.JSON', 'r') as f:
        outfile = open('checkin.txt', 'w')
        line = f.readline()
        morning = 0
        afternoon = 0
        evening = 0
        night = 0
        linecount = 0

        while line:
            data = json.loads(line)
            outfile.write('\n' + cleanStr4SQL(data['business_id'])+ '\n')
            time = data['time']
            for days, entries in time.items():
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

                outfile.write('             (' + str(days) + ', ' + str(morning) + ', ' + str(afternoon) + ', ' + str(evening) + ', ' + str(night) + ')\n')
                linecount += 1
                morning = 0
                afternoon = 0
                evening = 0
                night = 0
            line = f.readline()
        print(linecount)

def parseCheckinData():
    #Steven
    with open('.\yelp_checkin.JSON', 'r') as f:
        outfile = open('checkin.txt', 'w')
        line = f.readline()
        morning = 0
        afternoon = 0
        evening = 0
        night = 0
        linecount = 0

        while line:
            data = json.loads(line)
            outfile.write('\n' + cleanStr4SQL(data['business_id'])+ '\n')
            time = data['time']
            for days, entries in time.items():
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

                outfile.write('             (' + str(days) + ', ' + str(morning) + ', ' + str(afternoon) + ', ' + str(evening) + ', ' + str(night) + ')\n')
                linecount += 1
                morning = 0
                afternoon = 0
                evening = 0
                night = 0
            line = f.readline()
        print(linecount)
        outfile.close()
        f.close()


def parseReviewData():
    with open('.\yelp_review.JSON', 'r') as f:  # Assumes that the data files are available in the current directory. If not, you should set the path for the yelp data files.
        outfile =  open('review.txt', 'w')
        outfile.write('review_id,  user_id,  business_id,  text,  stars,  date,  funny,  useful,  cool\n')
        line = f.readline()
        count_line = 0
        #read each JSON abject and extract data
        while line:
            data = json.loads(line)
            outfile.write(cleanStr4SQL(data['review_id']) + ', ') #review_id
            outfile.write(cleanStr4SQL(data['user_id']) + ', ') #user_id
            outfile.write(cleanStr4SQL(data['business_id']) + ', ') #business_id
            outfile.write(cleanStr4SQL(data['text']) + ', ') #text
            outfile.write(str(data['stars']) + ', ') #stars
            outfile.write(cleanStr4SQL(data['date']) + ', ')  #date
            outfile.write(str(data['funny']) + ', ') #funny
            outfile.write(str(data['useful']) + ', ') #useful
            outfile.write(str(data['cool']) + '\n') #cool

            line = f.readline()
            count_line +=1
    print(count_line)
    outfile.close()
    f.close()

parseBusinessData()
parseUserData()
parseCheckinData()
parseReviewData()
