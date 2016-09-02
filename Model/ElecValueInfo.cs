using System;

namespace Delta.PECS.WebCSC.Model {
    [Serializable]
    public class ElecValueInfo {
        public int LscId { get; set; }

        public int NodeId { get; set; }

        public float Value { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}
