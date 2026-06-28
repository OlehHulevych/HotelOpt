-- ==================================================
-- HotelOpt Mock Data Seed Script
-- ==================================================
-- Run in this order:
--
-- STEP 1: Run PART 1 below (Tenant only)
--
-- STEP 2: Register 4 users via POST http://localhost:5092/api/auth/register
--         Use TenantId: aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa
--
--   { "name": "Anna", "surname": "Koval", "email": "anna.koval@hotel.com", "password": "Test@1234", "role": 2, "tenantId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa" }
--   { "name": "Ivan", "surname": "Petrenko", "email": "ivan.petrenko@hotel.com", "password": "Test@1234", "role": 3, "tenantId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa" }
--   { "name": "Maria", "surname": "Bondar", "email": "maria.bondar@hotel.com", "password": "Test@1234", "role": 3, "tenantId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa" }
--   { "name": "Dmytro", "surname": "Kravchenko", "email": "dmytro.kravchenko@hotel.com", "password": "Test@1234", "role": 3, "tenantId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa" }
--
-- STEP 3: Get user IDs:
--   SELECT "Id", "Email" FROM "AspNetUsers";
--
-- STEP 4: Replace placeholders at the bottom of this file:
--   019f0dca-7374-7492-aabe-af4fd569c890  → Anna Koval's ID
--   019f0dca-e5ca-793f-a5a5-b563885b14f6  → Ivan Petrenko's ID
--   019f0dcb-94e0-7661-bd06-05841c6274b2  → Maria Bondar's ID
--   019f0dcc-23d9-73aa-9beb-2bf0590210a1  → Dmytro Kravchenko's ID
--
-- STEP 5: Run PART 2 below
-- ==================================================


-- ==================== PART 1 ====================

INSERT INTO "Tenants" ("Id", "Name", "ContactEmail", "Status", "CreatedAt", "UpdatedAt")
VALUES (
    'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa',
    'Grand Hotels CEE',
    'contact@grandhotels.com',
    1,  -- Active
    NOW(), NOW()
);


-- ==================== PART 2 ====================
-- Replace 019f0dca-7374-7492-aabe-af4fd569c890, 019f0dca-e5ca-793f-a5a5-b563885b14f6, 019f0dcb-94e0-7661-bd06-05841c6274b2, 019f0dcc-23d9-73aa-9beb-2bf0590210a1 before running

-- Property
INSERT INTO "Properties" ("Id", "Name", "ContactEmail", "PhoneNumber", "Address", "StarRating", "TenantId", "CreatedAt", "UpdatedAt")
VALUES (
    'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb',
    'Grand Hotel Kyiv',
    'kyiv@grandhotels.com',
    '+380441234567',
    'Khreshchatyk 1, Kyiv',
    5,
    'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa',
    NOW(), NOW()
);

-- Rooms
-- RoomType:   Single=1, Double=2, Twin=3, Suite=4, Deluxe=5
-- RoomStatus: Available=1, Occupied=2, Cleaning=3, OutOfOrder=4
INSERT INTO "Rooms" ("Id", "RoomNumber", "Description", "PropertyId", "Status", "Type", "TenantId", "CreatedAt", "UpdatedAt") VALUES
('cc000001-cccc-cccc-cccc-cccccccccccc', '101', 'Standard single, city view',      'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 3, 1, 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', NOW(), NOW()),
('cc000002-cccc-cccc-cccc-cccccccccccc', '102', 'Standard double, garden view',     'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 1, 2, 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', NOW(), NOW()),
('cc000003-cccc-cccc-cccc-cccccccccccc', '201', 'Twin room, street view',           'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 3, 3, 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', NOW(), NOW()),
('cc000004-cccc-cccc-cccc-cccccccccccc', '301', 'Suite with panoramic view',        'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 2, 4, 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', NOW(), NOW()),
('cc000005-cccc-cccc-cccc-cccccccccccc', '302', 'Deluxe double, river view',        'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 1, 5, 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', NOW(), NOW());

