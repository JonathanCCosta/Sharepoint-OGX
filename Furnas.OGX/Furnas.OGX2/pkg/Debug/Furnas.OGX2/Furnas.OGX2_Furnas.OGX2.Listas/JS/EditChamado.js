$(document).ready(function () {
    getLoginFromPeoplePicker();
    ConfiguraCampos();

});

function ConfiguraCampos() {
    //SP.SOD.executeOrDelayUntilScriptLoaded(getLoginFromPeoplePicker, 'clientpeoplepicker.js');
    //SP.SOD.executeOrDelayUntilScriptLoaded(LoadPeoplePicker, 'clientpeoplepicker.js');
    $("input[title='Imediato do Responsável']").closest("tr").hide();
    $("input[title='Número do chamado Campo Obrigatório']").attr('disabled', 'disabled');
    $("input[title='Data da solicitação']").attr('disabled', 'disabled');
    $("input[title='Data da solicitação']").closest("tr").find("td:eq(1)").hide();
    $("textarea[title='Órgão']").attr('disabled', 'disabled');
    $("input[title='Data de encerramento']").attr('disabled', 'disabled');
    $("input[title='Data de encerramento']").closest("tr").find("td:eq(1)").hide();

    //if(!isMember("Administradores GGA")){
    /*if(resp){
		SP.SOD.executeOrDelayUntilScriptLoaded(LoadPeoplePickerSolicita, 'clientpeoplepicker.js');
		$("select[title='Controle de status Campo Obrigatório']").attr('disabled', 'disabled');
		$("input[title='Prazo de Atendimento Campo Obrigatório']").attr('disabled', 'disabled');
		$("select[title='Complexidade Campo Obrigatório']").attr('disabled', 'disabled');
		$("input[title='Data de encerramento']").attr('disabled', 'disabled');
		$("input[title='Data de encerramento']").closest("tr").find("td:eq(1)").hide();
		$("input[title='Assunto']").attr('disabled', 'disabled');
		$("textarea[title='Descrição do chamado Campo Obrigatório']").css('width','500px').attr('disabled', 'disabled');
	}
	else if(resp){
		$("input[title='Assunto']").attr('disabled', 'disabled');
		$("textarea[title='Descrição do chamado Campo Obrigatório']").css('width','500px').attr('disabled', 'disabled');
	}
	else{	
		SP.SOD.executeOrDelayUntilScriptLoaded(LoadPeoplePicker, 'clientpeoplepicker.js');
		$("select[title='Controle de status Campo Obrigatório']").attr('disabled', 'disabled');
		$("input[title='Prazo de Atendimento Campo Obrigatório']").attr('disabled', 'disabled');
		$("select[title='Complexidade Campo Obrigatório']").attr('disabled', 'disabled');
		$("input[title='Data de encerramento']").attr('disabled', 'disabled');
		$("input[title='Data de encerramento']").closest("tr").find("td:eq(1)").hide();
		$("select[title='Enviar email ao responsável Campo Obrigatório']").attr('disabled', 'disabled');
		
	}*/
}

