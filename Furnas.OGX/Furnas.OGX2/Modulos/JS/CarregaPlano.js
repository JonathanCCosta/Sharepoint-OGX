/*$(document).ready(function () {
    var param = getWQuerystringP("ID");

    var plano = TipoPlanoP(param);
    
    if(plano.TipoDeConteúdo == "Plano por Superação"){
		
    }	
	else{
		carregaMelhoriaReforco(plano);
   	}
});*/

function carregaSuperacao(plano) {
    $("input[title='Numeração']").val(plano.Numeração);
    $("input[title='Ciclo']").val(plano.Ciclo.NomeDoCiclo);
    $("select[title='Região Campo Obrigatório'] option:contains(" + plano.RegiãoValue + ")").attr('selected', 'selected');
    $("input[title='Agente Campo Obrigatório']").val(plano.Agente);
    $("input[title='Tipo de Equipamento']").val(plano.TipoDeEquipamento);
    //$("select[title='Instalação Campo Obrigatório'] option:contains(" + plano.Instalação + ")").attr('selected', 'selected');
    var instalacao = false;
    $("select[title='Instalação Campo Obrigatório'] option").filter(function () {
        if (retira_acentos(this.text) == retira_acentos(plano.Instalação)) {
            instalacao = true;
            //return plano.Instalação;
            return this.text;
        }
    }).attr('selected', 'selected');

    if (!instalacao) {
        $("select[title='Instalação Campo Obrigatório'] option:contains(Outra Instalação)").attr('selected', 'selected');
        $("input[title='Instalação - Outros']").closest("tr").show();
        $("input[title='Instalação - Outros']").val(plano.Instalação);
    }

    FuncaoTransmissao($("select[title='Instalação Campo Obrigatório']").val());
    $("select[title='Tensão Nominal (KV) Campo Obrigatório'] option:contains(" + plano.TensãoNominalKV + ")").attr('selected', 'selected');
    $("select[title='Tipo de Agente Campo Obrigatório'] option:contains(" + plano.TipoDeAgente + ")").attr('selected', 'selected');
    $("textarea[title='Razão principal para a superação deste equipamento']").val(plano.RazãoPrincipalParaASuperaçãoDesteEquipamento);
    $("input[title='Nome Do Vão']").val(plano.NomeDoVão);
    $("input[title='Código de Identificação']").val(plano.CódigoDeIdentificação);

    var FT = false;
    $("select[title='Função de Transmissão Campo Obrigatório'] option").filter(function () {
        if (retira_acentos(this.text) == retira_acentos(plano.FunçãoTransmissão)) {
            FT = true;
            //return plano.FunçãoTransmissão;
            return this.text;
        }
    }).attr('selected', 'selected');

    if (!FT) {
        $("select[title='Função de Transmissão Campo Obrigatório'] option:contains(Outra Função de transmissão)").attr('selected', 'selected');
        $("input[title='Função de Transmissão - Outros']").closest("tr").show();
        $("input[title='Função de Transmissão - Outros']").val(plano.FunçãoTransmissão);
    }

    //$("select[title='Função de Transmissão Campo Obrigatório'] option:contains(" +  plano.FunçãoTransmissão + ")").attr('selected', 'selected');

    $("input[title='Número da Barra no Anafas']").val(plano.NúmeroDaBarraNoAnafas);
    $("input[title='Nível de curto-circuito na barra monofásico (kA)']").val(plano.NívelDeCurtoCircuitoNaBarraMonofásicoKA);
    $("input[title='Nível de curto-circuito na barra trifásico (kA)']").val(plano.NívelDeCurtoCircuitoNaBarraTrifásicoKA);
    $("input[title='X/R na barra da subestação para curto-circuito monofásico']").val(plano.XRNaBarraDaSubestaçãoParaCurtoCircuitoMonofásico);
    $("input[title='X/R na barra da subestação para curto-circuito trifásico']").val(plano.XRNaBarraDaSubestaçãoParaCurtoCircuitoTrifásico);
    $("input[title='Capacidade interrupção simétrica nominal (kA)']").val(plano.CapacidadeInterrupçãoSimétricaNominalKA);
    $("input[title='Corrente de curto-circuito nominal (kA)']").val(plano.CorrenteDeCurtoCircuitoNominalKA);
    $("input[title='Valor nominal da crista da corrente de curto-circuito (kA)']").val(plano.ValorNominalDaCristaDaCorrenteDeCurtoCircuitoKA);

    $("input[title='Quantidade de equipamentos superados']").val(plano.QuantidadeDeEquipamentosSuperados);
    $("input[title='Corrente de curto-circuito monofásica através Do disjuntor (calculada) (kA)']").val(plano.CorrenteDeCurtoCircuitoMonofásicaAtravésDoDisjuntorCalculadaKA);
    $("input[title='Corrente de curto-circuito trifásica através Do disjuntor (calculada) (kA)']").val(plano.CorrenteDeCurtoCircuitoTrifásicaAtravésDoDisjuntorCalculadaKA);
    $("input[title='X/R Do disjuntor (nominal)']").val(plano.XRDoDisjuntorNominal);
    $("input[title='Fator de primeiro polo nominal']").val(plano.FatorDePrimeiroPoloNominal);

    $("input[title='Quantidade de secionadoras superadas no mesmo vão (bay) sem chave de terra']").val(plano.QuantidadeDeSecionadorasSuperadasNoMesmoVãoBaySemChaveDeTerra);
    $("input[title='Quantidade de secionadoras superadas no mesmo vão (bay) com chave de terra']").val(plano.QuantidadeDeSecionadorasSuperadasNoMesmoVãoBayComChaveDeTerra);
    $("input[title='Corrente de curto-circuito monofásica através da secionadora (calculada) (kA)']").val(plano.CorrenteDeCurtoCircuitoMonofásicaAtravésDaSecionadoraCalculadaKA);
    $("input[title='Corrente de curto-circuito trifásica através da secionadora (calculada) (kA)']").val(plano.CorrenteDeCurtoCircuitoTrifásicaAtravésDaSecionadoraCalculadaKA);
    $("input[title='Saturação']").val(plano.Saturação);

    $("input[title='Corrente de curto-circuito monofásica através da Bobina de bloqueio (calculada) (kA)']").val(plano.CorrenteDeCurtoCircuitoMonofásicaAtravésDaBobinaDeBloqueioCalculadaKA);
    $("input[title='Corrente de curto-circuito trifásica através da Bobina de bloqueio (calculada) (kA)']").val(plano.CorrenteDeCurtoCircuitoTrifásicaAtravésDaBobinaDeBloqueioCalculadaKA);
    $("input[title='Corrente de curto-circuito monofásica através Do TC (calculada) (kA)']").val(plano.CorrenteDeCurtoCircuitoMonofásicaAtravésDoTCCalculadaKA);
    $("input[title='Corrente de curto-circuito trifásica através Do TC (calculada) (kA)']").val(plano.CorrenteDeCurtoCircuitoTrifásicaAtravésDoTCCalculadaKA);
    $("input[title='Fator térmico']").val(plano.FatorTérmico);

    $("input[title='Corrente Nominal (A)']").val(plano.CorrenteNominalA);
    $("input[title='Corrente de carga calculada (A)']").val(plano.CorrenteDeCargaCalculadaA);
    $("input[title='Origem da indicação (empresa solicitante/documento de referência)']").val(plano.OrigemDaIndicaçãoEmpresaSolicitanteDocumentoDeReferência);
    $("input[title='Data de Necessidade']").val(dateToString(dateConvert(plano.DataDeNecessidade)));
    $("textarea[title='Observação']").val(plano.ObservaçãoFurnas);
    $("input[title='Projeto que Originou a Indicação']").val(plano.ProjetoQueOriginouAIndicação);
    $("input[title='DeRating 1F']").val(plano.DeRating1F);
    $("input[title='DeRating 3F']").val(plano.DeRating3F);
    $("input[title='Superação por corrente nominal']").val(plano.SuperaçãoPorCorrenteNominal);
    $("input[title='Superação por corrente de curto-circuito simétrica']").val(plano.SuperaçãoPorCorrenteDeCurtoCircuitoSimétrica);
    $("input[title='Superação pela crista da corrente de curto-circuito']").val(plano.SuperaçãoPelaCristaDaCorrenteDeCurtoCircuito);

    $("input[title='Superação por X/R']").val(plano.SuperaçãoPorXR);
    $("input[title='Capacidade nominal de curto-circuito (kA)']").val(plano.CapacidadeNominalDeCurtoCircuitoKA);
    $("input[title='Capacidade nominal de corrente (A)']").val(plano.CapacidadeNominalDeCorrenteA);
    $("input[title='Outras']").val(plano.Outras);
    $("input[title='Situação da Análise']").val(plano.SituaçãoDaAnáliseValue);


    $("input[title='Parecer final']").val(plano.ParecerFinal);
    $("textarea[title='Observação ONS']").val(plano.ObservaçãoONS);
    //$("input[title='Classificação Campo Obrigatório']").val(plano.Classificação);
    $("select[title='Classificação Campo Obrigatório'] option").filter(function () {
        if (this.text == plano.Classificação) {
            return this.text;
        }
    }).attr('selected', 'selected');

    $("input[title='Indicações']").val(plano.Indicações);
    $("input[title='Oficio/Resolução/Autorização ANEEL relativa a substituição Do equipamento']").val(plano.OficioResoluçãoAutorizaçãoANEELRelativaASubstituiçãoDoEquipamento);
    $("input[title='Data da Autorização']").val(dateToString(dateConvert(plano.DataDaAutorização)));
    $("input[title='Previsão da Implantação']").val(dateToString(dateConvert(plano.PrevisãoDaImplantação)));
    $("input[title='Data de Entrada em Operação']").val(dateToString(dateConvert(plano.DataDeEntradaEmOperação)));
    $("select[title='Status da Revitalização Campo Obrigatório'] option:contains(" + plano.StatusDaRevitalizaçãoValue + ")").attr('selected', 'selected');

    $("input[title='Aquisição de Materiais - Início']").val(dateToString(dateConvert(plano.AquisiçãoDeMateriaisInício)));
    $("input[title='Aquisição de Materiais - Fim']").val(dateToString(dateConvert(plano.AquisiçãoDeMateriaisFim)));
    $("input[title='Obras Civis - Início']").val(dateToString(dateConvert(plano.ObrasCivisInício)));
    $("input[title='Obras Civis - Fim']").val(dateToString(dateConvert(plano.ObrasCivisFim)));
    $("input[title='Montagem Eletromecânica - Início']").val(dateToString(dateConvert(plano.MontagemEletromecânicaInício)));
    $("input[title='Montagem Eletromecânica - Fim']").val(dateToString(dateConvert(plano.MontagemEletromecânicaFim)));
    $("input[title='Comissionamento - Início']").val(dateToString(dateConvert(plano.ComissionamentoInício)));
    $("input[title='Comissionamento - Fim']").val(dateToString(dateConvert(plano.ComissionamentoFim)));

    $("input[title='Operação Comercial']").val(dateToString(dateConvert(plano.OperaçãoComercial)));
    $("textarea[title='Observações Gerais']").val(plano.ObservaçõesGerais);
    $("select[title='Alteração Solicitada']").val("Alteração");
}

