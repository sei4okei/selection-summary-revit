using Autodesk.Revit.UI;
using RevitTestModule.View;
using RevitTestModule.ViewModel;
using System;

namespace RevitTestModule.DockablePane
{
    public class MyPane : IDockablePaneProvider
    {
        //Guid DockablePane'a
        private static readonly Guid guid = new Guid("{FEFC6C07-26BA-4DC9-8B56-EAA6A1B521AF}");
        private readonly SummaryViewModel _viewModel;

        public MyPane(SummaryViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public void SetupDockablePane(DockablePaneProviderData data)
        {
            try
            {
                var summaryView = new SummaryView();

                //Определение DataContext'a wpf представления
                summaryView.DataContext = _viewModel;

                var dockablePaneId = new DockablePaneId(guid);

                //Подключение wpf представления
                data.FrameworkElement = summaryView;

                data.InitialState = new DockablePaneState
                {
                    DockPosition = DockPosition.Tabbed,
                    TabBehind = DockablePanes.BuiltInDockablePanes.ProjectBrowser
                };
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
            }
        }

        public void RegisterDockablePane(UIControlledApplication application)
        {
            try
            {
                var paneId = new DockablePaneId(new Guid("{FEFC6C07-26BA-4DC9-8B56-EAA6A1B521AF}"));
                var pane = new MyPane(_viewModel);

                //Регистрация DockablePane'a
                application.RegisterDockablePane(paneId, "Selected Object Summary", pane as IDockablePaneProvider);
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
            }
        }
    }
}