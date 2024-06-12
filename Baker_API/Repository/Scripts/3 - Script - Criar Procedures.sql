USE DB_BAKER
GO


--------------------------------------------------------------------------------------------------------------------------------------------
-- SELECIONAR DADOS DO USUARIO
--------------------------------------------------------------------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.spSELUsuario 
    @CD_USUARIO UNIQUEIDENTIFIER = null
AS
BEGIN

	SELECT	US.CD_USUARIO,
			NM_USUARIO    = CASE WHEN TP.TP_USUARIO IS NOT NULL THEN US.NM_USUARIO + ' (' + TP.TP_USUARIO + ')' ELSE '' END,
			TP.TP_USUARIO

	FROM	dbo.TBL_USUARIOS	US	WITH(NOLOCK)

		OUTER APPLY
		(
			SELECT TP_USUARIO = CASE WHEN LEN(US.CD_CPF_CNPJ) = 11 THEN 'CLIENTE' 
							         WHEN LEN(US.CD_CPF_CNPJ) = 14 THEN 'PADEIRO' ELSE 'NÂO DEFINIDO'END

		) TP

	WHERE	US.CD_USUARIO = @CD_USUARIO OR @CD_USUARIO IS NULL


END
GO


--------------------------------------------------------------------------------------------------------------------------------------------
-- INSERIR PRODUTOS
--------------------------------------------------------------------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.spINSProduto
	@CD_PRODUTO          INT,
	@CD_USUARIO          UNIQUEIDENTIFIER,
	@NM_PRODUTO          VARCHAR(100), 
	@DS_PRODUTO          VARCHAR(300),
	@VL_PRECO            DECIMAL(6,2),
    @LS_ALIMENTOS_RESTRITOS_PRODUTO VARCHAR(MAX) = NULL,    -- Exemplo: 
                                                    --'<ALIMENTOSRESTRITOS>
                                                    --    <ITEM>
                                                    --        <CD_ALIMENTO_RESTRITO>1</CD_ALIMENTO_RESTRITO>
                                                    --    </ITEM>
                                                    --    <ITEM>
                                                    --        <CD_ALIMENTO_RESTRITO>2</CD_ALIMENTO_RESTRITO>
                                                    --    </ITEM>
                                                    --</ALIMENTOSRESTRITOS>'
    @VB_IMAGEM           VARBINARY(MAX)
AS
BEGIN


    -- CADASTRO DE PODUTOS
    INSERT INTO dbo.TBL_PRODUTOS
    (
		CD_USUARIO,
		NM_PRODUTO, 
		DS_PRODUTO,
		VL_PRECO,
        BO_CANCELADO,
        VB_IMAGEM
    )
    VALUES
    (
		@CD_USUARIO,
		@NM_PRODUTO, 
		@DS_PRODUTO,
		@VL_PRECO,
        0,
        @VB_IMAGEM
    )

    -- ADQUIRE O ID DO PRODUTO QUE ACABOU DE SER CADASTRADO 
    SET @CD_PRODUTO = SCOPE_IDENTITY()


    -- CADASTRAR PRODUTO E SEU ALIMENTOS RESTRITIVOS
    IF (@LS_ALIMENTOS_RESTRITOS_PRODUTO IS NOT NULL)
    BEGIN

        declare @hDoc AS INT    
        exec    sp_xml_preparedocument @hDoc OUTPUT, @LS_ALIMENTOS_RESTRITOS_PRODUTO

        ;WITH Dados AS  
        (  
            SELECT  
	                CD_PRODUTO = @CD_PRODUTO,
                    CD_ALIMENTO_RESTRITO
    
            FROM OPENXML(@hDoc, '/ALIMENTOSRESTRITOS/ITEM')  
            WITH  
            (     
                CD_ALIMENTO_RESTRITO INT 'CD_ALIMENTO_RESTRITO'
            )
        )   
        MERGE   dbo.TBL_PRODUTOS_ALIMENTOS_RESTRITOS as TargetTbl  
        USING   Dados   as SourceTbl  
  
        ON  TargetTbl.CD_PRODUTO            = SourceTbl.CD_PRODUTO  
        and TargetTbl.CD_ALIMENTO_RESTRITO  = SourceTbl.CD_ALIMENTO_RESTRITO  
   
        WHEN NOT MATCHED 
             THEN INSERT (CD_PRODUTO, CD_ALIMENTO_RESTRITO) VALUES (SourceTbl.CD_PRODUTO, SourceTbl.CD_ALIMENTO_RESTRITO);

    END

