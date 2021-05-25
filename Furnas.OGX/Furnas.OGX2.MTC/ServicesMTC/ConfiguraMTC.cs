using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using System.Xml;

namespace Furnas.OGX2.MTC.ServicesMTC
{
    public class ConfiguraMTC
    {
        private const string Admin = "Administradores GGA";
        private const string AdministradoresMTC = "Administradores GGA MTC";
        private const string Visualizadores = "Visualizadores GGA";
        private const string Colaboradores = "Colaboradores GGA";
        private const string Colab = "Colaboradores GGA MTC";

        public static void ChangeManualTecnico(SPWeb webcurrent)
        {
            SPList list = webcurrent.Lists.TryGetList("Manual Técnico de Campo");
            list.NavigateForFormsPages = true;
            list.EnableVersioning = true;
            list.DisableGridEditing = true;

            //SPField field = list.Fields["Código MTC"];
            //field.Required = false;
            //field.Title = "Código MTC";
            //field.Indexed = true;
            //field.EnforceUniqueValues = true;
            //field.Update();

            SPField title = list.Fields[SPBuiltInFieldId.Title];
            title.Title = "Ver detalhes";
            title.ShowInEditForm = false;
            title.ShowInNewForm = false;
            title.ShowInDisplayForm = false;
            title.DefaultValue = "Ver detalhes";
            title.Required = false;
            title.Update();

            SPFile allForm = webcurrent.GetFile(list.DefaultEditFormUrl);
            SPLimitedWebPartManager wpmE = allForm.GetLimitedWebPartManager(PersonalizationScope.Shared);

            if (wpmE.WebParts.Count == 1)
            {
                ScriptEditorWebPart wpE = new ScriptEditorWebPart();
                wpE.Content = "<script src='" + webcurrent.ServerRelativeUrl + "/SiteAssets/JSManual/EditManualTecnico.js'></script>";
                wpmE.AddWebPart(wpE, "Main", 3);

                ContentEditorWebPart wpAE = new ContentEditorWebPart();
                XmlDocument xmlDoc = new XmlDocument();
                XmlElement xmlElement = xmlDoc.CreateElement("Root");
                xmlElement.InnerText = 
                    "<script type='text/javascript' src='http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js'></script>" +
                    "<script>" +
                    "function ShowProgress() {" +
                    "        setTimeout(function () {" +
                    "            var modal = $('<div />');" +
                    "            modal.addClass('modal');" +
                    "            $('body').append(modal);" +
                    "            var loading = $('.loading');" +
                    "            loading.show();" +
                    "            var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);" +
                    "            var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);" +
                    "            loading.css({ top: top, left: left });" +
                    "        }, 200);" +
                    "}" +
                    "</script>" +
                    "<style>" +
                     " .modal{" +
                     "       position: fixed;" +
                     "       top: 0;" +
                     "       left: 0;" +
                     "       background-color: black;" +
                     "       z-index: 99;" +
                     "       opacity: 0.3;" +
                     "       filter: alpha(opacity=30);" +
                     "       -moz-opacity: 0.3;" +
                     "       min-height: 100%;" +
                     "       width: 100%;" +
                     "   }" +
                     "   .loading{" +
                     "       text-align: center;" +
                     "       font-family: Arial;" +
                     "       font-size: 10pt;" +
                     "       border: 2px solid #67CFF5;" +
                     "       width: 200px;" +
                     "       height: 110px;" +
                     "       display: none;" +
                     "       position: fixed;" +
                     "       background-color: White;" +
                     "       z-index: 999;" +
                     "       font-weight:bold;" +
                     "}" +
                "</style>" +
                "<div class='loading' style='display: none; top: 271.5px; left: 578px;'>" +
                "<br/>Carregando, por favor aguarde...<br/><br/>" +
                "<img src='../../SiteAssets/CSS/ajax-loader.gif' alt=''/>" +
                "<br/></div> ";
                wpAE.Content = xmlElement;
                wpAE.Content.InnerText = xmlElement.InnerText;
                wpmE.AddWebPart(wpAE, "Main", 1);

                allForm.Update();
            }

            SPFile NewForm = webcurrent.GetFile(list.DefaultNewFormUrl);
            SPLimitedWebPartManager wpmN = NewForm.GetLimitedWebPartManager(PersonalizationScope.Shared);

            if (wpmN.WebParts.Count == 1)
            {
                ScriptEditorWebPart wpN = new ScriptEditorWebPart();
                wpN.Content = "<script src='" + webcurrent.ServerRelativeUrl + "/SiteAssets/JSManual/ManualTecnico.js'></script>";
                wpmN.AddWebPart(wpN, "Main", 2);
                NewForm.Update();
            }

            SPFile DispForm = webcurrent.GetFile(list.DefaultDisplayFormUrl);
            SPLimitedWebPartManager wpmD = DispForm.GetLimitedWebPartManager(PersonalizationScope.Shared);

            if (wpmD.WebParts.Count == 1)
            {
                ScriptEditorWebPart wpD = new ScriptEditorWebPart();
                wpD.Content = "<script src='" + webcurrent.ServerRelativeUrl + "/SiteAssets/JSManual/DispManualTecnico.js'></script>";
                wpmD.AddWebPart(wpD, "Main", 3);

                System.Web.UI.WebControls.WebParts.WebPart displayAlterarcao = GetWebPart(webcurrent, "Furnas.OGX2.MTC - WP_AcoesRevisao");
                displayAlterarcao.ChromeType = PartChromeType.None;
                wpmD.AddWebPart(displayAlterarcao, "Main", 1);

                DispForm.Update();
            }

            list.BreakRoleInheritance(false, false);

            SPGroup groupD = webcurrent.SiteGroups[AdministradoresMTC];
            SPGroup groupA = webcurrent.SiteGroups[Admin];
            SPGroup groupB = webcurrent.SiteGroups[Visualizadores];
            SPGroup groupC = webcurrent.SiteGroups[Colab];

            SPRoleAssignment roleAssignmentA = new SPRoleAssignment(groupD);//Admin
            SPRoleAssignment roleAssignmentB = new SPRoleAssignment(groupA);//Admin
            SPRoleAssignment roleAssignmentC = new SPRoleAssignment(groupB);//Reader
            SPRoleAssignment roleAssignmentD = new SPRoleAssignment(groupC);//Contributor

            SPRoleDefinition roleDefinitionC = webcurrent.RoleDefinitions.GetByType(SPRoleType.Reader);
            SPRoleDefinition roleDefinitionA = webcurrent.RoleDefinitions.GetByType(SPRoleType.Administrator);
            SPRoleDefinition roleDefinitionB = webcurrent.RoleDefinitions.GetByType(SPRoleType.Contributor);

            
            roleAssignmentA.RoleDefinitionBindings.Add(roleDefinitionA);
            roleAssignmentB.RoleDefinitionBindings.Add(roleDefinitionA);
            roleAssignmentC.RoleDefinitionBindings.Add(roleDefinitionC);
            roleAssignmentD.RoleDefinitionBindings.Add(roleDefinitionB);

            list.RoleAssignments.Add(roleAssignmentC);
            list.RoleAssignments.Add(roleAssignmentA);
            list.RoleAssignments.Add(roleAssignmentB);
            list.RoleAssignments.Add(roleAssignmentD);
            list.Update();
        }

