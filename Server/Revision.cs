using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingWebsite.Server
{
    public class Revision
    {
        public string ChangedObject { get; private set; }

        public Revision(string changedObject)
        {
            ChangedObject = changedObject;
        }
    }
}
