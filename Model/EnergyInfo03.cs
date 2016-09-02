using System;
using System.Collections.Generic;

namespace Delta.PECS.WebCSC.Model {
    [Serializable]
    public class EnergyInfo03 {
        public string Key { get; set; }

        public string Current { get; set; }

        public string Period { get; set; }

        public float Value { get; set; }

        public float TValue { get; set; }
    }
}
