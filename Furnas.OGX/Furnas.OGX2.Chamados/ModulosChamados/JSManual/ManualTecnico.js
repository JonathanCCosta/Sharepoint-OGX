$(document).ready(function () {

    $("input[title='Código MTC']").closest("tr").hide();

    var param = getQueryString("ContentTypeId");
    if (param == "0x01008681C2F7D7664E3B8311169DE3B5D48E003A1D4FB5641BB342BB33520358A181F0") {
        $("select[title='Órgão Responsável Campo Obrigatório']").prepend("<option value='0' selected='selected'>Selecione uma opção</option>");
        $("select[title='Órgão Responsável Campo Obrigatório']").attr("style", "max-width:398px!important;");

        $("select[title='Tipo MTC Campo Obrigatório']").prepend("<option value='0' selected='selected'>Selecione uma opção</option>");

        $("select[title='Grupo Campo Obrigatório']").prepend("<option value='0' selected='selected'>Selecione uma opção</option>");
        $("select[title='Subgrupo Campo Obrigatório']").empty();
        $("select[title='Subgrupo Campo Obrigatório']").prepend("<option value='0' selected='selected'>Selecione uma opção</option>");
        $("select[title='Fabricante Campo Obrigatório']").prepend("<option value='0' selected='selected'>Selecione uma opção</option>");
        $("select[title='Orgão Responsável Campo Obrigatório']").prepend("<option value='0'  selected='selected>Selecione uma opção</option>");

        $("select[title='Controle de Status Campo Obrigatório'] option[value=]").remove();
        $("select[title='Órgão Responsável Campo Obrigatório'] option[value=]").remove();

        $("input[title='Consecutivo Campo Obrigatório'").attr('placeholder', '00');

        $("input[title='Revisão Campo Obrigatório'").attr('placeholder', '00');

        $("input[title='Consecutivo Campo Obrigatório'").attr('onkeypress', 'return event.charCode >= 48 && event.charCode <= 57');

        $("input[title='Revisão Campo Obrigatório'").attr('onkeypress', 'return event.charCode >= 48 && event.charCode <= 57');

        $("input[title='Consecutivo Campo Obrigatório'").attr('maxlength', '2');
        $("input[title='Revisão Campo Obrigatório'").attr('maxlength', '2');

        $("select[title='Controle de Status Campo Obrigatório'] option:contains('Vigente')").attr('selected', true);

    }
    else {//solicitacao
        $("select[title='Órgão Responsável Campo Obrigatório']").prepend("<option value='0' selected='selected'>Selecione uma opção</option>");
        $("select[title='Órgão Responsável Campo Obrigatório']").attr("style", "max-width:398px!important;");
        $("select[title='Tipo MTC'] option:contains('(Nenhum)')").remove();
        $("select[title='Tipo MTC']").prepend("<option value='0' selected='selected'>Selecione uma opção</option>");
        $("select[title='Grupo']").prepend("<option value='0' selected='selected'>Selecione uma opção</option>");
        $("select[title='Grupo'] option:contains('(Nenhum)')").remove();
        $("select[title='Subgrupo']").empty();
        $("select[title='Subgrupo']").prepend("<option value='0' selected='selected'>Selecione uma opção</option>");
        $("select[title='Fabricante'] option:contains('(Nenhum)')").remove();
        $("select[title='Fabricante']").prepend("<option value='0' selected='selected'>Selecione uma opção</option>");
        $("select[title='Controle de Status Campo Obrigatório']").closest("tr").hide();//.attr('disabled', 'disabled');
        $("select[title='Em Revisão']").closest("tr").hide();
        $("div[title='Palavra Chave']").closest("tr").hide();
    }

    $("input[title='Prazo de Vigência (anos) Campo Obrigatório']").keypress(function (e) {
        if ((e.which < 48 || e.which > 57)) {
            return false;
        }
    });


    $("select[title='Grupo']").change(function () {
        $("select[title='Subgrupo']").empty();
        $("select[title='Subgrupo']").prepend("<option value='0' selected='selected'>Selecione uma opção</option>");
        HillbillyCascade({
            parentFormField: "Grupo", //Display name on form of field from parent list
            childList: "Subgrupo MTC", //List name of child list
            childLookupField: "DescricaoSubgrupo", //Internal field name in Child List used in lookup
            childFormField: "Subgrupo", //Display name on form of the child field
            parentFieldInChildList: "Grupo", //Internal field name in Child List of the parent field
            /*firstOptionText: "Selecione uma opção"*/
        });
    });

    $("select[title='Grupo Campo Obrigatório']").change(function () {
        $("select[title='Subgrupo Campo Obrigatório']").empty();
        $("select[title='Subgrupo Campo Obrigatório']").prepend("<option value='0' selected='selected'>Selecione uma opção</option>");
        HillbillyCascade({
            parentFormField: "Grupo", //Display name on form of field from parent list
            childList: "Subgrupo MTC", //List name of child list
            childLookupField: "DescricaoSubgrupo", //Internal field name in Child List used in lookup
            childFormField: "Subgrupo", //Display name on form of the child field
            parentFieldInChildList: "Grupo", //Internal field name in Child List of the parent field
            /*firstOptionText: "Selecione uma opção"*/
        });
    });

});

