
-- 1. Check for duplicate email addresses
SELECT Email, COUNT(*) AS DuplicateCount
FROM Users
GROUP BY Email
HAVING COUNT(*) > 1;

-- 2. Check for duplicate enrolments
SELECT UserID, EventID, CategoryID,
       COUNT(*) AS DuplicateCount
FROM Enrolments
GROUP BY UserID, EventID, CategoryID
HAVING COUNT(*) > 1;

-- 3. Check for multiple results per enrolment
SELECT EnrolmentID, COUNT(*) AS DuplicateCount
FROM Results
GROUP BY EnrolmentID
HAVING COUNT(*) > 1;
