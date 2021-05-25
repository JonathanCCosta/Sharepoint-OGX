var admin = false; statusConsolidacao = ""; var consecutivoAciona = false; var ERevisao = false;
$(document).ready(function () {
    $(".ms-cui-tabContainer").attr('style', 'z-index:99!important');
    ShowProgress();
    $("select[title='Tipo de Conteúdo']").closest("tr").hide();
    $("input[title='Código MTC']").closest("tr").hide();
    $("[id='Ribbon.ListForm.Edit.Actions.AttachFile-Large']").hide();
    $("input[title='Existe Comentários']").closest("tr").hide();
    statusConsolidacao = $("select[title='Controle de Status']").val();
    $("select[title='Em Revisão']").closest("tr").hide();
    ERevisao = ($("select[title='Em Revisão']").val() == "Sim" ? true : false);
    ConfiguraCampos();

    $("input[title='Prazo de Vigência (anos) Campo Obrigatório']").keypress(function (e) {
        if ((e.which < 48 || e.which > 57)) {
            return false;
        }
    });

    $("select[title='Grupo']").change(function () {
        $("select[title='Subgrupo']").empty();
        $("select[title='Subgrupo']").prepend("<option value='0' selected='selected'>Selecione uma opção</option>");
        FiltraSubgrupo();
    });

    $("select[title='Controle de Status']").change(function () {
        try {
            $('#ErroDataCustomizado').remove(); $('#ErroDataCustomizado1').remove(); $('#ErroDataCustomizado2').remove(); $('#ErroDataCustomizado3').remove();
            $('#ErroDataCustomizado4').remove(); $('#ErroDataCustomizado5').remove(); $('#ErroDataCustomizado6').remove(); $('#ErroDataCustomizado7').remove();
            $('#ErroDataCustomizado8').remove(); $('#ErroDataCustomizado9').remove(); $('#ErroDataCustomizado10').remove();
            $('#ErroDataCustomizado11').remove(); $('#ErroDataCustomizado12').remove(); $('#ErroDataCustomizado13').remove();
            $('#ErroDataCustomizado14').remove(); $('#ErroDataCustomizado15').remove(); $('#ErroDataCustomizado16').remove();
        }
        catch (ex) { }

        if ($("select[title='Controle de Status']").val() == "Cancelado") {
            $("textarea[title='Justificativa Cancelamento']").closest('tr').show();
            $("textarea[title='Justificativa Cancelamento']").prop('disabled', false);
            $('nobr:contains("Justificativa Cancelamento")').after('<span id="span_cancel" class="ms-accentText" title="Este é um campo obrigatório."> *</span>');
            //Outros status
            $("input[title='Data Limite Comentário']").closest("table").closest("tr").hide();
            $("textarea[title='Comentários']").closest("tr").hide();
            $("span[id='span_revisores']").remove();
            $("span[id='span_tipoMTC']").remove();
            $("span[id='span_grupo']").remove();
            $("span[id='span_subgrupo']").remove();
            $("span[id='span_fabricante']").remove();
            $("span[id='span_consecutivo']").remove();
            $("input[title='Consecutivo']").closest("tr").hide();
            $("[id='Ribbon.ListForm.Edit.Actions.AttachFile-Large']").hide();
            $("div[id='Atualiza']").remove();
            $("div[id='comentatioFaixa']").remove();
            $($(".ms-cui-ctl-large")[0]).show();
            $("input[value='Salvar']").prop('disabled', false);
            $("span[id='span_data']").remove();
            $("input[title='Revisão Campo Obrigatório']").closest("tr").hide();
            $("input[title='Data de Início de Vigência Campo Obrigatório']").closest("table").closest('tr').hide();
        }
        if ($("select[title='Controle de Status']").val() == "Em consolidação") {
            //if(ERevisao)
            //$("[id='Ribbon.ListForm.Edit.Actions.AttachFile-Large']").show();
        }
        else if ($("select[title='Controle de Status']").val() == "Para comentários") {
            $("input[title='Data Limite Comentário']").closest("table").closest("tr").show();
            $("span[id='span_limite']").remove();
            $('nobr:contains("Data Limite Comentário")').after('<span id="span_data" id="span_limite" class="ms-accentText" title="Este é um campo obrigatório."> *</span>');
            $("textarea[title='Comentários']").closest("tr").show();
            $("textarea[title='Comentários']").prop('disabled', false);
            $("input[title='Data Limite Comentário']").prop('disabled', false);
            $("input[title='Data Limite Comentário']").closest("tr").find("td:eq(1)").show();
            SP.SOD.executeFunc('SP.js', 'SP.ClientContext', function () {
                CurrentUserMemberOfGroup("Administradores GGA", function (isCurrentUserInGroup) {
                    if (isCurrentUserInGroup) {
                        admin = true;
                    }
                });
                CurrentUserMemberOfGroup("Administradores GGA MTC", function (isCurrentUserInGroup) {
                    if (isCurrentUserInGroup) {
                        admin = true;
                    }
                });
            });

            //outros status
            $("span[id='span_revisores']").remove();
            $("span[id='span_tipoMTC']").remove();
            $("span[id='span_grupo']").remove();
            $("span[id='span_subgrupo']").remove();
            $("span[id='span_fabricante']").remove();
            $("span[id='span_consecutivo']").remove();
            $("input[title='Consecutivo']").closest("tr").hide();
            //if(!ERevisao)
            $("[id='Ribbon.ListForm.Edit.Actions.AttachFile-Large']").hide();
            //else
            //    $("[id='Ribbon.ListForm.Edit.Actions.AttachFile-Large']").show();
            $("div[id='Atualiza']").remove();
            $("div[id='comentatioFaixa']").remove();
            $($(".ms-cui-ctl-large")[0]).show();
            $("input[value='Salvar']").prop('disabled', false);

            $("textarea[title='Justificativa Cancelamento']").closest('tr').hide();
            $("input[title='Revisão Campo Obrigatório']").closest("tr").show();
            $("input[title='Revisão Campo Obrigatório']").attr('disabled', 'disabled');
            $("input[title='Data de Início de Vigência Campo Obrigatório']").closest("table").closest('tr').show();

        }
        else if ($("select[title='Controle de Status']").val() == "Vigente") {
            $('nobr:contains("Tipo MTC")').after('<span class="ms-accentText" id="span_tipoMTC" title="Este é um campo obrigatório."> *</span>');
            $('nobr:contains("Grupo")').after('<span class="ms-accentText" id="span_grupo" title="Este é um campo obrigatório."> *</span>');
            $('nobr:contains("Subgrupo")').after('<span class="ms-accentText" id="span_subgrupo" title="Este é um campo obrigatório."> *</span>');
            $('nobr:contains("Fabricante")').after('<span class="ms-accentText" id="span_fabricante" title="Este é um campo obrigatório."> *</span>');
            $('nobr:contains("Consecutivo")').after('<span class="ms-accentText" id="span_consecutivo" title="Este é um campo obrigatório."> *</span>');
            $('nobr:contains("Autores")').after('<span class="ms-accentText" id="span_grupo" title="Este é um campo obrigatório."> *</span>');
            $('nobr:contains("Data de Início de Vigência")').after('<span class="ms-accentText" id="span_grupo" title="Este é um campo obrigatório."> *</span>');
            $('nobr:contains("Prazo de Vigência (anos)")').after('<span class="ms-accentText" id="span_grupo" title="Este é um campo obrigatório."> *</span>');
            $("span[id='span_revisores']").remove();
            if (ERevisao) {
                $('nobr:contains("Revisores")').after('<span class="ms-accentText" id="span_grupo" title="Este é um campo obrigatório."> *</span>');
            }
            $("input[title='Consecutivo']").attr('disabled', 'disabled');
            $("input[title='Consecutivo']").closest("tr").show();
            $("[id='Ribbon.ListForm.Edit.Actions.AttachFile-Large']").show();
            if ($("div[id='Atualiza']").length == 0)
                $($(".s4-wpcell-plain")[0]).after("<div id='Atualiza' style='font-style:italic;margin-bottom:10px;border:1px solid green; background-color:#98fb98;padding-top:8px;padding-bottom:8px;padding-left:8px;margin-bottom:10px;color:green;border-radius:3px;'><span>Para inserir uma nova versão do Manual Técnico basta ir em 'Anexar Arquivo' e escolher o arquivo.</span></div>");

            if (!ERevisao)
                $($(".ms-cui-ctl-large")[0]).hide();

            if (!ERevisao)
                $("input[value='Salvar']").attr('disabled', 'disabled');

            if ($("input[title='Existe Comentários']").val() == "S") {
                if ($("div[id='comentatioFaixa']").length == 0)
                    $($(".s4-wpcell-plain")[0]).after("<div id='comentatioFaixa' style='font-style:italic;margin-bottom:10px;border:1px solid #e3474a;background-color:#f1a2a48f;padding-top:8px;padding-bottom:8px;padding-left:8px;margin-bottom:10px;color:#e3474a;border-radius:3px;'><span>Existem comentários para consolidação.</span></div>");
            }

            if ($("input[id='btGeraConsecutivo']").length == 0) {
                if (!ERevisao) {
                    $("input[title='Consecutivo']").closest("td").after("<input id='btGeraConsecutivo' type='button' value='Gerar Consecutivo' onclick='gerarConsecutivo()' />");
                }
            }

            //Outros campos de outros status
            $("input[title='Data Limite Comentário']").closest("table").closest("tr").hide();
            $("textarea[title='Comentários']").closest("tr").hide();
            $("textarea[title='Justificativa Cancelamento']").closest('tr').hide();

            $("input[title='Revisão Campo Obrigatório']").closest("tr").show();
            $("input[title='Revisão Campo Obrigatório']").attr("disabled", "disabled");
            $("input[title='Data de Início de Vigência Campo Obrigatório']").closest("table").closest('tr').show();

            if (!admin) {
                $("textarea[title='Justificativa Cancelamento']").attr("disabled", "disabled");
            }
        }
        else if ($("select[title='Controle de Status']").val() == "Aguardando Aprovação") {
            //Outros status
            $("input[title='Data Limite Comentário']").closest("table").closest("tr").hide();
            $("textarea[title='Comentários']").closest("tr").hide();
            $("span[id='span_revisores']").remove();
            $("span[id='span_tipoMTC']").remove();
            $("span[id='span_grupo']").remove();
            $("span[id='span_subgrupo']").remove();
            $("span[id='span_fabricante']").remove();
            $("span[id='span_consecutivo']").remove();
            $("input[title='Consecutivo']").closest("tr").hide();
            $("[id='Ribbon.ListForm.Edit.Actions.AttachFile-Large']").hide();
            $("div[id='Atualiza']").remove();
            $("div[id='comentatioFaixa']").remove();
            $($(".ms-cui-ctl-large")[0]).show();
            $("input[value='Salvar']").prop('disabled', false);
            $("textarea[title='Justificativa Cancelamento']").closest('tr').hide();
            $("span[id='span_data']").remove();

            $("input[title='Revisão Campo Obrigatório']").closest("tr").hide();
            //$("input[title='Data de Início de Vigência Campo Obrigatório']").closest("table").closest('tr').hide();
        }
        else if ($("select[title='Controle de Status']").val() == "Em revisão") {
            //Outros campos de outros status
            $("input[title='Data Limite Comentário']").closest("table").closest("tr").hide();
            $("textarea[title='Comentários']").closest("tr").hide();
            $("textarea[title='Justificativa Cancelamento']").closest('tr').hide();

            $("input[title='Revisão Campo Obrigatório']").closest("tr").show();
            $("input[title='Revisão Campo Obrigatório']").attr("disabled", "disabled");
            $("input[title='Data de Início de Vigência Campo Obrigatório']").closest("table").closest('tr').show();
        }
    });

});