END
GO


--------------------------------------------------------------------------------------------------------------------------------------------
-- ALTERAR PRODUTOS
--------------------------------------------------------------------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.spUPDProduto
	@CD_PRODUTO INT,
	@CD_USUARIO UNIQUEIDENTIFIER,
	@NM_PRODUTO VARCHAR(100), 
	@DS_PRODUTO VARCHAR(300),
	@VL_PRECO DECIMAL(6,2),
    @LS_ALIMENTOS_RESTRITOS_PRODUTO VARCHAR(MAX) = NULL,    -- Exemplo: 
                                                    --'<ALIMENTOSRESTRITOS>
                                                    --    <ITEM>
                                                    --        <CD_ALIMENTO_RESTRITO>1</CD_ALIMENTO_RESTRITO>
                                                    --    </ITEM>
                                                    --    <ITEM>
                                                    --        <CD_ALIMENTO_RESTRITO>2</CD_ALIMENTO_RESTRITO>
                                                    --    </ITEM>
                                                    --</ALIMENTOSRESTRITOS>'
    @VB_IMAGEM           VARBINARY(MAX)  = NULL

AS
BEGIN

    -- LIMPA TODOS OS ALIMENTOS RESTRITOS REFERENTE AO PRODUTO
    DELETE FROM dbo.TBL_PRODUTOS_ALIMENTOS_RESTRITOS WHERE CD_PRODUTO = @CD_PRODUTO      

    -- ATUALIZAR OS DADOS DO PRODUTO
    UPDATE  dbo.TBL_PRODUTOS
    SET
		CD_USUARIO	= @CD_USUARIO,
		NM_PRODUTO	= @NM_PRODUTO,
		DS_PRODUTO	= @DS_PRODUTO,
		VL_PRECO 	= @VL_PRECO,
        VB_IMAGEM   = isnull(@VB_IMAGEM, VB_IMAGEM) -- SE a variavel imagem for Null, então salva o proprio valor salvo no campo da tabela.
	WHERE
        CD_PRODUTO = @CD_Produto


     -- CADASTRAR PRODUTO E SEU ALIMENTOS RESTRITIVOS
    IF (@LS_ALIMENTOS_RESTRITOS_PRODUTO IS NOT NULL)
    BEGIN

        declare @hDoc AS INT    
        exec    sp_xml_preparedocument @hDoc OUTPUT, @LS_ALIMENTOS_RESTRITOS_PRODUTO

        ;WITH Dados AS  
        (  
            SELECT  
	                CD_PRODUTO = @CD_PRODUTO,
                    CD_ALIMENTO_RESTRITO
    
            FROM OPENXML(@hDoc, '/ALIMENTOSRESTRITOS/ITEM')  
            WITH  
            (     
                CD_ALIMENTO_RESTRITO INT 'CD_ALIMENTO_RESTRITO'
            )
        )   
        MERGE   dbo.TBL_PRODUTOS_ALIMENTOS_RESTRITOS as TargetTbl  
        USING   Dados   as SourceTbl  
  
        ON  TargetTbl.CD_PRODUTO            = SourceTbl.CD_PRODUTO  
        and TargetTbl.CD_ALIMENTO_RESTRITO  = SourceTbl.CD_ALIMENTO_RESTRITO  
   
        WHEN NOT MATCHED 
             THEN INSERT (CD_PRODUTO, CD_ALIMENTO_RESTRITO) VALUES (SourceTbl.CD_PRODUTO, SourceTbl.CD_ALIMENTO_RESTRITO);

    END

END
GO


--------------------------------------------------------------------------------------------------------------------------------------------
-- EXCLUIR PRODUTOS
--------------------------------------------------------------------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.spDELProduto
    @CD_PRODUTO INT
