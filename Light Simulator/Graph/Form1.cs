using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graph
{
    public partial class Form1 : Form
    {
        #region Load
        public Form1()
        {
            //load material data.
            InitializeComponent();
            string[] text = { "1 - אוויר", "מים נוזליים - 1.33", "זכוכית - 1.5(עד 1.9)", "יהלום - 2.42" };
            listBox1.Items.Add("1");
            listBox1.Items.Add("1.33");
            listBox1.Items.Add("1.5");
            listBox1.Items.Add("2.42");
            listBox2.Items.Add("1");
            listBox2.Items.Add("1.33");
            listBox2.Items.Add("1.5");
            listBox2.Items.Add("2.42");
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //Initialize light starting point and graphics foreach panel.
            lightXcord = 5;
            lightYcord = panel1.Height - 5;
            returnLightXcord = lightXcord;
            returnLightYcord = panel2.Height - lightYcord;
            panel1G = panel1.CreateGraphics();
            panel2G = panel2.CreateGraphics();
            panel3G = panel3.CreateGraphics();
            panel4G = panel4.CreateGraphics();
            panel5G = panel5.CreateGraphics();
            panel6G = panel6.CreateGraphics();
            panel7G = panel7.CreateGraphics();
            //Fullscreen
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
        }
        public void ShowControlsSlowly()
        {
            System.Threading.Thread.Sleep(2000);
            foreach(Control ctr in Controls)
            {
                ctr.Visible = true;
                System.Threading.Thread.Sleep(50);
            }
            this.BackgroundImage = null;
        }
        #endregion
        #region Member Variables
        //All of the graphics for the 7 different panels
        public Graphics panel1G;
        public Graphics panel2G;
        public Graphics panel3G;
        public Graphics panel4G;
        public Graphics panel5G;
        public Graphics panel6G;
        public Graphics panel7G;
        public Color penColor = Color.Yellow;
        //Mirror and break related variables.
        public float gradient = 0;
        public float hitXcord = 0;
        public int lightXcord;
        public int lightYcord;
        public int returnLightXcord;
        public int returnLightYcord;
        // 5 Different Program modes.
        public bool lightReturn = true;
        public bool lightBreak = false;
        public bool lightRecurse = false;
        public bool lightConcetrate = false;
        public bool lightSep = false;
        //Light break variables.
        public int nToWhere;
        public int nFromWhere;
        //Lens variables.
        public float objHeight;
        public float imgHeight;
        public float fcsDistance;
        public float objDistance;
        public float imgDistance;
        //Variables that determines whether the next recursive hit is going to be on the right mirror or on the mirror.
        public bool right = true;
        //Angle delta.
        public double jump = 1;
        #endregion
        #region Start 
        //
        //Starts the simulation.
        //
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (lightReturn)
                {
                    double angel = (90 - double.Parse(textBox2.Text)) * (Math.PI / 180);
                    gradient = -1 * float.Parse((Math.Tan(angel)).ToString());
                    textBox1.Text = (gradient * -1).ToString();
                    DrawLine();
                    DrawReflectedLine();
                }
                else if (lightBreak)
                {
                    double angel = (90 - double.Parse(textBox2.Text)) * (Math.PI / 180);
                    gradient = float.Parse((Math.Tan(angel)).ToString());
                    textBox2.Text = double.Parse(Math.Round(double.Parse(textBox2.Text), 3).ToString()).ToString();
                    textBox1.Text = (gradient).ToString();
                    DrawLine();
                    DrawReflectedLine();
                }
                else if (lightRecurse)
                {
                    double angel = (90 - double.Parse(textBox2.Text)) * (Math.PI / 180);
                    gradient = float.Parse((Math.Tan(angel)).ToString());
                    textBox1.Text = (gradient).ToString();
                    if (checkBox1.Checked)
                    {
                        panel5G.Clear(Color.Black);
                    }
                    DrawRecursively(lightXcord, 0);
                }
            }
            catch
            {
                MessageBox.Show("זווית או שיפוע לא חוקיים");
            }
        }
        #endregion
        #region Controls
        //Proccess controls.
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            double num = 0;
            switch (keyData)
            {
                //Controls for moving the light source.
                //2 different mode for moving the light source.
                //Move right.
                case Keys.Right:
                    if (lightXcord + 1 != panel1.Width)
                    {
                        
                        if (lightReturn) 
                        {
                            lightXcord++;
                            returnLightXcord++;
                            DrawLine();
                            DrawReflectedLine();
                            return true;
                        }
                        else if (lightBreak)
                        {
                            lightXcord++;
                            DrawLine();
                            DrawReflectedLine();
                            return true;
                        }
                    }

                    break;
                //Move left.
                case Keys.Left:
                    if (lightXcord - 1 != 0)
                    {
                        if (lightReturn)
                        {
                            lightXcord--;
                            returnLightXcord--;
                            DrawLine();
                            DrawReflectedLine();
                            return true;
                        }
                        else if (lightBreak)
                        {
                            lightXcord--;
                            DrawLine();
                            DrawReflectedLine();
                            return true;
                        }
                    }
                    break;
                    //Move up.
                case Keys.Up:
                    if (lightYcord - 1 != 0)
                    {
                        if (lightReturn)
                        {
                            lightYcord--;
                            returnLightYcord++;
                            DrawLine();
                            DrawReflectedLine();
                            return true;
                        }
                        else if (lightBreak)
                        {
                            lightYcord--;
                            DrawLine();
                            DrawReflectedLine();
                            return true;
                        }
                    }
                    break;
                    //Move down.
                case Keys.Down:
                    if (lightYcord + 1 != panel1.Height)
                    {
                        if (lightReturn)
                        {
                            lightYcord++;
                            returnLightYcord--;
                            DrawLine();
                            DrawReflectedLine();
                            return true;
                        }
                        else if(lightBreak)
                        {
                            lightYcord++;
                            DrawLine();
                            DrawReflectedLine();
                            return true;
                        }
                    }
                    break;
                    //Increase the angle by a value (variable "jump" controls the delta) in degrees.
                case Keys.O:
                    num = double.Parse(textBox1.Text);
                    num += jump;
                    gradient = float.Parse(num.ToString());
                    textBox1.Text = num.ToString();
                    if ((lightBreak) || (lightReturn))
                    {
                        DrawLine();
                        DrawReflectedLine();
                    }
                    else if (lightRecurse)
                    {
                        if (checkBox1.Checked)
                        {
                            panel5G.Clear(Color.Black);
                            DrawRecursively(lightXcord, 0);
                        }
                    }

                    return false;
                //Decrease the angle by a value (variable "jump" controls the delta) in degree.
                case Keys.L:
                    num = double.Parse(textBox1.Text);
                    if (!(num - jump <= 0))
                    {
                        num -= jump;
                        gradient = float.Parse(num.ToString());
                        textBox1.Text = num.ToString();
                        if ((lightBreak) || (lightReturn))
                        {
                            DrawLine();
                            DrawReflectedLine();
                        }
                        else if (lightRecurse)
                        {
                            if (checkBox1.Checked)
                            {
                                panel5G.Clear(Color.Black);
                                DrawRecursively(lightXcord, 0);
                            }
                        }
                    }
                    return false;
            }
            return false;
        }
        #endregion
        #region Graphics calculations and drawing
        //Method for drawing the line that hits the mirror / material. 
        //(build the function with the angle and the starting point).
        //2 Different modes.
        public void DrawLine()
        {
            textBox2.Text= (90 - Math.Abs(Math.Atan(gradient) * 180/Math.PI)).ToString();
            if (checkBox1.Checked)
            {
                panel1G.Clear(Color.Black);
                panel2G.Clear(Color.Black);
                panel3G.Clear(Color.Black);
                panel4G.Clear(Color.Black);
            }
            Brush b = Brushes.Yellow;
            Pen p = new Pen(b);
            p.Color = penColor;
            //Light hitting a mirror.
            if (lightReturn)
            {
                panel1G.DrawRectangle(p, lightXcord, lightYcord, 2, 2);
                float yCord = 0;
                hitXcord = (((-gradient * lightXcord) + lightYcord - yCord) / (-gradient));
                panel1G.DrawLine(p, lightXcord, lightYcord, hitXcord, yCord);
                if (checkBox2.Checked)
                {
                    p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    panel1G.DrawLine(p, lightXcord, lightYcord, lightXcord, 0);
                    p.Color = Color.White;
                    panel2G.DrawRectangle(p, returnLightXcord, returnLightYcord, 2, 2);
                    panel2G.DrawLine(p, returnLightXcord, returnLightYcord, hitXcord, panel2.Height);
                    panel2G.DrawLine(p, returnLightXcord, returnLightYcord, returnLightXcord, panel2.Height);
                }
            }
            //Light hitting a transparent object.
            else if (lightBreak)
            {
                panel3G.DrawRectangle(p, lightXcord, lightYcord, 2, 2);
                float yCord = panel3.Height;
                hitXcord = -((lightYcord - yCord) / gradient - lightXcord);
                panel3G.DrawLine(p, lightXcord, lightYcord, hitXcord, yCord);
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                p.Color = Color.White;
                panel3G.DrawLine(p, hitXcord, yCord, hitXcord, yCord - 100);
            }
            
        }

        //Draws a light from predefined starting point and an angle.
        //The light hits the mirros serveral times and therefore it's recursive.
        public void DrawRecursively(float x, float y)
        {
            Brush b = Brushes.Yellow;
            Pen p = new Pen(b);
            p.Color = penColor;
            panel1G.DrawRectangle(p, lightXcord, 0, 2, 2);
            if (right)
            {
                gradient *= -1;
                hitXcord = panel5.Width;
                float yCord = y + gradient*(x - hitXcord);
                panel5G.DrawLine(p, x, y, hitXcord, yCord);
                right = false;
                if ((yCord <= panel5.Height) && (yCord >= 0))
                {
                    DrawRecursively(hitXcord, yCord);
                }
                else
                {
                    right = true;
                }
            }
            else
            {
                gradient *= -1;
                hitXcord = 0;
                float yCord = y + gradient * x;
                panel5G.DrawLine(p, x, y, hitXcord, yCord);
                right = true;
                if ((yCord <= panel5.Height) && (yCord >= 0))
                {
                    DrawRecursively(hitXcord, yCord);
                }
            }
        }
        //Method for drawing the reflected light.
        public void DrawReflectedLine()
        {
            Brush b = Brushes.Yellow;
            Pen p = new Pen(b);
            p.Color = penColor;
            //If the light hits a mirror,it describes f(x) and drawing x,y values of it.
            if (lightReturn)
            {
                float returnX = panel1.Width - hitXcord;
                gradient *= -1;
                for (int i = 0; i < 100; i++)
                {
                    float destY = gradient * (hitXcord + i) - gradient * (hitXcord);
                    panel1G.DrawLine(p, hitXcord, 0, hitXcord + i, destY);
                }
                gradient *= -1;
            }
            //If the light breaks through a transparent object, ii calculates the angle after the breaking,
            //the critical angle (if required) and the new m for f(x).
            else if  ((lightBreak) && (textBox4.Text !="") && (textBox5.Text !=""))
            {
                double n1 = 0;
                double n2 = 0;
                try
                {
                    n1 = double.Parse(textBox4.Text);
                    n2 = double.Parse(textBox5.Text);
                }
                catch
                {
                    MessageBox.Show("מקדמי שבירה לא חוקיים");
                }
                if (n2 >= n1)
                {
                    double critangle = Math.Asin(n1 / n2) * 180 / Math.PI;
                    critangle = Math.Round(critangle, 3);
                    critAngleLbl.Text = "זוית קריטית : " + critangle.ToString();
                    double angle = (double.Parse(textBox2.Text));
                    double returnAngle = (Math.Sin((angle) * Math.PI/180) * n2) / n1;
                    returnAngle = Math.Asin(returnAngle) * 180/Math.PI;
                    returnAngle = Math.Round(returnAngle, 3);
                    textBox6.Text = "זוית שבירה :" +returnAngle;
                    if (angle > critangle)
                    {
                        gradient *= -1;
                        for (int i = 0; i < 100; i++)
                        {
                            float destY = panel3.Height - gradient*(hitXcord - hitXcord-i);
                            panel3G.DrawLine(p, hitXcord, panel3.Height, hitXcord + i, destY);
                        }
                        gradient *= -1;
                    }
                    else if(angle == critangle)
                    {
                        panel4G.DrawLine(p,hitXcord, panel3.Height, panel3.Width, panel3.Height);
                    }
                    else
                    {
                        float g = float.Parse(Math.Tan((Math.PI/2-returnAngle* Math.PI/180)).ToString());
                        float yCord = panel3.Height;
                        float destX = (((panel4.Height) / g) + hitXcord);
                        panel4G.DrawLine(p, hitXcord, 0, destX, yCord);
                        p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        p.Color = Color.White;
                        panel4G.DrawLine(p, hitXcord, 0, hitXcord, 100);
                    }
                }
                else
                {
                    double angle = (double.Parse(textBox2.Text));
                    double returnAngle = (Math.Sin(angle * Math.PI / 180) * n2) / n1;
                    returnAngle = Math.Asin(returnAngle) * 180/Math.PI;
                    returnAngle = Math.Round(returnAngle, 3);
                    textBox6.Text = "זוית שבירה :" + returnAngle;
                    float g = float.Parse(Math.Tan((Math.PI / 2 - returnAngle * Math.PI / 180)).ToString());
                    float yCord = panel3.Height;
                    float destX = (((panel4.Height) / g) + hitXcord);
                    panel4G.DrawLine(p, hitXcord, 0, destX, yCord);
                    p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    p.Color = Color.White;
                    panel4G.DrawLine(p, hitXcord, 0, hitXcord, 100);
                }
            }

        }

        //
        //Environment drawer for the lens. (draws the lens, the arrows, and a middle line).
        //
        public void DrawEnvironment(bool conc)
        {
            Brush b = Brushes.Yellow;
            Pen p = new Pen(b);
            p.Color = Color.White;
            if (conc)
            {
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                panel6G.DrawLine(p, 0, panel6.Height / 2, panel6.Width, panel6.Height / 2);
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                panel6G.DrawLine(p, panel6.Width / 2, panel6.Height / 2 - 250, panel6.Width / 2, panel6.Height / 2 + 250);
                panel6G.DrawLine(p, panel6.Width / 2, panel6.Height / 2 - 250, panel6.Width / 2 - 10, panel6.Height / 2 - 240);
                panel6G.DrawLine(p, panel6.Width / 2, panel6.Height / 2 - 250, panel6.Width / 2 + 10, panel6.Height / 2 - 240);
                panel6G.DrawLine(p, panel6.Width / 2, panel6.Height / 2 + 250, panel6.Width / 2 - 10, panel6.Height / 2 + 240);
                panel6G.DrawLine(p, panel6.Width / 2, panel6.Height / 2 + 250, panel6.Width / 2 + 10, panel6.Height / 2 + 240);
            }
            else
            {
                panel7G.DrawLine(p, 0, panel7.Height / 2, panel7.Width, panel7.Height / 2);
                panel7G.DrawLine(p, panel7.Width / 2, panel7.Height / 2 - 250, panel7.Width / 2, panel7.Height / 2 + 250);
                panel7G.DrawLine(p, panel7.Width / 2, panel7.Height / 2 - 250, panel7.Width / 2 - 10, panel7.Height / 2 - 260);
                panel7G.DrawLine(p, panel7.Width / 2, panel7.Height / 2 - 250, panel7.Width / 2 + 10, panel7.Height / 2 - 260);
                panel7G.DrawLine(p, panel7.Width / 2, panel7.Height / 2 + 250, panel7.Width / 2 - 10, panel7.Height / 2 + 260);
                panel7G.DrawLine(p, panel7.Width / 2, panel7.Height / 2 + 250, panel7.Width / 2 + 10, panel7.Height / 2 + 260);
            }

        }
        //
        //all the crazy lens stuff :)
        //
        private void button4_Click(object sender, EventArgs e)
        {
            Brush b = Brushes.Yellow;
            Font myFont = new Font(FontFamily.GenericMonospace, 9);
            Pen p = new Pen(b);
            p.Color = Color.White;
            int x = panel6.Width / 2;
            int y = panel6.Height / 2;
            objDistance = float.Parse(objDistanceTxt.Text);
            fcsDistance = float.Parse(fcsDistanceTxt.Text);
            double m = objHeight / imgHeight;

            objHeight = 100;
            if (lightConcetrate)
            {
                if (textBox7.Text == "0")
                {
                    if (checkBox1.Checked)
                    {
                        panel6G.Clear(Color.Black);
                    }
                    DrawEnvironment(true);
                    if ((objDistance > fcsDistance))
                    {
                        imgDistance = (objDistance * fcsDistance) / (objDistance - fcsDistance);
                        imgHeight = ((objHeight * fcsDistance) / (objDistance - fcsDistance));
                        imgDistance = float.Parse(Math.Round(double.Parse(imgDistance.ToString()), 2).ToString());
                        imgHeight = float.Parse(Math.Round(double.Parse(imgHeight.ToString()), 2).ToString());
                        imgHeightTxt.Text = imgHeight.ToString();
                        imgDistanceTxt.Text = imgDistance.ToString();
                        objHeightTxt.Text = objHeight.ToString();
                        panel6G.DrawLine(p, x - objDistance, y - objHeight, x - objDistance, y);
                        panel6G.DrawLine(p, x + imgDistance, y, x + imgDistance, y + imgHeight);
                        p.Color = Color.Lime;
                        panel6G.DrawLine(p, x - objDistance, y - objHeight, x + imgDistance, y + imgHeight);
                        p.Color = Color.Red;
                        panel6G.DrawLine(p, x - objDistance, y - objHeight, x, y - objHeight);
                        panel6G.DrawLine(p, x, y - objHeight, x + imgDistance, y + imgHeight);
                        panel6G.DrawString("u", myFont, p.Brush, x - (objDistance / 2), y - objHeight - 20);
                        p.Color = Color.Blue;
                        panel6G.DrawLine(p, x - fcsDistance, y, x, y);
                        panel6G.DrawLine(p, x + fcsDistance, y, x, y);
                        panel6G.DrawString("f", myFont, p.Brush, x + fcsDistance / 2, y + 5);
                        panel6G.DrawString("f", myFont, p.Brush, x - fcsDistance / 2, y + 5);
                        p.Color = Color.Cyan;
                        p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        panel6G.DrawLine(p, x + imgDistance, y + imgHeight, x, y + imgHeight);
                        panel6G.DrawString("v", myFont, p.Brush, x + imgDistance / 2, y + imgHeight + 20);
                    }
                    else if (objDistance == fcsDistance)
                    {
                        imgDistanceTxt.Text = "NaN";
                        imgHeightTxt.Text = "NaN";
                        panel6G.DrawLine(p, x - objDistance, y - objHeight, x - objDistance, y);
                        double _m = (objHeight) / (objDistance);
                        float cordx = float.Parse(((200) / (_m) + x).ToString());
                        p.Color = Color.Lime;
                        panel6G.DrawLine(p, x - objDistance, y - objHeight, cordx, y + 200);
                        p.Color = Color.Red;
                        panel6G.DrawString("u", myFont, p.Brush, x - (objDistance / 2), y - objHeight - 20);
                        panel6G.DrawLine(p, x - objDistance, y - objHeight, x, y - objHeight);
                        _m = (objHeight) / fcsDistance;
                        cordx = float.Parse(((200) / (_m) + x + fcsDistance).ToString());
                        panel6G.DrawLine(p, x, y - objHeight, cordx, y + 200);
                        p.Color = Color.Blue;
                        panel6G.DrawLine(p, x - fcsDistance, y, x, y);
                        panel6G.DrawLine(p, x + fcsDistance, y, x, y);
                        panel6G.DrawString("f", myFont, p.Brush, x + fcsDistance / 2, y + 5);
                        panel6G.DrawString("f", myFont, p.Brush, x - fcsDistance / 2, y + 5);
                    }

                    else
                    {
                        objHeight = 50;
                        imgDistance = (-fcsDistance * objDistance) / (objDistance - fcsDistance);
                        double vu = imgDistance / objDistance;
                        imgHeight = float.Parse((objHeight * vu).ToString());
                        imgHeightTxt.Text = imgHeight.ToString();
                        imgDistanceTxt.Text = imgDistance.ToString();
                        panel6G.DrawLine(p, x - objDistance, y - objHeight, x - objDistance, y);
                        panel6G.DrawLine(p, x - imgDistance, y - imgHeight, x - imgDistance, y);
                        p.Color = Color.Blue;
                        panel6G.DrawLine(p, x - fcsDistance, y, x, y);
                        panel6G.DrawLine(p, x + fcsDistance, y, x, y);
                        panel6G.DrawString("f", myFont, p.Brush, x + fcsDistance / 2, y + 5);
                        panel6G.DrawString("f", myFont, p.Brush, x - fcsDistance / 2, y + 5);
                        p.Color = Color.Green;
                        panel6G.DrawLine(p, x - objDistance, y - objHeight, x, y);
                        double _m = (objHeight) / (objDistance);
                        float cordx = float.Parse(((200) / _m + x).ToString());
                        panel6G.DrawLine(p, x, y, cordx, y + 200);
                        p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        panel6G.DrawLine(p, x - objDistance, y - objHeight, x - imgDistance, y - imgHeight);
                        p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                        p.Color = Color.Red;
                        panel6G.DrawLine(p, x - objDistance, y - objHeight, x, y - objHeight);
                        panel6G.DrawString("u", myFont, p.Brush, x - (objDistance / 2), y - objHeight - 20);
                        _m = (objHeight) / (fcsDistance);
                        cordx = float.Parse(((200) / _m + x + fcsDistance).ToString());
                        panel6G.DrawLine(p, x, y - objHeight, cordx, y + 200);
                        p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        panel6G.DrawLine(p, x, y - objHeight, x - imgDistance, y - imgHeight);
                        p.Color = Color.Cyan;
                        panel6G.DrawLine(p, x - imgDistance, y - imgHeight, x, y - imgHeight);
                        panel6G.DrawString("v", myFont, p.Brush, x - imgDistance / 2, y - imgHeight - 20);
                    }
                }
            }
            else
            {
                if (checkBox1.Checked)
                {
                    panel7G.Clear(Color.Black);
                }
                DrawEnvironment(false);
                imgDistance = (-fcsDistance * objDistance) / (-fcsDistance - objDistance);
                imgHeight = (imgDistance * objHeight) / objDistance;
                imgHeightTxt.Text = imgHeight.ToString();
                imgDistanceTxt.Text = imgDistance.ToString();
                panel7G.DrawLine(p, x - objDistance, y - objHeight, x - objDistance, y);
                panel7G.DrawLine(p, x - imgDistance, y - imgHeight, x - imgDistance, y);
                double _m = (objHeight) / (objDistance);
                float cordx = float.Parse((200 / _m + x).ToString());
                p.Color = Color.Green;
                panel7G.DrawLine(p, x - objDistance, y - objHeight, cordx, y + 200);
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                panel7G.DrawLine(p, x - objDistance, y - objHeight, x - imgDistance, y - imgHeight);
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                p.Color = Color.Red;
                panel7G.DrawLine(p, x - objDistance, y - objHeight, x, y - objHeight);
                panel7G.DrawString("u", myFont, p.Brush, x - (objDistance / 2), y - objHeight - 20);
                _m = (imgHeight) / (imgDistance - fcsDistance);
                cordx = float.Parse((-200 / _m + x - fcsDistance).ToString());
                panel7G.DrawLine(p, x, y - objHeight, cordx, y - 200);
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                panel7G.DrawLine(p, x, y - objHeight, x - fcsDistance, y);
                p.Color = Color.Blue;
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                panel7G.DrawLine(p, x - fcsDistance, y, x, y);
                panel7G.DrawLine(p, x + fcsDistance, y, x, y);
                panel7G.DrawString("f", myFont, p.Brush, x + fcsDistance / 2, y + 5);
                panel7G.DrawString("f", myFont, p.Brush, x - fcsDistance / 2, y + 5);
                p.Color = Color.Cyan;
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                panel7G.DrawLine(p, x - imgDistance, y - imgHeight, x, y - imgHeight);
                panel7G.DrawString("v", myFont, p.Brush, x - imgDistance / 2, y - imgHeight - 20);

            }
        }

        #endregion
        #region Updating Member Variables by user input
        //Events for textchanging, updates variables by user input.
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (lightReturn)
                {
                    gradient = -1 * float.Parse(textBox1.Text);
                    DrawLine();
                    DrawReflectedLine();
                }
                else if (lightBreak)
                {
                    gradient = float.Parse(textBox1.Text);
                    DrawLine();
                    DrawReflectedLine();
                }
            }
            catch { }

        }
        //3 different deltas for the angle increasement / decreasement. (0.001, 0.01,0.1).
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked == true)
            {
                jump = 0.001;
            }
            else if(radioButton2.Checked == true)
            {
                jump = 0.01;
            }
            else if (radioButton3.Checked == true)
            {
                jump = 0.1;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                jump = 0.001;
            }
            else if (radioButton2.Checked == true)
            {
                jump = 0.01;
            }
            else if (radioButton3.Checked == true)
            {
                jump =0.1;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                jump = 0.001;
            }
            else if (radioButton2.Checked == true)
            {
                jump = 0.01;
            }
            else if (radioButton3.Checked == true)
            {
                jump = 0.1;
            }
        }
        //
        //Event for tabchanging
        //
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                groupBox2.Enabled = true;
                groupBox3.Enabled = false;
                groupBox4.Enabled = false;
                lightRecurse = false;
                lightReturn = true;
                lightBreak = false;
                lightConcetrate = false;
                lightSep = false;
                lightXcord = 5;
                lightYcord = panel1.Height - 5;
                panel1G = panel1.CreateGraphics();
                panel2G = panel2.CreateGraphics();
                checkBox1.Enabled = true;
                textBox2.Text = "";
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                groupBox3.Enabled = false;
                groupBox2.Enabled = true;
                groupBox4.Enabled = false;
                lightReturn = false;
                lightBreak = false;
                lightConcetrate = false;
                lightRecurse = true;
                lightSep = false;
                lightXcord = panel5.Width / 2;
                lightYcord = 0;
                panel5G = panel5.CreateGraphics();
                checkBox1.Enabled = false;
                textBox2.Text = "60";
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                groupBox3.Enabled = true;
                groupBox2.Enabled = true;
                groupBox4.Enabled = false;
                lightReturn = false;
                lightBreak = true;
                lightConcetrate = false;
                lightRecurse = false;
                lightSep = false;
                lightXcord = 5;
                lightYcord = 5;
                panel3G = panel3.CreateGraphics();
                panel4G = panel4.CreateGraphics();
                checkBox1.Enabled = true;
                textBox2.Text = "";
            }
            else if (tabControl1.SelectedIndex == 3)
            {
                groupBox2.Enabled = false;
                groupBox4.Enabled = true;
                groupBox3.Enabled = false;
                lightReturn = false;
                lightBreak = false;
                lightRecurse = false;
                lightConcetrate = true;
                lightSep = false;
                lightXcord = 5;
                lightYcord = 5;
                panel6G = panel6.CreateGraphics();
                DrawEnvironment(true);
                checkBox1.Enabled = true;
                textBox2.Text = "";
            }
            else if (tabControl1.SelectedIndex == 4)
            {
                lightSep = true;
                groupBox3.Enabled = false;
                groupBox4.Enabled = true;
                groupBox2.Enabled = false;
                lightReturn = false;
                lightBreak = false;
                lightRecurse = false;
                lightConcetrate = false;
                panel7G = panel7.CreateGraphics();
                DrawEnvironment(false);
                checkBox1.Enabled = true;
                textBox2.Text = "";
            }

        }
        //
        //change n.
        //
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            try
            {
                nToWhere = int.Parse(textBox4.Text);
            }
            catch
            {

            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            try
            {
                nFromWhere = int.Parse(textBox5.Text);
            }
            catch
            {

            }
        }
        //
        //Change n by clicking on a listbox
        //
        private void listBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            switch (listBox2.Items[listBox2.SelectedIndex].ToString())
            {
                case "1":
                    textBox5.Text = "1";
                    break;
                case "1.33":
                    textBox5.Text = "1.33";
                    break;
                case "1.5":
                    textBox5.Text = "1.5";
                    break;
                case "2.42":
                    textBox5.Text = "2.42";
                    break;
            }
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            switch (listBox1.Items[listBox1.SelectedIndex].ToString())
            {
                case "1":
                    textBox4.Text = "1";
                    break;
                case "1.33":
                    textBox4.Text = "1.33";
                    break;
                case "1.5":
                    textBox4.Text = "1.5";
                    break;
                case "2.42":
                    textBox4.Text = "2.42";
                    break;
            }
        }

        #endregion
        #region Options
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                panel1G.Clear(Color.Black);
                panel2G.Clear(Color.Black);
            }

        } 
        //
        //Help and credits.
        //
        private void button5_Click(object sender, EventArgs e)
        {

            MessageBox.Show("כדי להתחיל בחר באחת הלשונית והזן נתונים בתיבות הטקסט המתאימות \n" +
                "WASD ניתן להזיז את מקור האור באמצעות מקשי \n O ו L ניתן להגדיל ולהקטין את הזווית באמצעות המקשים"
                 ,"עזרה", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
        }


        //
        //nothing important.
        //
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (textBox7.Text == "0")
            {
                imgDistanceTxt.Enabled = true;
                objDistanceTxt.Enabled = true;
                objHeightTxt.Enabled = false;
                imgHeightTxt.Enabled = false;
            }
        }

        //
        //close the form
        //
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //Change light color :D
        private void button2_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            penColor = colorDialog1.Color;
            DrawLine();
            DrawReflectedLine();
        }
        //
        //Other options.
        //
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                DrawLine();
            }
            else
            {
                panel2G.Clear(Color.Black);
                panel1G.Clear(Color.Black);
                DrawLine();
                DrawReflectedLine();
            }
        }
        #endregion
        #region Mouse Click Events
        //
        //Many click on panel events.
        //
        //
        //Event for drawing text on the panels.
        //
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            Point mousePress = new Point(e.X, e.Y);
            Brush b = Brushes.Yellow;
            Pen p = new Pen(b);
            p.Color = penColor;
            panel1G = panel1.CreateGraphics();
            Font myFont = new Font(FontFamily.GenericSerif, 9);
            panel1G.DrawString(textBox3.Text, myFont, p.Brush, mousePress);
        }
        private void panel2_MouseClick(object sender, MouseEventArgs e)
        {
            Point mousePress = new Point(e.X, e.Y);
            Brush b = Brushes.Yellow;
            Pen p = new Pen(b);
            p.Color = penColor;
            panel2G = panel2.CreateGraphics();
            Font myFont = new Font(FontFamily.GenericSerif, 9);
            panel2G.DrawString(textBox3.Text, myFont, p.Brush, mousePress);
        }

        private void panel5_MouseClick(object sender, MouseEventArgs e)
        {
            Point mousePress = new Point(e.X, e.Y);
            Brush b = Brushes.Yellow;
            Pen p = new Pen(b);
            p.Color = penColor;
            panel5G = panel5.CreateGraphics();
            Font myFont = new Font(FontFamily.GenericSerif, 9);
            panel5G.DrawString(textBox3.Text, myFont, p.Brush, mousePress);
        }

        private void panel3_MouseClick(object sender, MouseEventArgs e)
        {
            Point mousePress = new Point(e.X, e.Y);
            Brush b = Brushes.Yellow;
            Pen p = new Pen(b);
            p.Color = penColor;
            panel3G = panel3.CreateGraphics();
            Font myFont = new Font(FontFamily.GenericSerif, 9);
            panel3G.DrawString(textBox3.Text, myFont, p.Brush, mousePress);
        }

        private void panel4_MouseClick(object sender, MouseEventArgs e)
        {
            Point mousePress = new Point(e.X, e.Y);
            Brush b = Brushes.Yellow;
            Pen p = new Pen(b);
            p.Color = penColor;
            panel4G = panel4.CreateGraphics();
            Font myFont = new Font(FontFamily.GenericSerif, 9);
            panel4G.DrawString(textBox3.Text, myFont, p.Brush, mousePress);
        }

        private void panel6_MouseClick(object sender, MouseEventArgs e)
        {
            Point mousePress = new Point(e.X, e.Y);
            Brush b = Brushes.Yellow;
            Pen p = new Pen(b);
            p.Color = penColor;
            panel6G = panel6.CreateGraphics();
            Font myFont = new Font(FontFamily.GenericSerif, 9);
            panel6G.DrawString(textBox3.Text, myFont, p.Brush, mousePress);
        }

        private void panel7_MouseClick(object sender, MouseEventArgs e)
        {
            Point mousePress = new Point(e.X, e.Y);
            Brush b = Brushes.Yellow;
            Pen p = new Pen(b);
            p.Color = penColor;
            panel7G = panel7.CreateGraphics();
            Font myFont = new Font(FontFamily.GenericSerif, 9);
            panel7G.DrawString(textBox3.Text, myFont, p.Brush, mousePress);
        }
        #endregion
    }
}
