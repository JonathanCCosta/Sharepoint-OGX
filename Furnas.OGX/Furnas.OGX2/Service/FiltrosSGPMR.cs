using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace Furnas.OGX2.Service
{
    public class FiltrosSGPMR
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

        internal static void LoadDropDownH(System.Web.UI.HtmlControls.HtmlSelect ddlItems, SPWeb sPWeb, string listname, string lookupcolumn)
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
            query.Query = "<Where><Eq><FieldRef Name='"+ field +"' LookupId='TRUE' /><Value Type='Lookup'>" + lookupcolumn + "</Value></Eq></Where>";

            SPListItemCollection itens = list.GetItems(query);
            ddlItems.Items.Clear();
            ddlItems.Items.Insert(0, new ListItem("-- Selecione uma Função de Transmissão --","0"));
            foreach (SPListItem item in itens)
            {
                ListItem lt = new ListItem();
                lt.Value = item.ID.ToString();
                lt.Text = item.Title;

                ddlItems.Items.Add(lt);
            }

            List<ListItem> itemsDDL = ddlItems.Items.Cast<ListItem>().OrderBy(lt => lt.Text).ToList();
            ddlItems.Items.Clear();
            ddlItems.Items.AddRange(itemsDDL.ToArray());
        }

        internal static void LoadRegistrosExistentes(System.Web.UI.WebControls.DropDownList ddlItems, SPWeb sPWeb, string listname, string field)
        {
            SPList list = sPWeb.Lists[listname];

            SPQuery query = new SPQuery();
            query.Query = "<Where><IsNotNull><FieldRef Name='" + field + "' /></IsNotNull></Where>";

            SPListItemCollection itens = list.GetItems(query);

            foreach (SPListItem item in list.Items)
            {
                if (item[field] != null)
                {
                    ListItem lt = new ListItem();
                    lt.Value = item[field].ToString();
                    lt.Text = item[field].ToString();
                    ddlItems.Items.Add(lt);
                }
            }

            List<ListItem> itemsDDL = ddlItems.Items.Cast<ListItem>().OrderBy(lt => lt.Text).ToList();
            ddlItems.Items.Clear();
            ddlItems.Items.AddRange(itemsDDL.ToArray());
        }

        public static SPQuery MontarQueryFiltros(List<string> filtros, List<string> valores)
        {
            List<string> objColumns = new List<string>();
            int i = 0;
            foreach (string filtro in filtros)
            {
                if (filtro == "CodigoIdentificacao" || filtro == "FuncaoTransmissaoO" || filtro == "InstalacaoO")
                    objColumns.Add(filtro + ";Contains;Eq;" + valores[i]);
                else if (filtro == "PrevisaoImplantacao")
                    objColumns.Add(filtro + ";DateTime;Geq;" + valores[i]);
                else if (filtro == "PrevisaoImplantacaoF")
                    objColumns.Add("PrevisaoImplantacao;DateTime;Leq;" + valores[i]);
                else if (filtro == "StatusRevitalizacao")
                    objColumns.Add(filtro + ";Choice;Eq;" + valores[i]);
                else if (filtro == "Ciclo")
                    objColumns.Add(filtro + ";Lookup;Eq;" + valores[i]);
                else if (filtro == "ContentType")
                    objColumns.Add(filtro + ";Computed;Eq;" + valores[i]);
                else
                    objColumns.Add(filtro + ";Text;Eq;" + valores[i]);

                i++;
            }

            SPQuery query = new SPQuery();
            query.Query = CreateCAMLQuery(objColumns, "And", true);

            return query;
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
                    //if (parameters[i].Split(';')[0] != "Ciclo" && parameters[i].Split(';')[0] != "ContentType")
                    //    AppendEQ(sb, parameters[i], false);
                    if (parameters[i].Split(';')[0] == "ContentType")
                    {
                        if (!objetivo)
                        {
                            texto = AppendEQTexto(parameters[parameters.Count-1], parameters.Count); objetivo = true;
                        }
                    }
                    else if (parameters[i].Split(';')[0] == "FuncaoTransmissaoO")
                    {
                        AppendEQTextoContains(sb, parameters[i].Split(';')[3], "FuncaoTransmissao");
                    }
                    else if (parameters[i].Split(';')[0] == "InstalacaoO")
                    {
                        AppendEQTextoContains(sb, parameters[i].Split(';')[3], "Instalacao");
                    }
                    else if (parameters[i].Split(';')[0] == "Ciclo")
                        AppendEQ(sb, parameters[i], true);
                    else
                        AppendEQ(sb, parameters[i], false);

                    if (i > 0 && j > 0)
                    {
                        if (parameters[i].Split(';')[0] != "ContentType")
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

        public static string CreateCAMLQuery2(List<string> parameters, string orAndCondition, bool isIncludeWhereClause)
        {
            StringBuilder sb = new StringBuilder();
            bool objetivo = false; string texto = string.Empty;
            //int j = 0;
            for (int i = 0; i < parameters.Count; i++)
            {
                if (!string.IsNullOrEmpty(parameters[i].Split(';')[3]))
                {
                    if (parameters[i].Split(';')[0] != "Ciclo")
                    {
                        if (parameters[i].Split(';')[0] == "ContentType")
                        {
                            texto = AppendEQTexto(parameters[i], parameters.Count); objetivo = true;
                        }
                        else
                            AppendEQ(sb, parameters[i], false);
                    }
                    else
                        AppendEQ(sb, parameters[i], true);

                    if (!objetivo && i > 0)
                    {
                        sb.Insert(0, "<" + orAndCondition + ">");
                        sb.Append("</" + orAndCondition + ">");
                    }
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

            if (maisFiltros > 1)
                sb.AppendFormat("<And>");
            //else
            //    sb.AppendFormat("<Or>");

            if (value.Contains("Melhoria"))
                sb.AppendFormat("<Or><Eq><FieldRef Name='ContentType'/><Value Type='Computed'>Plano de Melhoria</Value></Eq><Eq><FieldRef Name='ContentType'/><Value Type='Computed'>Plano de Reforço</Value></Eq></Or>");
            else
                sb.AppendFormat("<Eq><FieldRef Name='ContentType'/><Value Type='Computed'>Plano Por Superação</Value></Eq>");

            return sb.ToString();
        }

        public static string AppendEQTextoContains(StringBuilder sb, string value, string campo)
        {
            sb.AppendFormat("<Contains><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Contains>", campo, value);
            return sb.ToString();
        }

        public static DataTable PesquisaPlano(SPWeb web, string numeracao, string tipoPlano)
        {
            
            DataTable item = null;
            try
            {
                SPQuery query = new SPQuery();
                StringBuilder sb = new StringBuilder();

                if (tipoPlano.Contains("Melhoria"))
                {
                    //sb.AppendFormat("<Where><Or><Eq><FieldRef Name='ContentType'/><Value Type='Computed'>Plano de Melhoria</Value></Eq><Eq><FieldRef Name='ContentType'/><Value Type='Computed'>Plano de Reforço</Value></Eq></Or>");
                    //sb.AppendFormat("<And><Eq><FieldRef Name='Title' /><Value Type='Text'>" + numeracao + "</Value></Eq></And></Where>");
                    query.Query = "<Where><And><Eq><FieldRef Name='Title' /><Value Type='Text'>"+ numeracao +"</Value>" +
                                  "</Eq><Or><Eq><FieldRef Name='ContentType'/><Value Type='Computed'>Plano de Melhoria</Value></Eq>" +
                                    "<Eq><FieldRef Name='ContentType'/><Value Type='Computed'>Plano de Reforço</Value></Eq></Or></And></Where>";
                }
                else
                {
                    //sb.AppendFormat("<Where><And><Eq><FieldRef Name='ContentType'/><Value Type='Computed'>Plano Por Superação</Value></Eq>");
                    //sb.AppendFormat("<Eq><FieldRef Name='Title' /><Value Type='Text'>" + numeracao + "</Value></Eq></And></Where>");
                    query.Query = "<Where><And><Eq><FieldRef Name='Title' /><Value Type='Text'>" + numeracao + "</Value>" +
                                  "</Eq><Eq><FieldRef Name='ContentType'/><Value Type='Computed'>Plano Por Superação</Value></Eq>" +
                                    "</And></Where>";
                }

                //query.Query = sb.ToString();
                item = web.Lists["Planos"].GetItems(query).GetDataTable();
            }
            catch (Exception e)
            {
                throw e;
            }

            return item;
        }
    }
}
