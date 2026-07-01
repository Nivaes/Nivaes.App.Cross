namespace Nivaes.App.Cross
{
    public abstract class CrossNumericOnlyValueCombiner
        : CrossObjectAsStringPairwiseValueCombiner
    {
        protected override bool CombineStringAndDouble(string input1, double input2, out object value)
        {
            value = null;
            return false;
        }

        protected override bool CombineStringAndLong(string input1, long input2, out object value)
        {
            value = null;
            return false;
        }

        protected override bool CombineStringAndDecimal(string input1, decimal input2, out object value)
        {
            value = null;
            return false;
        }

        protected override bool CombineStringAndNull(string input1, out object value)
        {
            value = null;
            return false;
        }

        protected sealed override bool CombineStringAndString(string input1, string input2, out object value)
        {
            value = null;
            return false;
        }

        protected sealed override bool CombineLongAndString(long input1, string input2, out object value)
        {
            value = null;
            return false;
        }

        protected sealed override bool CombineDoubleAndString(double input1, string input2, out object value)
        {
            value = null;
            return false;
        }

        protected sealed override bool CombineDecimalAndString(decimal input1, string input2, out object value)
        {
            value = null;
            return false;
        }

        protected sealed override bool CombineNullAndString(string input2, out object value)
        {
            value = null;
            return false;
        }
    }
}
