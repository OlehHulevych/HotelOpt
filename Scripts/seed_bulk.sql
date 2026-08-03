-- ============================================================
-- Bulk mock data: 70 rows each for HouseKeepingTasks,
-- MaintenanceTickets, Shifts, Guests, and Bookings
--
-- IDs used (from seed.sql):
--   Tenant  : aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa
--   Property: bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb
--   Rooms   : cc000001–cc000005
--   Anna    : 019f0dca-7374-7492-aabe-af4fd569c890  (Manager)
--   Ivan    : 019f0dca-e5ca-793f-a5a5-b563885b14f6  (Staff)
--   Maria   : 019f0dcb-94e0-7661-bd06-05841c6274b2  (Staff)
--   Dmytro  : 019f0dcc-23d9-73aa-9beb-2bf0590210a1  (Staff)
-- ============================================================


-- ──────────────────────────────────────────────────────────
-- HOUSEKEEPING TASKS  (70 rows)
-- Status: Pending=1, InProgress=2, Completed=3, Cancelled=4
-- ──────────────────────────────────────────────────────────
INSERT INTO "HouseKeepingTasks" (
  "Id", "Title", "AssignedToId", "AssignedById", "RoomId",
  "Status", "ScheduledAt", "CompletedAt",
  "TenantId", "PropertyId", "CreatedAt", "UpdatedAt"
)
SELECT
  gen_random_uuid(),

  (ARRAY[
    'Deep clean',
    'Change linens',
    'Sanitise bathroom',
    'Vacuum and mop',
    'Restock minibar',
    'Replace towels',
    'Clean windows',
    'Dust furniture',
    'Check AC filters',
    'Inspect smoke detector'
  ])[mod(n - 1, 10) + 1]
  || ' — Room '
  || (ARRAY['101','102','201','301','302'])[mod(n - 1, 5) + 1],

  -- cycle staff: Ivan / Maria / Dmytro
  (ARRAY[
    '019f0dca-e5ca-793f-a5a5-b563885b14f6',
    '019f0dcb-94e0-7661-bd06-05841c6274b2',
    '019f0dcc-23d9-73aa-9beb-2bf0590210a1'
  ])[mod(n - 1, 3) + 1]::uuid,

  '019f0dca-7374-7492-aabe-af4fd569c890'::uuid,   -- assigned by Anna

  (ARRAY[
    'cc000001-cccc-cccc-cccc-cccccccccccc',
    'cc000002-cccc-cccc-cccc-cccccccccccc',
    'cc000003-cccc-cccc-cccc-cccccccccccc',
    'cc000004-cccc-cccc-cccc-cccccccccccc',
    'cc000005-cccc-cccc-cccc-cccccccccccc'
  ])[mod(n - 1, 5) + 1]::uuid,

  -- Status distribution: 20 Pending, 10 InProgress, 35 Completed, 5 Cancelled
  CASE
    WHEN n <= 20 THEN 1
    WHEN n <= 30 THEN 2
    WHEN n <= 65 THEN 3
    ELSE 4
  END,

  -- ScheduledAt: Pending/InProgress = future, Completed/Cancelled = past
  CASE
    WHEN n <= 20 THEN NOW() + (n || ' days')::interval
    WHEN n <= 30 THEN NOW() - ((n - 20) * 2 || ' hours')::interval
    ELSE          NOW() - ((66 - n) || ' days')::interval
  END,

  -- CompletedAt: only for Completed rows
  CASE
    WHEN n BETWEEN 31 AND 65
      THEN NOW() - ((66 - n) || ' days')::interval + INTERVAL '3 hours'
    ELSE NULL
  END,

  'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa'::uuid,
  'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid,
  NOW() - (mod(n, 28) || ' days')::interval,
  NOW()

FROM generate_series(1, 70) AS n;


