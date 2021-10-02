namespace app.Controllers
{
    public class TableViewModel
    {
        public Table table;
        public string tableCode;

        public TableViewModel()
        {
        }

        public Table[] tables { get; internal set; }
    }
}