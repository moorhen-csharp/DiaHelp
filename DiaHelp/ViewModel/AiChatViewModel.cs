using DiaHelp.Interface;
using System.Windows.Input;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace DiaHelp.ViewModel
{
    public class AiChatViewModel : BaseViewModel
    {
        private string _userMessage;
        private readonly IAiChatService _aiChatService;
        private readonly IWindowService _windowService;
        private bool _isLoading;
        public ObservableCollection<ChatMessage> Messages { get; }

        public AiChatViewModel(IWindowService windowService, IAiChatService aiChatService)
        {
            _windowService = windowService;
            _aiChatService = aiChatService;

            Messages = new ObservableCollection<ChatMessage>
            {
                new ChatMessage("Привет! Я ваш помощник по диабету. Задайте вопрос.", false)
            };

            SendMessageCommand = new RelayCommand(async (parametr) => await SendMessage());
            BackCommand = new RelayCommand(GoBack);
        }

        public string UserMessage
        {
            get => _userMessage;
            set
            {
                if (_userMessage != value)
                {
                    _userMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged();
                }
            }
        }




        private async Task SendMessage()
        {
            if (string.IsNullOrWhiteSpace(UserMessage))
                return;

            try
            {
                IsLoading = true;
                var userMsg = UserMessage;
                Messages.Add(new ChatMessage(userMsg, true));
                UserMessage = string.Empty;

                var response = await _aiChatService.GetDiabetesResponseAsync(userMsg);
                Messages.Add(new ChatMessage(response, false));
            }
            catch (Exception ex)
            {
                Messages.Add(new ChatMessage($"Ошибка: {ex.Message}", false));
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void GoBack(object parameter)
        {
            Application.Current.MainPage = _windowService.GetAndCreateContentPage<MainViewModel>().View;
        }


        public ICommand SendMessageCommand { get; }
        public ICommand BackCommand { get; }
    }

    public record ChatMessage(string Text, bool IsUser);

}