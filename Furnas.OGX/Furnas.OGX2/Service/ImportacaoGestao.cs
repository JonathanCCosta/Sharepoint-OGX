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
    public class ImportacaoGestao
    {
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
                            int i = 0;
                            try
                            {
                                foreach (IXLCell cell in row.Cells(row.FirstCellUsed().Address.ColumnNumber, row.LastCellUsed().Address.ColumnNumber))
                                {
                                    if (i == 0)
                                        dt.Rows.Add();
                                    //}


                                    //int p = row.CellCount();
                                    //TODO: Na hora de obter o valor do trimestre não estava obtendo quando a celula estava nula.
                                    //Pra resolver tive que colocar o tipo, pelo menos nas celulas dos trimestres.
                                    //foreach (IXLCell cell in cells)
                                    //{
                                    dt.Rows[dt.Rows.Count - 1][i] = Convert.ToString(cell.Value);
                                    //dt.Rows.Add(cell.Value);
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

                
                return result.Tables[0];
            }
            catch (Exception e)
            {
                Log(web, e.Message.ToString(), "Leitura do Excel");
                throw new Exception(e.Message);
            }
        }

        public static void ValidaPlanilhaGestao(DataTable data, string plano, SPWeb web)
        {
            Regex IsData = new Regex(@"^\d{2}\/\d{2}\/\d{4} \d{2}:\d{2}:\d{2}$");
            Regex IsDecimal = new Regex(@"^\d*[0-9](\,\d*[0-9])?$");

            try
            {
                foreach (DataRow row in data.Rows)
                {
                    foreach (DataColumn coll in row.Table.Columns)
                    {
                        string NomeColuna = coll.ColumnName;
                        if (NomeColuna.Contains("No. SGPMR") && row[coll.ColumnName].ToString() == string.Empty)
                        {
                            throw new Exception("A coluna No. SGPRM deve ser prenchida!");
                        }
                        if (row[coll.ColumnName].ToString() != string.Empty)
                        {
                            if (NomeColuna == "Previsão de implantação FURNAS")
                            {
                                if (!IsData.IsMatch(row[coll.ColumnName].ToString()))
                                    throw new Exception("A coluna Previsão de implantação FURNAS deve conter somente datas.");
                            }
                            if (NomeColuna == "Data da última atualização de informações")
                            {
                                if (!IsData.IsMatch(row[coll.ColumnName].ToString()))
                                    throw new Exception("A coluna Data da última atualização de informações deve conter somente datas.");
                            }
                        }
                    }
                }	

                foreach (DataColumn collH in data.Columns)
                {
                    string NomeColuna = collH.ColumnName;
                    if (NomeColuna.Contains("No. SGPMR") || NomeColuna == "Forma de Solicitação" || NomeColuna == "Órgão solicitante" || NomeColuna == "Responsável no órgão solicitante" ||
                        NomeColuna == "Documento interno de aprovação" || NomeColuna == "Órgão gestor da obra" || NomeColuna == "Responsável no órgão gestor da obra" ||
                        NomeColuna == "Considerações do Responsável pelo gerenciamento" || NomeColuna == "Número do PEP" || NomeColuna == "Local de instalação SAP-PM" ||
                        NomeColuna == "N° do Equipamento SAP-PM" || NomeColuna == "Controle de atos autorizativos" || NomeColuna == "Ato Autorizativo Atual, PAR, POT ou PET" ||
                        NomeColuna == "Previsão de implantação FURNAS" || NomeColuna == "Status da revitalização FURNAS" || NomeColuna == "Documento de pleito de receita" ||
                        NomeColuna == "Status do pleito de Receita" || NomeColuna == "Considerações da Gestão de Ativos" || NomeColuna == "Data da última atualização de informações"){}
                    else
                    {
                        throw new Exception("A planilha não esta no Modelo esperado.");
                    }
                }
            }
            catch (Exception e)
            {
                Log(web, e.Message.ToString(), plano);
                throw e;
            }
        }

        public static void ImportDadosGestao(DataTable data, SPWeb web, int ciclo, string Plano, string cicloName)
        {
            try
            {
                SPList Gestao = web.Lists["Gestão Interna"];
                SPList Planos = web.Lists.TryGetList("Planos");
                SPListItemCollection ListIds = RetornaPlano(cicloName, Plano, Planos);

                foreach (DataRow row in data.Rows)
                {
                    SPListItem item = Gestao.AddItem();
                    bool ExisteNumeroCicloPlano = false;
                    foreach (DataColumn coll in row.Table.Columns)
                    {
                        if (row[coll.ColumnName].ToString() != string.Empty)
                        {
                            string Collumn = coll.ColumnName;
                            if (Collumn.Contains("No. SGPMR"))
                            {
                                //int IdPlan = RetornaPlano(cicloName, Plano, Planos, row[coll.ColumnName].ToString());
                                foreach (SPListItem IdPlan in ListIds)
                                {
                                    if (IdPlan[SPBuiltInFieldId.Title].ToString() == row[coll.ColumnName].ToString())
                                    {
                                        item["No. SGPMR"] = row[coll.ColumnName].ToString();//new SPFieldLookupValue(IdPlan.ID, row[coll.ColumnName].ToString());
                                        ExisteNumeroCicloPlano = true;
                                    }
                                }
                                //if (IdPlan > 0)
                                //    item["No. SGPMR"] = new SPFieldLookupValue(IdPlan, row[coll.ColumnName].ToString());
                                //else
                                //{
                                //    ExisteNumeroCicloPlano = false;
                                //    break;
                                //}
                                if (!ExisteNumeroCicloPlano)
                                    break;
                            }
                            else if (Collumn == "Previsão de implantação FURNAS" || Collumn == "Data da última atualização de informações")
                            {
                                try { item[coll.ColumnName] = TratarData(row[coll.ColumnName]); }
                                catch (Exception eDT) { Log(web, eDT.Message.ToString(), Plano); }
                            }
                            else
                            {
                                try { item[coll.ColumnName] = row[coll.ColumnName].ToString(); }
                                catch (Exception eGeral) { Log(web, eGeral.Message.ToString(), Plano); }
                            }
                        }
                    }
                    if (ExisteNumeroCicloPlano)
                    {
                        item["Aprovação da solicitação"] = "Aprovado";
                        EventFiring eventFiring = new EventFiring();
                        eventFiring.DisableHandleEventFiring();
                        item.Update();
                        eventFiring.EnableHandleEventFiring();
                    }
                }
            }
            catch (Exception e)
            {
                Log(web, e.Message.ToString() + "Ciclo:" + cicloName, Plano);
                throw new Exception("Ocorreu um erro durante a importação. Tente mais tarde.");
            }
        }

        public static SPListItemCollection RetornaPlano(string ciclo, string Plano, SPList list)//, string numeracao)
        {
            SPQuery query = new SPQuery();
            query.Query = "<Where><And>"+//<And>" +
                                "<Eq><FieldRef Name='Ciclo' /><Value Type='Lookup'>"+ ciclo +"</Value></Eq>" +
                                "<Eq><FieldRef Name='ContentType' /><Value Type='Computed'>"+ Plano +"</Value></Eq>"+
                                //"<Eq><FieldRef Name='Title' /><Value Type='Text'>"+ numeracao +"</Value>" +
                           //"</Eq></And>" +
                               // "<Eq><FieldRef Name='ContentType' /><Value Type='Computed'>"+ Plano +"</Value></Eq>"+
                            "</And></Where>";
            query.ViewFields = "<FieldRef Name='ID' /><FieldRef Name='Title' />";
            return list.GetItems(query);

            //int item = list.Items.OfType<SPListItem>().Where(p => Convert.ToString(p.ContentType.Name) == Plano && new SPFieldLookupValue(Convert.ToString(p["Ciclo"])).LookupValue == ciclo && p.Title == numeracao).Select(p=> p.ID).FirstOrDefault();

            //SPListItemCollection items = list.GetItems(query);

            //if (items.Count > 0)
            //    return items[0].ID;
            //else
            //    return 0;
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
    }
}
