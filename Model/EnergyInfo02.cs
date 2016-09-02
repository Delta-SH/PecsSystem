using System;
using System.Collections.Generic;

namespace Delta.PECS.WebCSC.Model {
    [Serializable]
    public class EnergyInfo02 {
        public string Key { get; set; }

        public string Current { get; set; }

        public List<EnergyDetailInfo02> Details { get; set; }
    }
}
