using ClassMonitor3.Model;
using System.Windows.Forms;

namespace ClassMonitor3.Interfaces
{
    public partial class ClassroomInfoForm : Form
    {
        public ClassroomInfoForm(ClassroomView classroomView)
        {
            InitializeComponent();
            propertyGrid1.SelectedObject = classroomView;
        }
    }
}
