using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furnas.OGX2.Chamados.ServiceChamados
{
    public class PermissaoChamado
    {
        private const string Colaboradores = "Colaboradores GGA";
        private const string Administradores = "Administradores GGA";
        public static void PermissaoAbreChamado(SPListItem item)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite ImpersonatedSite = new SPSite(item.Web.Url);
                SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();

                SPListItem itemN = ImpersonatedWeb.Lists["Chamados"].GetItemById(item.ID);

                //Quebra a herança
                if (!itemN.HasUniqueRoleAssignments)
                {
                    itemN.BreakRoleInheritance(false,true);
                }

                //Retira permissão
                /*SPGroup group = ImpersonatedWeb.SiteGroups[Colaboradores];
                SPRoleAssignment roleAssignments = new SPRoleAssignment(group);
                item.RoleAssignments.Remove(group);

                //Reintegra Permissão
                SPRoleDefinition roleDefinition = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Reader);
                roleAssignments.RoleDefinitionBindings.Add(roleDefinition);
                item.RoleAssignments.Add(roleAssignments);*/

                //Author Permite Editar
                SPRoleDefinition roleAuthor = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Contributor);
                SPPrincipal Author = (SPPrincipal)new SPFieldUserValue(ImpersonatedWeb, Convert.ToString(itemN[SPBuiltInFieldId.Author])).User;
                SPRoleAssignment roleAssignmentsAuthor = new SPRoleAssignment(Author);
                roleAssignmentsAuthor.RoleDefinitionBindings.Add(roleAuthor);
                itemN.RoleAssignments.Add(roleAssignmentsAuthor);
                
                //Admins
                SPGroup group = ImpersonatedWeb.SiteGroups[Administradores];
                SPRoleDefinition adminRole = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Administrator);
                SPRoleAssignment roleAssignments = new SPRoleAssignment(group);
                roleAssignments.RoleDefinitionBindings.Add(adminRole);
                itemN.RoleAssignments.Add(roleAssignments);

                //Imediatos
                SPRoleDefinition roleImediato = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Reader);
                foreach (string email in Convert.ToString(itemN["Imediato do Responsável"]).Split(';'))
                {
                    if (email != "null" && email != "")
                    {
                        SPPrincipalInfo u = SPUtility.ResolvePrincipal(ImpersonatedWeb, email, SPPrincipalType.All, SPPrincipalSource.All, null, true);
                        SPUser user = ImpersonatedWeb.EnsureUser(u.LoginName); //ImpersonatedWeb.SiteUsers.GetByEmail(email);
                        SPRoleAssignment roleAssignmentsImediato = new SPRoleAssignment(user);
                        roleAssignmentsImediato.RoleDefinitionBindings.Add(roleImediato);
                        itemN.RoleAssignments.Add(roleAssignmentsImediato);
                    }
                }

                /*SPFieldUserValueCollection users = new SPFieldUserValueCollection(ImpersonatedWeb, Convert.ToString(item["Imediato do Responsável"]));
                foreach(SPFieldUserValue user in users)
                {
                    SPPrincipal Imediato = (SPPrincipal)user.User;
                    SPRoleAssignment roleAssignmentsImediato = new SPRoleAssignment(Imediato);
                    roleAssignmentsImediato.RoleDefinitionBindings.Add(roleImediato);
                    item.RoleAssignments.Add(roleAssignmentsImediato);
                }*/
                
                ImpersonatedWeb.Close(); ImpersonatedWeb.Dispose();
                ImpersonatedSite.Close(); ImpersonatedSite.Dispose();

            });
        }
        
        public static void PermissaoChamadoEmAndamentoPosGerencia(SPListItem item)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite ImpersonatedSite = new SPSite(item.Web.Url);
                SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();

                SPListItem itemN = ImpersonatedWeb.Lists["Chamados"].GetItemById(item.ID);

                SPListItem itemGerencia = item.Web.Lists["Gerência Responsável"].GetItemById(new SPFieldLookupValue(itemN["Gerência Responsável"].ToString()).LookupId);
                SPFieldUserValueCollection usersGerencia = new SPFieldUserValueCollection(ImpersonatedWeb, Convert.ToString(itemGerencia["Responsáveis"]));

                foreach (string email in Convert.ToString(itemGerencia["Gerente Imediato"]).Split(';'))
                {
                    if (email != "null" && email != "")
                    {
                        SPPrincipalInfo u = SPUtility.ResolvePrincipal(ImpersonatedWeb, email, SPPrincipalType.All, SPPrincipalSource.All, null, true);
                        SPUser user = ImpersonatedWeb.EnsureUser(u.LoginName); //ImpersonatedWeb.SiteUsers.GetByEmail(email);
                        usersGerencia.Add(new SPFieldUserValue(item.Web, user.ID, user.Name));
                    }
                }

                foreach (SPFieldUserValue user in usersGerencia)
                {
                    //retira colaboração
                    SPRoleAssignment roleAssignment = new SPRoleAssignment(user.User);
                    itemN.RoleAssignments.Remove(user.User);
                    //coloca leitura
                    SPRoleDefinition roleDefinition = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Reader);
                    roleAssignment.RoleDefinitionBindings.Add(roleDefinition);

                    itemN.RoleAssignments.Add(roleAssignment);
                }

                ImpersonatedWeb.Close(); ImpersonatedWeb.Dispose();
                ImpersonatedSite.Close(); ImpersonatedSite.Dispose();

            });
        }

        public static void PermissaoChamadoEmAndamento(SPListItem item)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                
                SPSite ImpersonatedSite = new SPSite(item.Web.Url);
                SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();
                
                SPListItem itemN = ImpersonatedWeb.Lists["Chamados"].GetItemById(item.ID);

                
                SPPrincipal Author = (SPPrincipal)new SPFieldUserValue(ImpersonatedWeb, Convert.ToString(itemN[SPBuiltInFieldId.Author])).User;
                SPRoleAssignment roleAssigmentsAuthor = new SPRoleAssignment(Author);
                itemN.RoleAssignments.Remove(Author);

                //Author não Permite Editar
                SPRoleDefinition roleAuthor = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Reader);
                SPRoleAssignment roleAssignmentsAuthor = new SPRoleAssignment(Author);
                roleAssignmentsAuthor.RoleDefinitionBindings.Add(roleAuthor);
                itemN.RoleAssignments.Add(roleAssignmentsAuthor);

                ImpersonatedWeb.Close(); ImpersonatedWeb.Dispose();
                ImpersonatedSite.Close(); ImpersonatedSite.Dispose();

            });
        }

        public static void PermissaoChamadoInformacao(SPListItem item)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite ImpersonatedSite = new SPSite(item.Web.Url);
                SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();

                SPListItem itemN = ImpersonatedWeb.Lists["Chamados"].GetItemById(item.ID);

                SPPrincipal Author = (SPPrincipal)new SPFieldUserValue(ImpersonatedWeb, Convert.ToString(itemN[SPBuiltInFieldId.Author])).User;
                SPRoleAssignment roleAssigmentsAuthor = new SPRoleAssignment(Author);
                itemN.RoleAssignments.Remove(Author);

                //Author Permite Editar
                SPRoleDefinition roleAuthor = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Contributor);
                //SPPrincipal Author = (SPPrincipal)new SPFieldUserValue(ImpersonatedWeb, Convert.ToString(item[SPBuiltInFieldId.Author])).User;
                SPRoleAssignment roleAssignmentsAuthor = new SPRoleAssignment(Author);
                roleAssignmentsAuthor.RoleDefinitionBindings.Add(roleAuthor);
                itemN.RoleAssignments.Add(roleAssignmentsAuthor);

                ImpersonatedWeb.Close(); ImpersonatedWeb.Dispose();
                ImpersonatedSite.Close(); ImpersonatedSite.Dispose();

            });
        }

        public static void PermissaoChamadoOutraGerencia(SPListItem item, SPFieldUserValueCollection users)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite ImpersonatedSite = new SPSite(item.Web.Url);
                SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();

                SPListItem itemN = ImpersonatedWeb.Lists["Chamados"].GetItemById(item.ID);

                SPPrincipal Author = (SPPrincipal)new SPFieldUserValue(ImpersonatedWeb, Convert.ToString(itemN[SPBuiltInFieldId.Author])).User;
                SPRoleAssignment roleAssigmentsAuthor = new SPRoleAssignment(Author);
                itemN.RoleAssignments.Remove(Author);

                //Author não Permite Editar
                SPRoleDefinition roleAuthor = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Reader);
                SPRoleAssignment roleAssignmentsAuthor = new SPRoleAssignment(Author);
                roleAssignmentsAuthor.RoleDefinitionBindings.Add(roleAuthor);
                itemN.RoleAssignments.Add(roleAssignmentsAuthor);

                //SPListItem itemGerencia = item.Web.Lists["Gerência Responsável"].GetItemById(new SPFieldLookupValue(itemN["Gerência Responsável"].ToString()).LookupId);
                //SPFieldUserValueCollection usersGerencia = new SPFieldUserValueCollection(ImpersonatedWeb, Convert.ToString(itemGerencia["Responsáveis"]));

                foreach (SPFieldUserValue user in users)
                {
                    SPRoleAssignment roleAssignment = new SPRoleAssignment(user.User);
                    SPRoleDefinition roleDefinition = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Contributor);
                    roleAssignment.RoleDefinitionBindings.Add(roleDefinition);

                    itemN.RoleAssignments.Add(roleAssignment);
                }

                ImpersonatedWeb.Close(); ImpersonatedWeb.Dispose();
                ImpersonatedSite.Close(); ImpersonatedSite.Dispose();

            });
        }

        public static void PermissaoResponsavelAdded(SPListItem item)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite ImpersonatedSite = new SPSite(item.Web.Url);
                SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();
                
                SPListItem itemN = ImpersonatedWeb.Lists["Chamados"].GetItemById(item.ID);

                SPRoleDefinition roleResp = item.Web.RoleDefinitions.GetByType(SPRoleType.Contributor);
                SPPrincipal Resp = (SPPrincipal)new SPFieldUserValue(item.Web, Convert.ToString(item["Responsável"])).User;
                SPRoleAssignment roleAssignmentsAuthor = new SPRoleAssignment(Resp);
                roleAssignmentsAuthor.RoleDefinitionBindings.Add(roleResp);
                itemN.RoleAssignments.Add(roleAssignmentsAuthor);
                ImpersonatedSite.Close(); ImpersonatedSite.Dispose();

            });
        }

        public static void PermissaoResponsavelUpdating(SPItemEventProperties properties)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite ImpersonatedSite = new SPSite(properties.ListItem.Web.Url);
                SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();

                SPRoleDefinition roleResp = properties.ListItem.Web.RoleDefinitions.GetByType(SPRoleType.Contributor);
                SPPrincipal Resp = (SPPrincipal)new SPFieldUserValue(ImpersonatedWeb, Convert.ToString(properties.AfterProperties["Responsavel"])).User;
                SPRoleAssignment roleAssignmentsAuthor = new SPRoleAssignment(Resp);
                roleAssignmentsAuthor.RoleDefinitionBindings.Add(roleResp);
                properties.ListItem.RoleAssignments.Add(roleAssignmentsAuthor);
                ImpersonatedSite.Close(); ImpersonatedSite.Dispose();
            });
        }

        public static void PermissaoTrocaResponsavelUpdating(SPItemEventProperties properties)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite ImpersonatedSite = new SPSite(properties.ListItem.Web.Url);
                SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();
                
                SPPrincipal Resp = (SPPrincipal)new SPFieldUserValue(properties.ListItem.Web, Convert.ToString(properties.ListItem["Responsavel"])).User;
                SPPrincipal Author = (SPPrincipal)new SPFieldUserValue(properties.ListItem.Web, Convert.ToString(properties.ListItem[SPBuiltInFieldId.Author])).User;
                SPRoleAssignmentCollection SPRoleAssColn = properties.ListItem.RoleAssignments;
                for (int i = SPRoleAssColn.Count - 1; i >= 0; i--)
                {
                    SPRoleAssignment roleAssignmentSingle = SPRoleAssColn[i];
                    if (roleAssignmentSingle.Member.GetType().Name == "SPUser")
                    { 
                       SPRoleAssColn.Remove(i);
                    }
                }

                SPRoleDefinition roleResp = properties.ListItem.Web.RoleDefinitions.GetByType(SPRoleType.Contributor);
                SPRoleAssignment roleAssignmentsResp = new SPRoleAssignment(Resp);
                roleAssignmentsResp.RoleDefinitionBindings.Add(roleResp);
                properties.ListItem.RoleAssignments.Add(roleAssignmentsResp);

                if (properties.ListItem["Controle de status"].ToString() == "Aberto")
                {
                    SPRoleDefinition roleAuthor = properties.ListItem.Web.RoleDefinitions.GetByType(SPRoleType.Contributor);
                    SPRoleAssignment roleAssignmentsAuthor = new SPRoleAssignment(Author);
                    roleAssignmentsAuthor.RoleDefinitionBindings.Add(roleAuthor);
                    properties.ListItem.RoleAssignments.Add(roleAssignmentsAuthor);
                }
                else
                {
                    SPRoleDefinition roleAuthor = properties.ListItem.Web.RoleDefinitions.GetByType(SPRoleType.Reader);
                    SPRoleAssignment roleAssignmentsAuthor = new SPRoleAssignment(Author);
                    roleAssignmentsAuthor.RoleDefinitionBindings.Add(roleAuthor);
                    properties.ListItem.RoleAssignments.Add(roleAssignmentsAuthor);
                }
                

                ImpersonatedSite.Close(); ImpersonatedSite.Dispose();
            });
        }

        public static void PermissaoTrocaResponsavelUpdated(SPItemEventProperties properties, SPUser RespNovo)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite ImpersonatedSite = new SPSite(properties.ListItem.Web.Url);
                SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();
                SPListItem itemN = ImpersonatedWeb.Lists["Chamados"].GetItemById(properties.ListItem.ID);
                /*List<string> users = new List<string>();
                users.Add(new SPFieldUserValue(properties.ListItem.Web, Convert.ToString(properties.ListItem["Responsavel"])).User.LoginName);
                users.Add(new SPFieldUserValue(properties.ListItem.Web, Convert.ToString(properties.ListItem[SPBuiltInFieldId.Author])).User.LoginName);
                foreach (string email in Convert.ToString(properties.ListItem["Imediato do Responsável"]).Split(';'))
                {
                    users.Add(ImpersonatedWeb.SiteUsers.GetByEmail(email).LoginName);                     
                }*/

                if (Suporte.ValidaTextField(properties.ListItem["Responsavel"]) != string.Empty)
                {
                    SPPrincipal resposavel_antigo = new SPFieldUserValue(properties.ListItem.Web, Convert.ToString(properties.ListItem["Responsavel"])).User;
                    SPRoleAssignment roleAssignments = new SPRoleAssignment(resposavel_antigo);
                    itemN.RoleAssignments.Remove(resposavel_antigo);
                }

                if (RespNovo != null)
                {
                    //SPPrincipal Resp = new SPFieldUserValue(properties.ListItem.Web, Convert.ToString(properties.AfterProperties["Responsavel"])).User;
                    SPRoleDefinition roleResp = properties.ListItem.Web.RoleDefinitions.GetByType(SPRoleType.Contributor);
                    SPRoleAssignment roleAssignmentsResp = new SPRoleAssignment(RespNovo);
                    roleAssignmentsResp.RoleDefinitionBindings.Add(roleResp);
                    itemN.RoleAssignments.Add(roleAssignmentsResp);
                }

                ImpersonatedSite.Close(); ImpersonatedSite.Dispose();
            });
        }

        public static void PermissaoEncerra(SPItemEventProperties properties)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite ImpersonatedSite = new SPSite(properties.ListItem.Web.Url);
                SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();
                
                SPListItem itemN = ImpersonatedWeb.Lists["Chamados"].GetItemById(properties.ListItem.ID);

                //Grupo Administradores
                //SPGroup group = ImpersonatedWeb.SiteGroups[Administradores];
                //SPRoleAssignment roleAssignments = new SPRoleAssignment(group);
                //itemN.RoleAssignments.Remove(group);

                SPRoleDefinition roleDefinition = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Reader);
                //roleAssignments.RoleDefinitionBindings.Add(roleDefinition);
                //itemN.RoleAssignments.Add(roleAssignments);

                //Solicitante
                /*SPPrincipal Author = (SPPrincipal)new SPFieldUserValue(ImpersonatedWeb, Convert.ToString(properties.ListItem[SPBuiltInFieldId.Author])).User;
                SPRoleAssignment roleAssigmentsAuthor = new SPRoleAssignment(Author);
                itemN.RoleAssignments.Remove(Author);

                SPRoleAssignment roleAssignmentsAuthor = new SPRoleAssignment(Author);
                roleAssignmentsAuthor.RoleDefinitionBindings.Add(roleDefinition);
                itemN.RoleAssignments.Add(roleAssignmentsAuthor);*/

                // Responsavel
                //SPPrincipal Resp = (SPPrincipal)new SPFieldUserValue(ImpersonatedWeb, Convert.ToString(properties.ListItem["Responsavel"]).Split('|')[1]).User;
                /*SPPrincipal Resp = (SPPrincipal)new SPFieldUserValue(ImpersonatedWeb, Convert.ToString(properties.ListItem["Responsavel"])).User;
                SPRoleAssignment roleAssigmentsResp = new SPRoleAssignment(Resp);
                itemN.RoleAssignments.Remove(Resp);

                SPRoleAssignment roleAssignmentsResp = new SPRoleAssignment(Resp);
                roleAssignmentsResp.RoleDefinitionBindings.Add(roleDefinition);
                itemN.RoleAssignments.Add(roleAssignmentsResp);*/

                SPRoleAssignmentCollection SPRoleAssColn = itemN.RoleAssignments;
                for (int i = SPRoleAssColn.Count - 1; i >= 0; i--)
                {
                    SPRoleAssignment roleAssignmentSingle = SPRoleAssColn[i];
                    if (roleAssignmentSingle.Member.GetType().Name == "SPUser")
                    {
                        SPPrincipal user = roleAssignmentSingle.Member;
                        itemN.RoleAssignments.Remove(i);

                        SPRoleAssignment roleUser = new SPRoleAssignment(user);
                        roleUser.RoleDefinitionBindings.Add(roleDefinition);
                        itemN.RoleAssignments.Add(roleUser);
                    }
                }

                ImpersonatedSite.Close(); ImpersonatedSite.Dispose();
            });
        }

        public static void PermissaoEncerra(SPListItem item)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite ImpersonatedSite = new SPSite(item.Web.Url);
                SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();

                SPListItem itemN = ImpersonatedWeb.Lists["Chamados"].GetItemById(item.ID);

                //Grupo Administradores
                SPGroup group = ImpersonatedWeb.SiteGroups[Administradores];
                SPRoleAssignment roleAssignments = new SPRoleAssignment(group);
                itemN.RoleAssignments.Remove(group);

                SPRoleDefinition roleDefinition = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Reader);
                roleAssignments.RoleDefinitionBindings.Add(roleDefinition);
                itemN.RoleAssignments.Add(roleAssignments);

                SPRoleAssignmentCollection SPRoleAssColn = itemN.RoleAssignments;
                for (int i = SPRoleAssColn.Count - 1; i >= 0; i--)
                {
                    SPRoleAssignment roleAssignmentSingle = SPRoleAssColn[i];
                    if (roleAssignmentSingle.Member.GetType().Name == "SPUser")
                    {
                        SPPrincipal user = roleAssignmentSingle.Member;
                        itemN.RoleAssignments.Remove(i);

                        SPRoleAssignment roleUser = new SPRoleAssignment(user);
                        roleUser.RoleDefinitionBindings.Add(roleDefinition);
                        itemN.RoleAssignments.Add(roleUser);
                    }
                }

                ImpersonatedSite.Close(); ImpersonatedSite.Dispose();
            });
        }

        public static void PermissaoEncerraJobs(SPListItem item)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite ImpersonatedSite = new SPSite(item.Web.Url);
                SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();

                SPListItem itemN = ImpersonatedWeb.Lists["Chamados"].GetItemById(item.ID);

                //Grupo Administradores
                SPGroup group = ImpersonatedWeb.SiteGroups[Administradores];
                SPRoleAssignment roleAssignments = new SPRoleAssignment(group);
                itemN.RoleAssignments.Remove(group);

                SPRoleDefinition roleDefinition = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Reader);
                roleAssignments.RoleDefinitionBindings.Add(roleDefinition);
                itemN.RoleAssignments.Add(roleAssignments);

                //Solicitante
                SPPrincipal Author = (SPPrincipal)new SPFieldUserValue(ImpersonatedWeb, Convert.ToString(item[SPBuiltInFieldId.Author])).User;
                SPRoleAssignment roleAssigmentsAuthor = new SPRoleAssignment(Author);
                itemN.RoleAssignments.Remove(Author);

                SPRoleAssignment roleAssignmentsAuthor = new SPRoleAssignment(Author);
                roleAssignmentsAuthor.RoleDefinitionBindings.Add(roleDefinition);
                itemN.RoleAssignments.Add(roleAssignmentsAuthor);

                // Responsavel
                //SPPrincipal Resp = (SPPrincipal)new SPFieldUserValue(ImpersonatedWeb, Convert.ToString(properties.ListItem["Responsavel"]).Split('|')[1]).User;
                SPPrincipal Resp = (SPPrincipal)new SPFieldUserValue(ImpersonatedWeb, Convert.ToString(item["Responsavel"])).User;
                SPRoleAssignment roleAssigmentsResp = new SPRoleAssignment(Resp);
                itemN.RoleAssignments.Remove(Resp);

                SPRoleAssignment roleAssignmentsResp = new SPRoleAssignment(Resp);
                roleAssignmentsResp.RoleDefinitionBindings.Add(roleDefinition);
                itemN.RoleAssignments.Add(roleAssignmentsResp);

                /*SPRoleAssignmentCollection SPRoleAssColn = properties.ListItem.RoleAssignments;
                for (int i = SPRoleAssColn.Count - 1; i >= 0; i--)
                {
                    SPRoleAssignment roleAssignmentSingle = SPRoleAssColn[i];
                    if (roleAssignmentSingle.Member.GetType().Name == "SPUser")
                    {
                        SPRoleAssColn.Remove(i);
                    }
                }*/
                ImpersonatedSite.Close(); ImpersonatedSite.Dispose();
            });
        }

        public static void TrocaGerenciaResponsavel(SPFieldUserValueCollection users, SPListItem item)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                foreach (SPFieldUserValue user in users)
                {

                    SPSite ImpersonatedSite = new SPSite(item.Web.Url);
                    SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();

                    SPListItem itemNew = ImpersonatedWeb.Lists["Chamados"].GetItemById(item.ID);

                    SPRoleAssignment roleAssignment = new SPRoleAssignment(user.User);
                    itemNew.RoleAssignments.Remove(user.User);

                    ImpersonatedSite.Close(); ImpersonatedSite.Dispose();
                }
            });
        }
    }
}
