using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Furnas.OGX2.Webparts.WP_RemoverPlanos
{
    [ToolboxItemAttribute(false)]
    public partial class WP_RemoverPlanos : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public WP_RemoverPlanos()
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
                LoadDropDown(ddlCiclo, SPContext.Current.Web, "Ciclos");
            }
        }

        internal static void LoadDropDown(System.Web.UI.WebControls.DropDownList ddlItems, SPWeb sPWeb, string listname)
        {
            SPList list = sPWeb.Lists[listname];
            
            foreach (SPListItem item in list.Items)
            {
                //bool temPlano = sPWeb.Lists["Planos"].Items.OfType<SPListItem>().Any(p=> new SPFieldLookupValue(Convert.ToString(item["Ciclo"])).LookupId == new SPFieldLookupValue(Convert.ToString(p["Ciclo"])).LookupId);
                if (Convert.ToString(item["Fluxo de Controle"]) == "Fechado")// && temPlano)
                {
                    ListItem lt = new ListItem();
                    lt.Value = item.ID.ToString();
                    lt.Text = item.Title;

                    ddlItems.Items.Add(lt);
                }
            }

            List<ListItem> itemsDDL = ddlItems.Items.Cast<ListItem>().OrderBy(lt => lt.Text).ToList();
            ddlItems.Items.Clear();
            ddlItems.Items.AddRange(itemsDDL.ToArray());
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            SPWeb web = SPContext.Current.Web;
            try
            {
                int idDoc = Service.DeletePlanos.DeletePlanosEmCascata(web, ddlTipos.Items[ddlTipos.SelectedIndex].Text, ddlCiclo.Items[ddlCiclo.SelectedIndex].Text, Convert.ToInt32(ddlCiclo.SelectedValue));

                if (idDoc > 0)
                {
                    //Exclui Documento do Repositório
                    web.Lists.TryGetList("Repositório").GetItemById(idDoc).Delete();

                    //Grava Logs de Exclusão
                    SPListItem log = web.Lists["Logs de Exclusão"].AddItem();
                    log[SPBuiltInFieldId.Title] = ddlCiclo.Items[ddlCiclo.SelectedIndex].Text;
                    log["Plano"] = ddlTipos.Items[ddlTipos.SelectedIndex].Text;
                    log["Modificado por"] = web.CurrentUser;
                    log.Update();

                    Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Plano excluído com sucesso!');", true);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('As opções selecionadas de Ciclo e Tipo de Plano não contém Planos cadastrados.');", true);
                }
            }
            catch (Exception ex) {
                Service.Importacao.Log(web, ex.Message.ToString(), "Exclusão de Plano");
                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('" + ex.Message.Replace("'", "") + "');", true);
            }
        }
    }
}
