using System;
using System.Windows.Input;
using OSTUSA.IoT.Core.Domain.Messages;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using OSTUSA.IoT.Services.Azure;
using OSTUSA.IoT.Core.ViewModels;
using System.Threading.Tasks;

namespace OSTUSA.IoT.DemoApp.PageModels
{
    public class RemotePageModel : PageModel
    {
        #region bindables 

        public ICommand SendCommand { get; private set; }
        public ObservableCollection<string> Actions { get; private set; }

        #endregion

        #region constructor

        private int _messageId;
        private readonly IMessageService _messageService;
        private readonly ITwinService _twinService;

        public RemotePageModel(
            IMessageService messageService,
            ITwinService twinService
        )
        {
            _messageService = messageService;
            _twinService = twinService;

            SendCommand = new Command(Perform_SendCommand);
            Actions = new ObservableCollection<string>();
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

        #region lifecycle

        public override void OnAppearing(object sender, EventArgs e)
        {
            base.OnAppearing(sender, e);

            Task.Factory.StartNew(_twinService.Open);
        }

        #endregion
    }
}