AS
BEGIN


    -- A EXLUSÃO DO PRODUTO SERÁ IDENTIFICADO PELA COLUNA BO_CANCELADO, E NÃO EXLCUIDO DA TABELA, 
    -- ASSIM EVITA A PERDA DO HITÓRICO DESSE PRODUTO DE COMPRAS DESTES PRODUTO
    UPDATE  dbo.TBL_PRODUTOS
    SET     BO_CANCELADO = 1
	WHERE   CD_PRODUTO = @CD_Produto

END
GO

--------------------------------------------------------------------------------------------------------------------------------------------
-- SELECIONAR DADOS DO PRODUTO
--------------------------------------------------------------------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.spSELProduto
    @CD_PRODUTO INT
AS
BEGIN

	-- ESTA CTE RETORNA TODOS OS PRODUTOS DO PADEIRO, E SEUS RESPACTIVOS ALIMENTOS RESTRITOS EM FORMATO XML
    ;WITH ALIMENTOS AS
    (
		SELECT
				P.CD_PRODUTO,
				LS_ALIMENTOS_RESTRITOS_PRODUTO = (
					                                SELECT	CD_ALIMENTO_RESTRITO = AR.CD_ALIMENTO_RESTRITO,
							                                DS_ALIMENTO			 = AR.DS_ALIMENTO
					                                FROM	dbo.TBL_PRODUTOS_ALIMENTOS_RESTRITOS PA WITH (NOLOCK)
							                                INNER JOIN dbo.TBL_ALIMENTOS_RESTRITOS AR WITH (NOLOCK) ON PA.CD_ALIMENTO_RESTRITO = AR.CD_ALIMENTO_RESTRITO
					                                WHERE	P.CD_PRODUTO = PA.CD_PRODUTO
					                                FOR XML PATH('ITEM'), ROOT('ALIMENTOSRESTRITOS'), TYPE
				                                 )
		FROM	dbo.TBL_PRODUTOS P WITH (NOLOCK)
				INNER JOIN dbo.TBL_PRODUTOS_ALIMENTOS_RESTRITOS PA WITH (NOLOCK) 
				ON P.CD_PRODUTO = PA.CD_PRODUTO
		GROUP BY
				P.CD_USUARIO, P.CD_PRODUTO  
    )    
	SELECT
			PR.CD_PRODUTO,
			PR.NM_PRODUTO, 
			PR.DS_PRODUTO,
			PR.VL_PRECO,
			PR.VB_IMAGEM,
			LS_ALIMENTOS_RESTRITOS_PRODUTO = AL.LS_ALIMENTOS_RESTRITOS_PRODUTO
		
    FROM    dbo.TBL_PRODUTOS PR  WITH (NOLOCK)

            LEFT JOIN   ALIMENTOS       AL
			ON	AL.CD_PRODUTO = PR.CD_PRODUTO

	WHERE
			(PR.CD_PRODUTO = @CD_PRODUTO OR @CD_PRODUTO is null)
    AND		PR.BO_CANCELADO = 0

END
GO


--------------------------------------------------------------------------------------------------------------------------------------------
-- LISTAR PRODUTOS POR PADEIRO
--------------------------------------------------------------------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.spLSTProduto
    @CD_USUARIO UNIQUEIDENTIFIER
