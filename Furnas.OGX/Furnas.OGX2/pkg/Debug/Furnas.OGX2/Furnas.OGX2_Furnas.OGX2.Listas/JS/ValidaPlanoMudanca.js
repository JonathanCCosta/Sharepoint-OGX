$(document).ready(function () {

    $('input[value=Cancelar]').click(function () { history.go(-1); });
    //var id = getWQuerystring("ID");
    $("select[title='Alteração Solicitada']").closest("tr").hide();
    $("input[title='Ver detalhes Campo Obrigatório']").closest("tr").hide();

    if (document.location.pathname.indexOf("/EditForm.aspx") > -1) {
        if ($("select[title='Instalação Campo Obrigatório'] option:selected").text() == "Outra Instalação")
            $("input[title='Instalação - Outros']").closest("tr").show();

        if ($("select[title='Função de Transmissão Campo Obrigatório'] option:selected").text() == "Outra Função de transmissão")
            $("input[title='Função de Transmissão - Outros']").closest("tr").show();
    }
    else {
        $("input[title='Instalação - Outros']").closest("tr").hide();
        $("input[title='Função de Transmissão - Outros']").closest("tr").hide();

        $("select[title='Instalação Campo Obrigatório'] option:eq(1)").attr('selected', 'selected');
    }

    $("input[title='Ano Início em Operação']").keypress(function (e) {
        if (e.which != 8 && e.which != 0 && (e.which < 45 || e.which > 57)) {
            return false;
        }
    });

    $("input[title='Ano Final de Vida Útil']").keypress(function (e) {
        if (e.which != 8 && e.which != 0 && (e.which < 45 || e.which > 57)) {
            return false;
        }
    });

    $("input[title='Custo Estimado (R$)']").keypress(function (e) {
        if (e.which != 8 && e.which != 0 && (e.which < 45 || e.which > 57)) {
            return false;
        }
    });

    $("input[title='Valor a ser investido(R$)']").keypress(function (e) {
        if (e.which != 8 && e.which != 0 && (e.which < 45 || e.which > 57)) {
            return false;
        }
    });

    if (document.location.pathname.indexOf("/EditForm.aspx") > -1) {
        $("select[title='Tipo de Conteúdo']").closest("tr").hide();
        FuncaoTransmissao($("select[title='Instalação Campo Obrigatório']").val());
    }
    else {
        //$($(".ms-cui-ctl-large")[1]).hide();
        var param = getWQuerystringP("ID");
        $("input[title='Ciclo']").attr('disabled', 'disabled');
        var plano = TipoPlanoP(param);

        if (param != undefined) {
            if (plano.TipoDeConteúdo == "Plano Por Superação") {
                carregaSuperacao(plano);
            }
            else {
                carregaMelhoriaReforco(plano);
            }
        }
        else {
            $("input[title='Ciclo']").val(getQueryString("Ciclo"));
            $("input[title='Ciclo']").attr('disabled', 'disabled');
            FuncaoTransmissao($("select[title='Instalação Campo Obrigatório']").val());
        }
    }


    $("select[title='Instalação Campo Obrigatório']").change(function () {
        FuncaoTransmissao($("select[title='Instalação Campo Obrigatório']").val());
        if ($("select[title='Instalação Campo Obrigatório'] option:selected").text() == "Outra Instalação")
            $("input[title='Instalação - Outros']").closest("tr").show();
        else {
            $("input[title='Instalação - Outros']").closest("tr").hide();
            $("input[title='Instalação - Outros']").val("");
        }

        if ($("select[title='Função de Transmissão Campo Obrigatório'] option:selected").text() == "Outra Função de transmissão")
            $("input[title='Função de Transmissão - Outros']").closest("tr").show();
        else {
            $("input[title='Função de Transmissão - Outros']").closest("tr").hide();
            $("input[title='Função de Transmissão - Outros']").val("");
        }
    });

    $("select[title='Função de Transmissão Campo Obrigatório']").change(function () {
        if ($("select[title='Função de Transmissão Campo Obrigatório'] option:selected").text() == "Outra Função de transmissão")
            $("input[title='Função de Transmissão - Outros']").closest("tr").show();
        else {
            $("input[title='Função de Transmissão - Outros']").closest("tr").hide();
            $("input[title='Função de Transmissão - Outros']").val("");
        }
    });

    $($("input[type='radio']")[1]).change(function () {
        $("select[title='Tensão Nominal (KV) Campo Obrigatório'] option:contains(Outra)").attr('selected', 'selected');
    });

    if (getWQuerystring("IDGestao") == undefined && document.location.pathname.indexOf("/EditForm.aspx") == -1 && param == undefined) {
        //OpenDialogE('/portalativosfurnas/gga/Lists/GestaoInterna/NewForm.aspx', "Antes é necessário preencher os campos de Gestão para prosseguir...");
        $("input[title='Número de Gestão']").closest("tr").hide();
    }
    else {
        if (document.location.pathname.indexOf("/EditForm.aspx") > -1)
            $("input[title='Número de Gestão']").closest("tr").hide();
        else {
            if (getWQuerystring("IDGestao") == undefined) {
                var UrlD = _spPageContextInfo.webAbsoluteUrl + "/Lists/GestaoInterna/NewForm.aspx";
                $("input[title='Número de Gestão']").closest("tr").hide();//OpenDialogG(UrlD, 'Deseja alterar as informações de Gestão do Plano?');
            }
            else {
                $("input[title='Número de Gestão']").val(getWQuerystring("IDGestao")).closest("tr").hide();
                $("input[value='Cancelar']").hide(); $($(".ms-cui-ctl-large")[1]).hide();
            }
        }
    }

    try {
        $("a[id^='idHomePageNewItem']").attr("onclick", "OpenDialogT('" + _spPageContextInfo.webAbsoluteUrl + "/Lists/GestaoInterna/NewForm.aspx?RootFolder=" + $("input[title='Numeração']").val() + "','Adicionar Gestão Interna');return false;");
    }
    catch (ex) { }

    SP.SOD.executeFunc('SP.js', 'SP.ClientContext', function () {
        CurrentUserMemberOfGroup("Administradores GGA", function (isCurrentUserInGroup) {
            if (!isCurrentUserInGroup) {
                $("select[title='Status da solicitação Campo Obrigatório']").closest("tr").hide();
                $("input[title='Numeração']").closest("tr").hide();//.attr('disabled','disabled');

            }
        });
    });

});

