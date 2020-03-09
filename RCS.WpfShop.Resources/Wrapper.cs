namespace RCS.WpfShop.Resources
{
    // This class only exists because of the internal Labels constructor.
    // TODO Is this the only way?
    public class Wrapper
    {
        private static readonly Labels labels = new Labels();

        public Labels Labels { get { return labels; } }
    }
}
