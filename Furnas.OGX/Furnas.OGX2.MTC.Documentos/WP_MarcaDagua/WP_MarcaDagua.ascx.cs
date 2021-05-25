using Microsoft.SharePoint;
using PdfSharp.Drawing;
using PdfSharp.Pdf.IO;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Web.UI.WebControls.WebParts;

namespace Furnas.OGX2.MTC.Documentos.WP_MarcaDagua
{
    [ToolboxItemAttribute(false)]
    public partial class WP_MarcaDagua : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public WP_MarcaDagua()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string fileServer = string.Empty; bool sucess = false;
            try
            {
                string link = Page.Request.QueryString.GetValues("Link")[0];
                string docid = Page.Request.QueryString.GetValues("Doc")[0];
                fileServer = BuscaDocumento(link, Convert.ToInt32(docid));

                if (fileServer != string.Empty)
                {
                    InseriMarca(fileServer);
                    sucess = true;
                    Page.Response.ClearHeaders();
                    Page.Response.ContentType = "application/pdf";
                    Page.Response.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(fileServer));
                    Page.Response.TransmitFile(fileServer);
                    Page.Response.End();
                }
            }
            catch {
                if(sucess)
                {
                    //try { File.Delete(fileServer); }
                    //catch { }
                    Log(Path.GetFileName(fileServer));
                }
            }
        }

        protected void Log(string file)
        {
            try
            {
                SPWeb web = SPContext.Current.Web;

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                   SPSite ImpersonatedSite = new SPSite(web.Url);
                   SPWeb ImpersonatedWeb = ImpersonatedSite.OpenWeb();

                   SPList mtc = ImpersonatedWeb.Lists["Logs MTC"];
                   SPListItem item = mtc.AddItem();

                   string mat = web.CurrentUser.LoginName.Split('\\')[1];
                   //bool allowUpdates = web.AllowUnsafeUpdates; //store original value
                   ImpersonatedWeb.AllowUnsafeUpdates = true;

                   item[SPBuiltInFieldId.Title] = mat;
                   item["Documento"] = file;
                   item.Update();

                   ImpersonatedWeb.AllowUnsafeUpdates = false;
                });
            }
            catch(Exception ex) { }
        }

        public static void LogError(SPWeb web, string erro, string dados)
        {
            try
            {
                SPList log = web.Lists["Logs"];
                SPListItem item = log.AddItem();
                
                bool allowUpdates = web.AllowUnsafeUpdates; //store original value
                web.AllowUnsafeUpdates = true;

                item[SPBuiltInFieldId.Title] = dados;
                item["Descrição"] = erro;
                item.Update();

                web.AllowUnsafeUpdates = allowUpdates;
            }
            catch { }
        }

        protected string BuscaDocumento(string url, int idDoc)
        {
            try
            {
                SPList docLib = SPContext.Current.Web.Lists["Documentos Manual Técnico de Campo"];
                SPFile file = docLib.GetItemById(idDoc).File;

                string fullname = string.Empty;

                if (file != null)
                {

                    string path = @"C:\DocumentosMTC\";
                    if (!Directory.Exists(path))
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(path); directoryInfo.Create();
                    }
                    
                    fullname = path + Path.GetFileName(url);
                    // retrieve the file as a byte array byte[] bArray = file.OpenBinary(); 
                    string filePath = Path.Combine(path, file.Name);
                    //open the file stream and write the file 
                    using (FileStream filestream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        byte[] bArray = file.OpenBinary();
                        filestream.Write(bArray, 0, bArray.Length);
                    }
                }

                return fullname;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        protected void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        } 

        protected void InseriMarca(string pathDoc)
        {
            PdfSharp.Pdf.PdfDocument PDFDoc = PdfSharp.Pdf.IO.PdfReader.Open(pathDoc, PdfDocumentOpenMode.Modify);
            //PdfSharp.Pdf.PdfDocument PDFNewDoc = new PdfSharp.Pdf.PdfDocument();
            for (int Pg = 0; Pg < PDFDoc.Pages.Count; Pg++)
            {
                // Variation 3: Draw a watermark as a transparent graphical path above text.
                // NYI: Does not work in Core build.
                PdfSharp.Pdf.PdfPage page = PDFDoc.Pages[Pg];
                XFont font = new XFont("Verdana", 30, XFontStyle.Regular);
                string watermark = "Uso exclusivo de Furnas Centrais Elétricas";
                // Get an XGraphics object for drawing above the existing content.
                var gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Prepend);


                // Get the size (in points) of the text.
                var size = gfx.MeasureString(watermark, font);

                if (page.Width <= 596)
                {
                    // Define a rotation transformation at the center of the page.
                    gfx.TranslateTransform(page.Width / 2, page.Height / 2);
                    gfx.RotateTransform(-Math.Atan(page.Height / page.Width) * 180 / Math.PI);
                    gfx.TranslateTransform(-page.Width / 2, -page.Height / 2);
                }
                else
                {
                    gfx.TranslateTransform(page.Width / 5.5, page.Height / 5.5);
                    gfx.RotateTransform(-Math.Atan(page.Height / page.Width) * 180 / Math.PI);
                    gfx.TranslateTransform(-page.Width / 4, -page.Height / 5.5);
                }

                // Create a graphical path.
                var path = new XGraphicsPath();

                // Create a string format.
                var format = new XStringFormat();
                format.Alignment = XStringAlignment.Center;
                format.LineAlignment = XLineAlignment.Center;

                // Add the text to the path.
                // AddString is not implemented in PDFsharp Core.
                path.AddString(watermark, font.FontFamily, XFontStyle.BoldItalic, 35, new XPoint(298, 420), format);
                //new XPoint((page.Width - size.Width)/3, (page.Height - size.Height)/2),
                //format);

                // Create a dimmed red pen and brush.
                var pen = new XPen(XColor.FromArgb(50, 128, 128, 128), 1);
                XBrush brush = new XSolidBrush(XColor.FromArgb(50, 220, 220, 220));

                // Stroke the outline of the path.
                gfx.DrawPath(pen, brush, path);
            }

            PDFDoc.Save(pathDoc);
        }
    }
}