        public static void Manual_x_Docs(SPWeb web)
        {
            SPList mainList = web.Lists["Manual Técnico de Campo"];
            SPList relatedList = web.Lists["Documentos Manual Técnico de Campo"];
            SPFile dispForm = web.GetFile(mainList.DefaultDisplayFormUrl);
            SPLimitedWebPartManager wpm = dispForm.GetLimitedWebPartManager(PersonalizationScope.Shared);
            if (wpm.WebParts.Count == 1)
            {
                System.Web.UI.WebControls.WebParts.WebPart displayAlterarcao = GetWebPart(web, "Furnas.OGX2.MTC - WP_AcoesRevisao");
                displayAlterarcao.ChromeType = PartChromeType.None;
                wpm.AddWebPart(displayAlterarcao, "Main", 1);

                // create our List View web part
                XsltListViewWebPart wpM = new XsltListViewWebPart();

                wpM.ListId = relatedList.ID;
                wpM.ListName = relatedList.ID.ToString();
                wpM.ViewGuid = relatedList.DefaultView.ID.ToString();
                wpM.XmlDefinition = relatedList.DefaultView.GetViewXml();
                wpM.AllowConnect = true;
                wpM.ChromeType = PartChromeType.TitleOnly;
                wpM.Toolbar = "None";
                wpm.AddWebPart(wpM, "Main", 3);


                System.Web.UI.WebControls.WebParts.WebPart consumerM = wpm.WebParts[1];
                System.Web.UI.WebControls.WebParts.WebPart providerM = wpm.WebParts[0];
                ProviderConnectionPoint providerPointM = wpm.GetProviderConnectionPoints(providerM)["ListFormRowProvider_WPQ_"];
                ConsumerConnectionPoint consumerPointM = wpm.GetConsumerConnectionPoints(consumerM)["DFWP Filter Consumer ID"];

                SPRowToParametersTransformer optimusM = new SPRowToParametersTransformer();
                optimusM.ProviderFieldNames = new string[] { "ID" };
                optimusM.ConsumerFieldNames = new string[] { "Title" };
                wpm.SPConnectWebParts(providerM, providerPointM, consumerM, consumerPointM, optimusM);
                dispForm.Update();
            }
        }

