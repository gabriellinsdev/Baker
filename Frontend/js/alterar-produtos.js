let CD_PADEIRO = localStorage.getItem('token');
let CD_PRODUTO = localStorage.getItem('CD_PRODUTO');


// Inicializar uma lista vazia para armazenar os dados
let alimentosRestritos = [];
let imagemProduto

buscarProdutosPadeiro();

function buscarProdutosPadeiro() {
    fetch(`${BASE_URL_API2}/Produtos/Get?CD_PRODUTO=${CD_PRODUTO}`, {
        headers:{
            "Content-Type":"application/json",
            "Accept":"*/*"
        },
        method: 'GET',
        mode:"cors"
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Erro ao fazer a requisição');
        }
        return response.json();
    })
    .then(data => {
        let produtos = data.data;
        preencherProdutoNaPagina(produtos);
    })  
    .catch(error => {
        console.error('Erro:', error);
    });
}


function preencherProdutoNaPagina(produto) {

        // Preenchendo os campos do formulário com os dados do produto
        const nomeProdutoInput = document.getElementById('nome-do-produto');
        const precoProdutoInput = document.getElementById('preco-do-produto');
        const zeroGlutenCheckbox = document.getElementById('zeroGluten');
        const zeroLactoseCheckbox = document.getElementById('zeroLactose');
        const lowCarbCheckbox = document.getElementById('lowCarb');
        const artesanalCheckbox = document.getElementById('artesanal');

        nomeProdutoInput.value = produto.nM_PRODUTO;
        precoProdutoInput.value = produto.vL_PRECO;

        // Marcando os checkboxes de acordo com os alimentos restritos associados ao produto
        convertXMLToList(produto.lS_ALIMENTOS_RESTRITOS_PRODUTO);

        alimentosRestritos.forEach(alimentosRestritos => {
            switch (alimentosRestritos.CD_ALIMENTO_RESTRITO) {                
                case '1':
                    document.getElementById('zeroGluten').checked = true;
                    break;
                case '2':
                    document.getElementById('zeroLactose').checked = true;
                    break;
                case '3':
                    document.getElementById('lowCarb').checked = true;
                    break;
                case '4':
                    document.getElementById('artesanal').checked = true;
                    break;
                default:
                    console.log("Nenhum caso correspondente encontrado");
                    break;
            }
        });
}

function alterarProdutoPadeiro() {

    let formData = new FormData();
    formData.append('cD_PRODUTO', CD_PRODUTO);
    formData.append('cD_USUARIO', CD_PADEIRO);
    formData.append('nM_PRODUTO', document.getElementById('nome-do-produto').value);
    formData.append('dS_PRODUTO', "teste ");
    formData.append('vL_PRECO', document.getElementById('preco-do-produto').value);

    if (document.getElementById('produto-imagem').files[0] != null) {
        imagemProduto = document.getElementById('produto-imagem').files[0];
    } else {
        imagemProduto = null
    }

    formData.append('fF_IMAGEM', imagemProduto)

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


    fetch(`${BASE_URL_API2}/Produtos/Update`, {
        method: 'POST',
        mode: 'cors',
        body: formData
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Erro ao alterar produto!');
        }
        // Verifica o status da resposta para determinar o sucesso da inclusão
        if (response.status === 200) {
            window.alert("Produto alterado com sucesso");
            // Aqui você pode fazer alguma ação adicional, se necessário
        } else {
            throw new Error('Erro ao alterar produto!');
        }
    })    
    .catch(error => {
        console.error('Erro ao incluir o produto: ', error);
    });
}



function convertXMLToList (xmlString)
{
    // Criar um objeto DOMParser
    var parser = new DOMParser();

    // Parse do XML string para um objeto XML
    var xmlDoc = parser.parseFromString(xmlString, "text/xml");

    // Encontrar todos os elementos <ITEM>
    var items = xmlDoc.getElementsByTagName("ITEM");

    // Iterar sobre cada elemento <ITEM> e extrair os dados
    for (var i = 0; i < items.length; i++) {
        var item = items[i];
        var cdAlimentoRestrito = item.getElementsByTagName("CD_ALIMENTO_RESTRITO")[0].childNodes[0].nodeValue;
        var dsAlimento = item.getElementsByTagName("DS_ALIMENTO")[0].childNodes[0].nodeValue;
        
        // Criar um objeto com os dados e adicionar à lista
        alimentosRestritos.push({
            CD_ALIMENTO_RESTRITO: cdAlimentoRestrito,
            DS_ALIMENTO: dsAlimento
        });
    }
}
