$(document).ready(function () {

    ConfiguraCampos();

    $("select[title='Modelo de Chamado Campo Obrigatório']").empty();
    $("select[title='Modelo de Chamado Campo Obrigatório']").prepend("<option value='0' selected='selected'>Selecione uma opção</option>");

    $("select[title='Tipo de Chamado Campo Obrigatório']").change(function () {
        //ShowProgress();
        var arr = FiltraTipoModelo();

        $("select[title='Modelo de Chamado Campo Obrigatório']").empty();
        $("select[title='Modelo de Chamado Campo Obrigatório']").prepend("<option value='0' selected='selected'>Selecione uma opção</option>");
        $.each(arr, function (i, item) {
            $("select[title='Modelo de Chamado Campo Obrigatório']").append('<option value="' + item.ID + '">' + item.ModeloDeChamado + '</option>');
        });

        $("#modelosGrade").hide(); $("#downloadMsg").remove();
        $("#modelosGradeAntes").show();

        //LoadModelos();
    });

    $("select[title='Modelo de Chamado Campo Obrigatório']").change(function () {
        LoadModelos();
    });



    //SPRINT 7
    /* 28880 */
    /* Nas telas de inclusão e alteração do chamado, nos status: 
    Encaminhado para outra Gerência e Aguardando Informação. Alterar o nome dos campos da seguinte forma:
    "Esclarecimento" para "Esclarecimento Solicitante"
    "Esclarecimento da Gerência" para "Esclarecimento Outra Gerência"*/

    /*if ($("select[title='Controle de status Campo Obrigatório'] option:selected").text() === "Aguardando Informações"
        || $("select[title='Controle de status Campo Obrigatório'] option:selected").text() === "Encaminhado à outra gerência") {
        $("#Esclarecimentos nobr").text("Esclarecimento Solicitante");
        $("#EsclarecimentosGerencia nobr").text("Esclarecimento Outra Gerência");*/

    

   
});

function PreSaveAction() {
    var retorno = true;

    try {
        $('#ErroDataCustomizado1').remove(); $('#ErroDataCustomizado2').remove(); $('#ErroDataCustomizado3').remove();
    }
    catch (ex) { }

    if ($("select[title='Tipo de Chamado Campo Obrigatório']").val() == "0") {
        $("select[title='Tipo de Chamado Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado1' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Selecione uma opção.</span>");
        retorno = false;
    }

    if ($("select[title='Modelo de Chamado Campo Obrigatório']").val() == "0") {
        $("select[title='Modelo de Chamado Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado2' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Selecione uma opção.</span>");
        retorno = false;
    }

    var full = $('nobr:contains("Descrição")').closest('tr').find('div.ms-rtestate-write').text();
    if (full.length <= 1) {
        $('nobr:contains("Descrição")').closest('tr').find('div.ms-rtestate-write').closest("td").append("<span id='ErroDataCustomizado3' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
        retorno = false;
    }

    if (retorno) {
        var urlQuery = _spPageContextInfo.webServerRelativeUrl + "/_vti_bin/listdata.svc/ModeloDeChamado?$filter=ID eq " + $("select[title='Modelo de Chamado Campo Obrigatório']").val();
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
                    if (data.d.results[0].ObrigatórioValue == "Sim") {
                        if (document.getElementById("idAttachmentsTable").rows.length == 0) {
                            //$("select[title='Modelo de Chamado Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado2' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>O preenchimento e anexo do modelo é obrigatório.</span>");
                            alert("O preenchimento e anexo do modelo é obrigatório.");
                            retorno = false;
                        }
                    }
                }
            }
        });

        if (retorno) {
            $("input[title='Ano']").val(new Date().getFullYear());
            //$("input[title='Imediato do Responsável']").val("lmotta@furnas.com.br;cmota@furnas.com.br");//("jonathan@furnas.com.br");
            //$("input[title='Número do chamado Campo Obrigatório']").val(GetLastChamado());
            $("input[title='Número do chamado Campo Obrigatório']").val(ConsultaChamados());
        }
    }

    return retorno;

}

function FiltraTipoModelo() {
    var ar = [];
    var query = _spPageContextInfo.webAbsoluteUrl + "/_vti_bin/listdata.svc/ModeloDeChamado?$filter=" + "TipoDeChamadoId eq " + $("select[title='Tipo de Chamado Campo Obrigatório']").val();
    $.ajax({
        type: "GET",
        contentType: "application/json;charset=ISO-8859-1",
        url: encodeURI(query),
        cache: false,
        async: false,
        dataType: 'json',
        headers: { "Accept": "application/json; odata=verbose" },
        success: function (data) {
            ar = data.d.results;
        }
    });

    return ar;
}

