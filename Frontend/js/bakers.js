document.addEventListener('DOMContentLoaded', function() {
    fetchPadeirosByCity('guarujá');
    fetchPadeirosByCity('santos');

    document.querySelector('.btn-search').addEventListener('click', searchPadeiroByName);
    document.querySelector('.produtos_do_padeiro-filtro-lactose').addEventListener('click', () => toggleFilter('Zero Lactose'));
    document.querySelector('.produtos_do_padeiro-filtro-gluten').addEventListener('click', () => toggleFilter('Zero Gluten'));
    document.querySelector('.produtos_do_padeiro-filtro-lowcarb').addEventListener('click', () => toggleFilter('Low-Carb'));
    document.querySelector('.produtos_do_padeiro-filtro-artesanais').addEventListener('click', () => toggleFilter('Artesanal'));
});

let allPadeiros = [];
let currentFilter = null;

function fetchPadeirosByCity(city) {
    fetch(`${BASE_URL_API2}/Padeiros/ListLocation?NM_CIDADE=${city}`)
    .then(response => response.json())
    .then(data => {
        console.log(data)
        if (data.data != null) {
            allPadeiros = allPadeiros.concat(data.data);
            populatePadeiroList(allPadeiros);
        } else {
            console.error(`No bakers found in ${city}`);
        }
    })
    .catch(error => {
        console.error('There has been a problem with your fetch operation:', error);
    });
}

function populatePadeiroList(padeiros) {
    const resultContainer = document.querySelector('.produtos_do_padeiro-resultado');
    resultContainer.innerHTML = ''; // Limpar a lista antes de adicionar novos padeiros

    padeiros.forEach(padeiro => {
        console.log(padeiros)
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
                <button onclick="viewPadeiroProducts('${padeiro.cD_USUARIO}')">Clique para ver os produtos desse padeiro</button>
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

function toggleFilter(filter) {
    if (currentFilter === filter) {
        currentFilter = null;
        populatePadeiroList(allPadeiros);
    } else {
        currentFilter = filter;
        filterPadeiros(filter);
    }
}

function filterPadeiros(filter) {
    const filteredPadeiros = allPadeiros.filter(padeiro => {
        const especialidades = parseEspecialidades(padeiro.lS_ALIMENTOS_RESTRITOS);
        return especialidades.includes(filter);
    });
    populatePadeiroList(filteredPadeiros);
}

window.fetchPadeirosByCity = fetchPadeirosByCity;
window.populatePadeiroList = populatePadeiroList;
window.viewPadeiroProducts = viewPadeiroProducts;
window.searchPadeiroByName = searchPadeiroByName;
window.toggleFilter = toggleFilter;
window.filterPadeiros = filterPadeiros;
window.parseEspecialidades = parseEspecialidades;
