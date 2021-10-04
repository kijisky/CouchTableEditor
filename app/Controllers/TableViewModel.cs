using System.Collections.Generic;

namespace app.Controllers
{
    public class TableViewModel
    {
        public Table table;
        public string tableCode;
        public IEnumerable<IEnumerable<mdField>> columns;

        public TableViewModel()
        {
        }

        public Table[] tables { get; internal set; }
    }
}