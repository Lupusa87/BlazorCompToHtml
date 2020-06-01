using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Linq;


namespace BlazorLibCompToHTML
{
    public static class BCompToHTML
    {

        private static readonly Func<string, string> _encoder = (string t) => HtmlEncoder.Default.Encode(t);

        public static async Task<IEnumerable<string>> CovertToHtmlAsync(RenderFragment renderFragment)
        {
            var serviceProvider = new ServiceCollection().AddSingleton(renderFragment).BuildServiceProvider();

            var htmlRenderer = GetHtmlRenderer(serviceProvider);


            ComponentRenderedText r = await htmlRenderer.Dispatcher.InvokeAsync(() => htmlRenderer.RenderComponentAsync<TestComponent>(ParameterView.Empty));

            return r.Tokens;
        }


        public static IEnumerable<string> CovertToHtml(RenderFragment renderFragment)
        {
            var serviceProvider = new ServiceCollection().AddSingleton(renderFragment).BuildServiceProvider();


            var htmlRenderer = GetHtmlRenderer(serviceProvider);

            return GetResult(htmlRenderer.Dispatcher.InvokeAsync(() => htmlRenderer.RenderComponentAsync<TestComponent>(ParameterView.Empty)));
        }


        public static async Task<string> CovertToHtmlStringAsync(RenderFragment renderFragment)
        {
            var serviceProvider = new ServiceCollection().AddSingleton(renderFragment).BuildServiceProvider();

            var htmlRenderer = GetHtmlRenderer(serviceProvider);


            ComponentRenderedText r = await htmlRenderer.Dispatcher.InvokeAsync(() => htmlRenderer.RenderComponentAsync<TestComponent>(ParameterView.Empty));


            StringBuilder sb = new StringBuilder();

            foreach (var item in r.Tokens)
            {
                sb.Append(item);
            }

            

            return sb.ToString();
        }


        public static string CovertToHtmlString(RenderFragment renderFragment)
        {
            var serviceProvider = new ServiceCollection().AddSingleton(renderFragment).BuildServiceProvider();


            var htmlRenderer = GetHtmlRenderer(serviceProvider);

            IEnumerable<string> r= GetResult(htmlRenderer.Dispatcher.InvokeAsync(() => htmlRenderer.RenderComponentAsync<TestComponent>(ParameterView.Empty)));

            StringBuilder sb = new StringBuilder();

            foreach (var item in r)
            {
                sb.Append(item);
            }



            return sb.ToString();

        }


        private static HtmlRenderer GetHtmlRenderer(IServiceProvider serviceProvider)
        {
            return new HtmlRenderer(serviceProvider, NullLoggerFactory.Instance, _encoder);
        }


        private static IEnumerable<string> GetResult(Task<ComponentRenderedText> task)
        {

            return task.Result.Tokens;

        }

        private class TestComponent : Microsoft.AspNetCore.Components.IComponent
        {
            private RenderHandle _renderHandle;

            [Inject]
            public RenderFragment Fragment { get; set; }

            public void Attach(RenderHandle renderHandle)
            {
                _renderHandle = renderHandle;
            }

            public Task SetParametersAsync(ParameterView parameters)
            {
                _renderHandle.Render(Fragment);
                return Task.CompletedTask;
            }
        }
    }
}
