let cartItems = [];

async function fetchCartItems() {
    const userId = localStorage.getItem('token');
    if (!userId) {
        console.error('Usuário não encontrado no local storage');
        window.location.href = 'login.html'; // Redirecionar para a página de login
        return;
    }

    try {
        const response = await fetch(`${BASE_URL}/Carrinho/Itens/${userId}`);
        const cartResponse = await response.json();
        const cartItemsContainer = document.getElementById('cart-items');
        cartItemsContainer.innerHTML = '';
        console.log(cartResponse);

        if (cartResponse.status || cartResponse.message) {
            cartItemsContainer.innerHTML = '<div style="text-align: center;"><p>Sem pedidos</p></div>';
        } else {
            cartItems = cartResponse.sort((a, b) => a.codigoItemCarrinho - b.codigoItemCarrinho);
            renderCartItems();
            updateCartSummary();
        }
    } catch (error) {
        console.error('Erro ao buscar itens do carrinho:', error);
    }
}

function renderCartItems() {
    const cartItemsContainer = document.getElementById('cart-items');
    cartItemsContainer.innerHTML = '';

    cartItems.forEach(product => {
        const itemElement = document.createElement('section');
        itemElement.classList.add('carrinho-produto-adicionado');

        itemElement.innerHTML = `
            <div class="carrinho-img-produto">
                <img src="img/${product.nomeProduto}.png" alt="${product.nomeProduto}" class="img-carrinho">
            </div>
            <div class="carrinho-info-produtos">
                <h4>${product.nomeProduto}</h4>
                <p>R$ ${product.valor.toFixed(2)}</p>
            </div>
            <div class="carrinho-btns">
                <div class="carrinho-adc-exc">
                    <button class="carrinho-btn-controle" onclick="updateQuantity(${product.codigoItemCarrinho}, ${product.quantidade - 1})">-</button>
                    <button class="carrinho-btn-controle">${product.quantidade}</button>
                    <button class="carrinho-btn-controle" onclick="updateQuantity(${product.codigoItemCarrinho}, ${product.quantidade + 1})">+</button>
                </div>
                <button class="carrinho-excluir-produto" onclick="removeCartItem(${product.codigoItemCarrinho})">Remover</button>
            </div>
        `;
        cartItemsContainer.appendChild(itemElement);
    });
}

async function updateQuantity(codigoItemCarrinho, newQuantity) {
    const userId = localStorage.getItem('token');
    if (!userId) {
        console.error('Usuário não encontrado no local storage');
        window.location.href = 'login.html'; // Redirecionar para a página de login
        return;
    }

    if (newQuantity < 1) {
        removeCartItem(codigoItemCarrinho);
        return;
    }

    try {
        const response = await fetch(`${BASE_URL}/Carrinho/`, {
            method: 'PATCH',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                codigoItemDoCarrinho: codigoItemCarrinho,
                quantidade: newQuantity
            })
        });

        if (response.ok) {
            await fetchCartItems(); // Re-fetch cart items to get the updated quantities and sort them
        } else {
            console.error('Erro ao atualizar quantidade do item:', response.statusText);
        }
    } catch (error) {
        console.error('Erro ao atualizar quantidade do item:', error);
    }
}

async function removeCartItem(codigoItemCarrinho) {
    const userId = localStorage.getItem('token');
    if (!userId) {
        console.error('Usuário não encontrado no local storage');
        window.location.href = 'login.html'; // Redirecionar para a página de login
        return;
    }

    try {
        const response = await fetch(`${BASE_URL}/Carrinho/${codigoItemCarrinho}`, {
            method: 'DELETE'
        });

        if (response.ok) {
            cartItems = cartItems.filter(item => item.codigoItemCarrinho !== codigoItemCarrinho);
            renderCartItems();
            updateCartSummary();
        } else {
            console.error('Erro ao remover item do carrinho:', response.statusText);
        }
    } catch (error) {
        console.error('Erro ao remover item do carrinho:', error);
    }
}

