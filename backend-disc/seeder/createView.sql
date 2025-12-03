CREATE OR ALTER VIEW employees_own_profile
AS
    SELECT
        e.id,
        e.work_email,
        e.work_phone,
        e.first_name + ' ' + e.last_name AS full_name,
        e.image_path,

        dp.name AS disc_profile_name,
        dp.color AS disc_profile_color,

        p.name AS position_name,
        d.name AS department_name,

        epd.private_email,
        epd.private_phone,

        u.username
    FROM employees e
        LEFT JOIN disc_profiles dp ON e.disc_profile_id = dp.id
        LEFT JOIN positions p on e.position_id = p.id
        INNER JOIN departments d on e.department_id = d.id
        INNER JOIN employee_private_data epd on e.id = epd.employee_id
        INNER JOIN users u ON e.id = u.employee_id