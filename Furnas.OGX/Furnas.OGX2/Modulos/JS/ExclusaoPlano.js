﻿$(document).ready(function () {

    $("#btnUpload").click(function () {
        if ($("#ddlCiclo").val() == 0) { alert("Selecione um Ciclo e Tipo para a Exclusão!"); return false; }
        if ($("#ddlTipos").val() == 0) { alert("Selecione um Ciclo e Tipo para a Exclusão!"); return false; }
       
        if (confirm("Deseja continuar a exclusão?")) {
            ShowProgress();
            return true;
        }

        return false;
        
    });
});

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