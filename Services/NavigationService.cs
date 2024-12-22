namespace CKL_Studio.Services
{
    public class NavigationService : INavigationService
    {
        public void NavigateTo(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.MainView:
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    break;

                case ViewType.PreprocessingView:
                    var preprocessingWindow = new PreprocessingWindow();
                    preprocessingWindow.Show();
                    break;
            }
        }
    }
}
