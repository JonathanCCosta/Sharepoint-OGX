using ClosedXML.Excel;
using ExcelDataReader;
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
    public class Importacao
    {
        public static DataTable ReadExcelValida(Stream stream)
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
                                if (cell.Value.ToString() == "Classificação" && cell.Address.ColumnNumber > 35)
                                    dt.Columns.Add("Classificação ONS");
                                else if (cell.Value.ToString() == "Classificação" && cell.Address.ColumnNumber < 35)
                                    dt.Columns.Add("Classificação Furnas");
                                else if (cell.Value.ToString() == "(KV)")
                                    dt.Columns.Add("KV");
                                else if (cell.Value.ToString() == "Prazo após emissão Do PMI/Resolução Aneel (meses)")
                                    dt.Columns.Add("Prazo após emissão Do PMI/Resolução Aneel");
                                else if (cell.Value.ToString() == "Custo Estimado (R$)")
                                    dt.Columns.Add("Custo Estimado");
                                else if (cell.Value.ToString() == "Taxa de Depreciação (MCPSE – versão 9)")
                                    dt.Columns.Add("Taxa de Depreciação");
                                else if (cell.Value.ToString() == "Substituir nos próximos 4 anos (a partir de 1º de fevereiro Do ano corrente)?")
                                    dt.Columns.Add("Substituir nos próximos 4 anos?");
                                else if (cell.Value.ToString() == "Aumento de vida útil esperado, a partir da implantação Do reforço(anos)")
                                    dt.Columns.Add("Aumento de vida útil esperado, a partir da implantação Do reforço");
                                else if (cell.Value.ToString() == "Valor a ser investido(R$)")
                                    dt.Columns.Add("Valor a ser Investido");
                                else if (cell.Value.ToString() == "Identificação Do equipamento a ser substituído (número operativo, Do unifilar, etc.)")
                                    dt.Columns.Add("Identificação Do equipamento a ser substituído");
                                else if (cell.Value.ToString() == "Nº da recomendação no SGR")
                                    dt.Columns.Add("N° da recomendação no SGR");
                                else if (cell.Value.ToString() == "Nº Série")
                                    dt.Columns.Add("N° Série");
                                else if (cell.Value.ToString() == "Aquisição de Materiais - Início")
                                    dt.Columns.Add("Início da Aquisição Materiais");
                                else if (cell.Value.ToString() == "Aquisição de Materiais - Fim")
                                    dt.Columns.Add("Fim da Aquisição Materiais");
                                else if (cell.Value.ToString() == "Obras Civis - Início")
                                    dt.Columns.Add("Início das Obras Civis");
                                else if (cell.Value.ToString() == "Obras Civis - Fim")
                                    dt.Columns.Add("Fim das Obras Civis");
                                else if (cell.Value.ToString() == "Montagem Eletromecânica - Início")
                                    dt.Columns.Add("Início da Montagem Eletromecânica");
                                else if (cell.Value.ToString() == "Montagem Eletromecânica - Fim")
                                    dt.Columns.Add("Fim da Montagem Eletromecânica");
                                else if (cell.Value.ToString() == "Comissionamento - Início")
                                    dt.Columns.Add("Início Comissionamento");
                                else if (cell.Value.ToString() == "Comissionamento - Fim")
                                    dt.Columns.Add("Fim Comissionamento");
                                else
                                    dt.Columns.Add(cell.Value.ToString());
                            }
                            firstRow = false;
                        }
                        else
                        {
                            IXLCells cells = row.Cells();

                            dt.Rows.Add();
                            int i = 0;

                            foreach (IXLCell cell in cells)
                            {
                                dt.Rows[dt.Rows.Count - 1][i] = Convert.ToString(cell.Value);

                                i++;
                            }
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

        public static DataTable ReadExceX(Stream stream, SPWeb web)
        {
            try
            {
               IExcelDataReader reader = null;
                reader = ExcelReaderFactory.CreateBinaryReader(stream); 
                DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });
                reader.Close();

                int indexRow = 0;
                foreach (DataRow d in result.Tables[0].Rows)
                {
                    if (indexRow == 0)
                    {
                        indexRow++; int indexCol = 0;
                        foreach (DataColumn c in d.Table.Columns)
                        {
                            string termo = CaracterWeb(c.ColumnName);
                            //if (termo != c.ColumnName)
                            //    c.ColumnName = termo;
                            if (termo == "Classificação" && indexCol > 35)
                                c.ColumnName = "Classificação ONS";
                            else if (termo == "Classificação" && indexCol < 35)
                                c.ColumnName="Classificação Furnas";
                            else if (termo == "Nº da recomendação no SGR")
                                c.ColumnName = "N° da recomendação no SGR";
                            else if (termo == "Nº Série")
                                c.ColumnName = "N° Série";
                            else if (termo == "Observação" && indexCol > 50)
                                c.ColumnName = "Observação ONS";
                            else if (termo == "Observação" && indexCol < 50)
                                c.ColumnName = "Observação Furnas";
                            else
                                c.ColumnName = termo;

                            indexCol++;
                        }
                    }
                    else
                        break;
                }
                return result.Tables[0];
            }
            catch (Exception e)
            {
                Log(web, e.Message.ToString(), "Leitura do Excel");
                throw new Exception(e.Message);
            }
            
        }

        public static string CaracterWeb(string termo)
        {
            #region Switch
            switch (termo)
            {
                case "RegiÃ£o":
                    return "Região";
                case "NumeraÃ§Ã£o":
                    return "Numeração";
                case "InstalaÃ§Ã£o":
                    return "Instalação";
                case "TensÃ£o Nominal (kV)":
                    return "Tensão Nominal (kV)";
                case "RazÃ£o principal para a superaÃ§Ã£o deste equipamento":
                    return "Razão principal para a superação deste equipamento";
                case "Nome Do VÃ£o":
                    return "Nome Do Vão";
                case "CÃ³digo de IdentificaÃ§Ã£o":
                    return "Código de Identificação";
                case "FunÃ§Ã£o TransmissÃ£o":
                    return "Função Transmissão";
                case "NÃºmero da Barra no Anafas":
                    return "Número da Barra no Anafas";
                case "NÃ­vel de curto-circuito na barra monofÃ¡sico (kA)":
                    return "Nível de curto-circuito na barra monofásico (kA)";
                case "NÃ­vel de curto-circuito na barra trifÃ¡sico (kA)":
                    return "Nível de curto-circuito na barra trifásico (kA)";
                case "X/R na barra da subestaÃ§Ã£o para curto-circuito monofÃ¡sico":
                    return "X/R na barra da subestação para curto-circuito monofásico";
                case "X/R na barra da subestaÃ§Ã£o para curto-circuito trifÃ¡sico":
                    return "X/R na barra da subestação para curto-circuito trifásico";
                case "Capacidade interrupÃ§Ã£o simÃ©trica nominal (kA)":
                    return "Capacidade interrupção simétrica nominal (kA)";
                case "Corrente de curto-circuito monofÃ¡sica atravÃ©s Do disjuntor (calculada) (kA)":
                    return "Corrente de curto-circuito monofásica através Do disjuntor (calculada) (kA)";
                case "Corrente de curto-circuito trifÃ¡sica atravÃ©s Do disjuntor (calculada) (kA)":
                    return "Corrente de curto-circuito trifásica através Do disjuntor (calculada) (kA)";
                case "Quantidade de secionadoras superadas no mesmo vÃ£o (bay) sem chave de terra":
                    return "Quantidade de secionadoras superadas no mesmo vão (bay) sem chave de terra";
                case "Quantidade de secionadoras superadas no mesmo vÃ£o (bay) com chave de terra":
                    return "Quantidade de secionadoras superadas no mesmo vão (bay) com chave de terra";
                case "Corrente de curto-circuito monofÃ¡sica atravÃ©s da secionadora (calculada) (kA)":
                    return "Corrente de curto-circuito monofásica através da secionadora (calculada) (kA)";
                case "Corrente de curto-circuito trifÃ¡sica atravÃ©s da secionadora (calculada) (kA)":
                    return "Corrente de curto-circuito trifásica através da secionadora (calculada) (kA)";
                case "SaturaÃ§Ã£o":
                    return "Saturação";
                case "Corrente de curto-circuito monofÃ¡sica atravÃ©s da Bobina de bloqueio (calculada) (kA)":
                    return "Corrente de curto-circuito monofásica através da Bobina de bloqueio (calculada) (kA)";
                case "Corrente de curto-circuito trifÃ¡sica atravÃ©s da Bobina de bloqueio (calculada) (kA)":
                    return "Corrente de curto-circuito trifásica através da Bobina de bloqueio (calculada) (kA)";
                case "Corrente de curto-circuito monofÃ¡sica atravÃ©s Do TC (calculada) (kA)":
                    return "Corrente de curto-circuito monofásica através Do TC (calculada) (kA)";
                case "Corrente de curto-circuito trifÃ¡sica atravÃ©s Do TC (calculada) (kA)":
                    return "Corrente de curto-circuito trifásica através Do TC (calculada) (kA)";
                case "Fator tÃ©rmico":
                    return "Fator térmico";
                case "Origem da indicaÃ§Ã£o (empresa solicitante/documento de referÃªncia)":
                    return "Origem da indicação (empresa solicitante/documento de referência)";
                case "ObservaÃ§Ã£o":
                    return "Observação";
                case "ObservaÃ§Ã£o_1":
                    return "Observação ONS";
                case "Observação_1":
                    return "Observação ONS";
                case "Projeto que originou a indicaÃ§Ã£o":
                    return "Projeto que originou a indicação";
                case "SuperaÃ§Ã£o por corrente nominal":
                    return "Superação por corrente nominal";
                case "SuperaÃ§Ã£o por corrente de curto-circuito simÃ©trica":
                    return "Superação por corrente de curto-circuito simétrica";
                case "SuperaÃ§Ã£o pela crista da corrente de curto-circuito":
                    return "Superação pela crista da corrente de curto-circuito";
                case "SuperaÃ§Ã£o por X/R":
                    return "Superação por X/R";
                case "SituaÃ§Ã£o da AnÃ¡lise":
                    return "Situação da Análise";
                case "ClassificaÃ§Ã£o":
                    return "Classificação";
                case "ClassificaÃ§Ã£o_1":
                    return "Classificação ONS";
                case "Classificação_1":
                    return "Classificação ONS";
                case "IndicaÃ§Ãµes":
                    return "Indicações";
                case "Oficio/ResoluÃ§Ã£o/AutorizaÃ§Ã£o ANEEL relativa a substituiÃ§Ã£o Do equipamento":
                    return "Oficio/Resolução/Autorização ANEEL relativa a substituição Do equipamento";
                case "Data da AutorizaÃ§Ã£o":
                    return "Data da Autorização";
                case "PrevisÃ£o da ImplantaÃ§Ã£o":
                    return "Previsão da Implantação";
                case "Data de Entrada em OperaÃ§Ã£o":
                    return "Data de Entrada em Operação";
                case "Status da RevitalizaÃ§Ã£o":
                    return "Status da Revitalização";
                case "AquisiÃ§Ã£o de Materiais - InÃ­cio":
                    return "Aquisição de Materiais - Início";
                case "AquisiÃ§Ã£o de Materiais - Fim":
                    return "Aquisição de Materiais - Fim";
                case "Obras Civis - InÃ­cio":
                    return "Obras Civis - Início";
                case "Montagem EletromecÃ¢nica - InÃ­cio":
                    return "Montagem Eletromecânica - Início";
                case "Montagem EletromecÃ¢nica - Fim":
                    return "Montagem Eletromecânica - Fim";
                case "Comissionamento - InÃ­cio":
                    return "Comissionamento - Início";
                case "OperaÃ§Ã£o Comercial":
                    return "Operação Comercial";
                case "ObservaÃ§Ãµes Gerais":
                    return "Observações Gerais";
                default:
                    return termo;
            }
            #endregion
        }

        public static DataTable ReadExcel(Stream stream, SPWeb web)
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
                                string termo = CaracterWeb(cell.Value.ToString());
                                #region Antes de Testar termos
                                /*if (cell.Value.ToString() == "Classificação" && cell.Address.ColumnNumber > 35)
                                    dt.Columns.Add("Classificação ONS");
                                else if (cell.Value.ToString() == "Classificação" && cell.Address.ColumnNumber < 35)
                                    dt.Columns.Add("Classificação Furnas");
                                else if (cell.Value.ToString() == "Nº da recomendação no SGR")
                                    dt.Columns.Add("N° da recomendação no SGR");
                                else if (cell.Value.ToString() == "Nº Série")
                                    dt.Columns.Add("N° Série");
                                else if (cell.Value.ToString() == "Observação" && cell.Address.ColumnNumber > 50)
                                    dt.Columns.Add("Observação ONS");
                                else if (cell.Value.ToString() == "Observação" && cell.Address.ColumnNumber < 50)
                                    dt.Columns.Add("Observação Furnas");
                                else
                                    dt.Columns.Add(cell.Value.ToString());*/
                                #endregion
                                if (termo == "Classificação" && cell.Address.ColumnNumber > 35)
                                    dt.Columns.Add("Classificação ONS");
                                else if (termo == "Classificação" && cell.Address.ColumnNumber < 35)
                                    dt.Columns.Add("Classificação Furnas");
                                else if (termo == "Nº da recomendação no SGR")
                                    dt.Columns.Add("N° da recomendação no SGR");
                                else if (termo == "Nº Série")
                                    dt.Columns.Add("N° Série");
                                else if (termo == "Observação" && cell.Address.ColumnNumber > 50)
                                    dt.Columns.Add("Observação ONS");
                                else if (termo == "Observação" && cell.Address.ColumnNumber < 50)
                                    dt.Columns.Add("Observação Furnas");
                                else
                                    dt.Columns.Add(termo);
                            }
                            firstRow = false;
                        }
                        else
                        {
                            IXLCells cells = row.Cells();
                            
                            //dt.Rows.Add();
                            int i = 0;

                            try
                            {
                                foreach (IXLCell cell in row.Cells(row.FirstCellUsed().Address.ColumnNumber, row.LastCellUsed().Address.ColumnNumber))
                                //foreach (IXLCell cell in cells)
                                {
                                    if(i==0)
                                        dt.Rows.Add();
                                    dt.Rows[dt.Rows.Count - 1][i] = Convert.ToString(cell.Value);

                                    i++;
                                }
                            }
                            catch { }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log(web, e.Message.ToString(), "Leitura do Excel");
                throw new Exception(e.Message);
            }
            return dt;
        }

        public static string Colunas(string coluna)
        {
            string[] Campos_datas = { "Data de Necessidade Sistêmica", "Data da Autorização/Emissão PMI", "Previsão da Implantação", 
                     "Aquisição de Materiais - Início", "Aquisição de Materiais - Fim", "Obras Civis - Início", "Obras Civis - Fim", "Montagem Eletromecânica - Início", "Montagem Eletromecânica - Fim", "Comissionamento - Início", "Comissionamento - Fim", "Operação Comercial", 
                     "Data de Necessidade", "Data da Autorização", "Data de Entrada em Operação"};
            string[] Campos_numeros = { "Aumento de vida útil esperado, a partir da implantação Do reforço(anos)", "Ano Final de Vida Útil", "Ano Início em Operação"};
            string[] Campos_moedas ={"Valor a ser investido(R$)", "Custo Estimado (R$)" };
            if (coluna == "Ciclo")
                return coluna;
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

        public static void DeleteDados(int ciclo, string Plano, SPList list)
        {
            SPQuery query = new SPQuery();
            query.Query = string.Format("<Where><And><Eq><FieldRef Name='ContentType'/><Value Type='Computed'>{0}</Value></Eq><Eq><FieldRef Name='Ciclo' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq></And></Where>", Plano, ciclo);
            
            SPListItemCollection collItem = list.GetItems(query);
            
            if (collItem != null)
            {
                while (collItem.Count > 0)
                {
                    collItem[0].Delete();
                }
            }
        }

        public static SPListItem UpdateDados(int ciclo, string Plano, SPList list, string numeracao)
        {
            //SPQuery query = new SPQuery();
            //query.Query = string.Format("<Where><And><Eq><FieldRef Name='ContentType'/><Value Type='Computed'>{0}</Value></Eq><Eq><FieldRef Name='Ciclo' LookupId='TRUE' /><Value Type='Lookup'>{1}</Value></Eq></And><And><Eq><FieldRef Name='Title' /><Value Type='Text'>{2}</Value></Eq></And></Where>", Plano, ciclo, numeracao);

           SPListItem items = list.Items.OfType<SPListItem>().Where(p=> Convert.ToString(p.ContentType.Name) == Plano && new SPFieldLookupValue(Convert.ToString(p["Ciclo"])).LookupId == ciclo && p.Title == numeracao).FirstOrDefault();

           return items;
          
        }

        public static List<SPListItem> RetornaItensCiclo(int ciclo, string Plano, SPList list)
        {
            List<SPListItem> items = list.Items.OfType<SPListItem>().Where(p => Convert.ToString(p.ContentType.Name) == Plano && new SPFieldLookupValue(Convert.ToString(p["Ciclo"])).LookupId == ciclo).ToList();

            return items;

        }

        public static string Numeracao(DataRow row)
        {
            return row["Numeração"].ToString();
        }

        public static void ValidaPlanilha(DataTable data, string plano, SPWeb web)
        {
            Regex IsData = new Regex(@"^\d{2}\/\d{2}\/\d{4} \d{2}:\d{2}:\d{2}$");
            Regex IsDecimal = new Regex(@"^\d*[0-9](\,\d*[0-9])?$");

            try
            {
                int indexRow = 1; bool sup = false;
                foreach (DataRow row in data.Rows)
                {
                    int indexColl = 1;
                    foreach (DataColumn coll in row.Table.Columns)
                    {
                        if (coll.ColumnName == "Tensão Nominal (kV)")
                            sup = true;

                        if (row[coll.ColumnName].ToString() != string.Empty)
                        {
                            string Collumn = Colunas(coll.ColumnName);
                            if ("Datas" == Collumn)
                            {
                                if (!IsData.IsMatch(row[coll.ColumnName].ToString()))
                                    throw new Exception("Informação de Data Inconsistente na Coluna " + ColunaPlanilha(indexColl.ToString()) + " - Linha " + indexRow.ToString() + ".");
                            }
                            else if ("Numeros" == Collumn)
                            {
                                if (!IsDecimal.IsMatch(row[coll.ColumnName].ToString()))
                                    throw new Exception("Informação de Inconsistente na Coluna " + ColunaPlanilha(indexColl.ToString()) + " - Linha " + indexRow.ToString() + ". Nesta coluna só é permitido números.");
                            }
                            else if ("Moeda" == Collumn)
                            {
                                try{
                                    Decimal.Parse(row[coll.ColumnName].ToString().Replace(',','.'));
                                }
                                catch{
                                    throw new Exception("Informação de Inconsistente na Coluna " + ColunaPlanilha(indexColl.ToString()) + " - Linha " + indexRow.ToString() + ". Nesta coluna só é permitido números.");
                                }
                            }
                        }
                        indexColl++;
                    }
                    indexRow++;
                }

                if (sup == false && plano == "Plano Por Superação")
                    throw new Exception("Importação do " + plano + " incompatível com o arquivo selecionado.");

                if(sup == true && plano != "Plano Por Superação")
                    throw new Exception("Importação do " + plano + " incompatível com o arquivo selecionado.");
            }
            catch(Exception e){
                Log(web, e.Message.ToString(), plano);
                throw e;}
        }

        public static void ImportDados(DataTable data, SPWeb web, int ciclo, string Plano, string cicloName, int IdDoc)
        {
            //int eLinha = 0;
            try
            {
                SPList GerenciaPlanos = web.Lists["Planos"];
                SPList Ciclos = web.Lists.TryGetList("Ciclos");

                List<SPListItem> items = RetornaItensCiclo(ciclo,Plano, GerenciaPlanos);
                //DeleteDados(ciclo, Plano, GerenciaPlanos);
                
                foreach (DataRow row in data.Rows)
                {
                    //SPListItem item = UpdateDados(ciclo, Plano, GerenciaPlanos, Numeracao(row));
                    //SPListItem item = GerenciaPlanos.AddItem();

                    SPListItem item = items.Find(p => p.Title == Numeracao(row));
                    
                    if (item == null)
                        item = GerenciaPlanos.AddItem();

                    item[SPBuiltInFieldId.ContentTypeId] = item.ListItems.List.ContentTypes[Plano].Id;

                    foreach (DataColumn coll in row.Table.Columns)
                    {
                        if (row[coll.ColumnName].ToString() != string.Empty)
                        {
                            string Collumn = Colunas(coll.ColumnName);
                            if ("Ciclo" == Collumn)
                            {
                                item[coll.ColumnName] = new SPFieldLookupValue(ciclo, cicloName);
                            }
                            else if ("Datas" == Collumn)
                            {
                                try { item[coll.ColumnName] = TratarData(row[coll.ColumnName]); }
                                catch(Exception eDT) { Log(web, eDT.Message.ToString(), Plano); }
                            }
                            else if ("Numeros" == Collumn)
                            {
                                try { item[coll.ColumnName] = Convert.ToInt32(row[coll.ColumnName].ToString()); }
                                catch (Exception eNumeros) { Log(web, eNumeros.Message.ToString(), Plano); }
                            }
                            else if ("Moeda" == Collumn)
                            {
                                try { 
                                    string valorInvest = row[coll.ColumnName].ToString();
                                    if (valorInvest.Substring(valorInvest.Length - 3, 1) == ",")
                                        item[coll.ColumnName] = valorInvest.Replace(".",",").Remove(valorInvest.Length - 3).Insert(valorInvest.Length - 3, "." + valorInvest.Substring(valorInvest.Length-2,2));
                                    else
                                        item[coll.ColumnName] = valorInvest;
                                }
                                catch (Exception eMoeda) { Log(web, eMoeda.Message.ToString(), Plano); }
                            }
                            else
                            {
                                try { item[coll.ColumnName] = row[coll.ColumnName].ToString(); }
                                catch (Exception eGeral) { Log(web, eGeral.Message.ToString(), Plano); }
                            }
                        } //eLinha++;
                    }

                    item["Nome do Ciclo"] = cicloName + "-" + DateTime.Now.Year + "/" + (DateTime.Now.Month < 10 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString()) + "/" + (DateTime.Now.Day < 10 ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString());
                    item["ID Documento"] = IdDoc;
                    item.Update();

                }
            }
            catch (Exception e)
            {
                Log(web, e.Message.ToString() + "Ciclo:" + cicloName, Plano);
                throw new Exception("Ocorreu um erro durante a importação. Tente mais tarde.");
            }
        }

        public static DataTable TableViewHtml(DataTable dt)
        {
            DataTable dtTemp;
            dtTemp = dt.Copy();
            int countRows = dt.Rows.Count;
            int countColumns = dt.Columns.Count;
            string celula = string.Empty;

            Regex IsData = new Regex(@"^\d{2}\/\d{2}\/\d{4} \d{2}:\d{2}:\d{2}$");
            Regex IsDecimal = new Regex(@"^\d*[0-9](\,\d*[0-9])?$");

            for (int i = 0; i < countRows; i++)
            {
                for (int j = 0; j < countColumns; j++)
                {
                    celula = Convert.ToString(dt.Rows[i][j]);
                    if (IsData.IsMatch(celula))
                        dtTemp.Rows[i][j] = celula.Split(' ')[0];
                    else if (IsDecimal.IsMatch(celula))
                        dtTemp.Rows[i][j] = celula + "%";
                    else
                        dtTemp.Rows[i][j] = celula;
                }
            }

            return dtTemp;
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

        public static void Log(SPWeb web, string erro, string plano)
        {
            try
            {
                SPList GerenciaPlanos = web.Lists["Logs"];
                SPListItem item = GerenciaPlanos.AddItem();

                item[SPBuiltInFieldId.Title] = plano;
                item["Descrição"] = erro;
                item.Update();
            }
            catch { }
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
                default:
                    return colunaInt;
            }
            #endregion
        }
    }
}
