$(document).ready(function () {
    isMember("Administradores GGA");

    if (e_ListaGestaoDisp())
        $('input[value=Cancelar]').click(function () { history.go(-1); });

    if (e_ListaGestao()) {
        var acoes = null;
        try { acoes = getWQuerystring("Acoes"); } catch (ex) { }
        if (acoes == null) {
            $("#s4-ribbonrow").hide();
            $(".nomeSite").hide();
            //$("#globalNavBox").remove();
            //$("input[value='Cancelar']").hide();
            //$("input[title='No. SGPMR']").closest("tr").hide();
            $("input[title='No. SGPMR']").attr('disabled', 'disabled');
        }
        else {
            $("#s4-ribbonrow").hide();
            $(".nomeSite").hide();
            //$("input[value='Salvar']").val("Salvar Alterações");
            //$("input[value='Cancelar']").val("Sair");
            //Carregar informações
            //CarregaGestaoInterna(getWQuerystring("ID"));
        }
    }

    var id;

    if (document.location.pathname.indexOf("/EditForm.aspx") > -1) {
        /*$("select[title='No. SGPMR'] option").each(function () {// Campo Obrigatório
            if ($(this).is(':selected')) {
                $(this).attr('selected', true);
            }
            else {
                $(this).remove();
            }
        });*/
        $("input[title='No. SGPMR']").attr('disabled', 'disabled');
    }
    else {
        try {
            id = getWQuerystring("RootFolder");

            if (id == undefined)
                id = getQueryString("RootFolder");

        } catch (ex) { }

        if (typeof id != 'undefined') {
            if (id != "") {
                $("input[title='No. SGPMR']").val(id.replace(',', ''));// Campo Obrigatório
                $("input[title='No. SGPMR']").attr('disabled', 'disabled');
                /*$("select[title='No. SGPMR'] option").each(function () {// Campo Obrigatório
                    if ($(this).is(':selected')) {
                        $(this).attr('selected', true);
                    }
                    else {
                        $(this).remove();
                    }
                });*/
            }
        }
    }
});

function e_ListaGestao() {
    var pl = parent.location.href.split('/');
    for (i = 0; i < pl.length; i++) {
        if (pl[i] == "PlanosMudanca") {
            return true;
        }
    }
}
function e_ListaGestaoDisp() {
    var pl = parent.location.href.split('/');
    for (i = 0; i < pl.length; i++) {
        if (pl[i] == "GestaoInterna") {
            return true;
        }
    }
}

