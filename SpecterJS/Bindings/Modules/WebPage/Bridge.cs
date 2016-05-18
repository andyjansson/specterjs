using SpecterJS.Util;
using System.Runtime.InteropServices;

namespace SpecterJS.Bindings.Modules.WebPage
{
    [ComVisible(true)]
    public class Bridge
    {
        private WebPage webPage;

        public Bridge(WebPage webPage)
        {
            this.webPage = webPage;
        }

        public void ConsoleMessage(string msg, int line, string source)
        {
            if (webPage.OnConsoleMessage != null)
                ObjectHelpers.DynamicInvoke(webPage.OnConsoleMessage, msg, line, source);
        }

        public void Alert(string msg)
        {
            if (webPage.OnAlert != null)
                ObjectHelpers.DynamicInvoke(webPage.OnAlert, msg);
        }

        public bool Confirm(string msg)
        {
            if (webPage.OnConfirm != null)
                return (bool)ObjectHelpers.DynamicInvoke(webPage.OnConfirm, msg);
            return false;
        }

        public dynamic CallPhantom(dynamic data)
        {
            if (webPage.OnCallback != null)
                return ObjectHelpers.DynamicInvoke(webPage.OnCallback, data);
            return null;
        }

        public string Prompt(string msg, string defaultVal)
        {
            if (webPage.OnPrompt != null)
                return (string)ObjectHelpers.DynamicInvoke(webPage.OnPrompt, msg, defaultVal);
            return null;
        }
    }
}
