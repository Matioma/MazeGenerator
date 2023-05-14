namespace Assets.Scripts
{
    internal interface IGenerator
    {
        bool[,] GenerateMaze(int width, int depth);
    }
}