function carregaMelhoriaReforco(plano) {

    $("input[title='Numeração']").val(plano.Numeração);
    $("input[title='Ciclo']").val(plano.Ciclo.NomeDoCiclo);
    $("select[title='Região Campo Obrigatório'] option:contains(" + plano.RegiãoValue + ")").attr('selected', 'selected');
    $("input[title='Agente Campo Obrigatório']").val(plano.Agente);
    //$("select[title='Instalação Campo Obrigatório'] option:contains(" + retira_acentos(plano.Instalação) + ")").attr('selected', 'selected');
    //$("select[title='Instalação Campo Obrigatório'] option").filter(function () {
    //    return retira_acentos(this.text) == plano.Instalação;
    //}).attr('selected', 'selected');
    var instalacao = false;
    $("select[title='Instalação Campo Obrigatório'] option").filter(function () {
        //if(retira_acentos(this.text) == plano.Instalação){
        if (retira_acentos(this.text) == retira_acentos(plano.Instalação)) {
            instalacao = true;
            //return plano.Instalação;
            return this.text;
        }
    }).attr('selected', 'selected');

    if (!instalacao) {
        $("select[title='Instalação Campo Obrigatório'] option:contains(Outra Instalação)").attr('selected', 'selected');
        $("input[title='Instalação - Outros']").closest("tr").show();
        $("input[title='Instalação - Outros']").val(plano.Instalação);
    }

    FuncaoTransmissao($("select[title='Instalação Campo Obrigatório']").val());
    $("select[title='Tensão Nominal (KV) Campo Obrigatório']").val(plano.KV);
    $("select[title='Tipo de Agente Campo Obrigatório'] option:contains(" + plano.TipoDeAgente + ")").attr('selected', 'selected');
    //$("select[title='Função de Transmissão Campo Obrigatório'] option").filter(function () {
    //    return this.text == plano.FunçãoDeTransmissão;
    //}).attr('selected', 'selected');
    //$("select[title='Função de Transmissão Campo Obrigatório'] option[" +  plano.FunçãoDeTransmissão + ")").attr('selected', 'selected');

    var FT = false;
    $("select[title='Função de Transmissão Campo Obrigatório'] option").filter(function () {
        if (retira_acentos(this.text) == retira_acentos(plano.FunçãoDeTransmissão)) {
            FT = true;
            //return plano.FunçãoDeTransmissão;
            return this.text;
        }
    }).attr('selected', 'selected');

    if (!FT) {
        $("select[title='Função de Transmissão Campo Obrigatório'] option:contains(Outra Função de transmissão)").attr('selected', 'selected');
        $("input[title='Função de Transmissão - Outros']").closest("tr").show();
        $("input[title='Função de Transmissão - Outros']").val(plano.FunçãoDeTransmissão);
    }


    $("input[title='Origem da Indicação Campo Obrigatório']").val(plano.OrigemDaIndicação);
    $("input[title='Tipo de Revitalização']").val(plano.TipoDeRevitalização);
    $("textarea[title='Revitalização Necessária Campo Obrigatório']").val(plano.RevitalizaçãoNecessária);
    $("textarea[title='Justificativa Campo Obrigatório']").val(plano.Justificativa);
    $("textarea[title='Justificativa da Exclusão']").val(plano.JustificativaDaExclusão);
    $("textarea[title='Observação']").val(plano.Observação);
    //$("input[title='Classificação Campo Obrigatório']").val(plano.ClassificaçãoFurnas);
    $("select[title='Classificação Campo Obrigatório'] option").filter(function () {
        if (this.text == plano.ClassificaçãoFurnas) {
            return this.text;
        }
    }).attr('selected', 'selected');
    $("select[title='Prazo após emissão Do PMI/Resolução Aneel (meses) Campo Obrigatório']").val(plano.PrazoApósEmissãoDoPMIResoluçãoAneelMeses);
    $("input[title='Projeto que Originou a Indicação']").val(plano.ProjetoQueOriginouAIndicação);
    $("input[title='N° da recomendação no SGR']").val(plano.NDaRecomendaçãoNoSGR);
    try {
        $("input[title='Custo Estimado (R$)']").val(plano.CustoEstimadoR.toString().replace('.', ','));
    } catch (ex) { }
    $("textarea[title='Descrição Do equipamento a ser substituído ou dos componentes da linha']").val(plano.DescriçãoDoEquipamentoASerSubstituídoOuDosComponentesDaLinha);
    $("input[title='Identificação Do equipamento a ser substituído (número operativo, Do unifilar, etc.)']").val(plano.IdentificaçãoDoEquipamentoASerSubstituídoNúmeroOperativoDoUnifilarEtc);
    $("textarea[title='Evidência que justifique a Melhoria ou Reforço']").val(plano.EvidênciaQueJustifiqueAMelhoriaOuReforço);
    $("input[title='Fabricante']").val(plano.Fabricante);
    $("input[title='Modelo']").val(plano.Modelo);
    $("input[title='N° Série']").val(plano.NSérie);
    $("input[title='Ano Início em Operação']").val(plano.AnoInícioEmOperação);
    $("input[title='Taxa de Depreciação (MCPSE – versão 9)']").val(plano.TaxaDeDepreciaçãoMCPSEVersão9);
    $("input[title='Ano Final de Vida Útil']").val(plano.AnoFinalDeVidaÚtil);
    $("input[title='Substituir nos próximos 4 anos (a partir de 1º de fevereiro Do ano corrente)?']").val(plano.SubstituirNosPróximos4AnosAPartirDe1ºDeFevereiroDoAnoCorrente);
    $("input[title='Equipamento apto a permanecer por tempo adicional à Vida Útil']").val(plano.EquipamentoAptoAPermanecerPorTempoAdicionalÀVidaÚtil);
    $("textarea[title='Ações propostas']").val(plano.AçõesPropostas);
    $("input[title='Aumento de vida útil esperado, a partir da implantação Do reforço(anos)']").val(plano.AumentoDeVidaÚtilEsperadoAPartirDaImplantaçãoDoReforçoAnos);
    $("input[title='Valor a ser investido(R$)']").val(plano.ValorASerInvestidoR);
    $("input[title='Situação da Análise']").val(plano.SituaçãoDaAnáliseValue);
    $("input[title='Indicações']").val(plano.Indicações);
    $("textarea[title='Observação ONS']").val(plano.ObservaçãoONS);
    $("input[title='Classificação ONS']").val(plano.ClassificaçãoONS);

    $("input[title='Responsável pela Análise']").val(plano.ResponsávelPelaAnálise);
    $("input[title='Data de Necessidade Sistêmica']").val(dateToString(dateConvert(plano.DataDeNecessidadeSistêmica)));
    $("input[title='Oficio/Resolução/Autorização ANEEL relativa a substituição Do equipamento/PMI']").val(plano.OficioResoluçãoAutorizaçãoANEELRelativaASubstituiçãoDoEquipamentoPMI);
    $("input[title='Data da Autorização/Emissão PMI']").val(dateToString(dateConvert(plano.DataDaAutorizaçãoEmissãoPMI)));
    $("input[title='Previsão da Implantação']").val(dateToString(dateConvert(plano.PrevisãoDaImplantação)));
    $("select[title='Status da Revitalização Campo Obrigatório'] option:contains(" + plano.StatusDaRevitalizaçãoValue + ")").attr('selected', 'selected');
    $("input[title='Aquisição de Materiais - Início']").val(dateToString(dateConvert(plano.AquisiçãoDeMateriaisInício)));
    $("input[title='Aquisição de Materiais - Fim']").val(dateToString(dateConvert(plano.AquisiçãoDeMateriaisFim)));
    $("input[title='Obras Civis - Início']").val(dateToString(dateConvert(plano.ObrasCivisInício)));
    $("input[title='Obras Civis - Fim']").val(dateToString(dateConvert(plano.ObrasCivisFim)));
    $("input[title='Montagem Eletromecânica - Início']").val(dateToString(dateConvert(plano.MontagemEletromecânicaInício)));
    $("input[title='Montagem Eletromecânica - Fim']").val(dateToString(dateConvert(plano.MontagemEletromecânicaFim)));
    $("input[title='Comissionamento - Início']").val(dateToString(dateConvert(plano.ComissionamentoInício)));
    $("input[title='Comissionamento - Fim']").val(dateToString(dateConvert(plano.ComissionamentoFim)));

    $("input[title='Operação Comercial']").val(dateToString(dateConvert(plano.OperaçãoComercial)));
    $("textarea[title='Observações Gerais']").val(plano.ObservaçõesGerais);
    $("input[title='Numeração PMI / PMIS']").val(plano.NumeraçãoPMIPMIS);

    $("select[title='Alteração Solicitada']").val("Alteração");
}

