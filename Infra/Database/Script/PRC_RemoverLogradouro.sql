CREATE PROCEDURE RemoverLogradouro @logradouroId INT
AS
BEGIN
    DELETE Logradouro
    WHERE LogradouroId = @logradouroId;
END