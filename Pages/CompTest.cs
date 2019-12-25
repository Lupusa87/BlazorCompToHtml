using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace BlazorCompToHtml.Pages
{
    public class CompTest : ComponentBase
    {
        protected readonly Func<string, string> _encoder = (string t) => HtmlEncoder.Default.Encode(t);

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            test();

            base.BuildRenderTree(BuildTree(builder));

        }

        private RenderTreeBuilder BuildTree(RenderTreeBuilder builder)
        {

            int k = 0;
            builder.OpenElement(k++, "div");
            builder.AddAttribute(k++, "id", "div1");

            for (int i = 0; i < 15; i++)
            {
                builder.OpenElement(k++, "p");
                builder.AddAttribute(k++, "id", "p" + i);
                builder.AddAttribute(k++, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, OnClick));
                builder.AddContent(k++, "item_" + i);
                builder.CloseElement();
            }

            builder.CloseElement();



            //ArrayRange<RenderTreeFrame> frames = builder.GetFrames();


            //foreach (var frame in frames)
            //{
            //    Console.WriteLine(frame.ComponentId);
            //    Console.WriteLine(frame.Sequence);
            //    Console.WriteLine(frame.ElementName);
            //    Console.WriteLine(frame.AttributeName);
            //    Console.WriteLine(frame.AttributeValue);
            //    Console.WriteLine(frame.FrameType);
            //}

            return builder;

        }



        private async void test()
        {


            var serviceProvider = new ServiceCollection().AddSingleton(new RenderFragment(builder =>
            {

                BuildTree(builder);

            })).BuildServiceProvider();

            var htmlRenderer = GetHtmlRenderer(serviceProvider);


            IEnumerable<string> result = GetResult(htmlRenderer.Dispatcher.InvokeAsync(() => htmlRenderer.RenderComponentAsync<TestComponent>(ParameterView.Empty)));


            StringBuilder sb = new StringBuilder();

            foreach (var item in result)
            {
                sb.Append(item);
            }

            Console.WriteLine(sb.ToString());

        }

        private void OnClick(MouseEventArgs e)
        {
            Console.WriteLine("ttt");
        }

        private HtmlRenderer GetHtmlRenderer(IServiceProvider serviceProvider)
        {
            return new HtmlRenderer(serviceProvider, NullLoggerFactory.Instance, _encoder);
        }


        private IEnumerable<string> GetResult(Task<ComponentRenderedText> task)
        {

            return task.Result.Tokens;

        }

        private class TestComponent : IComponent
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
