using Autodesk.Revit.UI;
using RevitTestModule.DockablePane;
using RevitTestModule.Handler;
using RevitTestModule.ViewModel;
using System;
using System.Reflection;

namespace RevitTestModule
{
    public class Class1 : IExternalApplication
    {
        private ExternalEvent _externalEvent;
        private SelectionChangedHandler _handler;
        private SummaryViewModel _viewModel;
        private MyPane _myPane;

        public Class1()
        {
            try
            {
                _viewModel = new SummaryViewModel();
                _myPane = new MyPane(_viewModel);
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
            }
        }

        public Result OnStartup(UIControlledApplication application)
        {
            //Панель расширения
            RibbonPanel ribbonPanel = application.CreateRibbonPanel("Summary Panel");

            //Кнопки панели
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
            var buttons = ribbonPanel.AddStackedItems(new PushButtonData("Show", "Show Panel", thisAssemblyPath, "RevitTestModule.DockablePane.Controls.Show"),
                new PushButtonData("Hide", "Hide Panel", thisAssemblyPath, "RevitTestModule.DockablePane.Controls.Hide"));

            try
            {
                //Создание DockablePane
                _myPane.RegisterDockablePane(application);

                //Создение события изменения выделения
                _handler = new SelectionChangedHandler(_viewModel);
                _externalEvent = ExternalEvent.Create(_handler);

                //Подписка на событие
                Subscribe(application);

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);

                return Result.Failed;
            }
        }

        public void Subscribe(UIControlledApplication app)
        {
            try
            {
                app.SelectionChanged += OnSelectionChanged;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
            }
        }

        private void OnSelectionChanged(object sender, Autodesk.Revit.UI.Events.SelectionChangedEventArgs e)
        {
            _externalEvent.Raise();
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
