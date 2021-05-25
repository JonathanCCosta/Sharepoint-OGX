using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using Furnas.OGX2.Service;
using System.IO;
using System.Net;
using ClosedXML.Excel;
using System.Data;
using System.Web;
using System.Web.UI;

namespace Furnas.OGX2.Webparts.WP_ExportaPlanos
{
    [ToolboxItemAttribute(false)]
    public partial class WP_ExportaPlanos : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public WP_ExportaPlanos()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadDropDown(ddlCiclo, SPContext.Current.Web, "Ciclos");
            }
        }

        internal static void LoadDropDown(System.Web.UI.WebControls.DropDownList ddlItems, SPWeb sPWeb, string listname, string lookupcolumn = null)
        {
            SPList list = sPWeb.Lists[listname];

            foreach (SPListItem item in list.Items)
            {
                ListItem lt = new ListItem();
                if (Convert.ToString(item["Fluxo de Controle"]) == "Fechado")
                {
                    lt.Value = item.ID.ToString();
                    lt.Text = item.Title + " - Fechado";
                    ddlItems.Items.Add(lt);
                }
                else
                {
                    lt.Value = item.ID.ToString();
                    lt.Text = item.Title + " - Aberto";
                    ddlItems.Items.Add(lt);
                }
            }

            List<ListItem> itemsDDL = ddlItems.Items.Cast<ListItem>().OrderBy(lt => lt.Text).ToList();
            ddlItems.Items.Clear();
            ddlItems.Items.AddRange(itemsDDL.ToArray());
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            SPWeb web = SPContext.Current.Web;
            string ciclo = string.Empty;
            
            if (ddlCiclo.Items[ddlCiclo.SelectedIndex].Text.Contains("Fechado"))
                ciclo = ddlCiclo.Items[ddlCiclo.SelectedIndex].Text.Replace(" - Fechado", "");
            else
                ciclo = ddlCiclo.Items[ddlCiclo.SelectedIndex].Text.Replace(" - Aberto", "");

            string plano = ddlTipos.Items[ddlTipos.SelectedIndex].Text;
            try
            {
                DataTable dt = ExportacaoSolicitacao.ReturnDataTable(web, ciclo, plano);
                if (dt != null)
                {
                    string name = BuildFileName(DateTime.Now, ciclo + "-" + plano); string path = "C:\\ExportacoesGGA\\";
                    DirectoryInfo directoryInfo = new DirectoryInfo(path); directoryInfo.Create();
                    bool Exportou = false;

                    var wb = new XLWorkbook();
                    var ws = wb.Worksheets.Add(ddlTipos.Items[ddlTipos.SelectedIndex].Text);

                    int col=1;
                    foreach (DataColumn column in dt.Columns)
                    {
                        if (column.ColumnName != "Colunas Alteradas")
                        {
                            ws.Cell(1, col).Value = column.ColumnName;
                            //ws.Row(1).Style.Fill.BackgroundColor = XLColor.FromTheme(XLThemeColor.Accent1, 0.5);
                            col++;
                        }
                    }

                    int linha =2;
                    foreach (DataRow row in dt.Rows)
                    {
                        List<string> alteracoes = new List<string>();
                        if (Convert.ToString(row["Colunas Alteradas"]) != "")
                        {
                            alteracoes = Convert.ToString(row["Colunas Alteradas"]).Split(';').ToList();
                        }

                        int coluna=1;
                        foreach (DataColumn coll in row.Table.Columns)
                        {
                            if (coll.ColumnName != "Colunas Alteradas")
                            {
                                if (alteracoes.Contains(coll.ColumnName))
                                {
                                    ws.Cell(linha, coluna).Value = Convert.ToString(row[coll.ColumnName]);
                                    ws.Cell(linha, coluna).Style.Fill.BackgroundColor = XLColor.FromArgb(172, 254, 174);
                                }
                                else
                                    ws.Cell(linha, coluna).Value = Convert.ToString(row[coll.ColumnName]);

                                coluna++;
                            }
                        }
                            linha++;
                    }

                    ws.Range(ws.FirstCellUsed(), ws.LastCellUsed()).CreateTable().Theme = XLTableTheme.TableStyleLight9;
                    ws.Columns().AdjustToContents();
                    wb.SaveAs(path + name);
                    wb.Dispose();
                    Exportou = true;

                    /*using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt, ddlTipos.Items[ddlTipos.SelectedIndex].Text);
                        wb.SaveAs(path + name);
                        wb.Dispose();
                        Exportou = true;
                    }*/

                    if (Exportou)
                    {
                        if (ExportacaoSolicitacao.UploadFileDataBackup(web, path + name))
                        {
                            try { File.Delete(path + name); }
                            catch { }
                            string pagina = "<script>window.open('" + SPContext.Current.Web.Url.ToString() + "/Documentos Compartilhados/" + name + "','_blank');</script>";
                            Page.Response.Write(pagina);
                        }

                        Log(web, "Exportação concluída com sucesso.", plano + " - Ciclo: " + ciclo);
                    }
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Exportação efetuada com sucesso!');window.location.href='" + SPContext.Current.Web.Url.ToString() + "/SitePages/ExportarSolicitacao.aspx" + "';", true);
                }
                else
                {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Não existem dados a serem exportados neste Ciclo e Plano!');", true);
                }
                #region sem grid
                /*if (dt.Rows.Count > 0)
                {
                    string name = "attachment;filename=" + BuildFileName(DateTime.Now, ciclo + "-" + plano);
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt, ddlTipos.Items[ddlTipos.SelectedIndex].Text);

                        Page.Response.Clear();
                        Page.Response.ClearContent();
                        Page.Response.ClearHeaders();
                        Page.Response.Buffer = true;
                        Page.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Page.Response.AddHeader("content-disposition", name); //"attachment;filename=ttt.xlsx");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Page.Response.OutputStream);
                            Page.Response.Flush();
                        }
                        HttpContext.Current.ApplicationInstance.CompleteRequest();//HttpContext.Current.Response.End(); //Page.Response.End();
                    }
                }
                else
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Não existe nenhum registro de Solicitação de Mudança!');", true);

                HttpContext.Current.ApplicationInstance.CompleteRequest();
                Log(web, "Exportação Concluída", plano + " / " + ciclo);
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Exportação efetuada com sucesso!');", true);*/
                #endregion
            }
            catch (Exception ex)
            {
                Log(web, ex.Message.ToString(), plano + " / " + ciclo);
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('" + ex.Message.Replace("'", "") + "');", true);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "InfoAlert", "alert('Ocorreu um erro na exportação.');", true);
            }    
        }

        private void ExportGridToExcel()
        {
         /*   Page.Response.Clear();
            Page.Response.Buffer = true;
            Page.Response.ClearContent();
            Page.Response.ClearHeaders();
            Page.Response.Charset = "";
            string FileName = "Vithal" + DateTime.Now + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Page.Response.ContentType = "application/vnd.ms-excel";
            Page.Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            grid.GridLines = GridLines.Both;
            grid.HeaderStyle.Font.Bold = true;
            grid.RenderControl(htmltextwrtter);
            Page.Response.Write(strwritter.ToString());
            Page.Response.End();*/

        }  

        public static void Log(SPWeb web, string erro, string plano)
        {
            try
            {
                SPList GerenciaPlanos = web.Lists["Logs de Exportação"];
                SPListItem item = GerenciaPlanos.AddItem();

                item[SPBuiltInFieldId.Title] = plano;
                item["Descrição"] = erro;
                item.Update();
            }
            catch { }
        }

        private static string BuildFileName(DateTime currentdate, string CicloName)
        {
            return string.Format("Exportação Solicitações {0} - {1}.xlsx", new object[]
			{
				CicloName,
				currentdate.ToString("dd-MM-yyyy_hh-mm-ss")
			});
        }
    }
}
