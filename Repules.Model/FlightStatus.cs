using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Repules.Model
{
    public enum FlightStatus
    {
        [Display(Name = "Elfogadásra vár")]
        [Description("Elfogadásra vár")]
        WaitingForAcceptance,

        [Display(Name = "Elfogadva")]
        [Description("Elfogadva")]
        Accepted,

        [Display(Name = "Elutasítva")]
        [Description("Elutasítva")]
        Declined
    };
}