function getLoginFromPeoplePicker() {
    //Get the people picker field
    //var ppDiv = $("div[title='Responsável']")[0];
    //cast the object as type PeoplePicker
    SP.SOD.executeFunc('sp.js', 'SP.ClientContext', function () {

        //var peoplePicker = SPClientPeoplePicker.SPClientPeoplePickerDict[ppDiv.id]; 
        //Get list of users from field (assuming 1 in this case)
        //var userList = peoplePicker.GetAllUserInfo();
        //var userInfo = userList[0];

        //if(userInfo != null)
        //{
        var currentUserRequest = $.ajax({
            url: _spPageContextInfo.webAbsoluteUrl + '/_api/web/currentuser',
            method: 'GET',
            headers: {
                "accept": "application/json;odata=verbose",
                "content-type": "application/json;odata=verbose"
            }
        });
        currentUserRequest.done(function (result) {
            var ppDiv = $("div[title='Responsável']")[0];
            var peoplePicker = SPClientPeoplePicker.SPClientPeoplePickerDict[ppDiv.id];
            //Get list of users from field (assuming 1 in this case)
            var userList = peoplePicker.GetAllUserInfo();
            var userInfo = userList[0];
            if (userInfo != null) {
                if (userInfo.Key == result.d.LoginName) {
                    $("input[title='Assunto']").attr('disabled', 'disabled');
                    $("textarea[title='Descrição do chamado Campo Obrigatório']").css('width', '500px').attr('disabled', 'disabled');
                    $("select[title='Enviar email ao responsável']").attr('disabled', 'disabled');
                    SP.SOD.executeOrDelayUntilScriptLoaded(LoadPeoplePicker, 'clientpeoplepicker.js');
                }
                else {
                    if (isMember("Administradores GGA")) {
                        SP.SOD.executeOrDelayUntilScriptLoaded(LoadPeoplePickerSolicita, 'clientpeoplepicker.js');
                        $("select[title='Controle de status Campo Obrigatório']").attr('disabled', 'disabled');
                        $("input[title='Prazo de Atendimento Campo Obrigatório']").attr('disabled', 'disabled');
                        $("select[title='Complexidade Campo Obrigatório']").attr('disabled', 'disabled');
                        //$("input[title='Data de encerramento']").attr('disabled', 'disabled');
                        //$("input[title='Data de encerramento']").closest("tr").find("td:eq(1)").hide();
                        $("input[title='Assunto']").attr('disabled', 'disabled');
                        $("textarea[title='Descrição do chamado Campo Obrigatório']").css('width', '500px').attr('disabled', 'disabled');
                    }
                    else {
                        SP.SOD.executeOrDelayUntilScriptLoaded(LoadPeoplePicker, 'clientpeoplepicker.js');
                        $("select[title='Controle de status Campo Obrigatório']").attr('disabled', 'disabled');
                        $("input[title='Prazo de Atendimento Campo Obrigatório']").attr('disabled', 'disabled');
                        $("select[title='Complexidade Campo Obrigatório']").attr('disabled', 'disabled');
                        $("input[title='Data de encerramento']").attr('disabled', 'disabled');
                        $("input[title='Data de encerramento']").closest("tr").find("td:eq(1)").hide();
                        $("select[title='Enviar email ao responsável']").attr('disabled', 'disabled');
                    }
                }
            }
            else {
                if (isMember("Administradores GGA")) {
                    SP.SOD.executeOrDelayUntilScriptLoaded(LoadPeoplePickerSolicita, 'clientpeoplepicker.js');
                    $("select[title='Controle de status Campo Obrigatório']").attr('disabled', 'disabled');
                    $("input[title='Prazo de Atendimento Campo Obrigatório']").attr('disabled', 'disabled');
                    $("select[title='Complexidade Campo Obrigatório']").attr('disabled', 'disabled');
                    $("input[title='Data de encerramento']").attr('disabled', 'disabled');
                    $("input[title='Data de encerramento']").closest("tr").find("td:eq(1)").hide();
                    $("input[title='Assunto']").attr('disabled', 'disabled');
                    $("textarea[title='Descrição do chamado Campo Obrigatório']").css('width', '500px').attr('disabled', 'disabled');
                }
                else {
                    SP.SOD.executeOrDelayUntilScriptLoaded(LoadPeoplePicker, 'clientpeoplepicker.js');
                    $("select[title='Controle de status Campo Obrigatório']").attr('disabled', 'disabled');
                    $("input[title='Prazo de Atendimento Campo Obrigatório']").attr('disabled', 'disabled');
                    $("select[title='Complexidade Campo Obrigatório']").attr('disabled', 'disabled');
                    //$("input[title='Data de encerramento']").attr('disabled', 'disabled');
                    //$("input[title='Data de encerramento']").closest("tr").find("td:eq(1)").hide();
                    $("select[title='Enviar email ao responsável']").attr('disabled', 'disabled');
                }
            }
        });
        /*}
	    else{
	    	if(isMember("Administradores GGA")){
				SP.SOD.executeOrDelayUntilScriptLoaded(LoadPeoplePickerSolicita, 'clientpeoplepicker.js');
				$("select[title='Controle de status Campo Obrigatório']").attr('disabled', 'disabled');
				$("input[title='Prazo de Atendimento Campo Obrigatório']").attr('disabled', 'disabled');
				$("select[title='Complexidade Campo Obrigatório']").attr('disabled', 'disabled');
				$("input[title='Data de encerramento']").attr('disabled', 'disabled');
				$("input[title='Data de encerramento']").closest("tr").find("td:eq(1)").hide();
				$("input[title='Assunto']").attr('disabled', 'disabled');
				$("textarea[title='Descrição do chamado Campo Obrigatório']").css('width','500px').attr('disabled', 'disabled');
			}
			else{
				SP.SOD.executeOrDelayUntilScriptLoaded(LoadPeoplePicker, 'clientpeoplepicker.js');
				$("select[title='Controle de status Campo Obrigatório']").attr('disabled', 'disabled');
				$("input[title='Prazo de Atendimento Campo Obrigatório']").attr('disabled', 'disabled');
				$("select[title='Complexidade Campo Obrigatório']").attr('disabled', 'disabled');
				//$("input[title='Data de encerramento']").attr('disabled', 'disabled');
				//$("input[title='Data de encerramento']").closest("tr").find("td:eq(1)").hide();
				$("select[title='Enviar email ao responsável']").attr('disabled', 'disabled');
			}
	    }*/
    });
}

