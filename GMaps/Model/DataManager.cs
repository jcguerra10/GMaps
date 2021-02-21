using System.Collections;
using System.IO;

namespace GMaps.Model
{
    public class DataManager
    {
        private string path;
        private ArrayList l;

        public DataManager(string path)
        {
            this.path = path;
            regionConfig();
        }

        private void regionConfig()
        {
            string[] lines = File.ReadAllLines(path);
            l = new ArrayList();
            foreach ( var i in lines)
            {
                var valor = i.Split(',');
                l.Add(valor);
            }
        }

        public ArrayList getL()
        {
            return l;
        }
    }
}