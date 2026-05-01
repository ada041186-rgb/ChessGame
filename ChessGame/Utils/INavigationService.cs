using ChessGame.ViewModel.Base;

namespace ChessGame.Utils
{
    public interface INavigationService
    {
        BaseViewModel CurrentView { get; }
        event Action ViewChanged;
        void NavigateTo<T>() where T : BaseViewModel;
        void NavigateTo(BaseViewModel viewModel);
    }
}
