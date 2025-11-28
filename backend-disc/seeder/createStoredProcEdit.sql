CREATE PROCEDURE sp_UpdatePrivateInfo
    @id INT,
    @private_email NVARCHAR(255),
    @private_phone VARCHAR(25)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE employee_private_data
    SET private_email = @private_email,
        private_phone = @private_phone
    WHERE employee_id = @id;

    IF @@ROWCOUNT = 0
        THROW 50001, 'Employee not found', 1;
END;