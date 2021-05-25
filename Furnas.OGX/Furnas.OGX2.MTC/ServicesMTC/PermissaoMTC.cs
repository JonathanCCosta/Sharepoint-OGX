using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furnas.OGX2.MTC.ServicesMTC
{
    public class PermissaoMTC
    {
        private const string AdministradoresMTC = "Administradores GGA MTC";
        private const string Administradores = "Administradores GGA";
        private const string Visualizador = "Visualizadores GGA";

        public static void PermissaoAguardando(SPListItem item)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite ImpersonatedSite = new SPSite(item.Web.Url);
                SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();

                SPListItem itemN = ImpersonatedWeb.Lists["Manual Técnico de Campo"].GetItemById(item.ID);

                //Quebra a herança
                if (!itemN.HasUniqueRoleAssignments)
                {
                    itemN.BreakRoleInheritance(false,true);
                }

                SPRoleDefinition roleAuthor = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Reader);
                SPPrincipal Author = (SPPrincipal)new SPFieldUserValue(ImpersonatedWeb, Convert.ToString(itemN[SPBuiltInFieldId.Author])).User;
                SPRoleAssignment roleAssignmentsAuthor = new SPRoleAssignment(Author);
                roleAssignmentsAuthor.RoleDefinitionBindings.Add(roleAuthor);
                itemN.RoleAssignments.Add(roleAssignmentsAuthor);

                //SPList Orgaos = 
                SPListItem ItemOrgaos = ImpersonatedWeb.Lists["Gerência Responsável MTC"].GetItemById(new SPFieldLookupValue(itemN["Órgão Responsável"].ToString()).LookupId);
                //Imediatos
                SPRoleDefinition roleImediato = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Reader);
                foreach (string email in Convert.ToString(ItemOrgaos["Gerente Imediato"]).Split(';'))
                {
                    if (email != "null" && email != "")
                    {
                        SPPrincipalInfo u = SPUtility.ResolvePrincipal(ImpersonatedWeb, email, SPPrincipalType.All, SPPrincipalSource.All, null, true);
                        //string u = SPUtility.GetLoginNameFromEmail(ImpersonatedSite, email);
                        SPUser user = ImpersonatedWeb.EnsureUser(u.LoginName); //ImpersonatedWeb.SiteUsers.GetByEmail(email);
                        SPRoleAssignment roleAssignmentsImediato = new SPRoleAssignment(user);
                        roleAssignmentsImediato.RoleDefinitionBindings.Add(roleImediato);
                        itemN.RoleAssignments.Add(roleAssignmentsImediato);
                    }
                }

                SPFieldUserValueCollection usersGerencia = new SPFieldUserValueCollection(ImpersonatedWeb, Convert.ToString(ItemOrgaos["Responsáveis"]));
                foreach (SPFieldUserValue user in usersGerencia)
                {
                    SPRoleAssignment roleAssignment = new SPRoleAssignment(user.User);
                    SPRoleDefinition roleDefinition = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Reader);
                    roleAssignment.RoleDefinitionBindings.Add(roleDefinition);

                    itemN.RoleAssignments.Add(roleAssignment);
                }

                //Admins
                SPGroup group = ImpersonatedWeb.SiteGroups[Visualizador];
                SPRoleDefinition adminRoleV = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Reader);
                SPRoleAssignment roleAssignments = new SPRoleAssignment(group);
                roleAssignments.RoleDefinitionBindings.Add(adminRoleV);
                itemN.RoleAssignments.Add(roleAssignments);

                SPRoleDefinition adminRole = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Administrator);
                SPGroup groupMTC = ImpersonatedWeb.SiteGroups[AdministradoresMTC];
                SPRoleAssignment roleAssignmentsMTC = new SPRoleAssignment(groupMTC);
                roleAssignmentsMTC.RoleDefinitionBindings.Add(adminRole);
                itemN.RoleAssignments.Add(roleAssignmentsMTC);

                ImpersonatedWeb.Close(); ImpersonatedWeb.Dispose();
                ImpersonatedSite.Close(); ImpersonatedSite.Dispose();

            });

        }

        public static void PermissaoAguardandoUpdate(SPListItem item)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite ImpersonatedSite = new SPSite(item.Web.Url);
                SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();

                SPListItem itemN = ImpersonatedWeb.Lists["Manual Técnico de Campo"].GetItemById(item.ID);

                //Quebra a herança
                if (!itemN.HasUniqueRoleAssignments)
                {
                    itemN.BreakRoleInheritance(false, true);
                }

                SPRoleAssignmentCollection SPRoleAssColn = itemN.RoleAssignments;
                for (int i = SPRoleAssColn.Count - 1; i >= 0; i--)
                {
                    SPRoleAssignment roleAssignmentSingle = SPRoleAssColn[i];
                    if (roleAssignmentSingle.Member.GetType().Name == "SPUser")
                    {
                        SPPrincipal user = roleAssignmentSingle.Member;
                        itemN.RoleAssignments.Remove(i);
                    }
                }

                SPRoleDefinition roleAuthor = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Reader);
                SPPrincipal Author = (SPPrincipal)new SPFieldUserValue(ImpersonatedWeb, Convert.ToString(itemN[SPBuiltInFieldId.Author])).User;
                SPRoleAssignment roleAssignmentsAuthor = new SPRoleAssignment(Author);
                roleAssignmentsAuthor.RoleDefinitionBindings.Add(roleAuthor);
                itemN.RoleAssignments.Add(roleAssignmentsAuthor);

                //SPList Orgaos = 
                SPListItem ItemOrgaos = ImpersonatedWeb.Lists["Gerência Responsável MTC"].GetItemById(new SPFieldLookupValue(itemN["Órgão Responsável"].ToString()).LookupId);
                //Imediatos
                SPRoleDefinition roleImediato = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Reader);
                foreach (string email in Convert.ToString(ItemOrgaos["Gerente Imediato"]).Split(';'))
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

                SPFieldUserValueCollection usersGerencia = new SPFieldUserValueCollection(ImpersonatedWeb, Convert.ToString(ItemOrgaos["Responsáveis"]));
                foreach (SPFieldUserValue user in usersGerencia)
                {
                    SPRoleAssignment roleAssignment = new SPRoleAssignment(user.User);
                    SPRoleDefinition roleDefinition = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Reader);
                    roleAssignment.RoleDefinitionBindings.Add(roleDefinition);

                    itemN.RoleAssignments.Add(roleAssignment);
                }

                //Admins
                //SPGroup group = ImpersonatedWeb.SiteGroups[Administradores];
                SPRoleDefinition adminRole = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Administrator);
                /*SPRoleAssignment roleAssignments = new SPRoleAssignment(group);
                roleAssignments.RoleDefinitionBindings.Add(adminRole);
                itemN.RoleAssignments.Add(roleAssignments);*/

                SPGroup groupMTC = ImpersonatedWeb.SiteGroups[AdministradoresMTC];
                SPRoleAssignment roleAssignmentsMTC = new SPRoleAssignment(groupMTC);
                roleAssignmentsMTC.RoleDefinitionBindings.Add(adminRole);
                itemN.RoleAssignments.Add(roleAssignmentsMTC);

                ImpersonatedWeb.Close(); ImpersonatedWeb.Dispose();
                ImpersonatedSite.Close(); ImpersonatedSite.Dispose();

            });

        }

        public static void PermissaoRevisao(SPListItem item)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite ImpersonatedSite = new SPSite(item.Web.Url);
                SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();

                SPListItem itemN = ImpersonatedWeb.Lists["Manual Técnico de Campo"].GetItemById(item.ID);

                //Quebra a herança
                if (!itemN.HasUniqueRoleAssignments)
                {
                    itemN.BreakRoleInheritance(false, true);
                }

                SPRoleAssignmentCollection SPRoleAssColn = itemN.RoleAssignments;
                for (int i = SPRoleAssColn.Count - 1; i >= 0; i--)
                {
                    SPRoleAssignment roleAssignmentSingle = SPRoleAssColn[i];
                    if (roleAssignmentSingle.Member.GetType().Name == "SPUser")
                    {
                        SPPrincipal user = roleAssignmentSingle.Member;
                        itemN.RoleAssignments.Remove(i);
                    }
                }

                SPRoleDefinition roleAuthor = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Contributor);
                SPPrincipal Author = (SPPrincipal)new SPFieldUserValue(ImpersonatedWeb, Convert.ToString(itemN[SPBuiltInFieldId.Author])).User;
                SPRoleAssignment roleAssignmentsAuthor = new SPRoleAssignment(Author);
                roleAssignmentsAuthor.RoleDefinitionBindings.Add(roleAuthor);
                itemN.RoleAssignments.Add(roleAssignmentsAuthor);

                //SPList Orgaos = 
                SPListItem ItemOrgaos = ImpersonatedWeb.Lists["Gerência Responsável MTC"].GetItemById(new SPFieldLookupValue(itemN["Órgão Responsável"].ToString()).LookupId);
                //Imediatos
                SPRoleDefinition roleImediato = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Contributor);
                foreach (string email in Convert.ToString(ItemOrgaos["Gerente Imediato"]).Split(';'))
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

                SPFieldUserValueCollection usersGerencia = new SPFieldUserValueCollection(ImpersonatedWeb, Convert.ToString(ItemOrgaos["Responsáveis"]));
                foreach (SPFieldUserValue user in usersGerencia)
                {
                    SPRoleAssignment roleAssignment = new SPRoleAssignment(user.User);
                    SPRoleDefinition roleDefinition = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Contributor);
                    roleAssignment.RoleDefinitionBindings.Add(roleDefinition);

                    itemN.RoleAssignments.Add(roleAssignment);
                }

                //Admins
                //SPGroup group = ImpersonatedWeb.SiteGroups[Administradores];
                SPRoleDefinition adminRole = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Administrator);
                /*SPRoleAssignment roleAssignments = new SPRoleAssignment(group);
                roleAssignments.RoleDefinitionBindings.Add(adminRole);
                itemN.RoleAssignments.Add(roleAssignments);*/

                SPGroup groupMTC = ImpersonatedWeb.SiteGroups[AdministradoresMTC];
                SPRoleAssignment roleAssignmentsMTC = new SPRoleAssignment(groupMTC);
                roleAssignmentsMTC.RoleDefinitionBindings.Add(adminRole);
                itemN.RoleAssignments.Add(roleAssignmentsMTC);

                ImpersonatedWeb.Close(); ImpersonatedWeb.Dispose();
                ImpersonatedSite.Close(); ImpersonatedSite.Dispose();

            });

        }

        public static void PermissaoTrocaOrgao(SPListItem item, SPItemEventProperties itemNovos)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite ImpersonatedSite = new SPSite(item.Web.Url);
                SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();

                SPListItem itemN = ImpersonatedWeb.Lists["Manual Técnico de Campo"].GetItemById(item.ID);

                //Quebra a herança
                if (!itemN.HasUniqueRoleAssignments)
                {
                    itemN.BreakRoleInheritance(false, true);
                }

                SPRoleAssignmentCollection SPRoleAssColn = itemN.RoleAssignments;
                for (int i = SPRoleAssColn.Count - 1; i >= 0; i--)
                {
                    SPRoleAssignment roleAssignmentSingle = SPRoleAssColn[i];
                    if (roleAssignmentSingle.Member.GetType().Name == "SPUser" && roleAssignmentSingle.Member.ID != new SPFieldUserValue(ImpersonatedWeb, Convert.ToString(itemN[SPBuiltInFieldId.Author])).User.ID)
                    {
                        SPPrincipal user = roleAssignmentSingle.Member;
                        itemN.RoleAssignments.Remove(i);
                    }
                }

                SPListItem ItemOrgaos = ImpersonatedWeb.Lists["Gerência Responsável MTC"].GetItemById(new SPFieldLookupValue(itemNovos.AfterProperties["OrgaoResponsavelMTC"].ToString()).LookupId);
                //Imediatos
                SPRoleDefinition roleImediato = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Reader);
                foreach (string email in Convert.ToString(ItemOrgaos["Gerente Imediato"]).Split(';'))
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

                SPFieldUserValueCollection usersGerencia = new SPFieldUserValueCollection(ImpersonatedWeb, Convert.ToString(ItemOrgaos["Responsáveis"]));
                foreach (SPFieldUserValue user in usersGerencia)
                {
                    SPRoleAssignment roleAssignment = new SPRoleAssignment(user.User);
                    SPRoleDefinition roleDefinition = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Reader);
                    roleAssignment.RoleDefinitionBindings.Add(roleDefinition);

                    itemN.RoleAssignments.Add(roleAssignment);
                }

                ImpersonatedWeb.Close(); ImpersonatedWeb.Dispose();
                ImpersonatedSite.Close(); ImpersonatedSite.Dispose();

            });

        }

        public static void PermissaoEmConsolidacao(SPListItem item, SPItemEventProperties itemNovos)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite ImpersonatedSite = new SPSite(item.Web.Url);
                SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();

                SPListItem itemN = ImpersonatedWeb.Lists["Manual Técnico de Campo"].GetItemById(item.ID);

                //Quebra a herança
                if (!itemN.HasUniqueRoleAssignments)
                {
                    itemN.BreakRoleInheritance(false, true);
                }

                SPRoleAssignmentCollection SPRoleAssColn = itemN.RoleAssignments;
                for (int i = SPRoleAssColn.Count - 1; i >= 0; i--)
                {
                    SPRoleAssignment roleAssignmentSingle = SPRoleAssColn[i];
                    if (roleAssignmentSingle.Member.GetType().Name == "SPUser")
                    {
                        SPPrincipal user = roleAssignmentSingle.Member;
                        itemN.RoleAssignments.Remove(i);
                    }
                }

                SPRoleDefinition roleAuthor = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Contributor);
                SPPrincipal Author = (SPPrincipal)new SPFieldUserValue(ImpersonatedWeb, Convert.ToString(itemN[SPBuiltInFieldId.Author])).User;
                SPRoleAssignment roleAssignmentsAuthor = new SPRoleAssignment(Author);
                roleAssignmentsAuthor.RoleDefinitionBindings.Add(roleAuthor);
                itemN.RoleAssignments.Add(roleAssignmentsAuthor);

                SPListItem ItemOrgaos = ImpersonatedWeb.Lists["Gerência Responsável MTC"].GetItemById(new SPFieldLookupValue(itemNovos.AfterProperties["OrgaoResponsavelMTC"].ToString()).LookupId);
                //Imediatos
                SPRoleDefinition roleImediato = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Contributor);
                foreach (string email in Convert.ToString(ItemOrgaos["Gerente Imediato"]).Split(';'))
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

                SPFieldUserValueCollection usersGerencia = new SPFieldUserValueCollection(ImpersonatedWeb, Convert.ToString(ItemOrgaos["Responsáveis"]));
                foreach (SPFieldUserValue user in usersGerencia)
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

        public static void PermissaoEmConsolidacaoJob(SPListItem item)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite ImpersonatedSite = new SPSite(item.Web.Url);
                SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();

                SPListItem itemN = ImpersonatedWeb.Lists["Manual Técnico de Campo"].GetItemById(item.ID);

                //Quebra a herança
                if (!itemN.HasUniqueRoleAssignments)
                {
                    itemN.BreakRoleInheritance(false, true);
                }

                SPRoleAssignmentCollection SPRoleAssColn = itemN.RoleAssignments;
                for (int i = SPRoleAssColn.Count - 1; i >= 0; i--)
                {
                    SPRoleAssignment roleAssignmentSingle = SPRoleAssColn[i];
                    if (roleAssignmentSingle.Member.GetType().Name == "SPUser")
                    {
                        SPPrincipal user = roleAssignmentSingle.Member;
                        itemN.RoleAssignments.Remove(i);
                    }
                }

                SPRoleDefinition roleAuthor = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Contributor);
                SPPrincipal Author = (SPPrincipal)new SPFieldUserValue(ImpersonatedWeb, Convert.ToString(itemN[SPBuiltInFieldId.Author])).User;
                SPRoleAssignment roleAssignmentsAuthor = new SPRoleAssignment(Author);
                roleAssignmentsAuthor.RoleDefinitionBindings.Add(roleAuthor);
                itemN.RoleAssignments.Add(roleAssignmentsAuthor);

                SPListItem ItemOrgaos = ImpersonatedWeb.Lists["Gerência Responsável MTC"].GetItemById(new SPFieldLookupValue(item["OrgaoResponsavelMTC"].ToString()).LookupId);
                //Imediatos
                SPRoleDefinition roleImediato = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Contributor);
                foreach (string email in Convert.ToString(ItemOrgaos["Gerente Imediato"]).Split(';'))
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

                SPFieldUserValueCollection usersGerencia = new SPFieldUserValueCollection(ImpersonatedWeb, Convert.ToString(ItemOrgaos["Responsáveis"]));
                foreach (SPFieldUserValue user in usersGerencia)
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

        public static void PermissaoCancelado(SPListItem item)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite ImpersonatedSite = new SPSite(item.Web.Url);
                SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();

                SPListItem itemN = ImpersonatedWeb.Lists["Manual Técnico de Campo"].GetItemById(item.ID);

                //Quebra a herança
                if (!itemN.HasUniqueRoleAssignments)
                {
                    itemN.BreakRoleInheritance(false, true);
                }

                //Grupo Administradores
                SPGroup group = ImpersonatedWeb.SiteGroups[Administradores];
                SPRoleAssignment roleAssignments = new SPRoleAssignment(group);
                itemN.RoleAssignments.Remove(group);

                SPRoleDefinition roleDefinition = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Reader);
                roleAssignments.RoleDefinitionBindings.Add(roleDefinition);
                itemN.RoleAssignments.Add(roleAssignments);

                //Grupo Administradores MTC
                SPGroup groupMTC = ImpersonatedWeb.SiteGroups[AdministradoresMTC];
                SPRoleAssignment roleAssignmentsMTC = new SPRoleAssignment(groupMTC);
                itemN.RoleAssignments.Remove(groupMTC);

                roleAssignmentsMTC.RoleDefinitionBindings.Add(roleDefinition);
                itemN.RoleAssignments.Add(roleAssignmentsMTC);

                //Demais Users
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

                ImpersonatedWeb.Close(); ImpersonatedWeb.Dispose();
                ImpersonatedSite.Close(); ImpersonatedSite.Dispose();

            });
        }

        public static void PermissaoVigente(SPListItem item)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite ImpersonatedSite = new SPSite(item.Web.Url);
                SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();

                SPListItem itemN = ImpersonatedWeb.Lists["Manual Técnico de Campo"].GetItemById(item.ID);

                //Quebra a herança
                if (!itemN.HasUniqueRoleAssignments)
                {
                    itemN.BreakRoleInheritance(false, true);
                }

                //Grupo Administradores
                SPGroup group = ImpersonatedWeb.SiteGroups[Administradores];
                SPRoleAssignment roleAssignments = new SPRoleAssignment(group);
                itemN.RoleAssignments.Remove(group);

                SPRoleDefinition roleDefinition = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Reader);
                /*roleAssignments.RoleDefinitionBindings.Add(roleDefinition);
                itemN.RoleAssignments.Add(roleAssignments);*/

                //Grupo Administradores MTC
                SPGroup groupMTC = ImpersonatedWeb.SiteGroups[AdministradoresMTC];
                SPRoleAssignment roleAssignmentsMTC = new SPRoleAssignment(groupMTC);
                itemN.RoleAssignments.Remove(groupMTC);

                roleAssignmentsMTC.RoleDefinitionBindings.Add(roleDefinition);
                itemN.RoleAssignments.Add(roleAssignmentsMTC);

                //Demais Users
                SPRoleAssignmentCollection SPRoleAssColn = itemN.RoleAssignments;
                for (int i = SPRoleAssColn.Count - 1; i >= 0; i--)
                {
                    SPRoleAssignment roleAssignmentSingle = SPRoleAssColn[i];
                    if (roleAssignmentSingle.Member.GetType().Name == "SPUser")
                    {
                        SPPrincipal user = roleAssignmentSingle.Member;
                        itemN.RoleAssignments.Remove(i);
                    }
                }

                SPGroup groupMTCC = ImpersonatedWeb.SiteGroups["Visualizadores GGA"];
                SPRoleAssignment roleAssignmentsMTCC = new SPRoleAssignment(groupMTCC);
                roleAssignmentsMTCC.RoleDefinitionBindings.Add(roleDefinition);
                itemN.RoleAssignments.Add(roleAssignmentsMTCC);

                ImpersonatedWeb.Close(); ImpersonatedWeb.Dispose();
                ImpersonatedSite.Close(); ImpersonatedSite.Dispose();

            });
        }

        public static void PermissaoParaComentarios(SPListItem item)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPSite ImpersonatedSite = new SPSite(item.Web.Url);
                SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();

                SPListItem itemN = ImpersonatedWeb.Lists["Manual Técnico de Campo"].GetItemById(item.ID);

                //Quebra a herança
                if (!itemN.HasUniqueRoleAssignments)
                {
                    itemN.BreakRoleInheritance(false, true);
                }

                //Retira todos usuários, deixa somente grupos
                SPRoleAssignmentCollection SPRoleAssColn = itemN.RoleAssignments;
                for (int i = SPRoleAssColn.Count - 1; i >= 0; i--)
                {
                    SPRoleAssignment roleAssignmentSingle = SPRoleAssColn[i];
                    if (roleAssignmentSingle.Member.GetType().Name == "SPUser")
                    {
                        SPPrincipal user = roleAssignmentSingle.Member;
                        itemN.RoleAssignments.Remove(i);
                    }
                }

                //Autor
                /*SPRoleDefinition roleAuthor = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Reader);
                SPPrincipal Author = (SPPrincipal)new SPFieldUserValue(ImpersonatedWeb, Convert.ToString(itemN[SPBuiltInFieldId.Author])).User;
                SPRoleAssignment roleAssignmentsAuthor = new SPRoleAssignment(Author);
                roleAssignmentsAuthor.RoleDefinitionBindings.Add(roleAuthor);
                itemN.RoleAssignments.Add(roleAssignmentsAuthor);*/
                
                SPRoleDefinition roleColab = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Contributor);
                SPListItemCollection orgaos = itemN.Web.Lists["Gerência Responsável MTC"].GetItems();

                foreach (SPListItem orgao in orgaos)
                {
                    SPFieldUserValueCollection usersGerencia = new SPFieldUserValueCollection(item.Web, Convert.ToString(orgao["Responsáveis"]));

                    foreach (SPFieldUserValue userGerencia in usersGerencia)
                    {
                        SPRoleAssignment roleAssignment = new SPRoleAssignment(userGerencia.User);
                        SPRoleDefinition roleDefinition = ImpersonatedWeb.RoleDefinitions.GetByType(SPRoleType.Contributor);
                        roleAssignment.RoleDefinitionBindings.Add(roleDefinition);

                        itemN.RoleAssignments.Add(roleAssignment);
                    }

                    foreach (string email in Convert.ToString(orgao["Gerente Imediato"]).Split(';'))
                    {
                        if (email != "null" && email != "")
                        {
                            SPPrincipalInfo u = SPUtility.ResolvePrincipal(ImpersonatedWeb, email, SPPrincipalType.All, SPPrincipalSource.All, null, true);
                            SPUser user = ImpersonatedWeb.EnsureUser(u.LoginName); //ImpersonatedWeb.SiteUsers.GetByEmail(email);
                            SPRoleAssignment roleAssignmentsImediato = new SPRoleAssignment(user);
                            roleAssignmentsImediato.RoleDefinitionBindings.Add(roleColab);
                            itemN.RoleAssignments.Add(roleAssignmentsImediato);
                        }
                    }
                }

                ImpersonatedWeb.Close(); ImpersonatedWeb.Dispose();
                ImpersonatedSite.Close(); ImpersonatedSite.Dispose();

            });
        }
    }
}
