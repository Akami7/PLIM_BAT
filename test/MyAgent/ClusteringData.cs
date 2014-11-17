using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyAgent
{
    [DataContract]
    public class ClusteringData
    {

        public ClusteringData()
        {

            refCount = new List<int>() { 0, 0, 0, 0, 0, 0, 0 };           
            
        }

        
        [DataMember]
        public List<int> refCount;
        
        [DataMember]
        private double monday = 0.0;
        public double getMonday(){
            return this.monday;
        }

        public void setMonday(double value)
        {
             refCount[0]++;
             this.monday += value / refCount[0];
        }

        [DataMember]
        private double tuesday = 0.0;
        public double getTuesday()
        {
            return this.tuesday;
        }

        public void setTuesday(double value)
        {
            refCount[1]++;
            this.tuesday += value / refCount[1];
        }

        [DataMember]
        private double thursday = 0.0;
        public double getThursday()
        {
            return this.thursday;
        }

        public void setThursday(double value)
        {
            refCount[2]++;
            this.thursday += value / refCount[2];
        }

        [DataMember]
        private double wednesday = 0.0;
        public double getWednesday()
        {
            return this.wednesday;
        }

        public void setWednesday(double value)
        {
            refCount[3]++;
            this.wednesday += value / refCount[3];
        }

        [DataMember]
        private double friday = 0.0;
        public double getFriday()
        {
            return this.friday;
        }

        public void setFriday(double value)
        {
            refCount[4]++;
            this.friday += value / refCount[4];
        }


        [DataMember]
        private double saturday;
        public double getSaturday()
        {
            return this.saturday;
        }

        public void setSaturday(double value)
        {
            refCount[5]++;
            this.saturday += value / refCount[5];
        }

        [DataMember]
        private double sunday;
        public double getSunday()
        {
            return this.sunday;
        }

        public void setSunday(double value)
        {
            refCount[6]++;
            this.sunday += value / refCount[6];
        }

        
    }
}
