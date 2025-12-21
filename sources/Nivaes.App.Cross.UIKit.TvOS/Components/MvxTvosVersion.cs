namespace MvvmCross.Platforms.Tvos
{
    using MvvmCross.Exceptions;
    using Nivaes.App.Cross;

    public class MvxTvosVersion
    {
        public MvxTvosVersion(int[] parts)
        {
            if (parts == null || parts.Length == 0)
                throw new CrossException("Invalid parts in constructor for MvxTvosVersion");

            Parts = parts;
            Major = parts[0];

            if (parts.Length > 1)
            {
                Minor = parts[1];
            }
        }

        public int Major { get; private set; }
        public int Minor { get; private set; }
        public int[] Parts { get; private set; }
    }
}
