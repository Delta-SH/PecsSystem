using System;

namespace Delta.PECS.WebCSC.Model {
    [Serializable]
    public class EnergyDetailInfo02 {
        public string Period { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public float Value { get; set; }
    }
}