function ConfiguraCampos() {
    $("input[title='Ano']").closest("tr").hide();
    $("input[title='Imediato do Responsável']").closest("tr").hide();
    $("select[title='Controle de status Campo Obrigatório']").attr('disabled', 'disabled');
    $("input[title='Número do chamado Campo Obrigatório']").closest("tr").hide();
    $('td.ms-formlabel:contains("Descrição")').closest('td').next('td')[0].width = '600px';

    $("select[title='Tipo de Chamado Campo Obrigatório']").prepend("<option value='0' selected='selected'>Selecione uma opção</option>");
    $("select[title='Modelo de Chamado Campo Obrigatório']").prepend("<option value='0' selected='selected'>Selecione uma opção</option>");

    var currentUserRequest = $.ajax({
        url: _spPageContextInfo.webAbsoluteUrl + '/_api/web/currentuser',
        method: 'GET',
        headers: {
            "accept": "application/json;odata=verbose",
            "content-type": "application/json;odata=verbose"
        }
    });
    currentUserRequest.done(function (result) {
        GetOrgao(result.d.LoginName.split('|')[1].replace(/[^0-9]/gi, ''));
    });

    $("textarea[title='Órgão']").attr('disabled', 'disabled');
    //$("textarea[title='Descrição do chamado Campo Obrigatório']").css('width', '500px');

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


/*function PreSaveAction() {
    $("input[title='Número do chamado Campo Obrigatório']").val(GetLastChamado());
    return true;
}*/

function GetLastChamado() {
    var ID = 0;
    var query = _spPageContextInfo.webAbsoluteUrl + "/_vti_bin/listdata.svc/Chamados?$top=1&$filter=Ano eq '" + new Date().getFullYear() + "' &$select=ID&$orderby=DataDaSolicitação desc";
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

        return "GGA.O.0" + seqFormat + "." + new Date().getFullYear();
    }
    else
        return "GGA.O.00001." + new Date().getFullYear();
}

function ConsultaChamados() {
    var ID = 0;
    var query = _spPageContextInfo.webAbsoluteUrl + "/_vti_bin/listdata.svc/ContadorChamado";
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
                ID = parseInt(data.d.results[0].Título);
            }
        }
    });
    if (ID > 0) {
        var seq = ID + 1;
        var seqFormat = seq.toString();

        if (seqFormat.length == 1)
            seqFormat = "000" + seqFormat;
        else if (seqFormat.length == 2)
            seqFormat = "00" + seqFormat;
        else if (seqFormat.length == 3)
            seqFormat = "0" + seqFormat;

        return "GGA.O.0" + seqFormat + "." + new Date().getFullYear();
    }
    else
        return "GGA.O.00001." + new Date().getFullYear();
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

function LoadModelos() {
    //var urlQuery = _spPageContextInfo.webServerRelativeUrl + "/_vti_bin/listdata.svc/ModeloDeChamado?$filter=TipoDeChamadoId eq " + $("select[title='Modelo de Chamado Campo Obrigatório']").val();
    var urlQuery = _spPageContextInfo.webServerRelativeUrl + "/_vti_bin/listdata.svc/ModeloDeChamado?$filter=ID eq " + $("select[title='Modelo de Chamado Campo Obrigatório']").val();

    var retornoHTML = [];
    $.getJSON(urlQuery,
        function (data) {
            if (data.d.results.length > 0) {
                /*retornoHTML.push('<div class="modelos"><span>');
                retornoHTML.push('<h3>Modelos de Chamado para Download</h3>');
                retornoHTML.push('</span>');*/
                $("#modelosGrade").hide(); $("#downloadMsg").remove();

                $.each(data.d.results, function (i, item) {
                    var fileType = GetFileTypeImage(item.__metadata.media_src.split('.').pop());
                    retornoHTML.push('<div class="itemModelo">');
                    retornoHTML.push('<a href="' + item.__metadata.media_src + '">');
                    retornoHTML.push('<img src="' + fileType + '" alt="' + item.ModeloDeChamado + '" />');//</span>');
                    retornoHTML.push('<h2>' + item.Nome + '</h2></a>');
                    retornoHTML.push('</div></div>');
                });

                document.getElementById('modelosGrade').innerHTML = retornoHTML.join('');
                $("#modelosGrade").show(); $("#modelosGradeAntes").hide();

                if (data.d.results.length > 1) {
                    var porcent = 100 / data.d.results.length;
                    var sporcent = porcent.toString() + "%";
                    $(".itemModelo").css('width', sporcent);
                }
            }
            else {
                //document.getElementById('containerModelos').innerHTML = "";
                $("#modelosGrade").hide(); $("#downloadMsg").remove();
                $("#modelosGradeAntes").show();
            }
        });
}

function GetFileTypeImage(ext) {
    if (ext == "xls" || ext == "xlsx")
        return "/_layouts/15/images/lg_icxlsx.png";
    else if (ext == "pdf")
        return "/_layouts/15/images/icpdf.png";
    else
        return "/_layouts/15/images/icjpg.gif";
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