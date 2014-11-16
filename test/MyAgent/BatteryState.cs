using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Phone.Devices.Power;

namespace MyAgent
{
    [DataContract]
    public class BatteryState
    {

        public BatteryState(int aRefCount = 0)
        {
            if(aRefCount != 0) // aRefCount is the last saved ref_count
                RefCount = aRefCount;
        }

        private static int RefCount = 0;

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int level { get; set; }
        [DataMember]
        public DateTime time { get; set; }

        public BatteryState updateState()
        {

            Battery b = Battery.GetDefault();
            this.level = b.RemainingChargePercent;
            this.time = DateTime.Now;
            this.Id = RefCount++;
            return this;
        }
    }
}