function OpenDialogE(url, title) {
    var options = {
        url: url,
        width: 850,
        height: 600,
        title: title,
        showMaximized: true,
        showClose: false,
        dialogReturnValueCallback: Function.createDelegate(null, CloseDialogCallbackE)
    };
    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);
}

function OpenDialogG(url, title) {
    var options = {
        url: url,
        width: 850,
        height: 600,
        title: title,
        showMaximized: true,
        //showClose: false,
        dialogReturnValueCallback: Function.createDelegate(null, CloseDialogCallbackG)
    };
    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);
}
function CloseDialogCallbackG(dialogResult, returnValue) {
    if (dialogResult != 0) {
        var ID_Gestao;
        var query = _spPageContextInfo.webAbsoluteUrl + "/_vti_bin/listdata.svc/GestãoInterna?$top=1&$select=ID&$orderby=Criado desc";
        $.ajax({
            type: "GET",
            contentType: "application/json;charset=ISO-8859-1",
            url: encodeURI(query),
            cache: false,
            async: false,
            dataType: 'json',
            headers: { "Accept": "application/json; odata=verbose" },
            success: function (data) {
                if (data.d.length > 0) {
                    ID_Gestao = data.d[0].ID;
                }
            }
        });

        var url = window.parent.location.href + "&IDGestao=" + ID_Gestao;
        window.parent.location.href = url;
    }
}


