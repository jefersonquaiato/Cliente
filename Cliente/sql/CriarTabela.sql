CREATE TABLE [dbo].[cliente] (
    [id]           INT           IDENTITY (1, 1) NOT NULL,
    [nome]         VARCHAR (120) NOT NULL,
    [cpf]          VARCHAR (14)  NULL,
    [cnpj]         VARCHAR (18)  NULL,
    [cep]          VARCHAR (9)   NULL,
    [ie]           VARCHAR (11)  NULL,
    [razao_social] VARCHAR (120) NULL,
    [telefone]     VARCHAR (17)  NULL,
    [estado]       CHAR (2)      NULL,
    [cidade]       VARCHAR (60)  NULL,
    [bairro]       VARCHAR (60)  NULL,
    [rua]          VARCHAR (60)  NULL,
    [numero_casa]  INT           NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);