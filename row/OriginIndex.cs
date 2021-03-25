using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKinEditer.row
{
   public class OriginIndex
    {
        public OriginIndex(int columnIndex, int rowIndex)
        {
            ColumnIndex = columnIndex;
            RowIndex = rowIndex;
        }

        public int ColumnIndex { get; set; }
        public int RowIndex { get; set; }
    }
}
