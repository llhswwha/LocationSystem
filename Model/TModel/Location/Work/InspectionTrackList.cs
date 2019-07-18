using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TModel.Location.Work
{
    public class InspectionTrackList
    {
        public List<InspectionTrack> AddTrack { get; set; }

        public List<InspectionTrack> ReviseTrack { get; set; }

        public List<InspectionTrack> DeleteTrack { get; set; }

        public InspectionTrackList()
        {
            AddTrack = new List<InspectionTrack>();
            ReviseTrack = new List<InspectionTrack>();
            DeleteTrack = new List<InspectionTrack>();
        }
    }
}
