using ClosedXML.Excel;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Furnas.OGX2.Service
{
    public class ExportacaoSolicitacao
    {
        public static SPListItemCollection RetornaSolicitacoes(SPWeb web, string ciclo, string Plano)
        {
            SPList list  = web.Lists["Solicitação de Mudanças"];
             SPQuery query = new SPQuery();
            query.Query = "<Where><And>"+
                                "<Eq><FieldRef Name='CicloIrma' /><Value Type='Text'>" + ciclo + "</Value></Eq>" +
                                "<Eq><FieldRef Name='ContentType' /><Value Type='Computed'>" + DeParaSolicitacao(Plano) + "</Value></Eq>" +
                            "</And></Where>";

            //SPListItemCollectionPosition collPoss;
            //DataTable table = list.GetDataTable(query, SPListGetDataTableOptions.None, out collPoss);

            return list.GetItems(query);
        }

        public static DataTable SolicitacoesDataTable(SPWeb web, string ciclo, string Plano)
        {
            SPList list = web.Lists["Solicitação de Mudanças"];
            SPQuery query = new SPQuery();
            query.Query = "<Where><And>" +
                                "<Eq><FieldRef Name='CicloIrma' /><Value Type='Text'>" + ciclo + "</Value></Eq>" +
                                "<Eq><FieldRef Name='ContentType' /><Value Type='Computed'>" + DeParaSolicitacao(Plano) + "</Value></Eq>" +
                            "</And></Where>";
            DataTable dt = list.GetItems(query).GetDataTable();

            SPFieldCollection ct = list.ContentTypes["Plano de Melhoria - Mudanças"].Fields;
            Dictionary<string, string> cd = new Dictionary<string, string>();

            foreach (SPField f in ct)
            {
                cd.Add(f.Title, f.InternalName);
            }
            cd.Add("Modificado", "Modified"); cd.Add("Modificado por", "Editor");
            cd.Add("Criado", "Created"); cd.Add("Criado por", "Author");

            SPFieldCollection cGestao = web.Lists["Gestão Interna"].Fields;
            //foreach

            foreach (DataColumn column in dt.Columns.Cast<DataColumn>().ToList())
            {
                if (cd.ContainsValue(column.ColumnName) && column.ColumnName != "Title" && column.ColumnName != "AlteracoesProcessadas" && column.ColumnName != "NumGestao")
                {
                    try
                    {
                        column.ColumnName = list.Fields.GetFieldByInternalName(column.ColumnName).Title;
                    }
                    catch { }
                }
                else
                    dt.Columns.Remove(column.ColumnName);
            }

            return dt;
        }

        public static DataRow RetornaGI(string numeracao, string Numgestao, DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                //if (Convert.ToString(row["No. SGPMR"]) == numeracao)
                //    return row;
                //else if (Convert.ToString(row["ID"]) == Numgestao)
                if (Convert.ToString(row["ID"]) == Numgestao)
                    return row;
            }

            return null;
        }

        public static DataTable ReturnDataTable(SPWeb web, string ciclo, string Plano)
        {
            SPList list = web.Lists["Solicitação de Mudanças"];
            SPQuery query = new SPQuery();
            //query.Query = "<Where><And><And>" +
            query.Query = "<Where><And>" +
                                "<Eq><FieldRef Name='CicloIrma' /><Value Type='Text'>" + ciclo + "</Value></Eq>" +
                                "<Eq><FieldRef Name='ContentType' /><Value Type='Computed'>" + DeParaSolicitacao(Plano) + "</Value></Eq></And></Where>";
                                //"<Eq><FieldRef Name='StatusSolicitacao' /><Value Type='Choice'>Solicitado</Value></Eq>" +
                            //"</And></Where>";

            //Inseri os Campos escolhidos e seus nomes de Interface DT - Mudanças
            SPFieldCollection ct = list.ContentTypes[DeParaSolicitacao(Plano)].Fields;
            Dictionary<string, string> cd = new Dictionary<string, string>();
            foreach (SPField f in ct)
            {
                cd.Add(f.Title, f.InternalName);
            }

            DataTable dt = list.GetItems(query).GetDataTable();
            if (dt != null)
            {
                //DT
                foreach (DataColumn column in dt.Columns.Cast<DataColumn>().ToList())
                {
                    if (cd.ContainsValue(column.ColumnName) && column.ColumnName != "Title") //&& column.ColumnName != "AlteracoesProcessadas")
                        column.ColumnName = list.Fields.GetFieldByInternalName(column.ColumnName).Title;
                    else
                        dt.Columns.Remove(column.ColumnName);
                }

                //Inseri os campos escolhidos e seus nome de Interface DT - Gestão Interna
                SPList listG = web.Lists["Gestão Interna"];
                /*SPQuery queryG = new SPQuery();
                queryG.Query = "<Where>" +
                                    "<Eq><FieldRef Name='AprovacaoSolicitacao' /><Value Type='Choice'>Pendente</Value></Eq>" +
                                "</Where>";
                DataTable dt2 = listG.GetItems(queryG).GetDataTable();*/
                DataTable dt2 = listG.GetItems().GetDataTable();

                SPFieldCollection fieldG = listG.ContentTypes[0].Fields;
                Dictionary<string, string> cg = new Dictionary<string, string>();
                foreach (SPField fg in fieldG)
                {
                    try
                    {
                        cg.Add(fg.Title, fg.InternalName);
                    }
                    catch { }
                }
                cg.Add("ID", "ID");

                //DT2
                if (dt2 != null)
                {
                    foreach (DataColumn columng in dt2.Columns.Cast<DataColumn>().ToList())
                    {
                        if (cg.ContainsValue(columng.ColumnName) && columng.ColumnName != "Title" && columng.ColumnName != "Historico" && columng.ColumnName != "AprovacaoSolicitacao")
                        {
                            //if (columng.ColumnName == "NumSGPMR")
                            //    columng.ColumnName = "Numeração";
                            //else
                            columng.ColumnName = listG.Fields.GetFieldByInternalName(columng.ColumnName).Title;

                            try
                            {
                                dt.Columns.Add(columng.ColumnName);
                            }
                            catch { }
                        }
                        else
                            dt2.Columns.Remove(columng.ColumnName);
                    }

                    foreach (DataRow row in dt.Rows)
                    {
                        if (Convert.ToString(row["Alteração Solicitada"]) != "Exclusão")
                        {
                            DataRow rowGestao = RetornaGI(Convert.ToString(row["Numeração"]), Convert.ToString(row["Número de Gestão"]), dt2);

                            if (rowGestao != null)
                            {
                                foreach (DataColumn col in row.Table.Columns)
                                {
                                    if (cg.ContainsKey(col.ColumnName))
                                    {
                                        row[col.ColumnName] = Convert.ToString(rowGestao[col.ColumnName]);
                                    }
                                }
                            }
                        }
                    }

                    dt.Columns.Remove("Número de Gestão");
                    dt.Columns.Remove("Tipo de Conteúdo");
                    dt.Columns.Remove("ID");
                }
                else
                {
                    foreach (SPField f in fieldG)
                    {
                        if (f.Title != "Title" && f.Title != "Historico" && f.Title != "AprovacaoSolicitacao" && f.Title == "ID")
                            dt.Columns.Add(f.Title);
                    }
                    dt.Columns.Remove("Número de Gestão");
                    dt.Columns.Remove("Tipo de Conteúdo");
                }
                return dt;
            }
            else
                return dt;
        }

        public static string DeParaSolicitacao(string plano)
        {
            if(plano.Contains("Melhoria"))
                return "Plano de Melhoria - Mudanças";
            else if(plano.Contains("Reforço"))
                return "Plano de Reforço - Mudanças";
            else
                return "Plano por Superação - Mudanças";
        }

        public static Table GetTable(SPView wpView, SPListItemCollection items)
        {
            Table table = new Table();
            
            table.ID = "_tblListView";
            table.BorderStyle = BorderStyle.Solid;
            table.BorderWidth = Unit.Pixel(1);
            if (items != null && items.Count > 0)
            {
                DataTable dataTable = items.GetDataTable();
                DataView defaultView = dataTable.DefaultView;
                if (defaultView != null && defaultView.Count > 0)
                {
                    table.Rows.Add(new TableRow());
                    table.Rows[0].Font.Bold = true;
                    for (int i = 0; i < wpView.ViewFields.Count; i++)
                    {
                        table.Rows[0].Cells.Add(new TableCell());
                        table.Rows[0].Cells[i].Text = wpView.ViewFields[i];
                    }
                    for (int i = 0; i < defaultView.Count; i++)
                    {
                        table.Rows.Add(new TableRow());
                        for (int j = 0; j < wpView.ViewFields.Count; j++)
                        {
                            table.Rows[i + 1].Cells.Add(new TableCell());
                            if (dataTable.Columns.Contains(wpView.ViewFields[j].ToString()))
                            {
                                table.Rows[i + 1].Cells[j].Text = defaultView[i][wpView.ViewFields[j].ToString()].ToString();
                            }
                        }
                    }
                }
            }
            return table;
        }

        public static string ListToExcelConvertor(Table tblData, string filename)
        {
            string text = "C:\\ExportacoesGGA\\" + filename;
            StreamWriter streamWriter = null;
            StringWriter stringWriter = null;
            HtmlTextWriter htmlTextWriter = null;
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo("C:\\ExportacoesGGA\\");
                directoryInfo.Create();
                streamWriter = new StreamWriter(text, false, Encoding.GetEncoding("UTF-8"));
                stringWriter = new StringWriter();
                htmlTextWriter = new HtmlTextWriter(stringWriter);
                tblData.RenderControl(htmlTextWriter);
                streamWriter.Write(stringWriter.ToString());
            }
            catch (Exception innerException)
            {
                throw new Exception("Não foi possível criar o excel de backup.", innerException);
            }
            finally
            {
                htmlTextWriter.Close();
                streamWriter.Close();
                stringWriter.Close();
            }
            return text;
        }

        public static bool UploadFileDataBackup(SPWeb web, string fullfilename)
        {
            bool result;
            try
            {
                if (!File.Exists(fullfilename))
                {
                    throw new FileNotFoundException("File not found.", fullfilename);
                }
                SPDocumentLibrary sPFolder = web.Lists["Documentos"] as SPDocumentLibrary;
                string fileName = Path.GetFileName(fullfilename);
                FileStream file = File.OpenRead(fullfilename);

                SPFile file_in = sPFolder.RootFolder.Files.Add(fileName, file, true);


                if (file_in.CheckOutStatus != SPFile.SPCheckOutStatus.None)
                {
                    file_in.CheckIn(string.Empty);
                }
                file.Close(); file.Dispose();
                result = true;
            }
            catch (Exception innerException)
            {
                throw new Exception("Não foi possível realizar o upload do backup para a biblioteca de documentos.", innerException);
            }
            return result;
        }

        public static void EmailMudancaStatus(SPListItem item, string status)
        {
            StringBuilder message = new StringBuilder();

            message.Append("O status da solicitação <a href='" + item.ParentList.ParentWeb.Site.MakeFullUrl(item.ParentList.DefaultDisplayFormUrl) + "?ID=" + item.ID + "'>" + DuplicaItens.ValidaTextField(item["NumeracaoIrma"]) + "</a> foi alterado para Status " + status + ".");
            message.Append("</br>");
            //O status da solicitação <Nome do Plano> foi alterado para Status <Status>.

            EmailSGPMR.Email mail = new EmailSGPMR.Email();
            SPFieldUserValue Author = new SPFieldUserValue(item.Web, item[SPBuiltInFieldId.Author].ToString());
            mail.CC = Author.User.Email;

            mail.Assunto = "Atualização de Status do pedido de Mudança no SGPMR";
            mail.Corpo = message.ToString();

            mail.EnviaEmail(item.Web);
        }
    }
}