function CloseDialogCallbackE(dialogResult, returnValue) {
    var ID_Gestao;
    var query = _spPageContextInfo.webAbsoluteUrl + "/_vti_bin/listdata.svc/GestãoInterna?$top=1&$select=ID&$orderby=Criado desc";
    $.ajax({
        type: "GET",
        contentType: "application/json;charset=ISO-8859-1",
        url: encodeURI(query),
        cache: false,
        async: false,
        dataType: 'json',
        headers: { "Accept": "application/json; odata=verbose" },
        success: function (data) {
            if (data.d.length > 0) {
                ID_Gestao = data.d[0].ID;
            }
        }
    });

    var url = window.parent.location.href + "&IDGestao=" + ID_Gestao;
    window.parent.location.href = url;

}

function isMember(groupName) {
    $.ajax({
        url: _spPageContextInfo.webAbsoluteUrl + "/_api/web/sitegroups/getByName('" + groupName + "')/Users?$filter=Id eq " + _spPageContextInfo.userId,
        method: "GET",
        headers: { "Accept": "application/json; odata=verbose" },
        success: function (data) {
            if (data.d.results[0] == undefined) {
                $("select[title='Status da solicitação Campo Obrigatório']").closest("tr").hide();
                $("input[title='Numeração']").closest("tr").hide();//.attr('disabled','disabled');
            }
        }
    });
}

function FuncaoTransmissao(id) {
    var urlQuery = _spPageContextInfo.webAbsoluteUrl + "/_vti_bin/listdata.svc/DomínioDeFunçãoDeTransmissão?$filter=DomínioDeInstalaçãoId eq " + id + " &$orderby=Título asc";

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
                $("select[title='Função de Transmissão Campo Obrigatório']").empty();

                $.each(data.d.results, function (j, item) {
                    $("select[title='Função de Transmissão Campo Obrigatório']").append('<option value="' + item.ID + '">' + item.Título + '</option>');
                });
            }
        }
    });
}


function PreSaveAction() {

    var param = getQueryString("ContentTypeId");
    var retorno = true;
    if (param == "0x010033F536D5074F4318A40229399F349CC1001AF60EE56DAC7B42B4530C8AF8853335") {
        //return Superacao();
        retorno = Superacao();
    }
    else {
        //return MelhoriaRefoco();
        retorno = MelhoriaRefoco();
    }

    if (retorno == false)
        return false;

    if (getWQuerystringP("ID") == undefined) {
        if ($("input[title='Número de Gestão']").val() == "") {
            alert("É necessário preencher os dados de Gestão Interna, na parte inferir da tela para concluir a solicitação.");
            retorno = false;
        }
    }

    return retorno;
}

