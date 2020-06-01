using BlazorLibCompToHTML;
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
        

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {

            base.BuildRenderTree(BuildTree(builder));

        }


        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                GetAsHtml();
            }

            base.OnAfterRender(firstRender);

        }

        public string GetAsHtml()
        {
            string a = BCompToHTML.CovertToHtmlString(new RenderFragment(b =>
            {

                BuildTree(b);

            }));

            Console.WriteLine("Generated HTML:");
            Console.WriteLine(a);

            return a;
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

            return builder;

        }

        private void OnClick(MouseEventArgs e)
        {
            Console.WriteLine("item clicked");
        }

       
    }
}
