var quantidade = 1;
let CD_PADEIRO;
let NM_PADEIRO;

function desabilitaAlimentoRestrito() {
  const lactoseSim = document.querySelector("#padeiro-ideal_lactose-sim");
  const glutenSim = document.querySelector("#padeiro-ideal_gluten-sim");
  const lowCarbSim = document.querySelector("#padeiro-ideal_lowcarb-sim");

  if (lactoseSim && glutenSim && lowCarbSim) {
    lactoseSim.checked = false;
    glutenSim.checked = false;
    lowCarbSim.checked = false;
  }
}

function desabilitaAlimentoCaseiros() {
  if (
    document.querySelector("#padeiro-ideal_lactose-sim").checked ||
    document.querySelector("#padeiro-ideal_gluten-sim").checked ||
    document.querySelector("#padeiro-ideal_lowcarb-sim").checked ||
    document.querySelector("#padeiro-ideal_lactose-nao").checked ||
    document.querySelector("#padeiro-ideal_gluten-nao").checked ||
    document.querySelector("#padeiro-ideal_lowcarb-nao").checked
  ) {
    document.querySelector("#padeiro-ideal_caseiro-sim").checked = false;
  }
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

function buscarPadeiroIdeal() {
  // Obter o valor do CEP do input
  var cep = document.getElementById("padeiro_ideal-search").value;

  // Construir o XML baseado nas respostas dos checkboxes
  const gluten = document.querySelector("#padeiro-ideal_gluten-sim").checked
    ? "1"
    : "";
  const lactose = document.querySelector("#padeiro-ideal_lactose-sim").checked
    ? "2"
    : "";
  const lowCarb = document.querySelector("#padeiro-ideal_lowcarb-sim").checked
    ? "3"
    : "";

  const caseirosCheckbox = document.querySelector("#padeiro-ideal_caseiro-sim");
  const caseiros = caseirosCheckbox && caseirosCheckbox.checked ? "4" : "";

  let xmlString = null;
  if (gluten !== "" || lactose !== "" || lowCarb !== "" || caseiros !== "") {
    xmlString = `<ALIMENTOSRESTRITOS>${lactose
        ? `<ITEM><CD_ALIMENTO_RESTRITO>${lactose}</CD_ALIMENTO_RESTRITO></ITEM>`
        : ""
      }${gluten
        ? `<ITEM><CD_ALIMENTO_RESTRITO>${gluten}</CD_ALIMENTO_RESTRITO></ITEM>`
        : ""
      }${lowCarb
        ? `<ITEM><CD_ALIMENTO_RESTRITO>${lowCarb}</CD_ALIMENTO_RESTRITO></ITEM>`
        : ""
      }${caseiros
        ? `<ITEM><CD_ALIMENTO_RESTRITO>${caseiros}</CD_ALIMENTO_RESTRITO></ITEM>`
        : ""
      }</ALIMENTOSRESTRITOS>`;
  }
  // Fazer a requisição para a API
  fetch(`${BASE_URL_API2}/ListarPadeirosProximos?CEP_CLIENTE=${cep}&QT_LINHAS=${quantidade}&lS_ALIMENTOS_RESTRITOS_PADEIRO=${encodeURIComponent(
      xmlString
    )}`,
    {
      headers: {
        "Content-Type": "application/json",
        Accept: "*/*",
      },
      mode: "cors",
    }
  )
    .then((response) => {
      if (!response.ok) {
        throw new Error("Erro ao fazer a requisição");
      }
      return response.json();
    })
    .then((data) => {
      var dados = data.data;
      var mensagem = data.mensagem;
      var stacktrace = data.stacktrace;

      CD_PADEIRO = dados[0].cD_USUARIO;
      NM_PADEIRO = dados[0].nM_USUARIO;

      document.querySelector("#padeiro").textContent = NM_PADEIRO;

      let alimentos = parseAlimentosRestritos(dados[0].lS_ALIMENTOS_RESTRITOS_PADEIRO);
      document.getElementById("restricao").textContent = alimentos[0] || "";
      document.getElementById("restricaoExtra").textContent = alimentos[1] || "";
      document.getElementById("restricaoExtra1").textContent = alimentos[2] || "";
      document.getElementById("restricaoExtra2").textContent = alimentos[3] || "";

      document.getElementById("button").setAttribute("onclick", `redirectWithUserCode('${CD_PADEIRO}', '${NM_PADEIRO}')`);
    })
    .catch((error) => {
      console.error("Erro:", error);
    });
}

function redirectWithUserCode(CD_PADEIRO, NM_PADEIRO) {
  sessionStorage.setItem("CD_PADEIRO", CD_PADEIRO);
  sessionStorage.setItem("NM_PADEIRO", NM_PADEIRO);

  var destinationUrl = "http://127.0.0.1:5500/produtos-do-padeiro2.html";

  // Redirecionar para a nova URL
  window.location.href = destinationUrl;
}
