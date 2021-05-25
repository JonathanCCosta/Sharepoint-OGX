var E_Admin = false;
var ESolicitante = false; var E_resp = false;
$(document).ready(function () {
    $(".ms-cui-tabContainer").attr('style', 'z-index:99!important');
    ShowProgress();

    $("input[title='Imediato do Responsável']").closest("tr").hide();
    $("input[title='Número do chamado Campo Obrigatório']").attr('disabled', 'disabled');
    $("textarea[title='Órgão']").attr('disabled', 'disabled');
    $("select[title='Tipo de Chamado Campo Obrigatório']").attr('disabled', 'disabled');
    $("select[title='Modelo de Chamado Campo Obrigatório']").attr('disabled', 'disabled');
    $('td.ms-formlabel:contains("Descrição")').closest('td').next('td')[0].width = '800px';
    $('nobr:contains("Gerência Responsável")').after('<span class="ms-accentText" id="GerenteResponsavel" title="Este é um campo obrigatório."> *</span>');
    //Pra solicitante
    $("input[title='Prazo de Atendimento Campo Obrigatório']").closest("tr").hide();
    $('nobr:contains("Responsável")').closest('tr').hide();
    //$("select[title='Enviar email ao responsável']").closest("tr").hide();
    $("select[title='Complexidade']").closest("tr").hide();
    $("select[title='Importância']").closest("tr").hide();
    //$('nobr:contains("encerramento")').closest("tr").hide();
    //$("select[title='Gerência Responsável']").closest("tr").hide();

    if ($("select[title='Controle de status Campo Obrigatório']").val() == "Encaminhado à outra gerência da DO") {
        $("#Esclarecimentos nobr").text("Esclarecimento Solicitante");
        $("#EsclarecimentosGerencia nobr").text("Esclarecimento Outra Gerência");
        $("textarea[title='Esclarecimentos']").closest("tr").show();
        $("textarea[title='Esclarecimentos Gerência']").closest("tr").show();

    }

    if ($("select[title='Controle de status Campo Obrigatório']").val() == "Em andamento") {
        $("#Esclarecimentos nobr").text("Esclarecimento Solicitante");
        $("#EsclarecimentosGerencia nobr").text("Esclarecimento Outra Gerência");
        $("textarea[title='Esclarecimentos']").closest("tr").show();
        $("textarea[title='Esclarecimentos Gerência']").closest("tr").show();

    }




    CurrentUserMemberOfGroup("Administradores GGA", function (isCurrentUserInGroup) {
        if (isCurrentUserInGroup) {
            E_Admin = true;
        }
    });

    SP.SOD.executeFunc("clientpeoplepicker.js", "SP.ClientContext", function () {
        setTimeout(function () {
            ConfiguraCampos();
            //LoadPeoplePickerResp();
        }, 2000);
    });

    $("select[title='Controle de status Campo Obrigatório']").change(function () {


        if ($("select[title='Controle de status Campo Obrigatório']").val() == "Aguardando informações") {
            if ($("textarea[title='Esclarecimentos']").closest("span").next("div").text() == "Não há entradas existentes.") {

                /* SPRINT 7 */
                /* 28880 */
                $("#Esclarecimentos nobr").text("Esclarecimento Solicitante");
                $("#EsclarecimentosGerencia nobr").text("Esclarecimento Outra Gerência");
                $("textarea[title='Esclarecimentos']").closest("tr").show();
            }
            else {
                $("textarea[title='Esclarecimentos']").removeAttr('disabled');
                $("#Esclarecimentos nobr").text("Esclarecimento Solicitante");
                $("#EsclarecimentosGerencia nobr").text("Esclarecimento Outra Gerência");


            }

            if ($("textarea[title='Esclarecimentos Gerência']").closest("span").next("div").text() == "Não há entradas existentes.") {
                $("textarea[title='Esclarecimentos Gerência']").closest("tr").hide();
                $("select[title='Gerência Responsável']").closest("tr").hide();
            }
            else {
                $("#Esclarecimentos nobr").text("Esclarecimento Solicitante");
                $("#EsclarecimentosGerencia nobr").text("Esclarecimento Outra Gerência");
                $("select[title='Gerência Responsável']").closest("tr").hide();
            }
        }
        else if ($("select[title='Controle de status Campo Obrigatório']").val() == "Encaminhado à outra gerência da DO") {
            if ($("textarea[title='Esclarecimentos Gerência']").closest("span").next("div").text() == "Não há entradas existentes.") {
                $("#Esclarecimentos nobr").text("Esclarecimento Solicitante");
                $("#EsclarecimentosGerencia nobr").text("Esclarecimento Outra Gerência");

                $("textarea[title='Esclarecimentos Gerência']").closest("tr").show();
                $("select[title='Gerência Responsável']").closest("tr").show();
            }
            else {
                $("#Esclarecimentos nobr").text("Esclarecimento Solicitante");
                $("#EsclarecimentosGerencia nobr").text("Esclarecimento Outra Gerência");

                $("textarea[title='Esclarecimentos Gerência']").removeAttr('disabled');
                $("select[title='Gerência Responsável']").closest("tr").show();
            }

            if ($("textarea[title='Esclarecimentos']").closest("span").next("div").text() == "Não há entradas existentes.")
                $("textarea[title='Esclarecimentos']").closest("tr").hide();
            else {
                $("#Esclarecimentos nobr").text("Esclarecimento Solicitante");
                $("#EsclarecimentosGerencia nobr").text("Esclarecimento Outra Gerência");

                $("textarea[title='Esclarecimentos']").attr('disabled', 'disabled');//.removeAttr('disabled');
            }

        }
        else {
            if ($("textarea[title='Esclarecimentos']").closest("span").next("div").text() == "Não há entradas existentes.")
                $("textarea[title='Esclarecimentos']").closest("tr").hide();
            else {
                $("textarea[title='Esclarecimentos']").attr('disabled', 'disabled');
                $("#Esclarecimentos nobr").text("Esclarecimento Solicitante");
                $("#EsclarecimentosGerencia nobr").text("Esclarecimento Outra Gerência");

            }

            if ($("textarea[title='Esclarecimentos Gerência']").closest("span").next("div").text() == "Não há entradas existentes.") {
                $("textarea[title='Esclarecimentos Gerência']").closest("tr").hide();
                $("select[title='Gerência Responsável']").closest("tr").hide();
            }
            else {
                $("#Esclarecimentos nobr").text("Esclarecimento Solicitante");
                $("#EsclarecimentosGerencia nobr").text("Esclarecimento Outra Gerência");
                $("select[title='Gerência Responsável']").closest("tr").hide();
            }

        }
    });

    //if($("select[title='Gerência Responsável']").val() == "0"){
    $("select[title='Gerência Responsável'] option:contains('(Nenhum)')").text("Selecione uma opção");

    if ($("select[title='Controle de status Campo Obrigatório'] option:selected").text() === "Aguardando informações"
        /*&& $("select[title='Controle de status Campo Obrigatório'] option:selected").text() === "Encaminhado à outra gerência"*/) {
        $("#Esclarecimentos nobr").text("Esclarecimento Solicitante");
        $("#EsclarecimentosGerencia nobr").text("Esclarecimento Outra Gerência");

    }
    //}
});


