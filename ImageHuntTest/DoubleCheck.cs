using System;
using System.Collections.Generic;
using System.Text;
using NFluent;
using NFluent.Extensibility;

namespace ImageHuntTest
{
    public static class DoubleCheck
    {
      public static ICheckLink<ICheck<double>> IsEqualsWithDelta(this ICheck<double> check, double refValue, double delta)
      {
        var checker = ExtensibilityHelper.ExtractChecker(check);

        return checker.ExecuteCheck(
          () =>
          {
            if (Math.Abs(checker.Value - refValue) > delta)
            {
              var errorMesssage =
                FluentMessage.BuildMessage(
                  $"The {checker.Value} differs more than {delta} from reference value {refValue}");
              throw new FluentCheckException(errorMesssage.ToString());
            }
          },
          FluentMessage.BuildMessage(
            $"The {checker.Value} differs more than {delta} from reference value {refValue}").ToString());
      }
    }
}
