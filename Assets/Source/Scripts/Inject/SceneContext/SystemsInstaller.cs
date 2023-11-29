using Game.Systems;
using Zenject;

public class SystemsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<InputSystem>()
            .AsSingle();
        Container.BindInterfacesAndSelfTo<CreateBubbleSystem>()
            .AsSingle();
        Container.BindInterfacesAndSelfTo<ScaleBubbleSystem>()
            .AsSingle();
    }
}