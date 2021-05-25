<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WP_CargasAuxiliares.ascx.cs" Inherits="Furnas.OGX2.Cargas.WP_CargasAuxiliares.WP_CargasAuxiliares" %>

<style>
    fieldset {
        padding:20px;
        width:30%;
    }
    .espaco {
        float:left;
        margin-right:10px;
        margin-bottom:10px;
    }
</style>
<fieldset>
    <legend>INICIAR CARGAS</legend>
    <div class="espaco">
        <asp:Button ID="carregar" runat="server" Text="CARREGAR TIPO MTC" OnClick="carregar_Click" Width="240" />
    </div>
    <div class="espaco">
        <asp:Button ID="carregarFabricante" runat="server" Text="CARREGAR FABRICANTE" OnClick="carregarFabricante_Click" Width="240"  />
    </div>
    <div class="espaco">
        <asp:Button ID="carregarGrupo" runat="server" Text="CARREGAR GRUPO" OnClick="carregarGrupo_Click" Width="240" />
    </div>
    <div class="espaco">
        <asp:Button ID="carregarSubgrupo" runat="server" Text="CARREGAR SUBGRUPO" Width="240" OnClick="carregarSubgrupo_Click" />
    </div>
    <div class="espaco">
        <asp:Button ID="carregarMTC" runat="server" Text="CARREGAR GERÊNCIA RESP." Width="240" OnClick="carregarMTC_Click" />
    </div>
    <div class="espaco">
        <asp:Button ID="carregaPalavraChave" runat="server" Text="CARREGAR PALAVRA CHAVE" Width="240" OnClick="carregaPalavraChave_Click"/>
    </div>
</fieldset>