AS
BEGIN

	-- ESTA CTE RETORNA TODOS OS PRODUTOS DO PADEIRO, E SEUS RESPECTIVOS ALIMENTOS RESTRITOS EM FORMATO XML
    ;WITH ALIMENTOS_RESTRITOS_PRODUTO AS
    (
		SELECT
				P.CD_USUARIO,
				P.CD_PRODUTO,
				LS_ALIMENTOS_RESTRITOS_PRODUTO = (
					                                SELECT	CD_ALIMENTO_RESTRITO = AR.CD_ALIMENTO_RESTRITO,
							                                DS_ALIMENTO			 = AR.DS_ALIMENTO
					                                FROM	dbo.TBL_PRODUTOS_ALIMENTOS_RESTRITOS PA WITH (NOLOCK)
							                                INNER JOIN dbo.TBL_ALIMENTOS_RESTRITOS AR WITH (NOLOCK) ON PA.CD_ALIMENTO_RESTRITO = AR.CD_ALIMENTO_RESTRITO
					                                WHERE	P.CD_PRODUTO = PA.CD_PRODUTO
					                                FOR XML PATH('ITEM'), ROOT('ALIMENTOSRESTRITOS'), TYPE
                                                 )
		FROM	dbo.TBL_PRODUTOS P WITH (NOLOCK)
				INNER JOIN dbo.TBL_PRODUTOS_ALIMENTOS_RESTRITOS PA WITH (NOLOCK) 
				ON P.CD_PRODUTO = PA.CD_PRODUTO
        WHERE   P.BO_CANCELADO = 0
		GROUP BY
				P.CD_USUARIO, P.CD_PRODUTO  
    )
    ,   ALIMENTOS_RESTRITOS_PADEIRO AS
	(
        SELECT 
                CD_USUARIO = PD.CD_USUARIO,
			    LS_ALIMENTOS_RESTRITOS_PADEIRO = (
                                                    SELECT  DISTINCT
                                                            AR.CD_ALIMENTO_RESTRITO AS "CD_ALIMENTO_RESTRITO",
                                                            AR.DS_ALIMENTO AS "DS_ALIMENTO"
                                        
                                                    FROM    dbo.TBL_PRODUTOS AS PD_INNER
                                                
                                                            INNER JOIN dbo.TBL_PRODUTOS_ALIMENTOS_RESTRITOS AS PA_INNER ON PA_INNER.CD_PRODUTO = PD_INNER.CD_PRODUTO
                                                            INNER JOIN dbo.TBL_ALIMENTOS_RESTRITOS AS AR ON PA_INNER.CD_ALIMENTO_RESTRITO = AR.CD_ALIMENTO_RESTRITO

                                                    WHERE   PD_INNER.CD_USUARIO = PD.CD_USUARIO
                                                    AND     PD_INNER.BO_CANCELADO = 0
                                                    FOR XML PATH('ITEM'), ROOT('ALIMENTOSRESTRITOS'), TYPE
									             )
		FROM 
			dbo.TBL_PRODUTOS AS PD
        WHERE   PD.BO_CANCELADO = 0
		GROUP BY 
			PD.CD_USUARIO
	)  
	SELECT
			PR.CD_PRODUTO,
			PR.CD_USUARIO,
			PR.NM_PRODUTO, 
			PR.DS_PRODUTO,
			PR.VL_PRECO,
			PR.VB_IMAGEM,
			AE.LS_ALIMENTOS_RESTRITOS_PADEIRO,
			AO.LS_ALIMENTOS_RESTRITOS_PRODUTO
		
    FROM    dbo.TBL_PRODUTOS				PR  WITH (NOLOCK)

            LEFT JOIN ALIMENTOS_RESTRITOS_PRODUTO AO
            ON  AO.CD_USUARIO = PR.CD_USUARIO
			AND AO.CD_PRODUTO = PR.CD_PRODUTO

            LEFT JOIN ALIMENTOS_RESTRITOS_PADEIRO AE
            ON  AE.CD_USUARIO = PR.CD_USUARIO

	WHERE
			(PR.CD_USUARIO = @CD_USUARIO OR @CD_USUARIO is null)
    AND		PR.BO_CANCELADO = 0

END
GO


--------------------------------------------------------------------------------------------------------------------------------------------
-- LISTAR LOCALIZACAO DOS PADEIROS
--------------------------------------------------------------------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.spLSTLocalizacaoPadeiros
    @NM_CIDADE  VARCHAR(50) = NULL,
    @LS_ALIMENTOS_RESTRITOS XML = NULL
									    --'<ALIMENTOSRESTRITOS>
									    --    <ITEM>
									    --        <CD_ALIMENTO_RESTRITO>1</CD_ALIMENTO_RESTRITO>
									    --    </ITEM>
									    --    <ITEM>
									    --        <CD_ALIMENTO_RESTRITO>2</CD_ALIMENTO_RESTRITO>
									    --    </ITEM>
