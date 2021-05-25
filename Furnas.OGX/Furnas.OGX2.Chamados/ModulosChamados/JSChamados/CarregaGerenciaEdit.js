$(document).ready(function () {

    $("<tr><td nowrap='true' valign='top' width='113px' class='ms-formlabel'><h3 class='ms-standardheader'><nobr>Órgão<span class='ms-accentText' title='Este é um campo obrigatório.'> *</span></nobr></h3></td><td valign='top' width='350px' class='ms-formbody'><select id='org'><option value='0'>Selecione um Órgão</option></select></td></tr>").insertAfter($("input[title='Órgão Campo Obrigatório']").closest("tr"));

    $("input[title='Órgão Campo Obrigatório']").closest("tr").hide();

    GetOrgaos();

    $("select[id='org']").change(function () {
        //$(".ms-cui-tabContainer").attr('style', 'z-index:99!important'); ShowProgress();
        EmptyPeoplePicker("Responsáveis");
        FuncionariosOrgaos($("select[id='org']").val());
    });
    $("input[title='Gerente Imediato']").closest("tr").hide();
});

function PreSaveAction() {
    try {
        $('#ErroDataCustomizado1').remove(); $('#ErroDataCustomizado2').remove();
    }
    catch (ex) { }

    var retorno = true;
    if ($("select[id='org']").val() == "0") {
        $("select[id='org']").closest("td").append("<span id='ErroDataCustomizado1' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Selecione um Órgão.</span>");
        retorno = false;
    }

    var totalResp = 0;
    SP.SOD.executeFunc('sp.js', 'SP.ClientContext', function () {
        var peoplePickerCtrl = $('nobr:contains("Responsáveis")').closest('tr').find('div.sp-peoplepicker-topLevel')
        var peoplePickerCtrlId = $(peoplePickerCtrl).attr('id');
        var peoplePicker = SPClientPeoplePicker.SPClientPeoplePickerDict[peoplePickerCtrlId];
        totalResp = peoplePicker.TotalUserCount;
    });

    if (totalResp == 0) {
        $('nobr:contains("Responsáveis")').closest('tr').find('div.sp-peoplepicker-topLevel').closest("td").append("<span id='ErroDataCustomizado2' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Esse campo não pode ficar em branco.</span>");
        retorno = false;
    }

    if (!retorno)
        return false;
    else {
        var urlQuery = _spPageContextInfo.webServerRelativeUrl + "/_vti_bin/listdata.svc/GerênciaResponsável?$filter=Órgão eq '" + $("select[id='org'] option:selected").text() + "'";
        var retornoHTML = [];
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
                    if (data.d.results[0].ID != getWQuerystring("ID")) {
                        $('nobr:contains("Responsáveis")').closest('table').append("<tr><td width='116px'></td><td width='350px'><span id='ErroDataCustomizado3' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Este valor já existe na lista.</span></td></tr>");
                        retorno = false;
                    }
                }
            }
        });
    }

    if (!retorno)
        return false;

    $("input[title='Órgão Campo Obrigatório']").val($("select[id='org'] option:selected").text());

    /*url = "http://corp3025/colaboradordetalhe/" + mat + "/p";
    var itemColaborador = obterColaboradorREST(url);

    $("input[title='Gerente Imediato']").val(itemColaborador.orgao.responsavelOrgao.emailresponsavel + ";" + itemColaborador.orgao.responsavelOrgao.emailsubstituto)*/
    return true;

}


/* SPRINT 7 */
/* 28880 */
//Ordena os orgãos por ordem alfabética
function ordenarOrgaos(a, b) {
    var comparacao = 0;

    if (a.orgao > b.orgao)
        comparacao = 1;

    else if (a.orgao < b.orgao)
        comparacao = -1;

    return comparacao;
}

/* SPRINT 7 */
/* 28880 */
//Ordena os usuários por ordem alfabética
function ordenarUsuarios(a, b) {
    var comparacao = 0;

    if (a.nome > b.nome)
        comparacao = 1;

    else if (a.nome < b.nome)
        comparacao = -1;

    return comparacao;
}



var mat;

