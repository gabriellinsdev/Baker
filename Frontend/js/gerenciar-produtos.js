let CD_PADEIRO = localStorage.getItem("token");

document.addEventListener('DOMContentLoaded', function() {
buscarProdutosPadeiro();
});


const productsContainer = document.querySelector('.conf-padeiro-gerenciar-produto-main');
        productsContainer.innerHTML = `<div class="conf-padeiro-gerenciarprodutos-titulo">
        <h3>Gerenciar Produtos</h3>
        <button class="conf-padeiro-btncadastrar-produto"><a href="padeiro-configuracoes-cadastrar-produtos.html">Cadastrar um Produto</a></button>
    </div>`;

function buscarProdutosPadeiro() {

  fetch(`${BASE_URL_API2}/Produtos/List?CD_USUARIO=${CD_PADEIRO}`, {
    headers: {
      "Content-Type": "application/json",
      Accept: "*/*",
    },
    mode: "cors",
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error("Erro ao fazer a requisição");
      }
      return response.json();
    })
    .then((data) => {
      if (data.data != null) {
        preencherProdutosNaPagina((data.data));
    } else {
        console.error('No products found');
    }
      
    })
    .catch((error) => {
      console.error("Erro:", error);
    });
}



// Função para preencher os produtos na página
function preencherProdutosNaPagina(produtos) {
  console.log(produtos)
  const produtosContainer = document.querySelector(".conf-padeiro-gerenciar-produto-main");

  produtos.forEach((produto) => {
    const produtoSection = document.createElement("section");
    produtoSection.classList.add("conf-padeiro-produtos-cadastrados");

    const imagemDiv = document.createElement("div");
    imagemDiv.classList.add("conf-padeiro-img");

    // Criar a imagem e definir seus atributos
    const imagem = document.createElement("img");
    imagem.classList.add("conf-padeiro-img-cadastrada");
    imagem.src = `data:image/jpeg;base64,${produto.vB_IMAGEM}`; // Supondo que a imagem seja em formato JPEG
    imagem.alt = "Imagem do produto";

    // Adicionar a imagem à div de imagem
    imagemDiv.appendChild(imagem);

    const divInformacoes = document.createElement("div");
    divInformacoes.classList.add("conf-padeiro-informacoes-produto-cadastrado");
    const titulo = document.createElement("h4");
    titulo.textContent = produto.nM_PRODUTO;
    divInformacoes.appendChild(titulo);
    const descricao1 = document.createElement("p");
    descricao1.textContent = produto.dS_PRODUTO;
    divInformacoes.appendChild(descricao1);

    const descricao2 = document.createElement("p");
    const alimentosRestritosXML = produto.lS_ALIMENTOS_RESTRITOS_PRODUTO;
    const parser = new DOMParser();
    const xmlDoc = parser.parseFromString(alimentosRestritosXML, "text/xml");
    const items = xmlDoc.querySelectorAll("ITEM");
    let descricaoAlimentosRestritos = "";
    items.forEach((item) => {
      const nomeAlimento = item.querySelector("DS_ALIMENTO").textContent;
      descricaoAlimentosRestritos += nomeAlimento + ", ";
    });
    // Remover a vírgula extra no final
    descricao2.textContent = descricaoAlimentosRestritos.slice(0, -2);
    divInformacoes.appendChild(descricao2);

    const preco = document.createElement("p");
    preco.textContent = produto.vL_PRECO
      ? `R$ ${produto.vL_PRECO.toFixed(2)}`
      : "Preço indisponível";
    divInformacoes.appendChild(preco);

    const divBotoes = document.createElement("div");
    divBotoes.classList.add("conf-padeiro-btn-alteracoes");

    const botaoAlterar = document.createElement("button");
    botaoAlterar.textContent = "Alterar";
    botaoAlterar.classList.add("conf-padeiro-alterar-produto");
    botaoAlterar.addEventListener("click", () => {
        localStorage.setItem('CD_PRODUTO', produto.cD_PRODUTO)
        window.location.href = "http://127.0.0.1:5500/padeiro-configuracoes-alterar-produtos.html"
      });

    const botaoExcluir = document.createElement("button");
    botaoExcluir.textContent = "Excluir";
    botaoExcluir.classList.add("conf-padeiro-excluir-produto");
    botaoExcluir.addEventListener("click", () => {
      excluirProdutoPadeiro(produto.cD_PRODUTO); // Chama a função de exclusão passando o ID do produto
      // Remover a div do produto da interface após a exclusão
      divBotoes.parentElement.remove();
    });

    divBotoes.appendChild(botaoAlterar);
    divBotoes.appendChild(botaoExcluir);

    produtoSection.appendChild(imagemDiv);
    produtoSection.appendChild(divInformacoes);
    produtoSection.appendChild(divBotoes);

    produtosContainer.appendChild(produtoSection);
  });
}

function alterarProdutoPadeiro(PRODUTO) {
  let formData = new FormData();
  formData.append("CD_PRODUTO", PRODUTO.CD_PRODUTO);
  formData.append("CD_USUARIO", PRODUTO.CD_USUARIO);
  formData.append("NM_PRODUTO", PRODUTO.NM_PRODUTO);
  formData.append("DS_PRODUTO", PRODUTO.DS_PRODUTO);
  formData.append("VL_PRECO", PRODUTO.VL_PRECO);
  formData.append("VB_IMAGEM", PRODUTO.VB_IMAGEM);

  // Capturando os checkboxes selecionados
  let checkboxes = document.querySelectorAll('input[name="categoria"]:checked');
  let alimentosRestritos = [];
  checkboxes.forEach((checkbox) => {
    alimentosRestritos.push(checkbox.value);
  });
  formData.append("lS_ALIMENTOS_RESTRITOS_PRODUTO", JSON.stringify(alimentosRestritos));

  fetch(`https://localhost:7023/Produtos/Update`, {
    method: "PUT",
    body: formData,
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error("Erro ao alterar o produto!");
      }
      return response.blob(); // Retorna a resposta como um blob
    })
    .then((blob) => {
      // Aqui você pode manipular o blob retornado pela API
      console.log("Imagem em bytes:", blob);
    })
    .catch((error) => {
      console.error("Erro ao alterar o produto: ", error);
    });
}

function excluirProdutoPadeiro(CD_PRODUTO) {
  fetch(`${BASE_URL_API2}/Produtos/Delete?cd_produto=${CD_PRODUTO}`, {
    headers: {
      "Content-Type": "application/json",
      Accept: "*/*",
    },
    mode: "cors",
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error("Erro ao excluir o produto!");
      }
      // Verifica o status da resposta para determinar o sucesso da exclusão
      if (response.status === 200) {
        console.log("Exclusão efetuada com sucesso!");
        // Aqui você pode fazer alguma ação adicional, se necessário
      } else {
        throw new Error("Erro ao excluir o produto!");
      }
    })
    .catch((error) => {
      console.error("Erro ao excluir o produto: ", error);
    });
}
