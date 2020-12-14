using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication3
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:MyWebControl runat=server></{0}:MyWebControl>")]
    [ParseChildren(false)]
    public class MyWebControl : WebControl
    {
        public MyWebControl() : base("a")
        {
        }
        public string Href
        {
            get
            {
                String href = (String)ViewState["Href"];
                return ((href == null) ? String.Empty : href);
            }
            set
            {
                ViewState["Href"] = value;
            }
        }
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            Attributes["href"] = Href;
            base.RenderBeginTag(writer);
        }
        protected override void RenderContents(HtmlTextWriter output)
        {
            if (base.HasControls())
            {
                base.RenderContents(output);
                return;
            }
            output.Write(Href);
        }
    }
}
