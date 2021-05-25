<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WP_RequisitaMudanca.ascx.cs" Inherits="Furnas.OGX2.Webparts.WP_RequisitaMudanca.WP_RequisitaMudanca" %>


<style type="text/css">
    fieldset {
        padding:20px;
        width:100%;
    }
    .espaco {
        float:left;
        margin-right:10px;
    }
    #alteracaoPlano {
        border-radius: 5px;
        padding: 5px 10px 5px 35px;
        background: url(../../SiteAssets/edit_24x24.png) left 8px top 7px no-repeat;
        height: 40px;
        width: 200px!important;
        cursor:pointer;
    }
    #excluirPlano{
        border-radius: 5px;
        padding: 5px 10px 5px 35px;
        background: url(../../SiteAssets/remove_24x24.png) left 8px top 7px no-repeat;
        height: 40px;
        width: 200px!important;
        cursor:pointer;
    }
    #btnVoltar{
        border-radius: 5px;
        padding: 5px 10px 5px 35px;
        background: url(../../SiteAssets/previous24x24.png) left 30px top 7px no-repeat;
        height: 40px;
        width: 200px!important;
        cursor:pointer;
    }
</style>

<script>
    function Confirm() {
        var confirm_value = document.createElement("INPUT");
        confirm_value.type = "hidden";
        confirm_value.name = "confirm_value";
        if (confirm("Tem certeza da solcitação de exclusão?")) {
            confirm_value.value = "Sim";
        } else {
            confirm_value.value = "Não";
        }
        document.forms[0].appendChild(confirm_value);
    }

</script>

<script src="../../SiteAssets/JS/EncaminhaMudanca.js"></script>

<fieldset runat="server" id="actionsMudancas">
    <legend>Solicitação de Mudanças</legend>
    <div>
        <div class="espaco"><asp:Button ID="alteracaoPlano" runat="server" Text="Solicitar Alteração de Plano" ClientIDMode="Static" OnClick="alteracaoPlano_Click" Width="180px" /></div>
        
        <div class="espaco"><asp:Button ID="excluirPlano" runat="server" Text="Solicitar Exclusão do Plano" ClientIDMode="Static" Width="180px" /></div>

        <div class="espaco"><asp:Button ID="btnVoltar" runat="server" Text="Voltar aos Planos" ClientIDMode="Static" OnClick="btnVoltar_Click" Width="180px" /></div>

        <%--<div class="espaco"><asp:Button ID="cancelarExclusao" Visible="false" runat="server" Text="Cancelar Exclusão" ClientIDMode="Static" OnClick="cancelarExclusao_Click" /></div>--%>
        <br /><br />
    </div>
    <br />
    <div>
        <asp:Label ID="msgFluxo" runat="server" Visible="false" Text="O Ciclo relacionado a este Plano está fechado para alterações." ForeColor="Red"></asp:Label>
        <asp:Label ID="msgBtExclusao" runat="server" Visible="false" Text="A Exclusão do Plano já foi solicitada." ForeColor="Red"></asp:Label>
    </div>
</fieldset>