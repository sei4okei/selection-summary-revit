using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitTestModule.ViewModel;
using System.Collections.Generic;

namespace RevitTestModule.Handler
{
    internal class SelectionChangedHandler : IExternalEventHandler
    {
        private readonly SummaryViewModel _viewModel;

        public SelectionChangedHandler(SummaryViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public void Execute(UIApplication app)
        {
            UIDocument uiDoc = app.ActiveUIDocument;
            IList<ElementId> selectedIds = (IList<ElementId>)uiDoc.Selection.GetElementIds();

            //Запуск обновления в ViewModel
            _viewModel.UpdateSelection(selectedIds, uiDoc);
        }

        public string GetName() => "Selection Changed Event Handler";
    }
}
