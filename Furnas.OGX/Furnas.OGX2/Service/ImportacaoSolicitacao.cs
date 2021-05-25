using ClosedXML.Excel;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Furnas.OGX2.Service
{
    public class ImportacaoSolicitacao
    {
        public static DataTable ReadExcel(Stream stream)
        {
            DataTable dt;
            try
            {
                //Open the Excel file using ClosedXML.
                using (XLWorkbook workBook = new XLWorkbook(stream))
                {
                    //Read the first Sheet from Excel file.
                    IXLWorksheet workSheet = workBook.Worksheet(1);

                    //Create a new DataTable.
                    dt = new DataTable();

                    //Loop through the Worksheet rows.
                    bool firstRow = true;

                    foreach (IXLRow row in workSheet.Rows())
                    {
                        //Use the first row to add columns to DataTable.
                        if (firstRow)
                        {
                            IXLCells cells = row.Cells();
                            foreach (IXLCell cell in cells)
                            {
                                //if (cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
                                dt.Columns.Add(cell.Value.ToString());
                            }
                            firstRow = false;
                        }
                        else
                        {
                            IXLCells cells = row.Cells();
                            //if (cells.First().Value != null && !string.IsNullOrEmpty(cells.First().Value.ToString()))
                            //{
                            //Add rows to DataTable.
                            //dt.Rows.Add();
                            int i = 0;
                            //int p = row.CellCount();
                            //TODO: Na hora de obter o valor do trimestre não estava obtendo quando a celula estava nula.
                            //Pra resolver tive que colocar o tipo, pelo menos nas celulas dos trimestres.
                            //foreach (IXLCell cell in cells)
                            try
                            {
                                foreach (IXLCell cell in row.Cells(row.FirstCellUsed().Address.ColumnNumber, row.LastCellUsed().Address.ColumnNumber))
                                {

                                    if (i == 0)
                                        dt.Rows.Add();

                                    dt.Rows[dt.Rows.Count - 1][i] = Convert.ToString(cell.Value);
                                    //dt.Rows.Add(cell.Value);
                                    i++;
                                }
                            }
                            catch { }
                            
                            //}
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }

        public static string ValidaPlanilha(DataTable data, string plano, SPWeb web)
        {
            Regex IsData = new Regex(@"^\d{2}\/\d{2}\/\d{4} \d{2}:\d{2}:\d{2}$");
            Regex IsDecimal = new Regex(@"^\d*[0-9](\,\d*[0-9])?$");

            StringBuilder sb = new StringBuilder();

            try
            {
                int indexRow = 2; bool sup = false; bool ms = false;
                foreach (DataRow row in data.Rows)
                {
                    int indexColl = 1;
                    foreach (DataColumn coll in row.Table.Columns)
                    {
                        if (coll.ColumnName == "Nome Do Vão")
                            sup = true;
                        else if (coll.ColumnName == "Tipo de Revitalização")
                            ms = true;

                            string Collumn = Colunas(coll.ColumnName);
                            string CollumnName = coll.ColumnName;
                            if ("Datas" == Collumn)
                            {
                                if (row[coll.ColumnName].ToString() == "")
                                {
                                    if (ColunasObrigatorias(CollumnName))
                                        sb.AppendLine("Na Coluna: " + ColunaPlanilha(indexColl.ToString()) + " - Linha " + indexRow.ToString() + ", o preenchimento é obrigatório.</br>");
                                }
                                else
                                {
                                    if (!IsData.IsMatch(row[coll.ColumnName].ToString()))
                                        sb.AppendLine("Na Coluna: " + ColunaPlanilha(indexColl.ToString()) + " - Linha " + indexRow.ToString() + ", só é permitido formato Data.</br>");
                                }
                            }
                            else if ("Numeros" == Collumn)
                            {
                                if (row[coll.ColumnName].ToString() == "")
                                {
                                    if (ColunasObrigatorias(CollumnName))
                                        sb.AppendLine("Na Coluna: " + ColunaPlanilha(indexColl.ToString()) + " - Linha " + indexRow.ToString() + ", o preenchimento é obrigatório.</br>");
                                }
                                else
                                {
                                    if (!IsDecimal.IsMatch(row[coll.ColumnName].ToString()))
                                        sb.AppendLine("Na Coluna: " + ColunaPlanilha(indexColl.ToString()) + " - Linha " + indexRow.ToString() + ", só é permitido números.</br>");
                                }
                            }
                            else if ("Moeda" == Collumn)
                            {
                                if (row[coll.ColumnName].ToString() == "")
                                {
                                    if (ColunasObrigatorias(CollumnName))
                                        sb.AppendLine("Na Coluna: " + ColunaPlanilha(indexColl.ToString()) + " - Linha " + indexRow.ToString() + ", o preenchimento é obrigatório.</br>");
                                }
                                else
                                {
                                    try
                                    {
                                        Decimal.Parse(row[coll.ColumnName].ToString().Replace(',', '.'));
                                    }
                                    catch
                                    {
                                        sb.AppendLine("Na Coluna: " + ColunaPlanilha(indexColl.ToString()) + " - Linha " + indexRow.ToString() + ", só é permitido números.</br>");
                                    }
                                }
                            }
                            else
                            {
                                if (row[coll.ColumnName].ToString() == "")
                                {
                                    if (ColunasObrigatorias(CollumnName))
                                        sb.AppendLine("Na Coluna: " + ColunaPlanilha(indexColl.ToString()) + " - Linha " + indexRow.ToString() + ", o preenchimento é obrigatório.</br>");
                                }
                            }
                        indexColl++;
                    }
                    indexRow++;
                }


                if (sup == false && ms == false)
                {
                    sb.Clear();
                    sb.AppendLine("Existem colunas da planilha importada que são incompatíveis com o tipo de solicitação selecionado.");
                }
                else if (sup == false && plano == "Plano por Superação - Mudanças")
                {
                    sb.Clear();
                    sb.AppendLine("Importação do Plano por Superação incompatível com o arquivo selecionado.");
                    //throw new Exception("Importação do " + plano + " incompatível com o arquivo selecionado.");
                }
                else if (sup == true && plano != "Plano por Superação - Mudanças")
                {
                    sb.Clear();
                    sb.AppendLine("Importação do " + plano + " incompatível com o arquivo selecionado.");
                    //throw new Exception("Importação do " + plano + " incompatível com o arquivo selecionado.");
                }

                return sb.ToString();
            }
            catch (Exception e)
            {
                //Log(web, e.Message.ToString(), plano);
                //throw e;
                return e.Message;
            }
        }

        public static string Colunas(string coluna)
        {
            string[] Campos_datas = { "Data de Necessidade Sistêmica", "Data da Autorização/Emissão PMI", "Previsão da Implantação", 
                     "Aquisição de Materiais - Início", "Aquisição de Materiais - Fim", "Obras Civis - Início", "Obras Civis - Fim", "Montagem Eletromecânica - Início", "Montagem Eletromecânica - Fim", "Comissionamento - Início", "Comissionamento - Fim", "Operação Comercial", 
                     "Data de Necessidade", "Data da Autorização", "Data de Entrada em Operação",
                     "Previsão de implantação FURNAS", "Data da última atualização de informações"};
            string[] Campos_numeros = { "Aumento de vida útil esperado, a partir da implantação Do reforço(anos)", "Ano Final de Vida Útil", "Ano Início em Operação" };
            string[] Campos_moedas = { "Valor a ser investido(R$)", "Custo Estimado (R$)" };
            
            if (coluna == "Numeração")
                return coluna;
            else if (Campos_datas.Contains(coluna))
                return "Datas";
            else if (Campos_numeros.Contains(coluna))
                return "Numeros";
            else if (Campos_moedas.Contains(coluna))
                return "Moeda";
            else
                return "Textos";
        }

        public static bool ColunasObrigatorias(string coluna)
        {
            string[] campos = { "Região", "Agente", "Instalação", "Tensão Nominal (KV)", "Tipo de Agente",
            "Função de Transmissão", "Origem da Indicação", "Revitalização Necessária",
            "Classificação", "Prazo após emissão Do PMI/Resolução Aneel (meses)",
            "Status da Revitalização", "Previsão de implantação FURNAS", "Status de revitalização FURNAS",
            "Órgão solicitante", "Responsável no órgão solicitante", "Órgão Gestor da obra", "Responsável no órgão gestor da obra"};

            if(campos.Contains(coluna))
                return true;

            return false;
        }

        public static string ColunaPlanilha(string colunaInt)
        {
            #region Switch Colunas
            switch (colunaInt)
            {
                case "1":
                    return "A";
                case "2":
                    return "B";
                case "3":
                    return "C";
                case "4":
                    return "D";
                case "5":
                    return "E";
                case "6":
                    return "F";
                case "7":
                    return "G";
                case "8":
                    return "H";
                case "9":
                    return "I";
                case "10":
                    return "J";
                case "11":
                    return "K";
                case "12":
                    return "L";
                case "13":
                    return "M";
                case "14":
                    return "N";
                case "15":
                    return "O";
                case "16":
                    return "P";
                case "17":
                    return "Q";
                case "18":
                    return "R";
                case "19":
                    return "S";
                case "20":
                    return "T";
                case "21":
                    return "U";
                case "22":
                    return "V";
                case "23":
                    return "W";
                case "24":
                    return "X";
                case "25":
                    return "Y";
                case "26":
                    return "Z";
                case "27":
                    return "AA";
                case "28":
                    return "AB";
                case "29":
                    return "AC";
                case "30":
                    return "AD";
                case "31":
                    return "AE";
                case "32":
                    return "AF";
                case "33":
                    return "AG";
                case "34":
                    return "AH";
                case "35":
                    return "AI";
                case "36":
                    return "AJ";
                case "37":
                    return "AK";
                case "38":
                    return "AL";
                case "39":
                    return "AM";
                case "40":
                    return "AN";
                case "41":
                    return "AO";
                case "42":
                    return "AP";
                case "43":
                    return "AQ";
                case "44":
                    return "AR";
                case "45":
                    return "AS";
                case "46":
                    return "AT";
                case "47":
                    return "AU";
                case "48":
                    return "AV";
                case "49":
                    return "AW";
                case "50":
                    return "AX";
                case "51":
                    return "AY";
                case "52":
                    return "AZ";
                case "53":
                    return "BA";
                case "54":
                    return "BB";
                case "55":
                    return "BC";
                case "56":
                    return "BD";
                case "57":
                    return "BE";
                case "58":
                    return "BF";
                case "59":
                    return "BG";
                case "60":
                    return "BH";
                case "61":
                    return "BI";
                case "62":
                    return "BJ";
                case "63":
                    return "BK";
                case "64":
                    return "BL";
                case "65":
                    return "BM";
                case "66":
                    return "BN";
                case "67":
                    return "BO";
                case "68":
                    return "BP";
                case "69":
                    return "BQ";
                case "70":
                    return "BR";
                case "71":
                    return "BS";
                case "72":
                    return "BT";
                case "73":
                    return "BU";
                case "74":
                    return "BV";
                case "75":
                    return "BW";
                case "76":
                    return "BX";
                case "77":
                    return "BY";
                case "78":
                    return "BZ";
                case "79":
                    return "CA";
                case "80":
                    return "CB";
                case "81":
                    return "CC";
                case "82":
                    return "CD";
                case "83":
                    return "CE";
                case "84":
                    return "CF";
                case "85":
                    return "CG";
                case "86":
                    return "CH";
                case "87":
                    return "CI";
                case "88":
                    return "CJ";
                default:
                    return colunaInt;
            }
            #endregion
        }

        public static void ImportDadosSolicitacao(DataTable data, SPWeb web, string Plano, int lote)
        {
            try
            {
                SPList Solicitacoes = web.Lists["Solicitação de Mudanças"];
                SPList gestao = web.Lists["Gestão Interna"];
                foreach (DataRow row in data.Rows)
                {
                    SPListItem item = Solicitacoes.AddItem();
                    item[SPBuiltInFieldId.ContentTypeId] = item.ListItems.List.ContentTypes[Plano].Id;

                    SPListItem itemGestao = gestao.AddItem();
                    
                    int indexColl = 1;
                    foreach (DataColumn coll in row.Table.Columns)
                    {
                        if ((Plano == "Plano por Superação - Mudanças" && indexColl < 71) || (Plano != "Plano por Superação - Mudanças" && indexColl < 54))
                        {
                            if (row[coll.ColumnName].ToString() != string.Empty)
                            {
                                string Collumn = Colunas(coll.ColumnName);

                                if ("Datas" == Collumn)
                                {
                                    try { item[coll.ColumnName] = TratarData(row[coll.ColumnName]); }
                                    catch (Exception eDT) { Log(web, eDT.Message.ToString(), Plano); }
                                }
                                else if ("Numeros" == Collumn)
                                {
                                    try { item[coll.ColumnName] = Convert.ToInt32(row[coll.ColumnName].ToString()); }
                                    catch (Exception eNumeros) { Log(web, eNumeros.Message.ToString(), Plano); }
                                }
                                else if ("Moeda" == Collumn)
                                {
                                    try
                                    {
                                        string valorInvest = row[coll.ColumnName].ToString();
                                        if (valorInvest.Substring(valorInvest.Length - 3, 1) == ",")
                                            item[coll.ColumnName] = valorInvest.Replace(".", ",").Remove(valorInvest.Length - 3).Insert(valorInvest.Length - 3, "." + valorInvest.Substring(valorInvest.Length - 2, 2));
                                        else
                                            item[coll.ColumnName] = valorInvest;
                                    }
                                    catch (Exception eMoeda) { Log(web, eMoeda.Message.ToString(), Plano); }
                                }
                                else if ("Textos" == Collumn)
                                {
                                    SPListItem itemInstalação = null;
                                    if (coll.ColumnName == "Instalação")
                                    {
                                        SPList instalacao = web.Lists["Domínio de Instalação"];

                                        itemInstalação = instalacao.Items.OfType<SPListItem>().Where(p => p[SPBuiltInFieldId.Title].ToString() == row[coll.ColumnName].ToString()).FirstOrDefault();
                                        if (itemInstalação != null)
                                            item[coll.ColumnName] = new SPFieldLookupValue(itemInstalação.ID, itemInstalação[SPBuiltInFieldId.Title].ToString());
                                        else
                                        {
                                            itemInstalação = instalacao.Items.OfType<SPListItem>().Where(p => p[SPBuiltInFieldId.Title].ToString() == "Outra Instalação").FirstOrDefault();
                                            item[coll.ColumnName] = new SPFieldLookupValue(itemInstalação.ID, itemInstalação[SPBuiltInFieldId.Title].ToString());

                                            item["Instalação - Outros"] = row[coll.ColumnName].ToString();
                                        }
                                    }
                                    else if (coll.ColumnName == "Função de Transmissão")
                                    {
                                        SPList funcao = web.Lists["Domínio de Função de Transmissão"];

                                        SPListItem itemFuncao = funcao.Items.OfType<SPListItem>().Where(p => p[SPBuiltInFieldId.Title].ToString() == row[coll.ColumnName].ToString() && new SPFieldLookupValue(Convert.ToString(p["Domínio de Instalação"])).LookupId == itemInstalação.ID).FirstOrDefault();
                                        if (itemFuncao != null)
                                            item[coll.ColumnName] = new SPFieldLookupValue(itemFuncao.ID, itemFuncao[SPBuiltInFieldId.Title].ToString());
                                        else
                                        {
                                            itemFuncao = funcao.Items.OfType<SPListItem>().Where(p => p[SPBuiltInFieldId.Title].ToString() == "Outra Função de transmissão").FirstOrDefault();
                                            item[coll.ColumnName] = new SPFieldLookupValue(itemFuncao.ID, itemFuncao[SPBuiltInFieldId.Title].ToString());

                                            item["Função de Transmissão - Outros"] = row[coll.ColumnName].ToString();
                                        }
                                    }
                                    else if (coll.ColumnName == "Classificação")
                                    {
                                        SPList classificacao = web.Lists["Domínios de Classificação"];

                                        SPListItem itemClassificacao = classificacao.Items.OfType<SPListItem>().Where(p => p[SPBuiltInFieldId.Title].ToString() == row[coll.ColumnName].ToString()).FirstOrDefault();
                                        if (itemClassificacao != null)
                                            item[coll.ColumnName] = new SPFieldLookupValue(itemClassificacao.ID, itemClassificacao[SPBuiltInFieldId.Title].ToString());
                                        else
                                        {
                                            itemClassificacao = classificacao.Items.OfType<SPListItem>().Where(p => p[SPBuiltInFieldId.Title].ToString() == "Outra Classificação").FirstOrDefault();
                                            item[coll.ColumnName] = new SPFieldLookupValue(itemClassificacao.ID, itemClassificacao[SPBuiltInFieldId.Title].ToString());
                                        }
                                    }
                                    else
                                    {
                                        try { item[coll.ColumnName] = row[coll.ColumnName].ToString(); }
                                        catch (Exception eGeral) { Log(web, eGeral.Message.ToString(), Plano); }
                                    }
                                }
                            } //eLinha++;
                        }
                        else
                        {
                            if (row[coll.ColumnName].ToString() != string.Empty)
                            {
                                string Collumn = Colunas(coll.ColumnName);

                                if ("Datas" == Collumn)
                                {
                                    try { itemGestao[coll.ColumnName] = TratarData(row[coll.ColumnName]); }
                                    catch (Exception eDT) { Log(web, eDT.Message.ToString(), Plano); }
                                }
                                else if ("Numeros" == Collumn)
                                {
                                    try { itemGestao[coll.ColumnName] = Convert.ToInt32(row[coll.ColumnName].ToString()); }
                                    catch (Exception eNumeros) { Log(web, eNumeros.Message.ToString(), Plano); }
                                }
                                else if ("Moeda" == Collumn)
                                {
                                    try
                                    {
                                        string valorInvest = row[coll.ColumnName].ToString();
                                        if (valorInvest.Substring(valorInvest.Length - 3, 1) == ",")
                                            itemGestao[coll.ColumnName] = valorInvest.Replace(".", ",").Remove(valorInvest.Length - 3).Insert(valorInvest.Length - 3, "." + valorInvest.Substring(valorInvest.Length - 2, 2));
                                        else
                                            itemGestao[coll.ColumnName] = valorInvest;
                                    }
                                    catch (Exception eMoeda) { Log(web, eMoeda.Message.ToString(), Plano); }
                                }
                                else
                                {
                                    try { itemGestao[coll.ColumnName] = row[coll.ColumnName].ToString(); }
                                    catch (Exception eGeral) { Log(web, eGeral.Message.ToString(), Plano); }
                                }
                            }
                        }
                        indexColl++;
                    }

                    //itemGestao["No. SGPMR"] = item.ID.ToString();
                    itemGestao.Update();

                    item["Id Log"] = lote;
                    item["Número de Gestão"] = itemGestao.ID.ToString();
                    item.Update();
                }
            }
            catch (Exception e)
            {
                Log(web, e.Message.ToString(), Plano);
                throw new Exception("Ocorreu um erro durante a importação. Tente mais tarde.");
            }
        }

        public static void Log(SPWeb web, string erro, string plano)
        {
            try
            {
                SPList GerenciaPlanos = web.Lists["Log Solicitação Novos Planos"];
                SPListItem item = GerenciaPlanos.AddItem();

                item[SPBuiltInFieldId.Title] = plano;
                item["Descrição"] = erro;
                item.Update();
            }
            catch { }
        }

        private static DateTime? TratarData(object item)
        {
            if (item == null || item.ToString() == "")
                return null;

            int dia = Convert.ToInt32(item.ToString().Split('/').First());
            int mes = Convert.ToInt32(item.ToString().Split('/')[1]);
            int ano = Convert.ToInt32(item.ToString().Split('/').Last().Substring(0, 4));//11/01/2016 00:00:00

            return new DateTime(ano, mes, dia);
        }



    }
}
