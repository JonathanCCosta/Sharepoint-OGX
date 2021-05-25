using Microsoft.SharePoint;
using Microsoft.SharePoint.Taxonomy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace Furnas.OGX2.MTC.ServicesMTC
{
    public class Filtros
    {
        internal static void LoadDropDown(System.Web.UI.WebControls.DropDownList ddlItems, SPWeb sPWeb, string listname, string lookupcolumn)
        {
            SPList list = sPWeb.Lists[listname];

            foreach (SPListItem item in list.Items)
            {
                ListItem lt = new ListItem();
                lt.Value = item.ID.ToString();
                if (lookupcolumn == "")
                    lt.Text = item.Title;
                else
                    lt.Text = item[lookupcolumn].ToString();

                ddlItems.Items.Add(lt);
            }

            List<ListItem> itemsDDL = ddlItems.Items.Cast<ListItem>().OrderBy(lt => lt.Text).ToList();
            ddlItems.Items.Clear();
            ddlItems.Items.AddRange(itemsDDL.ToArray());
        }

        internal static void LoadDropDownLookup(System.Web.UI.WebControls.DropDownList ddlItems, SPWeb sPWeb, string listname, string lookupcolumn, string field)
        {
            SPList list = sPWeb.Lists[listname];
            
            SPQuery query = new SPQuery();
            query.Query = "<Where><Eq><FieldRef Name='Grupo' LookupId='TRUE' /><Value Type='Lookup'>"+ lookupcolumn +"</Value></Eq></Where>";

            SPListItemCollection itens = list.GetItems(query);
            ddlItems.Items.Clear();
            ddlItems.Items.Insert(0, new ListItem("-- Selecione um Subgrupo --"));
            foreach (SPListItem item in itens)
            {
                ListItem lt = new ListItem();
                lt.Value = item.ID.ToString();
                lt.Text = item[field].ToString();

                ddlItems.Items.Add(lt);
            }

            List<ListItem> itemsDDL = ddlItems.Items.Cast<ListItem>().OrderBy(lt => lt.Text).ToList();
            ddlItems.Items.Clear();
            ddlItems.Items.AddRange(itemsDDL.ToArray());
        }

        public static TermCollection Termos(SPSite site)
        {
            TermCollection all = null;
            try
            {
                TaxonomySession taxonomySession = new TaxonomySession(site);
                TermStore termStore = taxonomySession.TermStores[0];
                if (termStore != null)
                {
                    foreach (Group group in termStore.Groups)
                    {
                        if (group.Name.Contains("donet"))//("portalativosfurnas"))
                        //if (group.Name.Contains("portalativosfurnas"))
                        {
                            TermSetCollection termSetColl = group.TermSets;
                            foreach (TermSet termSet in termSetColl)
                            {
                                if (termSet.Name == "Palavra Chave")
                                {
                                    all = termSet.Terms;
                                }
                            }
                        }
                    }
                }
            }
            catch { }
            return all;
        }

        public static string CreateCAMLQuery(List<string> parameters, string orAndCondition, bool isIncludeWhereClause)
        {
            StringBuilder sb = new StringBuilder();
            bool objetivo = false; string texto = string.Empty;
            int j = 0;
            for (int i = 0; i < parameters.Count; i++)
            {
                if (!string.IsNullOrEmpty(parameters[i].Split(';')[3]))
                {
                    if (parameters[i].Split(';')[0] == "Palavra_x0020_Chave" || parameters[i].Split(';')[0] == "ControleDeStatusMTC" || parameters[i].Split(';')[0] == "DataInicioVigencia")
                        AppendEQ(sb, parameters[i], false);
                    else if (parameters[i].Split(';')[0] == "Objetivo" || parameters[i].Split(';')[0] == "DescricaoMTC")
                    {
                        if (!objetivo)
                        {
                            texto = AppendEQTexto(parameters[i], parameters.Count); objetivo = true;
                        }
                    }
                    else
                        AppendEQ(sb, parameters[i], true);

                    if (i > 0 && j > 0)
                    {
                        if (parameters[i].Split(';')[0] != "Objetivo" && parameters[i].Split(';')[0] != "DescricaoMTC")
                        {
                            sb.Insert(0, "<" + orAndCondition + ">");
                            sb.Append("</" + orAndCondition + ">");
                        }
                    }
                    j++;
                }
            }
            if (isIncludeWhereClause)
            {
                if (objetivo)
                {
                    if (parameters.Count > 1)
                    {
                        sb.Insert(0, "<Where>" + texto);
                        sb.Append("</And></Where>");
                    }
                    else
                    {
                        sb.Insert(0, "<Where>" + texto);
                        sb.Append("</Where>");
                    }
                }
                else
                {
                    sb.Insert(0, "<Where>");
                    sb.Append("</Where>");
                }
            }
            return sb.ToString();
        }

        public static void AppendEQ(StringBuilder sb, string value, bool isLookup)
        {
            string[] field = value.Split(';');
            sb.AppendFormat("<{0}>", field[2].ToString());
            if (isLookup)
                sb.AppendFormat("<FieldRef Name='{0}' LookupId='true' />", field[0].ToString());
            else
                sb.AppendFormat("<FieldRef Name='{0}'/>", field[0].ToString());

            sb.AppendFormat("<Value Type='{0}'>{1}</Value>", field[1].ToString(), field[3].ToString());
            sb.AppendFormat("</{0}>", field[2].ToString());
        }

        public static string AppendEQTexto(string value, int maisFiltros)
        {
            string[] field = value.Split(';');
            StringBuilder sb = new StringBuilder();
            //sb.AppendFormat("<Or><{0}>", field[2].ToString());
            if(maisFiltros > 1)
                sb.AppendFormat("<And><Or>");
            else
                sb.AppendFormat("<Or>");
            sb.AppendFormat("<Contains><FieldRef Name='Objetivo'/><Value Type='Note'>{0}</Value></Contains><Contains><FieldRef Name='DescricaoMTC'/><Value Type='Note'>{0}</Value></Contains>", field[3].ToString());
            sb.AppendFormat("</Or>");
            return sb.ToString();
        }

        public static SPQuery MontarQueryFiltros(List<string> filtros, List<string> valores)
        {
            List<string> objColumns = new List<string>();
            int i = 0;
            foreach (string filtro in filtros)
            {
                if (filtro == "Palavra_x0020_Chave")
                    objColumns.Add(filtro + ";Contains;Eq;" + valores[i]);
                else if (filtro == "DataInicioVigencia")
                    objColumns.Add(filtro + ";DateTime;Geq;" + valores[i]);
                else if (filtro == "DataInicioVigenciaF")
                    objColumns.Add("DataInicioVigencia;DateTime;Leq;" + valores[i]);
                else if (filtro == "ControleDeStatusMTC")
                    objColumns.Add(filtro + ";Choice;Eq;" + valores[i]);
                else if (filtro == "Objetivo")//filtro == "DescricaoMTC") || filtro == "Objetivo")
                    objColumns.Add(filtro + ";Note;Contains;" + valores[i]);
                else
                    objColumns.Add(filtro + ";Lookup;Eq;" + valores[i]);

                i++;
            }

            SPQuery query = new SPQuery();
            query.Query = CreateCAMLQuery(objColumns, "And", true);

            return query;
        }

        public static bool Perfil(SPWeb web, SPUser user, int idOrgao)
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

        private static bool E_admin(SPWeb web, string groupName, string userName)
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

        public static string Responsavel(SPWeb web)
        {
            SPUser user = web.CurrentUser;
            bool Eadmin = false; bool Eresposansavel = false;
            if (Filtros.E_admin(web, "Administradores GGA MTC", user.Name))
            {
                return "Admin";
            }

            if (!Eadmin)
            {
                SPListItemCollection items = web.Lists["Gerência Responsável MTC"].GetItems();

                foreach (SPListItem orgaos in items)
                {
                    foreach (string email in Convert.ToString(orgaos["Gerente Imediato"]).Split(';'))
                    {
                        if (email != "null" && email != "")
                        {
                            if (user.Email == email)
                            {
                                return orgaos[SPBuiltInFieldId.Title].ToString();
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
                                return orgaos[SPBuiltInFieldId.Title].ToString();
                            }
                        }
                    }

                    if (Eresposansavel)
                    {
                        break;
                    }
                }
            }

            return "Publico";
        }
    }
}
