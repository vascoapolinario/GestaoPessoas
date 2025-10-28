TRUNCATE TABLE public."Projects" RESTART IDENTITY CASCADE;
TRUNCATE TABLE public."ProjectWorker" RESTART IDENTITY CASCADE;

TRUNCATE TABLE public."Workers" RESTART IDENTITY CASCADE;

INSERT INTO public."Workers" ("Id", "Name", "JobTitle", "Email", "BirthDate", "TimeZone", "PortalDasFinancasUserName", "PortalDasFinancasUserPassword") VALUES (1, 'TestUser13', 'string', 'user@example.com', '2025-09-22', 'UTC', 'user13', 'Qr1/U4qQbX/qKTxFdP0Pkw==');
INSERT INTO public."Workers" ("Id", "Name", "JobTitle", "Email", "BirthDate", "TimeZone", "PortalDasFinancasUserName") VALUES (2, 'teste', 'testing', 'email@gmail.com', '2025-01-01', 'UTC', 'user3');
INSERT INTO public."Workers" ("Id", "Name", "JobTitle", "Email", "BirthDate", "TimeZone", "PortalDasFinancasUserName") VALUES (3, 'Vasco Jacinto Apolinario', 'testing', 'vascoapolinario@gmail.com', '2004-12-22', 'UTC', 'vascoapolinario');
INSERT INTO public."Workers" ("Id", "Name", "JobTitle", "Email", "BirthDate", "TimeZone", "PortalDasFinancasUserName") VALUES (4, 'string', 'string', 'user@example.com', '2025-09-24', 'UTC', 'user5');
INSERT INTO public."Workers" ("Id", "Name", "JobTitle", "Email", "BirthDate", "TimeZone", "PortalDasFinancasUserName") VALUES (5, 'testresponsecode', 'responsecode', 'response@gmail.com', '2025-09-24', 'UTC', 'responsecode');

INSERT INTO public."Projects" ("Id", "Name") VALUES (1, 'ProjectTest1');
INSERT INTO public."Projects" ("Id", "Name") VALUES (2, 'ProjectTest2');
INSERT INTO public."Projects" ("Id", "Name") VALUES (3, 'ProjectTest3');
INSERT INTO public."Projects" ("Id", "Name") VALUES (4, 'ProjectTest4');
INSERT INTO public."Projects" ("Id", "Name") VALUES (5, 'ProjectTest5');

SELECT setval(pg_get_serial_sequence('"Projects"', 'Id'), COALESCE(MAX("Id"), 1)) FROM "Projects";
SELECT setval(pg_get_serial_sequence('"Workers"', 'Id'), COALESCE(MAX("Id"), 1)) FROM "Workers";
