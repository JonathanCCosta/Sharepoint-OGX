$(document).ready(function () {

    try {
        $("#NumSolicitacao").hide();
        $("#NumSolicitacao").val(getQueryString("ID"));
    }
    catch (ex) { }

    $('#btSalvar').on('click', function () {
        if ($("textarea[id*='justificativa']").val() == "") {
            alert("Preencha a justificativa para continuar!");
            return false;
        }
        else
            ShowProgress();
    });

    $("#s4-workspace").css("overflow-x", "hidden");
});

function CloseFormCancel() {
    window.frameElement.cancelPopUp();
    return false;
}

function CloseFormOK() {
    window.frameElement.commitPopup();
    return false;
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
    var qs = decodeURIComponent(parent.location.search.substring(1));
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
