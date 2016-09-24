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

        private string title = "//*[@id='title-overview-widget']/div[2]/div[2]/div/div[2]/div[2]/h1";
        private string rating = "//*[@id='title-overview-widget']/div[2]/div[2]/div/div[1]/div[1]/div[1]/strong/span";
        //string description = "//*[@id='title-overview-widge']/div[3]/div[1]/div[1]";
        private string creator = "//*[@id='title-overview-widget']/div[3]/div[1]/div[2]/span/a/span";
        private string stars = "//*[@id='title-overview-widget']/div[3]/div[1]/div[3]";
        private string fImage = "//*[@id='title-overview-widget']/div[2]/div[4]/div[1]/a/img/@src";

        public Parser()
        {
            this.Text = "IMDBParser";
            this.StartPosition  = FormStartPosition.CenterScreen;
            this.Size = new Size(1000,500);
            this.BackColor = Color.FromArgb(255,51,51,51);

            filmLabel = new Label();
            filmLabel.Location = new System.Drawing.Point(32, 57);
            filmLabel.Text = "Get information about film whose id is:";
            filmLabel.Size = new System.Drawing.Size(200, 13);

            filmTextID = new TextBox();
            filmTextID.Location = new System.Drawing.Point(237, 54);
            filmTextID.Size = new System.Drawing.Size(159, 20);

            filmInfo = new Button();
            filmInfo.Location = new System.Drawing.Point(402, 54);
            filmInfo.Size = new System.Drawing.Size(85, 20);
            filmInfo.Text = "Get";
            filmInfo.Click += new System.EventHandler(GetInfoButtonEventHandler);

            searchLabel = new Label();
            searchLabel.Location = new System.Drawing.Point(32, 138);
            searchLabel.Text = "Search in IMDB:";
            searchLabel.Size = new System.Drawing.Size(200, 13);

            searchTextInfo = new TextBox();
            searchTextInfo.Location = new System.Drawing.Point(237, 135);
            searchTextInfo.Size = new System.Drawing.Size(159, 20);

            searchInfo = new Button();
            searchInfo.Location = new System.Drawing.Point(402, 133);
            searchInfo.Size = new System.Drawing.Size(85, 23);
            searchInfo.Text = "Search";
            searchInfo.Click += new System.EventHandler(SearchInfoButtonEventHandler);

            recieceInfo = new DataGridView();
            recieceInfo.Location = new System.Drawing.Point(575, 27);
            recieceInfo.Size = new System.Drawing.Size(370, 400);
            recieceInfo.Name = "SomeName";

            filmImage = new PictureBox();
            filmImage.Location = new System.Drawing.Point(300, 170);
            filmImage.Size = new System.Drawing.Size(182, 268);
            filmImage.BackColor = Color.Aqua;
            filmImage.Load("https://images-na.ssl-images-amazon.com/images/M/MV5BNzUwMDEyMTIxM15BMl5BanBnXkFtZTgwNDU3OTYyODE@._V1_UX182_CR0,0,182,268_AL_.jpg");
            //filmImage.Hide();

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
                showInformationOfFilm(filmTextID.Text);
            }

        }

        // Метод-обработчик событий
        void SearchInfoButtonEventHandler(object sender, EventArgs e)
        {
            HtmlWeb web = new HtmlWeb();
            
            HtmlAgilityPack.HtmlDocument doc = web.Load("http://www.imdb.com/find?ref_=nv_sr_fn&q=" + searchTextInfo.Text + "&s=all");
            if (doc != null)
            {
                foreach (HtmlNode tab in doc.DocumentNode.SelectNodes("//*[@id='main']/div/div[2]/table"))
                {
                    foreach (HtmlNode row in tab.SelectNodes("tr"))
                    {
                        foreach (HtmlNode cell in row.SelectNodes("th|td"))
                        {
                            string text = cell.InnerHtml;
                            String[] substrings = text.Split('/');

                            showInformationOfFilm(substrings[2]);
                            break;
                        }
                        break;
                    }
                    break;
                }

            }
        }

        void showInformationOfFilm(string url)
        {
            table.Clear();
            filmImage.Hide();
            HtmlWeb webId = new HtmlWeb();

            HtmlAgilityPack.HtmlDocument doc = webId.Load("http://www.imdb.com/title/" + url);

            if (doc != null)
            {
                HtmlNode recieveTitle = doc.DocumentNode.SelectSingleNode(title);
                HtmlNode recieveRating = doc.DocumentNode.SelectSingleNode(rating);
                //HtmlNodeCollection recieveDescription = doc.DocumentNode.SelectNodes(description);
                HtmlNode recieveCreator = doc.DocumentNode.SelectSingleNode(creator);
                HtmlNodeCollection recieveStars = doc.DocumentNode.SelectNodes(stars);
                //HtmlNode recieveImage = doc.DocumentNode.SelectSingleNode(fImage);

                //var col1 = recieveDescription.Select(node => node.InnerText);
                //var recStars = recieveStars.Select(node => node.InnerText);
                //var l = recieveImage["href"].Value;

                

                table.Rows.Add("Title", recieveTitle.InnerHtml);
                table.Rows.Add("Rating", recieveRating.InnerHtml);
                //table.Rows.Add("Description", recieveDescription.InnerHtml);
                table.Rows.Add("Creator", recieveCreator.InnerHtml);
                //table.Rows.Add("Srats", recStars);
                filmImage.Show();

                recieceInfo.DataSource = table;
            }
        }
    }
}

