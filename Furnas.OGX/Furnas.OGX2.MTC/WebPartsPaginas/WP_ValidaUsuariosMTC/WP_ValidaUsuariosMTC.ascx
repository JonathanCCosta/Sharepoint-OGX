<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WP_ValidaUsuariosMTC.ascx.cs" Inherits="Furnas.OGX2.MTC.WebPartsPaginas.WP_ValidaUsuariosMTC.WP_ValidaUsuariosMTC" %>


<style type="text/css">  
/*hr[class="ms-core-menu-separatorHr"]{display:none;}*/
li[text="Baixar uma cópia"]
{  display:none;}  
li[text="Fazer Check-in"]
{  display:none;}  
li[text="Descartar Check-out"]
{  display:none;}
/*li[text="Histórico de Versões"]
{  display:none;}*/
li[text="Detalhes da Conformidade"]
{  display:none;}
li[text="Fluxos de Trabalho"]
{  display:none;}
/*li[text="Compartilhado com"]
{  display:none;}*/
</style>

<style type="text/css">

li[id$='Ribbon.List.TagsAndNotes'] {
display:none;
}
li[id$='Ribbon.List.Actions'] {
display:none;
}
 li[id$='Ribbon.ListItem.Actions'] {
display:none;
}
 li[id$='Ribbon.List.Share'] {
display:none;
}
 li[id$='Ribbon.List.CustomizeList'] {
display:none;
}
 li[id$='Ribbon.Documents.Workflow'] {
display:none;
}
li[id$='Ribbon.ListItem.TagsAndNotes'] {
display:none;
}
li[id$='Ribbon.ListItem.Share'] {
display:none;
}
li[id$='Ribbon.ListItem.Workflow'] {
display:none;
}
</style>

<asp:HiddenField ID="ocultoResp" runat="server" Value="TRUE" />

<script>
    function Esconde() {
        if ($("input[id*='ocultoResp']").val() == 'FALSE') {
            $('#Hero-WPQ2').remove();
        }
        else if ($("input[id*='ocultoResp']").val() == 'ADMIN') {
            var url = _spPageContextInfo.webAbsoluteUrl + "/SitePages/NovoMTC.aspx";
            $("#idHomePageNewItem").attr("onclick", "OpenDialog('" + url + "','Manual Técnico de Campo');return false;");
        }
    }

    function OpenDialog(url, title) {
        var options = {
            url: url,
            width: 300,
            height: 250,
            title: title,
            dialogReturnValueCallback: Function.createDelegate(null, CloseDialogCallback)
        };
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);
    }
</script>

<!--<script type="text/javascript">
function EscondeUpload(){
    $("#Hero-WPQ2").remove();
}
</script>
<script type="text/javascript">
            AddSendSubMenu = function (m, ctx) { }
            _spBodyOnLoadFunctionNames.push("resetAddSendSubMenu()");
            function resetAddSendSubMenu() {
                AddSendSubMenu = function (m, ctx) { };
            }
</script>-->