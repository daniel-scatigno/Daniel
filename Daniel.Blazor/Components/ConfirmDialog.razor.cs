using Microsoft.AspNetCore.Components;

using System.Threading.Tasks;

namespace Daniel.Blazor.Components
{
    public partial class ConfirmDialog
    {
        public ConfirmDialog() { }

        [Parameter]
        public string Width { get; set; } = "350px";

        [Parameter]
        public string Height { get; set; } = "130px";

        /// <summary>
        /// Default: text-center
        /// </summary>
        [Parameter]
        public string TextAlign { get; set; } = "text-center";

        /// <summary>
        /// Seta o elemento onde será executado o Dialogo.
        /// Default: body
        /// </summary>
        [Parameter]
        public string Target { get; set; } = "body";

        [Parameter]
        public string Title { get; set; } = "Atenção";

        /// <summary>
        /// Mensagem que mostrará para cliente.
        /// </summary>
        [Parameter]
        public string Mensage { get; set; } = "Confirmar Mensagem";

        /// <summary>
        /// Mensagem que mostrará para cliente.
        /// Pode coloar parâmentros HTML. Ex.: <br>
        /// </summary>
        [Parameter]
        public MarkupString MarkupMensage { get; set; }

        [Parameter]
        public bool ShowCloseIcon{get;set;}

        [Parameter]
        public bool DialogVisibility { get; set; } = false;

        [Parameter]
        public EventCallback ConfirmCallback { get; set; }

        [Parameter]
        public EventCallback CancelCallback { get; set; }

        public async Task Confirm() => await ConfirmCallback.InvokeAsync();

        public async Task Cancel() => await CancelCallback.InvokeAsync();
    }
}
