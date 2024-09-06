using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace RevitTestModule.DockablePane.Controls
{
    [Transaction(TransactionMode.Manual)]
    internal class Hide : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;

            uiapp.GetDockablePane(new DockablePaneId(new Guid(@"{FEFC6C07-26BA-4DC9-8B56-EAA6A1B521AF}"))).Hide();

            return Result.Succeeded;
        }
    }
}
