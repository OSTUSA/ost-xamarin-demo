using System;
using System.Windows.Input;
using OSTUSA.IoT.Services.Networking;
using OSTUSA.IoT.Core.Domain.Messages;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace OSTUSA.IoT.DemoApp.ViewModels
{
    public class RemotePageModel
    {
        #region bindables 

        public ICommand SendCommand { get; private set; }
        public ObservableCollection<string> Actions { get; private set; }

        #endregion

        #region constructor

        private int _messageId;
        private readonly MessageService _messageService;

        public RemotePageModel()
        {
            SendCommand = new Command(Perform_SendCommand);
            Actions = new ObservableCollection<string>();

            _messageService = new MessageService();
        }

        #endregion

        #region command implementations

        private async void Perform_SendCommand(object parameter)
        {
            try
            {
                var command = (CommandType)parameter;
                var message = new CommandMessage()
                {
                    MessageId = _messageId++,
                    Command = command
                };
                await _messageService.SendMessageAsync(message);
                Actions.Add($"Sent \"{message}\"");
            }
            catch (Exception ex)
            {
                Actions.Add($"Exception: {ex.Message}");
            }
        }

        #endregion
    }
}
