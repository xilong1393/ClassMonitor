using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassMonitor3.Interfaces
{
    public partial class ListLocalDataForm : Form
    {
        public ListLocalDataForm(List<string> list)
        {
            InitializeComponent();
            dataGridView1.DataSource = list;
        }
    }
}
