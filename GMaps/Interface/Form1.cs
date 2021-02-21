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
                setPolygons();
            }
            else
            {
                setRoutes();
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

        private void setPolygons()
        {
            GMapPolygon gr = new GMapPolygon(polygons, "Polygon");
            gr.Fill = new SolidBrush(Color.FromArgb(50, Color.Coral));
            gr.Stroke = new Pen(Color.Coral, 2);
            
            ovPolygons .Polygons.Add(gr);
        }

        private void setRoutes()
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
                    comboBox3.Items.Add("Municipality");
                    break;
                case "Pie":
                    comboBox3.Items.Add("Department");
                    break;
                case "Points":
                    
                    break;
            }
        }

        private void setChart(object sender, MouseEventArgs e)
        {
            string op = comboBox2.Text;
            

            switch (op)
            {
                case "Bars":
                    string op1 = comboBox3.Text;
                    if (op1 == "Department")
                    {
                        setChartDepartment();
                    }
                    break;
                case "Pie":
                    setChartPieDepartment();
                    break;
                case "Points":
                    setChartPointsDepartment();
                    break;
            }
        }

        private void setChartDepartment()
        {
            string[] lines = File.ReadAllLines(dm.getPath());
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
        
        private void setChartPieDepartment()
        {
            string[] lines = File.ReadAllLines(dm.getPath());
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
        
        private void setChartPointsDepartment()
        {
            string[] lines = File.ReadAllLines(dm.getPath());
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
    }
}