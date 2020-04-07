// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="Helix Toolkit">
//   Copyright (c) 2014 Helix Toolkit contributors
// </copyright>
// <summary>
//   Provides a ViewModel for the Main window.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ThreeViewTool
{
    using System.Collections.Generic;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;

    using HelixToolkit.Wpf;

    /// <summary>
    /// Provides a ViewModel for the Main window.
    /// </summary>
    public class MainViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            Init();
        }

        private Model3DGroup modelGroup;

        private Material greenMaterial;
        private Material redMaterial;
        private Material blueMaterial;
        private Material insideMaterial;

        private Material defaultMaterial;

        private Dictionary<Color, Material> materialBuffer = new Dictionary<Color, Material>();

        public void Reset()
        {
            Clear();
            Init();
        }

        public void Init()
        {
            // Create a model group
            modelGroup = new Model3DGroup();

            // Create a mesh builder and add a box to it
            MeshBuilder meshBuilder = new MeshBuilder(false, false);
            //meshBuilder.AddBox(new Point3D(0, 0, 1), 1, 2, 0.5);
            //meshBuilder.AddBox(new Rect3D(0, 0, 1.2, 0.5, 1, 0.4));
            meshBuilder.AddEllipsoid(new Point3D(1, 1, 1), 2, 2, 2);

            //meshBuilder.AddArrow(new Point3D(0,0,0),new Point3D(10,10,10),2  );

            // Create a mesh from the builder (and freeze it)
            var mesh = meshBuilder.ToMesh(true);

            // Create some materials
            greenMaterial = MaterialHelper.CreateMaterial(Colors.Green);
            redMaterial = MaterialHelper.CreateMaterial(Colors.Red);
            blueMaterial = MaterialHelper.CreateMaterial(Colors.Blue);
            insideMaterial = MaterialHelper.CreateMaterial(Colors.Yellow);

            materialBuffer.Add(Colors.Green, greenMaterial);
            materialBuffer.Add(Colors.Red, redMaterial);
            materialBuffer.Add(Colors.Blue, blueMaterial);
            materialBuffer.Add(Colors.Yellow, insideMaterial);

            // Add 3 models to the group (using the same mesh, that's why we had to freeze it)
            modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Material = greenMaterial, Transform = new TranslateTransform3D(0, 0, 0), BackMaterial = insideMaterial });
            //modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Transform = new TranslateTransform3D(-2, 0, 0), Material = redMaterial, BackMaterial = insideMaterial });
            //modelGroup.Children.Add(new GeometryModel3D { Geometry = mesh, Transform = new TranslateTransform3D(2, 0, 0), Material = blueMaterial, BackMaterial = insideMaterial });

            // Set the property, which will be bound to the Content property of the ModelVisual3D (see MainWindow.xaml)
            this.Model = modelGroup;

            defaultMaterial = blueMaterial;
        }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>The model.</value>
        public Model3D Model { get; set; }

        public void Clear()
        {
            modelGroup.Children.Clear();
        }

        public double PointSize = 1;

        public double PositionScale = 1;

        public void DrawPoint(double x, double y, double z)
        {
            GeometryModel3D geometry = GetGeometryPoint(x, y, z);
            modelGroup.Children.Add(geometry);
        }

        private GeometryModel3D GetGeometryPoint(double x, double y, double z)
        {
            MeshBuilder meshBuilder = new MeshBuilder(false, false);
            meshBuilder.AddEllipsoid(new Point3D(0, 0, 0), PointSize, PointSize, PointSize);
            // Create a mesh from the builder (and freeze it)
            var mesh = meshBuilder.ToMesh(true);
            return new GeometryModel3D
            {
                Geometry = mesh,
                Transform = new TranslateTransform3D(x, y, z),
                Material = defaultMaterial,
                BackMaterial = insideMaterial
            };
        }

        private GeometryModel3D GetGeometryArrow(Point3D start,Point3D end)
        {
            MeshBuilder meshBuilder = new MeshBuilder(false, false);
            meshBuilder.AddArrow(start, end, PointSize);
            // Create a mesh from the builder (and freeze it)
            var mesh = meshBuilder.ToMesh(true);
            return new GeometryModel3D
            {
                Geometry = mesh,
                Material = defaultMaterial,
                BackMaterial = insideMaterial
            };
        }

        public void SetMaterial(Color color)
        {
            if (materialBuffer.ContainsKey(color))
            {
                defaultMaterial = materialBuffer[color];
            }
            else
            {
                var material = MaterialHelper.CreateMaterial(color);
                materialBuffer.Add(color, material);
                defaultMaterial = material;
            }
            
        }

        public List<Point3D> points=new List<Point3D>();

        public List<GeometryModel3D> geometryBuffer=new List<GeometryModel3D>();

        public void AddPoint(double x, double y, double z,bool addArrow)
        {
            x *= PositionScale;
            y *= PositionScale;
            z *= PositionScale;
           
            GeometryModel3D geometry1 = GetGeometryPoint(x, y, z);
            geometryBuffer.Add(geometry1);

            if (addArrow)
            {
                points.Add(new Point3D(x, y, z));
                if (points.Count > 1)
                {
                    Point3D end = points[points.Count - 1];
                    Point3D start = points[points.Count - 2];
                    GeometryModel3D geometry2 = GetGeometryArrow(start, end);
                    geometryBuffer.Add(geometry2);
                }
            }
            
        }

        public void ShowPoints()
        {
            foreach (GeometryModel3D geometry in geometryBuffer)
            {
                modelGroup.Children.Add(geometry);
            }
        }
    }
}