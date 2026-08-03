-- Fairness Score seed data — tasks for the current calendar week
-- Ivan: 28 tasks (overloaded), Maria: 15 tasks, Dmytro: 6 tasks
-- Run this against your Railway (or local) PostgreSQL database.
-- Uses email subqueries — no hardcoded GUIDs needed.

-- Ivan Petrenko — 28 tasks (overloaded, threshold is 25)
INSERT INTO "HouseKeepingTasks" (
    "Id", "Title", "AssignedToId", "AssignedById", "RoomId",
    "Status", "ScheduledAt", "CompletedAt",
    "TenantId", "PropertyId", "CreatedAt", "UpdatedAt"
)
SELECT
    gen_random_uuid(),
    'Weekly clean #' || n || ' (Ivan)',
    (SELECT "Id" FROM "AspNetUsers" WHERE "Email" = 'ivan.petrenko@hotel.com'),
    (SELECT "Id" FROM "AspNetUsers" WHERE "Email" = 'anna.koval@hotel.com'),
    (SELECT "Id" FROM "Rooms" WHERE "TenantId" = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa' ORDER BY "CreatedAt" LIMIT 1),
    CASE WHEN n % 3 = 0 THEN 3 ELSE 1 END,  -- Status: 3=Completed, 1=Pending
    DATE_TRUNC('week', NOW() AT TIME ZONE 'UTC') + ((n % 5) || ' days')::INTERVAL,
    CASE WHEN n % 3 = 0 THEN NOW() ELSE NULL END,
    'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa',
    (SELECT "Id" FROM "Properties" WHERE "TenantId" = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa' LIMIT 1),
    NOW(), NOW()
FROM generate_series(1, 28) AS n;

-- Maria Bondar — 15 tasks (healthy)
INSERT INTO "HouseKeepingTasks" (
    "Id", "Title", "AssignedToId", "AssignedById", "RoomId",
    "Status", "ScheduledAt", "CompletedAt",
    "TenantId", "PropertyId", "CreatedAt", "UpdatedAt"
)
SELECT
    gen_random_uuid(),
    'Weekly clean #' || n || ' (Maria)',
    (SELECT "Id" FROM "AspNetUsers" WHERE "Email" = 'maria.bondar@hotel.com'),
    (SELECT "Id" FROM "AspNetUsers" WHERE "Email" = 'anna.koval@hotel.com'),
    (SELECT "Id" FROM "Rooms" WHERE "TenantId" = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa' ORDER BY "CreatedAt" LIMIT 1),
    CASE WHEN n % 2 = 0 THEN 3 ELSE 1 END,
    DATE_TRUNC('week', NOW() AT TIME ZONE 'UTC') + ((n % 5) || ' days')::INTERVAL,
    CASE WHEN n % 2 = 0 THEN NOW() ELSE NULL END,
    'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa',
    (SELECT "Id" FROM "Properties" WHERE "TenantId" = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa' LIMIT 1),
    NOW(), NOW()
FROM generate_series(1, 15) AS n;

-- Dmytro Kravchenko — 6 tasks (light week)
INSERT INTO "HouseKeepingTasks" (
    "Id", "Title", "AssignedToId", "AssignedById", "RoomId",
    "Status", "ScheduledAt", "CompletedAt",
    "TenantId", "PropertyId", "CreatedAt", "UpdatedAt"
)
SELECT
    gen_random_uuid(),
    'Weekly clean #' || n || ' (Dmytro)',
    (SELECT "Id" FROM "AspNetUsers" WHERE "Email" = 'dmytro.kravchenko@hotel.com'),
    (SELECT "Id" FROM "AspNetUsers" WHERE "Email" = 'anna.koval@hotel.com'),
    (SELECT "Id" FROM "Rooms" WHERE "TenantId" = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa' ORDER BY "CreatedAt" LIMIT 1),
    1,  -- all Pending
    DATE_TRUNC('week', NOW() AT TIME ZONE 'UTC') + ((n % 5) || ' days')::INTERVAL,
    NULL,
    'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa',
    (SELECT "Id" FROM "Properties" WHERE "TenantId" = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa' LIMIT 1),
    NOW(), NOW()
FROM generate_series(1, 6) AS n;