function isMember(groupName) {
    $.ajax({
        url: _spPageContextInfo.webAbsoluteUrl + "/_api/web/sitegroups/getByName('" + groupName + "')/Users?$filter=Id eq " + _spPageContextInfo.userId,
        method: "GET",
        headers: { "Accept": "application/json; odata=verbose" },
        success: function (data) {
            if (data.d.results[0] == undefined) {
                $("select[title='Aprovação da solicitação']").closest("tr").hide();
            }
        }
    });
}
function getWQuerystring(ji) {
    hu = parent.location.search.substring(1).replace("[^a-zA-Z0-9/]", "-");
    gi = decodeURIComponent(hu).split("?");
    gy = gi[1].split("&");
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
    var qs = decodeURIComponent(location.search.substring(1));
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

function PreSaveAction() {
    try {
        $('#ErroDataCustomizado').remove(); $('#ErroDataCustomizado1').remove(); $('#ErroDataCustomizado2').remove(); $('#ErroDataCustomizado3').remove();
        $('#ErroDataCustomizado4').remove(); $('#ErroDataCustomizado5').remove(); $('#ErroDataCustomizado6').remove(); $('#ErroDataCustomizado7').remove();
    }
    catch (ex) { }
    var retorno = true;

    var dt = $("input[title='Previsão de implantação FURNAS Campo Obrigatório']").val();

    if ($("input[title='Órgão solicitante Campo Obrigatório']").val() == "") {
        $("input[title='Órgão solicitante Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado1' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
        retorno = false;
    }
    if ($("input[title='Responsável no órgão solicitante Campo Obrigatório']").val() == "") {
        $("input[title='Responsável no órgão solicitante Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado2' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
        retorno = false;
    }
    if ($("input[title='Órgão Gestor da obra Campo Obrigatório']").val() == "") {
        $("input[title='Órgão Gestor da obra Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado3' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
        retorno = false;
    }
    if ($("input[title='Responsável no órgão gestor da obra Campo Obrigatório']").val() == "") {
        $("input[title='Responsável no órgão gestor da obra Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado4' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
        retorno = false;
    }
    if ($("input[title='Status de revitalização FURNAS Campo Obrigatório']").val() == "") {
        $("input[title='Status de revitalização FURNAS Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado5' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
        retorno = false;
    }
    if (dt == "") {
        $($("input[title='Previsão de implantação FURNAS Campo Obrigatório']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado6' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
        retorno = false;
    }
    if (dt != "" && !isDate(dt)) {
        $($("input[title='Previsão de implantação FURNAS Campo Obrigatório']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
        retorno = false;
    }

    var dtInf = $("input[title='Data da última atualização de informações']").val();
    if (dtInf != "" && !isDate(dtInf)) {
        $($("input[title='Data da última atualização de informações']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado7' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
        retorno = false;
    }

    return retorno;
}

function isDate(txtDate) {
    var currVal = txtDate;
    if (currVal == '')
        return false;

    //Declare Regex 
    var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
    var dtArray = currVal.match(rxDatePattern); // is format OK?

    if (dtArray == null)
        return false;

    //Checks for mm/dd/yyyy format.
    dtMonth = dtArray[3];
    dtDay = dtArray[1];
    dtYear = dtArray[5];

    if (dtMonth < 1 || dtMonth > 12)
        return false;
    else if (dtDay < 1 || dtDay > 31)
        return false;

    else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
        return false;

    else if (dtMonth == 2) {
        var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
        if (dtDay > 29 || (dtDay == 29 && !isleap))
            return false;
    }
    return true;
}

function CloseForm() {
    window.frameElement.cancelPopUp();
    return false;
}

function CarregaGestaoInterna(gestao) {
    var ID_Gestao;
    var query = _spPageContextInfo.webAbsoluteUrl + "/_vti_bin/listdata.svc/GestãoInterna?$filter=ID eq " + gestao;
    $.ajax({
        type: "GET",
        contentType: "application/json;charset=ISO-8859-1",
        url: encodeURI(query),
        cache: false,
        async: false,
        dataType: 'json',
        headers: { "Accept": "application/json; odata=verbose" },
        success: function (data) {
            if (data.d.results.length > 0) {
                $.each(data.d.results, function (j, item) {
                    $("input[title='Forma de Solicitação']").val(item.FormaDeSolicitação);
                    $("input[title='Órgão solicitante Campo Obrigatório']").val(item.ÓrgãoSolicitante);
                    $("input[title='Responsável no órgão solicitante Campo Obrigatório']").val(item.ResponsávelNoÓrgãoSolicitante);
                    $("input[title='Documento interno de aprovação']").val(item.DocumentoInternoDeAprovação);
                    $("input[title='Órgão Gestor da obra Campo Obrigatório']").val(item.ÓrgãoGestorDaObra);
                    $("input[title='Responsável no órgão gestor da obra Campo Obrigatório']").val(item.ResponsávelNoÓrgãoGestorDaObra); //textarea
                    $("textarea[title='Considerações do Responsável pelo gerenciamento']").val(item.ConsideraçõesDoResponsávelPeloGerenciamento); //textarea
                    $("input[title='Número do PEP']").val(item.NúmeroDoPEP);
                    $("input[title='Local de instalação SAP-PM']").val(item.LocalDeInstalaçãoSAPPM);
                    $("input[title='N° do Equipamento SAP-PM']").val(item.NDoEquipamentoSAPPM);
                    $("input[title='Controle de atos autorizativos']").val(item.ControleDeAtosAutorizativos);
                    $("input[title='Ato Autorizativo Atual, PAR, POT ou PET']").val(item.AtoAutorizativoAtualPARPOTOuPET);
                    $("input[title='Previsão de implantação FURNAS Campo Obrigatório']").val(dateToString(dateConvert(item.PrevisãoDeImplantaçãoFURNAS)));// date
                    $("input[title='Status de revitalização FURNAS Campo Obrigatório']").val(item.StatusDeRevitalizaçãoFURNAS);
                    $("input[title='Documento de pleito de receita']").val(item.DocumentoDePleitoDeReceita);
                    $("input[title='Status do pleito de Receita']").val(item.StatusDoPleitoDeReceita);
                    $("textarea[title='Consideracões de Gestão de Ativos']").val(item.ConsideracõesDeGestãoDeAtivos); //textatra
                    $("input[title='Data da última atualização de informações']").val(dateToString(dateConvert(item.DataDaÚltimaAtualizaçãoDeInformações)));//date
                });
            }
        }
    });
}