function PreSaveAction() {

    /*var dtNS = $("input[title='Data de encerramento']").val();
	if(dtNS != "" && !isDate(dtNS)){
		$($("input[title='Data de encerramento']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
		retorno = false;
	}*/

    try {
        $('#ErroDataCustomizado').remove();
    }
    catch (ex) { }

    if (isMember("Administradores GGA")) {
        if ($('nobr:contains("Responsável")').closest('tr').find('div.sp-peoplepicker-topLevel')[0].innerText == "Digite um nome ou email...") {
            $('nobr:contains("Responsável")').closest("tr").find("td:eq(1)").append("<span id='ErroDataCustomizado' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
            return false;
        }
        else if ($("select[title='Controle de status Campo Obrigatório']").val() == "Encerrado" && $("input[title='Data de encerramento']").val() == "") {
            var d = new Date();
            $("input[title='Data de encerramento']").val((d.getDate() < 10 ? '0' : '') + d.getDate() + '/' +
				    ((d.getMonth() + 1) < 10 ? '0' : '') + (d.getMonth() + 1) + '/' +
				      d.getFullYear());
        }
    }
    else {
        if ($("select[title='Controle de status Campo Obrigatório']").val() == "Encerrado" && $("input[title='Data de encerramento']").val() == "") {
            var d = new Date();
            $("input[title='Data de encerramento']").val((d.getDate() < 10 ? '0' : '') + d.getDate() + '/' +
				    ((d.getMonth() + 1) < 10 ? '0' : '') + (d.getMonth() + 1) + '/' +
				      d.getFullYear());
        }
    }
    return true;
}


function GetOrgao(mat) {
    url = "http://corp3025/colaboradordetalhe/" + mat + "/p";
    var itemColaborador = obterColaboradorREST(url);
    $("textarea[title='Órgão']").val(itemColaborador.orgao.orgao);
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

function LoadPeoplePickerSolicita() {
    SP.SOD.executeFunc('sp.js', 'SP.ClientContext', function () {
        var peoplePickerCtrl = $('nobr:contains("Nome do solicitante")').closest('tr').find('div.sp-peoplepicker-topLevel');
        var peoplePickerCtrlId = $(peoplePickerCtrl).attr('id');
        var peoplePicker = SPClientPeoplePicker.SPClientPeoplePickerDict[peoplePickerCtrlId];
        peoplePicker.SetEnabledState(false);
        $('nobr:contains("Nome do solicitante")').closest("tr").find("td:eq(1)").find('.sp-peoplepicker-delImage').hide();
    });
}


function LoadPeoplePicker() {
    SP.SOD.executeFunc('sp.js', 'SP.ClientContext', function () {
        var peoplePickerCtrl = $('nobr:contains("Nome do solicitante")').closest('tr').find('div.sp-peoplepicker-topLevel');
        var peoplePickerCtrlId = $(peoplePickerCtrl).attr('id');
        var peoplePicker = SPClientPeoplePicker.SPClientPeoplePickerDict[peoplePickerCtrlId];
        peoplePicker.SetEnabledState(false);
        $('nobr:contains("Nome do solicitante")').closest("tr").find("td:eq(1)").find('.sp-peoplepicker-delImage').hide();

        var peoplePickerCtrlR = $('nobr:contains("Responsável")').closest('tr').find('div.sp-peoplepicker-topLevel')
        var peoplePickerCtrlIdR = $(peoplePickerCtrlR).attr('id');
        var peoplePickerR = SPClientPeoplePicker.SPClientPeoplePickerDict[peoplePickerCtrlIdR];
        peoplePickerR.SetEnabledState(false);
        //var idEdit = peoplePickerR.EditorElementId
        //$("input[id='"+idEdit+"']").prop("title","");
        $('nobr:contains("Responsável")').closest("tr").find("td:eq(1)").find('.sp-peoplepicker-delImage').hide();
    });
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