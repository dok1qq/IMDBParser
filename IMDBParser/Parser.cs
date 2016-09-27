using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace IMDBParser
{
    class Parser: Form
    {
        private Button filmInfo;
        private Button searchInfo;
        private PictureBox filmImage;
        private TextBox filmTextID;
        private TextBox searchTextInfo;
        private Label filmLabel;
        private DataGridView dataGridView1;
        private Label searchLabel;
        private DataGridView recieceInfo;
        private DataTable table;

        private string title = "//*[@itemprop='name']";
        private string rating = "//*[@itemprop='ratingValue']";
        private string description = "//*[@itemprop='description']";
        private string creator = "//*[@itemprop='creator']/a/span";
        private string fImage = "//*[@class='poster']/a";
        private string nImage = "//*[@class='image']/a";

        public Parser()
        {
            this.Text = "IMDBParser";
            this.StartPosition  = FormStartPosition.CenterScreen;
            this.Size = new Size(990,360);
            this.BackColor = Color.FromArgb(255,51,51,51);

            filmLabel = new Label();
            filmLabel.Location = new System.Drawing.Point(32, 57);
            filmLabel.Text = "Get information about film whose id is:";
            filmLabel.Size = new System.Drawing.Size(200, 13);

            filmTextID = new TextBox();
            filmTextID.Location = new System.Drawing.Point(32, 80);
            filmTextID.Size = new System.Drawing.Size(159, 20);

            filmInfo = new Button();
            filmInfo.Location = new System.Drawing.Point(200, 80);
            filmInfo.Size = new System.Drawing.Size(85, 20);
            filmInfo.Text = "Get";
            filmInfo.Click += new System.EventHandler(GetInfoButtonEventHandler);

            searchLabel = new Label();
            searchLabel.Location = new System.Drawing.Point(32, 138);
            searchLabel.Text = "Search in IMDB:";
            searchLabel.Size = new System.Drawing.Size(200, 13);

            searchTextInfo = new TextBox();
            searchTextInfo.Location = new System.Drawing.Point(32, 161);
            searchTextInfo.Size = new System.Drawing.Size(159, 20);

            searchInfo = new Button();
            searchInfo.Location = new System.Drawing.Point(200, 161);
            searchInfo.Size = new System.Drawing.Size(85, 23);
            searchInfo.Text = "Search";
            searchInfo.Click += new System.EventHandler(SearchInfoButtonEventHandler);

            recieceInfo = new DataGridView();
            recieceInfo.Location = new System.Drawing.Point(575, 27);
            recieceInfo.Size = new System.Drawing.Size(370, 268);
            recieceInfo.Name = "SomeName";

            filmImage = new PictureBox();
            filmImage.Location = new System.Drawing.Point(360, 27);
            filmImage.Size = new System.Drawing.Size(182, 268);
            filmImage.BackColor = Color.DimGray;

            table = new DataTable("Film info");
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Property", typeof(string));

            this.Controls.Add(filmLabel);
            this.Controls.Add(searchLabel);
            this.Controls.Add(filmTextID);
            this.Controls.Add(searchTextInfo);
            this.Controls.Add(filmInfo);
            this.Controls.Add(searchInfo);
            this.Controls.Add(filmImage);
            this.Controls.Add(recieceInfo);
        }

        static void Main()
        {
            Application.Run(new Parser());
        }

        // Метод-обработчик событий
        void GetInfoButtonEventHandler(object sender, EventArgs e)
        {
            if (filmTextID.Text != "")
            {
                showInformationOfFilm("title", filmTextID.Text);
            }

        }

        // Метод-обработчик событий
        void SearchInfoButtonEventHandler(object sender, EventArgs e)
        {
            HtmlWeb web = new HtmlWeb();
            
            HtmlAgilityPack.HtmlDocument doc = web.Load("http://www.imdb.com/find?ref_=nv_sr_fn&q=" + searchTextInfo.Text + "&s=all");
            if (doc != null)
            {
                HtmlNode cell = doc.DocumentNode.SelectSingleNode("//*[@class='findList']").SelectSingleNode("tr").SelectSingleNode("th|td");
                string text = cell.InnerHtml;
                string[] substrings = text.Split('/');

                showInformationOfFilm(substrings[1], substrings[2]);
            }
        }

        void showInformationOfFilm(string property, string id)
        {
            table.Clear();
            filmImage.Hide();
            HtmlWeb webId = new HtmlWeb();

            HtmlAgilityPack.HtmlDocument doc = webId.Load("http://www.imdb.com/" + property + "/" + id);

            if (doc != null)
            {
                HtmlNode recieveTitle = doc.DocumentNode.SelectSingleNode(title);
                if (recieveTitle != null)
                {
                    table.Rows.Add("Title", recieveTitle.InnerText);
                }

                HtmlNode recieveRating = doc.DocumentNode.SelectSingleNode(rating);
                if (recieveRating != null)
                {
                    table.Rows.Add("Rating", recieveRating.InnerText);
                }

                HtmlNode recieveDescription = doc.DocumentNode.SelectSingleNode(description);
                if (recieveDescription != null)
                {
                    table.Rows.Add("Desctiption", recieveDescription.InnerText);
                }

                HtmlNode recieveCreator = doc.DocumentNode.SelectSingleNode(creator);
                if (recieveCreator != null)
                {
                    table.Rows.Add("Creator", recieveCreator.InnerHtml);
                }

                if (property.Equals("title"))
                {
                    HtmlNode recieveImage = doc.DocumentNode.SelectSingleNode(fImage);
                    if (recieveImage != null)
                    {
                        string sImg = recieveImage.InnerHtml;
                        string[] substrings = sImg.Split('"');
                        filmImage.Load(substrings[5]);
                        filmImage.Show();
                    }
                }

                if (property.Equals("name"))
                {
                    HtmlNode recieveImage = doc.DocumentNode.SelectSingleNode(nImage);
                    if (recieveImage != null)
                    {
                        string sImg = recieveImage.InnerHtml;
                        string[] substrings = sImg.Split('"');
                        filmImage.Load(substrings[11]);
                        filmImage.Show();
                    }
                }     

                recieceInfo.DataSource = table;
            }
        }
    }
}