function gerarConsecutivo() {
    var urlQuery = _spPageContextInfo.webAbsoluteUrl + "/_vti_bin/listdata.svc/ManualTécnicoDeCampo?$filter=ControleDeStatusValue eq 'Vigente' &$top=1&$orderby=Consecutivo desc";
    var num = "01"
    $.ajax({
        type: "GET",
        contentType: "application/json;charset=ISO-8859-1",
        url: encodeURI(urlQuery),
        cache: false,
        async: false,
        dataType: 'json',
        headers: { "Accept": "application/json; odata=verbose" },
        success: function (data) {
            if (data.d.length > 0) {
                $("input[title='Consecutivo']").val(parseInt(data.d[0].Consecutivo) >= 9 ? parseInt(data.d[0].Consecutivo) + 1 : "0" + (parseInt(data.d[0].Consecutivo) + 1));
                $($(".ms-cui-ctl-large")[0]).show();
                $("input[value='Salvar']").prop('disabled', false); consecutivoAciona = true;
            }
            else {
                $("input[title='Consecutivo']").val(num);
                $($(".ms-cui-ctl-large")[0]).show();
                $("input[value='Salvar']").prop('disabled', false); consecutivoAciona = true;
            }
        }
    });
}

function ConfiguraCampos() {
    try {
        ConfiguraTela();
        $(".loading").hide(); $(".modal").hide();
    }
    catch (ex) { }
    $(".loading").hide(); $(".modal").hide();
}

