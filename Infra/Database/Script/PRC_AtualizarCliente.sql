CREATE PROCEDURE AtualizarCliente @clienteId INT, @nome Varchar(100), @email Varchar(100), @logotipo Varchar(100)
AS
BEGIN
    UPDATE Cliente
    SET Nome = @nome, Email = @email, Logotipo = @logotipo
    WHERE ClienteId = @clienteId;
END