using ChessGame.ViewModel.Base;

namespace ChessGame.Factories.ViewModelsFactories
{
    public interface IViewModelFactory<Param>
    {
        BaseViewModel CreateViewModelWithParams(Param param);
    }
}