function TipoPlanoP(id) {
    var urlQuery = _spPageContextInfo.webAbsoluteUrl + "/_vti_bin/listdata.svc/Planos?$expand=Ciclo&$filter=ID eq " + id;
    //+ "&$expand=Ciclos,DominioDeInstalacao";
    var ret = [];
    $.ajax({
        type: "GET",
        contentType: "application/json;charset=ISO-8859-1",
        url: encodeURI(urlQuery),
        cache: false,
        async: false,
        dataType: 'json',
        headers: { "Accept": "application/json; odata=verbose" },
        success: function (data) {
            if (data.d.results.length > 0) {
                ret = data.d.results[0];
            }
        }
    });

    return ret;
}

function getWQuerystringP(ji) {
    hu = parent.location.search.substring(1).replace("[^a-zA-Z0-9/]", "-");
    gy = hu.split("?");
    for (i = 0; i < gy.length; i++) {
        ft = gy[i].split("=");
        if (ft[0] == ji) {
            return ft[1];
        }
    }
}


function getQueryString() {
    var key = false, res = {}, itm = null;
    // get the query string without the ?
    var qs = location.search.substring(1);
    // check for the key as an argument
    if (arguments.length > 0 && arguments[0].length > 1)
        key = arguments[0];
    // make a regex pattern to grab key/value
    var pattern = /([^&=]+)=([^&]*)/g;
    // loop the items in the query string, either
    // find a match to the argument, or build an object
    // with key/value pairs
    while (itm = pattern.exec(qs)) {
        if (key !== false && decodeURIComponent(itm[1]) === key)
            return decodeURIComponent(itm[2]);
        else if (key === false)
            res[decodeURIComponent(itm[1])] = decodeURIComponent(itm[2]);
    }

    return key === false ? res : null;
}

function retira_acentos(palavra) {
    com_acento = 'áàãâäéèêëíìîïóòõôöúùûüÁÀÃÂÄÉÈÊËÍÌÎÏÓÒÕÖÔÚÙÛÜ';
    sem_acento = 'aaaaaeeeeiiiiooooouuuuAAAAAEEEEIIIIOOOOOUUUU';
    nova = '';
    if (palavra != null) {
        for (i = 0; i < palavra.length; i++) {
            try {
                if (com_acento.search(palavra.substr(i, 1)) >= 0) {
                    nova += sem_acento.substr(com_acento.search(palavra.substr(i, 1)), 1);
                }
                else {
                    nova += palavra.substr(i, 1);
                }
            } catch (ea) {
                nova += palavra[i];
            }
        }
        return nova.replace(/\s/g, '');
    }
    else
        return "";
}