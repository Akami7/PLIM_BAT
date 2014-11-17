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
            Monday =
            Tuesday =
            Thursday =
            Wednesday =
            Friday =
            Saturday =
            Sunday = 0.0;

            refCount = new List<int>() { 0, 0, 0, 0, 0, 0, 0 };
        }

        [DataMember]
        public List<int> refCount; 
        [DataMember]
        public double Monday
        {
            get { return Monday; }
            set { 
                refCount[0]++;
                Monday += value / refCount[0];
            }
        } 
        [DataMember]
        public double Tuesday
        {
            get { return Tuesday; }
            set
            {
                refCount[1]++;
                Tuesday += value / refCount[1];
            }
        }
        [DataMember]
        public double Thursday
        {
            get { return Thursday; }
            set
            {
                refCount[2]++;
                Thursday += value / refCount[2];
            }
        }
        [DataMember]
        public double Wednesday
        {
            get { return Wednesday; }
            set
            {
                refCount[3]++;
                Wednesday += value / refCount[3];
            }
        }
        [DataMember]
        public double Friday
        {
            get { return Friday; }
            set
            {
                refCount[4]++;
                Friday += value / refCount[4];
            }
        }
        [DataMember]
        public double Saturday
        {
            get { return Saturday; }
            set
            {
                refCount[5]++;
                Saturday += value / refCount[5];
            }
        }
        [DataMember]
        public double Sunday
        {
            get { return Sunday; }
            set
            {
                refCount[6]++;
                Sunday += value / refCount[6];
            }
        }
    }
}
