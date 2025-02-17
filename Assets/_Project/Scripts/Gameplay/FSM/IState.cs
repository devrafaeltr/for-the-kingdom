namespace FTKingdom
{
    public abstract class IState
    {
        internal abstract CharacterState StateType { get; }
        internal abstract void Start(CharacterBattle entity);
        internal abstract void Update(CharacterBattle entity);
        internal abstract void End(CharacterBattle entity);
    }
}