using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PDBUtility
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Connection Con = new Connection();
            //bool Status = Con.CheckOracleConnection("Data Source=IFSKPCL;User Id=IFSAPP;Password=ifs11gr2");
            
            //bool Status = OraUtility.CheckOracleConnection("Data Source=IFSKPCL;User Id=IFSAPP;Password=ifs11gr2");
            //label1.Text = Status.ToString() + Environment.NewLine + Ds.Tables[0].Rows[0][0].ToString();

            DataSet Ds = new DataSet();
            string SqlQuery = "SELECT USRID,LGNID,PWD,A.EMPID,B.USRGRPNAME,HRMS.HF_EMPNAME(A.EMPID) EMPNAME,HRMS.HF_EMP_DEPTNAME(C.EMPID) DEPTNAME "
                + "FROM SETP.SM_USRMST A,SETP.SM_USRGRPHDR B,HRMS.HM_EMPMST C WHERE A.USRGRPID = B.USRGRPID AND A.STATUS = 'A' AND B.STATUS = 'A' AND A.EMPID = C.EMPID(+) "
                + "AND UPPER(LGNID) = 'RAGHAVENDRAU' AND PWD = '83B9BD80A5C47E36CC8611ACDBD21D2FDD29D92F' ";
            Ds = OraUtility.GetData(SqlQuery, "Data Source=KPCLEPMS;User Id=ADMN;Password=epmskpcl", false);            
        }
    }
}