function FuncionariosOrgaos(orgao) {

    url = "http://corp3025/funcionariosorgao/" + orgao + "/p";
    var data = [];
    $.ajax({
        url: url,
        crossDomain: true,
        async: false,
        dataType: "json",
        success: function (res) {
            /*$.each(res, function (i, item) {
				var users = GetUserIdFromUserName("fr16995");
				
			});*/
            res.sort(ordenarUsuarios);
            data = res;
        },
        error: function (error) { console.log(error); }
    });

    var usuario = [];
    var usuarioNome = [];

    var usuarios = [];

    /*$.each(data, function (i, item) {
      

        var login = [];
        if (item.cargo.includes("PRESTADOR")){
            login = "fc" + item.mat;
            console.log("Dentro "+login);
            console.log("Dentro " + item.nome);
            
        }
        else {
            if (item.mat.charAt(0) == "0"){
                login = "fr" + item.mat.substr(1);
                console.log("Dentro " + login);
                console.log("Dentro " + item.nome);
               
            }

            else
            {
                login = "fr" + item.mat;
                console.log("Dentro " + login);
                console.log("Dentro " + item.nome);
                
            }
        }*/

    for (i = 0; i < data.length; i++) {

        var login = [];
        if (data[i].cargo.includes("PRESTADOR")) {
            login = "fc" + (data[i].mat);
            console.log("Fora " + login);
            console.log("Fora " + (data[i].nome));

        }
        else {
            if ((data[i].mat.charAt(0)) == "0") {
                login = "fr" + (data[i].mat.substr(1));
                console.log("Fora " + login);
                console.log("Fora " + (data[i].nome));

            }

            else {
                login = "fr" + (data[i].mat);
                console.log("Fora " + login);
                console.log("Fora " + data[i].nome);

            }
        }


        //ensureUser(login).done(function (data) {
        ensureUser(login).done(function (data) {


            SP.SOD.executeFunc('sp.js', 'SP.ClientContext', function () {
                var peoplePickerCtrl = $('nobr:contains("Responsáveis")').closest('tr').find('div.sp-peoplepicker-topLevel')
                var peoplePickerCtrlId = $(peoplePickerCtrl).attr('id');
                var peoplePicker = SPClientPeoplePicker.SPClientPeoplePickerDict[peoplePickerCtrlId];
                var userObj = { 'Key': data.d.LoginName };

                //var userObj = { 'Key': usuario[i] };
                mat = data.d.LoginName.split('|')[1].replace(/[^0-9]/gi, '');
                //mat = usuario[i];
                //ordenar userObj
                //peoplePicker.AddUnresolvedUser(userObj, true);

                usuarios.push(userObj);
                /*if (r == i) {
                    $(".modal").hide(); $(".loading").hide();
                }*/

            });
            //$(".loading").hide(); $(".modal").hide();
        })
            .fail(function (error) {
                console.log('An error occured while adding user');
                //$(".loading").hide(); $(".modal").hide();
            });
    }



    //ensureUser(login).done(function (data) {
    var usuariosOrdenados = usuarios.sort(ordenarUsuarios);

    for (i = 0; i < usuariosOrdenados.length; i++) {

        SP.SOD.executeFunc('sp.js', 'SP.ClientContext', function () {
            var peoplePickerCtrl = $('nobr:contains("Responsáveis")').closest('tr').find('div.sp-peoplepicker-topLevel')
            var peoplePickerCtrlId = $(peoplePickerCtrl).attr('id');
            var peoplePicker = SPClientPeoplePicker.SPClientPeoplePickerDict[peoplePickerCtrlId];
            //var userObj = { 'Key': data.d.LoginName };

            //var userObj = { 'Key': usuario[i] };
            //mat = data.d.LoginName.split('|')[1].replace(/[^0-9]/gi, '');
            //mat = usuario[i];
            //ordenar userObj
            peoplePicker.AddUnresolvedUser(usuariosOrdenados[i], true);
            /*if (r == i) {
                $(".modal").hide(); $(".loading").hide();
            }*/

            //});
            //$(".loading").hide(); $(".modal").hide();
        });

    }
    /*.fail(function (error) {
        console.log('An error occured while adding user');
        //$(".loading").hide(); $(".modal").hide();
    });*/

    //});
}


function ensureUser(loginName) {
    var payload = { 'logonName': loginName };
    return $.ajax({
        url: _spPageContextInfo.webAbsoluteUrl + "/_api/web/ensureuser",
        type: "POST",
        contentType: "application/json;odata=verbose",
        data: JSON.stringify(payload),
        headers: {
            "X-RequestDigest": $("#__REQUESTDIGEST").val(),
            "accept": "application/json;odata=verbose"
        }
    });
}

function GetOrgaos() {
    url = "http://corp3025/listaorgaos/p";
    var data;
    $.ajax({
        url: url,
        crossDomain: true,
        async: false,
        dataType: "json",
        success: function (res) {
            res.sort(ordenarOrgaos);
            data = res;
        },
        error: function (error) { console.log(error); }
    });

    var orgaoedit = $("input[title='Órgão Campo Obrigatório']").val();
    $.each(data, function (i, item) {
        if (item.orgao == orgaoedit)
            $("select[id='org']").append('<option value="' + item.sigla + '" selected>' + item.orgao + '</option>');
        else
            $("select[id='org']").append('<option value="' + item.sigla + '">' + item.orgao + '</option>');
    });

}

function EmptyPeoplePicker(peoplePickerId) {

    SP.SOD.executeFunc('sp.js', 'SP.ClientContext', function () {
        var peoplePickerCtrl = $('nobr:contains("Responsáveis")').closest('tr').find('div.sp-peoplepicker-topLevel')
        var peoplePickerCtrlId = $(peoplePickerCtrl).attr('id');
        var peoplePicker = SPClientPeoplePicker.SPClientPeoplePickerDict[peoplePickerCtrlId];
        var usersobject = peoplePicker.GetAllUserInfo();
        usersobject.forEach(function (index) {
            peoplePicker.DeleteProcessedUser(usersobject[index]);
        });
    });
}

function ShowProgress() {
    setTimeout(function () {
        var modal = $('<div />');
        modal.addClass("modal");
        $('body').append(modal);
        var loading = $(".loading");
        loading.show();
        var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
        var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
        loading.css({ top: top, left: left });
    }, 200);
}

function getWQuerystring(ji) {
    hu = location.search.substring(1).replace("[^a-zA-Z0-9/]", "-");
    gy = hu.split("&");
    for (i = 0; i < gy.length; i++) {
        ft = gy[i].split("=");
        if (ft[0] == ji) {
            return ft[1];
        }
    }
}