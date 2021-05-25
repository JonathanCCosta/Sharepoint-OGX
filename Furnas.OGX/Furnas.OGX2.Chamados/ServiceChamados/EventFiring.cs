using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furnas.OGX2.Chamados.ServiceChamados
{
    public class EventFiring : SPItemEventReceiver
    {
        public void DisableHandleEventFiring()
        {

            this.EventFiringEnabled = false;

        }

        public void EnableHandleEventFiring()
        {

            this.EventFiringEnabled = true;

        }
    }
}
