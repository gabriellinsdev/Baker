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