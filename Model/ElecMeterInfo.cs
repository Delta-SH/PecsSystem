using System;

namespace Delta.PECS.WebCSC.Model {
    [Serializable]
    public class ElecMeterInfo {
        public int LscId { get; set; }

        public int NodeId { get; set; }

        public float Value { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
