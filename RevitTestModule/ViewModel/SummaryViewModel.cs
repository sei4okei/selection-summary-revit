using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitTestModule.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace RevitTestModule.ViewModel
{

    public class SummaryViewModel : INotifyPropertyChanged
    {
        public List<Grouped> listGrouped { get; set; }

        public void UpdateSelection(IList<ElementId> elementIds, UIDocument doc)
        {
            var elements = elementIds.Select(x => doc.Document.GetElement(x)).ToList();

            // Словарь для хранения группированных данных по категориям
            var groupedElements = new Dictionary<string, Grouped>();

            // Перебираем все элементы
            foreach (var element in elements)
            {
                // Получаем категорию элемента
                var categoryName = element.Category?.Name ?? "Unknown Category";

                // Если категория не была добавлена в словарь, добавляем
                if (!groupedElements.ContainsKey(categoryName))
                {
                    groupedElements[categoryName] = new Grouped
                    {
                        CategoryName = categoryName
                    };
                }

                //Считаем кол-во элементов данной категории
                groupedElements[categoryName].ElementCount++;

                // Получаем параметры элемента
                foreach (var param in element.GetOrderedParameters())
                {
                    //Название параметра элемента
                    var paramName = param.Definition.Name;

                    //Проверка на наличие значения
                    if (!param.HasValue)
                    {
                        continue;
                    }

                    // Попробуем получить числовое значение
                    if (!double.TryParse(param.AsValueString(), out double paramValue))
                    {
                        continue;
                    }
                    // Если параметр уже есть в словаре, суммируем значения
                    if (groupedElements[categoryName].ParameterSums.ContainsKey(paramName))
                    {
                        groupedElements[categoryName].ParameterSums[paramName] += paramValue;
                    }
                    else
                    {
                        // Иначе добавляем новый параметр в словарь
                        groupedElements[categoryName].ParameterSums[paramName] = paramValue;
                    }
                }
            }

            // Возвращаем список сгруппированных данных
            listGrouped = groupedElements.Values.ToList();
            //Вызываем обновление списка
            OnPropertyChanged(nameof(listGrouped));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
            }
        }
    }
}
