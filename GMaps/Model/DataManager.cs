﻿using System.Collections;
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

        public string getPath()
        {
            return path;
        }

        public ArrayList getAllDeps()
        {
            string[] lines = File.ReadAllLines(path);
            ArrayList ar = new ArrayList();
            foreach ( var i in lines)
            {
                var valores = i.Split(',');
                string s = valores[2];

                if (ar.Count == 0)
                {
                    ar.Add(s);
                }
                else
                {
                    if (!ar.Contains(s))
                    {
                        ar.Add(s);
                    }
                }
            }

            return ar;
        }
    }
}