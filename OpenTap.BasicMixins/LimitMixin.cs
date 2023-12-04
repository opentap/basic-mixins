using System;
using System.Text;

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
            
            string BuildMessage()
            {
                var memberName = targetMember.GetDisplayAttribute().GetFullName();
                var sb = new StringBuilder();
                sb.Append($"Limit check for '{memberName}' in step '{eventArgs.TestStep.GetFormattedName()}' failed: Expected ");
                if (LowerLimitActive)
                    sb.Append($"{LowerLimit} < ");
                sb.Append($"'{memberName}'");
                if (UpperLimitActive)
                    sb.Append($" < {UpperLimit}");
                sb.Append($". (Was {value})");
                return sb.ToString();
            }
            
            if (value > UpperLimit && UpperLimitActive || value < LowerLimit && LowerLimitActive) {
                eventArgs.TestStep.UpgradeVerdict(Verdict.Fail);
                log.Info(BuildMessage());
            } else {
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
