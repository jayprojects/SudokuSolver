using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
namespace sudoku
{
    public partial class SudokuSolver : Form
    {
        public SudokuSolver()
        {
            InitializeComponent();
        }

        bool slectGame;
        List<Cell> cells;
        void populate()
        {
            cells = new List<Cell>();
            for (int i = 0; i < 81; i++)
            {
                Cell c = new Cell(tb(i).Text, i + 1);
                cells.Add(c);
            }
        }
        private void buttonSolve_Click(object sender, EventArgs e)
        {
            if (cells == null || cells.Count != 81)
                populate();
            solveSudoku();
            richTextBox1.Text = "All done!";
        }


        void solveSudoku()
        {
            bool allDone = false;
            int maxIter = 1000;
            
            string strText;
            while (!allDone && maxIter>0)
            {

                maxIter--;
                allDone = true;
                for (int i = 0; i < 81; i++)
                {
                    Cell c = cells.ElementAt(i);
                    if (c.posibleNumbers.Count == 1)
                    {
                        strText = c.posibleNumbers.ElementAt(0).ToString();
                        if (!strText.Equals(tb(i).Text))
                        {
                            tb(i).Text = "  "+strText;
                        }
                    }
                    else
                    {
                        allDone = false;
                        if (!(c.removeExisting(cells))) //remove that already exist
                        {
                            //no number were removed, hense no progress were made
                            int mb = c.mustBe(cells); //put in that no one else claim
                            if (mb > 0)
                            {
                                c.posibleNumbers.Clear();
                                c.posibleNumbers.Add(mb);
                            }
                        }
                    }

                }
            }
        }

        TextBox tb(int i)
        {
            i = i + 1;
            String text = "textBox" + i;
            return (TextBox)Controls[text];
        }
       
        private void Form1_Load(object sender, EventArgs e)
        {

            slectGame = false;
            int id=0;
            string filePath = @"C:\Users\Jayati\Documents\Visual Studio 2010\Projects\vs2010\sudoku\sudoku\sample.txt";
            List<string> lines = readFileLineByLine(filePath);
            foreach (string line in lines)
            {
                id++;
                //comboBox1.Items.Add(line);
                comboBox1.Items.Add(new ComboboxItem(id.ToString(), line));
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            slectGame = true;
            ComboboxItem selectedItem = (ComboboxItem)comboBox1.SelectedItem;
            string selectedSudoku = selectedItem.Value.ToString();
            string[] sArray = selectedSudoku.Split(',');
            for (int i = 0; i < 81; i++)
            {
                tb(i).BackColor = Color.Wheat;
                if (sArray[i].Trim() != "")
                {
                    tb(i).BackColor = Color.Gray;
                    tb(i).Enabled = false;
                }
                else
                {
                    tb(i).BackColor = Color.White;
                    tb(i).Enabled = true;
                }
                tb(i).Text = " "+sArray[i];
                
            }

          
                populate();
                slectGame = false;

        }

        List<string> readFileLineByLine(string filePath)
        {
            
            string line;
            List<string> lines = new List<string>();

            // Read the file and display it line by line.
            StreamReader file = new StreamReader(filePath);
            while ((line = file.ReadLine()) != null)
            {
                lines.Add(line);
            }

            file.Close();
            return lines;
        }
        int lastCangedId = 0;
        Color lastChangedColor = Color.White;

        private void cell_TextChanged(object sender, EventArgs e)
        {
            if (cells!=null && cells.Count== 81)
            {
                TextBox ctb = (TextBox)sender;
                ctb.BackColor = Color.White;
                string strN = ctb.Text.Trim();
                if (strN.Length > 0)
                {
                    strN = Regex.Replace(strN, "[^1-9]", "").Trim();
                    if (strN.Length == 1)
                    {
                        //if All is well!!
                        if (lastCangedId > 0) tb(lastCangedId).BackColor = lastChangedColor;

                        int n = int.Parse(strN);
                        int id = int.Parse(ctb.Name.Replace("textBox", "").Trim());
                        Cell c = cells.ElementAt(id - 1);
                        int tempN=c.isGood(cells, n);
                        if (tempN == 0)
                        {
                            cells[id - 1] = new Cell(strN, id);
                            //cells.ElementAt(id - 1) = new Cell(strN, id);
                            richTextBox1.AppendText("Looks good!" + Environment.NewLine);
                        }
                        else
                        {
                            lastCangedId = tempN - 1;
                            lastChangedColor = tb(lastCangedId).BackColor;
                            tb(lastCangedId).BackColor = Color.Red;
                            ctb.BackColor = Color.Red;

                            richTextBox1.AppendText("the number "+n.ToString()+" already exist!" + Environment.NewLine);
                        }
                        
                    }
                    else
                    {
                        ctb.Text = "";
                        richTextBox1.AppendText("Please enter number only" + Environment.NewLine);
                    }

                }
            }
            else if (!slectGame)
            {
                richTextBox1.AppendText("Please select a game first!" + Environment.NewLine);
            }
        }
    }
}


/*
 
 ------------------
  
  
            TextBox tb = (TextBox)sender;
            string strN = tb.Text.Trim();
            if (strN.Length > 0)
            {
                strN = Regex.Replace(strN, "[^0-9]", "").Trim();
                if (strN.Length == 1)
                {
                    int id = int.Parse(tb.Name.Replace("textBox", "").Trim());
                    Cell c = new Cell(tb.Text, id);
                    List<int> pN = c.posibleNumbers;
                    
                    //if (pN.Exists(element => element == c.Number))
                    foreach(int n in pN)
                    {
                        richTextBox1.AppendText(n.ToString());
                    }
                    richTextBox1.AppendText(Environment.NewLine);
                }
                else
                {
                    tb.Text = "";
                    richTextBox1.AppendText("Please enter number only" + Environment.NewLine);
                }

            }
  
  ---------
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
TextBox tb = (TextBox)sender;
            
            int id = int.Parse(tb.Name.Replace("textBox","").Trim());
            int row = 0, col = 0;
            if ((id % 9) == 0)
            {
                row = id / 9;
                col = 9;
            }
            else
            {
                row = id / 9 + 1;
                col = id % 9;
            }

            MessageBox.Show("Row: " +row.ToString() + " Col: " + col.ToString());
             
            //int id = int.Parse(tb.Name.Replace("textBox","").Trim());
           // Cell c = new Cell(tb.Text, id);
            string strN = tb.Text;
            
            strN = Regex.Replace(strN, "[^0-9]", "").Trim();
            if (strN.Length == 1)
            {
                int id = int.Parse(tb.Name.Replace("textBox", "").Trim());
                Cell c = new Cell(tb.Text, id);
                //MessageBox.Show("Row: " + c.posibleNumbers + " Col: " + col.ToString());
            }
            else
            {
                tb.Text = "";
                richTextBox1.AppendText("Please enter number only" + Environment.NewLine);
            }


            MessageBox.Show("N:" + strN);
            //MessageBox.Show("Row: " + c.posibleNumbers + " Col: " + col.ToString());
*/