AS
BEGIN

	DECLARE @hDoc AS INT    
    EXEC    sp_xml_preparedocument @hDoc OUTPUT, @LS_ALIMENTOS_RESTRITOS

	;WITH RESTRITOS AS
	(
		SELECT  
				CD_ALIMENTO_RESTRITO
    
		FROM	OPENXML(@hDoc, '/ALIMENTOSRESTRITOS/ITEM')  
		WITH  
		(     
			CD_ALIMENTO_RESTRITO INT 'CD_ALIMENTO_RESTRITO'
		)
	)
    ,   ALIMENTOS_RESTRITOS_PADEIRO AS
	(
        SELECT 
                CD_USUARIO = PD.CD_USUARIO,
			    LS_ALIMENTOS_RESTRITOS_PADEIRO = (
                                                    SELECT  DISTINCT
                                                            AR.CD_ALIMENTO_RESTRITO AS "CD_ALIMENTO_RESTRITO",
                                                            AR.DS_ALIMENTO AS "DS_ALIMENTO"
                                        
                                                    FROM    dbo.TBL_PRODUTOS AS PD_INNER
                                                
                                                            INNER JOIN dbo.TBL_PRODUTOS_ALIMENTOS_RESTRITOS AS PA_INNER ON PA_INNER.CD_PRODUTO = PD_INNER.CD_PRODUTO
                                                            INNER JOIN dbo.TBL_ALIMENTOS_RESTRITOS AS AR ON PA_INNER.CD_ALIMENTO_RESTRITO = AR.CD_ALIMENTO_RESTRITO

                                                    WHERE   PD_INNER.CD_USUARIO = PD.CD_USUARIO
                                                    AND		PD_INNER.BO_CANCELADO = 0
                                                    FOR XML PATH('ITEM'), ROOT('ALIMENTOSRESTRITOS'), TYPE
									             )
		FROM    dbo.TBL_PRODUTOS AS PD

                INNER JOIN dbo.TBL_PRODUTOS_ALIMENTOS_RESTRITOS   PA  WITH (NOLOCK)
                ON PA.CD_PRODUTO = PD.CD_PRODUTO

       WHERE   PD.BO_CANCELADO = 0

		GROUP BY 
			    PD.CD_USUARIO
	)   
    -- RETORNA OS DADOS DOS PADEIROS CADASTRADOS POR CIDADE.
    SELECT
            US.CD_USUARIO,
            NM_USUARIO      = CASE WHEN TP.TP_USUARIO IS NOT NULL THEN US.NM_USUARIO + ' (' + TP.TP_USUARIO + ')' ELSE '' END,
            US.DS_EMAIL,
            US.DS_TELEFONE,
            US.NM_ESTADO,
            US.NM_CIDADE,
            US.DS_ENDERECO,
            US.CD_CEP,
            US.CD_SENHA,
            US.CD_CPF_CNPJ,
            AE.LS_ALIMENTOS_RESTRITOS_PADEIRO,
            RS.FILTRAR_RESTRICAO

    FROM    dbo.TBL_USUARIOS                        US  WITH (NOLOCK)

            LEFT JOIN ALIMENTOS_RESTRITOS_PADEIRO   AE  WITH (NOLOCK)
            ON  AE.CD_USUARIO = US.CD_USUARIO
	
            OUTER APPLY
            (
                SELECT TP_USUARIO = CASE WHEN LEN(US.CD_CPF_CNPJ) = 11 THEN 'CLIENTE' 
                                         WHEN LEN(US.CD_CPF_CNPJ) = 14 THEN 'PADEIRO' ELSE 'NÂO DEFINIDO'END
            ) TP


            OUTER APPLY
            (
                SELECT  DISTINCT 
                        PD.CD_USUARIO
                FROM    dbo.TBL_PRODUTOS PD WITH (NOLOCK)
                        INNER JOIN dbo.TBL_PRODUTOS_ALIMENTOS_RESTRITOS   PA  WITH (NOLOCK)
                        ON PA.CD_PRODUTO = PD.CD_PRODUTO
                WHERE   PA.CD_ALIMENTO_RESTRITO IN (SELECT CD_ALIMENTO_RESTRITO FROM RESTRITOS)
                AND     PD.CD_USUARIO = US.CD_USUARIO
                AND		PD.BO_CANCELADO = 0
            ) CL


            OUTER APPLY
            (
                SELECT FILTRAR_RESTRICAO = CASE WHEN CL.CD_USUARIO IS NOT NULL THEN 1 ELSE 0 END
            ) RS

    WHERE
    (       UPPER(TRIM(US.NM_CIDADE)) = UPPER(@NM_CIDADE) OR @NM_CIDADE IS NULL)
    AND     LEN(TRIM(US.CD_CPF_CNPJ)) = 14      -- 14 caracteres define ser um CNPJ (PJ) e 11 caracteres define ser um CPF (PF)
    AND    (
                (@LS_ALIMENTOS_RESTRITOS IS NOT NULL AND RS.FILTRAR_RESTRICAO = 1)
                OR
                (@LS_ALIMENTOS_RESTRITOS IS NULL     AND RS.FILTRAR_RESTRICAO = 0)
           ) 
