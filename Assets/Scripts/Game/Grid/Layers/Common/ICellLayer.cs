namespace Assets.Scripts.Grid.Layers.Common
{
    public interface ICellLayer
    {
        void Create(LayerProperties properties);

        void Load();

        void UpdateColor(float r, float g, float b, float a = 1.0f);
    }
}
