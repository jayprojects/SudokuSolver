using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace sudoku
{
    class Cell
    {
        public List<int> posibleNumbers;
        public int Number;
        private int row;
        private int col;
        private int group;
        private int id;

        public Cell(string innerText, int cellId)
        {
            id = cellId; //IDs are from 1 to 99
            Number = format(innerText);
            // set row and colum number for the cell
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

            // set group number for the cell
            if (col < 4 && row < 4) group = 1;
            else if ((col > 3 && col < 7) && (row < 4)) group = 2;
            else if ((col > 6) && (row < 4)) group = 3;

            else if (col < 4 && (row > 3 && row < 7)) group = 4;
            else if ((col > 3 && col < 7) && (row > 3 && row < 7)) group = 5;
            else if ((col > 6) && (row > 3 && row < 7)) group = 6;

            else if (col < 4 && (row > 6)) group = 7;
            else if ((col > 3 && col < 7) && (row > 6)) group = 8;
            else if ((col > 6) && (row > 6)) group = 9;
            
            //initially if the cell is not already haven a value
            //it is posible any number 1 to 9 can be the value
            posibleNumbers = new List<int>();
            int n = format(innerText);
            if (n > 0)
            {
                posibleNumbers.Add(n);
            }
            else
            {
                posibleNumbers.Add(1);
                posibleNumbers.Add(2);
                posibleNumbers.Add(3);
                posibleNumbers.Add(4);
                posibleNumbers.Add(5);
                posibleNumbers.Add(6);
                posibleNumbers.Add(7);
                posibleNumbers.Add(8);
                posibleNumbers.Add(9);
            }
        }


        public int format(string str)
        {
            if (str == null || str.Trim().Equals(""))
                return 0;
            else
            {
                str = Regex.Replace(str, "[^1-9]", "").Trim();
                if (str.Equals("")) return 0;
                else return Int16.Parse(str);
            }
        }

        //Remove posible number from a cell that number already 
        //exist in the row, column or group
        public bool removeExisting(List<Cell> cells)
        {
            foreach (Cell c in cells)
            {
                if ((c.col == this.col || c.row == this.row || c.group == this.group) && (c.id != this.id))
                {
                    if (c.posibleNumbers.Count == 1)
                    {
                        //remove the element that already exist in the same row, column or group
                        this.posibleNumbers.Remove(c.posibleNumbers.ElementAt(0));
                    }
                }
            }
            return false;
        }


        //Every Row, Colmun, and group must have all 9 numbers
        //So, for example, if number "6" is not an elligibel number of
        //any other cell on that column then "6" must be the number for that cell
        public int mustBe(List<Cell> cells)
        {
            foreach (int n in this.posibleNumbers)
            {
                //check if this number is also candidate for any other group
                bool veto = false;
                //check for group
                foreach (Cell c in cells)
                {
                    if ((c.group == this.group) && (c.posibleNumbers.Count > 1) && (c.id != this.id))
                    {
                        if (c.posibleNumbers.Contains(n)) veto = true;
                    }
                }
                if (!veto)
                {
                    //if other cell in the group has any posiblility forthe number then this must be the number for this cell
                    return n;
                }
                else //if someone in the group veto then move on, and check for the row
                {
                    veto = false;
                    foreach (Cell c in cells)
                    {
                        if ((c.row == this.row) && (c.posibleNumbers.Count > 1) && (c.id != this.id))
                        {
                            if (c.posibleNumbers.Contains(n)) veto = true;
                        }
                    }
                    
                }

                if (!veto)
                {
                    //if other cell in the row has any posiblility forthe number then this must be the number for this cell
                    return n;
                }
                else //if someone in the row also veto then move on, and check for the column
                {
                    veto = false;
                    foreach (Cell c in cells)
                    {
                        if ((c.col == this.col) && (c.posibleNumbers.Count > 1) && (c.id != this.id))
                        {
                            if (c.posibleNumbers.Contains(n)) veto = true;
                        }
                    }

                }
                if (!veto)
                {
                    //if other cell in the columne has any posiblility forthe number then this must be the number for this cell
                    return n;
                }

            }
            //if somen one in the group, row, col veto then were not sure which number it would be
            //because there are other candidate for this number
            return 0;
        }


        //Remove posible number from a cell that number already 
        //exist in the row, column or group
        public int isGood(List<Cell> cells, int n)
        {
            
            foreach (Cell c in cells)
            {
                if ((c.col == this.col || c.row == this.row || c.group == this.group) && (c.id != this.id))
                {
                    if (c.Number == n)
                        return c.id;
                        //returnValue = false;
                }
            }
            return 0;
        }
    }
}

