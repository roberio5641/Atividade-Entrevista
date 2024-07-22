GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;
SET NUMERIC_ROUNDABORT OFF;
GO

:setvar DatabaseName "C:\USERS\THEJK\ONEDRIVE\ÁREA DE TRABALHO\ATIVIDADEENTREVISTA\FI.WEBATIVIDADEENTREVISTA\APP_DATA\BANCODEDADOS.MDF"
:setvar DefaultFilePrefix "C_\USERS\THEJK\ONEDRIVE\ÁREA DE TRABALHO\ATIVIDADEENTREVISTA\FI.WEBATIVIDADEENTREVISTA\APP_DATA\BANCODEDADOS.MDF_"
:setvar DefaultDataPath "C:\Users\thejk\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\"
:setvar DefaultLogPath "C:\Users\thejk\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\"
GO

:on error exit
GO

:setvar __IsSqlCmdEnabled "True"
GO

IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
BEGIN
    PRINT N'O modo SQLCMD deve ser habilitado para executar esse script com êxito.';
    SET NOEXEC ON;
END
GO

USE [$(DatabaseName)];
GO

IF (SELECT OBJECT_ID('tempdb..#tmpErrors')) IS NOT NULL DROP TABLE #tmpErrors
GO
CREATE TABLE #tmpErrors (Error int)
GO

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL READ COMMITTED
GO
BEGIN TRANSACTION
GO

-- Adicionando coluna CPF permitindo NULL temporariamente
PRINT N'Adicionando coluna CPF na tabela [dbo].[CLIENTES]...';
GO

IF COL_LENGTH('dbo.CLIENTES', 'CPF') IS NULL
BEGIN
    ALTER TABLE [dbo].[CLIENTES]
    ADD [CPF] VARCHAR(11) NULL;
END
GO

PRINT N'Atualizando valores da coluna CPF se necessário...';
GO

-- Atualizar valores da coluna CPF conforme necessário
-- UPDATE [dbo].[CLIENTES] SET [CPF] = 'valor_padrão' WHERE [CPF] IS NULL;

-- Após atualizar valores, alterar a coluna para não permitir NULL
PRINT N'Alterando coluna CPF para não permitir NULL...';
GO

IF COL_LENGTH('dbo.CLIENTES', 'CPF') IS NOT NULL
BEGIN
    ALTER TABLE [dbo].[CLIENTES]
    ALTER COLUMN [CPF] VARCHAR(11) NOT NULL;
END
GO

PRINT N'Alterando Procedimentos...';
GO

ALTER PROC FI_SP_AltCliente
    @NOME          VARCHAR (50) ,
    @SOBRENOME     VARCHAR (255),
    @NACIONALIDADE VARCHAR (50) ,
    @CEP           VARCHAR (9)  ,
    @ESTADO        VARCHAR (2)  ,
    @CIDADE        VARCHAR (50) ,
    @LOGRADOURO    VARCHAR (500),
    @EMAIL         VARCHAR (2079),
    @TELEFONE      VARCHAR (15),
    @CPF           VARCHAR (11),
    @Id            BIGINT
AS
BEGIN
    UPDATE CLIENTES 
    SET 
        NOME = @NOME, 
        SOBRENOME = @SOBRENOME, 
        NACIONALIDADE = @NACIONALIDADE, 
        CEP = @CEP, 
        ESTADO = @ESTADO, 
        CIDADE = @CIDADE, 
        LOGRADOURO = @LOGRADOURO, 
        EMAIL = @EMAIL, 
        TELEFONE = @TELEFONE,
        CPF = @CPF
    WHERE ID = @ID
END
GO

ALTER PROC FI_SP_ConsCliente
    @ID BIGINT
AS
BEGIN
    IF(ISNULL(@ID,0) = 0)
        SELECT NOME, SOBRENOME, NACIONALIDADE, CEP, ESTADO, CIDADE, LOGRADOURO, EMAIL, TELEFONE, CPF, ID FROM CLIENTES WITH(NOLOCK)
    ELSE
        SELECT NOME, SOBRENOME, NACIONALIDADE, CEP, ESTADO, CIDADE, LOGRADOURO, EMAIL, TELEFONE, CPF, ID FROM CLIENTES WITH(NOLOCK) WHERE ID = @ID
