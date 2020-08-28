namespace GameServices.UI
{
    public abstract class UIElementView<T> : UIElementBase where T : IDataModel
    {
        public abstract void UpdateView(T model);
    }
}
