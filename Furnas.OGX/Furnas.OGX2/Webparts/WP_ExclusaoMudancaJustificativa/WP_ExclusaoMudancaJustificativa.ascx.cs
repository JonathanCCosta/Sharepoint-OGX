using Furnas.OGX2.Service;
using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;

namespace Furnas.OGX2.Webparts.WP_ExclusaoMudancaJustificativa
{
    [ToolboxItemAttribute(false)]
    public partial class WP_ExclusaoMudancaJustificativa : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public WP_ExclusaoMudancaJustificativa()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
            //string id = Page.Request.QueryString.GetValues("IDE")[0];
            SPWeb web = SPContext.Current.Web;

            SPListItem plano = web.Lists["Planos"].GetItemById(Convert.ToInt32(NumSolicitacao.Text));
            string url = SPContext.Current.Web.Url.ToString();

            SPList listMudanca = web.Lists["Solicitação de Mudanças"];
            SPListItem itemMudanca = listMudanca.AddItem();

            
                if (plano.ContentType.Name == "Plano de Melhoria")
                    itemMudanca[SPBuiltInFieldId.ContentTypeId] = listMudanca.ContentTypes["Plano de Melhoria - Mudanças"].Id;
                else if (plano.ContentType.Name == "Plano de Reforço")
                    itemMudanca[SPBuiltInFieldId.ContentTypeId] = listMudanca.ContentTypes["Plano de Reforço - Mudanças"].Id;
                else
                    itemMudanca[SPBuiltInFieldId.ContentTypeId] = listMudanca.ContentTypes["Plano por Superação - Mudanças"].Id;

                itemMudanca["Alteração Solicitada"] = "Exclusão";
                itemMudanca["Justificativa da Exclusão"] = justificativa.Value;
                DuplicaItens.CopiarItemExclusao(itemMudanca, plano);

                Page.ClientScript.RegisterStartupScript(this.GetType(), "Close", "CloseFormOK();", true);
                //string urlP = url + "/Lists/Planos/DispForm.aspx?ID=" + id + "&Source=" + url + "/Lists/Planos" + "&Acoes=Sim";

                //Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Exclusão do plano solicitado com sucesso!');", true);
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Exclusão do plano solicitado com sucesso!');window.parent.location.href='" + urlP + "';", true);
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Exclusão do plano solicitado com sucesso!');", true);
                //Context.Response.Redirect(url + "/Lists/Planos/DispForm.aspx?ID=" + id + "&Source=" + url + "/Lists/Planos" + "&Acoes=Sim");
            }
            catch (Exception ex)
            {
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('" + ex.Message.Replace("'", "") + "');", true);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Ocorreu um erro. Comunique o administrador do sistema.');", true);
            }
        }
    }
}
