/**
 * @author Jay Das <jay11421@gmail.com>
 * @copyright 2012 Jay Das
 * @namespace sudoku
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web;

namespace sudoku
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<Cell> cells;
        private void buttonSolve_Click(object sender, EventArgs e)
        {
            cells = new List<Cell>();
            for (int i = 0; i < 81; i++)
            {
                Cell c = new Cell(tb(i).Text,i+1);
                cells.Add(c);
            }
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
                            tb(i).ForeColor = Color.Red;
                            tb(i).Text = strText;
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
       
        private void buttonErase_Click(object sender, EventArgs e)
        {
            tb(0).Text = "";
            tb(2).Text = "";
            tb(3).Text = "";
            tb(4).Text = "";
            tb(5).Text = "";
            tb(8).Text = "";
            
            tb(10).Text = "";
            tb(11).Text = "";
            tb(14).Text = "";
            tb(15).Text = "";
            tb(16).Text = "";
            
            tb(20).Text = "";
            tb(21).Text = "";
            tb(23).Text = "";
            tb(25).Text = "";
            tb(26).Text = "";
            
            tb(30).Text = "";//2
            tb(32).Text = "";
            tb(33).Text = "";
            tb(35).Text = "";

            tb(36).Text = "";//3
            tb(37).Text = "";//8
            tb(39).Text = "";
            tb(41).Text = "";
            tb(43).Text = "";
            tb(44).Text = "";
            
            tb(45).Text = ""; //4
            tb(47).Text = "";
            tb(48).Text = "";
            tb(50).Text = "";
            

            tb(54).Text = "";//8
            tb(55).Text = "";
            tb(57).Text = "";
            tb(59).Text = "";
            tb(60).Text = "";

            tb(64).Text = "";//3
            tb(65).Text = "";//5
            tb(66).Text = "";//7
            tb(69).Text = "";
            tb(70).Text = "";
            
            tb(72).Text = "";//7
            tb(75).Text = "";
            tb(76).Text = "";
            tb(77).Text = "";
            tb(78).Text = "";
            tb(80).Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

       
    }
}