END
GO

ALTER PROC FI_SP_IncClienteV2
    @NOME          VARCHAR (50) ,
    @SOBRENOME     VARCHAR (255),
    @NACIONALIDADE VARCHAR (50) ,
    @CEP           VARCHAR (9)  ,
    @ESTADO        VARCHAR (2)  ,
    @CIDADE        VARCHAR (50) ,
    @LOGRADOURO    VARCHAR (500),
    @EMAIL         VARCHAR (2079),
    @TELEFONE      VARCHAR (15),
    @CPF           VARCHAR (11)
AS
BEGIN
    INSERT INTO CLIENTES (NOME, SOBRENOME, NACIONALIDADE, CEP, ESTADO, CIDADE, LOGRADOURO, EMAIL, TELEFONE, CPF) 
    VALUES (@NOME, @SOBRENOME,@NACIONALIDADE,@CEP,@ESTADO,@CIDADE,@LOGRADOURO,@EMAIL,@TELEFONE, @CPF)

    SELECT SCOPE_IDENTITY()
END
GO

ALTER PROC FI_SP_PesqCliente
    @iniciarEm int,
    @quantidade int,
    @campoOrdenacao varchar(200),
    @crescente bit
AS
BEGIN
    DECLARE @SCRIPT NVARCHAR(MAX)
    DECLARE @CAMPOS NVARCHAR(MAX)
    DECLARE @ORDER VARCHAR(50)

    IF(@campoOrdenacao = 'EMAIL')
        SET @ORDER =  ' EMAIL '
    ELSE
        SET @ORDER = ' NOME '

    IF(@crescente = 0)
        SET @ORDER = @ORDER + ' DESC'
    ELSE
        SET @ORDER = @ORDER + ' ASC'

    SET @CAMPOS = '@iniciarEm int,@quantidade int'
    SET @SCRIPT = 
    'SELECT ID, NOME, SOBRENOME, NACIONALIDADE, CEP, ESTADO, CIDADE, LOGRADOURO, EMAIL, TELEFONE, CPF FROM
        (SELECT ROW_NUMBER() OVER (ORDER BY ' + @ORDER + ') AS Row, ID, NOME, SOBRENOME, NACIONALIDADE, CEP, ESTADO, CIDADE, LOGRADOURO, EMAIL, TELEFONE, CPF FROM CLIENTES WITH(NOLOCK))
        AS ClientesWithRowNumbers
    WHERE Row > @iniciarEm AND Row <= (@iniciarEm+@quantidade) ORDER BY'
    
    SET @SCRIPT = @SCRIPT + @ORDER
            
    EXECUTE SP_EXECUTESQL @SCRIPT, @CAMPOS, @iniciarEm, @quantidade

    SELECT COUNT(1) FROM CLIENTES WITH(NOLOCK)
END
GO

-- Finalizando transação
IF @@ERROR <> 0
   AND @@TRANCOUNT > 0
BEGIN
    ROLLBACK;
END

IF OBJECT_ID(N'tempdb..#tmpErrors') IS NULL
    CREATE TABLE [#tmpErrors] (
        Error INT
    );

IF @@TRANCOUNT = 0
BEGIN
    INSERT INTO #tmpErrors (Error)
    VALUES (1);
    BEGIN TRANSACTION;
END
GO

IF EXISTS (SELECT * FROM #tmpErrors) ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT > 0 
BEGIN
    PRINT N'A parte transacionada da atualização do banco de dados obteve êxito.'
    COMMIT TRANSACTION
END
ELSE 
    PRINT N'A parte transacionada da atualização do banco de dados falhou.'
GO

IF (SELECT OBJECT_ID('tempdb..#tmpErrors')) IS NOT NULL DROP TABLE #tmpErrors
GO

PRINT N'Atualização concluída.';
GO
