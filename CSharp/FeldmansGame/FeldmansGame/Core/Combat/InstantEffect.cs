using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mainframe.Core.Combat
{
    public class InstantEffect
    {
        protected string effectName;
        protected int tarAoERange, casterAoERange;
        protected float tarHPChange, tarEPChange,
            tarAoEHPChange, tarAoEEPChange,
            castAoEHPChange, castAoEEPChange;
        protected bool AoEFallsOff;

        public InstantEffect()
        {
            effectName = "KillTheThings";
            tarAoEEPChange = 0;
            tarAoEHPChange = 0;
            tarAoERange = 0;
            tarEPChange = 0;
            tarHPChange = 0;
            castAoEEPChange = 0;
            castAoEHPChange = 0;
            casterAoERange = 0;
            AoEFallsOff = false;
        }

        public InstantEffect(string Name, float TargetHPChange, float TargetEPChange, 
                            float TargetAoEHPChange, float TargetAoEEPChange, int TargetAoERange,
                            float CasterAoEHPChange, float CasterAoEEPChange, int CasterAoERange, bool AoEFallsOff)
        {
            effectName = Name;
            tarAoEEPChange = TargetAoEEPChange;
            tarAoEHPChange = TargetAoEHPChange;
            tarAoERange = TargetAoERange;
        }
    }
}