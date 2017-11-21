using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace PDBUtility
{
    public static class TextReport
    {

        private static char[] _Ch ={ '/', '.', '-' };

        public static string PrintLines(char PChar, int NoOfCols, int NoOfRows)
        {
            string lStr = "";
            for (int i = 1; i <= NoOfRows; i++)
            {
                if (NoOfCols == 0)
                    lStr = lStr + Environment.NewLine;
                else
                {
                    for (int j = 1; j <= NoOfCols; j++)
                        lStr = lStr + PChar;
                    lStr += Environment.NewLine;
                }
            }
            return lStr;
        }

        public static string StrPadding(string Input, int NoOfCols, string LRM)
        {
            string lPad = "", rtnString = "";
            try
            {
                lPad = Input;
                if (lPad.Length >= NoOfCols)
                {
                    rtnString = lPad.Substring(0, NoOfCols);
                    return rtnString;
                }
                if (LRM == "L")
                    for (int i = 1; i <= NoOfCols - lPad.Length; i++)
                        lPad = lPad + " ";
                else if (LRM == "R")
                    for (int i = 1; i <= NoOfCols - lPad.Length; i++)
                        lPad = " " + lPad;
                else if (LRM == "M")
                {
                    NoOfCols = NoOfCols - lPad.Length;
                    int MidCol = NoOfCols / 2;

                    for (int i = 1; i <= NoOfCols - MidCol; i++)
                        lPad = " " + lPad;

                    for (int i = (NoOfCols - MidCol); i <= NoOfCols - 1; i++)
                        lPad = lPad + " ";
                }
            }
            finally
            {
                rtnString = lPad;
            }
            return rtnString;
        }

    }
}