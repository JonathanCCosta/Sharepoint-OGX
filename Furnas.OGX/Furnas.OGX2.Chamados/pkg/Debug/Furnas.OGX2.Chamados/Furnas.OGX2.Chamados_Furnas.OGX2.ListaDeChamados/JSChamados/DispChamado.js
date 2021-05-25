$(document).ready(function () {
    SP.SOD.executeFunc('SP.js', 'SP.ClientContext', function () {
        CurrentUserMemberOfGroup("Administradores GGA", function (isCurrentUserInGroup) {
            if (!isCurrentUserInGroup) {
                $('td.ms-formlabel:contains("Prazo")').parent().hide();
                $('td.ms-formlabel:contains("Responsável")').parent().hide();
                $('td.ms-formlabel:contains("Enviar")').parent().hide();
                $('td.ms-formlabel:contains("Importância")').parent().hide();
                $('td.ms-formlabel:contains("Complexidade")').parent().hide();
                if ($("td.ms-formlabel:contains('Esclarecimento Solicitante')").closest("tr").find("td:eq(1)").text().trim() == "Não há entradas existentes.")
                    $('td.ms-formlabel:contains("Esclarecimentos")').parent().hide();
            }
            else {
                if ($("td.ms-formlabel:contains('Esclarecimentos')").closest("tr").find("td:eq(1)").text().trim() == "Não há entradas existentes.")
                    $('td.ms-formlabel:contains("Esclarecimentos")').parent().hide();
            }
        });
    });

    if ($("select[title='Controle de status Campo Obrigatório'] option:selected").text() === "Aguardando Informações"
      || $("select[title='Controle de status Campo Obrigatório'] option:selected").text() === "Encaminhado à outra gerência") {
        $("#Esclarecimentos nobr").text("Esclarecimento Solicitante");
        $("#EsclarecimentosGerencia nobr").text("Esclarecimento Outra Gerência");

    }
});

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
