// Constantes

const BASE_URL = 'https://localhost:7105';
const BASE_URL_API2 = 'https://localhost:7023';

function isUserLoggedIn() {
    return !!localStorage.getItem('token');
}

function redirectPage(page) {
    window.location.href = page;
}

// Certifique-se de que a função esteja disponível globalmente
window.redirectPage = redirectPage;

window.isUserLoggedIn = isUserLoggedIn;


// Menu Sanduíche NavBar

function menuShow() {
    // Seleciona o elemento do menu móvel pela classe CSS '.navbar-mobile'
    let menuMobile = document.querySelector('.navbar-mobile');
    
    // Verifica se o menu móvel possui a classe 'open'
    if (menuMobile.classList.contains('open')) {
        // Se o menu estiver aberto, remove a classe 'open' para ocultá-lo
        menuMobile.style.transitionDuration = '0.3s';
        menuMobile.classList.remove('open');
        // Altera a origem da imagem do ícone de menu para o ícone de menu sanduíche
        document.querySelector('.navbar-icone-menu-sanduiche').src = "img/menu-sanduiche.png";
    } else {
        menuMobile.style.transitionDuration = '0.3s';
        // Se o menu estiver fechado, adiciona a classe 'open' para exibi-lo
        menuMobile.classList.add('open');
        // Altera a origem da imagem do ícone de menu para o ícone de menu sanduíche com um 'x'
        document.querySelector('.navbar-icone-menu-sanduiche').src = "img/menu-sanduiche-x.png";
    }
}

window.addEventListener('resize', function() {
    let menuMobile = document.querySelector('.navbar-mobile');
    if (menuMobile.classList.contains('open')) {
        menuMobile.classList.remove('open');
        document.querySelector('.navbar-icone-menu-sanduiche').src = "img/menu-sanduiche.png";
    }
});

//HOME - Galeria de Produtos

const productContainers = document.querySelectorAll('.home-galeria');
        const nxtBtn = document.querySelectorAll('.nxt-btn');
        const preBtn = document.querySelectorAll('.pre-btn');

        for (let i = 0; i < productContainers.length; i++) {
            let item = productContainers[i];
            let containerDimensions = item.getBoundingClientRect();
            let containerWidth = containerDimensions.width;

            nxtBtn[i].addEventListener('click', function() {
                item.scrollLeft += containerWidth;
            });

            preBtn[i].addEventListener('click', function() {
                item.scrollLeft -= containerWidth;
            });
        }

// Configurações - Histórico de Pedidos
// Botão dropdown

document.getElementById('btn-conf-verpedido').addEventListener('click', function() {
    var mostrar = document.getElementById('conf-historico-pedidos-itens');
    if (mostrar.classList.contains('conf-historico-pedidos-mostrar')) {
      mostrar.classList.remove('conf-historico-pedidos-mostrar');
    } else {
      mostrar.classList.add('conf-historico-pedidos-mostrar');
    }
});

document.getElementById('btn-conf-verpedido2').addEventListener('click', function() {
    var mostrar = document.getElementById('conf-historico-pedidos-itens2');
    if (mostrar.classList.contains('conf-historico-pedidos-mostrar')) {
      mostrar.classList.remove('conf-historico-pedidos-mostrar');
    } else {
      mostrar.classList.add('conf-historico-pedidos-mostrar');
    }
});


