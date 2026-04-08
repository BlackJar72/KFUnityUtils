namespace kfutils.UI {

    /// <summary>
    /// Tags a class as having data that could be polled by / for the 
    /// UI, and plugged in when needed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IUIDataProvider <out T> {
        T RetrieveData();
    }

}