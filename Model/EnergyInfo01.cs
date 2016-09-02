using System;
using System.Collections.Generic;

namespace Delta.PECS.WebCSC.Model {
    [Serializable]
    public class EnergyInfo01 {
        public string Key { get; set; }

        public string Current { get; set; }

        public float NHPAT { get; set; }

        public List<EnergyDetailInfo01> NHPATDetail { get; set; }

        public float NHPBT { get; set; }

        public List<EnergyDetailInfo01> NHPBTDetail { get; set; }

        public float NHPCT { get; set; }

        public List<EnergyDetailInfo01> NHPCTDetail { get; set; }

        public float NHPDT { get; set; }

        public List<EnergyDetailInfo01> NHPDTDetail { get; set; }

        public float NHPET { get; set; }

        public List<EnergyDetailInfo01> NHPETDetail { get; set; }

        public float NHPFT { get; set; }

        public List<EnergyDetailInfo01> NHPFTDetail { get; set; }

        public float NHPT { get; set; }

        public List<EnergyDetailInfo01> NHPTDetail { get; set; }
    }
}
