document.addEventListener('DOMContentLoaded', function() {
    fetchPedidos();
});

function fetchPedidos() {
    const token = localStorage.getItem('token');
    const userId = token;

    fetch(`${BASE_URL}/Pedido/Usuario/${userId}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok ' + response.statusText);
        }
        return response.json();
    })
    .then(pedidos => {
        displayPedidos(pedidos);
    })
    .catch(error => {
        console.error('Erro ao buscar pedidos:', error);
        document.getElementById('sem-pedidos').style.display = 'block';
    });
}

function displayPedidos(pedidos) {
    const container = document.getElementById('pedidos-container');
    if (pedidos.length === 0) {
        document.getElementById('sem-pedidos').style.display = 'block';
    } else {
        pedidos.forEach(pedido => {
            const pedidoDiv = document.createElement('div');
            pedidoDiv.className = 'conf-historico-pedidos-main';
            
            pedidoDiv.innerHTML = `
                <div style='display: flex; justify-content: space-between; padding: 0; margin: 0'>
                    <div class="conf-historico-pedidos-info">
                        <h4>ID do Pedido: ${pedido.codigoPedido}</h4>
                        <p>Data do Pedido: ${new Date(pedido.data).toLocaleDateString()}</p>
                        <p>
                            Endereço: ${pedido.endereco}.
                            ${pedido.numero ? 'Nº ' + pedido.numero + '.' : ''}
                            ${pedido.complemento ? 'Complemento: ' + pedido.complemento + '.' : ''}
                            Cidade: ${pedido.cidade}.
                            Estado: ${pedido.estado}.
                        </p>
                    </div>
                    <div class="conf-historico-pedidos-valor">
                        <h4>Total R$ ${pedido.valor.toFixed(2)}</h4>
                    </div>
                    <div class="conf-historico-pedidos-btn">
                        <button id="btn-conf-verpedido" onclick="togglePedidoItens('${pedido.codigoPedido}')">Ver Pedido</button>
                    </div>
                </div>
                <div id="conf-historico-pedidos-itens" class="conf-historico-pedidos-mostrar pedido-itens-${pedido.codigoPedido}" style="display: none;">
                    <div class="line"></div>
                    <table class="conf-historico-pedidos-mostrando">
                        <thead>
                            <tr>
                                <th>Tipo do Item</th>
                                <th>Quantidade</th>
                                <th>Valor Unitário</th>
                                <th>Valor Total</th>
                            </tr>
                        </thead>
                        <tbody>
                            ${pedido.itens.map(item => `
                                <tr>
                                    <td>${item.nomeProduto}</td>
                                    <td>${item.quantidadeProduto > 1 ? item.quantidadeProduto+' unidades' : item.quantidadeProduto+' unidade'}</td>
                                    <td>R$ ${item.valor.toFixed(2)}</td>
                                    <td>R$ ${(item.valor * item.quantidadeProduto).toFixed(2)}</td>
                                </tr>
                            `).join('')}
                        </tbody>
                    </table>
                </div>
            `;
            container.appendChild(pedidoDiv);
        });
    }
}

function togglePedidoItens(pedidoId) {
    const itensDiv = document.getElementsByClassName(`pedido-itens-${pedidoId}`)[0];
    if (itensDiv.style.display === 'none' || itensDiv.style.display === '') {
        itensDiv.style.display = 'block';
    } else {
        itensDiv.style.display = 'none';
    }
}