function ConfiguraTela() {
    if ($("select[title='Tipo de Conteúdo'] option:selected").text() == "Legado") {

    }
    else {//solicitação

        if ($("select[title='Tipo MTC']").val() == "0") {
            $("select[title='Tipo MTC'] option:contains('(Nenhum)')").remove();
            $("select[title='Tipo MTC']").prepend("<option value='0' selected='selected'>Selecione uma opção</option>");
        }
        else {
            $("select[title='Tipo MTC'] option:contains('(Nenhum)')").remove();
            $("select[title='Tipo MTC']").prepend("<option value='0'>Selecione uma opção</option>");
        }

        if ($("select[title='Grupo']").val() == "0") {
            $("select[title='Grupo'] option:contains('(Nenhum)')").remove();
            $("select[title='Grupo']").prepend("<option value='0' selected='selected'>Selecione uma opção</option>");
        }
        else {
            $("select[title='Grupo'] option:contains('(Nenhum)')").remove();
            $("select[title='Grupo']").prepend("<option value='0'>Selecione uma opção</option>");
        }

        if ($("select[title='Subgrupo']").val() == "0") {
            $("select[title='Subgrupo']").empty();
            $("select[title='Subgrupo']").prepend("<option value='0' selected='selected'>Selecione uma opção</option>");
            FiltraSubgrupo();
        }
        else {
            FiltraSubgrupoSelected($("select[title='Subgrupo']").val());
            $("select[title='Subgrupo']").prepend("<option value='0'>Selecione uma opção</option>");
        }

        if ($("select[title='Fabricante']").val() == "0") {
            $("select[title='Fabricante'] option:contains('(Nenhum)')").remove();
            $("select[title='Fabricante']").prepend("<option value='0' selected='selected'>Selecione uma opção</option>");
        }
        else {
            $("select[title='Fabricante'] option:contains('(Nenhum)')").remove();
            $("select[title='Fabricante']").prepend("<option value='0'>Selecione uma opção</option>");
        }

        //Aguardando Aprovação
        if ($("select[title='Controle de Status']").val() == "Aguardando Aprovação") {
            //$("input[title='Consecutivo']").attr('disabled', 'disabled');
            $("input[title='Consecutivo']").closest('tr').hide();
            $("input[title='Revisão Campo Obrigatório']").attr('disabled', 'disabled');
            $('nobr:contains("Aprovado por")').closest('tr').hide();
            $("input[title='Data Aprovação']").closest("table").closest('tr').hide();
            //$('nobr:contains("Revisores")').closest('tr').hide();
            $("textarea[title='Justificativa Cancelamento']").closest('tr').hide();
            $("input[title='Data Limite Comentário']").closest("table").closest("tr").hide();
            $("textarea[title='Comentários']").closest('tr').hide();
            $("select[title='Controle de Status'] option:contains('Em consolidação')").remove();
            $("select[title='Controle de Status'] option:contains('Em revisão')").remove();
            $("select[title='Controle de Status'] option:contains('Histórico')").remove();
            //$('nobr:contains("Data de Início de Vigência")').after('<span class="ms-accentText" title="Este é um campo obrigatório."> *</span>');

            setTimeout(function () {
                if (ERevisao) {
                    SP.SOD.executeFunc("clientpeoplepicker.js", "SP.ClientContext", function () {
                        var ppDiv = $("div[title='Colaboradores']")[0];
                        var peoplePicker = SPClientPeoplePicker.SPClientPeoplePickerDict[ppDiv.id];
                        peoplePicker.SetEnabledState(false);
                        $('nobr:contains("Colaboradores")').closest("tr").find("td:eq(1)").find('.sp-peoplepicker-delImage').hide();

                        var ppDivA = $("div[title='Autores']")[0];
                        var peoplePickerA = SPClientPeoplePicker.SPClientPeoplePickerDict[ppDivA.id];
                        peoplePickerA.SetEnabledState(false);
                        $('nobr:contains("Autores")').closest("tr").find("td:eq(1)").find('.sp-peoplepicker-delImage').hide();
                    });
                    $("select[title='Tipo MTC']").attr('disabled', 'disabled');
                    $("select[title='Grupo']").attr('disabled', 'disabled');
                    $("select[title='Subgrupo']").attr('disabled', 'disabled');
                    $("select[title='Fabricante']").attr('disabled', 'disabled');
                }
                $(".loading").hide(); $(".modal").hide();

            }, 2000);
        }
        else if ($("select[title='Controle de Status']").val() == "Para comentários") {
            $("select[title='Controle de Status'] option:contains('Em revisão')").remove();
            $("select[title='Controle de Status'] option:contains('Histórico')").remove();
            $('nobr:contains("Data Limite Comentário")').after('<span class="ms-accentText" title="Este é um campo obrigatório."> *</span>');
            $("input[title='Consecutivo']").attr('disabled', 'disabled');
            $("input[title='Revisão Campo Obrigatório']").attr('disabled', 'disabled');
            $("input[title='Data Aprovação']").closest("table").closest("tr").hide();
            $("textarea[title='Justificativa Cancelamento']").closest("tr").hide();

            SP.SOD.executeFunc('SP.js', 'SP.ClientContext', function () {
                CurrentUserMemberOfGroup("Administradores GGA", function (isCurrentUserInGroup) {
                    if (isCurrentUserInGroup) {
                        admin = true;
                    }
                });
                CurrentUserMemberOfGroup("Administradores GGA MTC", function (isCurrentUserInGroup) {
                    if (isCurrentUserInGroup) {
                        admin = true;
                    }
                });
            });

            SP.SOD.executeFunc("clientpeoplepicker.js", "SP.ClientContext", function () {
                setTimeout(function () {
                    if (!admin) {
                        $("select[title='Controle de Status']").attr('disabled', 'disabled');
                        $("input[title='Data Limite Comentário']").attr('disabled', 'disabled');
                        $("input[title='Data Limite Comentário']").closest("tr").find("td:eq(1)").hide();
                        $("select[title='Tipo MTC']").attr('disabled', 'disabled');
                        $("select[title='Grupo']").attr('disabled', 'disabled');
                        $("select[title='Subgrupo']").attr('disabled', 'disabled');
                        $("select[title='Fabricante']").attr('disabled', 'disabled');
                        $("textarea[title='Descrição do MTC Campo Obrigatório']").attr('disabled', 'disabled');
                        $("textarea[title='Objetivo Campo Obrigatório']").attr('disabled', 'disabled');
                        $("select[title='Órgão Responsável Campo Obrigatório']").attr('disabled', 'disabled');
                        //$("input[title='Data Aprovação']").closest("table").closest("tr").hide();
                        $("input[title='Data de Início de Vigência']").attr('disabled', 'disabled');
                        $("input[title='Data de Início de Vigência']").closest("tr").find("td:eq(1)").hide();
                        $("input[title='Prazo de Vigência (anos)']").attr('disabled', 'disabled');
                        $("select[title='Controle de Status']").attr('disabled', 'disabled');
                        $("textarea[title='Justificativa Solicitação Campo Obrigatório']").attr('disabled', 'disabled');
                        //$("textarea[title='Justificativa Cancelamento']").closest("tr").hide();
                        $("div[title='Aprovado por']").closest("tr").hide();
                        $("div[title='Palavra Chave']").addClass("ms-taxonomy-disabled");
                        $("div[title='Palavra Chave']").parent().find('img').hide();
                        $("div[title='Palavra Chave']").children().prop("contenteditable", false);
                        $('nobr:contains("Comentários")').after('<span class="ms-accentText" title="Este é um campo obrigatório."> *</span>');
                        SP.SOD.executeFunc("clientpeoplepicker.js", "SP.ClientContext", function () {
                            var ppDiv = $("div[title='Colaboradores']")[0];
                            var peoplePicker = SPClientPeoplePicker.SPClientPeoplePickerDict[ppDiv.id];
                            peoplePicker.SetEnabledState(false);
                            $('nobr:contains("Colaboradores")').closest("tr").find("td:eq(1)").find('.sp-peoplepicker-delImage').hide();

                            var ppDivA = $("div[title='Autores']")[0];
                            var peoplePickerA = SPClientPeoplePicker.SPClientPeoplePickerDict[ppDivA.id];
                            peoplePickerA.SetEnabledState(false);
                            $('nobr:contains("Autores")').closest("tr").find("td:eq(1)").find('.sp-peoplepicker-delImage').hide();

                            /*var ppDivAP = $("div[title='Aprovado por']")[0];
				            var peoplePickerAP = SPClientPeoplePicker.SPClientPeoplePickerDict[ppDivAP.id];
					        peoplePickerAP.SetEnabledState(false);
					        $('nobr:contains("Aprovado por")').closest("tr").find("td:eq(1)").find('.sp-peoplepicker-delImage').hide();*/

                            var ppDivR = $("div[title='Revisores']")[0];
                            var peoplePickerR = SPClientPeoplePicker.SPClientPeoplePickerDict[ppDivR.id];
                            peoplePickerR.SetEnabledState(false);
                            $('nobr:contains("Revisores")').closest("tr").find("td:eq(1)").find('.sp-peoplepicker-delImage').hide();
                        });
                    }

                    if (ERevisao) {
                        if (admin) {
                            SP.SOD.executeFunc("clientpeoplepicker.js", "SP.ClientContext", function () {
                                var ppDiv = $("div[title='Colaboradores']")[0];
                                var peoplePicker = SPClientPeoplePicker.SPClientPeoplePickerDict[ppDiv.id];
                                peoplePicker.SetEnabledState(false);
                                $('nobr:contains("Colaboradores")').closest("tr").find("td:eq(1)").find('.sp-peoplepicker-delImage').hide();

                                var ppDivA = $("div[title='Autores']")[0];
                                var peoplePickerA = SPClientPeoplePicker.SPClientPeoplePickerDict[ppDivA.id];
                                peoplePickerA.SetEnabledState(false);
                                $('nobr:contains("Autores")').closest("tr").find("td:eq(1)").find('.sp-peoplepicker-delImage').hide();
                            });
                        }
                        $('nobr:contains("Revisores")').after('<span class="ms-accentText" id="span_revisores" title="Este é um campo obrigatório."> *</span>');
                        $("select[title='Tipo MTC']").attr('disabled', 'disabled');
                        $("select[title='Grupo']").attr('disabled', 'disabled');
                        $("select[title='Subgrupo']").attr('disabled', 'disabled');
                        $("select[title='Fabricante']").attr('disabled', 'disabled');
                    }
                    $(".loading").hide(); $(".modal").hide();
                }, 2000);
            });
        }
        else if ($("select[title='Controle de Status']").val() == "Em consolidação") {
            $("[id='Ribbon.ListForm.Edit.Actions.AttachFile-Large']").show();
            $('nobr:contains("Data de Início de Vigência")').after('<span class="ms-accentText" title="Este é um campo obrigatório."> *</span>')
            $("select[title='Controle de Status'] option:contains('Em revisão')").remove();
            $("select[title='Controle de Status'] option:contains('Histórico')").remove();
            $("input[title='Consecutivo']").attr('disabled', 'disabled');
            $("input[title='Revisão Campo Obrigatório']").attr('disabled', 'disabled');
            $("input[title='Data Aprovação']").closest("table").closest("tr").hide();
            $("textarea[title='Justificativa Cancelamento']").closest("tr").hide();
            $("textarea[title='Comentários']").closest("tr").show();
            $("textarea[title='Comentários']").attr('disabled', 'disabled');
            $("input[title='Data Limite Comentário']").attr('disabled', 'disabled');
            $("input[title='Data Limite Comentário']").closest("tr").find("td:eq(1)").hide();
            if ($("input[title='Existe Comentários']").val() == "S") {
                $($(".s4-wpcell-plain")[0]).after("<div id='comentatioFaixa' style='font-style:italic;margin-bottom:10px;border:1px solid #e3474a;background-color:#f1a2a48f;padding-top:8px;padding-bottom:8px;padding-left:8px;margin-bottom:10px;color:#e3474a;border-radius:3px;'><span>Existem comentários para consolidação.</span></div>");
            }
            $($(".s4-wpcell-plain")[0]).after("<div id='Atualiza' style='font-style:italic;margin-bottom:10px;border:1px solid green; background-color:#98fb98;padding-top:8px;padding-bottom:8px;padding-left:8px;margin-bottom:10px;color:green;border-radius:3px;'><span>Para inserir uma nova versão do Manual Técnico basta ir em 'Anexar Arquivo' e escolher o arquivo.</span></div>");

            //var admin = false;
            SP.SOD.executeFunc('SP.js', 'SP.ClientContext', function () {
                CurrentUserMemberOfGroup("Administradores GGA", function (isCurrentUserInGroup) {
                    if (isCurrentUserInGroup) {
                        admin = true;
                    }
                });
                CurrentUserMemberOfGroup("Administradores GGA MTC", function (isCurrentUserInGroup) {
                    if (isCurrentUserInGroup) {
                        admin = true;
                    }
                });
            });

            setTimeout(function () {
                /*if(!admin){
	            	$("select[title='Controle de Status']").attr('disabled', 'disabled');
	            	$("input[title='Data Limite Comentário']").attr('disabled', 'disabled');
			        $("input[title='Data Limite Comentário']").closest("tr").find("td:eq(1)").hide();
				}*/
                if (ERevisao) {
                    SP.SOD.executeFunc("clientpeoplepicker.js", "SP.ClientContext", function () {
                        var ppDiv = $("div[title='Colaboradores']")[0];
                        var peoplePicker = SPClientPeoplePicker.SPClientPeoplePickerDict[ppDiv.id];
                        peoplePicker.SetEnabledState(false);
                        $('nobr:contains("Colaboradores")').closest("tr").find("td:eq(1)").find('.sp-peoplepicker-delImage').hide();

                        var ppDivA = $("div[title='Autores']")[0];
                        var peoplePickerA = SPClientPeoplePicker.SPClientPeoplePickerDict[ppDivA.id];
                        peoplePickerA.SetEnabledState(false);
                        $('nobr:contains("Autores")').closest("tr").find("td:eq(1)").find('.sp-peoplepicker-delImage').hide();
                    });
                    $("select[title='Tipo MTC']").attr('disabled', 'disabled');
                    $("select[title='Grupo']").attr('disabled', 'disabled');
                    $("select[title='Subgrupo']").attr('disabled', 'disabled');
                    $("select[title='Fabricante']").attr('disabled', 'disabled');
                    $("textarea[title='Justificativa Solicitação Campo Obrigatório']").attr('disabled', 'disabled');
                }
                $(".loading").hide(); $(".modal").hide();
            }, 2000);
        }
        else if ($("select[title='Controle de Status']").val() == "Em revisão") {
            $("select[title='Controle de Status'] option:contains('Histórico')").remove();
            $("select[title='Controle de Status'] option:contains('Em consolidação')").remove();
            $("select[title='Controle de Status'] option:contains('Aguardando Aprovação')").remove();
            $("textarea[title='Justificativa Cancelamento']").closest("tr").hide();
            $("input[title='Consecutivo']").attr('disabled', 'disabled');
            $("input[title='Revisão Campo Obrigatório']").attr('disabled', 'disabled');
            $("textarea[title='Comentários']").closest("tr").hide();
            $("input[title='Data Limite Comentário']").closest("table").closest("tr").hide();
            $("input[title='Data Aprovação']").closest("table").closest("tr").hide();
            $('nobr:contains("Revisores")').after('<span class="ms-accentText" id="span_revisores" title="Este é um campo obrigatório."> *</span>');
            $('nobr:contains("Aprovado por")').closest('tr').hide();
            $("[id='Ribbon.ListForm.Edit.Actions.AttachFile-Large']").show();
            SP.SOD.executeFunc('SP.js', 'SP.ClientContext', function () {
                CurrentUserMemberOfGroup("Administradores GGA", function (isCurrentUserInGroup) {
                    if (isCurrentUserInGroup) {
                        admin = true;
                    }
                });
                CurrentUserMemberOfGroup("Administradores GGA MTC", function (isCurrentUserInGroup) {
                    if (isCurrentUserInGroup) {
                        admin = true;
                    }
                });
            });

            //SP.SOD.executeFunc("clientpeoplepicker.js", "SP.ClientContext", function () {
            setTimeout(function () {
                $("select[title='Tipo MTC']").attr('disabled', 'disabled');
                $("select[title='Grupo']").attr('disabled', 'disabled');
                $("select[title='Subgrupo']").attr('disabled', 'disabled');
                $("select[title='Fabricante']").attr('disabled', 'disabled');
                //$("input[title='Data Aprovação']").closest("table").closest("tr").hide();
                $("input[title='Data Limite Comentário']").closest("tr").hide();

                $("textarea[title='Justificativa Solicitação Campo Obrigatório']").attr('disabled', 'disabled');
                $("div[title='Aprovado por']").closest("tr").hide();
                $("div[title='Palavra Chave']").addClass("ms-taxonomy-disabled");
                $("div[title='Palavra Chave']").parent().find('img').hide();
                $("div[title='Palavra Chave']").children().prop("contenteditable", false);
                $('nobr:contains("Comentários")').after('<span class="ms-accentText" title="Este é um campo obrigatório."> *</span>');
                SP.SOD.executeFunc("clientpeoplepicker.js", "SP.ClientContext", function () {
                    var ppDiv = $("div[title='Colaboradores']")[0];
                    var peoplePicker = SPClientPeoplePicker.SPClientPeoplePickerDict[ppDiv.id];
                    peoplePicker.SetEnabledState(false);
                    $('nobr:contains("Colaboradores")').closest("tr").find("td:eq(1)").find('.sp-peoplepicker-delImage').hide();

                    var ppDivA = $("div[title='Autores']")[0];
                    var peoplePickerA = SPClientPeoplePicker.SPClientPeoplePickerDict[ppDivA.id];
                    peoplePickerA.SetEnabledState(false);
                    $('nobr:contains("Autores")').closest("tr").find("td:eq(1)").find('.sp-peoplepicker-delImage').hide();
                });
                if (!admin)
                    $("select[title='Controle de Status']").attr('disabled', 'disabled');


                $(".loading").hide(); $(".modal").hide();
            }, 2000);
            //});
        }
    }
}

