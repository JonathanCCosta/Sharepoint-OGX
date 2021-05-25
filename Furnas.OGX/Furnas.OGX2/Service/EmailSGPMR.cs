using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Furnas.OGX2.Service
{
    public class EmailSGPMR
    {
        public class Email
        {
            public string Assunto { get; set; }
            public string Corpo { get; set; }
            public string Para { get; set; }
            public string CC { get; set; }
            public string CCO { get; set; }

            private List<string> _Anexos;

            public List<string> Anexos
            {
                get
                {
                    if (_Anexos == null)
                        _Anexos = new List<string>();
                    return _Anexos;
                }
                set { _Anexos = value; }
            }

            public Email()
            {

            }

            public bool EnviaEmail(SPWeb Web)
            {
                string SharePointSmtp;
                string SharePointDe;

                try
                {
                    SharePointSmtp = SPAdministrationWebApplication.Local.OutboundMailServiceInstance.Server.Address;
                    SharePointDe = SPAdministrationWebApplication.Local.OutboundMailSenderAddress;
                }
                catch
                {
                    return false;
                }

                MailMessage eMail = new MailMessage();

                //Monta Email
                eMail.Subject = this.Assunto;
                eMail.Body = this.Corpo;
                eMail.IsBodyHtml = true;
                eMail.From = new MailAddress(SharePointDe, "Portal da Gerência de Gestão e Monitoramento de Ativos - GGA.O");
                foreach (string anexo in this.Anexos)
                {
                    SPFile file = Web.GetFile(anexo);
                    Attachment attach = new Attachment(file.OpenBinaryStream(), System.Net.Mime.MediaTypeNames.Application.Octet);
                    attach.Name = file.Name;
                    System.Net.Mime.ContentDisposition disposition = attach.ContentDisposition;
                    disposition.CreationDate = file.TimeCreated;
                    disposition.ModificationDate = file.TimeLastModified;
                    disposition.ReadDate = file.TimeLastModified;
                    eMail.Attachments.Add(attach);
                }
                if (!String.IsNullOrEmpty(this.Para))
                {
                    string[] destinatariosPara = this.Para.Split(';');

                    foreach (string destinatarioPara in destinatariosPara)
                    {
                        if (!String.IsNullOrEmpty(destinatarioPara))
                        {
                            MailAddress DestinatarioPara = new MailAddress(destinatarioPara);
                            eMail.To.Add(DestinatarioPara);
                        }
                    }
                }


                if (!String.IsNullOrEmpty(this.CC))
                {
                    string[] destinatariosCC = this.CC.Split(';');

                    foreach (string destinatarioCC in destinatariosCC)
                    {
                        if (!String.IsNullOrEmpty(destinatarioCC))
                        {
                            MailAddress DestinatarioCC = new MailAddress(destinatarioCC);
                            eMail.CC.Add(DestinatarioCC);
                        }
                    }
                }

                if (!String.IsNullOrEmpty(this.CCO))
                {
                    string[] destinatariosCCO = this.CCO.Split(';');

                    foreach (string destinatarioCCO in destinatariosCCO)
                    {
                        if (!String.IsNullOrEmpty(destinatarioCCO))
                        {
                            MailAddress DestinatarioCCO = new MailAddress(destinatarioCCO);
                            eMail.Bcc.Add(DestinatarioCCO);
                        }
                    }
                }

                SmtpClient smtpClient = new SmtpClient(SharePointSmtp);

                smtpClient.Send(eMail);
                return true;
            }
        }
    }
}