-- Shifts for tomorrow (auto-assignment job picks these up)
-- ShiftStatus: Scheduled=1
INSERT INTO "Shifts" ("Id", "StaffId", "PropertyId", "TenantId", "Status", "StartTime", "EndTime", "CreatedAt", "UpdatedAt") VALUES
('dd000001-dddd-dddd-dddd-dddddddddddd', '019f0dca-e5ca-793f-a5a5-b563885b14f6', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 1, NOW() + INTERVAL '1 day', NOW() + INTERVAL '1 day 8 hours', NOW(), NOW()),
('dd000002-dddd-dddd-dddd-dddddddddddd', '019f0dcb-94e0-7661-bd06-05841c6274b2', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 1, NOW() + INTERVAL '1 day', NOW() + INTERVAL '1 day 8 hours', NOW(), NOW()),
('dd000003-dddd-dddd-dddd-dddddddddddd', '019f0dcc-23d9-73aa-9beb-2bf0590210a1', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 1, NOW() + INTERVAL '1 day', NOW() + INTERVAL '1 day 8 hours', NOW(), NOW());

-- Housekeeping Tasks
-- HouseKeepingTaskStatus: Pending=1, InProgress=2, Completed=3, Cancelled=4

-- Pending tasks for tomorrow (will be picked up by auto-assignment Hangfire job)
INSERT INTO "HouseKeepingTasks" ("Id", "Title", "AssignedToId", "AssignedById", "RoomId", "Status", "ScheduledAt", "CompletedAt", "TenantId", "PropertyId", "CreatedAt", "UpdatedAt") VALUES
('ee000001-eeee-eeee-eeee-eeeeeeeeeeee', 'Deep clean room 101',       '00000000-0000-0000-0000-000000000000', '019f0dca-7374-7492-aabe-af4fd569c890', 'cc000001-cccc-cccc-cccc-cccccccccccc', 1, NOW() + INTERVAL '1 day', NULL, 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', NOW(), NOW()),
('ee000002-eeee-eeee-eeee-eeeeeeeeeeee', 'Change linens room 201',     '00000000-0000-0000-0000-000000000000', '019f0dca-7374-7492-aabe-af4fd569c890', 'cc000003-cccc-cccc-cccc-cccccccccccc', 1, NOW() + INTERVAL '1 day', NULL, 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', NOW(), NOW()),
('ee000003-eeee-eeee-eeee-eeeeeeeeeeee', 'Sanitise bathroom room 301', '00000000-0000-0000-0000-000000000000', '019f0dca-7374-7492-aabe-af4fd569c890', 'cc000004-cccc-cccc-cccc-cccccccccccc', 1, NOW() + INTERVAL '1 day', NULL, 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', NOW(), NOW()),
('ee000004-eeee-eeee-eeee-eeeeeeeeeeee', 'Vacuum and mop room 102',    '00000000-0000-0000-0000-000000000000', '019f0dca-7374-7492-aabe-af4fd569c890', 'cc000002-cccc-cccc-cccc-cccccccccccc', 1, NOW() + INTERVAL '1 day', NULL, 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', NOW(), NOW()),
('ee000005-eeee-eeee-eeee-eeeeeeeeeeee', 'Restock minibar room 302',   '00000000-0000-0000-0000-000000000000', '019f0dca-7374-7492-aabe-af4fd569c890', 'cc000005-cccc-cccc-cccc-cccccccccccc', 1, NOW() + INTERVAL '1 day', NULL, 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', NOW(), NOW());

-- Completed tasks this week (for fairness score: Staff1=3, Staff2=2, Staff3=1)
INSERT INTO "HouseKeepingTasks" ("Id", "Title", "AssignedToId", "AssignedById", "RoomId", "Status", "ScheduledAt", "CompletedAt", "TenantId", "PropertyId", "CreatedAt", "UpdatedAt") VALUES
('ee000006-eeee-eeee-eeee-eeeeeeeeeeee', 'Clean 101 Mon', '019f0dca-e5ca-793f-a5a5-b563885b14f6', '019f0dca-7374-7492-aabe-af4fd569c890', 'cc000001-cccc-cccc-cccc-cccccccccccc', 3, NOW() - INTERVAL '2 days', NOW() - INTERVAL '2 days', 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', NOW(), NOW()),
('ee000007-eeee-eeee-eeee-eeeeeeeeeeee', 'Clean 201 Tue', '019f0dca-e5ca-793f-a5a5-b563885b14f6', '019f0dca-7374-7492-aabe-af4fd569c890', 'cc000003-cccc-cccc-cccc-cccccccccccc', 3, NOW() - INTERVAL '1 day', NOW() - INTERVAL '1 day', 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', NOW(), NOW()),
('ee000008-eeee-eeee-eeee-eeeeeeeeeeee', 'Clean 301 Wed', '019f0dca-e5ca-793f-a5a5-b563885b14f6', '019f0dca-7374-7492-aabe-af4fd569c890', 'cc000004-cccc-cccc-cccc-cccccccccccc', 3, NOW() - INTERVAL '3 hours', NOW() - INTERVAL '2 hours', 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', NOW(), NOW()),
('ee000009-eeee-eeee-eeee-eeeeeeeeeeee', 'Clean 102 Mon', '019f0dcb-94e0-7661-bd06-05841c6274b2', '019f0dca-7374-7492-aabe-af4fd569c890', 'cc000002-cccc-cccc-cccc-cccccccccccc', 3, NOW() - INTERVAL '2 days', NOW() - INTERVAL '2 days', 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', NOW(), NOW()),
('ee000010-eeee-eeee-eeee-eeeeeeeeeeee', 'Clean 302 Tue', '019f0dcb-94e0-7661-bd06-05841c6274b2', '019f0dca-7374-7492-aabe-af4fd569c890', 'cc000005-cccc-cccc-cccc-cccccccccccc', 3, NOW() - INTERVAL '1 day', NOW() - INTERVAL '1 day', 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', NOW(), NOW()),
('ee000011-eeee-eeee-eeee-eeeeeeeeeeee', 'Clean 101 Tue', '019f0dcc-23d9-73aa-9beb-2bf0590210a1', '019f0dca-7374-7492-aabe-af4fd569c890', 'cc000001-cccc-cccc-cccc-cccccccccccc', 3, NOW() - INTERVAL '1 day', NOW() - INTERVAL '1 day', 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', NOW(), NOW());

-- Maintenance Tickets
-- TicketPriority: Low=1, Medium=2, High=3, Critical=4
-- TicketStatus:   Open=1, InProgress=2, Resolved=3, Closed=4
INSERT INTO "MaintenanceTickets" ("Id", "Title", "Description", "StaffId", "ReportedId", "Priority", "Status", "RoomId", "PropertyId", "TenantId", "ResolvedAt", "CreatedAt", "UpdatedAt") VALUES
('ff000001-ffff-ffff-ffff-ffffffffffff', 'Broken AC unit',  'Air conditioning in room 301 not working', '019f0dca-e5ca-793f-a5a5-b563885b14f6', '019f0dca-7374-7492-aabe-af4fd569c890', 3, 1, 'cc000004-cccc-cccc-cccc-cccccccccccc', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', NULL,                    NOW(), NOW()),
('ff000002-ffff-ffff-ffff-ffffffffffff', 'Leaking tap',     'Bathroom tap in room 102 is leaking',      '019f0dcb-94e0-7661-bd06-05841c6274b2', '019f0dca-7374-7492-aabe-af4fd569c890', 2, 2, 'cc000002-cccc-cccc-cccc-cccccccccccc', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', NULL,                    NOW(), NOW()),
('ff000003-ffff-ffff-ffff-ffffffffffff', 'Broken lamp',     'Bedside lamp not working in room 201',     '019f0dcc-23d9-73aa-9beb-2bf0590210a1', '019f0dca-7374-7492-aabe-af4fd569c890', 1, 3, 'cc000003-cccc-cccc-cccc-cccccccccccc', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', NOW() - INTERVAL '1 hour', NOW(), NOW());
