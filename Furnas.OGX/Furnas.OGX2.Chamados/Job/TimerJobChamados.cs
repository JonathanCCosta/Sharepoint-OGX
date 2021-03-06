using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furnas.OGX2.Chamados.Job
{
    public class TimerJobChamados : SPJobDefinition
    {
        public TimerJobChamados()
            : base()
        {
        }

        public TimerJobChamados(string jobName, SPService service,
               SPServer server, SPJobLockType lockType)
            : base(jobName, service, server, lockType)
        {
            this.Title = "Job - Controle Chamados GGA";
        }

        public TimerJobChamados(string jobName, SPWebApplication webapp)
            : base(jobName, webapp, null, SPJobLockType.Job)
        {
            this.Title = "Job - Controle Chamados GGA";
        }

        public override void Execute(Guid targetInstanceId)
        {
            SPWebApplication webApp = this.Parent as SPWebApplication;
            //SPSite site = webApp.Sites;

            foreach (SPSite site in webApp.Sites)
            {
                //if (site.ServerRelativeUrl.Contains("/portalativosfurnas"))
                if (site.ServerRelativeUrl.Contains("/donet"))
                {
                    try
                    {
                        string url = site.ServerRelativeUrl.ToString() + "/gga";
                        using (SPWeb web = site.OpenWeb(url))
                        {
                            VerificadorJobs.ChamadosStatus(web);
                        }

                    }
                    catch { }
                }
            }
            /*try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite("http://shpt15/portalativosfurnas/gga"))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            VerificadorJobs.ChamadosStatus(web);
                        }
                    }
                });
            }
            catch { }*/
        }
    }
}
