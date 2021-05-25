using Furnas.OGX2.Service;
using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using System.Collections.Generic;
using System.Web;

namespace Furnas.OGX2.Webparts.WP_RequisitaMudanca
{
    [ToolboxItemAttribute(false)]
    public partial class WP_RequisitaMudanca : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public WP_RequisitaMudanca()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    SPWeb web = SPContext.Current.Web;
                    string id = Page.Request.QueryString.GetValues("ID")[0];
                    CarregaAcoes(web, id);
                }catch{}

            }
        }

        protected void CarregaAcoes(SPWeb web, string id)
        {
            SPListItem itemPlano = web.Lists["Planos"].GetItemById(Convert.ToInt32(id));
            SPListItem itemCiclo = web.Lists["Ciclos"].GetItemById(new SPFieldLookupValue(Convert.ToString(itemPlano["Ciclo"])).LookupId);

            if (Convert.ToString(itemCiclo["Fluxo de Controle"]) == "Fechado")
            {
                alteracaoPlano.Enabled = false;
                excluirPlano.Enabled = false;
                msgFluxo.Visible = true;
            }
            /*else
            {
                List<SPListItem> planoMudanca = web.Lists["Solicitação de Mudanças de planos importados"].Items.OfType<SPListItem>().Where(p => Convert.ToString(p["Numeração"]) == Convert.ToString(itemPlano[SPBuiltInFieldId.Title])).ToList();

                if (planoMudanca.Count > 0)
                {
                    SPGroup admin = web.Groups["Administradores GGA"];
                    foreach (SPListItem item in planoMudanca)
                    {
                        SPFieldUserValue userAuthor = new SPFieldUserValue(web, Convert.ToString(item[SPBuiltInFieldId.Author]));
                        if (DuplicaItens.ValidaTextField(item["Alteração Solicitada"]) == "Exclusão")
                        {
                            excluirPlano.Enabled = false;
                            msgBtExclusao.Visible = true;

                            if (web.CurrentUser.ID == userAuthor.LookupId || admin.ContainsCurrentUser)
                                cancelarExclusao.Visible = true;
                        }
                    }
                }
                else
                {
                    excluirPlano.Enabled = true;
                    msgBtExclusao.Visible = false;
                    cancelarExclusao.Visible = false;
                }
            }*/
        }

        protected void alteracaoPlano_Click(object sender, EventArgs e)
        {
            string id = Page.Request.QueryString.GetValues("ID")[0];
            SPWeb web = SPContext.Current.Web;

            SPListItem plano = web.Lists["Planos"].GetItemById(Convert.ToInt32(id));
            string url = SPContext.Current.Web.Url.ToString();

            SPList listMudanca = web.Lists["Solicitação de Mudanças"];
            //SPListItem itemMudanca = listMudanca.AddItem();
            //int IdNovo = 0;

            if (plano.ContentType.Name == "Plano de Melhoria")
            {
                Context.Response.Redirect(url + "/Lists/PlanosMudanca/NewForm.aspx?ContentTypeId=" + listMudanca.ContentTypes["Plano de Melhoria - Mudanças"].Id + "&Source=" + url + "/Lists/Planos/DispForm.aspx?ID=" + id + "&Acoes=Sim");
                 //IdNovo= DuplicaItens.CopiarItem(itemMudanca, plano);
                //Context.Response.Redirect(url + "/Lists/PlanosMudanca/EditForm.aspx?ID=" + IdNovo + "&Source=" + url + "/Lists/Planos/DispForm.aspx?ID=" + id);
            }
            else if (plano.ContentType.Name == "Plano de Reforço")
            {
                Context.Response.Redirect(url + "/Lists/PlanosMudanca/NewForm.aspx?ContentTypeId=" + listMudanca.ContentTypes["Plano de Reforço - Mudanças"].Id + "&Source=" + url + "/Lists/Planos/DispForm.aspx?ID=" + id + "&Acoes=Sim");
                //IdNovo = DuplicaItens.CopiarItem(itemMudanca, plano);
                //Context.Response.Redirect(url + "/Lists/PlanosMudanca/NewForm.aspx?ContentTypeId=" + IdNovo + "&Source=" + url + "/Lists/Planos/DispForm.aspx?ID=" + id);
            }
            else
            {
                Context.Response.Redirect(url + "/Lists/PlanosMudanca/NewForm.aspx?ContentTypeId=" + listMudanca.ContentTypes["Plano por Superação - Mudanças"].Id + "&Source=" + url + "/Lists/Planos/DispForm.aspx?ID=" + id + "&Acoes=Sim");
                //IdNovo = DuplicaItens.CopiarItem(itemMudanca, plano);
                //Context.Response.Redirect(url + "/Lists/PlanosMudanca/NewForm.aspx?ContentTypeId=" + IdNovo + "&Source=" + url + "/Lists/Planos/DispForm.aspx?ID=" + id);
            }
        }

        /*protected void excluirPlano_Click(object sender, EventArgs e)
        {

            #region Antigo Processo de Exclusão
            string id = Page.Request.QueryString.GetValues("ID")[0];
            SPWeb web = SPContext.Current.Web;

            SPListItem plano = web.Lists["Planos"].GetItemById(Convert.ToInt32(id));
            string url = SPContext.Current.Web.Url.ToString();
            
            SPList listMudanca = web.Lists["Solicitação de Mudanças de planos importados"];
            SPListItem itemMudanca = listMudanca.AddItem();

            try
            {
                if(plano.ContentType.Name == "Plano de Melhoria")
                    itemMudanca[SPBuiltInFieldId.ContentTypeId] = listMudanca.ContentTypes["Plano de Melhoria - Mudanças"].Id;
                else if (plano.ContentType.Name == "Plano de Reforço")
                    itemMudanca[SPBuiltInFieldId.ContentTypeId] = listMudanca.ContentTypes["Plano de Reforço - Mudanças"].Id;
                else
                    itemMudanca[SPBuiltInFieldId.ContentTypeId] = listMudanca.ContentTypes["Plano por Superação - Mudanças"].Id;

                itemMudanca["Alteração Solicitada"] = "Exclusão";
                DuplicaItens.CopiarItemExclusao(itemMudanca, plano);

                string urlP = url + "/Lists/Planos/DispForm.aspx?ID=" + id + "&Source=" + url + "/Lists/Planos" + "&Acoes=Sim";
                
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Exclusão do plano solicitado com sucesso!');", true);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Exclusão do plano solicitado com sucesso!');window.location.href='" + urlP + "';", true);
                
                //Context.Response.Redirect(url + "/Lists/Planos/DispForm.aspx?ID=" + id + "&Source=" + url + "/Lists/Planos" + "&Acoes=Sim");
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('" + ex.Message.Replace("'", "") + "');", true);
            }
        }*/

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Context.Response.Redirect(SPContext.Current.Web.Url.ToString() + "/Lists/Planos");
        }
    }
}