END
GO


--------------------------------------------------------------------------------------------------------------------------------------------
-- RELATÓRIO DE VENDA POR PADEIRO
--------------------------------------------------------------------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.spRPTVendasPadeiros
    @CD_USUARIO  UNIQUEIDENTIFIER
AS
BEGIN

	SELECT
			PE.CD_PEDIDO,
			NM_PADEIRO          = PD.NM_USUARIO,
			NM_CLIENTE          = CASE WHEN TP.TP_USUARIO IS NOT NULL THEN US.NM_USUARIO + ' (' + TP.TP_USUARIO + ')' ELSE '' END,
			PE.DT_PEDIDO,
			PR.NM_PRODUTO,
			IT.QT_PRODUTO,
			PR.VL_PRECO,
            VL_SUBTOTAL         = PR.VL_PRECO * IT.QT_PRODUTO,
            VL.VL_TOTAL,
			US.NM_ESTADO,
			US.NM_CIDADE,
			US.DS_ENDERECO
	FROM
		dbo.TBL_PEDIDOS PE  WITH (NOLOCK)

        INNER JOIN dbo.TBL_ITENS_DO_PEDIDO  IT  WITH (NOLOCK)
        ON  PE.CD_PEDIDO = IT.CD_PEDIDO

        INNER JOIN dbo.TBL_PRODUTOS         PR  WITH (NOLOCK)
        ON  IT.CD_PRODUTO = PR.CD_PRODUTO
        AND		PR.BO_CANCELADO = 0

        INNER JOIN dbo.TBL_USUARIOS         US  WITH (NOLOCK)
        ON  PE.CD_CLIENTE = US.CD_USUARIO

        INNER JOIN dbo.TBL_USUARIOS         PD  WITH (NOLOCK)
        ON  PE.CD_PADEIRO = PD.CD_USUARIO

		OUTER APPLY
		(
			SELECT TP_USUARIO = CASE WHEN LEN(US.CD_CPF_CNPJ) = 11 THEN 'CLIENTE' 
							         WHEN LEN(US.CD_CPF_CNPJ) = 14 THEN 'PADEIRO' ELSE 'NÂO DEFINIDO'END

		) TP

        --  CALCULA O VALOR TOTAL DOS PRODUTOS, POR PEDIDO
        OUTER APPLY
        (
            SELECT  
                    P.CD_PEDIDO,
                    VL_TOTAL = SUM(R.VL_PRECO)
            
            FROM    dbo.TBL_PEDIDOS                     P  WITH (NOLOCK)

                    INNER JOIN dbo.TBL_ITENS_DO_PEDIDO  I  WITH (NOLOCK)
                    ON  P.CD_PEDIDO = I.CD_PEDIDO

                    INNER JOIN dbo.TBL_PRODUTOS         R  WITH (NOLOCK)
                    ON  I.CD_PRODUTO = R.CD_PRODUTO
                    AND		R.BO_CANCELADO = 0
            WHERE
                    P.CD_PEDIDO = PE.CD_PEDIDO
            GROUP BY
                    P.CD_PEDIDO
        ) VL

    WHERE
            PE.CD_PADEIRO = @CD_USUARIO

END
GO

