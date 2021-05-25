using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace Furnas.OGX2.Chamados.ER_Chamados.ER_Pesquisa
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class ER_Pesquisa : SPItemEventReceiver
    {
        /// <summary>
        /// An item was added.
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);

             SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite ImpersonatedSite = new SPSite(properties.Web.Url);
                SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();

                SPListItem itemN = ImpersonatedWeb.Lists["Pesquisa de Satisfação"].GetItemById(properties.ListItem.ID);

                //Quebra a herança
                if (!itemN.HasUniqueRoleAssignments)
                {
                    itemN.BreakRoleInheritance(false,true);
                }
                SPGroup group = ImpersonatedWeb.SiteGroups["Administradores GGA"];

                SPRoleDefinition adminRole = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Reader);
                SPRoleAssignment roleAssignments = new SPRoleAssignment(group);
                roleAssignments.RoleDefinitionBindings.Add(adminRole);
                itemN.RoleAssignments.Add(roleAssignments);

                ImpersonatedWeb.Close(); ImpersonatedWeb.Dispose();
                ImpersonatedSite.Close(); ImpersonatedSite.Dispose();

            });
        }


    }
}