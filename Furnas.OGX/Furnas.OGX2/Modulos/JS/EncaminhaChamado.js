$(document).ready(function () {

    var spans = document.getElementsByTagName("span");
    for (var i = 0; i < spans.length; i++) {
        if (spans[i].innerHTML == "novo item") {
            spans[i].innerHTML = "Abrir um Novo Chamado";
            break;
        }
    }

    var url = _spPageContextInfo.webAbsoluteUrl + "/SitePages/Chamados.aspx";
    $("#idHomePageNewItem").attr("href", "");
    $("#idHomePageNewItem").attr("onclick", "OpenDialog('" + url + "','Abrir Novo Chamado');return false;");

});

function OpenDialog(url, title) {
    var options = {
        url: url,
        width: 700,
        height: 350,
        title: title,
        dialogReturnValueCallback: Function.createDelegate(null, CloseDialogCallback)
    };
    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);
}

function CloseDialogCallback(dialogResult, returnValue) {
    //location.reload();
}
