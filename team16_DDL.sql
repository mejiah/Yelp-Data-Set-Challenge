CREATE TABLE userTable (
	name VARCHAR(100) NOT NULL,
	user_id VARCHAR(100),
	yelping_since CHAR(20),
	review_count INTEGER,
	fans INTEGER,
	average_stars FLOAT,
	funny INTEGER,
	useful INTEGER,
	cool INTEGER,
	PRIMARY KEY (user_id)
);

CREATE TABLE friendsTable (
	user_id VARCHAR(100),
	friend_id VARCHAR(100) NOT NULL,
	PRIMARY KEY (user_id, friend_id),
	FOREIGN KEY (user_id) REFERENCES userTable(user_id)
);

CREATE TABLE reviewTable (
	review_id VARCHAR(100),
	user_id VARCHAR(100) NOT NULL,
	business_id VARCHAR(100) NOT NULL,
	text VARCHAR(2000),
	stars FLOAT,
	date VARCHAR(20) NOT NULL,
	funny INTEGER,
	useful INTEGER,
	cool INTEGER,
	PRIMARY KEY (review_id),
	FOREIGN KEY (user_id) REFERENCES userTable(user_id),
	FOREIGN KEY (business_id) REFERENCES businessTable(business_id)
);


CREATE TABLE businessTable (
	business_id VARCHAR(100),
	name VARCHAR(100) NOT NULL,
	address VARCHAR(100) NOT NULL,
	state CHAR(2) NOT NULL,
	city VARCHAR(100) NOT NULL,
	postal_code INTEGER NOT NULL,
	latitude FLOAT NOT NULL,
	longitude FLOAT NOT NULL,
	stars FLOAT,
	review_count INTEGER,
	is_open INTEGER,
	numCheckins INTEGER,
	reviewrating FLOAT,
	PRIMARY KEY (business_id)
);


CREATE TABLE checkinTable (
	business_id VARCHAR(75),
	day VARCHAR(10),
	morning INTEGER,
	afternoon INTEGER,
	evening INTEGER,
	night INTEGER,
	PRIMARY KEY (business_id, day),
	FOREIGN KEY (business_id) REFERENCES businessTable(business_id)
);

CREATE TABLE categoriesTable (
	business_id VARCHAR(100),
	category_type VARCHAR(100) NOT NULL,
	PRIMARY KEY (business_id, category_type),
	FOREIGN KEY (business_id) REFERENCES businessTable(business_id)
);

CREATE TABLE businessTimesTable (
	business_id VARCHAR(100),
	day VARCHAR(100),
	open FLOAT NOT NULL,
	close FLOAT NOT NULL,
	PRIMARY KEY (business_id, day)
);

CREATE TABLE attributesTable (
	business_id VARCHAR(100),
	attribute_type VARCHAR(100) NOT NULL,
	attribute_value VARCHAR(100) NOT NULL,
	PRIMARY KEY (business_id, attribute_type),
	FOREIGN KEY (business_id) REFERENCES businessTable(business_id)
);



