using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMaps.Model;

namespace GMaps
{
    public partial class Form1 : Form
    {
        private DataManager dm = new DataManager();


        private List<PointLatLng> points;
        private List<PointLatLng> polygons;
        private List<PointLatLng> routes;

        private GMapOverlay ovPoints;
        private GMapOverlay ovPolygons;
        private GMapOverlay ovRoutes;

        public Form1()
        {
            InitializeComponent();
            dm = new DataManager();

            points = new List<PointLatLng>();
            polygons = new List<PointLatLng>();
            routes = new List<PointLatLng>();

            ovPoints = new GMapOverlay("points");
            ovPolygons = new GMapOverlay("polygons");
            ovRoutes = new GMapOverlay("routes");
        }

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
            
        }
    }
}