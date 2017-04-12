using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIEM.ExternalService.Extension
{
    public class TFSWrapper
    {
        private TFSQuery _TFSQuery;

        public TFSWrapper()
        {

        }

        public TFSWrapper(string TeamFoundationServer, string TeamProject, string wiql)
        {
            InitializeTFSQuery( TeamFoundationServer, TeamProject, wiql);
        }

        public TFSQuery TFSQuery
        {
            get
            {
                return _TFSQuery;
            }
        }

        public TFSQuery InitializeTFSQuery(string TeamFoundationServer, string TeamProject, string wiql)
        {
            if (_TFSQuery == null)
            {
                _TFSQuery = new TFSQuery(TeamFoundationServer, TeamProject, wiql);
            }
            else if (_TFSQuery.TFSCollectionURL == TeamFoundationServer && _TFSQuery.TeamProject == TeamProject)
            {
                _TFSQuery.ResetWiql(wiql);
            }
            else
            {
                _TFSQuery = new TFSQuery(TeamFoundationServer, TeamProject, wiql);
            }

            return _TFSQuery;
        }
    }
}
