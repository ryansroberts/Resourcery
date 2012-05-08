using System.IO;
using System.Linq;

namespace Resourcery.Serialisation
{

	/*
    public class NewtonSoftSerialiser : ResourceryJsonAdapter
    {
        protected Model.Resource resource;

        public NewtonSoftSerialiser(Model.Resource resource)
        {
            this.resource = resource;
        }

        public void Write(Stream ostr)
        {
            var writer = new JsonTextWriter(
                new StreamWriter(ostr));

            WriteResource(writer, resource);


            writer.Flush();
        }

        void WriteResource(JsonTextWriter writer, Model.Resource current)
        {
            writer.WriteStartObject();
            WriteLinks(writer, current);
            WriteEmbedded(writer, current);
            WriteForms(writer,current);
            WriteAttributes(writer, current);
            writer.WriteEndObject();
        }

        void WriteForms(JsonTextWriter writer, Model.Resource current)
        {
            if (!current.Forms().Any()) return;

            writer.WritePropertyName("_forms");
            writer.WriteStartObject();

            foreach (var form in current.Forms())
            {
                writer.WritePropertyName(form.Rel());
                WriteResource(writer, form);
            }

            writer.WriteEndObject();
        }

        void WriteEmbedded(JsonTextWriter writer, Model.Resource current)
        {
            if (!current.Embedded().Any()) return;

            writer.WritePropertyName("_embedded");
            writer.WriteStartObject();


            foreach (var group in current.Embedded().GroupBy(e => e.Rel()))
            {

                writer.WritePropertyName(group.Key);

                if (group.Count() == 1)
                {
                    WriteResource(writer, group.First());
                }
                else
                {
                    writer.WriteStartArray();
                    foreach (var res in group)
                        WriteResource(writer, res);
                    writer.WriteEndArray();
                }
            }

            writer.WriteEndObject();
        }

        void WriteAttributes(JsonTextWriter writer, Model.Resource current)
        {
            if (!current.Attributes().Properties().Any()) return;

            foreach (var property in current.Attributes().Properties())
            {
                writer.WritePropertyName(property.Name);
                writer.WriteRawValue(JsonConvert.SerializeObject(property.Value));
            }
        }

        void WriteLinks(JsonWriter writer, Model.Resource current)
        {
            if (!current.Links().Any()) return;
            writer.WritePropertyName("_links");
            writer.WriteStartObject();
            foreach (var link in current.Links())
            {
            
                writer.WritePropertyName(link.Rel);
                writer.WriteStartObject();

                writer.WritePropertyName("href");
                writer.WriteValue(link.Href);

                writer.WriteEndObject();

            }
            writer.WriteEndObject();
        }
    }
	 */
}