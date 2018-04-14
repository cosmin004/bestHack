using System;
using System.Collections.Generic;
using System.Text;

namespace TheBoringTeam.CIAssistant.BusinessEntities.Interfaces
{
    public interface ITrackable
    {
        DateTime? DateModification { get; set; }
        DateTime DateCreation { get; set; }
    }
}
