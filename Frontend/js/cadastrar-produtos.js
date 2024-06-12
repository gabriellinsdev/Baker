let CD_PADEIRO = localStorage.getItem('token');


function validarFormulario() {
    // Obtendo os campos do formulário
    var nomeProduto = document.getElementById("nome-do-produto").value.trim();
    var precoProduto = document.getElementById("preco-do-produto").value.trim();
    var imagemProduto = document.getElementById("produto-imagem").value.trim();

    // Verificando se os campos obrigatórios estão preenchidos
    if (nomeProduto === "" || precoProduto === "" || imagemProduto === "") {
        alert("Por favor, preencha todos os campos obrigatórios.");
        return false; // Impede o envio do formulário
    }

    // Se todos os campos obrigatórios estiverem preenchidos, chama o método inserirProdutoPadeiro()
    inserirProdutoPadeiro();
}


function inserirProdutoPadeiro() {
    let formData = new FormData();
    formData.append('cD_PRODUTO', '0');
    formData.append('cD_USUARIO', CD_PADEIRO);
    formData.append('nM_PRODUTO', document.getElementById('nome-do-produto').value);
    formData.append('dS_PRODUTO', "teste ");
    formData.append('vL_PRECO', document.getElementById('preco-do-produto').value);
    formData.append('fF_IMAGEM', document.getElementById('produto-imagem').files[0]);

    let checkboxes = document.querySelectorAll('input[type=checkbox]:checked');
    let categorias = [];
    checkboxes.forEach(checkbox => {
        categorias.push(checkbox.value);
    });

    let categoriasXML = '<ALIMENTOSRESTRITOS>';
    categorias.forEach(categoria => {
        categoriasXML += '<ITEM>\n';
        categoriasXML += '<CD_ALIMENTO_RESTRITO>' + categoria + '</CD_ALIMENTO_RESTRITO>\n';
        categoriasXML += '</ITEM>\n';
    });
    categoriasXML += '</ALIMENTOSRESTRITOS>';


    // Adicionar o XML das categorias ao formData
    formData.append('lS_ALIMENTOS_RESTRITOS_PRODUTO', categoriasXML);
    console.log(categoriasXML)


    fetch(`${BASE_URL_API2}/Produtos/Insert`, {
        method: 'POST',
        mode: 'cors',
        body: formData
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Erro ao incluir o produto!');
        }
        // Verifica o status da resposta para determinar o sucesso da inclusão
        if (response.status === 200) {
            window.alert("Produto cadastrado com sucesso");
            window.location.href = 'padeiro-configuracoes-gerenciar-produtos.html';
            // Aqui você pode fazer alguma ação adicional, se necessário
        } else {
            throw new Error('Erro ao incluir o produto!');
        }
    })    
    .catch(error => {
        console.error('Erro ao incluir o produto: ', error);
    });
}