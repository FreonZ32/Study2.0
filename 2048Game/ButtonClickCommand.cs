using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048Game
{
    internal class ButtonClickCommand: CommandBase
    {
        public ButtonClickCommand(Action<string> callback)
       => _callback = callback;

        private readonly Action<string>? _callback;

        public override void Execute(object? parameter)
        {
            _callback?.Invoke(parameter as string ?? string.Empty);
        }
    }
}
