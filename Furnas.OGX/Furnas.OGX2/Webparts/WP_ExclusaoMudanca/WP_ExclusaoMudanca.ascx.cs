using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using Furnas.OGX2.Service;
using System.Web;

namespace Furnas.OGX2.Webparts.WP_ExclusaoMudanca
{
    [ToolboxItemAttribute(false)]
    public partial class WP_ExclusaoMudanca : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public WP_ExclusaoMudanca()
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
                    urlBack.Value = HttpContext.Current.Request.UrlReferrer.OriginalString.ToString();
                }
                catch { }
            }
        }

        protected void CarregaAcoes(SPWeb web, string id)
        {
            SPListItem planoMudanca = web.Lists["Solicitação de Mudanças"].GetItemById(Convert.ToInt32(id));

            if (planoMudanca != null)
            {
                if (DuplicaItens.ValidaTextField(planoMudanca["Alteração Solicitada"]) == "Exclusão")
                {
                    SPListItem itemCiclo = web.Lists["Ciclos"].Items.OfType<SPListItem>().Where(p => Convert.ToString(p[SPBuiltInFieldId.Title]) == DuplicaItens.ValidaTextField(planoMudanca["Ciclo"])).FirstOrDefault();

                    if (itemCiclo != null)
                    {
                        if (Convert.ToString(itemCiclo["Fluxo de Controle"]) == "Fechado")
                        {
                            actionsMudancas.Visible = true;
                            cancelarExclusao.Visible = true;
                            cancelarExclusao.Enabled = false;
                            msgFluxo.Visible = true;
                        }
                        else
                        {
                            SPGroup admin = web.Groups["Administradores GGA"];
                            SPFieldUserValue userAuthor = new SPFieldUserValue(web, Convert.ToString(planoMudanca[SPBuiltInFieldId.Author]));

                            if ((web.CurrentUser.ID == userAuthor.LookupId || admin.ContainsCurrentUser) && DuplicaItens.ValidaTextField(planoMudanca["Status da solicitação"]) == "Solicitado")
                            {
                                actionsMudancas.Visible = true;
                                cancelarExclusao.Visible = true;
                            }
                        }
                    }
                }
            }
        }

        protected void cancelarExclusao_Click(object sender, EventArgs e)
        {
            string id = Page.Request.QueryString.GetValues("ID")[0];
            string url = SPContext.Current.Web.Url.ToString();
            SPWeb web = SPContext.Current.Web;
            
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
               {
                   SPSite ImpersonatedSite = new SPSite(web.Site.ID);
                   SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb(web.ID);

                   SPListItem plano = ImpersonatedWeb.Lists["Solicitação de Mudanças"].GetItemById(Convert.ToInt32(id));

                   if (plano != null)
                   {
                       ImpersonatedWeb.AllowUnsafeUpdates = true;
                       plano.Delete();
                       ImpersonatedWeb.AllowUnsafeUpdates = false;
                   }

                   ImpersonatedWeb.Close(); ImpersonatedWeb.Dispose();
                   ImpersonatedSite.Close(); ImpersonatedSite.Dispose();

               });
                
                //string urlP = url + "/Lists/Planos/DispForm.aspx?ID=" + id + "&Source=" + url + "/Lists/Planos";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Exclusão do plano cancelada!');window.location.href='" + urlBack.Value + "';", true);
            }
            catch
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Erro ao cancelar exclusão do Plano!');", true);
                
            }
        }
    }
}