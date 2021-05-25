using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;

namespace Furnas.OGX2.MTC.WebPartsPaginas.WP_ValidaUsuariosMTC
{
    [ToolboxItemAttribute(false)]
    public partial class WP_ValidaUsuariosMTC : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public WP_ValidaUsuariosMTC()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Page.IsPostBack)
            //{
                SPUser user = SPContext.Current.Web.CurrentUser;
                SPWeb web = SPContext.Current.Web;

                bool Eadmin = false; bool Eresposansavel = false;
                if (E_admin(web, "Administradores GGA MTC", user.Name))//E_admin(web, "Administradores GGA", user.Name) ||
                {
                    Eadmin = true;
                    ocultoResp.Value = "ADMIN";
                }
                
                if(!Eadmin){
                    SPListItemCollection items = web.Lists["Gerência Responsável MTC"].GetItems();

                    foreach (SPListItem orgaos in items)
                    {
                        foreach (string email in Convert.ToString(orgaos["Gerente Imediato"]).Split(';'))
                        {
                            if (email != "null" && email != "")
                            {
                                if (user.Email == email)
                                {
                                    Eresposansavel = true; ocultoResp.Value = "TRUE";
                                    break;
                                }
                            }
                        }

                        if (!Eresposansavel)
                        {
                            SPFieldUserValueCollection usersGerencia = new SPFieldUserValueCollection(web, Convert.ToString(orgaos["Responsáveis"]));
                            foreach (SPFieldUserValue users in usersGerencia)
                            {
                                if (user.ID == users.User.ID)
                                {
                                    Eresposansavel = true; ocultoResp.Value = "TRUE";
                                    break;
                                }
                            }
                        }

                        if (Eresposansavel)
                        {
                            break;
                        }
                    }

                    if (!Eresposansavel)
                    {
                        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "text", "EscondeUpload();", true);
                        ocultoResp.Value = "FALSE";
                    }
                }
            //}
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

        private void ReponsavelMTC()
        {
        }
    }
}
