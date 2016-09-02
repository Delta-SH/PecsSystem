using System;

namespace Delta.PECS.WebCSC.Model {
    [Serializable]
    public class EnergyDetailInfo {
        public string Name { get; set; }

        public string Start { get; set; }

        public string End { get; set; }

        public float Value { get; set; }
    }
}