function PreSaveAction() {
    try {
        $('#ErroDataCustomizado').remove(); $('#ErroDataCustomizado1').remove(); $('#ErroDataCustomizado2').remove(); $('#ErroDataCustomizado3').remove();
        $('#ErroDataCustomizado4').remove(); $('#ErroDataCustomizado5').remove(); $('#ErroDataCustomizado6').remove(); $('#ErroDataCustomizado7').remove();
        $('#ErroDataCustomizado8').remove(); $('#ErroDataCustomizado9').remove(); $('#ErroDataCustomizado10').remove();
        $('#ErroDataCustomizado11').remove(); $('#ErroDataCustomizado12').remove(); $('#ErroDataCustomizado13').remove();
        $('#ErroDataCustomizado14').remove(); $('#ErroDataCustomizado15').remove(); $('#ErroDataCustomizado16').remove(); $('#ErroDataCustomizado17').remove();
        $('#ErroDataCustomizado18').remove(); $('#ErroDataCustomizado19').remove(); $('#ErroDataCustomizado20').remove();
    }
    catch (ex) { }

    var retorno = true;

    if ($("select[title='Tipo de Conteúdo'] option:selected").text() == "Legado") {
    }
    else {
        if ($("textarea[title='Descrição do MTC Campo Obrigatório']").val() == "") {
            $("textarea[title='Descrição do MTC Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado1' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
            retorno = false;//$("textarea[title='Descrição do MTC Campo Obrigatório']").focus();
        }

        if ($("textarea[title='Objetivo Campo Obrigatório']").val() == "") {
            $("textarea[title='Objetivo Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado18' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
            retorno = false;//$("textarea[title='Descrição do MTC Campo Obrigatório']").focus();
        }

        if ($("select[title='Órgão Responsável Campo Obrigatório']").val() == "0") {
            $("select[title='Órgão Responsável Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado14' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Selecione uma opção.</span>");
            retorno = false;//$("input[title='Palavra Chave Campo Obrigatório']").focus();
        }

        if ($("select[title='Órgão Responsável Campo Obrigatório']").val() == "Selecione uma opção") {
            $("select[title='Órgão Responsável Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado2' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Selecione uma opção.</span>");
            retorno = false;//$("select[title='Órgão Responsável Campo Obrigatório']").focus();
        }

        /*var autores = $("div[title='Autores Campo Obrigatório']").children()[0].value;
        if (autores == "") {
            $('nobr:contains("Autores")').closest('tr').find('div.sp-peoplepicker-topLevel').closest("td").append("<span id='ErroDataCustomizado4' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Esse campo não pode ficar em branco.</span>");
            retorno = false;
        }*/

        /*if ($("input[title='Prazo de Vigência (anos) Campo Obrigatório']").val() == "") {
            $("input[title='Prazo de Vigência (anos) Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado5' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
            retorno = false;//$("textarea[title='Descrição do MTC Campo Obrigatório']").focus();
        }
        else {*/
        if ($("input[title='Prazo de Vigência (anos)']").val() != "") {
            if (parseInt($("input[title='Prazo de Vigência (anos)']").val()) < 1) {
                $("input[title='Prazo de Vigência (anos)']").closest("td").append("<span id='ErroDataCustomizado5' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Esse campo tem que ser maior que 0.</span>");
                retorno = false;//$("textarea[title='Descrição do MTC Campo Obrigatório']").focus();
            }
        }

        if ($("textarea[title='Justificativa Solicitação Campo Obrigatório']").val() == "") {
            $("textarea[title='Justificativa Solicitação Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado3' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
            retorno = false;//$("textarea[title='Descrição do MTC Campo Obrigatório']").focus();
        }

        if ($("input[title='Data de Início de Vigência']").val() != "") {
            if (!isDate($("input[title='Data de Início de Vigência']").val())) {
                $($("input[title='Data de Início de Vigência']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado9' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
                retorno = false;
            }
        }

        //if(retorno){
        if ($("select[title='Controle de Status']").val() == "Cancelado") {
            if ($("textarea[title='Justificativa Cancelamento']").val() == "") {
                alert("É preciso preencher a justificativa de cancelamento.");
                $("textarea[title='Justificativa Cancelamento']").closest("td").append("<span id='ErroDataCustomizado6' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
                $("textarea[title='Justificativa Cancelamento']").focus();
                retorno = false;
            }
        }
        else if ($("select[title='Controle de Status']").val() == "Para comentários") {
            if ($("input[title='Data Limite Comentário']").val() == "" || !isDate($("input[title='Data Limite Comentário']").val())) {
                $($("input[title='Data Limite Comentário']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado7' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
                $("input[title='Data Limite Comentário']").focus();
                retorno = false;
            }
            else if ($("input[title='Data Limite Comentário']").val() != "") {
                var dtIni = $("input[title='Data Limite Comentário']").val();
                var dtFim = new Date().toLocaleDateString();
                if (navigator.appName == "Microsoft Internet Explorer") {
                    var arrStartDate = dtIni.split("/");
                    var date1 = new Date(arrStartDate[2], arrStartDate[1], arrStartDate[0]);
                    if (new Date(date1) < dtFim) {
                        $($("input[title='Data Limite Comentário']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado7' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Data Limite Comentário deve ser maior que a Data de hoje.</span>");
                        $("input[title='Data Limite Comentário']").focus();
                        retorno = false;
                    }
                }
                else {
                    var data1 = dtIni.replace(/(\d+)\/(\d+)\/(\d+)/, "$3/$2/$1");
                    var dataDtInicio = new Date(data1);

                    if (dataDtInicio.getTime() < new Date().getTime()) {//toLocaleDateString()
                        $($("input[title='Data Limite Comentário']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado7' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Data Limite Comentário deve ser maior que a Data de hoje.</span>");
                        //alert("Data Fim deve ser maior que a Data Início");
                        $("input[title='Data Limite Comentário']").focus();
                        //return false;
                        retorno = false;
                    }
                }
            }
            if (!admin) {
                if ($("textarea[title='Comentários']").val() == "") {
                    //alert("É preciso preencher o campo Comentário para salvar.");
                    $("textarea[title='Comentários']").closest("td").append("<span id='ErroDataCustomizado6' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
                    $("textarea[title='Comentários']").focus();
                    retorno = false;
                }
            }
            if ($("textarea[title='Comentários']").val() != "") {
                $("input[title='Existe Comentários']").val("S");
            }
            if (ERevisao) {
                var revisores = $("div[title='Revisores']").children()[0].value;
                if (revisores == "" || revisores == "[]") {
                    $('nobr:contains("Revisores")').closest('tr').find('div.sp-peoplepicker-topLevel').closest("td").append("<span id='ErroDataCustomizado19' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Esse campo não pode ficar em branco.</span>");
                    retorno = false;
                }
            }
        }
        else if ($("select[title='Controle de Status']").val() == "Em consolidação") {
            if (document.getElementById("idAttachmentsTable").rows.length > 1) {
                alert("É permitido apenas 1 arquivo de Manual Técnico de campo.");
                retorno = false;
            }
            else if (document.getElementById("idAttachmentsTable").rows.length > 0) {
                if (document.getElementById("idAttachmentsTable").innerText.match("\.doc|\.docx") == null) {
                    alert("O documento anexado deverá estar no formato word.");
                    retorno = false;
                }
            }

            if (retorno) {
                if (statusConsolidacao == "Em consolidação" && $("select[title='Controle de Status']").val() == "Em consolidação") {
                    $("select[title='Controle de Status']").val("Aguardando Aprovação");
                }
            }
        }
        else if ($("select[title='Controle de Status']").val() == "Vigente") {
            if ($("select[title='Tipo MTC']").val() == "0") {
                $("select[title='Tipo MTC']").closest("td").append("<span id='ErroDataCustomizado10' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Selecione uma opção.</span>");
                retorno = false; $("select[title='Tipo MTC']").focus();
            }

            if ($("select[title='Grupo']").val() == "0") {
                $("select[title='Grupo']").closest("td").append("<span id='ErroDataCustomizado11' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Selecione uma opção.</span>");
                retorno = false; $("select[title='Grupo']").focus();
            }

            if ($("select[title='Subgrupo']").val() == "0") {
                $("select[title='Subgrupo']").closest("td").append("<span id='ErroDataCustomizado12' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Selecione uma opção.</span>");
                retorno = false; $("select[title='Subgrupo']").focus();
            }

            if ($("select[title='Fabricante']").val() == "0") {
                $("select[title='Fabricante']").closest("td").append("<span id='ErroDataCustomizado13' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Selecione uma opção.</span>");
                retorno = false; $("select[title='Fabricante']").focus();
            }

            if ($("input[title='Consecutivo']").val() == "") {
                $("input[title='Consecutivo']").closest("td").append("<span id='ErroDataCustomizado16' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
                $("input[title='Consecutivo']").focus();
                retorno = false;
            }//consecutivoAciona 
            else if (!consecutivoAciona && !ERevisao) {
                $("input[title='Consecutivo']").closest("td").append("<span id='ErroDataCustomizado16' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Atualize o número do Consecutivo.</span>");
                $("input[title='Consecutivo']").focus();
                retorno = false;
            }

            var autores = $("div[title='Autores']").children()[0].value;
            if (autores == "" || autores == "[]") {
                $('nobr:contains("Autores")').closest('tr').find('div.sp-peoplepicker-topLevel').closest("td").append("<span id='ErroDataCustomizado4' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Esse campo não pode ficar em branco.</span>");
                retorno = false;
            }

            if (ERevisao) {
                var revisores = $("div[title='Revisores']").children()[0].value;
                if (revisores == "" || revisores == "[]") {
                    $('nobr:contains("Revisores")').closest('tr').find('div.sp-peoplepicker-topLevel').closest("td").append("<span id='ErroDataCustomizado19' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Esse campo não pode ficar em branco.</span>");
                    retorno = false;
                }
            }

            if (!isDate($("input[title='Data de Início de Vigência']").val())) {
                $($("input[title='Data de Início de Vigência']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado9' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
                retorno = false;
            }

            if (parseInt($("input[title='Prazo de Vigência (anos)']").val()) < 1 || $("input[title='Prazo de Vigência (anos)']").val() == "") {
                $("input[title='Prazo de Vigência (anos)']").closest("td").append("<span id='ErroDataCustomizado5' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Esse campo tem que ser maior que 0.</span>");
                retorno = false;//$("textarea[title='Descrição do MTC Campo Obrigatório']").focus();
            }

            if (document.getElementById("idAttachmentsTable").rows.length > 1) {
                alert("É permitido apenas 1 arquivo de Manual Técnico de campo.");
                retorno = false;
            }
            else if (document.getElementById("idAttachmentsTable").rows.length > 0) {
                if (document.getElementById("idAttachmentsTable").innerText.match("\.doc|\.docx") == null) {
                    alert("O documento anexado deverá estar no formato word.");
                    retorno = false;
                }
            }
        }
        else if ($("select[title='Controle de Status']").val() == "Em revisão") {
            /*if(!isDate($("input[title='Data de Início de Vigência']").val())){
                $($("input[title='Data de Início de Vigência']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado17' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
                retorno = false;
            }*/
            if (ERevisao) {
                var revisores = $("div[title='Revisores']").children()[0].value;
                if (revisores == "" || revisores == "[]") {
                    $('nobr:contains("Revisores")').closest('tr').find('div.sp-peoplepicker-topLevel').closest("td").append("<span id='ErroDataCustomizado19' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Esse campo não pode ficar em branco.</span>");
                    retorno = false;
                }
            }
            if (document.getElementById("idAttachmentsTable").rows.length > 1) {
                alert("É permitido apenas 1 arquivo de Manual Técnico de campo.");
                retorno = false;
            }
            else if (document.getElementById("idAttachmentsTable").rows.length > 0) {
                if (document.getElementById("idAttachmentsTable").innerText.match("\.doc|\.docx") == null) {
                    alert("O documento anexado deverá estar no formato word.");
                    retorno = false;
                }
            }
        }
        //}
    }

    return retorno;
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

function FiltraSubgrupo() {
    var urlQuery = _spPageContextInfo.webAbsoluteUrl + "/_vti_bin/listdata.svc/SubgrupoMTC?$filter=CódigoDoGrupoId eq " + $("select[title='Grupo']").val();
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
                //$("select[title='Subgrupo']").prepend("<option value='0' selected='selected'>Selecione uma opção</option>");
                $.each(data.d.results, function (i, item) {
                    $("select[title='Subgrupo']").append('<option value="' + item.ID + '">' + item.DescriçãoSubgrupo + '</option>');
                });
            }
        }
    });
}

function FiltraSubgrupoSelected(sgrupo) {
    $("select[title='Subgrupo']").empty();
    var urlQuery = _spPageContextInfo.webAbsoluteUrl + "/_vti_bin/listdata.svc/SubgrupoMTC?$filter=CódigoDoGrupoId eq " + $("select[title='Grupo']").val();
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
                //$("select[title='Subgrupo']").prepend("<option value='0' selected='selected'>Selecione uma opção</option>");
                $.each(data.d.results, function (i, item) {
                    if (sgrupo == item.ID)
                        $("select[title='Subgrupo']").append('<option selected="selected" value="' + item.ID + '">' + item.DescriçãoSubgrupo + '</option>');
                    else
                        $("select[title='Subgrupo']").append('<option value="' + item.ID + '">' + item.DescriçãoSubgrupo + '</option>');
                });
            }
        }
    });
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