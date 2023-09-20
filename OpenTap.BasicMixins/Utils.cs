using System;
namespace OpenTap.BasicMixins
{
    static class Utils
    {
        public static void UpgradeVerdict(this ITestStep step, Verdict verdict)
        {
            if (step.Verdict < verdict)
                step.Verdict = verdict;
        }

        public static bool IsNumeric(this ITypeData type)
        {
            if (type is TypeData td)
            {
                if (td.IsValueType)
                {
                    return Type.GetTypeCode(td.Type).IsNumeric();
                }
            }
            return false;
        }

        public static bool IsNumeric(this TypeCode typeCode)
        {
            switch (typeCode)
            {

                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                default:
                    return false;
            }
        }
    }
}
