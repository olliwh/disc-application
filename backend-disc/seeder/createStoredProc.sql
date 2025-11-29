CREATE
OR ALTER PROCEDURE sp_AddEmployee @first_name NVARCHAR(255),
@last_name NVARCHAR(255),
@work_email NVARCHAR(255),
@work_phone VARCHAR(25) = NULL,
@image_path VARCHAR(255),
@department_id INT,
@position_id INT = NULL,
@disc_profile_id INT = NULL,
@cpr CHAR(10),
@private_email NVARCHAR(255),
@private_phone VARCHAR(25),
@username NVARCHAR(64),
@password_hash VARCHAR(255),
@user_role_id INT AS BEGIN BEGIN TRY BEGIN TRANSACTION
INSERT INTO employees (
        first_name,
        last_name,
        work_email,
        work_phone,
        image_path,
        department_id,
        position_id,
        disc_profile_id
    )
VALUES (
        @first_name,
        @last_name,
        @work_email,
        @work_phone,
        @image_path,
        @department_id,
        @position_id,
        @disc_profile_id
    );
DECLARE @GeneratedId INT = SCOPE_IDENTITY();
INSERT INTO employee_private_data (employee_id, cpr, private_email, private_phone)
VALUES (
        @GeneratedId,
        @cpr,
        @private_email,
        @private_phone
    );
INSERT INTO users (
        employee_id,
        username,
        password_hash,
        requires_reset,
        user_role_id
    )
VALUES (
        @GeneratedId,
        @username,
        @password_hash,
        1,
        @user_role_id
    );
COMMIT TRANSACTION;
SELECT @GeneratedId AS employee_id;
END TRY BEGIN CATCH ROLLBACK TRANSACTION;
THROW;
END CATCH
END;