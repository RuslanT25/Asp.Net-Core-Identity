using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Identity.Web.TagHelpers
{
    public class UserPictureTagHelper : TagHelper
    {
        public string? Image {  get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";
            if (string.IsNullOrEmpty(Image))
            {
                output.Attributes.SetAttribute("src", "/uploads/userpictures/default.png");
            }
            else
            {
                output.Attributes.SetAttribute("src", $"/uploads/userpictures/{Image}");
            }
        }
    }
}
