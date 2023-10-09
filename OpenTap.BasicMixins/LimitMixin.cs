using System;
namespace OpenTap.BasicMixins
{
    [Display("Limit Mixin")]
    class LimitMixin : ValidatingObject, ITestStepPostRunMixin
    {
        [Display("Upper Limit")]
        [EnabledIf(nameof(UpperLimitActive), HideIfDisabled = true)]
        public double UpperLimit { get; set; }
        
        [Display("Lower Limit")]
        [EnabledIf(nameof(LowerLimitActive), HideIfDisabled = true)]
        public double LowerLimit { get; set; }
        
        public bool LowerLimitActive { get; }
        
        public bool UpperLimitActive { get; }
        
        readonly string targetMemberName;
        static readonly TraceSource log = Log.CreateSource("Limits");
        public void OnPostRun(TestStepPostRunEventArgs eventArgs)
        {
            var targetMember = TypeData.GetTypeData(eventArgs.TestStep).GetMember(targetMemberName);
            if (targetMember == null)
            {
                eventArgs.TestStep.UpgradeVerdict(Verdict.Error);
                return;
            }
            var conv = targetMember.GetValue(eventArgs.TestStep) as IConvertible;
            var value = conv.ToDouble(null);
            if (value > UpperLimit && UpperLimitActive)
            {
                eventArgs.TestStep.UpgradeVerdict(Verdict.Fail);
                log.Debug($"Upper {targetMember.GetDisplayAttribute().Group} failed for ${eventArgs.TestStep.GetFormattedName()}.");
            }else if (value < LowerLimit && LowerLimitActive)
            {
                eventArgs.TestStep.UpgradeVerdict(Verdict.Fail);
                log.Debug($"Lower {targetMember.GetDisplayAttribute().Group} failed for ${eventArgs.TestStep.GetFormattedName()}.");
            }
            else
            {
                eventArgs.TestStep.UpgradeVerdict(Verdict.Pass);
            }
        }

        public LimitMixin(bool lowerLimitActive, bool upperLimitActive, string targetMemberName, double lowerLimit, double upperLimit)
        {
            LowerLimitActive = lowerLimitActive;
            UpperLimitActive = upperLimitActive;
            this.targetMemberName = targetMemberName;
            UpperLimit = upperLimit;
            LowerLimit = lowerLimit;
        }
    }

}
