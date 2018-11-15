using BLL.Blls.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Buffers
{
    public class RealPositionBuffer : BaseBuffer
    {
        public static RealPositionBuffer Instance;

        static RealPositionBuffer()
        {
            Instance = new RealPositionBuffer();
        }

        private Bll bll;

        private LocationCardPositionBll PosBll;

        private RealPositionBuffer(Bll bll)
        {
            this.bll = bll;
            PosBll = bll.LocationCardPositions;
        }

        private RealPositionBuffer()
        {
            this.bll = new Bll();
            PosBll = bll.LocationCardPositions;
        }

        protected override void UpdateData()
        {
            
        }

        public void UpdatePos()
        {

        }
    }
}
