namespace AoC_2018
{
    public interface IProblem
    {
        string FilePath { get; }

        void Solve_1();

        void Solve_2();
    }
}
