TRUNCATE TABLE public."AttendanceRecords" RESTART IDENTITY CASCADE;
TRUNCATE TABLE public."Workers" RESTART IDENTITY CASCADE;

INSERT INTO public."Workers" ("Id", "Name", "JobTitle", "Email", "BirthDate", "TimeZone", "PortalDasFinancasUserName", "PortalDasFinancasUserPassword") VALUES (1, 'TestUser13', 'string', 'user@example.com', '2025-09-22', 'UTC', 'user13', 'Qr1/U4qQbX/qKTxFdP0Pkw==');
INSERT INTO public."Workers" ("Id", "Name", "JobTitle", "Email", "BirthDate", "TimeZone", "PortalDasFinancasUserName") VALUES (2, 'teste', 'testing', 'email@gmail.com', '2025-01-01', 'UTC', 'user3');
INSERT INTO public."Workers" ("Id", "Name", "JobTitle", "Email", "BirthDate", "TimeZone", "PortalDasFinancasUserName") VALUES (3, 'Vasco Jacinto Apolinario', 'testing', 'vascoapolinario@gmail.com', '2004-12-22', 'UTC', 'vascoapolinario');
INSERT INTO public."Workers" ("Id", "Name", "JobTitle", "Email", "BirthDate", "TimeZone", "PortalDasFinancasUserName") VALUES (4, 'string', 'string', 'user@example.com', '2025-09-24', 'UTC', 'user5');
INSERT INTO public."Workers" ("Id", "Name", "JobTitle", "Email", "BirthDate", "TimeZone", "PortalDasFinancasUserName") VALUES (5, 'testresponsecode', 'responsecode', 'response@gmail.com', '2025-09-24', 'UTC', 'responsecode');


INSERT INTO public."AttendanceRecords" ("Id", "WorkerId", "EventType", "EventDate") VALUES
(1, 1, 'CheckIn',  '2025-01-01T08:30:00Z'),
(2, 1, 'CheckOut', '2025-01-01T17:30:00Z'),
(3, 2, 'CheckIn',  '2025-01-02T09:00:00Z'),
(4, 3, 'Undefined','2025-01-03T12:00:00Z'),
(5, 4, 'CheckIn',  '2025-01-03T12:00:00Z');

SELECT setval(pg_get_serial_sequence('"AttendanceRecords"', 'Id'), COALESCE(MAX("Id"), 1)) FROM "AttendanceRecords";
SELECT setval(pg_get_serial_sequence('"Workers"', 'Id'), COALESCE(MAX("Id"), 1)) FROM "Workers";