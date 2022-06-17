using Microsoft.AspNetCore.Components.Forms;

namespace PropMng.Web.Shared
{
    public class GdncInputSelect<TValue> : InputSelect<TValue>
    {
        protected override bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
        {
            if (typeof(TValue) == typeof(int))
            {
                if (int.TryParse(value, out var resutInt))
                {
                    result = (TValue)(object)resutInt;
                    validationErrorMessage = null;
                    return true;
                }
                else
                {
                    result = default;
                    validationErrorMessage = $"the selected value {value} is not v valid number";
                    return false;
                }
            }
            else if (typeof(TValue) == typeof(int?))
            {
                if (int.TryParse(value, out var resutInt))
                {
                    result = (TValue)(object)resutInt;
                    validationErrorMessage = null;
                    return true;
                }
                else
                {
                    result = default;
                    validationErrorMessage = $"the selected value {value} is not v valid number";
                    return false;
                }
            }
            else
            {
                return base.TryParseValueFromString(value, out result, out validationErrorMessage);
            }

        }
    }
}