-- ──────────────────────────────────────────────────────────
-- MAINTENANCE TICKETS  (70 rows)
-- Priority: Low=1, Medium=2, High=3, Critical=4
-- Status:   Open=1, InProgress=2, Resolved=3, Closed=4
-- ──────────────────────────────────────────────────────────
INSERT INTO "MaintenanceTickets" (
  "Id", "Title", "Description",
  "StaffId", "ReportedId",
  "Priority", "Status",
  "RoomId", "PropertyId", "TenantId",
  "ResolvedAt", "CreatedAt", "UpdatedAt"
)
SELECT
  gen_random_uuid(),

  (ARRAY[
    'Broken AC unit',
    'Leaking tap',
    'Broken lamp',
    'Faulty door lock',
    'Clogged drain',
    'Cracked window pane',
    'TV remote missing',
    'Shower pressure low',
    'Heating not working',
    'Elevator malfunction'
  ])[mod(n - 1, 10) + 1],

  (ARRAY[
    'Air conditioning unit not cooling properly.',
    'Bathroom tap drips continuously.',
    'Bedside lamp flickers and goes out.',
    'Electronic key card lock unresponsive.',
    'Sink drain is fully blocked.',
    'Window pane has a visible crack.',
    'TV remote is missing from the room.',
    'Shower head has very low pressure.',
    'Radiator not producing heat.',
    'Elevator stuck between floors 2 and 3.'
  ])[mod(n - 1, 10) + 1],

  -- cycle staff
  (ARRAY[
    '019f0dca-e5ca-793f-a5a5-b563885b14f6',
    '019f0dcb-94e0-7661-bd06-05841c6274b2',
    '019f0dcc-23d9-73aa-9beb-2bf0590210a1'
  ])[mod(n - 1, 3) + 1]::uuid,

  '019f0dca-7374-7492-aabe-af4fd569c890'::uuid,   -- reported by Anna

  -- Priority distribution: Low 25%, Medium 35%, High 25%, Critical 15%
  CASE
    WHEN n <= 17 THEN 1
    WHEN n <= 42 THEN 2
    WHEN n <= 59 THEN 3
    ELSE 4
  END,

  -- Status distribution: 20 Open, 20 InProgress, 20 Resolved, 10 Closed
  CASE
    WHEN n <= 20 THEN 1
    WHEN n <= 40 THEN 2
    WHEN n <= 60 THEN 3
    ELSE 4
  END,

  (ARRAY[
    'cc000001-cccc-cccc-cccc-cccccccccccc',
    'cc000002-cccc-cccc-cccc-cccccccccccc',
    'cc000003-cccc-cccc-cccc-cccccccccccc',
    'cc000004-cccc-cccc-cccc-cccccccccccc',
    'cc000005-cccc-cccc-cccc-cccccccccccc'
  ])[mod(n - 1, 5) + 1]::uuid,

  'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid,
  'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa'::uuid,

  -- ResolvedAt: only for Resolved and Closed
  CASE
    WHEN n > 40 THEN NOW() - ((61 - n) * 12 || ' hours')::interval
    ELSE NULL
  END,

  NOW() - (mod(n, 30) || ' days')::interval,
  NOW()

FROM generate_series(1, 70) AS n;


-- ──────────────────────────────────────────────────────────
-- SHIFTS  (70 rows)
-- Status: Scheduled=1, Active=2, Completed=3, Cancelled=4
-- ──────────────────────────────────────────────────────────
INSERT INTO "Shifts" (
  "Id", "StaffId", "PropertyId", "TenantId",
  "Status", "StartTime", "EndTime",
  "CreatedAt", "UpdatedAt"
)
SELECT
  gen_random_uuid(),

  -- cycle staff
  (ARRAY[
    '019f0dca-e5ca-793f-a5a5-b563885b14f6',
    '019f0dcb-94e0-7661-bd06-05841c6274b2',
    '019f0dcc-23d9-73aa-9beb-2bf0590210a1'
  ])[mod(n - 1, 3) + 1]::uuid,

  'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid,
  'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa'::uuid,

  -- Status distribution: 20 Scheduled, 10 Active, 30 Completed, 10 Cancelled
  CASE
    WHEN n <= 20 THEN 1
    WHEN n <= 30 THEN 2
    WHEN n <= 60 THEN 3
    ELSE 4
  END,

  -- StartTime: Scheduled/Active = future or today, Completed/Cancelled = past
  CASE
    WHEN n <= 20 THEN NOW() + (n || ' days')::interval + INTERVAL '8 hours'
    WHEN n <= 30 THEN NOW() - ((n - 20) || ' hours')::interval
    ELSE          NOW() - ((61 - n) || ' days')::interval + INTERVAL '7 hours'
  END,

  -- EndTime: StartTime + 8 hours
  CASE
    WHEN n <= 20 THEN NOW() + (n || ' days')::interval + INTERVAL '16 hours'
    WHEN n <= 30 THEN NOW() - ((n - 20) || ' hours')::interval + INTERVAL '8 hours'
    ELSE          NOW() - ((61 - n) || ' days')::interval + INTERVAL '15 hours'
  END,

  NOW() - (mod(n, 25) || ' days')::interval,
  NOW()

FROM generate_series(1, 70) AS n;


