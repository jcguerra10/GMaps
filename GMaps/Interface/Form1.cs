using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMaps.Model;

namespace GMaps
{
    public partial class Form1 : Form
    {
        private DataManager dm;


        private List<PointLatLng> points;
        private List<PointLatLng> polygons;
        private List<PointLatLng> routes;

        private GMapOverlay ovPoints;
        private GMapOverlay ovPolygons;
        private GMapOverlay ovRoutes;

        public Form1()
        {
            InitializeComponent();

            points = new List<PointLatLng>();
            polygons = new List<PointLatLng>();
            routes = new List<PointLatLng>();

            ovPoints = new GMapOverlay("points");
            ovPolygons = new GMapOverlay("polygons");
            ovRoutes = new GMapOverlay("routes");

            comboBox1.Items.Add("Point");
            comboBox1.Items.Add("Polygon");
            comboBox1.Items.Add("Route");

            comboBox2.Items.Add("Bars");
            comboBox2.Items.Add("Pie");
            comboBox2.Items.Add("Points");

            comboBox4.Items.Add("Region");
            comboBox4.Items.Add("Department Dane Code");
            comboBox4.Items.Add("Department");
        }

        // TAB1
        
        private void gMapControl1_Load(object sender, EventArgs e)
        {
            gMapControl1.MapProvider = GoogleMapProvider.Instance;

            gMapControl1.Overlays.Add(ovPoints);
            gMapControl1.Overlays.Add(ovPolygons);
            gMapControl1.Overlays.Add(ovRoutes);
        }


        private void SetPoint_Click(object sender, EventArgs e)
        {
            try
            {

                double la = double.Parse(textBox1.Text, CultureInfo.InvariantCulture);
                textBox1.Text = "";
                double lo = Double.Parse(textBox2.Text, CultureInfo.InvariantCulture);
                textBox2.Text = "";

                PointLatLng p = new PointLatLng(la, lo);

                if (comboBox1.Text == "Point")
                {
                    points.Add(p);
                }
                else if (comboBox1.Text == "Polygon")
                {
                    polygons.Add(p);
                }
                else
                {
                    routes.Add(p);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Empty lines");
            }
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            points.Clear();
            polygons.Clear();
            routes.Clear();
            ovPoints.Clear();
            ovPolygons.Clear();
            ovRoutes.Clear();
        }


        private void Show_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Point")
            {
                setPoint();
            }
            else if (comboBox1.Text == "Polygon")
            {
                SetPolygons();
            }
            else
            {
                SetRoutes();
            }
        }

        private void setPoint()
        {
            foreach (PointLatLng p in points)
            {
                GMapMarker gm = new GMarkerGoogle(p, GMarkerGoogleType.arrow);
                ovPoints.Markers.Add(gm);
            }
        }

        private void SetPolygons()
        {
            GMapPolygon gr = new GMapPolygon(polygons, "Polygon");
            gr.Fill = new SolidBrush(Color.FromArgb(50, Color.Coral));
            gr.Stroke = new Pen(Color.Coral, 2);
            
            ovPolygons .Polygons.Add(gr);
        }

        private void SetRoutes()
        {
            GMapRoute gr = new GMapRoute(routes, "Routes");
            gr.Stroke = new Pen(Color.Coral, 2);
            ovRoutes.Routes.Add(gr);
        }

