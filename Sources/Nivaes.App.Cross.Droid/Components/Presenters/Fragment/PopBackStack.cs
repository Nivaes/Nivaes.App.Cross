namespace Nivaes.App.Cross.Droid
{
    public enum PopBackStack
    {
        /// <summary>
        /// All entries up to but not including that entry will be removed.
        /// </summary>
        None = 0,
        /// <summary>
        /// All matching entries will be consumed until one that doesn't match is found or the bottom of the stack is reached.
        /// </summary>
        Inclusive = 1
    }
}