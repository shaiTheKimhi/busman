using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System.Device;
using System.Media;
using WMPLib;
using System.Threading;
namespace BUSMAN
{
    public partial class Form1 : Form
    {
        
        //System.Media.SoundPlayer player = new System.Media.SoundPlayer();
        
        int coutt = 0;
        int counter = 0, calls = 0;
        public Form1()
        {
            InitializeComponent();
        }
        public HtmlAgilityPack.HtmlNode[] fill(HtmlAgilityPack.HtmlDocument hd, string className)
        {
            IEnumerable<HtmlAgilityPack.HtmlNode> h = from node in hd.DocumentNode.Descendants() where node.Attributes.Contains("class") && node.Attributes["class"].Value == className select node; //== "ListTableCell" select node;
            if (h != null && h.Count() >= 1)
            {
                return h.ToArray();
            }
            return null;
        }

        public HtmlAgilityPack.HtmlNode[] fillNode(HtmlAgilityPack.HtmlNode hd, string className)
        {
            IEnumerable<HtmlAgilityPack.HtmlNode> h = from node in hd.Descendants() where node.Attributes.Contains("class") && node.Attributes["class"].Value == className select node; //== "ListTableCell" select node;
            if (h != null && h.Count() >= 1)
            {
                return h.ToArray();
            }
            return null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox14.Text = Properties.Settings.Default.CoordY.ToString();

            textBox15.Text = Properties.Settings.Default.CoordX.ToString();
            timer1.Enabled= true;
            //resizables = new List<Resizable>();
            for (int i = 0; i < Controls.Count; i++)
            {
                if (Controls[i] is Label)
                {
                   // resizables.Add(new ResizableText((Label)Controls[i], this, Controls[i].Top, Controls[i].Left, Controls[i].Height, Controls[i].Width));
                }
                else
                {
                   // resizables.Add(new Resizable(Controls[i], this, Controls[i].Top, Controls[i].Left, Controls[i].Height, Controls[i].Width));
                }
            }
            label5.Visible = false;
            label6.Visible = false;button1.Visible = false;
            textBox15.Visible = false;
            textBox14.Visible = false;
            textBox13.Visible = false;
            button2.Visible = false;
            WindowState = FormWindowState.Maximized;
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            timer1_Tick(null, null);
        }
        public string RemoveTRN(string s)
        {
            return s.Replace("\t", "").Replace("\r", "").Replace("\n", "");
        }

