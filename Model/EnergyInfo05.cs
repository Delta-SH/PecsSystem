using System;

namespace Delta.PECS.WebCSC.Model {
    [Serializable]
    public class EnergyInfo05 {
        public string Period { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string A { get; set; }

        public string B { get; set; }

        public float AValue { get; set; }

        public float BValue { get; set; }
    }
}
