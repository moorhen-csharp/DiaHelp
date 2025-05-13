using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace DiaHelp.Infrastructure
{
    public static class PageTransitionHelper
    {
        /// <summary>
        /// Анимированный переход между страницами.
        /// </summary>
        /// <param name="newPage">Новая страница</param>
        /// <param name="fromRight">true — анимация справа налево, false — слева направо</param>
        public static async Task AnimateTransition(ContentPage newPage, bool fromRight = true)
        {
            // Устанавливаем начальные параметры анимации
            newPage.Opacity = 0;
            newPage.TranslationX = fromRight ? 500 : -500;

            // Показываем страницу (но она пока "за кадром")
            Application.Current.MainPage = newPage;

            // Анимируем появление
            await newPage.TranslateTo(0, 0, 350, Easing.SinOut);
            await newPage.FadeTo(1, 200, Easing.SinIn);
        }
    }
} 