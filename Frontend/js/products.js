// Guid selecionado do banco de dados
const CD_USUARIO = '3E918766-A22F-4DEB-9E7C-A70C829AA6F3'; 

function fetchProducts() {
    fetch(`${BASE_URL_API2}/Produtos/List?CD_USUARIO=${CD_USUARIO}`)
    .then(response => response.json())
    .then(data => {
        if (data.data && data.data.length > 0) {
            populateProductGallery(data.data);
        } else {
            console.error('No products found');
        }
    })
    .catch(error => {
        console.error('There has been a problem with your fetch operation:', error);
    });
}

function fetchPadeiroName() {
    fetch(`${BASE_URL_API2}/Usuario/Get?CD_USUARIO=${CD_USUARIO}`)
    .then(response => response.json())
    .then(data => {
        if (data.data && data.data.nM_USUARIO) {
            const padeiroName = data.data.nM_USUARIO.split(' ');
            document.getElementById('padeiroFulano').textContent = `Padeiro ${padeiroName[0]}`;
        } else {
            console.error('No user found');
        }
    })
    .catch(error => {
        console.error('There has been a problem with your fetch operation:', error);
    });
}

function populateProductGallery(products) {
    const gallery = document.getElementById('product-gallery');
    gallery.innerHTML = ''; // Limpar a galeria antes de adicionar novos produtos

    products.forEach(product => {
        const productCard = document.createElement('div');
        productCard.className = 'home-card-produtos';

        const productContent = `
            <div class="home-produtos">
                <img src="img/${product.nM_PRODUTO}.png" class="home-imagem-produto" alt="imagem de ${product.nM_PRODUTO}">
                <div class="home-titulo">
                    <h3 class="home-titulo-pao">${product.nM_PRODUTO}</h3>
                    <h3 class="home-titulo-preco">R$ ${product.vL_PRECO.toFixed(2)}</h3>
                </div>
                <div class="home-filtros">
                    ${parseAlimentosRestritos(product.lS_ALIMENTOS_RESTRITOS)}
                </div>
                <button onclick="addItemToCart(${product.cD_PRODUTO}, ${product.vL_PRECO})" class="home-btn-adicionar" data-preco="${product.vL_PRECO}" data-codigoproduto="${product.cD_PRODUTO}">Adicionar no Carrinho</button>
            </div>
        `;

        productCard.innerHTML = productContent;
        gallery.appendChild(productCard);
    });

    // setupAddToCartButtons(); // Re-configurar os botões após a atualização da galeria
}

function parseAlimentosRestritos(alimentosXML) {
    const parser = new DOMParser();
    const xmlDoc = parser.parseFromString(alimentosXML, 'text/xml');
    const items = xmlDoc.getElementsByTagName('ITEM');
    let html = '';

    for (let i = 0; i < items.length; i++) {
        const item = items[i];
        const alimento = item.getElementsByTagName('DS_ALIMENTO')[0].textContent;
        html += `<p class="home-filtro-pao">${alimento}</p>`;
    }

    return html;
}

window.fetchProducts = fetchProducts;
window.fetchPadeiroName = fetchPadeiroName;
window.populateProductGallery = populateProductGallery;
window.parseAlimentosRestritos = parseAlimentosRestritos;