        public static void InseriWebParts(SPWeb web)
        {
            SPList mainList = web.Lists["Manual Técnico de Campo"];
            SPFile displayForm = web.GetFile(mainList.DefaultViewUrl);
            SPLimitedWebPartManager wpmD = displayForm.GetLimitedWebPartManager(PersonalizationScope.Shared);

            System.Web.UI.WebControls.WebParts.WebPart displayAlterarcao = GetWebPart(web, "Furnas.OGX2.MTC - WP_ValidaUsuariosMTC");
            if (displayAlterarcao != null)
            {
                displayAlterarcao.ChromeType = PartChromeType.None;
                wpmD.AddWebPart(displayAlterarcao, "Main", 2);
                
                ScriptEditorWebPart wpD = new ScriptEditorWebPart();
                wpD.Content = "<script>$(document).ready(function () {Esconde()});</script>";
                wpmD.AddWebPart(wpD, "Main", 3);

                displayForm.Update();
            }

        }

        protected static System.Web.UI.WebControls.WebParts.WebPart GetWebPart(SPWeb web, string webPartName)
        {
            SPList webPartGallery;
            if (web.IsRootWeb)
            {
                webPartGallery = web.GetCatalog(
                   SPListTemplateType.WebPartCatalog);
            }
            else
            {
                webPartGallery = web.ParentWeb.GetCatalog(
                   SPListTemplateType.WebPartCatalog);
            }
            var webParts = webPartGallery.GetItems();
            int i = 0;
            foreach (SPListItem w in webParts)
            {
                if (w.Title == webPartName) //"Furnas.OGX2 - WP_RequisitaMudanca")
                {
                    var typeName = webParts[i].GetFormattedValue("WebPartTypeName");
                    var assemblyName = webParts[i].GetFormattedValue("WebPartAssembly");
                    var webPartHandle = Activator.CreateInstance(
                        assemblyName, typeName);

                    System.Web.UI.WebControls.WebParts.WebPart webPart =
                        (System.Web.UI.WebControls.WebParts.WebPart)webPartHandle.Unwrap();

                    return webPart;
                }
                i++;
            }

            return null;
        }

        public static void CreateGroup(SPWeb webcurrent, string groupname, string description, SPRoleType sPRoleType, params SPUser[] predefinedusers)
        {
            SPGroup group = null;
            
            try
            {
                group = webcurrent.SiteGroups[groupname];
            }
            catch { }

            if (group == null)
            {
                webcurrent.SiteGroups.Add(groupname, webcurrent.Site.Owner as SPMember, webcurrent.Site.Owner, description);
                group = webcurrent.SiteGroups[groupname];

                SPRoleDefinition roleDefinition = webcurrent.RoleDefinitions.GetByType(sPRoleType);
                SPRoleAssignment roleAssignment = new SPRoleAssignment(group);
                roleAssignment.RoleDefinitionBindings.Add(roleDefinition);
                webcurrent.RoleAssignments.Add(roleAssignment);
                webcurrent.Update();

                foreach (SPUser user in predefinedusers)
                    group.AddUser(user);
                group.Update();
            }
        }
    }
}
