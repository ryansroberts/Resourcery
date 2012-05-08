namespace Resourcery.Model
{
    public class Link
    {
        public Link(string href, string rel)
        {
            Href = href;
            Rel = rel;
        }

        public string Href { get; set; }
        public string Rel { get; set; }

    }
}