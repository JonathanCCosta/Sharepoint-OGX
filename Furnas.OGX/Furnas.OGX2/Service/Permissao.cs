using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furnas.OGX2.Service
{
    public class Permissao
    {
        public static void AlterarPemrissao(SPListItem item)
        {

        }

        public static void FechaCiclo(SPListItem item)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite ImpersonatedSite = new SPSite(item.Web.Url);
                SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();

                //Quebra a herança
                if (!item.HasUniqueRoleAssignments)
                {
                    item.BreakRoleInheritance(true);
                }

                SPPrincipal Author = (SPPrincipal)new SPFieldUserValue(ImpersonatedWeb, Convert.ToString(item[SPBuiltInFieldId.Author])).User;
                item.RoleAssignments.Remove(Author);

                ImpersonatedWeb.Close(); ImpersonatedWeb.Dispose();
                ImpersonatedSite.Close(); ImpersonatedSite.Dispose();

            });
        }

        public static void AbreCiclo(SPListItem item)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite ImpersonatedSite = new SPSite(item.Web.Url);
                SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();

                //Quebra a herança
                if (!item.HasUniqueRoleAssignments)
                {
                    item.BreakRoleInheritance(true);
                }

                SPRoleDefinition roleAuthor = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Contributor);
                SPPrincipal Author = (SPPrincipal)new SPFieldUserValue(ImpersonatedWeb, Convert.ToString(item[SPBuiltInFieldId.Author])).User;
                SPRoleAssignment roleAssignmentsAuthor = new SPRoleAssignment(Author);
                roleAssignmentsAuthor.RoleDefinitionBindings.Add(roleAuthor);
                item.RoleAssignments.Add(roleAssignmentsAuthor);

                ImpersonatedWeb.Close(); ImpersonatedWeb.Dispose();
                ImpersonatedSite.Close(); ImpersonatedSite.Dispose();

            });
        }

        private const string Admin = "Administradores GGA";
        private const string Colaboradores = "Colaboradores GGA";
        private const string Visualizadores = "Visualizadores GGA";

        public static void PermissaoMudancaExclusaoInclusao(SPListItem item)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite ImpersonatedSite = new SPSite(item.Web.Url);
                SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();

                //Quebra a herança
                if (!item.HasUniqueRoleAssignments)
                {
                    item.BreakRoleInheritance(true);
                }
                
                //Retira permissão
                SPGroup group = ImpersonatedWeb.SiteGroups[Colaboradores];
                SPRoleAssignment roleAssignments = new SPRoleAssignment(group);
                item.RoleAssignments.Remove(group);

                //Reintegra Permissão
                
                SPRoleDefinition roleDefinition = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Reader);
                roleAssignments.RoleDefinitionBindings.Add(roleDefinition);

                item.RoleAssignments.Add(roleAssignments);

                ImpersonatedWeb.Close(); ImpersonatedWeb.Dispose();
                ImpersonatedSite.Close(); ImpersonatedSite.Dispose();

            });
        }

        public static void PermissaoMudancaAlteracao(SPListItem item)
        {
            if (item.ContentType.Name == "Plano por Superação - Mudanças")
                Service.DuplicaItens.CamposAlterados(item);
            else
                Service.DuplicaItens.CamposAlteradosMelhoriaReforco(item);

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite ImpersonatedSite = new SPSite(item.Web.Url);
                SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();

                //Quebra a herança
                if (!item.HasUniqueRoleAssignments)
                {
                    item.BreakRoleInheritance(true);
                }

                //Retira permissão
                SPGroup group = ImpersonatedWeb.SiteGroups[Colaboradores];
                SPRoleAssignment roleAssignments = new SPRoleAssignment(group);
                item.RoleAssignments.Remove(group);

                //Reintegra Permissão
                SPRoleDefinition roleDefinition = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Reader);
                roleAssignments.RoleDefinitionBindings.Add(roleDefinition);
                item.RoleAssignments.Add(roleAssignments);

                //Author Permite Editar
                SPRoleDefinition roleAuthor = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Contributor);
                SPPrincipal Author = (SPPrincipal)new SPFieldUserValue(ImpersonatedWeb, Convert.ToString(item[SPBuiltInFieldId.Author])).User;
                SPRoleAssignment roleAssignmentsAuthor = new SPRoleAssignment(Author);
                roleAssignmentsAuthor.RoleDefinitionBindings.Add(roleAuthor);
                item.RoleAssignments.Add(roleAssignmentsAuthor);

                ImpersonatedWeb.Close(); ImpersonatedWeb.Dispose();
                ImpersonatedSite.Close(); ImpersonatedSite.Dispose();

            });
        }
    }
}