        private void dataSetup_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                dm = new DataManager(path);
            }
        }
        
        //TAB2


        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string op = comboBox2.Text;
            
            comboBox3.Items.Clear();

            switch (op)
            {
                case "Bars":
                    comboBox3.Items.Add("Department");
                    break;
                case "Pie":
                    comboBox3.Items.Add("Department");
                    break;
                case "Points":
                    comboBox3.Items.Add("Department");
                    break;
            }
        }

        private void SetChart(object sender, MouseEventArgs e)
        {
            string op = comboBox2.Text;
            

            switch (op)
            {
                case "Bars":
                    string op1 = comboBox3.Text;
                    SetChartDepartment();
                    break;
                case "Pie":
                    SetChartPieDepartment();
                    break;
                case "Points":
                    SetChartPointsDepartment();
                    break;
            }
        }

        private void SetChartDepartment()
        {
            string[] lines = File.ReadAllLines(dm.Path);
            ArrayList dep = dm.getAllDeps();
            ArrayList count = new ArrayList();
            
            chart1.Series.Clear();
            
            for (int i = 0; i < dep.Count; i++)
            {
                count.Add(0);
            }
            for (int i = 0;i<dep.Count;i++)
            {
                foreach (var va in lines)
                {
                    var tva = va.Split(',');
                    if (tva[2].Equals(dep[i]))
                    {
                        count[i] = (int)count[i] + 1;
                    }
                }
            }
            
            for (int i = 0; i < dep.Count; i++)
            {

                Series serie = chart1.Series.Add((string)dep[i]);
                serie.Label = count[i].ToString();
                serie.Points.Add((int)count[i]);
            }
        }
        
        private void SetChartPieDepartment()
        {
            string[] lines = File.ReadAllLines(dm.Path);
            ArrayList dep = dm.getAllDeps();
            ArrayList count = new ArrayList();
            
            chart1.Series.Clear();
            
            for (int i = 0; i < dep.Count; i++)
            {
                count.Add(0);
            }
            for (int i = 0;i<dep.Count;i++)
            {
                foreach (var va in lines)
                {
                    var tva = va.Split(',');
                    if (tva[2].Equals(dep[i]))
                    {
                        count[i] = (int)count[i] + 1;
                    }
                }
            }
            
            chart1.Palette = ChartColorPalette.Bright;
            
            chart1.Series.Add("s");
            
            for (int i = 0; i < dep.Count; i++)
            {
                
                string s = (string) dep[i];
                chart1.Series["s"].ChartType = SeriesChartType.Pie;
                chart1.Series["s"].Points.AddXY(s,(int)count[i]);
            }
        }
        
        private void SetChartPointsDepartment()
        {
            string[] lines = File.ReadAllLines(dm.Path);
            ArrayList dep = dm.getAllDeps();
            ArrayList count = new ArrayList();
            
            chart1.Series.Clear();
            
            for (int i = 0; i < dep.Count; i++)
            {
                count.Add(0);
            }
            for (int i = 0;i<dep.Count;i++)
            {
                foreach (var va in lines)
                {
                    var tva = va.Split(',');
                    if (tva[2].Equals(dep[i]))
                    {
                        count[i] = (int)count[i] + 1;
                    }
                }
            }
            
            chart1.Series.Add("Departamentos");
            
            chart1.Palette = ChartColorPalette.Bright;

            for (int i = 0; i < dep.Count; i++)
            {
                chart1.Series["Departamentos"].ChartType = SeriesChartType.Point;
                chart1.Series["Departamentos"].Points.AddY((int)count[i]);
            }
        }
        
        // tab3

        // comboBox4.Items.Add("Department Dane Code");
        // comboBox4.Items.Add("Department");
        // comboBox4.Items.Add("Dane Code Of The Municipality");
        
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox5.Enabled = true;

            string s = comboBox4.Text;

            switch (s)
            {
                case "Region":
                    SetComboBox(0);
                    break;
                case "Department Dane Code":
                    SetComboBox(1);
                    break;
                case "Department":
                    SetComboBox(2);
                    break;
            }
        }

        private void SetComboBox(int j)
        {
            string[] lines = File.ReadAllLines(dm.Path);
            ArrayList ar = new ArrayList();
            comboBox5.Items.Clear();
            comboBox5.Text = "";
            foreach ( var i in lines)
            {
                var valores = i.Split(',');
                string s = valores[j];

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
            foreach(string f in ar) {
                comboBox5.Items.Add(f);
            }       
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = GetIndexCb4();
            string s = comboBox5.Text;

            dataGridView1.Rows.Clear();

            string[] lines = File.ReadAllLines(dm.Path);

            foreach (var i in lines)
            {
                var valores = i.Split(',');
                if (valores[index] == s)
                {
                    int n = dataGridView1.Rows.Add();
                    dataGridView1.Rows[n].Cells[0].Value = valores[0];
                    dataGridView1.Rows[n].Cells[1].Value = valores[1];
                    dataGridView1.Rows[n].Cells[2].Value = valores[2];
                    dataGridView1.Rows[n].Cells[3].Value = valores[3];
                    dataGridView1.Rows[n].Cells[4].Value = valores[4];
                }
            }
        }

        private int GetIndexCb4()
        {
            string s = comboBox4.Text;

            int i = -1;
            
            switch (s)
            {
                case "Region":
                    i = 0;
                    break;
                case "Department Dane Code":
                    i = 1;
                    break;
                case "Department":
                    i = 2;
                    break;
            }

            return i;
        }
    }
}