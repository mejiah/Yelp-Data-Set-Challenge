-- update count of reviews for a particular business, update averageRating also

CREATE OR REPLACE FUNCTION updateReviewCount() RETURNS trigger AS '
BEGIN
	UPDATE businessTable
		SET review_count = review_count + 1, reviewrating = temp.averageRating
		FROM (SELECT temp.business_id, (temp.sum / temp.count) as averageRating
	  		  FROM (SELECT business_id, SUM(stars), COUNT(business_id)
	                FROM reviewTable
                    GROUP BY business_id) as temp) as temp
		WHERE businessTable.business_id = New.business_id AND temp.business_id = New.business_id;
	RETURN NEW;
END
' LANGUAGE plpgsql;

CREATE TRIGGER IncreaseReviewCount
AFTER INSERT ON reviewTable
FOR EACH ROW
EXECUTE PROCEDURE updateReviewCount();


-- update number of checkins for a particular business

CREATE OR REPLACE FUNCTION updateCheckInCount() RETURNS trigger AS '
BEGIN
	UPDATE businessTable
		SET numcheckins = numcheckins + 1
		WHERE business_id = New.business_id;
	RETURN NEW;
END
' LANGUAGE plpgsql;

CREATE TRIGGER IncreaseCheckInCount
AFTER UPDATE OF morning,afternoon,evening,night ON checkinTable
FOR EACH ROW
WHEN (OLD.morning < NEW.morning OR OLD.afternoon < NEW.afternoon OR OLD.evening < NEW.evening OR OLD.night < NEW.night)
EXECUTE PROCEDURE updateCheckInCount();


