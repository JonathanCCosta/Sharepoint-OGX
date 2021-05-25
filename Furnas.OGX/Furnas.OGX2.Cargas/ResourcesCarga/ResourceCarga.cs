using ClosedXML.Excel;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furnas.OGX2.Cargas.ResourcesCarga
{
    public class ResourceCarga
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
                            dt.Rows.Add();
                            int i = 0;
                            foreach (IXLCell cell in cells)
                            {
                                try
                                {
                                    dt.Rows[dt.Rows.Count - 1][i] = Convert.ToString(cell.Value);
                                    //dt.Rows.Add(cell.Value);
                                    i++;
                                }
                                catch { }
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

        public static Stream LerDocumentNaBiblioteca(SPWeb web, string arquivo)
        {
            try
            {
                SPFile file = web.GetFile("DocumentosCarga/" + arquivo);
                Stream st = file.OpenBinaryStream();
                return st;
            }
            catch (Exception e1)
            {
                WriteLog(web, "Carga - Ler Aqruivo GrupoSubgrupo", e1.Message);
                return null;
            }
        }

        public static void WriteLog(SPWeb web, string titulo, string erro)
        {
            try
            {
                SPList list = web.Lists["Logs"];
                if (list != null)
                {
                    SPListItem item = list.AddItem();
                    item[SPBuiltInFieldId.Title] = titulo;
                    item["Descricao"] = erro;
                    item.Update();
                }
            }
            catch { }
        }

        public static bool PegaArquivo(SPWeb web, string fullfilename, SPListItem item)
        {
            bool result=false;
            try
            {
                if (!File.Exists(fullfilename))
                {
                    throw new FileNotFoundException("File not found.", fullfilename);
                }
                SPDocumentLibrary sPFolder = web.Lists["Documentos Manual Técnico de Campo"] as SPDocumentLibrary;
                string nameFileNovo = Path.GetFileNameWithoutExtension(fullfilename) + "_" + item.ID + Path.GetExtension(fullfilename);
                string destURL = sPFolder.RootFolder.Url + "/" + nameFileNovo;
                FileStream file = File.OpenRead(fullfilename);
                SPFile itemFile = sPFolder.RootFolder.Files.Add(destURL, file, true);
                SPListItem itemProp = itemFile.Item;
                itemProp["ManualTecnico"] = new SPFieldLookupValue(item.ID, item[SPBuiltInFieldId.Title].ToString());
                itemProp.Update();

                file.Close(); file.Dispose();
                result = true;
            }
            catch (Exception innerException)
            {
                ResourceCarga.WriteLog(item.Web, "Erro Documentos",  fullfilename + " - " + innerException.Message);
                //throw new Exception("Não foi possível realizar o upload do backup para a biblioteca de documentos.", innerException);
            }
            return result;
        }
    }
}