function ConfiguraCampos() {
    /*$("input[title='Imediato do Responsável']").closest("tr").hide();
    $("input[title='Número do chamado Campo Obrigatório']").attr('disabled', 'disabled');
    $("textarea[title='Órgão']").attr('disabled', 'disabled');
    $("select[title='Tipo de Chamado Campo Obrigatório']").attr('disabled', 'disabled');
    $("select[title='Modelo de Chamado Campo Obrigatório']").attr('disabled', 'disabled');
	$('td.ms-formlabel:contains("Descrição")').closest('td').next('td')[0].width = '600px';*/

    try {
        Roda();
        $(".loading").hide(); $(".modal").hide();
    }
    catch (ex) { }
    $(".loading").hide(); $(".modal").hide();
}

function LoadPeoplePickerResp() {
    var ppDiv = $("div[title='Responsável']")[0];
    var peoplePicker = SPClientPeoplePicker.SPClientPeoplePickerDict[ppDiv.id];
    var userList = peoplePicker.GetAllUserInfo();
    var userInfo = userList[0];
    $(".loading").hide(); $(".modal").hide();
}

function Roda() {
    var UserIDCurrent = _spPageContextInfo.userId;
    var UserIdChamado;
    //var E_Admin = false;
    //var ESolicitante = false;

    try {
        var query = _spPageContextInfo.webAbsoluteUrl + "/_vti_bin/listdata.svc/Chamados?$filter=ID eq " + getQueryString("ID");
        $.ajax({
            type: "GET",
            contentType: "application/json;charset=ISO-8859-1",
            url: encodeURI(query),
            cache: false,
            async: false,
            dataType: 'json',
            headers: { "Accept": "application/json; odata=verbose" },
            success: function (dataC) {
                if (dataC.d.results.length > 0) {
                    if (dataC.d.results[0].NomeDoSolicitanteId == UserIDCurrent) {
                        ESolicitante = true; UserIdChamado = dataC.d.results[0].NomeDoSolicitanteId
                    }
                }
            }
        });
    } catch (ec) { }


    if (document.getElementById("idAttachmentsTable").rows.length > 0) {
        $("[aria-label^='Excluir']").closest("td").remove();
    }
    //$("[id='Ribbon.ListForm.Edit.Actions.AttachFile-Large']").hide();

    if (E_Admin) {
        //sou admin e não sou solicitante
        if (ESolicitante == false) {
            $("input[title='Assunto']").attr('disabled', 'disabled');
            $(".ms-rtefield div[role='textbox']").attr("contenteditable", "false");
            $(".ms-rtefield div[role='textbox']").removeAttr('class'); $(".ms-rtefield div[role='textbox']").css("background-color", "rgb(253, 253, 253)"); $(".ms-rtefield div[role='textbox']").css("color", "rgb(177, 177, 177)");
            if (document.getElementById("idAttachmentsTable").rows.length > 0) {
                $("[aria-label^='Delete']").closest("td").remove();
                //$("[id='Ribbon.ListForm.Edit.Actions.AttachFile-Large']").hide();
            }
        }

        $("input[title='Prazo de Atendimento Campo Obrigatório']").closest("tr").show();
        $('nobr:contains("Responsável")').closest('tr').show();
        $("select[title='Complexidade']").closest("tr").show();
        $("select[title='Importância']").closest("tr").show();
        $('nobr:contains("encerramento")').closest("tr").show();
        //$("select[title='Enviar email ao responsável']").closest("tr").show();

        if ($("select[title='Controle de status Campo Obrigatório']").val() != "Aberto") {
            $("input[title='Assunto']").attr('disabled', 'disabled');
            $(".ms-rtefield div[role='textbox']").attr("contenteditable", "false");
            $(".ms-rtefield div[role='textbox']").removeAttr('class'); $(".ms-rtefield div[role='textbox']").css("background-color", "rgb(253, 253, 253)"); $(".ms-rtefield div[role='textbox']").css("color", "rgb(177, 177, 177)");
            if (document.getElementById("idAttachmentsTable").rows.length > 0) {
                $("[aria-label^='Delete']").closest("td").remove();
                //$("[id='Ribbon.ListForm.Edit.Actions.AttachFile-Large']").hide();
            }
        }

        if ($("select[title='Controle de status Campo Obrigatório']").val() != "Aguardando informações") {
            if ($("textarea[title='Esclarecimentos']").closest("span").next("div").text() == "Não há entradas existentes.") {
                $("textarea[title='Esclarecimentos']").closest("tr").hide();
            }
            else
                $("textarea[title='Esclarecimentos']").attr('disabled', 'disabled');

        }

        if ($("select[title='Controle de status Campo Obrigatório']").val() != "Encaminhado à outra gerência da DO") {
            if ($("textarea[title='Esclarecimentos Gerência']").closest("span").next("div").text() == "Não há entradas existentes.") {
                $("textarea[title='Esclarecimentos Gerência']").closest("tr").hide();
                $("select[title='Gerência Responsável']").closest("tr").hide();
            }
            else {
                $("textarea[title='Esclarecimentos Gerência']").attr('disabled', 'disabled');
                $("select[title='Gerência Responsável']").closest("tr").hide();
            }
        }

        $("input[title='Data de encerramento']").attr('disabled', 'disabled');
        $("input[title='Data de encerramento']").closest("tr").find("td:eq(1)").hide();
    }
    else {
        //Testa se tem Responsavel - Sem resposanvel então é solicitante		    
        if ($('nobr:contains("Responsável")').closest('tr').find('div.sp-peoplepicker-topLevel')[0].innerText == "Digite um nome ou email...") {
            if (ESolicitante) {
                //sou solicitante
                $("select[title='Controle de status Campo Obrigatório']").attr('disabled', 'disabled');
                /*$("input[title='Prazo de Atendimento Campo Obrigatório']").closest("tr").hide();
		        $('nobr:contains("Responsável")').closest('tr').hide();
		        $("select[title='Enviar email ao responsável']").closest("tr").hide();
		        $("select[title='Complexidade']").closest("tr").hide();
		        $("select[title='Importância']").closest("tr").hide();
		        $('nobr:contains("encerramento")').closest("tr").hide();*/
                $("input[title='Data de encerramento']").attr('disabled', 'disabled'); $("input[title='Data de encerramento']").closest("tr").find("td:eq(1)").hide();
                if ($("select[title='Controle de status Campo Obrigatório']").val() == "Aberto") {
                    $("textarea[title='Esclarecimentos']").closest("tr").hide();
                    $("textarea[title='Esclarecimentos Gerência']").closest("tr").hide();
                }
                else if ($("select[title='Controle de status Campo Obrigatório']").val() == "Aguardando informações") {
                    $("input[title='Assunto']").attr('disabled', 'disabled');
                    $(".ms-rtefield div[role='textbox']").attr("contenteditable", "false");
                    $(".ms-rtefield div[role='textbox']").removeAttr('class'); $(".ms-rtefield div[role='textbox']").css("background-color", "rgb(253, 253, 253)"); $(".ms-rtefield div[role='textbox']").css("color", "rgb(177, 177, 177)");
                }
                else if ($("select[title='Controle de status Campo Obrigatório']").val() == "Encaminhado à outra gerência da DO") {
                    $("input[title='Assunto']").attr('disabled', 'disabled');
                    $(".ms-rtefield div[role='textbox']").attr("contenteditable", "false");
                    $(".ms-rtefield div[role='textbox']").removeAttr('class'); $(".ms-rtefield div[role='textbox']").css("background-color", "rgb(253, 253, 253)"); $(".ms-rtefield div[role='textbox']").css("color", "rgb(177, 177, 177)");
                    $("textarea[title='Esclarecimentos Gerência']").attr('disabled', 'disabled');
                }
                else {
                    if ($("textarea[title='Esclarecimentos']").closest("span").next("div").text() == "Não há entradas existentes.") {
                        $("textarea[title='Esclarecimentos']").closest("tr").hide();
                    }
                    else
                        $("textarea[title='Esclarecimentos']").attr('disabled', 'disabled');

                    if ($("textarea[title='Esclarecimentos Gerência']").closest("span").next("div").text() == "Não há entradas existentes.") {
                        $("textarea[title='Esclarecimentos Gerência']").closest("tr").hide();
                    }
                    else
                        $("textarea[title='Esclarecimentos Gerência']").attr('disabled', 'disabled');

                    $("input[title='Assunto']").attr('disabled', 'disabled');
                    $(".ms-rtefield div[role='textbox']").attr("contenteditable", "false");
                    $(".ms-rtefield div[role='textbox']").removeAttr('class');
                    $("[id^=topDiv]").removeClass('ms-inputBox'); $($("div[id^=DescricaoChamado]")[0]).css('border', '1px solid rgb(225, 225, 225)');
                    $(".ms-rtefield div[role='textbox']").css("background-color", "rgb(253, 253, 253)"); $(".ms-rtefield div[role='textbox']").css("color", "rgb(177, 177, 177)");
                    if (document.getElementById("idAttachmentsTable").rows.length > 0) {
                        $("[aria-label^='Delete']").closest("td").remove();
                        //$("[id='Ribbon.ListForm.Edit.Actions.AttachFile-Large']").hide();
                    }
                }
            }
            var ppDiv = $("div[title='Responsável']")[0];
            var peoplePicker = SPClientPeoplePicker.SPClientPeoplePickerDict[ppDiv.id];
            peoplePicker.SetEnabledState(false);
            $('nobr:contains("Responsável")').closest("tr").find("td:eq(1)").find('.sp-peoplepicker-delImage').hide();

        }
        else {// Se tem Responsavel, pode ser responsavel
            var ppDiv = $("div[title='Responsável']")[0];
            var peoplePicker = SPClientPeoplePicker.SPClientPeoplePickerDict[ppDiv.id];
            var userList = peoplePicker.GetAllUserInfo();
            var userInfo = userList[0];
            if (userInfo != null) {
                if (userInfo.EntityData.SPUserID == UserIDCurrent) {// É o responsavel
                    E_resp = true;
                    $("input[title='Prazo de Atendimento Campo Obrigatório']").closest("tr").show();
                    $('nobr:contains("Responsável")').closest('tr').show();
                    //$("select[title='Enviar email ao responsável']").closest("tr").show();
                    $("select[title='Complexidade']").closest("tr").show();
                    $("select[title='Importância']").closest("tr").show();
                    $('nobr:contains("encerramento")').closest("tr").show();
                    //$("select[title='Enviar email ao responsável']").closest("tr").show();
                    $("input[title='Data de encerramento']").attr('disabled', 'disabled');
                    $("input[title='Data de encerramento']").closest("tr").find("td:eq(1)").hide();

                    if ($("select[title='Controle de status Campo Obrigatório']").val() != "Aberto") {
                        $("input[title='Assunto']").attr('disabled', 'disabled');
                        $(".ms-rtefield div[role='textbox']").attr("contenteditable", "false");
                        $(".ms-rtefield div[role='textbox']").removeAttr('class');
                        $("[id^=topDiv]").removeClass('ms-inputBox'); $($("div[id^=DescricaoChamado]")[0]).css('border', '1px solid rgb(225, 225, 225)');
                        $(".ms-rtefield div[role='textbox']").css("background-color", "rgb(253, 253, 253)"); $(".ms-rtefield div[role='textbox']").css("color", "rgb(177, 177, 177)");
                        if (document.getElementById("idAttachmentsTable").rows.length > 0) {
                            $("[aria-label^='Delete']").closest("td").remove();
                            //$("[id='Ribbon.ListForm.Edit.Actions.AttachFile-Large']").hide();
                        }
                    }

                    peoplePicker.SetEnabledState(false);
                    $('nobr:contains("Responsável")').closest("tr").find("td:eq(1)").find('.sp-peoplepicker-delImage').hide();


                    if (ESolicitante == false) {//Não é Admin e não é Solicitante
                        $("input[title='Assunto']").attr('disabled', 'disabled');
                        $(".ms-rtefield div[role='textbox']").attr("contenteditable", "false");
                        $(".ms-rtefield div[role='textbox']").removeAttr('class');
                        $("[id^=topDiv]").removeClass('ms-inputBox'); $($("div[id^=DescricaoChamado]")[0]).css('border', '1px solid rgb(225, 225, 225)');
                        $(".ms-rtefield div[role='textbox']").css("background-color", "rgb(253, 253, 253)"); $(".ms-rtefield div[role='textbox']").css("color", "rgb(177, 177, 177)");
                    }

                    if ($("select[title='Controle de status Campo Obrigatório']").val() != "Aguardando informações") {
                        if ($("textarea[title='Esclarecimentos']").closest("span").next("div").text() == "Não há entradas existentes.") {
                            $("textarea[title='Esclarecimentos']").closest("tr").hide();
                        }
                        else
                            $("textarea[title='Esclarecimentos']").attr('disabled', 'disabled');

                        //$("select[title='Gerência Responsável']").closest("tr").hide();
                    }

                    if ($("select[title='Controle de status Campo Obrigatório']").val() != "Encaminhado à outra gerência da DO") {
                        if ($("textarea[title='Esclarecimentos Gerência']").closest("span").next("div").text() == "Não há entradas existentes.") {
                            $("textarea[title='Esclarecimentos Gerência']").closest("tr").hide();
                            $("select[title='Gerência Responsável']").closest("tr").hide();
                        }
                        else {
                            $("textarea[title='Esclarecimentos Gerência']").attr('disabled', 'disabled');
                            $("select[title='Gerência Responsável']").closest("tr").hide();
                        }
                    }

                }//fim do responsavel
                else {//É solicitante
                    $("select[title='Controle de status Campo Obrigatório']").attr('disabled', 'disabled');
                    $("input[title='Data de encerramento']").attr('disabled', 'disabled'); $("input[title='Data de encerramento']").closest("tr").find("td:eq(1)").hide();
                    if ($("select[title='Controle de status Campo Obrigatório']").val() == "Aberto") {
                        $("textarea[title='Esclarecimentos']").closest("tr").hide();
                        $("textarea[title='Esclarecimentos Gerência']").closest("tr").hide();
                    }
                    else if ($("select[title='Controle de status Campo Obrigatório']").val() == "Aguardando informações") {
                        $("input[title='Assunto']").attr('disabled', 'disabled');
                        $(".ms-rtefield div[role='textbox']").attr("contenteditable", "false");
                        $(".ms-rtefield div[role='textbox']").removeAttr('class'); $(".ms-rtefield div[role='textbox']").css("background-color", "rgb(253, 253, 253)"); $(".ms-rtefield div[role='textbox']").css("color", "rgb(177, 177, 177)");
                        if (document.getElementById("idAttachmentsTable").rows.length > 0) {
                            $("[aria-label^='Delete']").closest("td").remove();
                            //$("[id='Ribbon.ListForm.Edit.Actions.AttachFile-Large']").hide();
                        }

                        if ($("textarea[title='Esclarecimentos Gerência']").closest("span").next("div").text() == "Não há entradas existentes.") {
                            $("textarea[title='Esclarecimentos Gerência']").closest("tr").hide();
                        }
                        else {
                            $("select[title='Gerência Responsável']").attr('disabled', 'disabled');
                        }

                    }
                    else if ($("select[title='Controle de status Campo Obrigatório']").val() == "Encaminhado à outra gerência da DO") {
                        $("input[title='Assunto']").attr('disabled', 'disabled');
                        $(".ms-rtefield div[role='textbox']").attr("contenteditable", "false");
                        $(".ms-rtefield div[role='textbox']").removeAttr('class'); $(".ms-rtefield div[role='textbox']").css("background-color", "rgb(253, 253, 253)"); $(".ms-rtefield div[role='textbox']").css("color", "rgb(177, 177, 177)");
                        $("textarea[title='Esclarecimentos Gerência']").attr('disabled', 'disabled');
                        if (document.getElementById("idAttachmentsTable").rows.length > 0) {
                            $("[aria-label^='Delete']").closest("td").remove();
                            //$("[id='Ribbon.ListForm.Edit.Actions.AttachFile-Large']").hide();
                        }

                        if (ESolicitante) {
                            if ($("textarea[title='Esclarecimentos Gerência']").closest("span").next("div").text() == "Não há entradas existentes.") {
                                $("textarea[title='Esclarecimentos Gerência']").closest("tr").hide();
                                $("textarea[title='Esclarecimentos']").closest("tr").hide();
                                $("select[title='Gerência Responsável']").closest("tr").hide();
                            }
                            else {
                                //$("textarea[title='Esclarecimentos Gerência']").attr('disabled', 'disabled');
                                $("textarea[title='Esclarecimentos']").closest("tr").hide();
                                $("select[title='Gerência Responsável']").attr('disabled', 'disabled');
                                $("select[title='Gerência Responsável']").closest("tr").show();
                            }
                        }
                        else {
                            if ($("textarea[title='Esclarecimentos Gerência']").closest("span").next("div").text() == "Não há entradas existentes.") {
                                $("textarea[title='Esclarecimentos Gerência']").attr('disabled', 'disabled');
                                $("textarea[title='Esclarecimentos']").closest("tr").hide();
                                $("select[title='Gerência Responsável']").attr('disabled', 'disabled');
                            }
                            else {
                                //$("textarea[title='Esclarecimentos Gerência']").attr('disabled', 'disabled');
                                $("textarea[title='Esclarecimentos']").closest("tr").hide();
                                $("select[title='Gerência Responsável']").attr('disabled', 'disabled');
                                $("select[title='Gerência Responsável']").closest("tr").show();
                                $("textarea[title='Esclarecimentos Gerência']").prop('disabled', false);
                            }
                        }

                    }
                    else {
                        if ($("textarea[title='Esclarecimentos']").closest("span").next("div").text() == "Não há entradas existentes.") {
                            $("textarea[title='Esclarecimentos']").closest("tr").hide();
                        }
                        else
                            $("textarea[title='Esclarecimentos']").attr('disabled', 'disabled');

                        if ($("textarea[title='Esclarecimentos Gerência']").closest("span").next("div").text() == "Não há entradas existentes.") {
                            $("textarea[title='Esclarecimentos Gerência']").closest("tr").hide();
                            $("select[title='Gerência Responsável']").closest("tr").hide();
                        }
                        else {
                            $("textarea[title='Esclarecimentos Gerência']").attr('disabled', 'disabled');
                            $("select[title='Gerência Responsável']").closest("tr").hide();
                        }


                        $("input[title='Assunto']").attr('disabled', 'disabled');
                        $(".ms-rtefield div[role='textbox']").attr("contenteditable", "false");
                        $(".ms-rtefield div[role='textbox']").removeAttr('class');
                        $("[id^=topDiv]").removeClass('ms-inputBox'); $($("div[id^=DescricaoChamado]")[0]).css('border', '1px solid rgb(225, 225, 225)');
                        $(".ms-rtefield div[role='textbox']").css("background-color", "rgb(253, 253, 253)"); $(".ms-rtefield div[role='textbox']").css("color", "rgb(177, 177, 177)");
                        if (document.getElementById("idAttachmentsTable").rows.length > 0) {
                            $("[aria-label^='Delete']").closest("td").remove();
                            //$("[id='Ribbon.ListForm.Edit.Actions.AttachFile-Large']").hide();
                        }
                    }
                }
            }//fim do else
        }
    }
    $("input[title='Imediato do Responsável']").closest("tr").hide();
}



