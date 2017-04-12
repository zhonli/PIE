using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIEM.Common.Model
{
    /// <summary>
    /// [Not Using]
    /// </summary>
    public class Feature
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public string ComponentLink { get; set; }

        public int? ParentFeatureID { get; set; }

        public virtual Feature ParentFeature { get; set; }

        public virtual IList<Feature> SubFeatures { get; set; }
    }
}
