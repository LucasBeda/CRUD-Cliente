CREATE PROCEDURE CadastrarCliente @nome Varchar(100), @email Varchar(100), @logotipo Varchar(100), @clienteId INT = NULL OUTPUT
AS
BEGIN
    INSERT INTO Cliente (Nome, Email, Logotipo)
    VALUES (@nome, @email, @logotipo);
    SET @clienteId = SCOPE_IDENTITY();
END