function Superacao() {
    try {
        $('#ErroDataCustomizado').remove(); $('#ErroDataCustomizado1').remove(); $('#ErroDataCustomizado2').remove(); $('#ErroDataCustomizado3').remove();
        $('#ErroDataCustomizado4').remove(); $('#ErroDataCustomizado5').remove(); $('#ErroDataCustomizado6').remove(); $('#ErroDataCustomizado7').remove();
        $('#ErroDataCustomizado8').remove(); $('#ErroDataCustomizado9').remove(); $('#ErroDataCustomizado10').remove(); $('#ErroDataCustomizado11').remove();
        $('#ErroDataCustomizado12').remove(); $('#ErroDataCustomizado13').remove(); $('#ErroDataCustomizado14').remove(); $('#ErroDataCustomizado15').remove();
        $('#ErroDataCustomizado16').remove(); $('#ErroDataCustomizado17').remove(); $('#ErroDataCustomizado18').remove(); $('#ErroDataCustomizado19').remove(); $('#ErroDataCustomizado20').remove();
        $('#ErroDataCustomizado21').remove(); $('#ErroDataCustomizado22').remove(); $('#ErroDataCustomizado23').remove(); $('#ErroDataCustomizado24').remove(); $('#ErroDataCustomizado25').remove();
    }
    catch (ex) { }
    var retorno = true;

    if ($("select[title='Região Campo Obrigatório']").val() == "" || $("select[title='Região Campo Obrigatório']").val() == "Selecione uma opção") {
        $("select[title='Região Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Selecione uma opação.</span>");
        retorno = false; $("select[title='Região Campo Obrigatório']").focus();
    }
    if ($("input[title='Agente Campo Obrigatório']").val() == "") {
        $("input[title='Agente Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado1' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
        retorno = false; $("input[title='Agente Campo Obrigatório']").focus();
    }
    if ($("input[select='Instalação Campo Obrigatório']").val() == "") {
        $("input[select='Instalação Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado2' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
        retorno = false; $("input[select='Instalação Campo Obrigatório']").focus();
    }
    if ($("select[title='Tensão Nominal (KV) Campo Obrigatório']").val() == "Selecione uma opção" && $($("input[type='radio']")[0]).prop('checked')) {
        $("select[title='Tensão Nominal (KV) Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado3' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Selecione uma opção.</span>");
        retorno = false; $("select[title='Tensão Nominal (KV) Campo Obrigatório']").focus();
    }
    else if ($("input[title='Tensão Nominal (KV) Campo Obrigatório: Especifique seu próprio valor:']").val() == "" && $($("input[type='radio']")[1]).prop('checked')) {
        $("input[title='Tensão Nominal (KV) Campo Obrigatório: Especifique seu próprio valor:']").closest("td").append("<span id='ErroDataCustomizado3' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
        retorno = false; $("input[title='Tensão Nominal (KV) Campo Obrigatório: Especifique seu próprio valor:']").focus();
    }

    if ($("select[title='Tipo de Agente Campo Obrigatório']").val() == "Selecione uma opção") {
        $("select[title='Tipo de Agente Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado4' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
        retorno = false; $("select[title='Tipo de Agente Campo Obrigatório']").focus();
    }
    if ($("input[title='Função de Transmissão Campo Obrigatório']").val() == "") {
        $("input[title='Função de Transmissão Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado5' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
        retorno = false; $("input[title='Função de Transmissão Campo Obrigatório']").focus();
    }
    if ($("select[title='Classificação Campo Obrigatório'] option:selected").text() == "Selecione uma opção") {
        $("select[title='Classificação Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado25' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Seleione uma opção</span>");
        retorno = false; $("select[title='Classificação Campo Obrigatório']").focus();
    }

    if ($("select[title='Status da Revitalização Campo Obrigatório']").val() == "" || $("select[title='Status da Revitalização Campo Obrigatório']").val() == "Selecione uma opção") {
        $("select[title='Status da Revitalização Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado6' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Selecione uma opção.</span>");
        retorno = false; $("select[title='Status da Revitalização Campo Obrigatório']")
    }
    if ($("select[title='Prazo após emissão Do PMI/Resolução Aneel (meses) Campo Obrigatório']").val() == "0") {
        $("select[title='Prazo após emissão Do PMI/Resolução Aneel (meses) Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado20' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Selecione uma opção.</span>");
        retorno = false; $("select[title='Prazo após emissão Do PMI/Resolução Aneel (meses) Campo Obrigatório']").focus();
    }
    if ($("select[title='Instalação Campo Obrigatório'] option:selected").text() == "Outra Instalação") {
        if ($("input[title='Instalação - Outros']").val() == "") {
            $("input[title='Instalação - Outros']").closest("td").append("<span id='ErroDataCustomizado23' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
            retorno = false; $("input[title='Instalação - Outros']").focus();
        }
    }
    if ($("select[title='Função de Transmissão Campo Obrigatório'] option:selected").text() == "Outra Função de transmissão") {
        if ($("input[title='Função de Transmissão - Outros']").val() == "") {
            $("input[title='Função de Transmissão - Outros']").closest("td").append("<span id='ErroDataCustomizado24' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
            retorno = false; $("input[title='Função de Transmissão - Outros']").focus();
        }
    }


    var dt = $("input[title='Data de Necessidade']").val();
    var dt1 = $("input[title='Data da Autorização']").val();
    var dt2 = $("input[title='Previsão da Implantação']").val();
    var dt3 = $("input[title='Data de Entrada em Operação']").val();
    var dt4 = $("input[title='Aquisição de Materiais - Início']").val();
    var dt5 = $("input[title='Aquisição de Materiais - Fim']").val();
    var dt6 = $("input[title='Obras Civis - Início']").val();
    var dt7 = $("input[title='Obras Civis - Fim']").val();
    var dt8 = $("input[title='Montagem Eletromecânica - Início']").val();
    var dt9 = $("input[title='Montagem Eletromecânica - Fim']").val();
    var dt10 = $("input[title='Comissionamento - Início']").val();
    var dt11 = $("input[title='Comissionamento - Fim']").val();
    var dt12 = $("input[title='Operação Comercial']").val();

    if (dt != "" && !isDate(dt)) {
        $($("input[title='Data de Necessidade']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado7' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
        retorno = false; $("input[title='Data de Necessidade']").focus();
    }
    if (dt1 != "" && !isDate(dt1)) {
        $($("input[title='Data da Autorização']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado8' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
        retorno = false; $("input[title='Data da Autorização']").focus();
    }
    if (dt2 != "" && !isDate(dt2)) {
        $($("input[title='Previsão da Implantação']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado9' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
        retorno = false; $("input[title='Previsão da Implantação']").focus();
    }
    if (dt3 != "" && !isDate(dt3)) {
        $($("input[title='Data de Entrada em Operação']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado10' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
        retorno = false; $("input[title='Data de Entrada em Operação']").focus();
    }
    if (dt4 != "" && !isDate(dt4)) {
        $($("input[title='Aquisição de Materiais - Início']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado11' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
        retorno = false; $("input[title='Aquisição de Materiais - Início']").focus();
    }
    if (dt5 != "" && !isDate(dt5)) {
        $($("input[title='Aquisição de Materiais - Fim']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado12' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
        retorno = false; $("input[title='Aquisição de Materiais - Fim']").focus();
    }
    if (dt6 != "" && !isDate(dt6)) {
        $($("input[title='Obras Civis - Início']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado13' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
        retorno = false; $("input[title='Obras Civis - Início']").focus();
    }
    if (dt7 != "" && !isDate(dt7)) {
        $($("input[title='Obras Civis - Fim']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado14' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
        retorno = false; $("input[title='Obras Civis - Fim']").focus();
    }
    if (dt8 != "" && !isDate(dt8)) {
        $($("input[title='Montagem Eletromecânica - Início']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado15' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
        retorno = false; $("input[title='Montagem Eletromecânica - Início']").focus();
    }
    if (dt9 != "" && !isDate(dt9)) {
        $($("input[title='Montagem Eletromecânica - Fim']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado16' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
        retorno = false; $("input[title='Montagem Eletromecânica - Fim']").focus();
    }
    if (dt10 != "" && !isDate(dt10)) {
        $($("input[title='Comissionamento - Início']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado17' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
        retorno = false; $("input[title='Comissionamento - Início']").focus();
    }
    if (dt11 != "" && !isDate(dt11)) {
        $($("input[title='Comissionamento - Fim']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado18' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
        retorno = false; $("input[title='Comissionamento - Fim']").focus();
    }
    if (dt12 != "" && !isDate(dt12)) {
        $($("input[title='Operação Comercial']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado19' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
        retorno = false; $("input[title='Operação Comercial']").focus();
    }

    return retorno;
}

function MelhoriaRefoco() {
    try {
        $('#ErroDataCustomizado').remove(); $('#ErroDataCustomizado1').remove(); $('#ErroDataCustomizado2').remove(); $('#ErroDataCustomizado3').remove();
        $('#ErroDataCustomizado4').remove(); $('#ErroDataCustomizado5').remove(); $('#ErroDataCustomizado6').remove(); $('#ErroDataCustomizado7').remove();
        $('#ErroDataCustomizado8').remove(); $('#ErroDataCustomizado9').remove(); $('#ErroDataCustomizado10').remove(); $('#ErroDataCustomizado11').remove();
        $('#ErroDataCustomizado12').remove(); $('#ErroDataCustomizado13').remove(); $('#ErroDataCustomizado14').remove(); $('#ErroDataCustomizado15').remove();
        $('#ErroDataCustomizado16').remove(); $('#ErroDataCustomizado17').remove(); $('#ErroDataCustomizado18').remove(); $('#ErroDataCustomizado19').remove();
        $('#ErroDataCustomizado20').remove(); $('#ErroDataCustomizado21').remove(); $('#ErroDataCustomizado22').remove(); $('#ErroDataCustomizado23').remove(); $('#ErroDataCustomizado25').remove(); $('#ErroDataCustomizado24').remove();
    }
    catch (ex) { }
    var retorno = true;

    if ($("select[title='Região Campo Obrigatório']").val() == "" || $("select[title='Região Campo Obrigatório']").val() == "Selecione uma opção") {
        $("select[title='Região Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Selecione uma opação.</span>");
        retorno = false; $("select[title='Região Campo Obrigatório']").focus();
    }
    if ($("input[title='Agente Campo Obrigatório']").val() == "") {
        $("input[title='Agente Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado1' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
        retorno = false; $("input[title='Agente Campo Obrigatório']").focus();
    }
    if ($("input[select='Instalação Campo Obrigatório']").val() == "") {
        $("input[select='Instalação Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado2' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
        retorno = false; $("input[select='Instalação Campo Obrigatório']").focus();
    }
    if ($("select[title='Tensão Nominal (KV) Campo Obrigatório']").val() == "Selecione uma opção" && $($("input[type='radio']")[0]).prop('checked')) {
        $("select[title='Tensão Nominal (KV) Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado3' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Selecione uma opção.</span>");
        retorno = false; $("select[title='Tensão Nominal (KV) Campo Obrigatório']").focus();
    }
    else if ($("input[title='Tensão Nominal (KV) Campo Obrigatório: Especifique seu próprio valor:']").val() == "" && $($("input[type='radio']")[1]).prop('checked')) {
        $("input[title='Tensão Nominal (KV) Campo Obrigatório: Especifique seu próprio valor:']").closest("td").append("<span id='ErroDataCustomizado3' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
        retorno = false; $("input[title='Tensão Nominal (KV) Campo Obrigatório: Especifique seu próprio valor:']").focus();
    }

    if ($("select[title='Tipo de Agente Campo Obrigatório']").val() == "Selecione uma opção") {
        $("select[title='Tipo de Agente Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado4' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
        retorno = false; $("select[title='Tipo de Agente Campo Obrigatório']").focus();
    }
    if ($("input[title='Função de Transmissão Campo Obrigatório']").val() == "") {
        $("input[title='Função de Transmissão Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado5' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
        retorno = false; $("input[title='Função de Transmissão Campo Obrigatório']").focus();
    }
    if ($("input[title='Origem da Indicação Campo Obrigatório']").val() == "") {
        $("input[title='Origem da Indicação Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado6' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
        retorno = false;
    }
    if ($("textarea[title='Revitalização Necessária Campo Obrigatório']").val() == "") {
        $("textarea[title='Revitalização Necessária Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado7' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
        retorno = false; $("textarea[title='Revitalização Necessária Campo Obrigatório']").focus();
    }
    if ($("select[title='Status da Revitalização Campo Obrigatório']").val() == "" || $("select[title='Status da Revitalização Campo Obrigatório']").val() == "Selecione uma opção") {
        $("select[title='Status da Revitalização Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado8' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Selecione uma opação.</span>");
        retorno = false; $("select[title='Status da Revitalização Campo Obrigatório']").focus();
    }
    if ($("select[title='Prazo após emissão Do PMI/Resolução Aneel (meses) Campo Obrigatório']").val() == "0") {
        $("select[title='Prazo após emissão Do PMI/Resolução Aneel (meses) Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado21' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Selecione uma opção.</span>");
        retorno = false; $("select[title='Prazo após emissão Do PMI/Resolução Aneel (meses) Campo Obrigatório']").focus();
    }
    if ($("select[title='Instalação Campo Obrigatório'] option:selected").text() == "Outra Instalação") {
        if ($("input[title='Instalação - Outros']").val() == "") {
            $("input[title='Instalação - Outros']").closest("td").append("<span id='ErroDataCustomizado21' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
            retorno = false; $("input[title='Instalação - Outros']").focus();
        }
    }
    if ($("select[title='Função de Transmissão Campo Obrigatório'] option:selected").text() == "Outra Função de transmissão") {
        if ($("input[title='Função de Transmissão - Outros']").val() == "") {
            $("input[title='Função de Transmissão - Outros']").closest("td").append("<span id='ErroDataCustomizado22' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
            retorno = false; $("input[title='Função de Transmissão - Outros']").focus();
        }
    }

    if ($("select[title='Classificação Campo Obrigatório'] option:selected").text() == "Selecione uma opção") {
        $("select[title='Classificação Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado23' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Seleione uma opção</span>");
        retorno = false; $("select[title='Classificação Campo Obrigatório']").focus();
    }

    if ($("textarea[title='Justificativa Campo Obrigatório']").val() == "") {
        $("textarea[title='Justificativa Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado24' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
        retorno = false; $("textarea[title='Justificativa Campo Obrigatório']").focus();
    }


    var dt = $("input[title='Data de Necessidade Sistêmica']").val();
    var dt2 = $("input[title='Data da Autorização/Emissão PMI']").val();
    var dt3 = $("input[title='Previsão da Implantação']").val();
    var dt4 = $("input[title='Aquisição de Materiais - Início']").val();
    var dt5 = $("input[title='Aquisição de Materiais - Fim']").val();
    var dt6 = $("input[title='Obras Civis - Início']").val();
    var dt7 = $("input[title='Obras Civis - Fim']").val();
    var dt8 = $("input[title='Montagem Eletromecânica - Início']").val();
    var dt9 = $("input[title='Montagem Eletromecânica - Fim']").val();
    var dt10 = $("input[title='Comissionamento - Início']").val();
    var dt11 = $("input[title='Comissionamento - Fim']").val();
    var dt12 = $("input[title='Operação Comercial']").val();


    if (dt != "" && !isDate(dt)) {
        $($("input[title='Data de Necessidade Sistêmica']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado9' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
        retorno = false; $("input[title='Data de Necessidade Sistêmica']").focus();
    }
    if (dt2 != "" && !isDate(dt2)) {
        $($("input[title='Data da Autorização/Emissão PMI']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado10' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
        retorno = false; $("input[title='Data da Autorização/Emissão PMI']").focus();
    }
    if (dt3 != "" && !isDate(dt3)) {
        $($("input[title='Previsão da Implantação']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado11' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
        retorno = false; $("input[title='Previsão da Implantação']").focus();
    }
    if (dt4 != "" && !isDate(dt4)) {
        $($("input[title='Aquisição de Materiais - Início']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado12' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
        retorno = false; $("input[title='Aquisição de Materiais - Início']").focus();
    }
    if (dt5 != "" && !isDate(dt5)) {
        $($("input[title='Aquisição de Materiais - Fim']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado13' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
        retorno = false; $("input[title='Aquisição de Materiais - Fim']").focus();
    }
    if (dt6 != "" && !isDate(dt6)) {
        $($("input[title='Obras Civis - Início']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado14' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
        retorno = false; $("input[title='Obras Civis - Início']").focus();
    }
    if (dt7 != "" && !isDate(dt7)) {
        $($("input[title='Obras Civis - Fim']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado15' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
        retorno = false; $("input[title='Obras Civis - Fim']").focus();
    }
    if (dt8 != "" && !isDate(dt8)) {
        $($("input[title='Montagem Eletromecânica - Início']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado16' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
        retorno = false; $("input[title='Montagem Eletromecânica - Início']").focus();
    }
    if (dt9 != "" && !isDate(dt9)) {
        $($("input[title='Montagem Eletromecânica - Fim']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado17' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
        retorno = false; $("input[title='Montagem Eletromecânica - Fim']").focus();
    }
    if (dt10 != "" && !isDate(dt10)) {
        $($("input[title='Comissionamento - Início']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado18' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
        retorno = false; $("input[title='Comissionamento - Início']").focus();
    }
    if (dt11 != "" && !isDate(dt11)) {
        $($("input[title='Comissionamento - Fim']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado19' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
        retorno = false; $("input[title='Comissionamento - Fim']").focus();
    }
    if (dt12 != "" && !isDate(dt12)) {
        $($("input[title='Operação Comercial']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado20' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
        retorno = false; $("input[title='Operação Comercial']").focus();
    }

    return retorno;
}

function getWQuerystring(ji) {
    hu = parent.location.search.substring(1).replace("[^a-zA-Z0-9/]", "-");
    gy = hu.split("&");
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

function isDate(txtDate) {
    try {
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
    catch (ex) { return true; }
}

function OpenDialogT(url, title) {
    var options = {
        url: url,
        width: 850,
        height: 600,
        title: title,
        dialogReturnValueCallback: Function.createDelegate(null, CloseDialogCallbackT)
    };
    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);
}

function CloseDialogCallbackT(dialogResult, returnValue) {
    if (dialogResult != SP.UI.DialogResult.cancel) {
        var query = _spPageContextInfo.webAbsoluteUrl + "/_vti_bin/listdata.svc/GestãoInterna?$top=1&$select=ID,Criado&$orderby=Criado desc";
        $.ajax({
            type: "GET",
            contentType: "application/json;charset=ISO-8859-1",
            url: encodeURI(query),
            cache: false,
            async: false,
            dataType: 'json',
            headers: { "Accept": "application/json; odata=verbose" },
            success: function (data) {
                if (data.d.length > 0) {
                    $("input[title='Número de Gestão']").val(data.d[0].ID);
                    var url = _spPageContextInfo.webAbsoluteUrl + "/Lists/GestaoInterna/DispForm.aspx?ID=" + data.d[0].ID;
                    $("#linkV").attr("onclick", "OpenDialogV('" + url + "','Adicionar Gestão Interna');return false;");
                    var urlE = _spPageContextInfo.webAbsoluteUrl + "/Lists/GestaoInterna/EditForm.aspx?ID=" + data.d[0].ID;
                    $("#linkE").attr("onclick", "OpenDialogV('" + urlE + "','Ediatr Gestão Interna');return false;");
                    $("#Criado").append(dateToString(dateConvert(data.d[0].Criado)));
                    $("#linha").show(); $("a[id^='idHomePageNewItem']").hide();
                }
            }
        });
    }
    //else
    //alert("cancela");
}

function OpenDialogV(url, title) {
    var options = {
        url: url,
        width: 850,
        height: 600,
        title: title,
        dialogReturnValueCallback: Function.createDelegate(null, CloseDialogCallback)
    };
    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);
}


function updateAddNewLinks() {
    $("a[id^='idHomePageNewItem']").each(function () {
        var url = location.href;
        this.href = "";
        this.onclick = "OpenDialogT('/Lists/GestaoInterna/NewForm.aspx','Adicionar Gestão Interna');return false;";
    });
}
//_spBodyOnLoadFunctionNames.push("updateAddNewLinks");

function CurrentUserMemberOfGroup(groupName, OnComplete) {

    var ctx = new SP.ClientContext.get_current();
    var currentWeb = ctx.get_web();
    var currentUser = ctx.get_web().get_currentUser();
    ctx.load(currentUser);
    var Groups = currentWeb.get_siteGroups();
    ctx.load(Groups);
    var group = Groups.getByName(groupName);
    ctx.load(group);
    var groupUsers = group.get_users();
    ctx.load(groupUsers);

    ctx.executeQueryAsync(
            function (sender, args) {
                var userInGroup = UserInGroup(currentUser, group);
                OnComplete(userInGroup);
            },
            function OnFailure(sender, args) {
                OnComplete(false);
            }
    );

    function UserInGroup(user, group) {
        var groupUsers = group.get_users();
        var userInGroup = false;
        var groupUserEnumerator = groupUsers.getEnumerator();
        while (groupUserEnumerator.moveNext()) {
            var groupUser = groupUserEnumerator.get_current();
            if (groupUser.get_id() == user.get_id()) {
                userInGroup = true;
                break;
            }
        }
        return userInGroup;
    }
}