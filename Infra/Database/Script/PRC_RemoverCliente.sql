CREATE PROCEDURE RemoverCliente @clienteId INT
AS
BEGIN
    DELETE Logradouro
    WHERE ClienteId = @clienteId;
    DELETE Cliente
    WHERE ClienteId = @clienteId;
END