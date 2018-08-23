-- update checkins
	
UPDATE businessTable
    SET numcheckins = temp.sum
FROM (SELECT temp.business_id, SUM(temp.sum) 
      FROM (SELECT *, (morning + afternoon + evening + night) AS Sum
            FROM checkintable) as temp
      GROUP BY temp.business_id) as temp
WHERE businessTable.business_id = temp.business_id

-- update reviewratings

UPDATE businessTable
	SET reviewrating = temp.averagerating
FROM (SELECT temp.business_id, (temp.sum / temp.count) as averageRating
	  FROM (SELECT business_id, SUM(stars), COUNT(business_id)
	        FROM reviewTable
      GROUP BY business_id) as temp) as temp
WHERE businessTable.business_id = temp.business_id

-- update reviewcount

UPDATE businessTable
SET review_count = temp.Count
FROM 
	(
      SELECT b.business_id, count(r.review_id) as Count
	  FROM reviewtable AS r, businesstable b
      WHERE r.business_id = b.business_id
      GROUP BY b.business_id
	 ) AS temp
WHERE businesstable.business_id = temp.business_id