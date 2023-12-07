CREATE PROCEDURE CadastrarLogradouro @endereco Varchar(150), @clienteId INT, @logradouroId INT = NULL OUTPUT
AS
BEGIN
    INSERT INTO Logradouro (Endereco, ClienteId)
    VALUES (@endereco, @clienteId);
    SET @logradouroId = SCOPE_IDENTITY();
END