function PreSaveAction() {
    try {
        $('#ErroDataCustomizado').remove(); $('#ErroDataCustomizado1').remove(); $('#ErroDataCustomizado2').remove(); $('#ErroDataCustomizado3').remove();
        $('#ErroDataCustomizado4').remove(); $('#ErroDataCustomizado5').remove(); $('#ErroDataCustomizado6').remove(); $('#ErroDataCustomizado7').remove();
        $('#ErroDataCustomizado8').remove(); $('#ErroDataCustomizado9').remove(); $('#ErroDataCustomizado10').remove();
        $('#ErroDataCustomizado11').remove(); $('#ErroDataCustomizado12').remove(); $('#ErroDataCustomizado13').remove();
        $('#ErroDataCustomizado14').remove(); $('#ErroDataCustomizado15').remove();
    }
    catch (ex) { }

    var retorno = true;

    var param = getQueryString("ContentTypeId");
    if (param == "0x01008681C2F7D7664E3B8311169DE3B5D48E003A1D4FB5641BB342BB33520358A181F0") {

        if ($("select[title='Tipo MTC Campo Obrigatório']").val() == "0") {
            $("select[title='Tipo MTC Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado1' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Selecione uma opção.</span>");
            retorno = false;//$("select[title='Tipo MTC Campo Obrigatório']").focus();
        }

        if ($("select[title='Grupo Campo Obrigatório']").val() == "0") {
            $("select[title='Grupo Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado2' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Selecione uma opção.</span>");
            retorno = false;//$("select[title='Grupo Campo Obrigatório']").focus();
        }

        if ($("select[title='Subgrupo Campo Obrigatório']").val() == "0") {
            $("select[title='Subgrupo Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado3' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Selecione uma opção.</span>");
            retorno = false;//$("select[title='Subgrupo Campo Obrigatório']").focus();
        }

        if ($("select[title='Fabricante Campo Obrigatório']").val() == "0") {
            $("select[title='Fabricante Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado4' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Selecione uma opção.</span>");
            retorno = false;//$("select[title='Fabricante Campo Obrigatório']").focus();
        }

        if ($("textarea[title='Descrição do MTC Campo Obrigatório']").val() == "") {
            $("textarea[title='Descrição do MTC Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado5' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
            retorno = false;//$("textarea[title='Descrição do MTC Campo Obrigatório']").focus();
        }

        if ($("select[title='Órgão Responsável Campo Obrigatório']").val() == "0") {
            $("select[title='Órgão Responsável Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado6' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Selecione uma opção.</span>");
            retorno = false;//$("select[title='Órgão Responsável Campo Obrigatório']").focus();
        }

        var dt = $("input[title='Data de Início de Vigência Campo Obrigatório']").val();
        if (!isDate(dt)) {
            $($("input[title='Data de Início de Vigência Campo Obrigatório']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado7' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
            retorno = false; //$("input[title='Data de Início de Vigência Campo Obrigatório']").focus();
        }

        if ($("select[title='Controle de Status Campo Obrigatório']").val() == "0") {
            $("select[title='Controle de Status Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado8' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Selecione uma opção.</span>");
            retorno = false;
        }

        if ($("input[title='Prazo de Vigência (anos) Campo Obrigatório']").val() == "") {
            $("input[title='Prazo de Vigência (anos) Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado9' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
            retorno = false;//$("input[title='Prazo de Vigência (anos) Campo Obrigatório']").focus();
        }
        else {
            if (parseInt($("input[title='Prazo de Vigência (anos) Campo Obrigatório']").val()) < 1) {
                $("input[title='Prazo de Vigência (anos) Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado10' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Esse campo tem que ser maior que 0.</span>");
                retorno = false;//$("input[title='Prazo de Vigência (anos) Campo Obrigatório']").focus();
            }
        }

        if ($("input[title='Consecutivo Campo Obrigatório']").val() == "") {
            $("input[title='Consecutivo Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado11' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
            retorno = false;
        }
        else if ($("input[title='Consecutivo Campo Obrigatório']").val().length < 2) {
            $("input[title='Consecutivo Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado11' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>O campo deve conter 2 digitos.</span>");
            retorno = false;//$("input[title='Prazo de Vigência (anos) Campo Obrigatório']").focus();
        }

        if ($("input[title='Revisão Campo Obrigatório']").val() == "") {
            $("input[title='Revisão Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado13' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
            retorno = false;
        }

        var dt = $("input[title='Data Aprovação']").val();
        if (dt != "") {
            if (!isDate(dt)) {
                $($("input[title='Data Aprovação']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado13' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
                retorno = false; //$("input[title='Data de Início de Vigência Campo Obrigatório']").focus();
            }
        }

        var dt = $("input[title='Data Limite Comentário']").val();
        if (dt != "") {
            if (!isDate(dt)) {
                $($("input[title='Data Limite Comentário']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado12' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
                retorno = false; //$("input[title='Data de Início de Vigência Campo Obrigatório']").focus();
            }
        }

        if (retorno) {
            if (document.getElementById("idAttachmentsTable").rows.length == 0) {
                alert("O anexo do Manual Técnico de campo é obrigatório.");
                retorno = false;
            }
            else if (document.getElementById("idAttachmentsTable").rows.length > 1) {
                alert("É permitido apenas 1 arquivo de Manual Técnico de campo.");
                retorno = false;
            }
            else if (document.getElementById("idAttachmentsTable").innerText.match("\.doc|\.docx") == null) {
                alert("O documento anexado deverá estar no formato word.");
                retorno = false;
            }
        }

        return retorno;
    }
    else {//solicitação
        /*if ($("select[title='Tipo MTC']").val() == "0") {
	        $("select[title='Tipo MTC']").closest("td").append("<span id='ErroDataCustomizado10' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Selecione uma opção.</span>");
	        retorno = false;//$("select[title='Tipo MTC Campo Obrigatório']").focus();
	    }
	
	    if ($("select[title='Grupo']").val() == "0") {
	        $("select[title='Grupo']").closest("td").append("<span id='ErroDataCustomizado11' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Selecione uma opção.</span>");
	        retorno = false;//$("select[title='Tipo MTC Campo Obrigatório']").focus();
	    }
	
	    if ($("select[title='Subgrupo']").val() == "0") {
	        $("select[title='Subgrupo']").closest("td").append("<span id='ErroDataCustomizado12' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Selecione uma opção.</span>");
	        retorno = false;//$("select[title='Tipo MTC Campo Obrigatório']").focus();
	    }
	
	    if ($("select[title='Fabricante']").val() == "0") {
	        $("select[title='Fabricante']").closest("td").append("<span id='ErroDataCustomizado13' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Selecione uma opção.</span>");
	        retorno = false;//$("select[title='Tipo MTC Campo Obrigatório']").focus();
	    }*/

        if ($("textarea[title='Descrição do MTC Campo Obrigatório']").val() == "") {
            $("textarea[title='Descrição do MTC Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado1' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
            retorno = false;//$("textarea[title='Descrição do MTC Campo Obrigatório']").focus();
        }

        if ($("textarea[title='Objetivo Campo Obrigatório']").val() == "") {
            $("textarea[title='Objetivo Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado2' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
            retorno = false;//$("textarea[title='Descrição do MTC Campo Obrigatório']").focus();
        }


        if ($("select[title='Órgão Responsável Campo Obrigatório']").val() == "0") {
            $("select[title='Órgão Responsável Campo Obrigatório']").closest("td").append("<span id='ErroDataCustomizado14' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Selecione uma opção.</span>");
            retorno = false;//$("input[title='Palavra Chave Campo Obrigatório']").focus();
        }

        /*var autores = $("div[title='Autores Campo Obrigatório']").children()[0].value;
	    if (autores == "") {
	             $('nobr:contains("Autores")').closest('tr').find('div.sp-peoplepicker-topLevel').closest("td").append("<span id='ErroDataCustomizado4' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Esse campo não pode ficar em branco.</span>");
	             retorno = false;
	     }*/

        if ($("input[title='Data de Início de Vigência']").val() != "") {
            if (!isDate($("input[title='Data de Início de Vigência']").val())) {
                $($("input[title='Data de Início de Vigência']").closest("table")[0]).closest("td").append("<span id='ErroDataCustomizado9' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Insira uma data válida.</span>");
                retorno = false;
            }
        }

        /*if ($("input[title='Prazo de Vigência (anos)']").val() == "") {
	        $("input[title='Prazo de Vigência (anos)']").closest("td").append("<span id='ErroDataCustomizado5' class='ms-formvalidation ms-csrformvalidation' style='width:100%'>Você não pode deixar em branco.</span>");
	        retorno = false;//$("textarea[title='Descrição do MTC Campo Obrigatório']").focus();
	    }
	    else{*/
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

        if (retorno) {
            if (document.getElementById("idAttachmentsTable").rows.length == 0) {
                alert("O anexo do Manual Técnico de campo é obrigatório.");
                retorno = false;
            }
            else if (document.getElementById("idAttachmentsTable").rows.length > 1) {
                alert("É permitido apenas 1 arquivo de Manual Técnico de campo.");
                retorno = false;
            }
            else if (document.getElementById("idAttachmentsTable").innerText.match("\.doc|\.docx") == null) {
                alert("O documento anexado deverá estar no formato word.");
                retorno = false;
            }
        }

        return retorno;
    }
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

            res.sort(ordenarOrgaosResponsavies);
            data = res;

            $("select[title='Órgão Responsável Campo Obrigatório']").append("<option value='0' selected='selected'>Selecione uma opção</option>");
        },
        error: function (error) { console.log(error); }
    });

    $.each(data, function (i, item) {

        $("select[title='Órgão Responsável Campo Obrigatório']").append('<option value="' + item.orgao + '">' + item.orgao + '</option>');
    });

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

/* SPRINT 7 */
/* 28880 */
//Ordena os orgãos por ordem alfabética
function ordenarOrgaosResponsavies(a, b) {
    var comparacao = 0;

    if (a.orgao > b.orgao)
        comparacao = 1;

    else if (a.orgao < b.orgao)
        comparacao = -1;

    return comparacao;
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


function HillbillyCascade(params) {

    var parent = $("select[Title='" + params.parentFormField + "'], select[Title='" +
        //params.parentFormField + " Required Field']");
    params.parentFormField + " Campo Obrigatório']");

    /*$(parent).change(function () {
     DoHillbillyCascade(this.value, params);
 });*/

    var currentParent = $(parent).val();
    //if (currentParent != 0) {
    DoHillbillyCascade(currentParent, params);
    // }

}


function DoHillbillyCascade(parentID, params) {

    var child = $("select[Title='" + params.childFormField + "'], select[Title='" +
        //params.childFormField + " Required Field']," +
        params.childFormField + " Campo Obrigatório']," +
       "select[Title='" + params.childFormField + " possible values']");

    $(child).empty();
    var options = "";

    if (parentID != 0) {

        var call = $.ajax({
            url: _spPageContextInfo.webAbsoluteUrl + "/_api/Web/Lists/GetByTitle('" + params.childList +
                "')/items?$select=Id," + params.childLookupField + "," + params.parentFieldInChildList +
                "/Id&$expand=" + params.parentFieldInChildList + "/Id&$filter=" + params.parentFieldInChildList +
                "/Id eq " + parentID,
            type: "GET",
            dataType: "json",
            headers: {
                Accept: "application/json;odata=verbose"
            }

        });
        call.done(function (data, textStatus, jqXHR) {

            if (data.d.results.length > 0) {

                options += "<option value='0'>Selecione uma opção</option>";

                for (index in data.d.results) {

                    if (data.d.results[index].Id == parseInt(parentID)) {
                        options += "<option value='" + data.d.results[index].Id + "' selected='selected'>" +
                                data.d.results[index][params.childLookupField] + "</option>";

                    }

                    else {

                        options += "<option value='" + data.d.results[index].Id + "'>" +
                            data.d.results[index][params.childLookupField] + "</option>";

                    }
                }


                $(child).append(options);

            }
            else {
                options += "<option value='0'>Selecione uma opção</option>";
                $(child).append(options);

            }

        });
        call.fail(function (jqXHR, textStatus, errorThrown) {
            alert("Error retrieving information from list: " + params.childList + jqXHR.responseText);
            $(child).append(options);
        });

    }

    else {
        options += "<option value='0' selected='selected'>Selecione uma opção</option>";
        $(child).append(options);
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