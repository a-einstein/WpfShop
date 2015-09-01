namespace WpfTestApplication.Model
{
    // Note that nullable properties are changed in the xsd to have NullValue to be 'Null' instead of 'Throw exception'. 
    // For value types like int a true value may have been chosen, like 0.
    // It is done by hand. There might be some setting to generate that standardly, but only in paid versions of VS.
    // Strangely, setting ProductDetailsRow.LargePhoto to have NullValue to be 'Empty' made the property disappear altogether.
    // This did bot occur with ProductsOverviewRow.ThumbNailPhoto.
    // This has been done to prevent StrongTypingException.
    // The exception may not occur anyway, as it was suppressed after an occurance. 
    // In VS2015 Express I have not seen an explicit setting to reset that, while the general control of exceptions that was present in VS2013 Express seems to have been taken out.

    partial class ProductsDataSet
    {
    }
}