function Pefil() {
    if (isMember("Administradores GGA")) {

        var resp = 0;

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
                var ESolicitante = false;

                var query = _spPageContextInfo.webAbsoluteUrl + "/_vti_bin/listdata.svc/Chamados?$filter=ID eq " + getQueryString("ID");
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
                            if (data.d.results[0].NomeDoSolicitanteId = result.d.Id)
                                ESolicitante = true;
                        }
                    }
                });

                //try{
                var ppDiv = $("div[title='Responsável']")[0];
                var peoplePicker = SPClientPeoplePicker.SPClientPeoplePickerDict[ppDiv.id];
                var userList = peoplePicker.GetAllUserInfo();
                var userInfo = userList[0];
                if (userInfo != null) {
                    if (userInfo.Key == result.d.LoginName) {
                        $("input[title='Data de encerramento']").attr('disabled', 'disabled');
                        $("input[title='Data de encerramento']").closest("tr").find("td:eq(1)").hide();

                        if ($("select[title='Controle de status Campo Obrigatório']").val() != "Aberto") {
                            $("input[title='Assunto']").attr('disabled', 'disabled');
                            $(".ms-rtefield div[role='textbox']").attr("contenteditable", "false");
                            $(".ms-rtefield div[role='textbox']").removeAttr('class');
                            $("[id^=topDiv]").removeClass('ms-inputBox'); $($("div[id^=DescricaoChamado]")[0]).css('border', '1px solid rgb(225, 225, 225)');
                            $(".ms-rtefield div[role='textbox']").css("background-color", "rgb(253, 253, 253)"); $(".ms-rtefield div[role='textbox']").css("color", "rgb(177, 177, 177)");

                        }
                        else {
                            if (ESolicitante == false) {
                                $("input[title='Assunto']").attr('disabled', 'disabled');
                                $(".ms-rtefield div[role='textbox']").attr("contenteditable", "false");
                                $(".ms-rtefield div[role='textbox']").removeAttr('class');
                                $("[id^=topDiv]").removeClass('ms-inputBox'); $($("div[id^=DescricaoChamado]")[0]).css('border', '1px solid rgb(225, 225, 225)');
                                $(".ms-rtefield div[role='textbox']").css("background-color", "rgb(253, 253, 253)"); $(".ms-rtefield div[role='textbox']").css("color", "rgb(177, 177, 177)");
                            }
                            else {
                                //LoadModelos();
                            }
                        }

                        if ($("select[title='Controle de status Campo Obrigatório']").val() != "Aguardando informações") {
                            $("textarea[title='Esclarecimentos']").closest("tr").hide();
                        }
                        resp = userInfo.Key;
                    }
                    else {
                        /*$("select[title='Controle de status Campo Obrigatório']").attr('disabled', 'disabled');
                        $("input[title='Prazo de Atendimento Campo Obrigatório']").attr('disabled', 'disabled');
                        $("select[title='Enviar email ao responsável']").attr('disabled', 'disabled');
                        $("select[title='Complexidade']").attr('disabled', 'disabled');
                        $("select[title='Importância']").attr('disabled', 'disabled');*/
                        $("input[title='Data de encerramento']").attr('disabled', 'disabled');
                        $("input[title='Data de encerramento']").closest("tr").find("td:eq(1)").hide();

                        if ($("select[title='Controle de status Campo Obrigatório']").val() != "Aberto") {
                            $("input[title='Assunto']").attr('disabled', 'disabled');
                            $(".ms-rtefield div[role='textbox']").attr("contenteditable", "false");
                            $(".ms-rtefield div[role='textbox']").removeAttr('class');
                            $("[id^=topDiv]").removeClass('ms-inputBox'); $($("div[id^=DescricaoChamado]")[0]).css('border', '1px solid rgb(225, 225, 225)');
                            $(".ms-rtefield div[role='textbox']").css("background-color", "rgb(253, 253, 253)"); $(".ms-rtefield div[role='textbox']").css("color", "rgb(177, 177, 177)");
                        }
                        else {
                            if (ESolicitante == false) {
                                $("input[title='Assunto']").attr('disabled', 'disabled');
                                $(".ms-rtefield div[role='textbox']").attr("contenteditable", "false");
                                $(".ms-rtefield div[role='textbox']").removeAttr('class');
                                $("[id^=topDiv]").removeClass('ms-inputBox'); $($("div[id^=DescricaoChamado]")[0]).css('border', '1px solid rgb(225, 225, 225)');
                                $(".ms-rtefield div[role='textbox']").css("background-color", "rgb(253, 253, 253)"); $(".ms-rtefield div[role='textbox']").css("color", "rgb(177, 177, 177)");
                            }
                            else {
                                //LoadModelos();
                            }
                        }

                        if ($("select[title='Controle de status Campo Obrigatório']").val() != "Aguardando informações") {
                            $("textarea[title='Esclarecimentos']").closest("tr").hide();
                        }
                    }
                }
                else {
                    /*$("select[title='Controle de status Campo Obrigatório']").attr('disabled', 'disabled');
                    $("input[title='Prazo de Atendimento Campo Obrigatório']").attr('disabled', 'disabled');
                    $("select[title='Enviar email ao responsável']").attr('disabled', 'disabled');
                    $("select[title='Complexidade']").attr('disabled', 'disabled');
                    $("select[title='Importância']").attr('disabled', 'disabled');*/
                    $("input[title='Data de encerramento']").attr('disabled', 'disabled');
                    $("input[title='Data de encerramento']").closest("tr").find("td:eq(1)").hide();
                    if ($("select[title='Controle de status Campo Obrigatório']").val() != "Aberto") {
                        $("input[title='Assunto']").attr('disabled', 'disabled');
                        $(".ms-rtefield div[role='textbox']").attr("contenteditable", "false");
                    }
                    else {
                        if (ESolicitante == false) {
                            $("input[title='Assunto']").attr('disabled', 'disabled');
                            $(".ms-rtefield div[role='textbox']").attr("contenteditable", "false");
                            $(".ms-rtefield div[role='textbox']").removeAttr('class');
                            $("[id^=topDiv]").removeClass('ms-inputBox'); $($("div[id^=DescricaoChamado]")[0]).css('border', '1px solid rgb(225, 225, 225)');
                            $(".ms-rtefield div[role='textbox']").css("background-color", "rgb(253, 253, 253)"); $(".ms-rtefield div[role='textbox']").css("color", "rgb(177, 177, 177)");
                        }
                        else {
                            //LoadModelos();
                        }
                    }

                    if ($("select[title='Controle de status Campo Obrigatório']").val() != "Aguardando informações") {
                        $("textarea[title='Esclarecimentos']").closest("tr").hide();
                    }
                }
                //}
                //catch(ex){}
            });
        });
    }
    else {
        $("select[title='Controle de status Campo Obrigatório']").attr('disabled', 'disabled');
        $("input[title='Prazo de Atendimento Campo Obrigatório']").closest("tr").hide();
        $('nobr:contains("Responsável")').closest('tr').hide();
        //$("select[title='Enviar email ao responsável']").closest("tr").hide();
        $("select[title='Complexidade']").closest("tr").hide();
        $("select[title='Importância']").closest("tr").hide();
        $('nobr:contains("encerramento")').closest("tr").hide();

        if ($("select[title='Controle de status Campo Obrigatório']").val() == "Aberto") {
            $("textarea[title='Esclarecimentos']").closest("tr").hide();
            //LoadModelos();	
        }
        else if ($("select[title='Controle de status Campo Obrigatório']").val() == "Aguardando informações") {
            $("input[title='Assunto']").attr('disabled', 'disabled');
            $(".ms-rtefield div[role='textbox']").attr("contenteditable", "false");
            $(".ms-rtefield div[role='textbox']").removeAttr('class'); $(".ms-rtefield div[role='textbox']").css("background-color", "rgb(253, 253, 253)"); $(".ms-rtefield div[role='textbox']").css("color", "rgb(177, 177, 177)");
        }
        else {
            $("textarea[title='Esclarecimentos']").closest("tr").hide();
            $("input[title='Assunto']").attr('disabled', 'disabled');
            $(".ms-rtefield div[role='textbox']").attr("contenteditable", "false");
            $(".ms-rtefield div[role='textbox']").removeAttr('class');
            $("[id^=topDiv]").removeClass('ms-inputBox'); $($("div[id^=DescricaoChamado]")[0]).css('border', '1px solid rgb(225, 225, 225)');
            $(".ms-rtefield div[role='textbox']").css("background-color", "rgb(253, 253, 253)"); $(".ms-rtefield div[role='textbox']").css("color", "rgb(177, 177, 177)");
        }
    }

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
            var ppDiv = $("div[title='Responsável']")[0];
            var peoplePicker = SPClientPeoplePicker.SPClientPeoplePickerDict[ppDiv.id];
            //Get list of users from field (assuming 1 in this case)
            var userList = peoplePicker.GetAllUserInfo();
            var userInfo = userList[0];
            if (userInfo != null) {
                if (userInfo.Key == result.d.LoginName) {
                    $("input[title='Assunto']").attr('disabled', 'disabled');
                    $("textarea[title='Descrição do chamado Campo Obrigatório']").css('width', '500px').attr('disabled', 'disabled');
                    //$("select[title='Enviar email ao responsável']").attr('disabled', 'disabled');
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
                        //$("select[title='Enviar email ao responsável']").attr('disabled', 'disabled');
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
                    //$("select[title='Enviar email ao responsável']").attr('disabled', 'disabled');
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
    var retorno = true;
    $('#ErroDataCustomizado').remove();
    $('#ErroDataCustomizado3').remove();
    $('#ErroDataCustomizado18').remove();

    if ($("input[title='Prazo de Atendimento Campo Obrigatório']").val() == "") {
        //$('nobr:contains("Prazo de Atendimento")').closest('tr').find('nobr:contains("Em dias")').closest("td").append("<span id='ErroDataCustomizado18' class='ms-formvalidation ms-csrformvalidation'>Você não pode deixar em branco.</span>");    
        //$("input[title='Prazo de Atendimento Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado18' class='ms-formvalidation ms-csrformvalidation'>Você não pode deixar em branco.</span>");
        $("input[title='Prazo de Atendimento Campo Obrigatório']").closest("span").next("span").append("<br><span id='ErroDataCustomizado18' class='ms-formvalidation ms-csrformvalidation'>Você não pode deixar em branco.</span>");
        retorno = false;
    }
    else if (parseInt($("input[title='Prazo de Atendimento Campo Obrigatório']").val()) < 1) {
        $("input[title='Prazo de Atendimento Campo Obrigatório']").closest("span").next("span").append("<br><span id='ErroDataCustomizado18' class='ms-formvalidation ms-csrformvalidation'>Prazo deve ser maior que 0.</span>");
        retorno = false;
    }

    if ($("select[title='Controle de status Campo Obrigatório']").val() == "Aberto") {
        if (ESolicitante) {
            var full = $('nobr:contains("Descrição")').closest('tr').find('div.ms-rtestate-write').text();
            if (full.length <= 1) {
                $('nobr:contains("Descrição")').closest('tr').find('div.ms-rtestate-write').closest("td").append("<span id='ErroDataCustomizado3' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
                retorno = false;
            }
        }
        else {
            var full = $($("div[id^='DescricaoChamado']")[2]).text();
            if (full.length <= 1) {
                $($("div[id^='DescricaoChamado']")[2]).closest("td").append("<span id='ErroDataCustomizado3' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
                retorno = false;
            }
        }
    }

    if ($("select[title='Controle de status Campo Obrigatório']").val() == "Aguardando informações") {
        //if(!$("textarea[title='Esclarecimentos']").is(':disabled')){
        if ($("textarea[title='Esclarecimentos']").val() == "") {
            alert("É preciso inserir uma informação para esclarecimento!");
            $("textarea[title='Esclarecimentos']").focus();
            retorno = false;
        }
        else {
            if (ESolicitante)
                $("select[title='Controle de status Campo Obrigatório'] option:contains(Em andamento)").attr('selected', 'selected');
        }
        //}	
    }

    if ($("select[title='Controle de status Campo Obrigatório']").val() == "Encaminhado à outra gerência da DO") {

        if ($("textarea[title='Esclarecimentos Gerência']").val() == "") {
            //alert("É preciso inserir uma informação para esclarecimento!");
            $("textarea[title='Esclarecimentos Gerência']").closest("td").append("<span id='ErroDataCustomizado6' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
            $("textarea[title='Esclarecimentos Gerência']").focus();
            retorno = false;
        }
        if ($("select[title='Gerência Responsável']").val() == "0") {
            $("select[title='Gerência Responsável']").closest("td").append("<span id='ErroDataCustomizado' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Selecione uma opção.</span>");
            retorno = false; $("select[title='Gerência Responsável']").focus();
        }
        else {
            if (!E_Admin && !E_resp)
                $("select[title='Controle de status Campo Obrigatório'] option:contains(Em andamento)").attr('selected', 'selected');
        }
    }

    if ($("select[title='Controle de status Campo Obrigatório']").val() == "Encerrado") {
        var d = new Date();
        $("input[title='Data de encerramento']").val((d.getDate() < 10 ? '0' : '') + d.getDate() + '/' +
			    ((d.getMonth() + 1) < 10 ? '0' : '') + (d.getMonth() + 1) + '/' +
			      d.getFullYear());
    }


    return retorno;

    /*var dtNS = $("input[title='Data de encerramento']").val();
	if(dtNS != "" && !isDate(dtNS)){
		$($("input[title='Data de encerramento']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
		retorno = false;
	}

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
    return true;*/
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

function LoadModelos() {
    var urlQuery = _spPageContextInfo.webServerRelativeUrl + "/_vti_bin/listdata.svc/ModeloDeChamado?$filter=TipoDeChamadoId eq " + $("select[title='Tipo de Chamado Campo Obrigatório']").val();
    var retornoHTML = [];
    $.getJSON(urlQuery,
        function (data) {
            if (data.d.results.length > 0) {
                retornoHTML.push('<div class="modelos"><span>');
                retornoHTML.push('<h3>Modelos de Chamado para Download</h3>');
                retornoHTML.push('</span>');

                $.each(data.d.results, function (i, item) {
                    var fileType = GetFileTypeImage();
                    retornoHTML.push('<div class="itemModelo">');
                    retornoHTML.push('<a href="' + item.__metadata.media_src + '">');
                    retornoHTML.push('<img src="' + fileType + '" alt="' + item.ModeloDeChamado + '" />');//</span>');
                    retornoHTML.push('<h2>' + item.ModeloDeChamado + '</h2></a>');
                    retornoHTML.push('</div></div>');
                });
                retornoHTML.push('<span style="color:red;font-size:0.8em;">Clique no arquivo para fazer o Download.</span>');
                document.getElementById('containerModelos').innerHTML = retornoHTML.join('');
            }
            else
                document.getElementById('containerModelos').innerHTML = "";
        });
}

function GetFileTypeImage() {
    return "/_layouts/15/images/lg_icxlsx.png";
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