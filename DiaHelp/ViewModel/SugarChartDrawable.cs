using DiaHelp.Model;
using Microsoft.Maui.Graphics;
using System.Collections.Generic;

namespace DiaHelp.ViewModel
{
    public class SugarChartDrawable : IDrawable
    {
        private readonly IEnumerable<SugarModel> _sugarNotes;

        public SugarChartDrawable(IEnumerable<SugarModel> sugarNotes)
        {
            _sugarNotes = sugarNotes;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (_sugarNotes == null || !_sugarNotes.Any()) return;

            // Настройка размеров графика
            var margin = 40;
            var graphWidth = dirtyRect.Width - 2 * margin;
            var graphHeight = dirtyRect.Height - 2 * margin;

            // Очистка фона
            canvas.FillColor = Colors.Transparent;
            canvas.FillRectangle(dirtyRect);

            // Отрисовка осей
            // Горизонтальная ось

            // Отрисовка линии графика
            canvas.StrokeColor = Colors.Purple;
            canvas.StrokeSize = 3;

            var maxSugarLevel = (float)_sugarNotes.Max(n => n.SugarLevel); // Максимальное значение уровня сахара
            var points = new List<PointF>();

            foreach (var (note, index) in _sugarNotes.Select((n, i) => (n, i)))
            {
                var x = margin + (index / (float)(_sugarNotes.Count() - 1)) * graphWidth;
                var y = dirtyRect.Height - margin - ((float)note.SugarLevel / maxSugarLevel) * graphHeight; // Преобразование SugarLevel в float
                points.Add(new PointF(x, y));
            }

            for (int i = 1; i < points.Count; i++)
            {
                canvas.DrawLine(points[i - 1].X, points[i - 1].Y, points[i].X, points[i].Y);
            }

            // Отрисовка меток на осях
            canvas.FontSize = 20;
            canvas.FontColor = Colors.White;

            foreach (var (note, index) in _sugarNotes.Select((n, i) => (n, i)))
            {
                var x = margin + (index / (float)(_sugarNotes.Count() - 1)) * graphWidth;
                var y = dirtyRect.Height - margin - ((float)note.SugarLevel / maxSugarLevel) * graphHeight;

                canvas.DrawString(note.SugarLevel.ToString(), x - 10, y - 20, HorizontalAlignment.Center);
            }
        }
    }
}