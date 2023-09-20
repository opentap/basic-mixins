using System;
namespace OpenTap.BasicMixins
{
    [Display("Limit Mixin")]
    class LimitMixin : ValidatingObject, ITestStepPostRunMixin
    {
        [Display("Upper Limit")]
        [EnabledIf(nameof(UpperLimitActive))]
        public double UpperLimit { get; set; }
        
        [Display("Lower Limit")]
        [EnabledIf(nameof(LowerLimitActive))]
        public double LowerLimit { get; set; }
        
        public bool LowerLimitActive { get; }
        
        public bool UpperLimitActive { get; }
        
        
        readonly string targetMemberName;

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
            if (value > UpperLimit)
            {
                eventArgs.TestStep.UpgradeVerdict(Verdict.Fail);
            }else if (value < LowerLimit)
            {
                eventArgs.TestStep.UpgradeVerdict(Verdict.Fail);
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
