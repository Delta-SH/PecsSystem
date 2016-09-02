using System;

namespace Delta.PECS.WebCSC.Model {
    [Serializable]
    public class EnergyInfo04 {
        public string Key { get; set; }

        public string Current { get; set; }

        public string Period { get; set; }

        public float Value { get; set; }

        public float HValue { get; set; }
    }
}
