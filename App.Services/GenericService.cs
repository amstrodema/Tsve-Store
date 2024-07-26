namespace App.Services
{
    public class GenericService
    {
        public static string AmountPreProcessor(decimal val)
        {
            return val >= 10000 ? val / 1000 + "k" : val.ToString();
        }
        public static string GetResource(string type, string fileName, string folder)
        {
            if (type == "Img") return ImageService.GetLargeImagePath(fileName, folder);
            else if (type == "Vid") return VideoServices.GetVideoPath(fileName, folder);
            else
            {
                //var val = fileName.Split('.');
                //if (val[val.Count() - 1] == "pdf")
                //{
                //    return "../img/icn-pdf.png";
                //}
                //return "../img/icn-txt2.png";
                return DocumentService.GetDocumentPath(fileName, folder);
            }
        }
        public static string GetTag(string title)
        {
            string tag = "";
            string liner = "abcdefghijklmnopqrstuvwxyz0123456789";
            if (string.IsNullOrEmpty(title)) { return title; }

            foreach (var item in title.Trim())
            {
                string val = item.ToString();

                if (val == " ")
                {
                    tag += "-";
                }
                else if (liner.Contains(val.ToLower()))
                {
                    tag += val.ToLower();
                }
            }

            return tag;
        }
        public static string GetSectionName(string section)
        {
            switch (section.ToLower())
            {
                case "country":
                    {
                        section = "Country";
                        break;
                    }
                case "learning":
                    {
                        section = "Learning";
                        break;
                    }
                case "resources":
                    {
                        section = "Resources";
                        break;
                    }
                case "news":
                    {
                        section = "News";
                        break;
                    }
                case "events":
                    {
                        section = "Events";
                        break;
                    }
                case "project":
                    {
                        section = "Project";
                        break;
                    }
                case "job":
                    {
                        section = "Job";
                        break;
                    }
                case "commerce":
                    {
                        section = "Commerce";
                        break;
                    }
                default:
                    throw new Exception();
            }

            return section;
        }
    }

}