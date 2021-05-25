<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WP_AcoesRevisao.ascx.cs" Inherits="Furnas.OGX2.MTC.WebPartsPaginas.WP_AcoesRevisao.WP_AcoesRevisao" %>

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
</style>

<fieldset runat="server" id="actionsRevisao" visible="false">
    <legend>Ações de Solicitação</legend>
    <div>
        <div class="espaco"><asp:Button ID="btRevisao" runat="server" Text="Solicitar Revisão do MTC" ClientIDMode="Static" Width="180px" OnClick="btRevisao_Click" /></div>
        <br /><br />
    </div>
</fieldset>