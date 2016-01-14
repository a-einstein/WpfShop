namespace Demo.Resources
{
    // This class only exists because of the internal Labels constructor.
    // TODO Is this the only way?
    public class Wrapper
    {
        private static Labels labels = new Labels();

        public Labels Labels { get { return labels; } }
    }
}
