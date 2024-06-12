document.addEventListener('DOMContentLoaded', function() {
    fetchPadeiros();

    document.querySelector('.btn-search').addEventListener('click', searchPadeiroByName);
});

let allPadeiros = [];
let currentFilter = null;

function fetchPadeiros(codigoAlimentoRestrito = null) {
    let xmlString = null;

    if (codigoAlimentoRestrito !== null) {
        xmlString = `<ALIMENTOSRESTRITOS><ITEM><CD_ALIMENTO_RESTRITO>${codigoAlimentoRestrito}</CD_ALIMENTO_RESTRITO></ITEM></ALIMENTOSRESTRITOS>`;
    }

    fetch(`${BASE_URL_API2}/Padeiros/ListLocation?LS_ALIMENTOS_RESTRITOS_PADEIRO=${xmlString}`)
        .then(response => response.json())
        .then(data => {
            if (data.data !== null) {
                // Redefine a lista de padeiros antes de adicionar os novos
                allPadeiros = data.data;
                populatePadeiroList(allPadeiros);
            } else {
                console.error('Nenhum padeiro encontrado.');
            }
        })
        .catch(error => {
            console.error('Houve um problema com a operação fetch:', error);
        });
}

function populatePadeiroList(padeiros) {
    const resultContainer = document.querySelector('.produtos_do_padeiro-resultado');
    resultContainer.innerHTML = ''; // Limpar a lista antes de adicionar novos padeiros

    padeiros.forEach(padeiro => {
        const padeiroElement = document.createElement('div');
        padeiroElement.className = 'produtos_do_padeiro-resultado-padeiro';
        const nomePadeiro = padeiro.nM_USUARIO.split(' ')[0];
        const especialidades = parseAlimentosRestritos(padeiro.lS_ALIMENTOS_RESTRITOS_PADEIRO);
        const especialidadesText = especialidades.join(' | ');

        const padeiroContent = `
            <img src="img/logo-padeiro.png" alt="logo do padeiro" class="produtos_do_padeiro-logo-padeiro"> 
            <div class="produtos_do_padeiro-conteudo-padeiro">
                <h3>${nomePadeiro}</h3>
                <p>${especialidadesText}</p>
                <button onclick="redirectWithUserCode('${padeiro.cD_USUARIO}','${padeiro.nM_USUARIO}')">Clique para ver os produtos desse padeiro</button>
            </div>
        `;

        padeiroElement.innerHTML = padeiroContent;
        resultContainer.appendChild(padeiroElement);
    });
}

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

function viewPadeiroProducts(padeiroId) {
    window.location.href = `produtos-do-padeiro2.html?padeiroId=${padeiroId}`;
}

function searchPadeiroByName() {
    const searchInput = document.getElementById('produtos_do_padeiro-search').value.trim();
    if (!searchInput) return;

    const filteredPadeiros = allPadeiros.filter(padeiro => padeiro.nM_USUARIO.toLowerCase().includes(searchInput.toLowerCase()));
    populatePadeiroList(filteredPadeiros);
}


function filterPadeiros(filter) {
    const filteredPadeiros = allPadeiros.filter(padeiro => {
        const especialidades = parseEspecialidades(padeiro.lS_ALIMENTOS_RESTRITOS);
        return especialidades.includes(filter);
    });
    populatePadeiroList(filteredPadeiros);
}

function parseEspecialidades(xmlString) {
    const parser = new DOMParser();
    const xmlDoc = parser.parseFromString(xmlString, "text/xml");
    const items = xmlDoc.getElementsByTagName("ITEM");
    let especialidades = [];
    for (let item of items) {
        let especialidade = item.getElementsByTagName("DS_ALIMENTO")[0].textContent;
        especialidades.push(especialidade);
    }
    return especialidades;
}

function redirectWithUserCode(CD_PADEIRO, NM_PADEIRO) {
    sessionStorage.setItem('CD_PADEIRO', CD_PADEIRO);
    sessionStorage.setItem('NM_PADEIRO', NM_PADEIRO);

    var destinationUrl = 'http://127.0.0.1:5500/produtos-do-padeiro2.html';
    
    // Redirecionar para a nova URL
    window.location.href = destinationUrl;
}

window.fetchPadeiros = fetchPadeiros;
window.populatePadeiroList = populatePadeiroList;
window.viewPadeiroProducts = viewPadeiroProducts;
window.searchPadeiroByName = searchPadeiroByName;
window.toggleFilter = toggleFilter;
window.filterPadeiros = filterPadeiros;
window.parseAlimentosRestritos = parseAlimentosRestritos;
