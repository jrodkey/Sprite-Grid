namespace SpriteGridTests
{
    using Assets.Scripts.Game.Grid.Cell;
    using NUnit.Framework;

    public class SpriteGridCellTests
    {
        [Test]
        public void Create_NewGridCell_WhenInstanceIsValid()
        {
            SpriteGridCell cell = new SpriteGridCell();
            Assert.Pass();
        }

        [TestCase(1,1,1,1)]
        public void SetColor_ChangeColor_WhenColorIsValid(float r, float g, float b, float a)
        {
            SpriteGridCell cell = new SpriteGridCell();
            cell.SetColor(r,g,b,a);
            Assert.Pass();
        }
    }
}