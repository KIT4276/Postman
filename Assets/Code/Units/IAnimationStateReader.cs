public interface IAnimationStateReader
{
    void EnteredStaste(int stateHash);
    void ExitedStste(int stateHash);

    AnimatorState State { get; }
}