-- ──────────────────────────────────────────────────────────
-- GUESTS + BOOKINGS + BOOKING↔GUEST links  (70 rows each)
--
-- All three inserts are one CTE statement so the UUIDs
-- generated in guest_ids / booking_ids are evaluated once
-- and reused consistently across all three tables.
--
-- Status: Confirmed=1, CheckedIn=2, CheckedOut=3, Cancelled=4
--   n  1–15  → Confirmed  (future stays)
--   n 16–20  → CheckedIn  (currently in hotel)
--   n 21–60  → CheckedOut (past stays)
--   n 61–70  → Cancelled
-- ──────────────────────────────────────────────────────────
WITH
guest_ids AS MATERIALIZED (
  SELECT gen_random_uuid() AS id, n FROM generate_series(1, 70) AS n
),
booking_ids AS MATERIALIZED (
  SELECT gen_random_uuid() AS id, n FROM generate_series(1, 70) AS n
),
ins_guests AS (
  INSERT INTO "Guests" (
    "Id", "FirstName", "LastName", "PassportNumber",
    "TenantId", "Email", "Phone", "CreatedAt", "UpdatedAt"
  )
  SELECT
    g.id,
    (ARRAY[
      'Oleksiy','Ivan','Mykola','Vasyl','Andriy',
      'Serhiy','Pavlo','Ihor','Roman','Dmytro',
      'Olena','Natalia','Iryna','Maria','Svitlana',
      'Kateryna','Yulia','Halyna','Oksana','Tetiana'
    ])[mod(g.n - 1, 20) + 1],
    (ARRAY[
      'Kovalenko','Petrenko','Bondar','Kravchenko','Melnyk',
      'Shevchenko','Oliynyk','Tkachenko','Kovalchuk','Savchenko',
      'Moroz','Lysenko','Marchenko','Rudenko','Havrylenko',
      'Dziuba','Fedorenko','Hryhorenko','Ivanchenko','Karpenko'
    ])[mod(g.n - 1, 20) + 1],
    'UA' || LPAD(g.n::text, 7, '0'),
    'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa'::uuid,
    lower(
      (ARRAY[
        'oleksiy','ivan','mykola','vasyl','andriy',
        'serhiy','pavlo','ihor','roman','dmytro',
        'olena','natalia','iryna','maria','svitlana',
        'kateryna','yulia','halyna','oksana','tetiana'
      ])[mod(g.n - 1, 20) + 1]
      || '.'
      || (ARRAY[
        'kovalenko','petrenko','bondar','kravchenko','melnyk',
        'shevchenko','oliynyk','tkachenko','kovalchuk','savchenko',
        'moroz','lysenko','marchenko','rudenko','havrylenko',
        'dziuba','fedorenko','hryhorenko','ivanchenko','karpenko'
      ])[mod(g.n - 1, 20) + 1]
    ) || g.n || '@example.com',
    '+38093' || LPAD(g.n::text, 7, '0'),
    NOW() - (mod(g.n, 60) || ' days')::interval,
    NOW()
  FROM guest_ids g
  RETURNING "Id"
),
ins_bookings AS (
  INSERT INTO "Bookings" (
    "Id", "RoomId", "TenantId", "PropertyId",
    "CheckInDate", "CheckOutDate", "Status",
    "CreatedAt", "UpdatedAt"
  )
  SELECT
    b.id,
    (ARRAY[
      'cc000001-cccc-cccc-cccc-cccccccccccc',
      'cc000002-cccc-cccc-cccc-cccccccccccc',
      'cc000003-cccc-cccc-cccc-cccccccccccc',
      'cc000004-cccc-cccc-cccc-cccccccccccc',
      'cc000005-cccc-cccc-cccc-cccccccccccc'
    ])[mod(b.n - 1, 5) + 1]::uuid,
    'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa'::uuid,
    'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'::uuid,
    CASE
      WHEN b.n <= 15 THEN NOW() + (b.n * 6        || ' days')::interval
      WHEN b.n <= 20 THEN NOW() - INTERVAL '1 day'
      WHEN b.n <= 60 THEN NOW() - ((61 - b.n) * 4 || ' days')::interval
      ELSE                NOW() - ((b.n - 60) * 7  || ' days')::interval
    END,
    CASE
      WHEN b.n <= 15 THEN NOW() + ((b.n * 6 + 4)        || ' days')::interval
      WHEN b.n <= 20 THEN NOW() + ((b.n - 15) * 2        || ' days')::interval
      WHEN b.n <= 60 THEN NOW() - (((61 - b.n) * 4 - 3)  || ' days')::interval
      ELSE                NOW() - (((b.n - 60) * 7 - 4)   || ' days')::interval
    END,
    CASE
      WHEN b.n <= 15 THEN 1
      WHEN b.n <= 20 THEN 2
      WHEN b.n <= 60 THEN 3
      ELSE 4
    END,
    NOW() - (mod(b.n, 60) || ' days')::interval,
    NOW()
  FROM booking_ids b
  RETURNING "Id"
)
-- Link each booking to its matching guest (booking n → guest n)
INSERT INTO "BookingGuest" ("BookingsId", "GuestsId")
SELECT b.id, g.id
FROM booking_ids b
JOIN guest_ids   g ON b.n = g.n
WHERE EXISTS (SELECT 1 FROM ins_guests)   -- ensure ins_guests ran
  AND EXISTS (SELECT 1 FROM ins_bookings); -- ensure ins_bookings ran