function updateCartSummary() {
    let subtotal = 0;
    cartItems.forEach(item => {
        subtotal += item.quantidade * item.valor;
    });

    document.getElementById('subtotal').innerText = `R$ ${subtotal.toFixed(2)}`;
    // Supondo um valor fixo para frete, ajuste conforme necessário
    const frete = 10.00;
    document.getElementById('frete').innerText = `R$ ${frete.toFixed(2)}`;
    document.getElementById('total').innerText = `R$ ${(subtotal + frete).toFixed(2)}`;
}

async function addItemToCart(productId) {
    const userId = localStorage.getItem('token');
    if (!userId) {
        console.error('Usuário não encontrado no local storage');
        window.location.href = 'login.html'; // Redirecionar para a página de login
        return;
    }

    try {
        // Consultar o produto antes de adicionar ao carrinho
        const productResponse = await fetch(`${BASE_URL_API2}/Produtos/Get?CD_PRODUTO=${productId}`);
        const product = await productResponse.json();

        if (!product) {
            console.error('Produto não encontrado');
            return;
        }

        const response = await fetch(`${BASE_URL_API2}/Carrinho/Save`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                cD_USUARIO: userId,
                cD_PRODUTO: product.data.cD_PRODUTO,
                qT_PRODUTO: 1,
                vL_PRECO: product.data.vL_PRECO
            })
        });

        if (response.ok) {
            fetchCartItems();
        } else {
            console.error('Erro ao adicionar item ao carrinho:', response.statusText);
        }
    } catch (error) {
        console.error('Erro ao adicionar item ao carrinho:', error);
    }
}

async function finalizeOrder() {
    const userId = localStorage.getItem('token');
    if (!userId) {
        console.error('Usuário não encontrado no local storage');
        window.location.href = 'login.html'; // Redirecionar para a página de login
        return;
    }

    try {
        // Primeiro, obter as informações do usuário
        const userResponse = await fetch(`${BASE_URL}/Usuario/${userId}`);
        if (!userResponse.ok) {
            throw new Error('Erro ao obter informações do usuário');
        }
        const userData = await userResponse.json();

        console.log(userData)

        // Compor os dados do pedido
        const orderItems = cartItems.map(item => item.codigoItemCarrinho);

        const orderData = {
            codigoCliente: userId,
            estado: userData.estado,
            cidade: userData.cidade,
            endereco: userData.endereco,
            cep: userData.cep,
            observacao: '', // Adicione aqui qualquer observação que queira enviar
            codigoItemDoCarrinho: orderItems
        };

        // Log dos dados do pedido para debug
        console.log('Dados do pedido:', JSON.stringify(orderData, null, 2));

        const response = await fetch(`${BASE_URL}/Pedido`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(orderData)
        });

        if (response.ok) {
            window.location.href = 'index.html'; // Redirecionar para a página inicial
            await clearCart(); // Limpar o carrinho após a criação do pedido
        } else {
            const errorResponse = await response.text();
            console.error('Erro ao finalizar pedido:', response.statusText);
            console.error('Corpo da resposta de erro:', errorResponse);
        }
    } catch (error) {
        console.error('Erro ao finalizar pedido:', error);
    }
}



async function clearCart() {
    try {
        for (const item of cartItems) {
            const response = await fetch(`${BASE_URL}/Carrinho/${item.codigoItemCarrinho}`, {
                method: 'DELETE'
            });

            if (!response.ok) {
                console.error(`Erro ao remover item do carrinho (ID: ${item.codigoItemCarrinho}):`, response.statusText);
            }
        }
        cartItems = [];
        renderCartItems();
        updateCartSummary();
    } catch (error) {
        console.error('Erro ao limpar carrinho:', error);
    }
}

// Adicionar evento de clique ao botão de finalizar pedido
if(document.getElementsByClassName('carrinho-btn-finalizar-pedido')[0]){
    document.getElementsByClassName('carrinho-btn-finalizar-pedido')[0].addEventListener('click', finalizeOrder);
}
