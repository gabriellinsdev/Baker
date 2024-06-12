document.addEventListener('DOMContentLoaded', function() {
    const userId = localStorage.getItem('token');
    if (!userId) {
        console.error('Usuário não encontrado no local storage');
        window.location.href = 'login.html'; // Redirecionar para a página de login
        return;
    }

    fetchDashboardData(userId);
});

function fetchDashboardData(userId) {
    fetch(`${BASE_URL_API2}/Padeiros/SalesReport?CD_USUARIO=${userId}`)
        .then(response => response.json())
        .then(data => {
            if (data.data && data.data.length > 0) {
                populateDashboard(data.data);
            } else {
                console.error('No sales data found');
            }
        })
        .catch(error => {
            console.error('Error fetching sales data:', error);
        });
}

function populateDashboard(data) {
    const topProducts = {};
    const frequentClients = {};
    const salesLast30Days = {};

    data.forEach(sale => {
        // Top Products
        if (!topProducts[sale.nM_PRODUTO]) {
            topProducts[sale.nM_PRODUTO] = 0;
        }
        topProducts[sale.nM_PRODUTO] += sale.qT_PRODUTO;

        // Frequent Clients
        if (!frequentClients[sale.nM_CLIENTE]) {
            frequentClients[sale.nM_CLIENTE] = 0;
        }
        frequentClients[sale.nM_CLIENTE] += 1;

        // Sales Last 30 Days
        if (!salesLast30Days[sale.nM_PRODUTO]) {
            salesLast30Days[sale.nM_PRODUTO] = 0;
        }
        salesLast30Days[sale.nM_PRODUTO] += sale.qT_PRODUTO;
    });

    populateTopProducts(topProducts);
    populateFrequentClients(frequentClients);
    populateSalesQuantity(salesLast30Days);
}

function populateTopProducts(products) {
    const topProductsContainer = document.querySelector('.dashboard-maisvendido');
    topProductsContainer.innerHTML = '<h4>Produtos Mais Vendidos</h4>'; // Resetar o conteúdo

    Object.keys(products).forEach(productName => {
        const productElement = document.createElement('p');
        productElement.textContent = productName;
        topProductsContainer.appendChild(productElement);
    });
}

function populateFrequentClients(clients) {
    const frequentClientsContainer = document.querySelector('.dashboard-compradores');
    frequentClientsContainer.innerHTML = '<h4>Clientes Frequentes</h4>'; // Resetar o conteúdo

    Object.keys(clients).forEach(clientName => {
        const clientElement = document.createElement('p');
        clientElement.textContent = clientName.split(' ')[0];
        frequentClientsContainer.appendChild(clientElement);
    });
}

function populateSalesQuantity(salesData) {
    const salesQuantityContainer1 = document.querySelector('.dashboard-compras30dias-info1');
    const salesQuantityContainer2 = document.querySelector('.dashboard-compras30dias-info2');
    salesQuantityContainer1.innerHTML = ''; // Resetar o conteúdo
    salesQuantityContainer2.innerHTML = ''; // Resetar o conteúdo

    Object.keys(salesData).forEach(productName => {
        const quantityElement = document.createElement('p');
        quantityElement.textContent = salesData[productName];
        salesQuantityContainer1.appendChild(quantityElement);

        const productElement = document.createElement('p');
        productElement.textContent = productName;
        salesQuantityContainer2.appendChild(productElement);
    });
}
