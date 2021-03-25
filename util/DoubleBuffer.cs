using System;
using System.Reflection;
using System.Windows.Forms;

namespace SKinEditer.util
{
    static class  DoubleBuffer
    {
    
            public static void DoubleBufferedDataGirdView(this DataGridView dgv, bool flag=true)
            {
                Type dgvType = dgv.GetType();
                PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                pi.SetValue(dgv, flag, null);
            }

 
             public static void DoubleBufferedListView(this ListView lv, bool flag=true)
            {
                    Type lvType = lv.GetType();
                    PropertyInfo pi = lvType.GetProperty("DoubleBuffered",
                        BindingFlags.Instance | BindingFlags.NonPublic);
                    pi.SetValue(lv, flag, null);
            }

    }
}
