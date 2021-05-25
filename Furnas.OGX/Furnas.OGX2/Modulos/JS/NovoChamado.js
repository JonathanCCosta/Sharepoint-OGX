$(document).ready(function () {

    ConfiguraCampos();

});

function ConfiguraCampos() {

    SP.SOD.executeOrDelayUntilScriptLoaded(loadCurrentUser, 'clientpeoplepicker.js');
    $("input[title='Imediato do Responsável']").closest("tr").hide();
    $("select[title='Controle de status Campo Obrigatório']").attr('disabled', 'disabled');
    $("input[title='Número do chamado Campo Obrigatório']").closest("tr").hide();
    $("input[title='Data da solicitação']").attr('disabled', 'disabled');
    $("input[title='Data da solicitação']").closest("tr").find("td:eq(1)").hide();
    $("input[title='Data de encerramento']").attr('disabled', 'disabled');
    $("input[title='Data de encerramento']").closest("tr").find("td:eq(1)").hide();
    $("textarea[title='Órgão']").attr('disabled', 'disabled');
    $("textarea[title='Descrição do chamado Campo Obrigatório']").css('width', '500px');

    if (isMember("Administradores GGA")) {
        $("input[title='Prazo de Atendimento Campo Obrigatório']").attr('disabled', 'disabled');
        $("select[title='Complexidade Campo Obrigatório']").attr('disabled', 'disabled');
        //$("#DataEncerramento").closest("tr").hide();//.attr('disabled', 'disabled');
    }
    else {
        $("select[title='Complexidade Campo Obrigatório']").closest("tr").hide();//.attr('disabled', 'disabled');
        //$("#DataEncerramento").closest("tr").hide();//.attr('disabled', 'disabled');
        $("input[title='Prazo de Atendimento Campo Obrigatório']").attr('disabled', 'disabled');
        $("select[title='Enviar email ao responsável']").closest("tr").hide();
        $('div[title="Responsável"]').closest("tr").hide();
    }
}

function GetOrgao(mat) {
    url = "http://corp3025/colaboradordetalhe/" + mat + "/p";
    var itemColaborador = obterColaboradorREST(url);
    $("textarea[title='Órgão']").val(itemColaborador.orgao.orgao);
    $("input[title='Imediato do Responsável']").val(itemColaborador.orgao.responsavelOrgao.emailresponsavel + ";" + itemColaborador.orgao.responsavelOrgao.emailsubstituto);
}

function obterColaboradorREST(url) {
    var data;

    $.ajax({
        url: url,
        crossDomain: true,
        async: false,
        dataType: "json",
        success: function (res) {
            data = res;
        },
        error: function (error) { console.log(error); }
    });
    return data;
}


function PreSaveAction() {
    $("input[title='Número do chamado Campo Obrigatório']").val(GetLastChamado());
    return true;
}

function GetLastChamado() {
    var ID = 0;
    var query = _spPageContextInfo.webAbsoluteUrl + "/_vti_bin/listdata.svc/Chamados?$top=1&$select=ID&$orderby=Criado desc";
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
                ID = data.d[0].ID;
            }
        }
    });
    if (ID > 0) {
        var seq = parseInt(ID) + 1;
        var seqFormat = seq.toString();

        if (seqFormat.length == 1)
            seqFormat = "000" + seqFormat;
        else if (seqFormat.length == 2)
            seqFormat = "00" + seqFormat;
        else if (seqFormat.length == 3)
            seqFormat = "0" + seqFormat;

        return "GGA.O.9" + seqFormat + "." + new Date().getFullYear();
    }
    else
        return "GGA.O.90001." + new Date().getFullYear();
}


function loadCurrentUser() {
    SP.SOD.executeFunc('sp.js', 'SP.ClientContext', function () {
        var currentUserRequest = $.ajax({
            url: _spPageContextInfo.webAbsoluteUrl + '/_api/web/currentuser',
            method: 'GET',
            headers: {
                "accept": "application/json;odata=verbose",
                "content-type": "application/json;odata=verbose"
            }
        });
        currentUserRequest.done(function (result) {
            var peoplePickerCtrl = $('nobr:contains("Nome do solicitante")').closest('tr').find('div.sp-peoplepicker-topLevel')
            var peoplePickerCtrlId = $(peoplePickerCtrl).attr('id');
            var peoplePicker = SPClientPeoplePicker.SPClientPeoplePickerDict[peoplePickerCtrlId];
            var userObj = { 'Key': result.d.LoginName };
            peoplePicker.AddUnresolvedUser(userObj, true);
            peoplePicker.SetEnabledState(false);
            $('.sp-peoplepicker-resolveList').find('.sp-peoplepicker-delImage').hide();

            GetOrgao(result.d.LoginName.split('|')[1].replace(/[^0-9]/gi, ''));
        });
    });
}

function isMember(groupName) {
    var flag = true;
    $.ajax({
        url: _spPageContextInfo.webAbsoluteUrl + "/_api/web/sitegroups/getByName('" + groupName + "')/Users?$filter=Id eq " + _spPageContextInfo.userId,
        method: "GET",
        headers: { "Accept": "application/json; odata=verbose" },
        success: function (data) {
            if (data.d.results[0] == undefined) {
                flag = false;
            }
        }
    });
    return flag;
}