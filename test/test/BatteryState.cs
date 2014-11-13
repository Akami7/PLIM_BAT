using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Phone.Devices.Power;

namespace test
{
    class BatteryState
    {

        BatteryState()
        {
            this.updateState();
        }

        public int level { get; set; }
        public DateTime time { get; set; }

        public void updateState()
        {

            Battery b = Battery.GetDefault();
            this.level = b.RemainingChargePercent;
            this.time = DateTime.Now;

        }
    }
}
