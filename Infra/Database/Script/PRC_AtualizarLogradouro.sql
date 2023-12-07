CREATE PROCEDURE AtualizarLogradouro @logradouroId INT, @endereco Varchar(150)
AS
BEGIN
    UPDATE Logradouro
    SET Endereco = @endereco
    WHERE LogradouroId = @logradouroId;
END