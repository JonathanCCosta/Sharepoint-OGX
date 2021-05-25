using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Furnas.OGX2.MTC.ServicesMTC;

namespace Furnas.OGX2.MTC.WebPartsPaginas.WP_AcoesRevisao
{
    [ToolboxItemAttribute(false)]
    public partial class WP_AcoesRevisao : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public WP_AcoesRevisao()
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

                    E_Visivel(web, id);
                }
                catch { }

            }
        }

        public void E_Visivel(SPWeb web,string id)
        {
            SPListItem MTC = web.Lists["Manual Técnico de Campo"].GetItemById(Convert.ToInt32(id));

            if (ServiceManual.ValidaTextField(MTC["Controle de Status"]) == "Vigente")
            {
                if(Perfil(web, SPContext.Current.Web.CurrentUser, new SPFieldLookupValue(MTC["OrgaoResponsavelMTC"].ToString()).LookupId))
                {
                    actionsRevisao.Visible = true;
                }
            }
        }

        public bool Perfil(SPWeb web, SPUser user, int idOrgao)
        {
            bool Eadmin = false; bool Eresposansavel = false;
            if (E_admin(web, "Administradores GGA MTC", user.Name))
            {
                return true;
            }

            if (!Eadmin)
            {
                SPListItem item = web.Lists["Gerência Responsável MTC"].GetItemById(idOrgao);

                foreach (string email in Convert.ToString(item["Gerente Imediato"]).Split(';'))
                {
                    if (email != "null" && email != "")
                    {
                        if (user.Email == email)
                        {
                            return true;
                        }
                    }
                }

                if (!Eresposansavel)
                {
                    SPFieldUserValueCollection usersGerencia = new SPFieldUserValueCollection(web, Convert.ToString(item["Responsáveis"]));
                    foreach (SPFieldUserValue users in usersGerencia)
                    {
                        if (user.ID == users.User.ID)
                        {
                            return true;
                        }
                    }
                }
            }

            if (!Eresposansavel || !Eadmin)
            {
                return false;
            }

            return false;
        }

        private bool E_admin(SPWeb web, string groupName, string userName)
        {
            bool userFound = false;
            SPFieldUserValueCollection userCols = new SPFieldUserValueCollection();
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite objSite = new SPSite(SPContext.Current.Web.Url);
                SPWeb objWeb = objSite.OpenWeb();
                SPGroup spGroup = objWeb.SiteGroups[groupName];
                foreach (SPUser groupUser in spGroup.Users)
                {
                    if (groupUser.Name == userName)
                    {
                        userFound = true;
                        break;
                    }
                }
            });
            return userFound;
        }

        protected void btRevisao_Click(object sender, EventArgs e)
        {
            string id = Page.Request.QueryString.GetValues("ID")[0];
            SPWeb web = SPContext.Current.Web;

            //duplica
            if (ServiceManual.TemRevisaoAtiva(web, id))
                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Ja existe uma revisão em andamento.');", true);
            else
            {
                try
                {
                    SPListItem item = null;
                    item = ServiceManual.CopyItemRevisao(web, id);
                    ServiceManual.DocumentoCopy(item, id);

                    try
                    {
                        EmailMTC.EmailRevisao(item, item["DescricaoMTC"].ToString());
                        PermissaoMTC.PermissaoRevisao(item);
                    }
                    catch { }

                    if (item != null)
                    {
                        Context.Response.Redirect(SPContext.Current.Web.Url.ToString() + "/Lists/ManualTecnico/EditForm.aspx?ID=" + item.ID);
                    }
                }
                catch
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Ja existe uma revisão em andamento.');", true);
                }

            }
        }
    }
}
