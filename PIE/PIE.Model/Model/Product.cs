using System;
using System.ComponentModel;

namespace PIEM.Common.Model
{
    [Obsolete]
    public enum Product
    {
        Analog = 1,
        Apps = 2,
        Content = 3,
        [Description("OS Core")]
        OSCore = 4,
        [Description("PC, Tablet, Phone")]
        PTP = 5,
        Server = 6,
        Store = 7,
        Studios = 8,
        [Description("Windows Servicing and Delivery")]
        WSD = 9,
        [Description("Xbox Platform")]
        XboxPlatform = 10
    }
}
