using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using System.Linq;

namespace Furnas.OGX2.Chamados.WebPartPaginas.WP_PesquisaSatisfacao
{
    [ToolboxItemAttribute(false)]
    public partial class WP_PesquisaSatisfacao : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public WP_PesquisaSatisfacao()
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
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    SPSite ImpersonatedSite = new SPSite(SPContext.Current.Web.Url);
                    SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();
                    try
                    {
                        string[] url = Page.Request.Url.ToString().Split('=');
                        if (url.Length > 1)
                        {
                            int IDPesquisa = Convert.ToInt32(url[1]);//Page.Request.QueryString.GetValues("ID")[0]);
                            SPList list = ImpersonatedWeb.Lists["Pesquisa de Satisfação"];
                            //SPListItem item = list.Items.OfType<SPListItem>().Where(p => new SPFieldLookupValue(Convert.ToString(p["Número do Chamado"])).LookupId == IDPesquisa).FirstOrDefault();
                            int count = (from SPListItem item in list.Items
                                         where Convert.ToInt32(item[SPBuiltInFieldId.ID]) == IDPesquisa
                                         select item).Count();
                            if (count > 0) //(item != null)
                            {
                                containerDone.Visible = false;
                                Cta.Visible = false;
                            }
                            else
                            {
                                containerDone.Visible = false;
                                ContainerDuplicidade.Visible = false;
                            }
                        }
                        ImpersonatedSite.Close(); ImpersonatedSite.Dispose();
                    }
                    catch
                    {
                        containerDone.Visible = false;
                        ContainerDuplicidade.Visible = false;
                        ImpersonatedSite.Close(); ImpersonatedSite.Dispose();
                    }
                });
            }

        }

        protected void Enviar_Click(object sender, EventArgs e)
        {
            try
            {
                SPList list = SPContext.Current.Web.Lists["Pesquisa de Satisfação"];

                SPListItem item = list.AddItem();

                item[SPBuiltInFieldId.Title] = "Ver detalhes";
                item["Número do Chamado"] = new SPFieldLookupValue(Convert.ToInt32(IDChamado.Text), NumeroChamado.Text);
                item["Classificação"] = Convert.ToInt32(TextoClassicacao.Text);
                if (comentario.Text != string.Empty)
                    item["Comentário"] = comentario.Text;
                item.Update();

                containerDone.Visible = true;
                ContainerDuplicidade.Visible = false;
                Cta.Visible = false;
            }
            catch
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Ocorreu um erro. Tente mais tarde!');", true);
            }
        }
    }
}
