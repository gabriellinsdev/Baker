let CD_PADEIRO = sessionStorage.getItem('CD_PADEIRO');
let NM_PADEIRO = sessionStorage.getItem('NM_PADEIRO');
let CD_CLIENTE = sessionStorage.getItem('CD_CLIENTE');
let NM_CLIENTE = sessionStorage.getItem('NM_CLIENTE');
let alimentos = [];


const nomePadeiro = document.querySelector('#padeiro1')
nomePadeiro.innerHTML = NM_PADEIRO

buscarProdutosPadeiro();

function parseAlimentosRestritos(xmlString) {
    const parser = new DOMParser();
    const xmlDoc = parser.parseFromString(xmlString, "text/xml");
    const items = xmlDoc.getElementsByTagName("ITEM");
    let alimentos = [];
    for (let item of items) {
      let alimento = item.getElementsByTagName("DS_ALIMENTO")[0].textContent;
      alimentos.push(alimento);
    }
    return alimentos;
  }

function buscarProdutosPadeiro() {
    fetch(`${BASE_URL_API2}/Produtos/List?CD_USUARIO=${CD_PADEIRO}`, {
        headers:{
            "Content-Type":"application/json",
            "Accept":"*/*"
        },
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
        
        let alimentos = parseAlimentosRestritos(produtos[0].lS_ALIMENTOS_RESTRITOS_PADEIRO);
        document.getElementById("restricao").textContent = alimentos[0] || "";
        document.getElementById("restricaoExtra").textContent = alimentos[1] || "";
        document.getElementById("restricaoExtra1").textContent = alimentos[2] || "";
        document.getElementById("restricaoExtra2").textContent = alimentos[3] || "";
        preencherProdutosNaPagina(produtos);
    })  
    .catch(error => {
        console.error('Erro:', error);
    });
}

function preencherProdutosNaPagina(produtos) {
    const produtosContainer = document.querySelector('.produtos_do_padeiro-produtos');

    produtos.forEach(produto => {
        const produtoCard = document.createElement('div');
        produtoCard.classList.add('produtos_do_padeiro-card-produto');

        const produtoInfo = document.createElement('div');
        produtoInfo.classList.add('produtos_do_padeiro-produto');

        const imagem = document.createElement('img');
        imagem.classList.add('produtos_do_padeiro-imagem-produto');
        imagem.src = `data:image/jpeg;base64,${produto.vB_IMAGEM}`;
        imagem.alt = "Imagem do produto";

        const titulo = document.createElement('div');
        titulo.classList.add('produtos_do_padeiro-titulo');

        const tituloPao = document.createElement('h4');
        tituloPao.classList.add('produtos_do_padeiro-titulo-pao');
        tituloPao.textContent = produto.nM_PRODUTO;

        const preco = document.createElement('h4');
        preco.classList.add('produtos_do_padeiro-titulo-preco');
        preco.textContent = produto.vL_PRECO ? `R$ ${produto.vL_PRECO.toFixed(2)}` : 'Preço indisponível';

        const filtros = document.createElement('div');
        filtros.classList.add('produtos_do_padeiro-filtros');

        const alimentosRestritosXML =  produto.lS_ALIMENTOS_RESTRITOS_PRODUTO;
        const parser = new DOMParser();
        const xmlDoc = parser.parseFromString(alimentosRestritosXML, "text/xml");
        const items = xmlDoc.querySelectorAll('ITEM');
        items.forEach(item => {
            const filtroElement = document.createElement('p');
            filtroElement.classList.add('produtos_do_padeiro-filtro-pao');
            filtroElement.textContent = item.querySelector('DS_ALIMENTO').textContent;
            filtros.appendChild(filtroElement);
        });

        const botaoAdicionar = document.createElement('button');
        botaoAdicionar.classList.add('produtos_do_padeiro-btn-adicionar');
        botaoAdicionar.textContent = 'Adicionar no Carrinho';

        produtoInfo.appendChild(imagem);
        produtoInfo.appendChild(titulo);
        titulo.appendChild(tituloPao);
        titulo.appendChild(preco);
        produtoInfo.appendChild(filtros);
        produtoInfo.appendChild(botaoAdicionar);

        produtoCard.appendChild(produtoInfo);
        produtosContainer.appendChild(produtoCard);
    });
}




