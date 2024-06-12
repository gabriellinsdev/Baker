document.addEventListener('DOMContentLoaded', function() {
    const userId = localStorage.getItem('token');
    if (!userId) {
        console.error('Usuário não encontrado no local storage');
        window.location.href = 'login.html'; // Redirecionar para a página de login
        return;
    }

    buscarVendasPorPadeiro(userId);
});



function buscarVendasPorPadeiro(CD_PADEIRO) {
    // Fazer a requisição para a API
    fetch(`${BASE_URL_API2}/Padeiros/SalesReport?CD_USUARIO=${CD_PADEIRO}`, {
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
        var vendas = data.data;
        var mensagem = data.mensagem;
        var stacktrace = data.stacktrace;
            
        // Verificar se os dados estão presentes e se há algum erro
        if (!vendas || mensagem || stacktrace) {
            console.error('Erro:', mensagem || stacktrace);
            return;
        }
    
        // Selecionar a seção onde as vendas serão exibidas
        const vendasSection = document.querySelector('.conf-padeiro-minhas-vendas');
    
        // Limpar qualquer conteúdo pré-existente dentro da seção de vendas
        vendasSection.innerHTML = '';
    
        // Criar um objeto para armazenar as vendas agrupadas por CD_PEDIDO
        const vendasAgrupadas = {};
    
        // Agrupar as vendas por CD_PEDIDO
        vendas.forEach(venda => {
            if (!vendasAgrupadas[venda.cD_PEDIDO]) {
                vendasAgrupadas[venda.cD_PEDIDO] = {
                    info: venda,
                    produtos: []
                };
            }
            vendasAgrupadas[venda.cD_PEDIDO].produtos.push(venda);
        });
    
        // Iterar sobre as vendas agrupadas e adicionar elementos HTML para cada uma
        for (const venda in vendasAgrupadas) {
            if (Object.hasOwnProperty.call(vendasAgrupadas, venda)) {
                const vendaData = vendasAgrupadas[venda].info;
                const produtosDaVenda = vendasAgrupadas[venda].produtos;
    
                // Criar elemento de venda
                const vendaElement = document.createElement('section');
                vendaElement.classList.add('conf-padeiro-minhas-vendas-main');
    
                // Construir HTML para a venda
                vendaElement.innerHTML = `
                    <div class="conf-padeiro-minhas-vendas-info">
                        <h4>ID do Pedido: ${vendaData.cD_PEDIDO}</h4>
                        <p>Data do Pedido: ${vendaData.dT_PEDIDO}</p>
                        <p>Usuário ${vendaData.nM_CLIENTE}</p>
                        <p>
                            Endereço: ${vendaData.dS_ENDERECO}.
                            Nº ${vendaData.nM_CIDADE}.
                            Complemento: ${vendaData.nM_ESTADO}.
                            Cidade: ${vendaData.nM_CIDADE}.
                            Estado: ${vendaData.nM_ESTADO}.
                        </p>
                    </div>
                    <div class="conf-padeiro-minhas-vendas-valor">
                        <h4>Total R$ ${vendaData.vL_TOTAL}</h4>
                    </div>
                `;
    
                // Adicionar elemento de venda à seção de vendas
                vendasSection.appendChild(vendaElement);
    
                // Criar tabela de itens da venda
                const tabelaElement = document.createElement('div');
                tabelaElement.classList.add('conf-padeiro-minhas-vendas-mostrar');
    
                // Construir HTML para a tabela de itens da venda
                let tabelaHTML = `
                    <table class="conf-padeiro-minhas-vendas-mostrando">
                        <thead>
                            <tr>
                                <th>Tipo do Item</th>
                                <th>Quantidade</th>
                                <th>Valor Unitário</th>
                                <th>Sub-Total</th>
                            </tr>
                        </thead>
                        <tbody>
                `;
    
                // Adicionar produtos da venda à tabela de itens da venda
                produtosDaVenda.forEach(produto => {
                    tabelaHTML += `
                        <tr>
                            <td>${produto.nM_PRODUTO}</td>
                            <td>${produto.qT_PRODUTO} unidades</td>
                            <td>R$ ${produto.vL_PRECO}</td>
                            <td>R$ ${produto.vL_SUBTOTAL}</td>
                        </tr>
                    `;
                });
    
                // Fechar a tabela de itens da venda
                tabelaHTML += `
                        </tbody>
                    </table>
                `;
    
                // Adicionar HTML da tabela à tabela de itens da venda
                tabelaElement.innerHTML = tabelaHTML;
    
                // Adicionar tabela de itens da venda ao elemento de venda
                vendaElement.appendChild(tabelaElement);
            }
        }
    })
    .catch(error => {
        console.error('Erro:', error);
    });

}