--------------------------------------------------------------------------------------------------------------------------------------------
-- CADASTRAR CARRINHO
--------------------------------------------------------------------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.spINSCarrinho 
	@CD_USUARIO UNIQUEIDENTIFIER,
  	@CD_PRODUTO INT,
	@QT_PRODUTO SMALLINT, 
	@VL_PRECO   DECIMAL(6,2)

AS    
BEGIN    

    -- CADASTRAR UM CARRINHO POR CLIENTE
    ;WITH DADOS AS
    (
        SELECT  CD_USUARIO = @CD_USUARIO
    )
    MERGE   dbo.TBL_CARRINHOS AS TargetTbl  
    USING   DADOS             AS SourceTbl  
  
    ON  TargetTbl.CD_USUARIO = SourceTbl.CD_USUARIO
  
    WHEN NOT MATCHED
         THEN INSERT (CD_USUARIO) VALUES (SourceTbl.CD_USUARIO);


    -- OBTEM O CODIGO DO CARRINHO
    DECLARE @CD_CARRINHO INT

    SELECT  @CD_CARRINHO = CD_CARRINHO
    FROM    dbo.TBL_CARRINHOS CA WITH (NOLOCK)
    WHERE   CA.CD_USUARIO = @CD_USUARIO


    -- SALVA O PRODUTO PARA O CARRINHO/CLIENTE, CASO O PRODUTO JÁ ESTEJA CADASTRADO SERÁ ATUALIZADO APENAS A QUANTIDADE DO PRODUTO
    ;WITH Dados AS  
    (  
        SELECT  
                CD_CARRINHO = @CD_CARRINHO,
	            CD_PRODUTO  = @CD_PRODUTO,
                QT_PRODUTO  = @QT_PRODUTO,
                VL_PRECO    = @VL_PRECO
    )   
    MERGE   dbo.TBL_ITENS_DO_CARRINHO as TargetTbl  
    USING   Dados   as SourceTbl  
  
    ON  TargetTbl.CD_CARRINHO = SourceTbl.CD_CARRINHO  
    and TargetTbl.CD_PRODUTO  = SourceTbl.CD_PRODUTO  
   
    WHEN NOT MATCHED 
            THEN INSERT (CD_CARRINHO, CD_PRODUTO, QT_PRODUTO, VL_PRECO) VALUES (SourceTbl.CD_CARRINHO, SourceTbl.CD_PRODUTO, SourceTbl.QT_PRODUTO, SourceTbl.VL_PRECO)

	WHEN MATCHED THEN
		UPDATE SET QT_PRODUTO = SourceTbl.QT_PRODUTO;

END
GO


--------------------------------------------------------------------------------------------------------------------------------------------
-- LISTAR PRODUTOS DO CARRINHO
--------------------------------------------------------------------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.spLSTCarrinho
	@CD_USUARIO UNIQUEIDENTIFIER
AS
BEGIN

     SELECT  
		     IT.CD_ITENS_DO_CARRINHO,
             CA.CD_CARRINHO,
             PR.CD_PRODUTO,
             PR.NM_PRODUTO,
             IT.QT_PRODUTO,
             PR.VL_PRECO,
             PR.VB_IMAGEM

     FROM    dbo.TBL_CARRINHOS   CA  WITH (NOLOCK)

             INNER JOIN dbo.TBL_ITENS_DO_CARRINHO  IT  WITH (NOLOCK)
             ON  CA.CD_CARRINHO = IT.CD_CARRINHO

             INNER JOIN dbo.TBL_PRODUTOS  PR  WITH (NOLOCK)
             ON  IT.CD_PRODUTO = PR.CD_PRODUTO
             AND		PR.BO_CANCELADO = 0

    WHERE   CA.CD_USUARIO = @CD_USUARIO
END
GO


--------------------------------------------------------------------------------------------------------------------------------------------
-- REMOVER PRODUTO DO CARRINHO
--------------------------------------------------------------------------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.spDELCarrinho 
	@CD_ITENS_DO_CARRINHO INT

AS    
BEGIN  

    -- EXCLUI DA TABELA OS PRODUTOS DO CARRINHO DO CLIENTE
    DELETE FROM dbo.TBL_ITENS_DO_CARRINHO WHERE CD_ITENS_DO_CARRINHO = @CD_ITENS_DO_CARRINHO

END
GO


