function login(email, senha) {
    const loginData = {
        email: email,
        senha: senha
    };

    fetch(`${BASE_URL}/Usuario/Logar`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(loginData)
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok ' + response.statusText);
        }
        return response.json();
    })
    .then(result => {
        localStorage.setItem('token', result);
        window.location.href = 'index.html'; // Redirecionar para a página inicial
    })
    .catch(error => {
        console.error('There has been a problem with your fetch operation:', error);
    });
}

function register(userData) {
    fetch(`${BASE_URL}/Usuario`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(userData)
    })
    .then(response => {
        if (!response.ok) {
            return response.json().then(errorData => {
                console.error('Erro na resposta:', errorData);
                throw new Error('Network response was not ok ' + response.statusText);
            });
        }
        return response.json();
    })
    .then(result => {
        console.log(result); // Criar mensagem de sucesso
        window.location.href = 'login.html'; // Redirecionar para a página de login após cadastro bem-sucedido
    })
    .catch(error => {
        console.error('There has been a problem with your fetch operation:', error);
    });
}

function logout() {
    localStorage.removeItem('token');
    window.location.href = 'login.html'; // Redirecionar para a página de login
}

function setupLoginForm() {
    const loginForm = document.querySelector('.login-formulario form');
    if (loginForm) {
        loginForm.addEventListener('submit', function(event) {
            event.preventDefault();
            const email = document.getElementById('usuario').value;
            const senha = document.getElementById('senha').value;
            login(email, senha);
        });
    }
}

function setupRegisterForm() {
    const registerForm = document.querySelector('.cadastrese-formulario form');
    if (registerForm) {
        registerForm.addEventListener('submit', function(event) {
            event.preventDefault();
            const userData = {
                nome: document.getElementById('cadastrese-nome').value,
                email: document.getElementById('cadastrese-email').value,
                telefone: document.getElementById('cadastrese-telefone').value,
                cep: document.getElementById('cadastrese-cep').value,
                estado: document.getElementById('cadastrese-estado').value,
                endereco: document.getElementById('cadastrese-endereco').value,
                cidade: document.getElementById('cadastrese-cidade').value,
                senha: document.getElementById('cadastrese-senha').value,
                cpfCnpj: document.getElementById('cadastrese-cnpj').value || document.getElementById('cadastrese-cpf').value
            };
            register(userData);
        });
    }
}

function ChangeaddressUI() {
    const token = localStorage.getItem('token');
    fetch(`${BASE_URL}/Usuario/${token}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok ' + response.statusText);
        }
        return response.json();
    })
    .then(result => {
        if(document.querySelector('.conf-usuario-endereco-escrito p')) document.querySelector('.conf-usuario-endereco-escrito p').innerHTML = `Endereço: ${result.endereco}. Cidade: ${result.cidade}. Estado: ${result.estado}`
    })
    .catch(error => {
        console.error('There has been a problem with your fetch operation:', error);
    });
}

function isAuthenticated() {
    const token = localStorage.getItem('token');
    return token !== null;
}

function ensureAuthenticated() {
    const restrictedPages = ['padeiros-proximos.html', 'padeiro-ideal.html', 'produtos-do-padeiro1.html', 'usuario-configuracoes-alterar.html', 'usuario-configuracoes-historico.html'];
    const currentPage = window.location.pathname.split('/').pop();
    if (restrictedPages.includes(currentPage) && !isAuthenticated()) {
        window.location.href = 'login.html'; // Redirecionar para a página de login se não estiver autenticado
    }
}

function updateAuthUI() {
    const loginButtons = document.querySelectorAll('.login-btn')[0];
    
        if (isAuthenticated()) {
            const token = localStorage.getItem('token');
            fetch(`${BASE_URL_API2}/Usuario/Get?CD_USUARIO=${token}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok ' + response.statusText);
                }
                return response.json();
            })
            .then(result => {
                loginButtons.innerHTML = '<a href="usuario-configuracoes-historico.html">User</a>';
                if (result.data.tP_USUARIO == "PADEIRO") {

                    const navList = document.querySelector('.conf-usuario-nav ul');
                    const padeiroNome = document.querySelector('.conf-avatar-info h3')
                    if(padeiroNome) padeiroNome.innerHTML = result.data.nM_USUARIO.split(' ')[0]
                   
                    if (navList) {
                        const manageProducts = document.createElement('li');
                        manageProducts.className = 'conf-link-icon';
                        manageProducts.innerHTML = `
                            <img src="img/cadastrar-produto.png" alt="icone de usuario">
                            <a href="padeiro-configuracoes-gerenciar-produtos.html" class="">Gerenciar Produtos</a>
                        `;

                        const mySales = document.createElement('li');
                        mySales.className = 'conf-link-icon';
                        mySales.innerHTML = `
                            <img src="img/vendas.png" alt="icone de usuario">
                            <a href="padeiro-configuracoes-vendas.html" class="">Minhas Vendas</a>
                        `;

                        const dashboard = document.createElement('li');
                        dashboard.className = 'conf-link-icon';
                        dashboard.innerHTML = `
                            <img src="img/dashboard.png" alt="icone de usuario">
                            <a href="padeiro-configuracoes-dashboard.html" class="">Dashboard</a>
                        `;

                        navList.appendChild(manageProducts);
                        navList.appendChild(mySales);
                        navList.appendChild(dashboard);

                    }
                }
                loginButtons.removeAttribute('href');
            })
            .catch(error => {
                console.error('There has been a problem with your fetch operation:', error);
            });
        }
}
function updateUserData(data) {
    const token = localStorage.getItem('token');
    return fetch(`${BASE_URL}/Usuario`, {
        method: 'PATCH',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(data)
    })
    .then(response => {
        if (response.status === 204) {
            return {}; // Retorna um objeto vazio se o status for 204
        }
        if (!response.ok) {
            return response.json().then(errorData => {
                console.error('Erro na resposta:', errorData); // Criar mensagem de erro
                throw new Error('Network response was not ok ' + response.statusText);
            });
        }
        return response.json();
    })
    .then(result => {
        console.log('Resultado da atualização:', result); // Criar mensagem de sucesso
        ChangeaddressUI()
        return result;
    })
    .catch(error => {
        console.error('There has been a problem with your fetch operation:', error); // Criar mensagem de erro
        throw error;
    });
}

// Adicionar as funções de inicialização ao escopo global
window.setupLoginForm = setupLoginForm;
window.setupRegisterForm = setupRegisterForm;
window.isAuthenticated = isAuthenticated;
window.ensureAuthenticated = ensureAuthenticated;
window.updateAuthUI = updateAuthUI;
window.logout = logout;
window.updateUserData = updateUserData;

// Executar ensureAuthenticated em todas as páginas para garantir que o redirecionamento funcione
document.addEventListener('DOMContentLoaded', function() {
    ensureAuthenticated();
    updateAuthUI();
    ChangeaddressUI()
});
