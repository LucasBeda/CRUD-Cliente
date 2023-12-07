Create Table Cliente(
    ClienteId Int IDENTITY(1,1) PRIMARY KEY,
    Nome Varchar(100) NOT NULL,
    Email Varchar(100) NOT NULL,
    Logotipo Varchar(100) NULL
);

Create Table Logradouro(
    LogradouroId Int IDENTITY(1,1) PRIMARY KEY,
    Endereco Varchar(150) NOT NULL,
    ClienteId INT NOT NULL,
    CONSTRAINT "FK_Cliente" FOREIGN KEY (ClienteId) REFERENCES Cliente(ClienteId)
);