        public string HTTP_GET(string url)
        {
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Credentials = CredentialCache.DefaultCredentials;
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return responseFromServer;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private void onLoad(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            float f = 0;
            calls++;
            //f = float.Parse(textBox14.Text);
            //f = float.Parse(textBox13.Text);
            try
            {

                textBox1.Text = GetBus(float.Parse(textBox15.Text), float.Parse(textBox14.Text));
                /*  st = textBox1.Text;
                  .Show(st);*/
                //    textBox1.Text = System.IO.File.ReadAllText(@"C: \Users\User\Documents\Visual Studio 2015\Projects\PythonApplication1\PythonApplication1\htmlRes.txt");
                //textBox1.Text = (@"http://www.bus.co.il/otobusimmvc/he/nearonlinetimes?CurrentX=35.1127841&CurrentY=32.8113832");

                HtmlAgilityPack.HtmlDocument hd = new HtmlAgilityPack.HtmlDocument();
                hd.LoadHtml(textBox1.Text);
                HtmlAgilityPack.HtmlNode[] h = fill(hd, "ListTable");
                /*for (int i=0;i<h.Length;i++)
                {
                    listBox1.Items.Add(h[i].InnerHtml);
                }*/
                if (h != null) //TABLE
                {
                    string[] arr1 = new string[4];
                    string[] arr2 = new string[4];
                    string[] arr3 = new string[4];
                    HtmlAgilityPack.HtmlNode[] h2 = fillNode(h[0], "ListTableRow");
                    textBox1.Text = h2[0].InnerHtml;
                    HtmlAgilityPack.HtmlNode[] h3 = fillNode(h2[0], "ListTableCell");
                    for (int i = 0; i < 4; i++)
                    {
                        if (i == 0)
                        {
                            IEnumerable<HtmlAgilityPack.HtmlNode> h4 = h3[i].Descendants("b");
                            if (h4.Count() > 0)
                            {

                                //    listBox1.Items.Add(h4.ToArray()[0].InnerHtml);
                                arr1[i] = h4.ToArray()[0].InnerText;
                            }
                        }
                        else
                        {
                            //  listBox1.Items.Add(RemoveTRN(h3[i].InnerHtml));
                            arr1[i] = RemoveTRN(h3[i].InnerHtml);
                        }
                    }
                    try
                    {
                        h3 = fillNode(h2[1], "ListTableCell");
                        for (int i = 0; i < 4; i++)
                        {
                            if (i == 0)
                            {
                                IEnumerable<HtmlAgilityPack.HtmlNode> h4 = h3[i].Descendants("b");
                                if (h4.Count() > 0)
                                {
                                    arr2[i] = h4.ToArray()[0].InnerText;
                                }
                                //               listBox2.Items.Add(h4.ToArray()[0].InnerHtml);
                            }
                            else
                            {
                                arr2[i] = RemoveTRN(h3[i].InnerHtml);
                            }
                            //           listBox2.Items.Add(RemoveTRN(h3[i].InnerHtml));
                        }
                    }
                    catch { }
                    try
                    {
                        h3 = fillNode(h2[2], "ListTableCell");
                        for (int i = 0; i < 4; i++)
                        {
                            if (i == 0)
                            {
                                IEnumerable<HtmlAgilityPack.HtmlNode> h4 = h3[i].Descendants("b");
                                if (h4.Count() > 0)
                                {
                                    arr3[i] = h4.ToArray()[0].InnerText;
                                }
                                //               listBox2.Items.Add(h4.ToArray()[0].InnerHtml);
                            }
                            else
                            {
                                arr3[i] = RemoveTRN(h3[i].InnerHtml);
                            }
                        }
                        //           listBox2.Items.Add(RemoveTRN(h3[i].InnerHtml));

                    }
                    catch { }



                    textBox1.Text = arr3[0];
                    // System.Threading.Thread.Sleep(1000);
                    //textBox1.Text = "shai";
                    if (arr1[0] == null) return;
                    textBox1.Text = arr1[0];
                    textBox2.Text = arr1[1];
                    textBox3.Text = arr1[2];
                    textBox4.Text = arr1[3];
                    if (textBox2.Text.Contains("אגד"))
                    {
                        textBox1.ForeColor = Color.Green;
                        textBox2.ForeColor = Color.Green;
                        textBox3.ForeColor = Color.Green;
                        textBox4.ForeColor = Color.Green;
                    }
                    else if (arr1[1].Contains("דן"))
                    {
                        textBox1.ForeColor = Color.Blue;
                        textBox2.ForeColor = Color.Blue;
                        textBox3.ForeColor = Color.Blue;
                        textBox4.ForeColor = Color.Blue;
                    }
                    else if (arr1[1].Contains("אקספרס"))
                    {
                        textBox1.ForeColor = Color.Orange;
                        textBox2.ForeColor = Color.Orange;
                        textBox3.ForeColor = Color.Orange;
                        textBox4.ForeColor = Color.Orange;
                    }
                    else if (arr1[1].Contains("טורס"))
                    {
                        textBox1.ForeColor = Color.Gray;
                        textBox2.ForeColor = Color.Gray;
                        textBox3.ForeColor = Color.Gray;
                        textBox4.ForeColor = Color.Gray;
                    }
                    else if (arr1.Contains("סופרבוס"))
                    {
                        textBox1.ForeColor = Color.Green;
                        textBox2.ForeColor = Color.Blue;
                        textBox3.ForeColor = Color.Green;
                        textBox4.ForeColor = Color.Blue;
                    }
                else
                {
                    textBox1.ForeColor = Color.White;
                    textBox2.ForeColor = Color.White;
                    textBox3.ForeColor = Color.White;
                    textBox4.ForeColor = Color.White;
                }
                    if (arr2[0] == null) return;
                    textBox5.Text = arr2[0];
                    textBox6.Text = arr2[1];
                    textBox7.Text = arr2[2];
                    textBox8.Text = arr2[3];

                    if (arr2[1].Contains("אגד"))
                    {
                        textBox5.ForeColor = Color.Green;
                        textBox6.ForeColor = Color.Green;
                        textBox7.ForeColor = Color.Green;
                        textBox8.ForeColor = Color.Green;
                    }
                    else if (arr2[1].Contains("דן"))
                    {
                        textBox5.ForeColor = Color.Blue;
                        textBox6.ForeColor = Color.Blue;
                        textBox7.ForeColor = Color.Blue;
                        textBox8.ForeColor = Color.Blue;
                    }
                    else if (arr2[1].Contains("אקספרס"))
                    {
                        textBox5.ForeColor = Color.Orange;
                        textBox6.ForeColor = Color.Orange;
                        textBox7.ForeColor = Color.Orange;
                        textBox8.ForeColor = Color.Orange;
                    }
                    else if (arr2[1].Contains("טורס"))
                    {
                        textBox5.ForeColor = Color.Gray;
                        textBox6.ForeColor = Color.Gray;
                        textBox7.ForeColor = Color.Gray;
                        textBox8.ForeColor = Color.Gray;
                    }
                    else if (arr2.Contains("סופרבוס"))
                    {
                        textBox5.ForeColor = Color.Green;
                        textBox6.ForeColor = Color.Blue;
                        textBox7.ForeColor = Color.Green;
                        textBox8.ForeColor = Color.Blue;
                    }
                    else
                    {
                        textBox5.ForeColor = Color.White;
                        textBox6.ForeColor = Color.White;
                        textBox7.ForeColor = Color.White;
                        textBox8.ForeColor = Color.White;
                    }
                    if (arr3[0] == null) return;
                    textBox9.Text = arr3[0];
                    textBox10.Text = arr3[1];
                    textBox11.Text = arr3[2];
                    textBox12.Text = arr3[3];
                    if (arr3[1].Contains("אגד"))
                    {
                        textBox9.ForeColor = Color.Green;
                        textBox10.ForeColor = Color.Green;
                        textBox11.ForeColor = Color.Green;
                        textBox12.ForeColor = Color.Green;
                    }
                    else if (arr3[1].Contains("דן"))
                    {
                        textBox9.ForeColor = Color.Blue;
                        textBox10.ForeColor = Color.Blue;
                        textBox11.ForeColor = Color.Blue;
                        textBox12.ForeColor = Color.Blue;
                        //    textBox2.BackColor = Color.Green;

                    }
                    else if (arr3[1].Contains("אקספרס"))
                    {
                        textBox9.ForeColor = Color.Orange;
                        textBox10.ForeColor = Color.Orange;
                        textBox11.ForeColor = Color.Orange;
                        textBox12.ForeColor = Color.Orange;
                    }
                    else if (arr3[1].Contains("טורס"))
                    {
                        textBox9.ForeColor = Color.Gray;
                        textBox10.ForeColor = Color.Gray;
                        textBox11.ForeColor = Color.Gray;
                        textBox12.ForeColor = Color.Gray;
                    }
                    else if (arr3.Contains("סופרבוס"))
                    {
                        textBox9.ForeColor = Color.Green;
                        textBox10.ForeColor = Color.Blue;
                        textBox11.ForeColor = Color.Green;
                        textBox12.ForeColor = Color.Blue;
                    }
                    else
                    {
                        textBox9.ForeColor = Color.White;
                        textBox10.ForeColor = Color.White;
                        textBox11.ForeColor = Color.White;
                        textBox12.ForeColor = Color.White;
                    }
                    /* if (true)
                     {
                         .Show("");
                         //linenum(int.Parse(arr1[0]));
                         InTime(arr1[3]);
                     }*/
                    try
                    {
                        // MessageBox.Show(arr2[0]);
                        
                        Thread t = new Thread(()=>
                        {
                            try
                            {
                                if (calls > 1 && calls % 6 == 0)
                                {
                                    //    .Show("");
                                    int lnm = int.Parse(arr1[0]);
                                    linenum(lnm);
                                    InTime(arr1[3]);
                                }
                                if (calls > 1 && (calls) % 6 == 2)
                                {
                                    linenum(int.Parse(arr2[0]));
                                    InTime(arr2[3]);
                                }
                                if (calls > 1 && (calls) % 6 == 4)
                                {
                                    linenum(int.Parse(arr3[0]));
                                    InTime(arr3[3]);
                                }
                            }
                            catch
                            {

                            }
                        }
                        );
                        t.Start();
                    }
                    catch (Exception e8)
                    {
                        //MessageBox.Show("Error: " + arr2[0]);
                    }

                }
            }

            catch (Exception ex)
            {
                counter++;
            //    .Show(ex.ToString());
            }
        }

        private void DBLCLK(object sender, EventArgs e)
        {
            bool update = !textBox14.Visible;
            textBox14.Visible = update;
            textBox15.Visible = update;
            button1.Visible = update;
            button1.Enabled = update;
            label5.Visible = update;
            label6.Visible = update;
            ///new part
            textBox13.Visible = update;
            button2.Visible = update;
           
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        public string GetBus(float x,float y)
        {
            // int cou = 0;
             string url = "http://www.bus.co.il/otobusimmvc/he/NearOnlineTimes?CurrentX=" + x.ToString() + "&CurrentY=" + y.ToString() + "&Device=Default";
            //  string st = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(resStream);
            //    st = reader.ReadToEnd().ToString();
            return reader.ReadToEnd();
            // return st;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox14.Text!=null&&textBox15.Text!=null)
            {
                Properties.Settings.Default.CoordX = textBox15.Text;
                Properties.Settings.Default.CoordY = textBox14.Text;
                Properties.Settings.Default.Save();
            }
            else
            {
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
          //  .Show(counter.ToString());
            if(textBox14.Visible==false)
            {
                textBox14.Visible = true;
                textBox15.Visible = true;
                button1.Visible = true;
                button1.Enabled = true;
                label5.Visible = true;
                label6.Visible = true;
            }
            else
            {
                textBox14.Visible = false;
                textBox15.Visible = false;
                button1.Visible = false;
                button1.Enabled = false;
                label5.Visible = false;
                label6.Visible = false;
            }
            
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < resizables.Count; i++)
            {
                resizables[i].Resize();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox11_Click(object sender, EventArgs e)
        {

        }

        private void textBox12_Click(object sender, EventArgs e)
        {

        }

        static void linenum(int num)
        {
          //  .Show(num.ToString());
            System.Media.SoundPlayer pl = new System.Media.SoundPlayer();
            pl.SoundLocation = "קו.wav";
            pl.PlaySync();
            //string st = "";
            int n = 0;
            bool and = false;
            bool tens = false;
            n = num / 100;
            switch (n)
            {
                case 1:
                    pl.SoundLocation = "מאה.wav";//meah
                    pl.PlaySync();
                    and = true;
                    break;
        /*        case 2:
                   pl.SoundLocation = "sound.wav";//200
                    pl.PlaySync();
                    and = true;
                    break;
                case 3:
                   pl.SoundLocation = "sound.wav";//300
                    pl.PlaySync();
                    and = true;
                    break;
                case 4:
                   pl.SoundLocation = "sound.wav";//400
                    pl.PlaySync();
                    and = true;
                    break;*/
                /*case 5:
                      st += " חמישים";
                      break;
                  case 6:
                      st += " שישים";
                      break;
                  case 7:
                      st += " שבעים";
                      break;
                  case 8:
                      st += " שמונים";
                      break;
                  case 9:
                      st += " תשעים";
                      break;*/
                default:
                    break;
            }
            num = num % 100;
            n = num / 10;
            switch (n)
            {
                case 1:
               //     .Show("");
                    tens = true;
                    break;
                case 2:
                   pl.SoundLocation = "עשרים.wav";
                    pl.PlaySync();//20
                    and = true;
                    break;
                case 3:
                   pl.SoundLocation = "שלושים.wav";
                    pl.PlaySync();//30
                    and = true;
                    break;
                case 4:
                   pl.SoundLocation = "ארבעים.wav";
                    pl.PlaySync();//40
                    and = true;
                    break;
                case 5:
                   pl.SoundLocation = "חמישים.wav";
                    pl.PlaySync();//50
                    and = true;
                    break;
                case 6:
                   pl.SoundLocation = "שישים.wav";
                    pl.PlaySync();//60
                    and = true;
                    break;
                case 7:
                   pl.SoundLocation = "שבעים.wav";
                    pl.PlaySync();//70
                    and = true;
                    break;
                case 8:
                   pl.SoundLocation = "שמונים.wav";
                    pl.PlaySync();//80
                    and = true;
                    break;
                case 9:
                   pl.SoundLocation = "תשעים.wav";
                    pl.PlaySync();//90
                    and = true;
                    break;
                default:
                    break;
            }
            if(and)
            {
                pl.SoundLocation = "ו.wav";
                //pl.PlaySync();//veeeeee!!!!
            }
           // .Show((num % 10).ToString());
            n = num % 10;
            switch (n)
            {
                case 1:
                    pl.PlaySync();//veeeeee!!!!
                    pl.SoundLocation = tens ? "אחת עשרה.wav" : "אחת.wav";//11 and 1
                    pl.PlaySync();
                    break;
                case 2:
                    pl.PlaySync();//veeeeee!!!!
                    pl.SoundLocation = tens ? "שתיים עשרה.wav" : "שתיים.wav";//11 and 1
                    pl.PlaySync();//2
                    break;
                case 3:
                    // .Show("");
                    pl.PlaySync();//veeeeee!!!!
                    pl.SoundLocation = tens ? "שלוש עשרה.wav" : "שלוש.wav";//11 and 1
                    pl.PlaySync();//3
                    break;
                case 4:
                    pl.PlaySync();//veeeeee!!!!
                    pl.SoundLocation = tens ? "ארבע עשרה.wav" : "ארבע.wav";//11 and 1
                    pl.PlaySync();//4
                    break;
                case 5:
                    pl.PlaySync();//veeeeee!!!!
                    pl.SoundLocation = tens ? "חמש עשרה.wav" : "חמש.wav";//11 and 1
                    pl.PlaySync();//5
                    break;
                case 6:
                    pl.PlaySync();//veeeeee!!!!
                    pl.SoundLocation = tens ? "שש עשרה.wav" : "שש.wav";//11 and 1
                    pl.PlaySync();//6
                    break;
                case 7:
                    pl.PlaySync();//veeeeee!!!!
                    pl.SoundLocation = tens ? "שבע עשרה.wav" : "שבע.wav";//11 and 1
                    pl.PlaySync();//7
                    break;
                case 8:
                    pl.PlaySync();//veeeeee!!!!
                    pl.SoundLocation = tens ? "שמונה עשרה.wav" : "שמונה.wav";//11 and 1
                    pl.PlaySync();//8
                    break;
                case 9:
                    pl.PlaySync();//veeeeee!!!!
                    pl.SoundLocation = tens ? "תשע עשרה.wav" : "תשע.wav";//11 and 1
                    pl.PlaySync();//9
                    break;
                default:
                    break;
            }

        }
        static void InTime(string t)
        {
           // .Show("");
            List<string> l = t.Split(':').ToList<string>();
            int minutes = int.Parse(l[0]) * 60 + int.Parse(l[1]);
            int timeByM = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
            minutes = minutes - timeByM;
            if(minutes<=100)
            {
          //      .Show(minutes.ToString());
                callTime(minutes);
            }

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            string[] lonLat = textBox13.Text.Split(",".ToCharArray());
            textBox14.Text = lonLat[0];
            textBox15.Text = lonLat[1];
        }

        static void callTime(int t)
        {
        //    .Show("got here11111111");
            System.Media.SoundPlayer pl = new System.Media.SoundPlayer();
            int count = 0;
            bool tens = false,and=false,much=false;
            if(t==0)
            {
                pl.SoundLocation = "עכשיו";
            }
            else
            {
                pl.SoundLocation = "בעוד.wav";
            }
            pl.PlaySync();
            switch (t/10)
            {
                case 1:
              //      .Show("got here");5
             //       pl.SoundLocation = "עשר.wav";
              //      .Show("got here");
               //     pl.PlaySync();
                    tens = true;
                    break;
                case 2:
                    // .Show("got here");
                    pl.SoundLocation ="עשרים.wav";//11 and 1
                    pl.PlaySync();//2
                    much = true;
                    and = true;
                    break;
                case 3:
                    //  .Show("got here");
                    pl.SoundLocation ="שלושים.wav";//11 and 1
                    pl.PlaySync();//3
                    much = true;
                    and = true;
                    break;
                case 4:
                    //    .Show("got here");
                    pl.SoundLocation = "ארבעים.wav";//11 and 1
                    pl.PlaySync();//4
                    much = true;
                    and = true;
                    break;
                case 5:
                    //  .Show("got here");
                    pl.SoundLocation = "חמישים.wav";//11 and 1
                    pl.PlaySync();//5
                    much = true;
                    and = true;
                    break;
                case 6:
                    // .Show("got here");
                    pl.SoundLocation = "שישים.wav";//11 and 1
                    pl.PlaySync();//6
                    much = true;
                    and = true;
                    break;
                case 7:
                    pl.SoundLocation ="שבעים.wav";//11 and 1
                    pl.PlaySync();//7
                    and = true;
                    much = true;
                    break;
                case 8:
                    pl.SoundLocation = "שמונים.wav";//11 and 1
                    pl.PlaySync();//8
                    much = true;
                    and = true;
                    break;
                case 9:
                    pl.SoundLocation = "תשעים.wav";//11 and 1
                    pl.PlaySync();//9
                    much = true;
                    and = true;
                    break;
                default:
                    break;
            }
            if (and)
            {
                pl.SoundLocation = "ו.wav";

            }
            else
                pl.SoundLocation = "_.wav";
            switch (t % 10)
            {
                case 1:
                    pl.PlaySync();
                    //   .Show("got here");
                    if (!and && !tens)
                    {
                        pl.SoundLocation = "דקה.wav";//daka
                        pl.PlaySync();
                    }
                    pl.SoundLocation = tens ? "אחת עשרה.wav" : "אחת.wav";//11 and 1
                    pl.PlaySync();
                    break;
                case 2:
                    pl.PlaySync();
                    // .Show("got here");
                    pl.SoundLocation = tens ? "שתיים עשרה.wav" :and? "שתיים.wav":"שתי.wav";//11 and 1
                    pl.PlaySync();//2
                    much = true;
                    break;
                case 3:
                    pl.PlaySync();
                    //  .Show("got here");
                    pl.SoundLocation = tens ? "שלוש עשרה.wav" : "שלוש.wav";//11 and 1
                    pl.PlaySync();//3
                    much = true;
                    break;
                case 4:
                    pl.PlaySync();
                    //    .Show("got here");
                    pl.SoundLocation = tens ? "ארבע עשרה.wav" : "ארבע.wav";//11 and 1
                    pl.PlaySync();//4
                    much = true;
                    break;
                case 5:
                    pl.PlaySync();
                    //  .Show("got here");
                    pl.SoundLocation = tens ? "חמש עשרה.wav" : "חמש.wav";//11 and 1
                    pl.PlaySync();//5
                    much = true;
                    break;
                case 6:
                    pl.PlaySync();
                    // .Show("got here");
                    pl.SoundLocation = tens ? "שש עשרה.wav" : "שש.wav";//11 and 1
                    pl.PlaySync();//6
                    much = true;
                    break;
                case 7:
                    pl.PlaySync();
                    pl.SoundLocation = tens ? "שבע עשרה.wav" : "שבע.wav";//11 and 1
                    pl.PlaySync();//7
                    much = true;
                    break;
                case 8:
                    pl.PlaySync();
                    pl.SoundLocation = tens ? "שמונה עשרה.wav" : "שמונה.wav";//11 and 1
                    pl.PlaySync();//8
                    much = true;
                    break;
                case 9:
                    pl.PlaySync();
                    pl.SoundLocation = tens ? "תשע עשרה.wav" : "תשע.wav";//11 and 1
                    pl.PlaySync();//9
                    much = true;
                    break;
                default:
                    if(tens)
                    {
                        pl.SoundLocation = "עשר.wav";
                        pl.PlaySync();
                    }
                    count++;
                    break;
            }
            if(and||tens||much)
            {
                pl.SoundLocation = "דקות.wav";
                pl.PlaySync();
            }


        }
    }

   
    
}